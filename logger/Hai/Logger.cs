using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;				// File writing.

namespace Hai
{
	/// <summary>
	/// Summary description for Logger.
	/// </summary>
	public class Logger : System.Windows.Forms.Form
	{
		StreamWriter file;
		Hai myHai;
		Select mySelect;
		int interval;

		const int MAX_STRINGS = 100;
		string[] strBuffer = new string[MAX_STRINGS];
		int cLines = 0;
		struct State
		{
			public string name;
			public byte status;
			public int time;
			public bool bChanged;	// Changed since last printing.
		}
		int cZoneBlocks = 0;
		int[] iZoneStart = new int[(Hai.MAX_ZONES-1) / Hai.MAX_STAT_REQUEST + 1];
		int[] cZoneLen = new int[(Hai.MAX_ZONES-1) / Hai.MAX_STAT_REQUEST + 1];
		int cControlBlocks = 0;
		int[] iControlStart = new int[(Hai.MAX_UNITS-1) / Hai.MAX_STAT_REQUEST + 1];
		int[] cControlLen = new int[(Hai.MAX_UNITS-1) / Hai.MAX_STAT_REQUEST + 1];
		State[][] zone;
		State[][] control;
		int day = 0;

		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Timer timer1;
		private System.ComponentModel.IContainer components;

		public Logger(Hai hai,Select sel,StreamWriter sw,int interval,
			bool[] bZones,bool[] bControls)
		{
			InitializeComponent();	// Required for Windows Form Designer support
			this.myHai = hai;
			this.mySelect = sel;
			this.file = sw;
			this.interval = interval;
			// Count the zones to track.
			int iLast = Hai.MAX_ZONES-1;
			while (iLast >= 0 && bZones[iLast] == false) iLast--;
			if (iLast >= 0)
			{
				// Figure out how many request blocks we need.
				//  - There is apparently a 51 zone limit on stat requests.
				int iBlockFirst = 0;
				while (iBlockFirst <= iLast)
				{
					while (bZones[iBlockFirst]==false) iBlockFirst++;
					int iBlockLast = iBlockFirst + Hai.MAX_STAT_REQUEST - 1;
					if (iBlockLast > iLast) iBlockLast = iLast;
					while (bZones[iBlockLast]==false) iBlockLast--;
					this.iZoneStart[this.cZoneBlocks] = iBlockFirst;
					this.cZoneLen[this.cZoneBlocks] = iBlockLast - iBlockFirst + 1;
					this.cZoneBlocks++;
					iBlockFirst = iBlockLast + 1;
				}
				// Allocate the State arrays.  Mark the active zones with a non-null name.
				this.zone = new State[this.cZoneBlocks][];
				for (int ii=0; ii<this.cZoneBlocks; ii++)
				{
					this.zone[ii] = new State[this.cZoneLen[ii]];
					for (int jj=0; jj<this.cZoneLen[ii]; jj++)
					{
						int iIndex = this.iZoneStart[ii] + jj;
						if (bZones[iIndex])
						{
							this.zone[ii][jj].name = this.myHai.haiNames.zones[iIndex];
						}
					}
				}
			}

			// Count the controls to track.
			iLast = Hai.MAX_UNITS-1;
			while (iLast >= 0 && bControls[iLast] == false) iLast--;
			if (iLast >= 0)
			{
				// Figure out how many request blocks we need.
				//  - There is apparently a 51 control limit on stat requests.
				int iBlockFirst = 0;
				while (iBlockFirst <= iLast)
				{
					while (bControls[iBlockFirst]==false) iBlockFirst++;
					int iBlockLast = iBlockFirst + Hai.MAX_STAT_REQUEST - 1;
					if (iBlockLast > iLast) iBlockLast = iLast;
					while (bControls[iBlockLast]==false) iBlockLast--;
					this.iControlStart[this.cControlBlocks] = iBlockFirst;
					this.cControlLen[this.cControlBlocks] = iBlockLast - iBlockFirst + 1;
					this.cControlBlocks++;
					iBlockFirst = iBlockLast + 1;
				}
				// Allocate the State arrays.  Mark the active controls with a non-null name.
				this.control = new State[this.cControlBlocks][];
				for (int ii=0; ii<this.cControlBlocks; ii++)
				{
					this.control[ii] = new State[this.cControlLen[ii]];
					for (int jj=0; jj<this.cControlLen[ii]; jj++)
					{
						int iIndex = this.iControlStart[ii] + jj;
						if (bControls[iIndex])
						{
							this.control[ii][jj].name = this.myHai.haiNames.units[iIndex];
						}
					}
				}
			}
			// Get the initial states.
			GetStatus();
			// Make sure all get printed on startup.
			if (this.cZoneBlocks != 0)
			{
				for (int ii=0; ii<this.cZoneBlocks; ii++)
				{
					for (int jj=0; jj<this.cZoneLen[ii]; jj++)
					{
						zone[ii][jj].bChanged = true;
					}
				}
			}
			if (this.cControlBlocks != 0)
			{
				for (int ii=0; ii<this.cControlBlocks; ii++)
				{
					for (int jj=0; jj<this.cControlLen[ii]; jj++)
					{
						control[ii][jj].bChanged = true;
					}
				}
			}
			PrintNewStates();
			timer1.Interval = this.interval * 1000;
			timer1.Start();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textBox1.Location = new System.Drawing.Point(8, 15);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new System.Drawing.Size(432, 320);
			this.textBox1.TabIndex = 0;
			this.textBox1.TabStop = false;
			this.textBox1.Text = "";
			this.textBox1.WordWrap = false;
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// Logger
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(448, 350);
			this.Controls.Add(this.textBox1);
			this.Name = "Logger";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Logger";
			this.ResumeLayout(false);

		}
		#endregion

