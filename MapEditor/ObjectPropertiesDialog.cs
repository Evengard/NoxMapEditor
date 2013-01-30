using System;
using System.Reflection;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using MapEditor;
using System.Diagnostics;

using NoxShared;

namespace NoxMapEditor
{
	public class ObjectPropertiesDialog : System.Windows.Forms.Form
	{
		protected Map.Object obj;
        string m_ExePath;
		public Map.Object Object
		{
			get
			{
				return obj;
			}
			set
			{
				obj = value;
				nameBox.Text = obj.Name;
				xBox.Text = obj.Location.X.ToString();
				yBox.Text = obj.Location.Y.ToString();
				propertiesBox.SelectedItem = obj.Properties;
				extentBox.Text = obj.Extent.ToString();
				teamBox.Text = obj.Team.ToString();
				scrNameBox.Text = obj.Scr_Name;
                xtraBox.Checked = (obj.Terminator > 0);
                if (!xtraBox.Checked)
                {
                    scrNameBox.Enabled = false;
                    pickupBox.Enabled = false;
                    teamBox.Enabled = false;
                    invenButton.Enabled = false;
                    autoEquipCheck.Enabled = false;
                }
				autoEquipCheck.Checked = obj.AutoEquip;
				t2Box.Text = String.Format("{0:x}", obj.Type2);

                pickupBox.Text = obj.pickup_func;

                //print out the bytes in hex
				boxMod.Clear();
				foreach (byte b in obj.modbuf)
					boxMod.Text += String.Format("{0:x2} ", b);

				if(((ThingDb.Thing)ThingDb.Things[obj.Name]).Init=="ModifierInit")
					enchantButton.Enabled = true;
				else if((((ThingDb.Thing)ThingDb.Things[obj.Name]).Class & ThingDb.Thing.ClassFlags.DOOR)==ThingDb.Thing.ClassFlags.DOOR)
					lockButton.Enabled = true;
            }
		}
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox xBox;
		private System.Windows.Forms.TextBox yBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox extentBox;
		private System.Windows.Forms.ComboBox nameBox;
		private System.Windows.Forms.Button enchantButton;
		private System.Windows.Forms.Button lockButton;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox teamBox;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox scrNameBox;
		private System.Windows.Forms.CheckBox xtraBox;
		private System.Windows.Forms.Button invenButton;
		private System.Windows.Forms.ComboBox propertiesBox;
		private System.Windows.Forms.Label label7;
        private TextBox pickupBox;
        private Label label8;
		private CheckBox autoEquipCheck;
		private TextBox t2Box;
		private Label label9;
        private Button button1;
        private CheckBox chkmod;

        private System.Windows.Forms.TextBox boxMod;

