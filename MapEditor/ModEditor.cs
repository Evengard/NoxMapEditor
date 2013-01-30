using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NoxShared;
using System.IO;
using System.Diagnostics;
using ModDatabase;
using System.Text.RegularExpressions;

//
// Have to make it load and parse auto
//
//
//
//
//



namespace MapEditor
{
    public partial class ModEditor : Form
    {
        TreeNode ROOTNODE = new TreeNode();
        TreeNode STRINGS = new TreeNode();
        TreeNode PROPS = new TreeNode();
        TreeNode COLORS = new TreeNode();
        TreeNode ENCHANTS = new TreeNode();
        TreeNode MISC = new TreeNode();
        TreeNode LOOPS = new TreeNode();
        TreeNode DIRECTION = new TreeNode();
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
        ScriptFunctions Scripting;
        Map.Object obj;
        ArrayList modlayout;
        List<ScriptType> modlist = new List<ScriptType>();
        string lastval;
        string m_ExePath;
        bool loadfailed = false;

        public ModEditor()
        {
            InitializeComponent();
   
        }
        public void ClearNodes()
        {
            treeView1.Nodes.Clear();
            DIRECTION.Name = "ROOT";
            DIRECTION.Text = "Direction";
            DIRECTION.Nodes.Clear();
            STRINGS.Name = "ROOT";
            STRINGS.Text = "Strings";
            STRINGS.Nodes.Clear();
            PROPS.Name = "ROOT";
            PROPS.Text = "Properties";
            PROPS.Nodes.Clear();
            COLORS.Name = "ROOT";
            COLORS.Text = "Colors";
            COLORS.Nodes.Clear();
            MISC.Name = "ROOT";
            MISC.Text = "Misc";
            MISC.Nodes.Clear();
            ENCHANTS.Name = "ROOT";
            ENCHANTS.Text = "Enchantments";
            ENCHANTS.Nodes.Clear();
            LOOPS.Name = "ROOT";
            LOOPS.Text = "Loop/Inventory";
            LOOPS.Nodes.Clear();

            treeView1.Nodes.Add(DIRECTION);
            treeView1.Nodes.Add(ENCHANTS);
            treeView1.Nodes.Add(PROPS);
            treeView1.Nodes.Add(STRINGS);
            treeView1.Nodes.Add(COLORS);
            treeView1.Nodes.Add(LOOPS);
            treeView1.Nodes.Add(MISC);
        }
        public ModEditor(Map.Object objectMod,ScriptFunctions scrfuncs, string path)
        {
            InitializeComponent();
            ClearNodes();
            obj = objectMod;
            Scripting = scrfuncs;
            lblname.Text = obj.Name;
            lblextent.Text = obj.Extent.ToString();
            m_ExePath = path;
            HandleMod();
        }
        public Map.Object GetObj()
        {
            return(obj);
        }
        private void HandleMod()
        {
            if (Scripting.CheckMod(m_ExePath, obj.Name))
            { }
            else
                return;

            modlayout = Scripting.EditMod(m_ExePath, obj.Name);

            try
            {
                int count = modlayout.Count;
                string[] mod = (string[])modlayout.ToArray(typeof(string));
               // listMod.Items.Clear();
                modlayout.Clear();

                System.IO.MemoryStream rS = new System.IO.MemoryStream(obj.modbuf);
                System.IO.BinaryReader rR = new System.IO.BinaryReader(rS);
                bool isLoop = false;
                int loopLen = 0;
                ArrayList loopList = new ArrayList();
                string loopName = "";

                for (int i = 0; i < count && mod[i] != null; )
                {
                    ScriptType loopItem = new ScriptType();


                    if (mod[i + 1] == "LOOP")
                    {
                        loopName = mod[i];
                        loopLen = rR.ReadByte();
                        loopLen--;
                        isLoop = true;
                        loopItem.len = loopLen + 1;
                        loopItem.scrID = ScriptType.ScriptID.LOOP;
                        loopItem.loopval = new List<ScriptType>();

                        i += 2;
                        string str = mod[i];
                        while (str != "END")
                        {
                            loopList.Add(str);
                            i++;
                            str = mod[i];
                        }
                        i++;
                    }

                    int max = (loopLen * (loopList.Count));
                    //if (isLoop)
                    //    max++;

                    for (int j = 0; j <= max; j++)
                    {
                        TreeNode nodey = new TreeNode();
                        ScriptType item = new ScriptType();
                        string switchVal;
                        if (isLoop)
                            switchVal = loopList[(j % loopList.Count)].ToString();
                        else
                            switchVal = mod[i + 1];

                        switch (switchVal)
                        {
                            case "EXTENT":
                                if (rR.BaseStream.Position <= obj.modbuf.Length - 4)
                                {
                                    item.intval = rR.ReadInt32();
                                    item.scrID = ScriptType.ScriptID.INT;
                                    if (isLoop)
                                        loopItem.loopval.Add(item);
                                    else
                                    {
                                        modlist.Add(item);
                                       // listMod.Items.Add(mod[i + 1] + " : " + mod[i]);
                                        nodey.Text = mod[i + 1] + " : " + mod[i];
                                        nodey.Tag = modlist.Count-1;
                                        MISC.Nodes.Add(nodey);
                                        //treeView1.Nodes.Add(nodey);
                                    }
                                }
                                break;
                            case "BYTE":
                                if (rR.BaseStream.Position <= obj.modbuf.Length)
                                {
                                    item.byteval = rR.ReadByte();
                                    item.scrID = ScriptType.ScriptID.BYTE;
                                    if (isLoop)
                                        loopItem.loopval.Add(item);
                                    else
                                    {
                                        modlist.Add(item);
                                        //listMod.Items.Add(mod[i + 1] + " : " + mod[i]);
                                        nodey.Text = mod[i + 1] + " : " + mod[i];
                                        nodey.Tag = modlist.Count-1;
                                        //treeView1.Nodes.Add(nodey);
                                        MISC.Nodes.Add(nodey);
                                    }
                                }
                                break;

                            case "SHORT":
                                if (rR.BaseStream.Position <= obj.modbuf.Length - 2)
                                {
                                    item.shortval = rR.ReadInt16();
                                    item.scrID = ScriptType.ScriptID.SHORT;
                                    if (isLoop)
                                        loopItem.loopval.Add(item);
                                    else
                                    {
                                        modlist.Add(item);
                                        //listMod.Items.Add(mod[i + 1] + " : " + mod[i]);
                                        nodey.Text = mod[i + 1] + " : " + mod[i];
                                        nodey.Tag = modlist.Count-1;
                                        //treeView1.Nodes.Add(nodey);
                                        MISC.Nodes.Add(nodey);
                                    }
                                }
                                break;

                            case "INT":
                                if (rR.BaseStream.Position <= obj.modbuf.Length - 4)
                                {
                                    item.intval = rR.ReadInt32();
                                    item.scrID = ScriptType.ScriptID.INT;
                                    if (isLoop)
                                        loopItem.loopval.Add(item);
                                    else
                                    {
                                        modlist.Add(item);
                                        //listMod.Items.Add(mod[i + 1] + " : " + mod[i]);
                                        nodey.Text = mod[i + 1] + " : " + mod[i];
                                        nodey.Tag = modlist.Count-1;
                                       // treeView1.Nodes.Add(nodey);
                                        MISC.Nodes.Add(nodey);
                                    }
                                }
                                break;

                            case "STRING":

                                if (rR.BaseStream.Position <= obj.modbuf.Length - 4)
                                {
                                    item.len = rR.ReadInt32();
                                    item.scrID = ScriptType.ScriptID.STRING;
                                    if (rR.BaseStream.Position <= obj.modbuf.Length - item.len)
                                    {
                                        item.val = new byte[item.len];
                                        item.val = rR.ReadBytes(item.len);
                                        if (isLoop)
                                            loopItem.loopval.Add(item);
                                        else
                                        {
                                            modlist.Add(item);
                                            //listMod.Items.Add(mod[i + 1] + " : " + mod[i] + " : " + item.len);
                                            nodey.Text = mod[i] + " : " + item.len;
                                            nodey.Tag = modlist.Count-1;
                                            //treeView1.Nodes.Add(nodey);
                                            STRINGS.Nodes.Add(nodey);
                                        }
                                    }
                                }
                                break;

                            case "BOOL":
                                if (rR.BaseStream.Position <= obj.modbuf.Length)
                                {
                                    item.scrID = ScriptType.ScriptID.BOOL;
                                    item.boolval = rR.ReadBoolean();
                                    if (isLoop)
                                        loopItem.loopval.Add(item);
                                    else
                                    {
                                        modlist.Add(item);
                                        //listMod.Items.Add(mod[i + 1] + " : " + mod[i]);
                                        nodey.Text = mod[i + 1] + " : " + mod[i];
                                        nodey.Tag = modlist.Count-1;
                                        //treeView1.Nodes.Add(nodey);
                                        MISC.Nodes.Add(nodey);
                                    }
                                }
                                break;

                            case "FLOAT":
                                if (rR.BaseStream.Position <= obj.modbuf.Length - 4)
                                {
                                    item.floatval = (float)rR.ReadUInt32();
                                    item.scrID = ScriptType.ScriptID.FLOAT;
                                    if (isLoop)
                                        loopItem.loopval.Add(item);
                                    else
                                    {
                                        modlist.Add(item);
                                       // listMod.Items.Add(mod[i + 1] + " : " + mod[i]);
                                        nodey.Text = mod[i + 1] + " : " + mod[i];
                                        nodey.Tag = modlist.Count-1;
                                        //treeView1.Nodes.Add(nodey);
                                        MISC.Nodes.Add(nodey);
                                    }
                                }
                                break;

                            case "VOID":
                                if (rR.BaseStream.Position <= obj.modbuf.Length - Convert.ToInt32(mod[i + 2]))
                                {
                                    item.len = Convert.ToInt32(mod[i + 2]);
                                    item.scrID = ScriptType.ScriptID.VOID;
                                    item.val = new byte[item.len];
                                    item.val = rR.ReadBytes(item.len);
                                    if (isLoop)
                                        loopItem.loopval.Add(item);
                                    else
                                    {
                                        modlist.Add(item);
                                       // listMod.Items.Add(mod[i + 1] + " : " + mod[i]);
                                        nodey.Text = mod[i + 1] + " : " + mod[i];
                                        nodey.Tag = modlist.Count-1;
                                        treeView1.Nodes.Add(nodey);
                                        MISC.Nodes.Add(nodey);
                                    }
                                }
                                break;

                            case "HIDDEN":
                                if (rR.BaseStream.Position <= obj.modbuf.Length - Convert.ToInt32(mod[i + 2]))
                                {
                                    item.len = Convert.ToInt32(mod[i + 2]);
                                    item.scrID = ScriptType.ScriptID.HIDDEN;
                                    item.val = new byte[item.len];
                                    item.val = rR.ReadBytes(item.len);

                                    if (isLoop)
                                        loopItem.loopval.Add(item);
                                    else
                                    {
                                        modlist.Add(item);
                                        //listMod.Items.Add(" ");
                                    }
                                }
                                break;
                            case "DIRECTION":

                                if (rR.BaseStream.Position <= obj.modbuf.Length - 8)
                                {
                                        item.scrID = ScriptType.ScriptID.DIRECTION;
                                        ulong val = rR.ReadUInt64();
                                        item.dirval = (ScriptType.Direction)val;
                                        if (isLoop)
                                            loopItem.loopval.Add(item);
                                        else
                                        {
                                            modlist.Add(item);
                                            //listMod.Items.Add(mod[i + 1] + " : " + mod[i] + " : " + item.len);
                                            nodey.Text = mod[i];
                                            nodey.Tag = modlist.Count-1;
                                            //treeView1.Nodes.Add(nodey);
                                            DIRECTION.Nodes.Add(nodey);
                                        }
                                }
                                break;
                            case "PROP":

                                if (rR.BaseStream.Position <= obj.modbuf.Length)
                                {
                                    item.len = (int)rR.ReadByte();//rR.ReadInt32();
                                    item.scrID = ScriptType.ScriptID.PROP;
                                    if (rR.BaseStream.Position <= obj.modbuf.Length - item.len)
                                    {
                                        item.val = new byte[item.len];
                                        item.val = rR.ReadBytes(item.len);
                                        if (isLoop)
                                            loopItem.loopval.Add(item);
                                        else
                                        {
                                            modlist.Add(item);
                                           // listMod.Items.Add(mod[i + 1] + " : " + mod[i] + " : " + item.len);
                                            nodey.Text = mod[i] + " : " + item.len;
                                            nodey.Tag = modlist.Count-1;
                                            //treeView1.Nodes.Add(nodey);
                                            PROPS.Nodes.Add(nodey);
                                        }
                                    }
                                }
                                break;
                            case "LOADREST":
                                if (rR.BaseStream.Position <= obj.modbuf.Length)
                                {
                                    if (rR.BaseStream.Position == obj.modbuf.Length)
                                        item.len = 1;
                                    else
                                        item.len = (int)(obj.modbuf.Length - rR.BaseStream.Position);
                                    item.scrID = ScriptType.ScriptID.HIDDEN;
                                    item.val = new byte[item.len];
                                    item.val = rR.ReadBytes(item.len);

                                    if (isLoop)
                                        loopItem.loopval.Add(item);
                                    else
                                    {
                                        modlist.Add(item);
                                    }
                                }
                                break;
                            case "SPELL":

                                if (rR.BaseStream.Position <= obj.modbuf.Length)
                                {
                                    item.len = (int)rR.ReadByte();//rR.ReadInt32();
                                    item.scrID = ScriptType.ScriptID.SPELL;
                                    if (rR.BaseStream.Position <= obj.modbuf.Length - item.len)
                                    {
                                        item.val = new byte[item.len];
                                        item.val = rR.ReadBytes(item.len);
                                        if (isLoop)
                                            loopItem.loopval.Add(item);
                                        else
                                        {
                                            modlist.Add(item);
                                            //listMod.Items.Add(mod[i + 1] + " : " + mod[i] + " : " + item.len);
                                            nodey.Text = mod[i + 1] + " : " + mod[i] + " : " + item.len;
                                            nodey.Tag = modlist.Count-1;
                                            //treeView1.Nodes.Add(nodey);
                                            PROPS.Nodes.Add(nodey);
                                        }
                                    }
                                }
                                break;
                            case "ENCHANTMENT":

                                if (rR.BaseStream.Position <= obj.modbuf.Length)
                                {
                                    item.len = (int)rR.ReadByte();//rR.ReadInt32();
                                    item.scrID = ScriptType.ScriptID.ENCHANTMENT;
                                    if (rR.BaseStream.Position <= obj.modbuf.Length - item.len)
                                    {
                                        item.val = new byte[item.len];
                                        item.val = rR.ReadBytes(item.len);
                                        if (isLoop)
                                            loopItem.loopval.Add(item);
                                        else
                                        {
                                            modlist.Add(item);
                                            //listMod.Items.Add(mod[i + 1] + " : " + mod[i] + " : " + item.len);
                                            nodey.Text = mod[i + 1] + " : " + mod[i] + " : " + item.len;
                                            nodey.Tag = modlist.Count-1;
                                            //treeView1.Nodes.Add(nodey);
                                            PROPS.Nodes.Add(nodey);
                                        }
                                    }
                                }
                                break;
                            case "OBJECT":

                                if (rR.BaseStream.Position <= obj.modbuf.Length)
                                {
                                    item.byteval = rR.ReadByte();
                                    item.len = (int)rR.ReadByte();//rR.ReadInt32();
                                    item.scrID = ScriptType.ScriptID.OBJECT;
                                    if (rR.BaseStream.Position <= obj.modbuf.Length - item.len)
                                    {
                                        item.val = new byte[item.len];
                                        item.val = rR.ReadBytes(item.len);

                                        item.len2 = rR.ReadByte();
                                        item.val2 = rR.ReadBytes(item.len2);
                                        item.loopval = new List<ScriptType>();
                                        for (int p = 0; p < 4; p++)
                                        {
                                            if (rR.BaseStream.Position <= obj.modbuf.Length)
                                            {
                                                ScriptType me = new ScriptType();
                                                me.scrID = ScriptType.ScriptID.ENCHANTMENT;
                                                me.len = (int)rR.ReadByte();
                                                me.val = new byte[me.len];
                                                if (me.len > 0)
                                                {
                                                    me.val = rR.ReadBytes(me.len);
                                                }
                                                item.loopval.Add(me);
                                            }
                                        }
                                        if (isLoop)
                                            loopItem.loopval.Add(item);
                                        else
                                        {
                                            modlist.Add(item);
                                           // listMod.Items.Add(mod[i + 1] + " : " + mod[i] + " : " + item.len);
                                            nodey.Text = mod[i + 1] + " : " + mod[i] + " : " + item.len;
                                            nodey.Tag = modlist.Count-1;
                                            //treeView1.Nodes.Add(nodey);
                                            MISC.Nodes.Add(nodey);
                                        }
                                    }
                                }
                                break;
                            case "ABILITY":

                                if (rR.BaseStream.Position <= obj.modbuf.Length)
                                {
                                    item.len = (int)rR.ReadByte();//rR.ReadInt32();
                                    item.scrID = ScriptType.ScriptID.ABILITY;
                                    if (rR.BaseStream.Position <= obj.modbuf.Length - item.len)
                                    {
                                        item.val = new byte[item.len];
                                        item.val = rR.ReadBytes(item.len);
                                        if (isLoop)
                                            loopItem.loopval.Add(item);
                                        else
                                        {
                                            modlist.Add(item);
                                            //listMod.Items.Add(mod[i + 1] + " : " + mod[i] + " : " + item.len);
                                            nodey.Text = mod[i + 1] + " : " + mod[i] + " : " + item.len;
                                            nodey.Tag = modlist.Count-1;
                                            //treeView1.Nodes.Add(nodey);
                                            PROPS.Nodes.Add(nodey);
                                        }
                                    }
                                }
                                break;

                            case "COLOR":
                                if (rR.BaseStream.Position <= obj.modbuf.Length - 3)
                                {
                                    byte r = rR.ReadByte();
                                    byte g = rR.ReadByte();
                                    byte b = rR.ReadByte();
                                    Color col = Color.FromArgb(r, g, b);
                                    item.col = col;
                                    item.scrID = ScriptType.ScriptID.COLOR;
                                    if (isLoop)
                                        loopItem.loopval.Add(item);
                                    else
                                    {
                                        modlist.Add(item);
                                        //listMod.Items.Add(mod[i + 1] + " : " + mod[i]);
                                        nodey.Text = mod[i];
                                        nodey.Tag = modlist.Count-1;
                                        COLORS.Nodes.Add(nodey);
                                        //treeView1.Nodes.Add(nodey);
                                    }
                                }
                                break;

                            default: break;
                        }
                    }
                    if (isLoop)
                    {
                        modlist.Add(loopItem);
                        //listMod.Items.Add("LOOP" + " : " + loopName);
                        TreeNode nodey = new TreeNode();
                        nodey.Text = loopName;
                        nodey.Tag = modlist.Count-1;
                        //treeView1.Nodes.Add(nodey);
                        LOOPS.Nodes.Add(nodey);
                        isLoop = false;
                        loopLen = 0;
                    }
                    else
                        i += 3;
                }

                rR.Close();
                rS.Close();
            }
            catch
            {
                MessageBox.Show("*WARNING* Bad mod editor / corrupt mod box");
                loadfailed = true;
            }
        }

