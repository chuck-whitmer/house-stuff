using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace Hai
{
	/// <summary>
	/// Summary description for EventLog.
	/// </summary>
	public class EventLog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox textBox1;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Timer timer1;

		Hai myHai;		// For low-level calls.
		Select sel;		// Parent window.
		int interval;	// Polling time interval, in seconds.
		StreamWriter file;	// Output file.
		const int MAX_STRINGS = 100;
		string[] strBuffer = new string[MAX_STRINGS];
		int cLines = 0;
		int day;
		Email email;
		int emailSentLines = 0;
		DateTime emailSentTime;

		public EventLog(Hai hai, Select sel, StreamWriter sw, int interval, Email email)
		{
			InitializeComponent();	// Required for Windows Form Designer support
			this.myHai = hai;
			this.sel = sel;
			this.file = sw;
			this.interval = interval;
			this.timer1.Interval = this.interval * 1000;
			this.email = email;
			Int16[] events = myHai.GetEvents();		// Clear the event buffer.
			this.timer1.Start();
			DateTime dt = DateTime.Now;
			string str = String.Format("{0:HH:mm:ss}  {1,-16}  {2:yyyy.MM.dd}",	dt, "[New Date]", dt);
			WriteLine(str);
			this.day = dt.Day;
			str = String.Format("{0:HH:mm:ss}  {1,-16}", dt, "Start Event Log");
			WriteLine(str);
			UpdateTextBox();
			file.Flush();
			if (email != null)
			{
				email.Send();
				emailSentLines = cLines;
				emailSentTime = DateTime.Now;
			}
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
			this.textBox1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textBox1.Location = new System.Drawing.Point(8, 8);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new System.Drawing.Size(384, 376);
			this.textBox1.TabIndex = 0;
			this.textBox1.TabStop = false;
			this.textBox1.Text = "";
			this.textBox1.WordWrap = false;
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// EventLog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(400, 438);
			this.Controls.Add(this.textBox1);
			this.MaximizeBox = false;
			this.Name = "EventLog";
			this.Text = "EventLog";
			this.ResumeLayout(false);

		}
		#endregion

		private void WriteLine(string str)
		{
			string myStr = str + "\r\n";
			if (file != null) file.Write(myStr);	// Worry about last timer tick after closing.
			strBuffer[cLines % MAX_STRINGS] = myStr;
			cLines++;
			if (email != null) email.Add(str);
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
			DateTime dt = DateTime.Now;
			int cOldLines = this.cLines;
			if (dt.Day != this.day)
			{
				string str = String.Format("{0:HH:mm:ss}  {1,-16}  {2:yyyy.MM.dd}", dt, "[New Date]", dt);
				WriteLine(str);
				this.day = dt.Day;
			}
			Int16[] events;
			try
			{
				events = myHai.GetEvents();
				if (events != null)
				{
					string str, str1, str2;
					int cc = events.Length;
					for (int ii=0; ii<cc; ii++)
					{
						if ((events[ii] & 0xFC00) == 0x0400)	// Zone
						{
							str1 = myHai.haiNames.zones[(events[ii] & 0x01FF)-1];
							str2 = ((events[ii] & 0x0200) == 0x0200) ? "Not Ready" : "Secure";
						}
						else if ((events[ii] & 0xFC00) == 0x0800)	// Control
						{
							str1 = myHai.haiNames.units[(events[ii] & 0x01FF)-1];
							str2 = ((events[ii] & 0x0200) == 0x0200) ? "On" : "Off";
						}
						else
						{
							switch (events[ii])
							{
								case 0x0300:
									str1 = "Phone";
									str2 = "Dead";
									break;
								case 0x0301:
									str1 = "Phone";
									str2 = "Ring";
									break;
								case 0x0302:
									str1 = "Phone";
									str2 = "Off Hook";
									break;
								case 0x0303:
									str1 = "Phone";
									str2 = "On Hook";
									break;
								case 0x0304:
									str1 = "AC Power";
									str2 = "Off";
									break;
								case 0x0305:
									str1 = "AC Power";
									str2 = "On";
									break;
								case 0x0306:
									str1 = "Battery";
									str2 = "Low";
									break;
								case 0x0307:
									str1 = "Battery";
									str2 = "OK";
									break;
								default:
									str1 = "Unknown Code";
									str2 = events[ii].ToString("X4");
									break;
							}
						}
						str = String.Format("{0:HH:mm:ss}  {1,-16}  {2}", dt, str1, str2);
						WriteLine(str);
					}
					if (cc == 15) // This seems to be a maximum...
					{
						str = String.Format("{0:HH:mm:ss}  [More?]", dt);
						WriteLine(str);
					}
				}
			}
			catch (Hai.OmniException oex)
			{
				if (oex.Error == Hai.EHAITIMEOUT)
				{
					string str = String.Format("{0:HH:mm:ss}  [Net Timeout]", dt);
					WriteLine(str);
				}
				else
				{
					throw oex;
				}
			}
			if (cOldLines != this.cLines)
			{
				UpdateTextBox();
				if (file != null) file.Flush();
			}
			if (email != null)
			{
				if ((dt - emailSentTime).TotalMinutes >= 10.0 && emailSentLines < cLines)
				{
					email.Send();
					emailSentTime = dt;
					emailSentLines = cLines;
				}
				else if ((dt - emailSentTime).TotalMinutes >= 40.0 && emailSentLines == cLines) //!!! Raise time
				{
                    string str = String.Format("{0:HH:mm:ss}  All Clear", dt);
					email.Add(str);
                    email.Send();
                    emailSentTime = dt;
                }
            }
		}
	
		protected override void OnClosed(EventArgs e)
		{
			timer1.Stop();
			DateTime dt = DateTime.Now;
			String str = String.Format("{0:HH:mm:ss}  {1,-16}", dt, "Stop Event Log");
			WriteLine(str);
			file.Close();
			file = null;
			base.OnClosed (e);
		}

	}
}