		public ObjectPropertiesDialog()
		{
			InitializeComponent();
            m_ExePath = Process.GetCurrentProcess().MainModule.FileName;
            m_ExePath = Path.GetDirectoryName(m_ExePath);
			foreach (string s in ThingDb.Things.Keys)
				nameBox.Items.Add(s);
			foreach (Map.Object.Property prop in Enum.GetValues(typeof(Map.Object.Property)))
				propertiesBox.Items.Add(prop);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjectPropertiesDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.xBox = new System.Windows.Forms.TextBox();
            this.yBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.extentBox = new System.Windows.Forms.TextBox();
            this.nameBox = new System.Windows.Forms.ComboBox();
            this.boxMod = new System.Windows.Forms.TextBox();
            this.enchantButton = new System.Windows.Forms.Button();
            this.lockButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.teamBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.scrNameBox = new System.Windows.Forms.TextBox();
            this.xtraBox = new System.Windows.Forms.CheckBox();
            this.invenButton = new System.Windows.Forms.Button();
            this.propertiesBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.pickupBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.autoEquipCheck = new System.Windows.Forms.CheckBox();
            this.t2Box = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.chkmod = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
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
            // xBox
            // 
            resources.ApplyResources(this.xBox, "xBox");
            this.xBox.Name = "xBox";
            // 
            // yBox
            // 
            resources.ApplyResources(this.yBox, "yBox");
            this.yBox.Name = "yBox";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // extentBox
            // 
            resources.ApplyResources(this.extentBox, "extentBox");
            this.extentBox.Name = "extentBox";
            // 
            // nameBox
            // 
            this.nameBox.DropDownWidth = 200;
            this.nameBox.FormattingEnabled = true;
            resources.ApplyResources(this.nameBox, "nameBox");
            this.nameBox.Name = "nameBox";
            this.nameBox.SelectedIndexChanged += new System.EventHandler(this.nameBox_SelectedIndexChanged);
            // 
            // boxMod
            // 
            resources.ApplyResources(this.boxMod, "boxMod");
            this.boxMod.Name = "boxMod";
            // 
            // enchantButton
            // 
            resources.ApplyResources(this.enchantButton, "enchantButton");
            this.enchantButton.Name = "enchantButton";
            this.enchantButton.Click += new System.EventHandler(this.enchantButton_Click);
            // 
            // lockButton
            // 
            resources.ApplyResources(this.lockButton, "lockButton");
            this.lockButton.Name = "lockButton";
            this.lockButton.Click += new System.EventHandler(this.lockButton_Click);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // teamBox
            // 
            resources.ApplyResources(this.teamBox, "teamBox");
            this.teamBox.Name = "teamBox";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // scrNameBox
            // 
            resources.ApplyResources(this.scrNameBox, "scrNameBox");
            this.scrNameBox.Name = "scrNameBox";
            // 
            // xtraBox
            // 
            resources.ApplyResources(this.xtraBox, "xtraBox");
            this.xtraBox.Name = "xtraBox";
            this.xtraBox.CheckedChanged += new System.EventHandler(this.xtraBox_CheckedChanged);
            // 
            // invenButton
            // 
            resources.ApplyResources(this.invenButton, "invenButton");
            this.invenButton.Name = "invenButton";
            this.invenButton.Click += new System.EventHandler(this.invenButton_Click);
            // 
            // propertiesBox
            // 
            this.propertiesBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.propertiesBox.FormattingEnabled = true;
            resources.ApplyResources(this.propertiesBox, "propertiesBox");
            this.propertiesBox.Name = "propertiesBox";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // pickupBox
            // 
            resources.ApplyResources(this.pickupBox, "pickupBox");
            this.pickupBox.Name = "pickupBox";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // autoEquipCheck
            // 
            resources.ApplyResources(this.autoEquipCheck, "autoEquipCheck");
            this.autoEquipCheck.Name = "autoEquipCheck";
            // 
            // t2Box
            // 
            resources.ApplyResources(this.t2Box, "t2Box");
            this.t2Box.Name = "t2Box";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkmod
            // 
            resources.ApplyResources(this.chkmod, "chkmod");
            this.chkmod.Name = "chkmod";
            this.chkmod.UseVisualStyleBackColor = true;
            this.chkmod.CheckedChanged += new System.EventHandler(this.chkmod_CheckedChanged);
            // 
            // ObjectPropertiesDialog
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.chkmod);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.t2Box);
            this.Controls.Add(this.autoEquipCheck);
            this.Controls.Add(this.pickupBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.propertiesBox);
            this.Controls.Add(this.scrNameBox);
            this.Controls.Add(this.teamBox);
            this.Controls.Add(this.boxMod);
            this.Controls.Add(this.extentBox);
            this.Controls.Add(this.yBox);
            this.Controls.Add(this.xBox);
            this.Controls.Add(this.invenButton);
            this.Controls.Add(this.xtraBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lockButton);
            this.Controls.Add(this.enchantButton);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ObjectPropertiesDialog";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }
		#endregion

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			//verify that we have valid input
			if (ThingDb.GetThing(nameBox.Text) == null)
			{
				MessageBox.Show("Invalid object name.", "Error");
				return;
			}
			//commit the changes

            obj.Name = nameBox.Text;

            obj.Location.X = Single.Parse(xBox.Text);
			obj.Location.Y = Single.Parse(yBox.Text);
            
            //REMOVEOBJECT,ADDOBJECT
            if( propertiesBox.SelectedItem != null)
			obj.Properties = (Map.Object.Property) propertiesBox.SelectedItem;
            //MessageBox.Show("got here");
			obj.Extent = Int32.Parse(extentBox.Text);
			obj.Terminator = (byte)(xtraBox.Checked==true? 0xFF : 0x00);
			obj.Team = Byte.Parse(teamBox.Text);
			obj.Scr_Name = scrNameBox.Text;
            obj.pickup_func = pickupBox.Text;
			obj.AutoEquip = autoEquipCheck.Checked;
			obj.Type2 = Int16.Parse(t2Box.Text, System.Globalization.NumberStyles.HexNumber);
			//get the contents of the box and parse it to turn it into a byte[] and use it as the modbuf
            