        private void txtval_TextChanged(object sender, EventArgs e)
        {
            ScriptType val;
            int index = (int)treeView1.SelectedNode.Tag;
            if (listLoop.Items.Count > 0)
            {
                val = modlist[index].loopval[listLoop.SelectedIndex];
            }
            else
                val = modlist[index];
            try
            {
                if (val.scrID != ScriptType.ScriptID.STRING && 
                    val.scrID != ScriptType.ScriptID.PROP &&
                    val.scrID != ScriptType.ScriptID.SPELL &&
                    val.scrID != ScriptType.ScriptID.ENCHANTMENT &&
                    val.scrID != ScriptType.ScriptID.ABILITY
                    )
                {
                    if (txtval.Text == "")
                        return;
                    txtval.Text = Regex.Replace(txtval.Text, @"\D", string.Empty);
                    if (txtval.Text == "")
                        return;
                }

                switch (val.scrID)
                {
                    case ScriptType.ScriptID.INT:
                        {
                            val.intval = Convert.ToInt32(txtval.Text);
                        } break;
                    case ScriptType.ScriptID.SHORT:
                        {
                            val.shortval = Convert.ToInt16(txtval.Text);
                        } break;
                    case ScriptType.ScriptID.BYTE:
                        {
                            val.byteval = Convert.ToByte(txtval.Text);
                        } break;
                    case ScriptType.ScriptID.BOOL:
                        {
                            val.boolval = Convert.ToBoolean(txtval.Text);
                        } break;
                    case ScriptType.ScriptID.STRING:
                        {
                            val.val = new byte[txtval.Text.Length];
                            val.val = Encoding.ASCII.GetBytes(txtval.Text);
                            val.len = txtval.Text.Length;
                        } break;
                    case ScriptType.ScriptID.SPELL:
                    case ScriptType.ScriptID.ENCHANTMENT:
                    case ScriptType.ScriptID.ABILITY:
                    case ScriptType.ScriptID.PROP:
                        {
                            val.val = new byte[txtval.Text.Length];
                            val.val = Encoding.ASCII.GetBytes(txtval.Text);
                            val.len = txtval.Text.Length;
                        } break;
                    case ScriptType.ScriptID.VOID: break;
                    case ScriptType.ScriptID.HIDDEN: break;
                    default: txtval.Text = "ERROR, NO VALUE"; break;
                }
            }
            catch (Exception ex)
            {
                txtval.Text = lastval;
                MessageBox.Show(ex.Message);
            }
            lastval = txtval.Text;
        }
        void WriteScriptValue(System.IO.BinaryWriter rR, ScriptType var)
        {
            switch (var.scrID)
                {
                    case ScriptType.ScriptID.INT:
                        {
                            rR.Write(var.intval);
                        } break;
                    case ScriptType.ScriptID.SHORT:
                        {
                            rR.Write(var.shortval);
                        } break;
                    case ScriptType.ScriptID.BYTE:
                        {
                            rR.Write(var.byteval);
                        } break;
                    case ScriptType.ScriptID.BOOL:
                        {
                            rR.Write(var.boolval);
                        } break;
                    case ScriptType.ScriptID.STRING:
                        {
                            rR.Write(var.len);
                            rR.Write(var.val,0,var.len);
                        } break;
                    case ScriptType.ScriptID.VOID:
                        {
                            rR.Write(var.val);
                        } break;
                    case ScriptType.ScriptID.HIDDEN:
                        {
                            rR.Write(var.val);
                        } break;
                    case ScriptType.ScriptID.DIRECTION:
                        {
                            rR.Write((ulong)var.dirval);
                        } break;
                   case ScriptType.ScriptID.SPELL:
                   case ScriptType.ScriptID.ENCHANTMENT:
                   case ScriptType.ScriptID.ABILITY:
                   case ScriptType.ScriptID.PROP:
                        {
                            rR.Write(Convert.ToByte(var.len));
                            rR.Write(var.val, 0, var.len);
                        } break;
                    case ScriptType.ScriptID.OBJECT:
                        {
                            rR.Write(Convert.ToByte(var.byteval));
                            rR.Write(Convert.ToByte(var.len));
                            rR.Write(var.val, 0, var.len);
                            rR.Write(Convert.ToByte(var.len2));
                            rR.Write(var.val2, 0, var.len2);
                            for (int p = 0; p < 4; p++)
                            {
                                rR.Write(Convert.ToByte(var.loopval[p].len));
                                rR.Write(var.loopval[p].val, 0, var.loopval[p].len);
                            }
                        } break;
                    case ScriptType.ScriptID.COLOR:
                        {
                            rR.Write(var.col.R);
                            rR.Write(var.col.G);
                            rR.Write(var.col.B);
                        } break;

                    default: break;
                }

        }
        private void ModEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (loadfailed)
                return;

