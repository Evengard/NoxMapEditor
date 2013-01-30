using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using NoxShared;


namespace NoxMapEditor
{
	/// <summary>
	/// Summary description for ObjectEnchantDialog.
	/// </summary>
	public class ObjectEnchantDialog : System.Windows.Forms.Form
	{
		protected Map.Object obj;
		protected byte[] temp1;
		protected byte[] temp2;
		protected string endMod;
		private System.Windows.Forms.ComboBox enchant1;
		private System.Windows.Forms.ComboBox enchant2;
		private System.Windows.Forms.ComboBox enchant3;
		private System.Windows.Forms.ComboBox enchant4;
		private System.Windows.Forms.Button okButton;

        private System.Windows.Forms.Button cancelButton;

        public static ArrayList objEnchants = new ArrayList(new String[]{
																			"WeaponPower1",
																			"WeaponPower2",
																			"WeaponPower3",
																			"WeaponPower4",
																			"WeaponPower5",
																			"WeaponPower6",
																			"ArmorQuality1",
																			"ArmorQuality2",
																			"ArmorQuality3",
																			"ArmorQuality4",
																			"ArmorQuality5",
																			"ArmorQuality6",
																			"Material1",
																			"Material2",
																			"Material3",
																			"Material4",
																			"Material5",
																			"Material6",
																			"Material7",
																			"MaterialTeamRed",
																			"MaterialTeamGreen",
																			"MaterialTeamBlue",
																			"MaterialTeamYellow",
																			"MaterialTeamCyan",
																			"MaterialTeamViolet",
																			"MaterialTeamBlack",
																			"MaterialTeamWhite",
																			"MaterialTeamOrange",
																			"Stun1",
																			"Stun2",
																			"Stun3",
																			"Stun4",
																			"Fire1",
																			"Fire2",
																			"Fire3",
																			"Fire4",
																			"FireRing1",
																			"FireRing2",
																			"FireRing3",
																			"FireRing4",
																			"BlueFireRing1",
																			"BlueFireRing2",
																			"BlueFireRing3",
																			"BlueFireRing4",
																			"Impact1",
																			"Impact2",
																			"Impact3",
																			"Impact4",
																			"Confuse1",
																			"Confuse2",
																			"Confuse3",
																			"Confuse4",
																			"Lightning1",
																			"Lightning2",
																			"Lightning3",
																			"Lightning4",
																			"ManaSteal1",
																			"ManaSteal2",
																			"ManaSteal3",
																			"ManaSteal4",
																			"Vampirism1",
																			"Vampirism2",
																			"Vampirism3",
																			"Vampirism4",
																			"Venom1",
																			"Venom2",
																			"Venom3",
																			"Venom4",
																			"Brilliance1",
																			"FireProtect1",
																			"FireProtect2",
																			"FireProtect3",
																			"FireProtect4",
																			"LightningProtect1",
																			"LightningProtect2",
																			"LightningProtect3",
																			"LightningProtect4",
																			"Regeneration1",
																			"Regeneration2",
																			"Regeneration3",
																			"Regeneration4",
																			"PoisonProtect1",
																			"PoisonProtect2",
																			"PoisonProtect3",
																			"PoisonProtect4",
																			"Speed1",
																			"Speed2",
																			"Speed3",
																			"Speed4",
																			"Readiness1",
																			"Readiness2",
																			"Readiness3",
																			"Readiness4",
																			"ProjectileSpeed1",
																			"ProjectileSpeed2",
																			"ProjectileSpeed3",
																			"ProjectileSpeed4",
																			"Replenishment1",
																			"ContinualReplenishment1",
																			"UserColor1",
																			"UserColor2",
																			"UserColor3",
																			"UserColor4",
																			"UserColor5",
																			"UserColor6",
																			"UserColor7",
																			"UserColor8",
																			"UserColor9",
																			"UserColor10",
																			"UserColor11",
																			"UserColor12",
																			"UserColor13",
																			"UserColor14",
																			"UserColor15",
																			"UserColor16",
																			"UserColor17",
																			"UserColor18",
																			"UserColor19",
																			"UserColor20",
																			"UserColor21",
																			"UserColor22",
																			"UserColor23",
																			"UserColor24",
																			"UserColor25",
																			"UserColor26",
																			"UserColor27",
																			"UserColor28",
																			"UserColor29",
																			"UserColor30",
																			"UserColor31",
																			"UserColor32",
																			"UserColor33",
																			"UserMaterialColor1",
																			"UserMaterialColor2",
																			"UserMaterialColor3",
																			"UserMaterialColor4",
																			"UserMaterialColor5",
																			"UserMaterialColor6",
																			"UserMaterialColor7",
																			"UserMaterialColor8",
																			"UserMaterialColor9",
																			"UserMaterialColor10",
																			"UserMaterialColor11",
																			"UserMaterialColor12",
																			"UserMaterialColor13",
																			"UserMaterialColor14",
																			"UserMaterialColor15",
																			"UserMaterialColor16",
																			"UserMaterialColor17",
																			"UserMaterialColor18",
																			"UserMaterialColor19",
																			"UserMaterialColor20",
																			"UserMaterialColor21",
																			"UserMaterialColor22",
																			"UserMaterialColor23",
																			"UserMaterialColor24",
																			"UserMaterialColor25",
																			"UserMaterialColor26",
																			"UserMaterialColor27",
																			"UserMaterialColor28",
																			"UserMaterialColor29",
																			"UserMaterialColor30",
																			"UserMaterialColor31",
																			"UserMaterialColor32"
																		});
	
