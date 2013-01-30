namespace NoxMapEditor
{
	partial class WaypointPropertiesDialog
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
            this.wpBox = new System.Windows.Forms.ComboBox();
            this.connList = new System.Windows.Forms.ListBox();
            this.nameText = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.addButton = new System.Windows.Forms.Button();
            this.enabledCheck = new System.Windows.Forms.CheckBox();
            this.flagText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.delButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // wpBox
            // 
            this.wpBox.FormattingEnabled = true;
            this.wpBox.Location = new System.Drawing.Point(133, 163);
            this.wpBox.Name = "wpBox";
            this.wpBox.Size = new System.Drawing.Size(120, 21);
            this.wpBox.TabIndex = 0;
            // 
            // connList
            // 
            this.connList.FormattingEnabled = true;
            this.connList.HorizontalScrollbar = true;
            this.connList.Location = new System.Drawing.Point(133, 23);
            this.connList.Name = "connList";
            this.connList.Size = new System.Drawing.Size(120, 134);
            this.connList.TabIndex = 1;
            // 
            // nameText
            // 
            this.nameText.Location = new System.Drawing.Point(12, 23);
            this.nameText.Name = "nameText";
            this.nameText.Size = new System.Drawing.Size(115, 20);
            this.nameText.TabIndex = 2;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(12, 160);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(12, 189);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(132, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Connections";
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(186, 189);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(34, 23);
            this.addButton.TabIndex = 7;
            this.addButton.Text = "Add";
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // enabledCheck
            // 
            this.enabledCheck.AutoSize = true;
            this.enabledCheck.Location = new System.Drawing.Point(13, 49);
            this.enabledCheck.Name = "enabledCheck";
            this.enabledCheck.Size = new System.Drawing.Size(65, 17);
            this.enabledCheck.TabIndex = 8;
            this.enabledCheck.Text = "Enabled";
            this.enabledCheck.CheckedChanged += new System.EventHandler(this.enabledCheck_CheckedChanged);
            // 
            // flagText
            // 
            this.flagText.Location = new System.Drawing.Point(162, 189);
            this.flagText.Name = "flagText";
            this.flagText.Size = new System.Drawing.Size(21, 20);
            this.flagText.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(132, 192);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Flag";
            // 
            // delButton
            // 
            this.delButton.Location = new System.Drawing.Point(222, 189);
            this.delButton.Name = "delButton";
            this.delButton.Size = new System.Drawing.Size(31, 23);
            this.delButton.TabIndex = 11;
            this.delButton.Text = "Del";
            this.delButton.Click += new System.EventHandler(this.delButton_Click);
            // 
            // WaypointPropertiesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(265, 222);
            this.Controls.Add(this.delButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.flagText);
            this.Controls.Add(this.enabledCheck);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.nameText);
            this.Controls.Add(this.connList);
            this.Controls.Add(this.wpBox);
            this.Name = "WaypointPropertiesDialog";
            this.Text = "WaypointPropertiesDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox wpBox;
		private System.Windows.Forms.ListBox connList;
		private System.Windows.Forms.TextBox nameText;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.CheckBox enabledCheck;
		private System.Windows.Forms.TextBox flagText;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button delButton;
	}
}