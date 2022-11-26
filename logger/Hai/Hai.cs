using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Hai
{
	/// <summary>
	/// This class will hold up info about the hardware.
	/// </summary>
	public class Hai
	{
		[DllImport("hai.dll")]
		static extern int start_winsock();
		[DllImport("hai.dll")]
		static extern int hai_net_open(out int id, [In, MarshalAs(UnmanagedType.LPStr)] string ip_address, int port, 
			[In, MarshalAs(UnmanagedType.LPArray)] byte[] private_key);
		[DllImport("hai.dll")]
		static extern int hai_net_close(int id);
		[DllImport("hai.dll")]
		static unsafe extern int omni_get_first_name(int id, byte* data, out int bLongIndex);
		[DllImport("hai.dll")]
		static unsafe extern int omni_get_next_name(int id, byte* data, out int bLongIndex);
		[DllImport("hai.dll")]
		static extern int omni_login(int id, [In, MarshalAs(UnmanagedType.LPArray)] byte[] code);
		[DllImport("hai.dll")]
		static unsafe extern int omni_zone_stat(int id, int start, int end, byte *data);
		[DllImport("hai.dll")]
		static unsafe extern int omni_unit_stat(int id, int start, int end, byte *data);
		[DllImport("hai.dll")]
		static unsafe extern int omni_sys_events(int id, byte *data, out int num);

		static bool bWinSockCalled = false;
		int netHandle;
		ASCIIEncoding ascii = new ASCIIEncoding();	// Needed to convert byte stream to strings.
		
		public const int MAX_ZONES      = 215;
		public const int MAX_UNITS      = 511;
		public const int MAX_BUTTONS    = 128;
		public const int MAX_AUXS       = 176;
		public const int MAX_CODES      = 99;
		public const int MAX_AREAS      = 8;
		public const int MAX_THERMOS    = 64;
		public const int MAX_MESGS      = 128;
		public const int MAX_NAME_CHARS = 16;
		public const int MAX_EVENTS     = 64;
		public const int MAX_STAT_REQUEST = 51;		// Can request only this many stats at a time.
		const int EOMNIEOD = 2014;
		public const int EHAITIMEOUT = 2004;

		public enum NameType { Zones, Units, Buttons, Codes, Areas, Thermos, Messages }

		public class OmniException : Exception
		{
			int err;

			public OmniException(string str, int err) : base(str)
			{
				this.err = err;
			}
			public int Error
			{
				get 
				{
					return err;
				}
			}
		}

		public HaiNames haiNames;
		
		public Hai(string ipAddress, int port, byte[] privateKey, byte[] code)
		{
			int err;
			// Startup WinSock only on the first call.
			if (!bWinSockCalled)
			{
				if ((err = start_winsock()) != 0)
					throw new OmniException("start_winsock() error code = " + err.ToString(),err);
				bWinSockCalled = true;
			}
			// Open the net connection and get a handle.
			if ((err = hai_net_open(out netHandle,ipAddress,port,privateKey)) != 0)
				throw new OmniException("hai_net_open() error code = " + err.ToString(),err);
			if ((err = omni_login(netHandle,code)) != 0)
			{
				Close();
				throw new OmniException("omni_login() error code = " + err.ToString(),err);
			}

			haiNames = new HaiNames(this);
			haiNames.Show();
			haiNames.UploadNames();
			haiNames.Hide();
		}

		~Hai()
		{
			Close();
		}

		/// <summary>
		/// Ought to be called by a user when finished.
		/// </summary>
		public void Close()
		{
			if (netHandle != 0)
			{
				int err;
				err = hai_net_close(netHandle);
				netHandle = 0;
				if (err != 0)
					throw new OmniException("hai_net_close() error = " + err.ToString(),err);
			}
		}

		/// <summary>
		/// Starts retrieving defined text strings.  Returns true if out variables filled.  Continue with GetNextName.
		/// </summary>
		public bool GetFirstName(out int type, out int index, out string name)
		{
			return GetName(true,out type,out index,out name);
		}
		/// <summary>
		/// Continues retrieving defined text strings.  Returns true if out variables filled.
		/// </summary>
		public bool GetNextName(out int type, out int index, out string name)
		{
			return GetName(false,out type,out index,out name);
		}
		/// <summary>
		/// Common conversions for both public routines.
		/// </summary>
		unsafe bool GetName(bool bFirst, out int type, out int index, out string name)
		{
			const int BUFLEN = 25;
			byte[] data = new byte[BUFLEN];
			type = 0;
			index = 0;
			name = null;
			int bLongIndex;
			int err;
			
			fixed (byte* pdata = data)
			{
				err = bFirst ? omni_get_first_name(netHandle, pdata, out bLongIndex) : 
					omni_get_next_name(netHandle, pdata, out bLongIndex);
			}
			if (err == EOMNIEOD) return false;
			if (err != 0)
				throw new OmniException("omni_get_first_name() error = " + err.ToString(),err);
			type = data[0];
			int iStart = 2;
			index = data[1];
			if (bLongIndex != 0)
			{
				index = index *256 + data[2];
				iStart = 3;
			}
			int ii = iStart;
			while (ii<BUFLEN && data[ii] != 0) ii++;
			int cLen = ii - iStart;
			name = ascii.GetString(data,iStart,cLen);
			return true;
		}

		public struct Zone
		{
			public byte status;
			public byte loop;
		}

		/*
		// The actual structure filled by omni_zone_stat() is zone_stat.
		typedef struct
		{
			val8 status;
			val8 loop;
		} zone;

		typedef struct
		{
			zone zones[MAX_ZONES];
		} zone_stat;
*/
		unsafe public Zone[] ZoneStatus(int start, int end)
		{
			int err;
			byte *pj = stackalloc byte[2*MAX_ZONES];
			err = omni_zone_stat(netHandle,start,end,pj);
			if (err != 0)
				throw new OmniException("omni_zone_stat() error = " + err.ToString(),err);
			int cc = end - start + 1;
			Zone[] zoneStat = new Zone[cc];
			for (int ii=0; ii<cc; ii++)
			{
				zoneStat[ii].status = pj[2*ii];
				zoneStat[ii].loop = pj[2*ii+1];
			}
			return zoneStat;
		}

		
		public struct Unit
		{
			public byte status;
			public int time;
		}
		
/*
		// The actual structure filled by omni_unit_stat() is unit_stat.
		typedef struct
		{
			val8 status;
			val16 time;
		} unit;

		typedef struct
		{
			unit units[MAX_UNITS];
		} unit_stat;
*/
		unsafe public Unit[] UnitStatus(int start, int end)
		{
			int err;
			byte *pj = stackalloc byte[3*MAX_UNITS];
			err = omni_unit_stat(netHandle,start,end,pj);
			if (err != 0)
				throw new OmniException("omni_unit_stat() error = " + err.ToString(),err);
			int cc = end - start + 1;
			Unit[] unitStat = new Unit[cc];
			for (int ii=0; ii<cc; ii++)
			{
				unitStat[ii].status = pj[3*ii];
				unitStat[ii].time = pj[3*ii+1]*256 + pj[3*ii+2];
			}
			return unitStat;
		}

		// static unsafe extern int omni_sys_events(int id, byte *data, out int num);

		unsafe public Int16[] GetEvents()
		{
			int err;
			int cc;
			byte *pj = stackalloc byte[2*MAX_EVENTS];
			err = omni_sys_events(netHandle, pj, out cc);
			if (err != 0)
				throw new OmniException("omni_sys_events() error = " + err.ToString(),err);
			if (cc != 0)
			{
				Int16[] events = new Int16[cc];
				for (int ii=0; ii<cc; ii++)
					events[ii] = (short) ((pj[2*ii] << 8) + pj[2*ii+1]);
				return events;
			}
			return null;
		}

	}
}
