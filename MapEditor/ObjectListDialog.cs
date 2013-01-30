using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using NoxShared;

namespace NoxMapEditor
{
	/// <summary>
	/// Summary description for ObjectListDialog.
	/// </summary>
	public class ObjectListDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.DataGrid dataGrid1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		protected DataTable objList;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem goToObjectToolStripMenuItem;
        private ToolStripMenuItem deleteObjectToolStripMenuItem;
        private ToolStripMenuItem editObjectToolStripMenuItem;
    
		public Map.ObjectTable objTable
		{
			set
			{
				objList = new DataTable("objList");
				objList.Columns.Add("Extent",Type.GetType("System.UInt32"));
				objList.Columns.Add("X-Coor.",Type.GetType("System.Single"));
				objList.Columns.Add("Y-Coor.",Type.GetType("System.Single"));
				objList.Columns.Add("Name",Type.GetType("System.String"));
				objList.Columns.Add("Scr. Name", Type.GetType("System.String"));
				foreach (Map.Object obj in value)
					objList.Rows.Add(new Object[] {obj.Extent,obj.Location.X,obj.Location.Y,obj.Name,obj.Scr_Name});
				dataGrid1.DataSource = objList;
               
			}
		}
		public MapView map;

		public ObjectListDialog()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjectListDialog));
            this.dataGrid1 = new System.Windows.Forms.DataGrid();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.goToObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGrid1
            // 
            this.dataGrid1.AllowNavigation = false;
            this.dataGrid1.DataMember = "";
            resources.ApplyResources(this.dataGrid1, "dataGrid1");
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.ReadOnly = true;
            this.dataGrid1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGrid1_MouseDoubleClick);
            this.dataGrid1.Navigate += new System.Windows.Forms.NavigateEventHandler(this.dataGrid1_Navigate);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goToObjectToolStripMenuItem,
            this.deleteObjectToolStripMenuItem,
            this.editObjectToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // goToObjectToolStripMenuItem
            // 
            this.goToObjectToolStripMenuItem.Name = "goToObjectToolStripMenuItem";
            resources.ApplyResources(this.goToObjectToolStripMenuItem, "goToObjectToolStripMenuItem");
            this.goToObjectToolStripMenuItem.Click += new System.EventHandler(this.goToObjectToolStripMenuItem_Click);
            // 
            // deleteObjectToolStripMenuItem
            // 
            this.deleteObjectToolStripMenuItem.Name = "deleteObjectToolStripMenuItem";
            resources.ApplyResources(this.deleteObjectToolStripMenuItem, "deleteObjectToolStripMenuItem");
            this.deleteObjectToolStripMenuItem.Click += new System.EventHandler(this.deleteObjectToolStripMenuItem_Click);
            // 
            // editObjectToolStripMenuItem
            // 
            this.editObjectToolStripMenuItem.Name = "editObjectToolStripMenuItem";
            resources.ApplyResources(this.editObjectToolStripMenuItem, "editObjectToolStripMenuItem");
            // 
            // ObjectListDialog
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.dataGrid1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ObjectListDialog";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void dataGrid1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			map.centerPoint(new Point((int)((float)objList.Rows[dataGrid1.CurrentRowIndex]["X-Coor."]), (int)((float)objList.Rows[dataGrid1.CurrentRowIndex]["Y-Coor."])));
		}

        private void dataGrid1_Navigate(object sender, NavigateEventArgs ne)
        {

        }

        private void goToObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            map.centerPoint(new Point((int)((float)objList.Rows[dataGrid1.CurrentRowIndex]["X-Coor."]), (int)((float)objList.Rows[dataGrid1.CurrentRowIndex]["Y-Coor."])));
        }

        private void deleteObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //map.RemoveObject(new Point((int)((float)objList.Rows[dataGrid1.CurrentRowIndex]["X-Coor."]), (int)((float)objList.Rows[dataGrid1.CurrentRowIndex]["Y-Coor."])));
        }
	}
}