		private void GetStatus()
		{
			if (this.cZoneBlocks != 0)
			{
				for (int ii=0; ii<this.cZoneBlocks; ii++)
				{
					Hai.Zone[] zoneStat = myHai.ZoneStatus(this.iZoneStart[ii]+1,this.iZoneStart[ii]+this.cZoneLen[ii]);
					for (int jj=0; jj<this.cZoneLen[ii]; jj++)
					{
						zone[ii][jj].bChanged = ((zone[ii][jj].status&3) != (zoneStat[jj].status&3));
						zone[ii][jj].status = zoneStat[jj].status;
					}
				}
			}
			if (this.cControlBlocks != 0)
			{
				for (int ii=0; ii<this.cControlBlocks; ii++)
				{
					Hai.Unit[] controlStat = myHai.UnitStatus(this.iControlStart[ii]+1,this.iControlStart[ii]+this.cControlLen[ii]);
					for (int jj=0; jj<this.cControlLen[ii]; jj++)
					{
						control[ii][jj].bChanged = ((control[ii][jj].status) != (controlStat[jj].status));
						control[ii][jj].status = controlStat[jj].status;
						control[ii][jj].time = controlStat[jj].time;
					}
				}
			}
		}

		static string[] strZoneState = {"Secure", "Not ready", "Trouble"};  // (status & 0x03)

		private void PrintNewStates()
		{
			DateTime dt = DateTime.Now;
			int cOldLines = this.cLines;
			if (dt.Day != this.day)
			{
				string str = String.Format("{0:HH:mm:ss}  {1,-16}  {2:yyyy.MM.dd}",
					dt, "[New Date]", dt);
				WriteLine(str);
				this.day = dt.Day;
			}
			if (this.cZoneBlocks != 0)
			{
				for (int ii=0; ii<this.cZoneBlocks; ii++)
				{
					for (int jj=0; jj<this.cZoneLen[ii]; jj++)
					{
						if (zone[ii][jj].name != null && zone[ii][jj].bChanged)
						{
							string str = String.Format("{0:HH:mm:ss}  {1,-16}  {2}",
								dt, this.zone[ii][jj].name, 
								Logger.strZoneState[this.zone[ii][jj].status & 3]);
							WriteLine(str);
						}
					}
				}
			}
			if (this.cControlBlocks != 0)
			{
				for (int ii=0; ii<this.cControlBlocks; ii++)
				{
					for (int jj=0; jj<this.cControlLen[ii]; jj++)
					{
						if (control[ii][jj].name != null && control[ii][jj].bChanged)
						{
							string str = String.Format("{0:HH:mm:ss}  {1,-16}  {2,3}",
								dt, this.control[ii][jj].name, 
								this.control[ii][jj].status);
							if (this.control[ii][jj].time != 0)
								str = str + String.Format("  (for {0} sec)",this.control[ii][jj].time);
							WriteLine(str);
						}
					}
				}
			}
			if (cOldLines != this.cLines)
			{
				UpdateTextBox();
				if (file != null) file.Flush();
			}
		}

		private void WriteLine(string str)
		{
            if (file != null) file.WriteLine(str);    // Worry about last timer tick after closing.
            string myStr = str + "\r\n";
			strBuffer[cLines % MAX_STRINGS] = myStr;
			cLines++;
		}

		private void UpdateTextBox()
		{
			string str = "";
			int ii;

			if (cLines > MAX_STRINGS)
			{
				for (ii=(cLines%MAX_STRINGS); ii<MAX_STRINGS; ii++)
					str += this.strBuffer[ii];
			}
			for (ii=0; ii<(cLines%MAX_STRINGS); ii++)
				str += this.strBuffer[ii];
			this.textBox1.Text = str;
			// Make sure to show the latest text.
			textBox1.SelectionStart = textBox1.Text.Length; 
			textBox1.SelectionLength = 0; 
			textBox1.ScrollToCaret();
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			GetStatus();
			PrintNewStates();
		}
	
		protected override void OnClosed(EventArgs e)
		{
			timer1.Stop();
			file.Close();
			file = null;
			base.OnClosed (e);
		}
	}
}
