namespace sELedit.SUB_FORM
{
	partial class LogsChangedForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogsChangedForm));
			this.richTextBox_log = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// richTextBox_log
			// 
			this.richTextBox_log.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
			this.richTextBox_log.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox_log.ForeColor = System.Drawing.Color.White;
			this.richTextBox_log.Location = new System.Drawing.Point(0, 0);
			this.richTextBox_log.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.richTextBox_log.Name = "richTextBox_log";
			this.richTextBox_log.ReadOnly = true;
			this.richTextBox_log.Size = new System.Drawing.Size(2044, 881);
			this.richTextBox_log.TabIndex = 0;
			this.richTextBox_log.Text = "";
			this.richTextBox_log.TextChanged += new System.EventHandler(this.richTextBox_log_TextChanged);
			// 
			// LogsChangedForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(2044, 881);
			this.Controls.Add(this.richTextBox_log);
			this.Font = new System.Drawing.Font("Microsoft Tai Le", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.Name = "LogsChangedForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Logs Changed";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox richTextBox_log;
	}
}