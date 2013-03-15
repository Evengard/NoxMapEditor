using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using NoxShared;
using MapInterface;
using MapEditor;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace NoxMapEditor
{
	public class MainWindow : Form
    {
        // Need to fix delegates before I can use callback features
       // public delegate void OBJMoved_CALLBACK(int ObjNum,int X,int Y);
        public static CompilerResults results;
        public static MAP_GUI myMap;
        int lastX = 0; // Last click location
        int lastY = 0; // Last click location
        bool RMouseDown = false;
        public bool Magnify = false;
        bool JustUpdated = false;
        private TabPage largeMap;
		private MainMenu mainMenu1;
		private MenuItem menuItemOpen;
		private MenuItem menuSeparator1;
		private MenuItem menuItemExit;
		private TabControl tabControl1;
		private TabPage tabPage1;
		private Label labelTitle;
		private GroupBox groupBox1;
		private TextBox mapSummary;
		private Label labelDescription;
		private Label labelVersion;
		private TextBox mapDescription;
		private Label labelAuthor;
		private Label labelAuthor2;
		private Label labelEmail;
		private Label labelEmail2;
		private TextBox mapAuthor;
		private TextBox mapEmail;
		private TextBox mapEmail2;
		private TextBox mapAuthor2;
		private Label labelDate;
		private TextBox mapDate;
		private TextBox mapVersion;
		private Label labelCopyright;
		private MenuItem menuItemSave;
		private Label minRecLbl;
		private Label maxRecLbl;
		private Label recommendedLbl;
		private Label mapTypeLbl;
		private TextBox mapMinRec;
		private TextBox mapMaxRec;
		private ComboBox mapType;
		private MenuItem menuHelp;
		private MenuItem menuItemAbout;
		private MenuItem menuItemNew;
		private MenuItem menuItemSaveAs;
		private MenuItem menuMap;
		private MenuItem viewObjects;
		private TextBox mapCopyright;
		private MenuItem menuOptions;
		private MenuItem menuItemGrid;
		private MenuItem menuItemNxz;
		private MenuItem menuItemStrings;
		int mapZoom = 2, mapDimension = 256;

		protected Map map;
		private MenuItem langSelect;
		public MapView mapView;
		private MenuItem menuFile;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox questTitle;
		private System.Windows.Forms.TextBox questGraphic;
		private System.Windows.Forms.Label label3;
        private System.ComponentModel.IContainer components;
        private MenuItem menuItemGroups;
		private MenuItem menuWallCoors;
		private MenuItem exportMenu;
		private MenuItem importMenu;
		private MenuItem templateMenu;
        private MenuItem exportImageMenu;
        private TabPage tabGUI;
        private System.Windows.Forms.Timer timer1;
        private MenuItem menuItem1;
        private MenuItem menuItem2;
        private MenuItem menuItem3;
        private MenuItem menuItem4;
        private MenuItem menuItem5;
        private MenuItem menuItem6;
        private MenuItem menuItem7;
        private MenuItem menuItem8;
        private MenuItem menuItem9;
        private MenuItem menuItem10;
        private MenuItem menuItem11;
        private TabPage WallViewer;
        private Panel panel1;
        private Panel MinimapPanel;
        private GroupBox groupBox2;
        private Button buttonCenter;
        private MenuItem menuItem12;
        private Button button1;
        private Button button3;
        private Button button2;
        private CheckBox chkMass;
        private MenuItem menuItem16;
        private MenuItem menuItem18;
        private MenuItem menuItem19;
        private MenuItem menuItem20;
        private MenuItem menuItem21;
        private MenuItem menuItem22;
        private CheckBox chkDevide;
        private CheckBox chkDevide2;
        private MenuItem menuItem17;
        private Panel GuiPanel;
        private Panel ToolPanel;
        private CheckBox chkShowNPC;
        protected IList cultures;



		public MainWindow()
        {
            Splash spl = new Splash();
            spl.Show();
            spl.Refresh();

            //BEGIN COMPILATION
            Compiler com = new Compiler();
            ArrayList list = new ArrayList();
            string error = "Script function compilation failed!\n\n";
            results = com.CompileCDir("scripts", "ScriptFuncs.dll");
            if (results.Errors.Count > 0)
            {
                MessageBox.Show(error + com.errors, "ERROR");
            }
            //END COMPILATION

                if (Directory.Exists(Application.StartupPath + "\\updated"))
                {
                    Directory.Delete(Application.StartupPath + "\\updated", true);
                    JustUpdated = true;
                }
                AutoUpdate.UpdateFileName = "Update.txt";

                if (AutoUpdate.IsUpdatable("http://www.noxhub.net/updates/MapEditor",""))
                {
                    if (MessageBox.Show(null, "NoxMapEditor updates are available, do you wish to update now?", "Update Report", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        AutoUpdate.UpdateFiles("http://www.noxhub.net/updates/MapEditor","");
                        if (AutoUpdate.IsUpdatable("http://www.noxhub.net/updates/MapEditor/Files", ""))
                        {
                                AutoUpdate.UpdateFiles("http://www.noxhub.net/updates/MapEditor/Files", "");
                        }
                        MessageBox.Show("Update Complete, Please restart the program!\nUse the Help-Updates menu to view changes");
                        Environment.Exit(1);
                        return;
                    }
                }
                else if (AutoUpdate.IsUpdatable("http://www.noxhub.net/updates/MapEditor/Files", ""))
                {
                    if (MessageBox.Show(null, "NoxMapEditor updates are available, do you wish to update now?", "Update Report", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        AutoUpdate.UpdateFiles("http://www.noxhub.net/updates/MapEditor/Files", "");
                        MessageBox.Show("Update Complete, Please restart the program!\nUse the Help-Updates menu to view changes");
                        Environment.Exit(1);
                        return;
                    }
                }
                if (AutoUpdate.IsUpdatable("http://www.noxhub.net/updates/MapEditor/scripts/defaults","scripts\\objects\\defaultmods\\"))
                {
                    if (MessageBox.Show(null, "New default mods are available, do you wish to update now?", "Update Report", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        //Directory.Delete("scripts\\objects\\defaultmods\\",true);
                        AutoUpdate.UpdateFiles("http://www.noxhub.net/updates/MapEditor/scripts/defaults","scripts\\objects\\defaultmods\\");
                        MessageBox.Show("Update Complete, please use the Help-Updates menu to view changes");
                    }
                }
                if (AutoUpdate.IsUpdatable("http://www.noxhub.net/updates/MapEditor/scripts/editors","scripts\\objects\\modeditors\\"))
                {
                    if (MessageBox.Show(null, "New mod editors are available, do you wish to update now?", "Update Report", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        AutoUpdate.UpdateFiles("http://www.noxhub.net/updates/MapEditor/scripts/editors","scripts\\objects\\modeditors\\");
                        MessageBox.Show("Update Complete, please use the Help-Updates menu to view changes");
                    }
                }
                if (AutoUpdate.IsUpdatable("http://www.noxhub.net/updates/MapEditor/scripts/functions", "scripts\\functions\\"))
                {
                    if (MessageBox.Show(null, "A new Functions.cs is available, do you wish to update now?", "Update Report", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        AutoUpdate.UpdateFiles("http://www.noxhub.net/updates/MapEditor/scripts/functions", "scripts\\functions\\");
                        MessageBox.Show("Update Complete, please use the Help-Updates menu to view changes");
                    }
                }

            map = new Map();
            myMap = new MAP_GUI();
            cultures = GetSupportedCultures();
			InitializeComponent();

            MenuItem item;
			foreach (CultureInfo culture in cultures)
			{
				item = new MenuItem(culture.NativeName, new EventHandler(lang_Click));
				item.RadioCheck = true;
				langSelect.MenuItems.Add(item);
			}
            if (!Directory.Exists("Templates/"))
                Directory.CreateDirectory("Templates/");
			foreach (String templates in Directory.GetDirectories("Templates/"))
			{
				item = new MenuItem(templates.Replace("Templates/",""), templatesClick);
				templateMenu.MenuItems.Add(item);
			}

			mapType.Items.AddRange(new ArrayList(Map.MapInfo.MapTypeNames.Values).ToArray());
			mapType.SelectedIndex = 3;//arena by default

            unsafe
            {
                uint pHandle = (uint)GuiPanel.Handle.ToInt32();

                myMap.InitScreen(pHandle);
                myMap.Update_Window();
                //myMap.InitScreen(pHandle);
                //Encoding.ASCII ascii = new Encoding.ASCII;
                //Encoding unicode = Encoding.Unicode;

                //char * text = (char*)myMap.getName(11);
                //tabGUI.Text = 
                //string str = Marshal.PtrToStringAnsi(text);
               

                //myMap.AddObject(1840, 100, 100);
            }
            LoadNewMap();
            spl.Close();
            if (JustUpdated)
            {
                UpdateList ulist = new UpdateList();
                ulist.ShowDialog();
            }
		}

		#region Windows Form Designer generated code

		/// <summary>

		/// Required method for Designer support - do not modify

		/// the contents of this method with the code editor.

		/// </summary>

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.menuFile = new System.Windows.Forms.MenuItem();
            this.menuItemNew = new System.Windows.Forms.MenuItem();
            this.menuItemOpen = new System.Windows.Forms.MenuItem();
            this.menuItemSave = new System.Windows.Forms.MenuItem();
            this.menuItemSaveAs = new System.Windows.Forms.MenuItem();
            this.menuSeparator1 = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.menuMap = new System.Windows.Forms.MenuItem();
            this.viewObjects = new System.Windows.Forms.MenuItem();
            this.menuItemStrings = new System.Windows.Forms.MenuItem();
            this.menuItemGroups = new System.Windows.Forms.MenuItem();
            this.exportMenu = new System.Windows.Forms.MenuItem();
            this.importMenu = new System.Windows.Forms.MenuItem();
            this.exportImageMenu = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.menuItem21 = new System.Windows.Forms.MenuItem();
            this.menuItem22 = new System.Windows.Forms.MenuItem();
            this.menuOptions = new System.Windows.Forms.MenuItem();
            this.menuItemGrid = new System.Windows.Forms.MenuItem();
            this.menuItemNxz = new System.Windows.Forms.MenuItem();
            this.langSelect = new System.Windows.Forms.MenuItem();
            this.menuWallCoors = new System.Windows.Forms.MenuItem();
            this.menuItem16 = new System.Windows.Forms.MenuItem();
            this.menuHelp = new System.Windows.Forms.MenuItem();
            this.menuItem18 = new System.Windows.Forms.MenuItem();
            this.menuItemAbout = new System.Windows.Forms.MenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.largeMap = new System.Windows.Forms.TabPage();
            this.WallViewer = new System.Windows.Forms.TabPage();
            this.MinimapPanel = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkDevide2 = new System.Windows.Forms.CheckBox();
            this.chkDevide = new System.Windows.Forms.CheckBox();
            this.chkMass = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonCenter = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.questGraphic = new System.Windows.Forms.TextBox();
            this.questTitle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.mapType = new System.Windows.Forms.ComboBox();
            this.mapTypeLbl = new System.Windows.Forms.Label();
            this.recommendedLbl = new System.Windows.Forms.Label();
            this.maxRecLbl = new System.Windows.Forms.Label();
            this.minRecLbl = new System.Windows.Forms.Label();
            this.mapMaxRec = new System.Windows.Forms.TextBox();
            this.mapMinRec = new System.Windows.Forms.TextBox();
            this.mapCopyright = new System.Windows.Forms.TextBox();
            this.labelCopyright = new System.Windows.Forms.Label();
            this.mapVersion = new System.Windows.Forms.TextBox();
            this.mapDate = new System.Windows.Forms.TextBox();
            this.labelDate = new System.Windows.Forms.Label();
            this.mapAuthor2 = new System.Windows.Forms.TextBox();
            this.mapEmail2 = new System.Windows.Forms.TextBox();
            this.mapEmail = new System.Windows.Forms.TextBox();
            this.mapAuthor = new System.Windows.Forms.TextBox();
            this.labelEmail2 = new System.Windows.Forms.Label();
            this.labelEmail = new System.Windows.Forms.Label();
            this.labelAuthor2 = new System.Windows.Forms.Label();
            this.labelAuthor = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.mapDescription = new System.Windows.Forms.TextBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.mapSummary = new System.Windows.Forms.TextBox();
            this.tabGUI = new System.Windows.Forms.TabPage();
            this.ToolPanel = new System.Windows.Forms.Panel();
            this.chkShowNPC = new System.Windows.Forms.CheckBox();
            this.GuiPanel = new System.Windows.Forms.Panel();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.templateMenu = new System.Windows.Forms.MenuItem();
            this.menuItem20 = new System.Windows.Forms.MenuItem();
            this.menuItem17 = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.menuItem19 = new System.Windows.Forms.MenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.mapView = new NoxMapEditor.MapView();
            this.tabControl1.SuspendLayout();
            this.largeMap.SuspendLayout();
            this.WallViewer.SuspendLayout();
            this.MinimapPanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabGUI.SuspendLayout();
            this.ToolPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuFile
            // 
            this.menuFile.Index = 0;
            this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemNew,
            this.menuItemOpen,
            this.menuItemSave,
            this.menuItemSaveAs,
            this.menuSeparator1,
            this.menuItemExit});
            resources.ApplyResources(this.menuFile, "menuFile");
            // 
            // menuItemNew
            // 
            this.menuItemNew.Index = 0;
            resources.ApplyResources(this.menuItemNew, "menuItemNew");
            this.menuItemNew.Click += new System.EventHandler(this.menuItemNew_Click);
            // 
            // menuItemOpen
            // 
            this.menuItemOpen.Index = 1;
            resources.ApplyResources(this.menuItemOpen, "menuItemOpen");
            this.menuItemOpen.Click += new System.EventHandler(this.menuItemOpen_Click);
            // 
            // menuItemSave
            // 
            this.menuItemSave.Index = 2;
            resources.ApplyResources(this.menuItemSave, "menuItemSave");
            this.menuItemSave.Click += new System.EventHandler(this.menuItemSave_Click);
            // 
            // menuItemSaveAs
            // 
            this.menuItemSaveAs.Index = 3;
            resources.ApplyResources(this.menuItemSaveAs, "menuItemSaveAs");
            this.menuItemSaveAs.Click += new System.EventHandler(this.menuItemSaveAs_Click);
            // 
            // menuSeparator1
            // 
            this.menuSeparator1.Index = 4;
            resources.ApplyResources(this.menuSeparator1, "menuSeparator1");
            // 
            // menuItemExit
            // 
            this.menuItemExit.Index = 5;
            resources.ApplyResources(this.menuItemExit, "menuItemExit");
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // menuMap
            // 
            this.menuMap.Index = 1;
            this.menuMap.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.viewObjects,
            this.menuItemStrings,
            this.menuItemGroups,
            this.exportMenu,
            this.importMenu,
            this.exportImageMenu,
            this.menuItem12,
            this.menuItem21});
            resources.ApplyResources(this.menuMap, "menuMap");
            // 
            // viewObjects
            // 
            this.viewObjects.Index = 0;
            resources.ApplyResources(this.viewObjects, "viewObjects");
            this.viewObjects.Click += new System.EventHandler(this.viewObjects_Click);
            // 
            // menuItemStrings
            // 
            this.menuItemStrings.Index = 1;
            resources.ApplyResources(this.menuItemStrings, "menuItemStrings");
            this.menuItemStrings.Click += new System.EventHandler(this.menuItemStrings_Click);
            // 
            // menuItemGroups
            // 
            this.menuItemGroups.Index = 2;
            resources.ApplyResources(this.menuItemGroups, "menuItemGroups");
            this.menuItemGroups.Click += new System.EventHandler(this.menuItemGroups_Click);
            // 
            // exportMenu
            // 
            this.exportMenu.Index = 3;
            resources.ApplyResources(this.exportMenu, "exportMenu");
            this.exportMenu.Click += new System.EventHandler(this.exportMenu_Click);
            // 
            // importMenu
            // 
            this.importMenu.Index = 4;
            resources.ApplyResources(this.importMenu, "importMenu");
            this.importMenu.Click += new System.EventHandler(this.importMenu_Click);
            // 
            // exportImageMenu
            // 
            this.exportImageMenu.Index = 5;
            resources.ApplyResources(this.exportImageMenu, "exportImageMenu");
            this.exportImageMenu.Click += new System.EventHandler(this.exportImageMenu_Click);
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 6;
            resources.ApplyResources(this.menuItem12, "menuItem12");
            this.menuItem12.Click += new System.EventHandler(this.menuItem12_Click);
            // 
            // menuItem21
            // 
            this.menuItem21.Index = 7;
            this.menuItem21.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem22});
            resources.ApplyResources(this.menuItem21, "menuItem21");
            // 
            // menuItem22
            // 
            this.menuItem22.Index = 0;
            resources.ApplyResources(this.menuItem22, "menuItem22");
            this.menuItem22.Click += new System.EventHandler(this.menuItem22_Click);
            // 
            // menuOptions
            // 
            this.menuOptions.Index = 2;
            this.menuOptions.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemGrid,
            this.menuItemNxz,
            this.langSelect,
            this.menuWallCoors,
            this.menuItem16});
            resources.ApplyResources(this.menuOptions, "menuOptions");
            // 
            // menuItemGrid
            // 
            this.menuItemGrid.Index = 0;
            resources.ApplyResources(this.menuItemGrid, "menuItemGrid");
            this.menuItemGrid.Click += new System.EventHandler(this.menuItemGrid_Click);
            // 
            // menuItemNxz
            // 
            this.menuItemNxz.Checked = true;
            this.menuItemNxz.Index = 1;
            resources.ApplyResources(this.menuItemNxz, "menuItemNxz");
            this.menuItemNxz.Click += new System.EventHandler(this.menuItemNxz_Click);
            // 
            // langSelect
            // 
            this.langSelect.Index = 2;
            resources.ApplyResources(this.langSelect, "langSelect");
            this.langSelect.Popup += new System.EventHandler(this.langSelect_Popup);
            // 
            // menuWallCoors
            // 
            this.menuWallCoors.Index = 3;
            resources.ApplyResources(this.menuWallCoors, "menuWallCoors");
            this.menuWallCoors.Click += new System.EventHandler(this.menuWallCoors_Click);
            // 
            // menuItem16
            // 
            this.menuItem16.Index = 4;
            resources.ApplyResources(this.menuItem16, "menuItem16");
            this.menuItem16.Click += new System.EventHandler(this.menuItem16_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.Index = 3;
            this.menuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem18,
            this.menuItemAbout});
            resources.ApplyResources(this.menuHelp, "menuHelp");
            // 
            // menuItem18
            // 
            this.menuItem18.Index = 0;
            resources.ApplyResources(this.menuItem18, "menuItem18");
            this.menuItem18.Click += new System.EventHandler(this.menuItem18_Click);
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Index = 1;
            resources.ApplyResources(this.menuItemAbout, "menuItemAbout");
            this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.largeMap);
            this.tabControl1.Controls.Add(this.WallViewer);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabGUI);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Resize += new System.EventHandler(this.tabGUI_Resize);
            // 
            // largeMap
            // 
            this.largeMap.Controls.Add(this.mapView);
            resources.ApplyResources(this.largeMap, "largeMap");
            this.largeMap.Name = "largeMap";
            this.largeMap.UseVisualStyleBackColor = true;
            // 
            // WallViewer
            // 
            this.WallViewer.Controls.Add(this.MinimapPanel);
            resources.ApplyResources(this.WallViewer, "WallViewer");
            this.WallViewer.Name = "WallViewer";
            this.WallViewer.UseVisualStyleBackColor = true;
            // 
            // MinimapPanel
            // 
            this.MinimapPanel.Controls.Add(this.groupBox2);
            this.MinimapPanel.Controls.Add(this.panel1);
            resources.ApplyResources(this.MinimapPanel, "MinimapPanel");
            this.MinimapPanel.Name = "MinimapPanel";
            this.MinimapPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.MinimapPanel_Paint);
            this.MinimapPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MinimapPanel_MouseDown);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.chkDevide2);
            this.groupBox2.Controls.Add(this.chkDevide);
            this.groupBox2.Controls.Add(this.chkMass);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.buttonCenter);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // chkDevide2
            // 
            resources.ApplyResources(this.chkDevide2, "chkDevide2");
            this.chkDevide2.Name = "chkDevide2";
            this.chkDevide2.UseVisualStyleBackColor = true;
            this.chkDevide2.CheckedChanged += new System.EventHandler(this.chkDevide2_CheckedChanged);
            // 
            // chkDevide
            // 
            resources.ApplyResources(this.chkDevide, "chkDevide");
            this.chkDevide.Name = "chkDevide";
            this.chkDevide.UseVisualStyleBackColor = true;
            this.chkDevide.CheckedChanged += new System.EventHandler(this.chkDevide_CheckedChanged);
            // 
            // chkMass
            // 
            resources.ApplyResources(this.chkMass, "chkMass");
            this.chkMass.Name = "chkMass";
            this.chkMass.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseMnemonic = false;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonCenter
            // 
            resources.ApplyResources(this.buttonCenter, "buttonCenter");
            this.buttonCenter.Name = "buttonCenter";
            this.buttonCenter.UseVisualStyleBackColor = true;
            this.buttonCenter.Click += new System.EventHandler(this.buttonCenter_Click);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Name = "panel1";
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.questGraphic);
            this.groupBox1.Controls.Add(this.questTitle);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.mapType);
            this.groupBox1.Controls.Add(this.mapTypeLbl);
            this.groupBox1.Controls.Add(this.recommendedLbl);
            this.groupBox1.Controls.Add(this.maxRecLbl);
            this.groupBox1.Controls.Add(this.minRecLbl);
            this.groupBox1.Controls.Add(this.mapMaxRec);
            this.groupBox1.Controls.Add(this.mapMinRec);
            this.groupBox1.Controls.Add(this.mapCopyright);
            this.groupBox1.Controls.Add(this.labelCopyright);
            this.groupBox1.Controls.Add(this.mapVersion);
            this.groupBox1.Controls.Add(this.mapDate);
            this.groupBox1.Controls.Add(this.labelDate);
            this.groupBox1.Controls.Add(this.mapAuthor2);
            this.groupBox1.Controls.Add(this.mapEmail2);
            this.groupBox1.Controls.Add(this.mapEmail);
            this.groupBox1.Controls.Add(this.mapAuthor);
            this.groupBox1.Controls.Add(this.labelEmail2);
            this.groupBox1.Controls.Add(this.labelEmail);
            this.groupBox1.Controls.Add(this.labelAuthor2);
            this.groupBox1.Controls.Add(this.labelAuthor);
            this.groupBox1.Controls.Add(this.labelVersion);
            this.groupBox1.Controls.Add(this.mapDescription);
            this.groupBox1.Controls.Add(this.labelDescription);
            this.groupBox1.Controls.Add(this.labelTitle);
            this.groupBox1.Controls.Add(this.mapSummary);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // questGraphic
            // 
            resources.ApplyResources(this.questGraphic, "questGraphic");
            this.questGraphic.Name = "questGraphic";
            // 
            // questTitle
            // 
            resources.ApplyResources(this.questTitle, "questTitle");
            this.questTitle.Name = "questTitle";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // mapType
            // 
            this.mapType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mapType.FormattingEnabled = true;
            resources.ApplyResources(this.mapType, "mapType");
            this.mapType.Name = "mapType";
            // 
            // mapTypeLbl
            // 
            resources.ApplyResources(this.mapTypeLbl, "mapTypeLbl");
            this.mapTypeLbl.Name = "mapTypeLbl";
            // 
            // recommendedLbl
            // 
            resources.ApplyResources(this.recommendedLbl, "recommendedLbl");
            this.recommendedLbl.Name = "recommendedLbl";
            // 
            // maxRecLbl
            // 
            resources.ApplyResources(this.maxRecLbl, "maxRecLbl");
            this.maxRecLbl.Name = "maxRecLbl";
            // 
            // minRecLbl
            // 
            resources.ApplyResources(this.minRecLbl, "minRecLbl");
            this.minRecLbl.Name = "minRecLbl";
            // 
            // mapMaxRec
            // 
            resources.ApplyResources(this.mapMaxRec, "mapMaxRec");
            this.mapMaxRec.Name = "mapMaxRec";
            // 
            // mapMinRec
            // 
            resources.ApplyResources(this.mapMinRec, "mapMinRec");
            this.mapMinRec.Name = "mapMinRec";
            // 
            // mapCopyright
            // 
            resources.ApplyResources(this.mapCopyright, "mapCopyright");
            this.mapCopyright.Name = "mapCopyright";
            // 
            // labelCopyright
            // 
            resources.ApplyResources(this.labelCopyright, "labelCopyright");
            this.labelCopyright.Name = "labelCopyright";
            // 
            // mapVersion
            // 
            resources.ApplyResources(this.mapVersion, "mapVersion");
            this.mapVersion.Name = "mapVersion";
            // 
            // mapDate
            // 
            resources.ApplyResources(this.mapDate, "mapDate");
            this.mapDate.Name = "mapDate";
            // 
            // labelDate
            // 
            resources.ApplyResources(this.labelDate, "labelDate");
            this.labelDate.Name = "labelDate";
            // 
            // mapAuthor2
            // 
            resources.ApplyResources(this.mapAuthor2, "mapAuthor2");
            this.mapAuthor2.Name = "mapAuthor2";
            // 
            // mapEmail2
            // 
            resources.ApplyResources(this.mapEmail2, "mapEmail2");
            this.mapEmail2.Name = "mapEmail2";
            // 
            // mapEmail
            // 
            resources.ApplyResources(this.mapEmail, "mapEmail");
            this.mapEmail.Name = "mapEmail";
            // 
            // mapAuthor
            // 
            resources.ApplyResources(this.mapAuthor, "mapAuthor");
            this.mapAuthor.Name = "mapAuthor";
            // 
            // labelEmail2
            // 
            resources.ApplyResources(this.labelEmail2, "labelEmail2");
            this.labelEmail2.Name = "labelEmail2";
            // 
            // labelEmail
            // 
            resources.ApplyResources(this.labelEmail, "labelEmail");
            this.labelEmail.Name = "labelEmail";
            // 
            // labelAuthor2
            // 
            resources.ApplyResources(this.labelAuthor2, "labelAuthor2");
            this.labelAuthor2.Name = "labelAuthor2";
            // 
            // labelAuthor
            // 
            resources.ApplyResources(this.labelAuthor, "labelAuthor");
            this.labelAuthor.Name = "labelAuthor";
            // 
            // labelVersion
            // 
            resources.ApplyResources(this.labelVersion, "labelVersion");
            this.labelVersion.Name = "labelVersion";
            // 
            // mapDescription
            // 
            resources.ApplyResources(this.mapDescription, "mapDescription");
            this.mapDescription.Name = "mapDescription";
            // 
            // labelDescription
            // 
            resources.ApplyResources(this.labelDescription, "labelDescription");
            this.labelDescription.Name = "labelDescription";
            // 
            // labelTitle
            // 
            resources.ApplyResources(this.labelTitle, "labelTitle");
            this.labelTitle.Name = "labelTitle";
            // 
            // mapSummary
            // 
            resources.ApplyResources(this.mapSummary, "mapSummary");
            this.mapSummary.Name = "mapSummary";
            // 
            // tabGUI
            // 
            this.tabGUI.Controls.Add(this.ToolPanel);
            this.tabGUI.Controls.Add(this.GuiPanel);
            resources.ApplyResources(this.tabGUI, "tabGUI");
            this.tabGUI.Name = "tabGUI";
            this.tabGUI.UseVisualStyleBackColor = true;
            this.tabGUI.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tabGUI_MouseMove);
            this.tabGUI.Leave += new System.EventHandler(this.tabGUI_Leave);
            this.tabGUI.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabGUI_MouseDown);
            this.tabGUI.Resize += new System.EventHandler(this.tabGUI_Resize);
            this.tabGUI.Enter += new System.EventHandler(this.tabGUI_Enter);
            this.tabGUI.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tabGUI_MouseUp);
            // 
            // ToolPanel
            // 
            this.ToolPanel.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ToolPanel.Controls.Add(this.chkShowNPC);
            resources.ApplyResources(this.ToolPanel, "ToolPanel");
            this.ToolPanel.Name = "ToolPanel";
            // 
            // chkShowNPC
            // 
            resources.ApplyResources(this.chkShowNPC, "chkShowNPC");
            this.chkShowNPC.Name = "chkShowNPC";
            this.chkShowNPC.UseVisualStyleBackColor = true;
            this.chkShowNPC.CheckedChanged += new System.EventHandler(this.chkShowNPC_CheckedChanged);
            // 
            // GuiPanel
            // 
            resources.ApplyResources(this.GuiPanel, "GuiPanel");
            this.GuiPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.GuiPanel.Name = "GuiPanel";
            this.GuiPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.GuiPanel_Paint);
            this.GuiPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GuiPanel_MouseMove);
            this.GuiPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GuiPanel_MouseDown);
            this.GuiPanel.Resize += new System.EventHandler(this.GuiPanel_Resize);
            this.GuiPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GuiPanel_MouseUp);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFile,
            this.menuMap,
            this.menuOptions,
            this.menuHelp,
            this.templateMenu,
            this.menuItem1});
            // 
            // templateMenu
            // 
            this.templateMenu.Index = 4;
            this.templateMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem20,
            this.menuItem17});
            resources.ApplyResources(this.templateMenu, "templateMenu");
            // 
            // menuItem20
            // 
            this.menuItem20.Index = 0;
            resources.ApplyResources(this.menuItem20, "menuItem20");
            this.menuItem20.Click += new System.EventHandler(this.menuItem20_Click);
            // 
            // menuItem17
            // 
            this.menuItem17.Index = 1;
            resources.ApplyResources(this.menuItem17, "menuItem17");
            this.menuItem17.Click += new System.EventHandler(this.menuItem17_Click_1);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 5;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2,
            this.menuItem19});
            resources.ApplyResources(this.menuItem1, "menuItem1");
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem3,
            this.menuItem8});
            resources.ApplyResources(this.menuItem2, "menuItem2");
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 0;
            this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem4,
            this.menuItem5,
            this.menuItem6,
            this.menuItem7});
            resources.ApplyResources(this.menuItem3, "menuItem3");
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 0;
            resources.ApplyResources(this.menuItem4, "menuItem4");
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 1;
            resources.ApplyResources(this.menuItem5, "menuItem5");
            this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 2;
            resources.ApplyResources(this.menuItem6, "menuItem6");
            this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 3;
            resources.ApplyResources(this.menuItem7, "menuItem7");
            this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 1;
            this.menuItem8.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem9,
            this.menuItem10,
            this.menuItem11});
            resources.ApplyResources(this.menuItem8, "menuItem8");
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 0;
            resources.ApplyResources(this.menuItem9, "menuItem9");
            this.menuItem9.Click += new System.EventHandler(this.menuItem9_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Checked = true;
            this.menuItem10.Index = 1;
            resources.ApplyResources(this.menuItem10, "menuItem10");
            this.menuItem10.Click += new System.EventHandler(this.menuItem10_Click);
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 2;
            resources.ApplyResources(this.menuItem11, "menuItem11");
            this.menuItem11.Click += new System.EventHandler(this.menuItem11_Click);
            // 
            // menuItem19
            // 
            this.menuItem19.Index = 1;
            resources.ApplyResources(this.menuItem19, "menuItem19");
            this.menuItem19.Click += new System.EventHandler(this.menuItem19_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 33;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // mapView
            // 
            resources.ApplyResources(this.mapView, "mapView");
            this.mapView.Name = "mapView";
            // 
            // MainWindow
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tabControl1);
            this.Menu = this.mainMenu1;
            this.Name = "MainWindow";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainWindow_KeyUp);
            this.Move += new System.EventHandler(this.MainWindow_Move);
            this.tabControl1.ResumeLayout(false);
            this.largeMap.ResumeLayout(false);
            this.WallViewer.ResumeLayout(false);
            this.MinimapPanel.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabGUI.ResumeLayout(false);
            this.ToolPanel.ResumeLayout(false);
            this.ToolPanel.PerformLayout();
            this.ResumeLayout(false);

        }
		#endregion

		[STAThread]
		static void Main()
		{
            try
            {
                Debug.Listeners.Add(new TextWriterTraceListener("Debug.log"));
				Debug.AutoFlush = true;
				Debug.WriteLine(String.Format("Started at {0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to open debug log.", "Non-fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            try
            {
                Application.Run(new MainWindow());
			}
		catch (Exception ex)
			{
				new ExceptionDialog(ex).ShowDialog();
				Environment.Exit(1);
			}
		}
		private void menuItemOpen_Click(object sender, EventArgs e)
		{
            
			OpenFileDialog fd = new OpenFileDialog();

			fd.Filter = "Nox Map Files (*.map)|*.map";

			if (fd.ShowDialog() == DialogResult.OK && File.Exists(fd.FileName))
			{
                myMap.ClearMap();
				//TODO: check for changes and prompt to save before opening another map
				map = new Map(fd.FileName);

				//questTitle.DataBindings.Clear();
				//questTitle.DataBindings.Add(new Binding("Text", map.Info, "QIntroTitle"));

				mapView.Map = map;
				mapView.SelectedObject = null;

				mapType.SelectedIndex = Map.MapInfo.MapTypeNames.IndexOfKey(map.Info.Type);
				mapSummary.Text = map.Info.Summary;
				mapDescription.Text = map.Info.Description;

				mapAuthor.Text = map.Info.Author;
				mapEmail.Text = map.Info.Email;
				mapAuthor2.Text = map.Info.Author2;
				mapEmail2.Text = map.Info.Email2;

				mapVersion.Text = map.Info.Version;
				mapCopyright.Text = map.Info.Copyright;
				mapDate.Text = map.Info.Date;

				mapMinRec.Text = String.Format("{0}", map.Info.RecommendedMin);
				mapMaxRec.Text = String.Format("{0}", map.Info.RecommendedMax);

				questTitle.Text = map.Info.QIntroTitle;
				questGraphic.Text = map.Info.QIntroGraphic;

                foreach (Map.Tile tile in map.Tiles.Values)
                {
                    unsafe
                    {
                        IntPtr ptr = Marshal.StringToHGlobalAnsi(tile.Graphic);
                        myMap.AddTile(ptr.ToPointer(), tile.Location.Y, tile.Location.X,(int)tile.Variation);
                    }
                }

                foreach (Map.Object obj in map.Objects)
                {
                    unsafe
                    {
                        obj.UniqueID = mapView.IdCount++;
                        IntPtr ptr = Marshal.StringToHGlobalAnsi(obj.Name);
                        int val = obj.UniqueID;
                        if ((ThingDb.Things[obj.Name].Class & ThingDb.Thing.ClassFlags.DOOR) == ThingDb.Thing.ClassFlags.DOOR)
                        {
                           myMap.AddObject(ptr.ToPointer(), (int)obj.Location.X, (int)obj.Location.Y,val, obj.modbuf[0]);
                        }
                        else
                        myMap.AddObject(ptr.ToPointer(), (int)obj.Location.X, (int)obj.Location.Y,val,-1);
                    }
                }
                myMap.SetXY(1, 1);

				Invalidate(true);
			}
		}

		private void menuItemExit_Click(object sender, EventArgs e)
		{
           if (MessageBox.Show("Are you sure you wish to close?", "CLOSING EDITOR!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;
			Close();
		}

		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		{
			//TODO: prompt for save if unsaved
		}

		protected override void OnClosed(EventArgs e)
		{
			Environment.Exit(0);
		}

		private void MinimapPanel_Paint(object sender, PaintEventArgs e)
		{
			//if (map == null)
		//		return;
		//	e.Graphics.FillRectangle(new SolidBrush(Color.Black), 0, 0, mapDimension*mapZoom, mapDimension*mapZoom);

//			foreach (Point pt in map.Walls.Keys)
//				e.Graphics.DrawRectangle(new Pen(Color.White, 1), pt.X*mapZoom, pt.Y*mapZoom, mapZoom/2, mapZoom/2);
        }

		private void menuItemSave_Click(object sender, EventArgs e)
		{
			if(map == null)
				return;
			else if (map.FileName == "")
			{
				menuItemSaveAs.PerformClick();
				return;
			}

			//TODO: check lengths for each to make sure they aren't too long
			map.Info.Type = (Map.MapInfo.MapType) Map.MapInfo.MapTypeNames.GetKey(mapType.SelectedIndex);//FIXME: default to something if unspecified
			map.Info.Summary = mapSummary.Text;
			map.Info.Description = mapDescription.Text;

			map.Info.Author = mapAuthor.Text;
			map.Info.Email = mapEmail.Text;
			map.Info.Author2 = mapAuthor2.Text;
			map.Info.Email2 = mapEmail2.Text;

			map.Info.Version = mapVersion.Text;
			map.Info.Copyright = mapCopyright.Text;
			map.Info.Date = mapDate.Text;
			map.Info.RecommendedMin = mapMinRec.Text.Length == 0 ? (byte) 0 : Convert.ToByte(mapMinRec.Text);
			map.Info.RecommendedMax = mapMaxRec.Text.Length == 0 ? (byte) 0 : Convert.ToByte(mapMaxRec.Text);
			map.Info.QIntroTitle = questTitle.Text;
			map.Info.QIntroGraphic = questGraphic.Text;

			map.WriteMap();
			if (menuItemNxz.Checked)
				try
				{
					map.WriteNxz();
				}
				catch (Exception)
				{
					MessageBox.Show("Couldn't write the compressed map. Map compression is still buggy. Try changing your map in any way and saving again.");
				}
		}

		private void menuItemAbout_Click(object sender, EventArgs e)
		{
			AboutDialog dlg = new AboutDialog();
			dlg.ShowDialog();
		}
        void LoadNewMap()
        {
            // System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            // Stream strm =  resources.GetStream("BlankMap.map");
            Stream strm;
            Assembly asm = Assembly.GetExecutingAssembly();

            string fullName = null;
            foreach (string str in asm.GetManifestResourceNames())
            {
                if (str.EndsWith("BlankMap.map"))
                {
                    fullName = str;
                    break;
                }
            }
            if (fullName == null)
                throw new Exception("Can not find " + "Blank Map" + " resource in container file");

            strm = asm.GetManifestResourceStream(fullName);

            if( strm == null )
                throw new Exception("Could not load " + "Blank Map");
            
               // MessageBox.Show("it worked");
                NoxBinaryReader rdr = new NoxBinaryReader(strm, CryptApi.NoxCryptFormat.MAP);
                map.FileName = "";
                myMap.ClearMap();
                //TODO: check for changes and prompt to save before opening another map
                map = new Map(rdr);
                //questTitle.DataBindings.Clear();
                //questTitle.DataBindings.Add(new Binding("Text", map.Info, "QIntroTitle"));

                mapView.Map = map;
                mapView.SelectedObject = null;

                mapType.SelectedIndex = Map.MapInfo.MapTypeNames.IndexOfKey(map.Info.Type);
                mapSummary.Text = map.Info.Summary;
                mapDescription.Text = map.Info.Description;

                mapAuthor.Text = map.Info.Author;
                mapEmail.Text = map.Info.Email;
                mapAuthor2.Text = map.Info.Author2;
                mapEmail2.Text = map.Info.Email2;

                mapVersion.Text = map.Info.Version;
                mapCopyright.Text = map.Info.Copyright;
                mapDate.Text = map.Info.Date;

                mapMinRec.Text = String.Format("{0}", map.Info.RecommendedMin);
                mapMaxRec.Text = String.Format("{0}", map.Info.RecommendedMax);

                questTitle.Text = map.Info.QIntroTitle;
                questGraphic.Text = map.Info.QIntroGraphic;

                foreach (Map.Tile tile in map.Tiles.Values)
                {
                    unsafe
                    {
                        IntPtr ptr = Marshal.StringToHGlobalAnsi(tile.Graphic);
                        myMap.AddTile(ptr.ToPointer(), tile.Location.Y, tile.Location.X, (int)tile.Variation);
                    }
                }

                foreach (Map.Object obj in map.Objects)
                {
                    unsafe
                    {
                        obj.UniqueID = mapView.IdCount++;
                        IntPtr ptr = Marshal.StringToHGlobalAnsi(obj.Name);
                        int val = obj.UniqueID;
                        if ((ThingDb.Things[obj.Name].Class & ThingDb.Thing.ClassFlags.DOOR) == ThingDb.Thing.ClassFlags.DOOR)
                        {
                            myMap.AddObject(ptr.ToPointer(), (int)obj.Location.X, (int)obj.Location.Y, val, obj.modbuf[0]);
                        }
                        else
                        myMap.AddObject(ptr.ToPointer(), (int)obj.Location.X, (int)obj.Location.Y, val,-1);
                    }
                }
                myMap.SetXY(1, 1);

                Invalidate(true);
                //map = new Map(fd.FileName);
        }
		private void menuItemNew_Click(object sender, EventArgs e)
        {
            LoadNewMap();
		}

		private void menuItemSaveAs_Click(object sender, EventArgs e)
		{
			SaveFileDialog fd = new SaveFileDialog();
			fd.Filter = "Nox Map Files (*.map)|*.map";

			if (fd.ShowDialog() == DialogResult.OK)//&& fd.FileName)
			{
				map.FileName = fd.FileName;
				menuItemSave.PerformClick();
			}
		}

		private void viewObjects_Click(object sender, EventArgs e)
		{
			ObjectListDialog objLd = new ObjectListDialog();
			objLd.objTable = map.Objects;
			objLd.map = this.mapView;
			objLd.Show();
			objLd.Owner = this;
		}

		private void menuItemGrid_Click(object sender, EventArgs e)
		{
			menuItemGrid.Checked = !menuItemGrid.Checked;
			mapView.DrawGrid = menuItemGrid.Checked;
			Invalidate(true);
		}

		private void menuItemNxz_Click(object sender, EventArgs e)
		{
			menuItemNxz.Checked = !menuItemNxz.Checked;
		}

		private void menuItemStrings_Click(object sender, EventArgs e)
		{
            ScriptFunctionDialog strSd = new ScriptFunctionDialog();
            strSd.Scripts = map.Scripts;
			strSd.ShowDialog(this);
			map.Scripts = strSd.Scripts;
		}

		private void lang_Click(object sender, EventArgs e)
		{
			Thread.CurrentThread.CurrentUICulture = (CultureInfo) cultures[langSelect.MenuItems.IndexOf((MenuItem) sender)];
			FormLanguageSwitchSingleton.Instance.ChangeLanguage(this);
		}

		private void langSelect_Popup(object sender, EventArgs e)
		{
			int ndx = Math.Max(0, cultures.IndexOf(Thread.CurrentThread.CurrentUICulture));

			foreach (MenuItem item in langSelect.MenuItems)
				item.Checked = false;
			langSelect.MenuItems[ndx].Checked = true;
		}

		public static IList GetSupportedCultures()
		{
			ArrayList list = new ArrayList();
			list.Add(CultureInfo.InvariantCulture);
			foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
			{
				try
				{
					Assembly.GetExecutingAssembly().GetSatelliteAssembly(culture);
					list.Add(culture);//won't get added if not found (exception will be thrown)
				}
				catch (Exception) {}
			}
			return list;
		}

        private void menuItemGroups_Click(object sender, EventArgs e)
        {
            GroupDialog gd = new GroupDialog();
            gd.GroupD = map.Groups;
            gd.ShowDialog(this);
            map.Groups = gd.GroupD;
        }
		private void snapToGridMenu_Click(object sender, EventArgs e)
		{

		}
		private void MinimapPanel_MouseDown(object sender, MouseEventArgs e)
		{
			//Rectangle minimapBounds = new Rectangle(new Point(0, 0), new Size(mapDimension * mapZoom, mapDimension * mapZoom));
			//if (minimapBounds.Contains(e.X, e.Y))
			//{
			//	mapView.centerPoint(new Point(e.X / mapZoom * MapView.squareSize, e.Y / mapZoom * MapView.squareSize));
           //     myMap.SetLoc((int)(e.X / mapZoom * MapView.squareSize), (int)(e.Y / mapZoom * MapView.squareSize));
           //     tabControl1.SelectTab("tabGUI");
			//}
		}
		private void menuWallCoors_Click(object sender, EventArgs e)
		{
			menuWallCoors.Checked = !menuWallCoors.Checked;
			mapView.WallCoors = menuWallCoors.Checked;
		}

		private void exportMenu_Click(object sender, EventArgs e)
		{
            if (mapView.SelectedObject != null)
            {

                SaveFileDialog sfd = new SaveFileDialog(); // Map save dialog
                sfd.Filter = "XML Document|*.xml";
                sfd.DefaultExt = "xml";
                sfd.AddExtension = true;
                sfd.OverwritePrompt = true;
                sfd.ShowDialog();
                System.IO.Stream stream = sfd.OpenFile();
               // foreach (Object o in itemList.SelectedItems)
                XML.exportClassToXml(mapView.SelectedObject, stream);
                stream.Close();
            }
		}

		private void importMenu_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "XML Document|*.xml";
			ofd.DefaultExt = "xml";
			ofd.AddExtension = true;
			ofd.ValidateNames = true;
			ofd.CheckFileExists = true;
			ofd.ShowDialog();
			Object obj = XML.importXml(ofd.OpenFile());
            if (obj != null)
            {
                Map.Object obj2 = (Map.Object)obj;
                //switch (obj.GetType().FullName)
                //{
                  //  case "Map.Object":
                    //    map.Objects.Add(obj);
                      //  break;
                //}
                int ex = mapView.SelectedObject.Extent;
                mapView.SelectedObject = obj2;
                mapView.SelectedObject.Extent = ex;
            }
		}

		private void templatesClick(object sender, EventArgs e)
		{
			Template template = new Template(((MenuItem)sender).Text, map);
			template.Show();
		}

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabGUI)
            {
                myMap.Render(Magnify);
                //unsafe
                //{
                    //IntPtr pt = (IntPtr)myMap.GetSurface();
                    //mapView.DrawWallsToSurface(pt);
                    //myMap.ReleaseSurface((int*)pt);
                //}
                    //mapView.
            }
        }
        private void tabGUI_Enter(object sender, EventArgs e)
        {
            myMap.SetLoc(mapView.winX, mapView.winY);
            myMap.Update_Window();
            timer1.Enabled = true;
        }
        private void tabGUI_Leave(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }
        private void MainWindow_Move(object sender, EventArgs e)
        {
            myMap.Update_Window();
        }
        private void tabGUI_Resize(object sender, EventArgs e)
        {
            //myMap.ReInit(GuiPanel.Width, GuiPanel.Height);
            //myMap.Update_Window();
        }


        private void exportImageMenu_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Windows Bitmap|*.bmp|JPEG Image|*.jpg|PNG Image|*.png";   
            sfd.AddExtension = true;
            sfd.ValidateNames = true;
            sfd.OverwritePrompt = true;
            sfd.ShowDialog();
            if (sfd.FileName != "")
            {
                Bitmap mapBitmap = mapView.MapToImage();
                if (mapBitmap != null)
                {
                    System.Drawing.Imaging.ImageFormat imageFormat;
                    switch (sfd.FilterIndex)
                    {
                        case 1:
                            imageFormat = System.Drawing.Imaging.ImageFormat.Bmp; // Does this export?
                            break;
                        case 2:
                            imageFormat = System.Drawing.Imaging.ImageFormat.Jpeg; // Only PNG right?
                            break;
                        case 3:
                            imageFormat = System.Drawing.Imaging.ImageFormat.Png; // Why have the other options?
                            break;
                        default:
                            return;
                    }
                    mapBitmap.Save(sfd.FileName, imageFormat);
                }
            }
        }
    
        private void tabGUI_MouseDown(object sender, MouseEventArgs e)
        {
        }
        private void tabGUI_MouseUp(object sender, MouseEventArgs e)
        {
        }
        private void tabGUI_MouseMove(object sender, MouseEventArgs e)
        {
        }
        private void menuItem4_Click(object sender, EventArgs e)
        {
            myMap.SetBG(Color.Blue.R,Color.Blue.G,Color.Blue.B);
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
            myMap.SetBG(Color.Red.R, Color.Red.G, Color.Red.B);
        }

        private void menuItem6_Click(object sender, EventArgs e)
        {
            myMap.SetBG(Color.Green.R, Color.Green.G, Color.Green.B);
        }

        private void menuItem7_Click(object sender, EventArgs e)
        {
            myMap.SetBG(0, 0, 0); // Set background to black
        }

        private void menuItem10_Click(object sender, EventArgs e)
        {
            Magnify = false;
            menuItem9.Checked = false;
            menuItem10.Checked = true;

        }

        private void menuItem9_Click(object sender, EventArgs e)
        {
            Magnify = true;
            menuItem10.Checked = false;
            menuItem9.Checked = true;
        }

        private void menuItem11_Click(object sender, EventArgs e)
        {
            MagSet fd = new MagSet(myMap.MAG_MAX,myMap.MAG_SIZE);
            fd.ShowDialog();
            myMap.Magnify(fd.MagMax, fd.MagSize);
        }

        private static void tileCallback(int x, int y, int tile)
        {
           // Console.WriteLine("Callback invoked for " +
                                                  //count + " time");
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (map == null)
                return;
            panel1.Width = mapDimension * mapZoom;
            panel1.Height = mapDimension * mapZoom;
            // Add back in for minimap zoom, but needs work so that it works correctly.
            /*int Width = mapDimension*((tabControl1.Width - groupBox2.Width) / mapDimension);
            int Height = mapDimension*( tabControl1.Height / mapDimension);

            if (Height < Width)
                Width = Height;

            else if (Width < Height)
                Height = Width;

            mapZoom = Width / mapDimension;
            if (mapZoom < 2)
            {
                mapZoom = 2;
                Width = mapDimension * mapZoom;
                Height = Width;
            }
            panel1.Width = Width;
            panel1.Height = Height;
            */
            e.Graphics.Clear(Color.Black);
            //e.Graphics.FillRectangle(new SolidBrush(Color.Black), 0, 0, mapDimension * mapZoom, mapDimension * mapZoom);

            foreach (Point pt in map.Walls.Keys)
                e.Graphics.DrawRectangle(new Pen(Color.White, 1), pt.X * mapZoom, pt.Y * mapZoom, mapZoom / 2, mapZoom / 2);
            if (chkDevide.Checked)
            {
                e.Graphics.DrawLine(new Pen(Color.Aqua, 1), new Point(0, 0), new Point(512, 512));
            }
            if( chkDevide2.Checked)
            {
                e.Graphics.DrawLine(new Pen(Color.Aqua, 1), new Point(512, 0), new Point(0, 512));
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            Rectangle minimapBounds = new Rectangle(new Point(0, 0), new Size(mapDimension * mapZoom, mapDimension * mapZoom));
            if (minimapBounds.Contains(e.X, e.Y))
            {
                mapView.centerPoint(new Point(e.X / mapZoom * MapView.squareSize, e.Y / mapZoom * MapView.squareSize));
                myMap.SetLoc((int)(e.X / mapZoom * MapView.squareSize)-mapView.WidthMod, (int)(e.Y / mapZoom * MapView.squareSize));
                if (e.Button == MouseButtons.Left)
                    tabControl1.SelectTab("largeMap");
                else
                    tabControl1.SelectTab("tabGUI");
            }
        }

        private void buttonCenter_Click(object sender, EventArgs e)
        {
                mapView.centerPoint(new Point((mapDimension / 2) * MapView.squareSize, (mapDimension / 2) * MapView.squareSize));
                myMap.SetLoc((int)((mapDimension / 2) * MapView.squareSize), (int)((mapDimension / 2) * MapView.squareSize));
                tabControl1.SelectTab("tabGUI");
        }

        private void menuItem12_Click(object sender, EventArgs e)
        {
            int val = 20;
            foreach (Map.Object obj in map.Objects)
            {
                obj.Extent = val++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (chkMass.Checked)
            {
                for (int i = 0; i < 255; i++)
                {
                    for (int j = 0; j < 255; j++)
                    {
                        map.Walls.Remove(new Point(i * 23, j * 23));
                    }
                }
                chkMass.Checked = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (chkMass.Checked)
            {
                foreach (Map.Object obj in map.Objects)
                {
                    map.Objects.Remove(obj);
                }
                myMap.ClearObjects();
                chkMass.Checked = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (chkMass.Checked)
            {
                for (int i = 0; i < 255; i++)
                {
                    for (int j = 0; j < 255; j++)
                    {
                        map.Tiles.Remove(new Point(i*23,j*23));
                    }
                }
                myMap.ClearTiles();
                chkMass.Checked = false;
            }
        }

        private void menuItem16_Click(object sender, EventArgs e)
        {
            menuItem16.Checked = !menuItem16.Checked;
            if (menuItem16.Checked)
                mapView.ColorLayout.InvertColors();
            else
                mapView.ColorLayout.ResetColors();
        }

        private void menuItem17_Click(object sender, EventArgs e)
        {
            HelpBrowser hlp = new HelpBrowser();
            hlp.Show();
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
           /* if (e.KeyCode == Keys.F1)
            {
                HelpBrowser hlp = new HelpBrowser();
                hlp.Show();
            }*/
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
        }

        private void menuItem18_Click(object sender, EventArgs e)
        {
            UpdateList list = new UpdateList();
            list.ShowDialog();
        }

        private void menuItem19_Click(object sender, EventArgs e)
        {
            chkShowNPC.Checked = !chkShowNPC.Checked;
        }

        private void menuItem20_Click(object sender, EventArgs e)
        {
            ModList moddy = new ModList();
            moddy.ShowDialog();
        }

        private void menuItem22_Click(object sender, EventArgs e)
        {
            FlipMap flip = new FlipMap();
            flip.view = mapView;
            flip.ShowDialog();
        }

        private void chkDevide_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Refresh();
        }

        private void chkDevide2_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Refresh();
        }

        private void menuItem17_Click_1(object sender, EventArgs e)
        {
            DefaultList def = new DefaultList(map.Objects);
            def.ShowDialog();
        }
        private void GuiPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                RMouseDown = true;
                lastX = e.X;
                lastY = e.Y;
            }
            if (e.Button == MouseButtons.Middle)
            {
                //myMap.Magnify(100, 0);
            }
        }
        private void GuiPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                RMouseDown = false;
        }
        private void GuiPanel_MouseMove(object sender, MouseEventArgs e)
        {
            myMap.MOUSEX = e.X;
            myMap.MOUSEY = e.Y;
            myMap.SetMouse(e.X, e.Y);
            if (RMouseDown)
            {
                myMap.SetXY(-(e.X - lastX), -(e.Y - lastY));
                mapView.centerPoint(new Point(myMap.GetCenterX() - mapView.WidthMod, myMap.GetCenterY()));
                lastX = e.X;
                lastY = e.Y;
            }
        }

        private void GuiPanel_Resize(object sender, EventArgs e)
        {
            myMap.ReInit(GuiPanel.Width, GuiPanel.Height);
            myMap.Update_Window();
        }

        private void GuiPanel_Paint(object sender, PaintEventArgs e)
        {
            myMap.Update_Window();
        }

        private void chkShowNPC_CheckedChanged(object sender, EventArgs e)
        {
            menuItem19.Checked = chkShowNPC.Checked;
            myMap.Scale(menuItem19.Checked);
        }
     /*   public static void obMoveCALLBACK(int ObjNum,int X,int Y)
        {

            bool test = true;
        }*/

	}
}




