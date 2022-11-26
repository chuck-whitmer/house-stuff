using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Hai
{
	/// <summary>
	/// Summary description for HaiNames.
	/// </summary>
	public class HaiNames : System.Windows.Forms.Form
	{
		string[][] names = 
		{
			new string[Hai.MAX_ZONES], 
			new string[Hai.MAX_UNITS],
			new string[Hai.MAX_BUTTONS],
			new string[Hai.MAX_CODES],
			new string[Hai.MAX_AREAS],
			new string[Hai.MAX_THERMOS],
			new string[Hai.MAX_MESGS]
		};
		int cNames = 0;
		Hai hai;

		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public HaiNames(Hai _hai)
		{
			hai = _hai;
			InitializeComponent();	// Required for Windows Form Designer support
			label1.Text = "";
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
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(24, 23);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(168, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Uploading Names ...";
			// 
			// HaiNames
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(216, 62);
			this.ControlBox = false;
			this.Controls.Add(this.label1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "HaiNames";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Hai Getting Names";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Retrieves all defined names from the HAI system.
		/// </summary>
		public int UploadNames()
		{
			int type;
			int index;
			string name;
			this.Refresh();  // For some reason this causes the button to paint correctly.
			bool bValid = hai.GetFirstName(out type,out index,out name);
			while (bValid)
			{
				names[type-1][index-1] = name;
				cNames++;
				label1.Text = "Uploading Names ... " + cNames.ToString();
				label1.Refresh();
				bValid = hai.GetNextName(out type,out index,out name);
			}
			return cNames;
		}

		/// <summary>
		/// Returns the total number of names loaded.
		/// </summary>
		public int Count
		{
			get
			{
				return cNames;
			}
		}

		public string[] zones
		{
			get
			{
				return names[(int) Hai.NameType.Zones];
			}
		}
		public string[] units
		{
			get
			{
				return names[(int) Hai.NameType.Units];
			}
		}
	}
}