		public string ReadString(int bytes, System.IO.BinaryReader rdr)
		{
			char[] buffer;
			if (bytes == 0 ||bytes >rdr.BaseStream.Length-rdr.BaseStream.Position)
				return null;
			try 
			{			
				buffer = rdr.ReadChars(bytes);
			}
			catch(System.IO.EndOfStreamException)
			{
				return null;
			}
			string str = new string(buffer);

			if (str.IndexOf('\0') >= 0)
				str = str.Substring(0, str.IndexOf('\0'));

			return str;
		}

		public Map.Object Object
		{
			get
			{
				return obj;
			}
			set
			{
				temp1 = new Byte[] {0, 0, 0, 1, 0};
				temp2 = new Byte[] {00, 00, 00,00, 00, 00, 00, 01, 00, 00,00,00,00,00,00,00,00};
				obj = value;
				endMod = "";
				if (obj.modbuf.Length >= 4)
				{
					System.IO.BinaryReader rdr = new System.IO.BinaryReader(new System.IO.MemoryStream(obj.modbuf));
					string mod = null;
					long pos;
					byte fByte;
					char[] buffer;
					obj.enchants = new ArrayList();
					if(((ThingDb.Thing)ThingDb.Things[obj.Name]).Init == "ModifierInit")
					{
						pos = rdr.BaseStream.Position;
						int i = 0;
						while(i < 4)
						{
							fByte = rdr.ReadByte();
						
							buffer = rdr.ReadChars(fByte);
							mod = new string(buffer);
							obj.enchants.Add(mod);
							i++;
						}
						while (rdr.BaseStream.Position < rdr.BaseStream.Length)
						{
							endMod += String.Format("{0:x2} ", rdr.ReadByte());
						}
						enchant1.Text = (string)obj.enchants[0];
						enchant2.Text = (string)obj.enchants[1];
						enchant3.Text = (string)obj.enchants[2];
						enchant4.Text = (string)obj.enchants[3];
					}
					else
					{
						System.Windows.Forms.MessageBox.Show("This object does not accept enchants.","Not compatible");
					}
				}
			}
		}

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ObjectEnchantDialog()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			enchant1.Items.AddRange(objEnchants.ToArray());
			enchant2.Items.AddRange(objEnchants.ToArray());
			enchant3.Items.AddRange(objEnchants.ToArray());
			enchant4.Items.AddRange(objEnchants.ToArray());
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjectEnchantDialog));
            this.enchant1 = new System.Windows.Forms.ComboBox();
            this.enchant2 = new System.Windows.Forms.ComboBox();
            this.enchant3 = new System.Windows.Forms.ComboBox();
            this.enchant4 = new System.Windows.Forms.ComboBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
// 
// enchant1
// 
            this.enchant1.FormattingEnabled = true;
            resources.ApplyResources(this.enchant1, "enchant1");
            this.enchant1.Name = "enchant1";
            this.enchant1.Sorted = true;
// 
// enchant2
// 
            this.enchant2.FormattingEnabled = true;
            resources.ApplyResources(this.enchant2, "enchant2");
            this.enchant2.Name = "enchant2";
            this.enchant2.Sorted = true;
// 
// enchant3
// 
            this.enchant3.FormattingEnabled = true;
            resources.ApplyResources(this.enchant3, "enchant3");
            this.enchant3.Name = "enchant3";
            this.enchant3.Sorted = true;
// 
// enchant4
// 
            this.enchant4.FormattingEnabled = true;
            resources.ApplyResources(this.enchant4, "enchant4");
            this.enchant4.Name = "enchant4";
            this.enchant4.Sorted = true;
// 
// okButton
// 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
// 
// cancelButton
// 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
// 
// ObjectEnchantDialog
// 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.cancelButton;
            this.ControlBox = false;
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.enchant4);
            this.Controls.Add(this.enchant3);
            this.Controls.Add(this.enchant2);
            this.Controls.Add(this.enchant1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ObjectEnchantDialog";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

        }
		#endregion

		private void okButton_Click(object sender, System.EventArgs e)
		{
			System.IO.MemoryStream stream = new System.IO.MemoryStream();
			System.IO.BinaryWriter wtr = new System.IO.BinaryWriter(stream);
			if (objEnchants.Contains(enchant1.Text))
			{
				wtr.Write((byte)enchant1.Text.Length);
				wtr.Write(enchant1.Text.ToCharArray());
				//wtr.Write('\0');
			}
			else
				wtr.Write('\0');
			
			if (objEnchants.Contains(enchant2.Text))
			{
				wtr.Write((byte)(enchant2.Text.Length));
				wtr.Write(enchant2.Text.ToCharArray());
				//wtr.Write('\0');
			}
			else
				wtr.Write('\0');
			if (objEnchants.Contains(enchant3.Text))
			{
				wtr.Write((byte)(enchant3.Text.Length));
				wtr.Write(enchant3.Text.ToCharArray());
				//wtr.Write('\0');
			}
			else
				wtr.Write('\0');
			if (objEnchants.Contains(enchant4.Text))
			{
				wtr.Write((byte)(enchant4.Text.Length));
				wtr.Write(enchant4.Text.ToCharArray());
				//wtr.Write('\0');
			}
			else
				wtr.Write('\0');

			Regex bytes = new Regex("[0-9|a-f|A-F]{2}");
			foreach (Match match in bytes.Matches(endMod))
				wtr.Write(Convert.ToByte(match.Value, 16));
			obj.modbuf = stream.ToArray();
			if(obj.modbuf.Length == 0)
				obj.modbuf = new byte[] {0x00, 0x00};
			Close();
		}

		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			Close();
		}
	}
}
