using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;

using NoxShared;

namespace NoxMapEditor
{
	public class BlendDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ComboBox tile1Graphic;
		private System.Windows.Forms.ComboBox tile1Var;
		private System.Windows.Forms.ComboBox tile2Var;
		private System.Windows.Forms.ComboBox tile2Graphic;
		private System.Windows.Forms.ComboBox tile4Var;
		private System.Windows.Forms.ComboBox tile4Graphic;
		private System.Windows.Forms.ComboBox tile3Var;
		private System.Windows.Forms.ComboBox tile3Graphic;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox tile4Dir;
		private System.Windows.Forms.ComboBox tile3Dir;
		private System.Windows.Forms.ComboBox tile2Dir;
		private System.Windows.Forms.ComboBox tile1Dir;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox tile1Enabled;
		private System.Windows.Forms.CheckBox tile2Enabled;
		private System.Windows.Forms.CheckBox tile3Enabled;
		private System.Windows.Forms.CheckBox tile4Enabled;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox tile1BlendType;
		private System.Windows.Forms.ComboBox tile2BlendType;
		private System.Windows.Forms.ComboBox tile3BlendType;
        private System.Windows.Forms.ComboBox tile4BlendType;
        private Panel panelTilePreview;

		public ArrayList Blends = new ArrayList();
		
