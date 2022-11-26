using Microsoft.Win32;
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
//using System.Runtime.InteropServices;
using System.Globalization;

/*
[StructLayout(LayoutKind.Sequential)] 
public class LOGFONT 
{ 
	public const int LF_FACESIZE = 32; 
	public int lfHeight; 
	public int lfWidth; 
	public int lfEscapement; 
	public int lfOrientation; 
	public int lfWeight; 
	public byte lfItalic; 
	public byte lfUnderline; 
	public byte lfStrikeOut; 
	public byte lfCharSet; 
	public byte lfOutPrecision; 
	public byte lfClipPrecision; 
	public byte lfQuality; 
	public byte lfPitchAndFamily; 
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst=LF_FACESIZE)] 
	public string lfFaceName; 
} 
*/



namespace Hai
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Connect : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtCode;
		private System.Windows.Forms.Button btnConnect;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.TextBox txtIpAddress;
		private System.Windows.Forms.TextBox txtPrivateKey;
		public const string strMyRegistry = "Software\\Whitmer\\HAI";

		public Connect()
		{
			InitializeComponent();	// Required for Windows Form Designer support
			// Try to load previously typed values.
			RegistryKey regkey = Registry.CurrentUser.OpenSubKey(strMyRegistry);
			if (regkey != null)
			{
				txtIpAddress.Text = (string) regkey.GetValue("IP Address", "0.0.0.0");
				txtPort.Text = (string) regkey.GetValue("Port", "0");
				txtPrivateKey.Text = (string) regkey.GetValue("Private Key", "00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00");
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
            this.txtIpAddress = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtPrivateKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtIpAddress
            // 
            this.txtIpAddress.Location = new System.Drawing.Point(19, 28);
            this.txtIpAddress.Name = "txtIpAddress";
            this.txtIpAddress.Size = new System.Drawing.Size(106, 22);
            this.txtIpAddress.TabIndex = 1;
            this.txtIpAddress.Text = "000.000.000.000";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(19, 83);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(58, 22);
            this.txtPort.TabIndex = 2;
            this.txtPort.Text = "0000";
            // 
            // txtPrivateKey
            // 
            this.txtPrivateKey.Location = new System.Drawing.Point(19, 138);
            this.txtPrivateKey.Name = "txtPrivateKey";
            this.txtPrivateKey.Size = new System.Drawing.Size(307, 22);
            this.txtPrivateKey.TabIndex = 3;
            this.txtPrivateKey.Text = "00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 19);
            this.label1.TabIndex = 4;
            this.label1.Text = "IP Address";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(19, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 18);
            this.label2.TabIndex = 5;
            this.label2.Text = "Port";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(19, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(221, 18);
            this.label3.TabIndex = 6;
            this.label3.Tag = "";
            this.label3.Text = "Private Key";
            // 
            // btnExit
            // 
            this.btnExit.CausesValidation = false;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(202, 258);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(86, 37);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(19, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 19);
            this.label4.TabIndex = 9;
            this.label4.Text = "Code";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(19, 194);
            this.txtCode.Name = "txtCode";
            this.txtCode.PasswordChar = 'x';
            this.txtCode.Size = new System.Drawing.Size(144, 22);
            this.txtCode.TabIndex = 4;
            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnConnect.Location = new System.Drawing.Point(38, 217);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(96, 37);
            this.btnConnect.TabIndex = 5;
            this.btnConnect.Text = "Connect";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // Connect
            // 
            this.AcceptButton = this.btnConnect;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPrivateKey);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtIpAddress);
            this.Name = "Connect";
            this.Text = "HAI - Connection";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Connect());
		}

		protected override void OnClosed(EventArgs e)
		{
			// Record connection data in the registry.
			RegistryKey regkey = Registry.CurrentUser.OpenSubKey(strMyRegistry,true);
			if (regkey == null)
				regkey = Registry.CurrentUser.CreateSubKey(strMyRegistry);
			regkey.SetValue("IP Address",this.txtIpAddress.Text);
			regkey.SetValue("Port",this.txtPort.Text);
			regkey.SetValue("Private Key",this.txtPrivateKey.Text);

			base.OnClosed (e);
		}

		private void btnExit_Click(object sender, System.EventArgs e)
		{
			base.Close();
		}

		private void btnConnect_Click(object sender, System.EventArgs e)
		{
			int port;
			try 
			{
				port = Int32.Parse(txtPort.Text);
			}
			catch 
			{
				MessageBox.Show("The port must be a valid number.", "Invalid Port",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			byte[] privateKey = new byte[16];
			try
			{
				char[] delim = {':'};
				string[] tokens = this.txtPrivateKey.Text.Split(delim);
				if (tokens.Length != 16)
					throw new Exception();
				for (int ii=0; ii<16; ii++)
				{
					privateKey[ii] = byte.Parse(tokens[ii], NumberStyles.AllowHexSpecifier);
				}
			}
			catch
			{
				MessageBox.Show("The private key must contain 16 hex bytes separated by colons.", "Invalid Key",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			byte[] code = new byte[4];
			try 
			{
				if (txtCode.Text.Length != 4)
					throw new Exception("Invalid code length.");
				for (int ii=0; ii<4; ii++)
				{
					code[ii] = (byte) (txtCode.Text[ii] - '0');
					if (code[ii] > 9)
						throw new Exception("Invalid code digit.");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Invalid Code",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			// Try to open a connection.

			Hai myHai;
			try 
			{
				myHai = new Hai(txtIpAddress.Text,port,privateKey,code);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message,"Net Open Failure",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			this.Hide();
			Select sel = new Select(myHai);
			sel.ShowDialog();
			this.Show();
			myHai.Close();
		}

	}
}
