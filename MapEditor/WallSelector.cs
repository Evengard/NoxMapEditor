using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using NoxShared;

namespace NoxMapEditor
{
	/// <summary>
	/// Summary description for WallSelector.
	/// </summary>
	public class WallSelector : UserControl
	{
        public bool AutoVariW = true;
		private Button button1;
		private Button button2;
		private Button button3;
		private Button button6;
		private Button button7;
		private Button button8;
		private Button button10;
		private Button button11;
		private Button button12;
		private Button button4;
		private Button button5;
		private ComboBox material;
		private Label label1;
		private Label labelWallFacing;
		private ImageList imageList1;
		private Label labelWallMaterial;
		private TextBox minimapGroup;
		private Label labelMinimapGroup;
		private IContainer components;

        private Color tCol;
        public PictureBox picBox;//this must be set by the parent after construction
		public MapView Mapview;//this must be set by the parent after construction
		protected ArrayList wallFacingButtons;
		private Label labelVariation;
		private ComboBox boxVar;
        private CheckBox chkVariW;
		protected Map.Wall.WallFacing facing = Map.Wall.WallFacing.NORTH;

		public WallSelector()
		{
			InitializeComponent();

			wallFacingButtons = new ArrayList(new Button[] {button1, button8, button12, button10, button4, button6, button3, button5, button7, button2, button11});
			material.Items.AddRange(ThingDb.Walls.ToArray());
			material.SelectedIndex = 0;
			minimapGroup.Text = "100";
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WallSelector));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.material = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelWallFacing = new System.Windows.Forms.Label();
            this.labelWallMaterial = new System.Windows.Forms.Label();
            this.minimapGroup = new System.Windows.Forms.TextBox();
            this.labelMinimapGroup = new System.Windows.Forms.Label();
            this.labelVariation = new System.Windows.Forms.Label();
            this.boxVar = new System.Windows.Forms.ComboBox();
            this.chkVariW = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.Click += new System.EventHandler(this.wallFacingButton_Click);
            this.button1.Leave += new System.EventHandler(this.wallFacingButton_Leave);
            this.button1.Enter += new System.EventHandler(this.wallFacingButton_Enter);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.Click += new System.EventHandler(this.wallFacingButton_Click);
            this.button2.Leave += new System.EventHandler(this.wallFacingButton_Leave);
            this.button2.Enter += new System.EventHandler(this.wallFacingButton_Enter);
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.ImageList = this.imageList1;
            this.button3.Name = "button3";
            this.button3.Click += new System.EventHandler(this.wallFacingButton_Click);
            this.button3.Leave += new System.EventHandler(this.wallFacingButton_Leave);
            this.button3.Enter += new System.EventHandler(this.wallFacingButton_Enter);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            // 
            // button6
            // 
            resources.ApplyResources(this.button6, "button6");
            this.button6.ImageList = this.imageList1;
            this.button6.Name = "button6";
            this.button6.Click += new System.EventHandler(this.wallFacingButton_Click);
            this.button6.Leave += new System.EventHandler(this.wallFacingButton_Leave);
            this.button6.Enter += new System.EventHandler(this.wallFacingButton_Enter);
            // 
            // button7
            // 
            resources.ApplyResources(this.button7, "button7");
            this.button7.Name = "button7";
            this.button7.Click += new System.EventHandler(this.wallFacingButton_Click);
            this.button7.Leave += new System.EventHandler(this.wallFacingButton_Leave);
            this.button7.Enter += new System.EventHandler(this.wallFacingButton_Enter);
            // 
            // button8
            // 
            resources.ApplyResources(this.button8, "button8");
            this.button8.Name = "button8";
            this.button8.Click += new System.EventHandler(this.wallFacingButton_Click);
            this.button8.Leave += new System.EventHandler(this.wallFacingButton_Leave);
            this.button8.Enter += new System.EventHandler(this.wallFacingButton_Enter);
            // 
            // button10
            // 
            resources.ApplyResources(this.button10, "button10");
            this.button10.ImageList = this.imageList1;
            this.button10.Name = "button10";
            this.button10.Click += new System.EventHandler(this.wallFacingButton_Click);
            this.button10.Leave += new System.EventHandler(this.wallFacingButton_Leave);
            this.button10.Enter += new System.EventHandler(this.wallFacingButton_Enter);
            // 
            // button11
            // 
            resources.ApplyResources(this.button11, "button11");
            this.button11.Name = "button11";
            this.button11.Click += new System.EventHandler(this.wallFacingButton_Click);
            this.button11.Leave += new System.EventHandler(this.wallFacingButton_Leave);
            this.button11.Enter += new System.EventHandler(this.wallFacingButton_Enter);
            // 
            // button12
            // 
            resources.ApplyResources(this.button12, "button12");
            this.button12.Name = "button12";
            this.button12.Click += new System.EventHandler(this.wallFacingButton_Click);
            this.button12.Leave += new System.EventHandler(this.wallFacingButton_Leave);
            this.button12.Enter += new System.EventHandler(this.wallFacingButton_Enter);
            // 
            // button4
            // 
            resources.ApplyResources(this.button4, "button4");
            this.button4.ImageList = this.imageList1;
            this.button4.Name = "button4";
            this.button4.Click += new System.EventHandler(this.wallFacingButton_Click);
            this.button4.Leave += new System.EventHandler(this.wallFacingButton_Leave);
            this.button4.Enter += new System.EventHandler(this.wallFacingButton_Enter);
            // 
            // button5
            // 
            resources.ApplyResources(this.button5, "button5");
            this.button5.Name = "button5";
            this.button5.Click += new System.EventHandler(this.wallFacingButton_Click);
            this.button5.Leave += new System.EventHandler(this.wallFacingButton_Leave);
            this.button5.Enter += new System.EventHandler(this.wallFacingButton_Enter);
            // 
            // material
            // 
            this.material.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.material.DropDownWidth = 200;
            this.material.FormattingEnabled = true;
            resources.ApplyResources(this.material, "material");
            this.material.Name = "material";
            this.material.Sorted = true;
            this.material.SelectedIndexChanged += new System.EventHandler(this.material_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // labelWallFacing
            // 
            resources.ApplyResources(this.labelWallFacing, "labelWallFacing");
            this.labelWallFacing.Name = "labelWallFacing";
            // 
            // labelWallMaterial
            // 
            resources.ApplyResources(this.labelWallMaterial, "labelWallMaterial");
            this.labelWallMaterial.Name = "labelWallMaterial";
            this.labelWallMaterial.Click += new System.EventHandler(this.labelWallMaterial_Click);
            // 
            // minimapGroup
            // 
            resources.ApplyResources(this.minimapGroup, "minimapGroup");
            this.minimapGroup.Name = "minimapGroup";
            // 
            // labelMinimapGroup
            // 
            resources.ApplyResources(this.labelMinimapGroup, "labelMinimapGroup");
            this.labelMinimapGroup.Name = "labelMinimapGroup";
            // 
            // labelVariation
            // 
            resources.ApplyResources(this.labelVariation, "labelVariation");
            this.labelVariation.Name = "labelVariation";
            // 
            // boxVar
            // 
            this.boxVar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.boxVar.FormattingEnabled = true;
            resources.ApplyResources(this.boxVar, "boxVar");
            this.boxVar.Name = "boxVar";
            // 
            // chkVariW
            // 
            resources.ApplyResources(this.chkVariW, "chkVariW");
            this.chkVariW.Checked = true;
            this.chkVariW.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkVariW.Name = "chkVariW";
            this.chkVariW.UseVisualStyleBackColor = true;
            this.chkVariW.CheckedChanged += new System.EventHandler(this.chkVariW_CheckedChanged);
            // 
            // WallSelector
            // 
            this.Controls.Add(this.chkVariW);
            this.Controls.Add(this.boxVar);
            this.Controls.Add(this.minimapGroup);
            this.Controls.Add(this.labelWallMaterial);
            this.Controls.Add(this.labelWallFacing);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.material);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelMinimapGroup);
            this.Controls.Add(this.labelVariation);
            this.Name = "WallSelector";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        private void wallFacingButton_Click(object sender, EventArgs e)
        {
            facing = (Map.Wall.WallFacing)wallFacingButtons.IndexOf(sender);
            Mapview.CurrentMode = MapView.Mode.MAKE_WALL;
        }
		public Map.Wall NewWall(Point location)
		{
			return new Map.Wall(location, facing, (byte)((ThingDb.Wall)material.SelectedItem).Id, Convert.ToByte(minimapGroup.Text), (byte)Mapview.GetVariation(boxVar));
		}

		private void material_SelectedIndexChanged(object sender, EventArgs e)
		{
			MapView.RepopulateVariations(boxVar, ((ThingDb.Wall) material.SelectedItem).Variations);
         //   if( picBox !=null)
           //     MapView.SetWallBox(picBox,((ThingDb.Wall)material.SelectedItem).Name);
		}

        private void chkVariW_CheckedChanged(object sender, EventArgs e)
        {
            AutoVariW = chkVariW.Checked;
        }

        private void labelWallMaterial_Click(object sender, EventArgs e)
        {

        }
        private void wallFacingButton_Enter(object sender, EventArgs e)
        {
            tCol = ((Button)sender).BackColor;
            ((Button)sender).BackColor = Color.LightPink;
        }
        private void wallFacingButton_Leave(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = tCol;
        }

	}
}
