using Microsoft.Win32;	// For Registry
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;				// File writing.

namespace Hai
{
	/// <summary>
	/// Presents all defined names for selection.
	/// </summary>
	public class Select : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnDisconnect;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TabPage tpEvents;
		private System.Windows.Forms.CheckedListBox lbLogZones;
		private System.Windows.Forms.CheckedListBox lbLogControls;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.TextBox txtFileName;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btnHide;
		private System.Windows.Forms.Button btnStartLog;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtLogInterval;

		Hai myHai;
		StreamWriter fileLog;
		StreamWriter fileEventLog;
		private System.Windows.Forms.TextBox txtEventInterval;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Button btnStartEventLog;
		private System.Windows.Forms.Button btnEventFileDialog;
		private System.Windows.Forms.TextBox txtEventFileName;
		private System.Windows.Forms.OpenFileDialog openFileDialog2;
		Logger log;
		EventLog evlog;
		Email email;

		class ListEntry
		{
			public string name;
			public int number;

			public ListEntry(string _name,int _index)
			{
				name = _name;
				number = _index+1;
			}
			
			public override string ToString()
			{
				return name;
			}
		}

		public Select(Hai hai)
		{
			InitializeComponent();	// Required for Windows Form Designer support
			myHai = hai;
		
			// Recall last log file.
			RegistryKey regkey = Registry.CurrentUser.OpenSubKey(Connect.strMyRegistry);
			if (regkey != null)
			{
				string str = (string) regkey.GetValue("LogFileName", "");
				if (str.Length > 0)
				{
					this.txtFileName.Text = str;
					this.openFileDialog1.FileName = str;
				}
				str = (string) regkey.GetValue("EventLogFileName", "");
				if (str.Length > 0)
				{
					this.txtEventFileName.Text = str;
					this.openFileDialog2.FileName = str;
				}
			}

			int ii;
			for (ii=0; ii<Hai.MAX_ZONES; ii++)
			{
				if (myHai.haiNames.zones[ii] != null)
				{
					ListEntry le = new ListEntry(myHai.haiNames.zones[ii],ii);
					this.lbLogZones.Items.Add(le,false);
				}
			}
			for (ii=0; ii<Hai.MAX_UNITS; ii++)
			{
				if (myHai.haiNames.units[ii] != null)
				{
					ListEntry le = new ListEntry(myHai.haiNames.units[ii],ii);
					this.lbLogControls.Items.Add(le,false);
				}
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
			this.btnDisconnect = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.txtLogInterval = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.btnStartLog = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.txtFileName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lbLogControls = new System.Windows.Forms.CheckedListBox();
			this.lbLogZones = new System.Windows.Forms.CheckedListBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.txtEventInterval = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.btnStartEventLog = new System.Windows.Forms.Button();
			this.btnEventFileDialog = new System.Windows.Forms.Button();
			this.txtEventFileName = new System.Windows.Forms.TextBox();
			this.tpEvents = new System.Windows.Forms.TabPage();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.btnHide = new System.Windows.Forms.Button();
			this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnDisconnect
			// 
			this.btnDisconnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDisconnect.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnDisconnect.Location = new System.Drawing.Point(16, 456);
			this.btnDisconnect.Name = "btnDisconnect";
			this.btnDisconnect.Size = new System.Drawing.Size(96, 32);
			this.btnDisconnect.TabIndex = 0;
			this.btnDisconnect.Text = "Disconnect";
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(8, 8);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(312, 440);
			this.tabControl1.TabIndex = 1;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.txtLogInterval);
			this.tabPage1.Controls.Add(this.label8);
			this.tabPage1.Controls.Add(this.label7);
			this.tabPage1.Controls.Add(this.btnStartLog);
			this.tabPage1.Controls.Add(this.label5);
			this.tabPage1.Controls.Add(this.button1);
			this.tabPage1.Controls.Add(this.txtFileName);
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.lbLogControls);
			this.tabPage1.Controls.Add(this.lbLogZones);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(304, 414);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Logging";
			// 
			// txtLogInterval
			// 
			this.txtLogInterval.Location = new System.Drawing.Point(8, 384);
			this.txtLogInterval.Name = "txtLogInterval";
			this.txtLogInterval.Size = new System.Drawing.Size(88, 20);
			this.txtLogInterval.TabIndex = 4;
			this.txtLogInterval.Text = "30";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 360);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(120, 16);
			this.label8.TabIndex = 9;
			this.label8.Text = "Interval (seconds)";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 304);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(152, 16);
			this.label7.TabIndex = 8;
			this.label7.Text = "Log File Name";
			// 
			// btnStartLog
			// 
			this.btnStartLog.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnStartLog.Location = new System.Drawing.Point(144, 368);
			this.btnStartLog.Name = "btnStartLog";
			this.btnStartLog.Size = new System.Drawing.Size(104, 32);
			this.btnStartLog.TabIndex = 5;
			this.btnStartLog.Text = "Start Logging";
			this.btnStartLog.Click += new System.EventHandler(this.btnStartLog_Click);
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(8, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(256, 24);
			this.label5.TabIndex = 6;
			this.label5.Text = "Select Zones and Controls for Logging";
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.button1.Location = new System.Drawing.Point(258, 328);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(34, 22);
			this.button1.TabIndex = 3;
			this.button1.Text = "...";
			this.button1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// txtFileName
			// 
			this.txtFileName.Location = new System.Drawing.Point(8, 328);
			this.txtFileName.Name = "txtFileName";
			this.txtFileName.Size = new System.Drawing.Size(248, 20);
			this.txtFileName.TabIndex = 2;
			this.txtFileName.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(160, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Controls";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Zones";
			// 
			// lbLogControls
			// 
			this.lbLogControls.CheckOnClick = true;
			this.lbLogControls.Location = new System.Drawing.Point(160, 64);
			this.lbLogControls.Name = "lbLogControls";
			this.lbLogControls.Size = new System.Drawing.Size(136, 229);
			this.lbLogControls.TabIndex = 1;
			// 
			// lbLogZones
			// 
			this.lbLogZones.CheckOnClick = true;
			this.lbLogZones.Location = new System.Drawing.Point(8, 64);
			this.lbLogZones.Name = "lbLogZones";
			this.lbLogZones.Size = new System.Drawing.Size(136, 229);
			this.lbLogZones.TabIndex = 0;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.txtEventInterval);
			this.tabPage2.Controls.Add(this.label9);
			this.tabPage2.Controls.Add(this.label10);
			this.tabPage2.Controls.Add(this.btnStartEventLog);
			this.tabPage2.Controls.Add(this.btnEventFileDialog);
			this.tabPage2.Controls.Add(this.txtEventFileName);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(304, 414);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Events";
			// 
			// txtEventInterval
			// 
			this.txtEventInterval.Location = new System.Drawing.Point(8, 88);
			this.txtEventInterval.Name = "txtEventInterval";
			this.txtEventInterval.Size = new System.Drawing.Size(88, 20);
			this.txtEventInterval.TabIndex = 12;
			this.txtEventInterval.Text = "30";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 64);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(120, 16);
			this.label9.TabIndex = 15;
			this.label9.Text = "Interval (seconds)";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(8, 8);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(152, 16);
			this.label10.TabIndex = 14;
			this.label10.Text = "Event Log File Name";
			// 
			// btnStartEventLog
			// 
			this.btnStartEventLog.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnStartEventLog.Location = new System.Drawing.Point(144, 72);
			this.btnStartEventLog.Name = "btnStartEventLog";
			this.btnStartEventLog.Size = new System.Drawing.Size(112, 32);
			this.btnStartEventLog.TabIndex = 13;
			this.btnStartEventLog.Text = "Start Event Logging";
			this.btnStartEventLog.Click += new System.EventHandler(this.btnStartEventLog_Click);
			// 
			// btnEventFileDialog
			// 
			this.btnEventFileDialog.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnEventFileDialog.Location = new System.Drawing.Point(264, 32);
			this.btnEventFileDialog.Name = "btnEventFileDialog";
			this.btnEventFileDialog.Size = new System.Drawing.Size(34, 22);
			this.btnEventFileDialog.TabIndex = 11;
			this.btnEventFileDialog.Text = "...";
			this.btnEventFileDialog.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.btnEventFileDialog.Click += new System.EventHandler(this.btnEventFileDialog_Click);
			// 
			// txtEventFileName
			// 
			this.txtEventFileName.Location = new System.Drawing.Point(8, 32);
			this.txtEventFileName.Name = "txtEventFileName";
			this.txtEventFileName.Size = new System.Drawing.Size(248, 20);
			this.txtEventFileName.TabIndex = 10;
			this.txtEventFileName.Text = "EventLog.txt";
			// 
			// tpEvents
			// 
			this.tpEvents.Location = new System.Drawing.Point(0, 0);
			this.tpEvents.Name = "tpEvents";
			this.tpEvents.TabIndex = 0;
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.CheckFileExists = false;
			this.openFileDialog1.DefaultExt = "txt";
			this.openFileDialog1.Filter = "Text Files|*.txt|All Files|*.*";
			this.openFileDialog1.Title = "Specify Log File";
			this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
			// 
			// btnHide
			// 
			this.btnHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnHide.Enabled = false;
			this.btnHide.Location = new System.Drawing.Point(232, 456);
			this.btnHide.Name = "btnHide";
			this.btnHide.Size = new System.Drawing.Size(72, 32);
			this.btnHide.TabIndex = 2;
			this.btnHide.Text = "Hide";
			// 
			// openFileDialog2
			// 
			this.openFileDialog2.CheckFileExists = false;
			this.openFileDialog2.DefaultExt = "txt";
			this.openFileDialog2.Filter = "Text Files|*.txt|All Files|*.*";
			this.openFileDialog2.Title = "Specify Event Log File";
			this.openFileDialog2.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog2_FileOk);
			// 
			// Select
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(328, 494);
			this.Controls.Add(this.btnHide);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.btnDisconnect);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.Name = "Select";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select";
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.txtFileName.Text = this.openFileDialog1.FileName;
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.openFileDialog1.ShowDialog();
		}
	
		private void openFileDialog2_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.txtEventFileName.Text = this.openFileDialog2.FileName;
		}

		private void btnEventFileDialog_Click(object sender, System.EventArgs e)
		{
			this.openFileDialog2.ShowDialog();
		}

		protected override void OnClosed(EventArgs e)
		{
			// Record connection data in the registry.
			RegistryKey regkey = Registry.CurrentUser.OpenSubKey(Connect.strMyRegistry,true);
			if (regkey == null)
				regkey = Registry.CurrentUser.CreateSubKey(Connect.strMyRegistry);
			regkey.SetValue("LogFileName",this.txtFileName.Text);
			regkey.SetValue("EventLogFileName",this.txtEventFileName.Text);

			base.OnClosed (e);
		}

		private void btnStartLog_Click(object sender, System.EventArgs e)
		{
			// validate time interval
			int logInterval;
			try
			{
				logInterval = Int32.Parse(this.txtLogInterval.Text);
			}
			catch
			{
				MessageBox.Show("Invalid logging interval.","Invalid Entry",MessageBoxButtons.OK);
				return;
			}

			// validate and open file
			try
			{
				fileLog = File.AppendText(this.txtFileName.Text);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message,"File Open Error",MessageBoxButtons.OK);
				return;
			}

			// Create boolean arrays.
			bool[] bZones = new bool[Hai.MAX_ZONES];
			bool[] bControls = new bool[Hai.MAX_UNITS];
			int cChecked = 0;
			for (int ii=0; ii < this.lbLogZones.Items.Count; ii++)
			{
				if (this.lbLogZones.GetItemChecked(ii))
				{
					bZones[((ListEntry) this.lbLogZones.Items[ii]).number-1] = true;
					cChecked++;
				}
			}
			for (int ii=0; ii < this.lbLogControls.Items.Count; ii++)
			{
				if (this.lbLogControls.GetItemChecked(ii))
				{
					bControls[((ListEntry) this.lbLogControls.Items[ii]).number-1] = true;
					cChecked++;
				}
			}

			// launch logger
			log = new Logger(myHai,this,fileLog,logInterval,bZones,bControls);
			log.Show();
		}

		private void btnStartEventLog_Click(object sender, System.EventArgs e)
		{
			// validate time interval
			int logInterval;
			try
			{
				logInterval = Int32.Parse(this.txtEventInterval.Text);
			}
			catch
			{
				MessageBox.Show("Invalid event logging interval.","Invalid Entry",MessageBoxButtons.OK);
				return;
			}

			// validate and open file
			try
			{
				fileEventLog = File.AppendText(this.txtEventFileName.Text);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message,"File Open Error",MessageBoxButtons.OK);
				return;
			}

            try
            {
                email = new Email("chuck@whitmer.us");
            }
            catch 
            {
                email = null;
            }

            // launch logger
            evlog = new EventLog(myHai, this, fileEventLog, logInterval, email);
			evlog.Show();
		
		}

	}
}
