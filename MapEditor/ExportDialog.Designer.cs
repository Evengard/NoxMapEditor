namespace NoxMapEditor
{
	partial class ExportDialog
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
			this.categoryList = new System.Windows.Forms.ListBox();
			this.itemList = new System.Windows.Forms.ListBox();
			this.exportButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// categoryList
			// 
			this.categoryList.FormattingEnabled = true;
			this.categoryList.Items.AddRange(new object[] {
            "Tiles",
            "Objects",
            "Polygons",
            "Scripts",
            "Groups",
            "Walls"});
			this.categoryList.Location = new System.Drawing.Point(12, 12);
			this.categoryList.Name = "categoryList";
			this.categoryList.Size = new System.Drawing.Size(112, 95);
			this.categoryList.TabIndex = 0;
			this.categoryList.SelectedIndexChanged += new System.EventHandler(this.categoryList_SelectedIndexChanged);
			// 
			// itemList
			// 
			this.itemList.FormattingEnabled = true;
			this.itemList.Location = new System.Drawing.Point(147, 12);
			this.itemList.Name = "itemList";
			this.itemList.Size = new System.Drawing.Size(207, 290);
			this.itemList.TabIndex = 1;
			// 
			// exportButton
			// 
			this.exportButton.Location = new System.Drawing.Point(12, 113);
			this.exportButton.Name = "exportButton";
			this.exportButton.Size = new System.Drawing.Size(75, 23);
			this.exportButton.TabIndex = 2;
			this.exportButton.Text = "Export";
			this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
			// 
			// ExportDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(366, 314);
			this.Controls.Add(this.exportButton);
			this.Controls.Add(this.itemList);
			this.Controls.Add(this.categoryList);
			this.Name = "ExportDialog";
			this.Text = "Export Item";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox categoryList;
		private System.Windows.Forms.ListBox itemList;
		private System.Windows.Forms.Button exportButton;
	}
}