		public BlendDialog()
		{
			InitializeComponent();

            ArrayList al = ThingDb.FloorTileNames;

			tile1Graphic.Items.AddRange(ThingDb.FloorTileNames.ToArray());
			tile2Graphic.Items.AddRange(ThingDb.FloorTileNames.ToArray());
			tile3Graphic.Items.AddRange(ThingDb.FloorTileNames.ToArray());
			tile4Graphic.Items.AddRange(ThingDb.FloorTileNames.ToArray());

			tile1Dir.Items.AddRange(Enum.GetNames(typeof(Map.Tile.EdgeTile.Direction)));
			tile2Dir.Items.AddRange(Enum.GetNames(typeof(Map.Tile.EdgeTile.Direction)));
			tile3Dir.Items.AddRange(Enum.GetNames(typeof(Map.Tile.EdgeTile.Direction)));
			tile4Dir.Items.AddRange(Enum.GetNames(typeof(Map.Tile.EdgeTile.Direction)));

			tile1BlendType.Items.AddRange(ThingDb.EdgeTileNames.ToArray());
			tile2BlendType.Items.AddRange(ThingDb.EdgeTileNames.ToArray());
			tile3BlendType.Items.AddRange(ThingDb.EdgeTileNames.ToArray());
			tile4BlendType.Items.AddRange(ThingDb.EdgeTileNames.ToArray());
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BlendDialog));
            this.tile1Enabled = new System.Windows.Forms.CheckBox();
            this.tile2Enabled = new System.Windows.Forms.CheckBox();
            this.tile3Enabled = new System.Windows.Forms.CheckBox();
            this.tile4Enabled = new System.Windows.Forms.CheckBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.tile1Graphic = new System.Windows.Forms.ComboBox();
            this.tile1Var = new System.Windows.Forms.ComboBox();
            this.tile2Var = new System.Windows.Forms.ComboBox();
            this.tile2Graphic = new System.Windows.Forms.ComboBox();
            this.tile4Var = new System.Windows.Forms.ComboBox();
            this.tile4Graphic = new System.Windows.Forms.ComboBox();
            this.tile3Var = new System.Windows.Forms.ComboBox();
            this.tile3Graphic = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tile4Dir = new System.Windows.Forms.ComboBox();
            this.tile3Dir = new System.Windows.Forms.ComboBox();
            this.tile2Dir = new System.Windows.Forms.ComboBox();
            this.tile1Dir = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tile1BlendType = new System.Windows.Forms.ComboBox();
            this.tile2BlendType = new System.Windows.Forms.ComboBox();
            this.tile3BlendType = new System.Windows.Forms.ComboBox();
            this.tile4BlendType = new System.Windows.Forms.ComboBox();
            this.panelTilePreview = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // tile1Enabled
            // 
            resources.ApplyResources(this.tile1Enabled, "tile1Enabled");
            this.tile1Enabled.Name = "tile1Enabled";
            // 
            // tile2Enabled
            // 
            resources.ApplyResources(this.tile2Enabled, "tile2Enabled");
            this.tile2Enabled.Name = "tile2Enabled";
            // 
            // tile3Enabled
            // 
            resources.ApplyResources(this.tile3Enabled, "tile3Enabled");
            this.tile3Enabled.Name = "tile3Enabled";
            // 
            // tile4Enabled
            // 
            resources.ApplyResources(this.tile4Enabled, "tile4Enabled");
            this.tile4Enabled.Name = "tile4Enabled";
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // tile1Graphic
            // 
            this.tile1Graphic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tile1Graphic.DropDownWidth = 180;
            resources.ApplyResources(this.tile1Graphic, "tile1Graphic");
            this.tile1Graphic.Name = "tile1Graphic";
            this.tile1Graphic.SelectedIndexChanged += new System.EventHandler(this.tile1Graphic_SelectedIndexChanged);
            // 
            // tile1Var
            // 
            this.tile1Var.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.tile1Var, "tile1Var");
            this.tile1Var.Name = "tile1Var";
            // 
            // tile2Var
            // 
            this.tile2Var.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.tile2Var, "tile2Var");
            this.tile2Var.Name = "tile2Var";
            // 
            // tile2Graphic
            // 
            this.tile2Graphic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tile2Graphic.DropDownWidth = 180;
            resources.ApplyResources(this.tile2Graphic, "tile2Graphic");
            this.tile2Graphic.Name = "tile2Graphic";
            this.tile2Graphic.SelectedIndexChanged += new System.EventHandler(this.tile2Graphic_SelectedIndexChanged);
            // 
            // tile4Var
            // 
            this.tile4Var.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.tile4Var, "tile4Var");
            this.tile4Var.Name = "tile4Var";
            // 
            // tile4Graphic
            // 
            this.tile4Graphic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tile4Graphic.DropDownWidth = 180;
            resources.ApplyResources(this.tile4Graphic, "tile4Graphic");
            this.tile4Graphic.Name = "tile4Graphic";
            this.tile4Graphic.SelectedIndexChanged += new System.EventHandler(this.tile4Graphic_SelectedIndexChanged);
            // 
            // tile3Var
            // 
            this.tile3Var.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.tile3Var, "tile3Var");
            this.tile3Var.Name = "tile3Var";
            // 
            // tile3Graphic
            // 
            this.tile3Graphic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tile3Graphic.DropDownWidth = 180;
            resources.ApplyResources(this.tile3Graphic, "tile3Graphic");
            this.tile3Graphic.Name = "tile3Graphic";
            this.tile3Graphic.SelectedIndexChanged += new System.EventHandler(this.tile3Graphic_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // tile4Dir
            // 
            this.tile4Dir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tile4Dir.DropDownWidth = 128;
            resources.ApplyResources(this.tile4Dir, "tile4Dir");
            this.tile4Dir.Name = "tile4Dir";
            // 
            // tile3Dir
            // 
            this.tile3Dir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tile3Dir.DropDownWidth = 128;
            resources.ApplyResources(this.tile3Dir, "tile3Dir");
            this.tile3Dir.Name = "tile3Dir";
            // 
            // tile2Dir
            // 
            this.tile2Dir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tile2Dir.DropDownWidth = 128;
            resources.ApplyResources(this.tile2Dir, "tile2Dir");
            this.tile2Dir.Name = "tile2Dir";
            // 
            // tile1Dir
            // 
            this.tile1Dir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tile1Dir.DropDownWidth = 128;
            resources.ApplyResources(this.tile1Dir, "tile1Dir");
            this.tile1Dir.Name = "tile1Dir";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // tile1BlendType
            // 
            this.tile1BlendType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tile1BlendType.DropDownWidth = 180;
            resources.ApplyResources(this.tile1BlendType, "tile1BlendType");
            this.tile1BlendType.Name = "tile1BlendType";
            // 
            // tile2BlendType
            // 
            this.tile2BlendType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tile2BlendType.DropDownWidth = 180;
            resources.ApplyResources(this.tile2BlendType, "tile2BlendType");
            this.tile2BlendType.Name = "tile2BlendType";
            // 
            // tile3BlendType
            // 
            this.tile3BlendType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tile3BlendType.DropDownWidth = 180;
            resources.ApplyResources(this.tile3BlendType, "tile3BlendType");
            this.tile3BlendType.Name = "tile3BlendType";
            // 
            // tile4BlendType
            // 
            this.tile4BlendType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tile4BlendType.DropDownWidth = 180;
            resources.ApplyResources(this.tile4BlendType, "tile4BlendType");
            this.tile4BlendType.Name = "tile4BlendType";
            // 
            // panelTilePreview
            // 
            this.panelTilePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.panelTilePreview, "panelTilePreview");
            this.panelTilePreview.Name = "panelTilePreview";
            this.panelTilePreview.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTilePreview_Paint);
            // 
            // BlendDialog
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.panelTilePreview);
            this.Controls.Add(this.tile4BlendType);
            this.Controls.Add(this.tile3BlendType);
            this.Controls.Add(this.tile2BlendType);
            this.Controls.Add(this.tile1BlendType);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tile4Dir);
            this.Controls.Add(this.tile3Dir);
            this.Controls.Add(this.tile2Dir);
            this.Controls.Add(this.tile1Dir);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tile4Var);
            this.Controls.Add(this.tile4Graphic);
            this.Controls.Add(this.tile3Var);
            this.Controls.Add(this.tile3Graphic);
            this.Controls.Add(this.tile2Var);
            this.Controls.Add(this.tile2Graphic);
            this.Controls.Add(this.tile1Var);
            this.Controls.Add(this.tile1Graphic);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.tile4Enabled);
            this.Controls.Add(this.tile3Enabled);
            this.Controls.Add(this.tile2Enabled);
            this.Controls.Add(this.tile1Enabled);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BlendDialog";
            this.ResumeLayout(false);

		}
		#endregion

		private byte GetVariation(ComboBox box)
		{
			return box.SelectedIndex == 0 ? (byte) new Random().Next(((ThingDb.Tile) ThingDb.FloorTiles[box.SelectedIndex]).Variations.Count) : Convert.ToByte(box.Text);
		}

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			Blends = new ArrayList();

			if (tile1Enabled.Checked)
				Blends.Add(new Map.Tile.EdgeTile(
					(byte) tile1Graphic.SelectedIndex,
					GetVariation(tile1Var),
					(Map.Tile.EdgeTile.Direction) Enum.GetValues(typeof(Map.Tile.EdgeTile.Direction)).GetValue(tile1Dir.SelectedIndex),
					(byte) tile1BlendType.SelectedIndex));
			if (tile2Enabled.Checked)
				Blends.Add(new Map.Tile.EdgeTile(
					(byte) tile2Graphic.SelectedIndex,
					GetVariation(tile2Var),
					(Map.Tile.EdgeTile.Direction) Enum.GetValues(typeof(Map.Tile.EdgeTile.Direction)).GetValue(tile2Dir.SelectedIndex),
					(byte) tile2BlendType.SelectedIndex));
			if (tile3Enabled.Checked)
				Blends.Add(new Map.Tile.EdgeTile(
					(byte) tile3Graphic.SelectedIndex,
					GetVariation(tile3Var),
					(Map.Tile.EdgeTile.Direction) Enum.GetValues(typeof(Map.Tile.EdgeTile.Direction)).GetValue(tile3Dir.SelectedIndex),
					(byte) tile3BlendType.SelectedIndex));
			if (tile4Enabled.Checked)
				Blends.Add(new Map.Tile.EdgeTile(
					(byte) tile4Graphic.SelectedIndex,
					GetVariation(tile4Var),
					(Map.Tile.EdgeTile.Direction) Enum.GetValues(typeof(Map.Tile.EdgeTile.Direction)).GetValue(tile4Dir.SelectedIndex),
					(byte) tile4BlendType.SelectedIndex));

			Hide();
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			Hide();
		}

		private void tile1Graphic_SelectedIndexChanged(object sender, EventArgs e)
		{
			MapView.RepopulateVariations(tile1Var, ThingDb.FloorTiles[((ComboBox)sender).SelectedIndex].Variations.Count);
		}

		private void tile2Graphic_SelectedIndexChanged(object sender, EventArgs e)
		{
			MapView.RepopulateVariations(tile2Var, ThingDb.FloorTiles[((ComboBox)sender).SelectedIndex].Variations.Count);
		}

		private void tile3Graphic_SelectedIndexChanged(object sender, EventArgs e)
		{
			MapView.RepopulateVariations(tile3Var, ThingDb.FloorTiles[((ComboBox)sender).SelectedIndex].Variations.Count);
		}

		private void tile4Graphic_SelectedIndexChanged(object sender, EventArgs e)
		{
			MapView.RepopulateVariations(tile4Var, ThingDb.FloorTiles[((ComboBox)sender).SelectedIndex].Variations.Count);
		}

        private void panelTilePreview_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.Black, new Rectangle(new Point(0, 0), new Size(panelTilePreview.Width, panelTilePreview.Height)));
        }
	}
}