            if (boxMod.Text.Length > 0)
			{
				MemoryStream stream = new MemoryStream();
				BinaryWriter wtr = new BinaryWriter(stream);
				Regex bytes = new Regex("[0-9|a-f|A-F]{2}");
				foreach (Match match in bytes.Matches(boxMod.Text))
					wtr.Write(Convert.ToByte(match.Value, 16));
				obj.modbuf = stream.ToArray();
			}
			else
				obj.modbuf = new byte[0];
			this.Visible = false;
		}

		private void enchantButton_Click(object sender, System.EventArgs e)
		{
			ObjectEnchantDialog enchantDlg = new ObjectEnchantDialog();
			enchantDlg.Object = obj;
			if (enchantDlg.ShowDialog() == DialogResult.OK)
			{
				boxMod.Clear();
				foreach (byte b in obj.modbuf)
					boxMod.Text += String.Format("{0:x2} ", b);
			}
		}

		private void lockButton_Click(object sender, System.EventArgs e)
		{
			DoorProperties doorDlg = new DoorProperties();
			doorDlg.Object = obj;
			doorDlg.ShowDialog();
			boxMod.Clear();
			foreach (byte b in obj.modbuf)
				boxMod.Text += String.Format("{0:x2} ", b);
		}

		private void nameBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(((ThingDb.Thing)ThingDb.Things[nameBox.Text]).Init=="ModifierInit")
				enchantButton.Enabled = true;
			else if((((ThingDb.Thing)ThingDb.Things[nameBox.Text]).Class & ThingDb.Thing.ClassFlags.DOOR)==ThingDb.Thing.ClassFlags.DOOR)
				lockButton.Enabled = true;
            if (nameBox.Text != obj.Name)
            {
                obj.Name = nameBox.Text;
                boxMod.Clear();
                string mod = "";
                if (File.Exists(m_ExePath + "\\scripts\\objects\\defaultmods\\" + obj.Name + ".txt"))
                {
                    StreamReader SR;
                    SR = File.OpenText(m_ExePath + "\\scripts\\objects\\defaultmods\\" + obj.Name + ".txt");
                    if (SR != null)
                    {
                        obj.Properties = (Map.Object.Property)Convert.ToInt16(SR.ReadLine());
                        obj.Terminator = Convert.ToByte(SR.ReadLine());
                        while (!SR.EndOfStream)
                        {
                            mod += SR.ReadLine() + " ";
                        }
                    }
                    SR.Close();
                }
                boxMod.Text = mod;

                if (boxMod.Text.Length > 0)
                {
                    MemoryStream stream = new MemoryStream();
                    BinaryWriter wtr = new BinaryWriter(stream);
                    Regex bytes = new Regex("[0-9|a-f|A-F]{2}");
                    foreach (Match match in bytes.Matches(boxMod.Text))
                        wtr.Write(Convert.ToByte(match.Value, 16));
                    obj.modbuf = stream.ToArray();
                }
                else
                    obj.modbuf = new byte[0];
            }
            propertiesBox.SelectedItem = obj.Properties;
            xtraBox.Checked = (obj.Terminator > 0);
		}

		private void xtraBox_CheckedChanged(object sender, System.EventArgs e)
		{
			if(xtraBox.Checked)
			{
				scrNameBox.Enabled = true;
				teamBox.Enabled = true;
				invenButton.Enabled = true;
                pickupBox.Enabled = true;
				autoEquipCheck.Enabled = true;
            }
			else
			{
				scrNameBox.Enabled = false;
                pickupBox.Enabled = false;
                teamBox.Enabled = false;
				invenButton.Enabled = false;
				autoEquipCheck.Enabled = false;
			}	
		}

		private void invenButton_Click(object sender, System.EventArgs e)
		{
			ObjectInventoryDialog invenDlg = new ObjectInventoryDialog();
			invenDlg.Object = obj;
			invenDlg.ShowDialog();
		}
        private void button1_Click(object sender, EventArgs e)
        {
            ScriptFunctions me = new ScriptFunctions();
            if (me.CheckMod(m_ExePath, obj.Name))
            {
                ModEditor me2 = new ModEditor(obj, me, m_ExePath);
                me2.ShowDialog();
                obj = me2.GetObj();
                boxMod.Clear();
                foreach (byte b in obj.modbuf)
                    boxMod.Text += String.Format("{0:x2} ", b);
            }
            else
            {
                MessageBox.Show("There is no mod editor for that object.");
            }
        }
        private void chkmod_CheckedChanged(object sender, EventArgs e)
        {
            boxMod.Enabled = chkmod.Checked;
        }
	}
}