            ArrayList wR = new ArrayList();
            System.IO.MemoryStream rS = new System.IO.MemoryStream();
            System.IO.BinaryWriter rR = new System.IO.BinaryWriter(rS);

            for (int i = 0; i < modlist.Count; ++i)
            {
                if (modlist[i].scrID == ScriptType.ScriptID.LOOP)
                {
                    rR.Write((byte)modlist[i].len);
                    for (int j = 0; j < modlist[i].loopval.Count; j++)
                    {
                        WriteScriptValue(rR, modlist[i].loopval[j]);
                    }
                }
                else
                {
                   // if( modlist[i].len > 0 && modlist[i].val.ToString() != "EMPTY")
                        WriteScriptValue(rR, modlist[i]);
                   // else
                   //     MessageBox.Show("One of the objects was not given a value and has been skipped.");
                }
            }
                obj.modbuf = rS.ToArray();
                rR.Close();
                rS.Close();
        }
        private void listPresets_Click(object sender, EventArgs e)
        {
            int index = (int)treeView1.SelectedNode.Tag;
            if (listPresets.Items.Count > 0 && modlist[index].scrID != ScriptType.ScriptID.DIRECTION)
                txtval.Text = (string)listPresets.SelectedItem;
        }

        private void listLoop_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listLoop_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int index = (int)treeView1.SelectedNode.Tag;
            if (modlist[index].scrID == ScriptType.ScriptID.LOOP)
            {
                if (modlist[index].loopval.Count > 0 && modlist[index].loopval[0].scrID != ScriptType.ScriptID.OBJECT)
                {
                        ScriptType item = new ScriptType();
                        item.scrID = modlist[index].loopval[0].scrID;
                        modlist[index].loopval.Add(item);
                        listLoop.Items.Add(item.scrID);
                }
                else
                {
                    ScriptType item = new ScriptType();
                    item.scrID = ScriptType.ScriptID.OBJECT;
                    item.val = new byte[5];
                    item.val = Encoding.ASCII.GetBytes("EMPTY");
                    item.val2 = new byte[13];
                    item.val2 = Encoding.ASCII.GetBytes("SPELL_INVALID");
                    item.loopval = new List<ScriptType>();
                    item.len = 5;
                    item.len2 = 13;
                    for (int i = 0; i < 4; i++)
                    {
                        ScriptType tem = new ScriptType();
                        tem.len = 0;
                        item.loopval.Add(tem);
                    }
                    modlist[index].loopval.Add(item);
                    listLoop.Items.Add("EMPTY");
                }
                modlist[index].len++;
            }
            }

        private void button2_Click(object sender, EventArgs e)
        {
            int index = (int)treeView1.SelectedNode.Tag;
            if (listLoop.Items.Count > 0)
            {
                if (listLoop.SelectedItem != null)
                {
                    modlist[index].loopval.RemoveAt(listLoop.SelectedIndex);
                    listLoop.Items.RemoveAt(listLoop.SelectedIndex);
                    modlist[index].len--;
                }
            }
        }

        private void listPresets_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = (int)treeView1.SelectedNode.Tag;
            if (modlist[index].scrID == ScriptType.ScriptID.DIRECTION)
            {
                modlist[index].dirval = (ScriptType.Direction)listPresets.SelectedItem;
            }
        }

        private void listLoop_DoubleClick(object sender, EventArgs e)
        {
            int index = (int)treeView1.SelectedNode.Tag;
            if (listLoop.Items.Count > 0)
            {
                if (listLoop.Items.Count == 0 || listLoop.SelectedIndex < 0 || listLoop.SelectedItem == null)
                    return;
                txtval.Visible = true;
                listPresets.Items.Clear();

                switch (modlist[index].loopval[listLoop.SelectedIndex].scrID)
                {
                    case ScriptType.ScriptID.INT:
                        {
                            txtval.Text = modlist[index].loopval[listLoop.SelectedIndex].intval.ToString();
                        } break;
                    case ScriptType.ScriptID.SHORT:
                        {
                            txtval.Text = modlist[index].loopval[listLoop.SelectedIndex].shortval.ToString();
                        } break;
                    case ScriptType.ScriptID.BYTE:
                        {
                            txtval.Text = modlist[index].loopval[listLoop.SelectedIndex].byteval.ToString();
                        } break;
                    case ScriptType.ScriptID.BOOL:
                        {
                            txtval.Text = modlist[index].loopval[listLoop.SelectedIndex].boolval.ToString();
                        } break;
                    case ScriptType.ScriptID.SPELL:
                        {
                            foreach (ThingDb.Spell spell in ThingDb.Spells.Values)
                            {
                                listPresets.Items.Add(spell.Name);
                            }

                            txtval.Text = System.Text.ASCIIEncoding.ASCII.GetString(modlist[index].loopval[listLoop.SelectedIndex].val);
                        } break;
                    case ScriptType.ScriptID.ENCHANTMENT:
                        {
                            listPresets.Items.AddRange(objEnchants.ToArray());
                            txtval.Text = System.Text.ASCIIEncoding.ASCII.GetString(modlist[index].loopval[listLoop.SelectedIndex].val);
                        } break;
                    case ScriptType.ScriptID.OBJECT:
                        {
                            ObjectEditorDialog enchantDlg = new ObjectEditorDialog();
              
                            enchantDlg.Object = modlist[index].loopval[listLoop.SelectedIndex];
                            if (enchantDlg.ShowDialog() == DialogResult.OK)
                            {
                                listLoop.Items[listLoop.SelectedIndex] = System.Text.ASCIIEncoding.ASCII.GetString(enchantDlg.Object.val);
                            }
                        } break;
                    case ScriptType.ScriptID.ABILITY:
                        {
                            foreach (ThingDb.Ability ability in ThingDb.Abilities.Values)
                            {
                                listPresets.Items.Add(ability.Name);
                            }
                            txtval.Text = System.Text.ASCIIEncoding.ASCII.GetString(modlist[index].loopval[listLoop.SelectedIndex].val);
                        } break;
                    case ScriptType.ScriptID.PROP:
                    case ScriptType.ScriptID.STRING:
                        {
                            txtval.Text = System.Text.ASCIIEncoding.ASCII.GetString(modlist[index].loopval[listLoop.SelectedIndex].val);
                        } break;
                    case ScriptType.ScriptID.VOID:
                        {
                            MessageBox.Show("You cannot edit void values in the edit dialog yet.");
                        } break;
                    case ScriptType.ScriptID.COLOR:
                        {
                            txtval.Visible = false;
                            ColorDialog ColorDialog1 = new ColorDialog();
                            ColorDialog1.AllowFullOpen = true;
                            ColorDialog1.FullOpen = true;
                            ColorDialog1.ShowHelp = true;
                            ColorDialog1.Color = modlist[index].loopval[listLoop.SelectedIndex].col;
                            ColorDialog1.ShowDialog();
                            modlist[index].loopval[listLoop.SelectedIndex].col = ColorDialog1.Color;

                        } break;
                    case ScriptType.ScriptID.HIDDEN: break;
                    default: txtval.Text = "ERROR, NO VALUE"; break;
                }
            }
        }

        private void listMod_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

            if (treeView1.SelectedNode.Parent == null || treeView1.SelectedNode.Parent.Name != "ROOT")
                return;
            if (treeView1.Nodes.Count == 0 || treeView1.SelectedNode == null)
                return;
            txtval.Visible = true;
            listPresets.Items.Clear();
            listLoop.Items.Clear();
            button1.Enabled = false;
            button2.Enabled = false;

            int index = (int)treeView1.SelectedNode.Tag;
            if (modlist.Count > 0 && modlist.Count >= index)
            {
                switch (modlist[index].scrID)
                {
                    case ScriptType.ScriptID.INT:
                        {
                            txtval.Text = modlist[index].intval.ToString();
                        } break;
                    case ScriptType.ScriptID.SHORT:
                        {
                            txtval.Text = modlist[index].shortval.ToString();
                        } break;
                    case ScriptType.ScriptID.BYTE:
                        {
                            txtval.Text = modlist[index].byteval.ToString();
                        } break;
                    case ScriptType.ScriptID.BOOL:
                        {
                            txtval.Text = modlist[index].boolval.ToString();
                        } break;
                    case ScriptType.ScriptID.DIRECTION:
                        {
                            listPresets.Items.Add(ScriptType.Direction.East);
                            listPresets.Items.Add(ScriptType.Direction.North);
                            listPresets.Items.Add(ScriptType.Direction.NorthEast);
                            listPresets.Items.Add(ScriptType.Direction.NorthWest);
                            listPresets.Items.Add(ScriptType.Direction.South);
                            listPresets.Items.Add(ScriptType.Direction.SouthEast);
                            listPresets.Items.Add(ScriptType.Direction.SouthWest);
                            listPresets.Items.Add(ScriptType.Direction.West);
                            try
                            {
                                listPresets.SelectedIndex = listPresets.Items.IndexOf(modlist[index].dirval);
                            }
                            catch
                            {
                                MessageBox.Show("INVALID DIRECTION SPECIFIED IN MODBOX!!!");
                            }
                            //txtval.Text = System.Text.ASCIIEncoding.ASCII.GetString(modlist[listMod.SelectedIndex].val);
                        } break;
                    case ScriptType.ScriptID.SPELL:
                        {
                            foreach (ThingDb.Spell spell in ThingDb.Spells.Values)
                            {
                                listPresets.Items.Add(spell.Name);
                            }

                            txtval.Text = System.Text.ASCIIEncoding.ASCII.GetString(modlist[index].val);
                        } break;
                    case ScriptType.ScriptID.ENCHANTMENT:
                        {
                            listPresets.Items.AddRange(objEnchants.ToArray());
                            txtval.Text = System.Text.ASCIIEncoding.ASCII.GetString(modlist[index].val);
                        } break;
                    case ScriptType.ScriptID.OBJECT:
                        {
                            ObjectEditorDialog enchantDlg = new ObjectEditorDialog();
                            enchantDlg.Object = modlist[index];
                            if (enchantDlg.ShowDialog() == DialogResult.OK)
                            {
                            }
                        } break;
                    case ScriptType.ScriptID.ABILITY:
                        {
                            foreach (ThingDb.Ability ability in ThingDb.Abilities.Values)
                            {
                                listPresets.Items.Add(ability.Name);
                            }
                            txtval.Text = System.Text.ASCIIEncoding.ASCII.GetString(modlist[index].val);
                        } break;
                    case ScriptType.ScriptID.PROP:
                    case ScriptType.ScriptID.STRING:
                        {
                            txtval.Text = System.Text.ASCIIEncoding.ASCII.GetString(modlist[index].val);
                        } break;
                    case ScriptType.ScriptID.LOOP:
                        {
                            button1.Enabled = true;
                            button2.Enabled = true;
                            for (int i = 0; i < modlist[index].loopval.Count; i++)
                            {
                                if (modlist[index].loopval[i].scrID == ScriptType.ScriptID.OBJECT)
                                    listLoop.Items.Add(System.Text.ASCIIEncoding.ASCII.GetString(modlist[index].loopval[i].val));
                                else
                                    listLoop.Items.Add(modlist[index].loopval[i].scrID);
                            }

                        } break;
                    case ScriptType.ScriptID.VOID:
                        {
                            MessageBox.Show("You cannot edit void values in the edit dialog yet.");
                        } break;
                    case ScriptType.ScriptID.COLOR:
                        {
                            txtval.Visible = false;
                            ColorDialog ColorDialog1 = new ColorDialog();
                            ColorDialog1.AllowFullOpen = true;
                            ColorDialog1.FullOpen = true;
                            ColorDialog1.ShowHelp = true;
                            ColorDialog1.Color = modlist[index].col;
                            ColorDialog1.ShowDialog();
                            modlist[index].col = ColorDialog1.Color;

                        } break;
                    case ScriptType.ScriptID.HIDDEN: break;
                    default: txtval.Text = "ERROR, NO VALUE"; break;
                }

            }
            else
                txtval.Text = "ERROR, BAD MOD / BAD TEMPLATE";
        }
        }
        }
   // }

    namespace MapEditor
    {
        public class ScriptFunctions
        {
            public ArrayList EditMod(string basepath, string objType)
            {
                ModifierDatabase modDB = new ModifierDatabase();
                ArrayList layout = modDB.Layout(basepath, objType);
                return (layout);
            }
            public bool CheckMod(string basepath, string objType)
            {
                return File.Exists(basepath + "\\scripts\\objects\\modeditors\\" + objType.ToLower() + ".txt");
            }
        }

        public class ScriptType
        {
            public enum Direction : ulong
            {
                South = (ulong)(0x0000000100000000),
                North = (ulong)(0xFFFFFFFF00000000),
                East =  (ulong)(0x0000000000000001),
                West =  (ulong)(0x00000000FFFFFFFF),
                SouthWest = (ulong)(0x00000001FFFFFFFF),
                NorthWest = (ulong)(0xFFFFFFFFFFFFFFFF),
                SouthEast = (ulong)(0x0000000100000001),
                NorthEast = (ulong)(0xFFFFFFFF00000001)
            };
            public enum ScriptID
            {
                INT,
                SHORT,
                BYTE,
                STRING,
                BOOL,
                FLOAT,
                HIDDEN,
                PROP,
                COLOR,
                SPELL,
                ENCHANTMENT,
                ABILITY,
                LOOP,
                END,
                DIRECTION,
                OBJECT,
                EMPTY,
                LOADREST,
                VOID
            };
            public ScriptType()
            {
                len = 0;
                intval = 0;
                shortval = 0;
                byteval = 0;
                boolval = false;
                floatval = 0;
                longval = 0;
                dirval = Direction.North;
            }

            public List<ScriptType> loopval;
            public ScriptType.ScriptID scrID;
            public int len;
            public byte[] val;
            public int len2;
            public byte[] val2;
            public Color col;
            public int intval;
            public short shortval;
            public byte byteval;
            public bool boolval;
            public ulong longval;
            public Direction dirval;
            public float floatval;
        }
    }

 /*************************************************************
   ModDataBase Types
   
   Mod Entry Format: 
  
   "INT" = INT Value
   "BOOL" = Boolean
   "VOID" = Void bytes, use len for length in bytes
   "SHORT" = Short Value
   "BYTE" = Byte Value
   "EXTENT" = Extent Value
   "STRING" = String value
   "FLOAT" = Float value
   "PROP" = Nox property value (len + string, repeat)
   "COLOR" = NoxColor value

 *************************************************************/

    namespace ModDatabase
    {
        public class ModifierDatabase
        {
            static ArrayList layout = new ArrayList();
            public ArrayList Layout(string basepath, string objType)
            {
                StreamReader SR;
                string LABEL, ID, VAL;
                SR = File.OpenText(basepath + "\\scripts\\objects\\modeditors\\" + objType.ToLower() + ".txt");
                if (SR != null)
                {
                    while (!SR.EndOfStream)
                    {
                        LABEL = SR.ReadLine();
                        ID = SR.ReadLine();
                        VAL = SR.ReadLine();
                        Add(LABEL, ID, VAL);
                    }
                }
                SR.Close();
                return (layout);
            }
            /**************************************************************************
             * 
             * 
             * 
             * 
             * 
             *************************************************************************/
            private void Add(string label, string type, string number)
            {
                layout.Add(label);
                layout.Add(type);
                layout.Add(number);
            }
        }
    }