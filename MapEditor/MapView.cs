using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

using System.Reflection;
using NoxShared;
using NoxMapEditor;
using System.IO;
using System.Diagnostics;   
using System.Text.RegularExpressions;

namespace NoxMapEditor
{
    public class MapView : UserControl
    {
        public uint MapPanelID = 0;
        public Brush floorBrush = Brushes.Gray;
        public bool AutoVari = true;
        public int IdCount = 1;
        public int WidthMod = 0;
        public int winX = 0;
        public int winY = 0;
        public Map Map;
        public static int squareSize = 23;
        protected int objectSelectionRadius = 7;
        protected Button currentButton;
        public Map.Object SelectedObject = new Map.Object();
        public bool DrawGrid;
        public bool ShowObjectNames = false;
        public bool SnapGrid = false;
        public bool SnapHalfGrid = false;
        public bool WallCoors;
        public bool ShowWaypoints = true;
        public bool ShowPolygons = true;
        public bool ShowExtents = false;
        public bool ShowAllExtents = false;
        public bool DrawWalls3D = false;
        protected const int wallThickness = 2;
        protected const int gridThickness = 1;
        protected Map.Object DefaultObject = new Map.Object();
        protected PolygonDialog polyDlg = new PolygonDialog();
        private bool dragging;
        private Point wallDrag;
        private Point wallMouseLocation;
        private Point tileDrag;
        private Point mouseLocation;
        //string tempPath;
        public class KeyState
        {
            public /*IntPtr*/ Map.Waypoint wp1; // Waypoint holder
            public bool Shift;
            public bool MLeft;
            public bool MRight;
            public bool WheelClick;
            public int lastX;
            public int lastY;

            public void Reset()
            {
                wp1 = null;
                lastX = 0; // Last drag location X
                lastY = 0; // Last drag location Y
                Shift = false; // Shift state
                MLeft = false; // M1 State
                MRight = false; // M2 State
                WheelClick = false; // M3 State
            }
            public KeyState()
            {
                Reset(); // Reset all values to NULL
            }
        };
        public KeyState KeyStates = new KeyState();
        public class ColorLay
        {
            public Pen Tiles;
            public Pen Tiles2;
            public Brush Walls;
            public Color Background;
            public Pen Objects;

            public ColorLay()
            {
                ResetColors();
            }
            public void InvertColors()
            {
                Tiles2 = Pens.Blue;
                Tiles = Pens.Green;
                Walls = Brushes.Black;
                Background = Color.White;
                Objects = Pens.Red;
            }
            public void ResetColors()
            {
                Tiles2 = Pens.Yellow;
                Tiles = Pens.Gray;
                Walls = Brushes.White;
                Background = Color.Black;
                Objects = Pens.Blue;
            }

        };
        public class DirSettings
        {
            public bool SE;
            public bool SW;
            public bool NE;
            public bool NW;
            public bool E;
            public bool N;
            public bool W;
            public bool S;
            public DirSettings()
            {
                Clear();
            }
            public void Clear()
            {
                SE = false;
                SW = false;
                NE = false;
                NW = false;
                S = false;
                W = false;
                N = false;
                E = false;
            }

        };
        public ColorLay ColorLayout = new ColorLay();
        private bool OverrideChecking = false;
        public enum Mode
        {
            MAKE_WALL,
            MAKE_OBJECT,
            SELECT,
            MAKE_WINDOW,
            MAKE_DESTRUCTABLE,
            MAKE_SECRET,
            MAKE_FLOOR,
            MAKE_WAYPOINT,
            ADD_POLYGON_PT,
            EDIT_EDGE,
            EDIT_WAYPOINT,
            SELECT_WAYPOINT
        };
        public Mode CurrentMode;
        private MenuItem contextMenuDelete;
        private MenuItem contextMenuProperties;
        private MenuItem menuItem3;
        private StatusBarPanel statusWall;
        private StatusBarPanel statusTile;
        private StatusBarPanel statusObject;
        private StatusBar statusBar;
        private StatusBarPanel statusLocation;
        private Panel scrollPanel;//WARNING: the form designer is not happy with this
        private MapView.FlickerFreePanel mapPanel;
        private MenuItem contextMenuCopy;
        private MenuItem contextMenuExportDefault;
        private MenuItem contextMenuExportBinary;
        private MenuItem contextMenuPaste;

        //private Menu contextMenu;
        private ContextMenu contextMenu;
        private Timer tmrInvalidate;
        private System.ComponentModel.IContainer components;
        private GroupBox groupAdv;
        private CheckBox chkTileGrid;
        private CheckBox chkShowObj;
        private CheckBox chkGrid;
        private CheckBox checkBox2;
        private CheckBox chkAltGrid;
        private TabControl tabMapTools;
        private TabPage tabWalls;
        private GroupBox wallGroup;
        private CheckedListBox secretFlagsBoxes;
        private WallSelector wallSelector;
        private Button buttonSecret;
        private Button destructableButton;
        private Button windowsButton;
        private TabPage tabTiles;
        private GroupBox groupBox2;
        private CheckBox chkIgnoreOn;
        private Label label2;
        private Label label1;
        private ComboBox cboEdgeIgnoreBox;
        private ComboBox cboAutoEdge;
        private Button button1;
        private GroupBox floorGroup;
        private CheckBox checkBox1;
        private CheckBox threeFloorBox;
        private CheckBox chkAutoEdge;
        private Button buttonBlend;
        private ComboBox tileVar;
        private ComboBox tileGraphic;
        private Button floorButton;
        private TabPage tabObjects;
        private GroupBox objectGroup;
        public ComboBox cboObjCreate;
        private Button selectButton;
        private Button newObjectButton;
        private TabPage tabPoly;
        private GroupBox wpGroup;
        private Button editWpButton;
        private TextBox wpNameBox;
        private Button wpAddButton;
        private GroupBox groupPolygons;
        private Button buttonPolygonDelete;
        private Button buttonPolygonNew;
        private Button buttonEditPolygon;
        private Button buttonPoints;
        private ComboBox listPolygons;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private Label label3;
        private Label label4;
        private TextBox txtWpDefault;
        private Button buttonSelWay;
        private CheckBox checkBox3;
        private RadioButton radFullSnap;
        private RadioButton radCenterSnap;
        private RadioButton radNoSnap;
        private GroupBox groupBox1;
        private RadioButton radioButton1;
        private RadioButton radioButton3;
        private RadioButton radioButton2;
        private CheckBox checkBox5;
        private CheckBox checkBox4;
        private CheckBox checkBox6;
        //private VideoBag video = null;

        event System.Windows.Forms.KeyEventHandler DeletePressed;

        //16bit windows needs the deprectated version of DoubleBufferring to work. Otherwise it crashes :\
        protected class FlickerFreePanel : Panel 
        { 
            public FlickerFreePanel() : base() 
            {SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.Opaque, true);} 
        // set styles to reduce flicker and painting over twice
/*
 * had just double buffer
 * Setstyle(Controlstyles.AllPaintingInWmPaint, true);
Setstyle(Controlstyles.UserPaint, true);
Setstyle(Controlstyles.OptimizedDoubleBuffer, true); */
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapView));
            this.contextMenuDelete = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.contextMenuProperties = new System.Windows.Forms.MenuItem();
            this.contextMenuExportDefault = new System.Windows.Forms.MenuItem();
            this.contextMenuExportBinary = new System.Windows.Forms.MenuItem();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.statusLocation = new System.Windows.Forms.StatusBarPanel();
            this.statusWall = new System.Windows.Forms.StatusBarPanel();
            this.statusTile = new System.Windows.Forms.StatusBarPanel();
            this.statusObject = new System.Windows.Forms.StatusBarPanel();
            this.groupAdv = new System.Windows.Forms.GroupBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.chkAltGrid = new System.Windows.Forms.CheckBox();
            this.tabMapTools = new System.Windows.Forms.TabControl();
            this.tabWalls = new System.Windows.Forms.TabPage();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.wallGroup = new System.Windows.Forms.GroupBox();
            this.secretFlagsBoxes = new System.Windows.Forms.CheckedListBox();
            this.wallSelector = new NoxMapEditor.WallSelector();
            this.buttonSecret = new System.Windows.Forms.Button();
            this.destructableButton = new System.Windows.Forms.Button();
            this.windowsButton = new System.Windows.Forms.Button();
            this.tabTiles = new System.Windows.Forms.TabPage();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkIgnoreOn = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboEdgeIgnoreBox = new System.Windows.Forms.ComboBox();
            this.cboAutoEdge = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.floorGroup = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.threeFloorBox = new System.Windows.Forms.CheckBox();
            this.chkAutoEdge = new System.Windows.Forms.CheckBox();
            this.buttonBlend = new System.Windows.Forms.Button();
            this.tileVar = new System.Windows.Forms.ComboBox();
            this.tileGraphic = new System.Windows.Forms.ComboBox();
            this.floorButton = new System.Windows.Forms.Button();
            this.tabObjects = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radFullSnap = new System.Windows.Forms.RadioButton();
            this.radCenterSnap = new System.Windows.Forms.RadioButton();
            this.radNoSnap = new System.Windows.Forms.RadioButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.objectGroup = new System.Windows.Forms.GroupBox();
            this.cboObjCreate = new System.Windows.Forms.ComboBox();
            this.selectButton = new System.Windows.Forms.Button();
            this.newObjectButton = new System.Windows.Forms.Button();
            this.tabPoly = new System.Windows.Forms.TabPage();
            this.wpGroup = new System.Windows.Forms.GroupBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.buttonSelWay = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtWpDefault = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.editWpButton = new System.Windows.Forms.Button();
            this.wpNameBox = new System.Windows.Forms.TextBox();
            this.wpAddButton = new System.Windows.Forms.Button();
            this.groupPolygons = new System.Windows.Forms.GroupBox();
            this.buttonPolygonDelete = new System.Windows.Forms.Button();
            this.buttonPolygonNew = new System.Windows.Forms.Button();
            this.buttonEditPolygon = new System.Windows.Forms.Button();
            this.buttonPoints = new System.Windows.Forms.Button();
            this.listPolygons = new System.Windows.Forms.ComboBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.chkShowObj = new System.Windows.Forms.CheckBox();
            this.chkGrid = new System.Windows.Forms.CheckBox();
            this.chkTileGrid = new System.Windows.Forms.CheckBox();
            this.scrollPanel = new System.Windows.Forms.Panel();
            this.mapPanel = new NoxMapEditor.MapView.FlickerFreePanel();
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.contextMenuCopy = new System.Windows.Forms.MenuItem();
            this.contextMenuPaste = new System.Windows.Forms.MenuItem();
            this.tmrInvalidate = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.statusLocation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusWall)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusTile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusObject)).BeginInit();
            this.groupAdv.SuspendLayout();
            this.tabMapTools.SuspendLayout();
            this.tabWalls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.wallGroup.SuspendLayout();
            this.tabTiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.floorGroup.SuspendLayout();
            this.tabObjects.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.objectGroup.SuspendLayout();
            this.tabPoly.SuspendLayout();
            this.wpGroup.SuspendLayout();
            this.groupPolygons.SuspendLayout();
            this.scrollPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuDelete
            // 
            this.contextMenuDelete.Index = 2;
            resources.ApplyResources(this.contextMenuDelete, "contextMenuDelete");
            this.contextMenuDelete.Click += new System.EventHandler(this.contextMenuDelete_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 3;
            resources.ApplyResources(this.menuItem3, "menuItem3");
            // 
            // contextMenuProperties
            // 
            this.contextMenuProperties.Index = 4;
            resources.ApplyResources(this.contextMenuProperties, "contextMenuProperties");
            this.contextMenuProperties.Click += new System.EventHandler(this.contextMenuProperties_Click);
            // 
            // contextMenuExportDefault
            // 
            this.contextMenuExportDefault.Index = 5;
            resources.ApplyResources(this.contextMenuExportDefault, "contextMenuExportDefault");
            this.contextMenuExportDefault.Click += new System.EventHandler(this.contextMenuExportDefault_Click);
            // 
            // contextMenuExportBinary
            // 
            this.contextMenuExportBinary.Index = 6;
            resources.ApplyResources(this.contextMenuExportBinary, "contextMenuExportBinary");
            this.contextMenuExportBinary.Click += new System.EventHandler(this.contextMenuExportBinary_Click);
            // 
            // statusBar
            // 
            resources.ApplyResources(this.statusBar, "statusBar");
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusLocation,
            this.statusWall,
            this.statusTile,
            this.statusObject});
            this.statusBar.ShowPanels = true;
            this.statusBar.SizingGrip = false;
            // 
            // statusLocation
            // 
            resources.ApplyResources(this.statusLocation, "statusLocation");
            // 
            // statusWall
            // 
            this.statusWall.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            resources.ApplyResources(this.statusWall, "statusWall");
            // 
            // statusTile
            // 
            this.statusTile.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            resources.ApplyResources(this.statusTile, "statusTile");
            // 
            // statusObject
            // 
            this.statusObject.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            resources.ApplyResources(this.statusObject, "statusObject");
            // 
            // groupAdv
            // 
            resources.ApplyResources(this.groupAdv, "groupAdv");
            this.groupAdv.Controls.Add(this.checkBox6);
            this.groupAdv.Controls.Add(this.checkBox5);
            this.groupAdv.Controls.Add(this.checkBox4);
            this.groupAdv.Controls.Add(this.chkAltGrid);
            this.groupAdv.Controls.Add(this.tabMapTools);
            this.groupAdv.Controls.Add(this.checkBox2);
            this.groupAdv.Controls.Add(this.chkShowObj);
            this.groupAdv.Controls.Add(this.chkGrid);
            this.groupAdv.Controls.Add(this.chkTileGrid);
            this.groupAdv.Name = "groupAdv";
            this.groupAdv.TabStop = false;
            // 
            // checkBox6
            // 
            resources.ApplyResources(this.checkBox6, "checkBox6");
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.UseVisualStyleBackColor = true;
            this.checkBox6.CheckedChanged += new System.EventHandler(this.checkBox6_CheckedChanged);
            // 
            // checkBox5
            // 
            resources.ApplyResources(this.checkBox5, "checkBox5");
            this.checkBox5.Checked = true;
            this.checkBox5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
            // 
            // checkBox4
            // 
            resources.ApplyResources(this.checkBox4, "checkBox4");
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // chkAltGrid
            // 
            resources.ApplyResources(this.chkAltGrid, "chkAltGrid");
            this.chkAltGrid.Name = "chkAltGrid";
            this.chkAltGrid.UseVisualStyleBackColor = true;
            // 
            // tabMapTools
            // 
            this.tabMapTools.Controls.Add(this.tabWalls);
            this.tabMapTools.Controls.Add(this.tabTiles);
            this.tabMapTools.Controls.Add(this.tabObjects);
            this.tabMapTools.Controls.Add(this.tabPoly);
            resources.ApplyResources(this.tabMapTools, "tabMapTools");
            this.tabMapTools.Name = "tabMapTools";
            this.tabMapTools.SelectedIndex = 0;
            // 
            // tabWalls
            // 
            this.tabWalls.BackColor = System.Drawing.Color.Silver;
            this.tabWalls.Controls.Add(this.pictureBox2);
            this.tabWalls.Controls.Add(this.wallGroup);
            resources.ApplyResources(this.tabWalls, "tabWalls");
            this.tabWalls.Name = "tabWalls";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Black;
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.pictureBox2, "pictureBox2");
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.TabStop = false;
            // 
            // wallGroup
            // 
            this.wallGroup.Controls.Add(this.secretFlagsBoxes);
            this.wallGroup.Controls.Add(this.wallSelector);
            this.wallGroup.Controls.Add(this.buttonSecret);
            this.wallGroup.Controls.Add(this.destructableButton);
            this.wallGroup.Controls.Add(this.windowsButton);
            resources.ApplyResources(this.wallGroup, "wallGroup");
            this.wallGroup.Name = "wallGroup";
            this.wallGroup.TabStop = false;
            // 
            // secretFlagsBoxes
            // 
            this.secretFlagsBoxes.FormattingEnabled = true;
            resources.ApplyResources(this.secretFlagsBoxes, "secretFlagsBoxes");
            this.secretFlagsBoxes.Name = "secretFlagsBoxes";
            // 
            // wallSelector
            // 
            resources.ApplyResources(this.wallSelector, "wallSelector");
            this.wallSelector.Name = "wallSelector";
            this.wallSelector.Load += new System.EventHandler(this.wallSelector_Load);
            // 
            // buttonSecret
            // 
            resources.ApplyResources(this.buttonSecret, "buttonSecret");
            this.buttonSecret.Name = "buttonSecret";
            this.buttonSecret.Click += new System.EventHandler(this.buttonSecret_Click);
            // 
            // destructableButton
            // 
            resources.ApplyResources(this.destructableButton, "destructableButton");
            this.destructableButton.Name = "destructableButton";
            this.destructableButton.Click += new System.EventHandler(this.destructableButton_Click);
            // 
            // windowsButton
            // 
            resources.ApplyResources(this.windowsButton, "windowsButton");
            this.windowsButton.Name = "windowsButton";
            this.windowsButton.Click += new System.EventHandler(this.windowsButton_Click);
            // 
            // tabTiles
            // 
            this.tabTiles.BackColor = System.Drawing.Color.Silver;
            this.tabTiles.Controls.Add(this.pictureBox3);
            this.tabTiles.Controls.Add(this.groupBox2);
            this.tabTiles.Controls.Add(this.floorGroup);
            resources.ApplyResources(this.tabTiles, "tabTiles");
            this.tabTiles.Name = "tabTiles";
            this.tabTiles.UseVisualStyleBackColor = true;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Black;
            this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.pictureBox3, "pictureBox3");
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkIgnoreOn);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cboEdgeIgnoreBox);
            this.groupBox2.Controls.Add(this.cboAutoEdge);
            this.groupBox2.Controls.Add(this.button1);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // chkIgnoreOn
            // 
            resources.ApplyResources(this.chkIgnoreOn, "chkIgnoreOn");
            this.chkIgnoreOn.Name = "chkIgnoreOn";
            this.chkIgnoreOn.UseVisualStyleBackColor = true;
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
            // cboEdgeIgnoreBox
            // 
            this.cboEdgeIgnoreBox.FormattingEnabled = true;
            resources.ApplyResources(this.cboEdgeIgnoreBox, "cboEdgeIgnoreBox");
            this.cboEdgeIgnoreBox.Name = "cboEdgeIgnoreBox";
            this.cboEdgeIgnoreBox.Sorted = true;
            // 
            // cboAutoEdge
            // 
            this.cboAutoEdge.FormattingEnabled = true;
            resources.ApplyResources(this.cboAutoEdge, "cboAutoEdge");
            this.cboAutoEdge.Name = "cboAutoEdge";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // floorGroup
            // 
            this.floorGroup.BackColor = System.Drawing.Color.Silver;
            this.floorGroup.Controls.Add(this.checkBox1);
            this.floorGroup.Controls.Add(this.threeFloorBox);
            this.floorGroup.Controls.Add(this.chkAutoEdge);
            this.floorGroup.Controls.Add(this.buttonBlend);
            this.floorGroup.Controls.Add(this.tileVar);
            this.floorGroup.Controls.Add(this.tileGraphic);
            this.floorGroup.Controls.Add(this.floorButton);
            resources.ApplyResources(this.floorGroup, "floorGroup");
            this.floorGroup.Name = "floorGroup";
            this.floorGroup.TabStop = false;
            // 
            // checkBox1
            // 
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // threeFloorBox
            // 
            resources.ApplyResources(this.threeFloorBox, "threeFloorBox");
            this.threeFloorBox.Name = "threeFloorBox";
            // 
            // chkAutoEdge
            // 
            resources.ApplyResources(this.chkAutoEdge, "chkAutoEdge");
            this.chkAutoEdge.Name = "chkAutoEdge";
            this.chkAutoEdge.UseVisualStyleBackColor = true;
            // 
            // buttonBlend
            // 
            resources.ApplyResources(this.buttonBlend, "buttonBlend");
            this.buttonBlend.Name = "buttonBlend";
            this.buttonBlend.Click += new System.EventHandler(this.buttonBlend_Click);
            // 
            // tileVar
            // 
            this.tileVar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tileVar.DropDownWidth = 40;
            this.tileVar.FormattingEnabled = true;
            resources.ApplyResources(this.tileVar, "tileVar");
            this.tileVar.Name = "tileVar";
            // 
            // tileGraphic
            // 
            this.tileGraphic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tileGraphic.DropDownWidth = 180;
            this.tileGraphic.FormattingEnabled = true;
            resources.ApplyResources(this.tileGraphic, "tileGraphic");
            this.tileGraphic.Name = "tileGraphic";
            this.tileGraphic.Sorted = true;
            this.tileGraphic.SelectedIndexChanged += new System.EventHandler(this.tileGraphic_SelectedIndexChanged);
            // 
            // floorButton
            // 
            resources.ApplyResources(this.floorButton, "floorButton");
            this.floorButton.Name = "floorButton";
            this.floorButton.Click += new System.EventHandler(this.floorButton_Click);
            // 
            // tabObjects
            // 
            this.tabObjects.BackColor = System.Drawing.Color.Silver;
            this.tabObjects.Controls.Add(this.groupBox1);
            this.tabObjects.Controls.Add(this.radFullSnap);
            this.tabObjects.Controls.Add(this.radCenterSnap);
            this.tabObjects.Controls.Add(this.radNoSnap);
            this.tabObjects.Controls.Add(this.pictureBox1);
            this.tabObjects.Controls.Add(this.objectGroup);
            resources.ApplyResources(this.tabObjects, "tabObjects");
            this.tabObjects.Name = "tabObjects";
            this.tabObjects.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // radioButton3
            // 
            resources.ApplyResources(this.radioButton3, "radioButton3");
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioButton2
            // 
            resources.ApplyResources(this.radioButton2, "radioButton2");
            this.radioButton2.Checked = true;
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.TabStop = true;
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            resources.ApplyResources(this.radioButton1, "radioButton1");
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radFullSnap
            // 
            resources.ApplyResources(this.radFullSnap, "radFullSnap");
            this.radFullSnap.Name = "radFullSnap";
            this.radFullSnap.UseVisualStyleBackColor = true;
            this.radFullSnap.CheckedChanged += new System.EventHandler(this.radFullSnap_CheckedChanged);
            // 
            // radCenterSnap
            // 
            resources.ApplyResources(this.radCenterSnap, "radCenterSnap");
            this.radCenterSnap.Name = "radCenterSnap";
            this.radCenterSnap.UseVisualStyleBackColor = true;
            this.radCenterSnap.CheckedChanged += new System.EventHandler(this.radCenterSnap_CheckedChanged);
            // 
            // radNoSnap
            // 
            resources.ApplyResources(this.radNoSnap, "radNoSnap");
            this.radNoSnap.Checked = true;
            this.radNoSnap.Name = "radNoSnap";
            this.radNoSnap.TabStop = true;
            this.radNoSnap.UseVisualStyleBackColor = true;
            this.radNoSnap.CheckedChanged += new System.EventHandler(this.radNoSnap_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // objectGroup
            // 
            this.objectGroup.Controls.Add(this.cboObjCreate);
            this.objectGroup.Controls.Add(this.selectButton);
            this.objectGroup.Controls.Add(this.newObjectButton);
            resources.ApplyResources(this.objectGroup, "objectGroup");
            this.objectGroup.Name = "objectGroup";
            this.objectGroup.TabStop = false;
            // 
            // cboObjCreate
            // 
            this.cboObjCreate.FormattingEnabled = true;
            resources.ApplyResources(this.cboObjCreate, "cboObjCreate");
            this.cboObjCreate.Name = "cboObjCreate";
            this.cboObjCreate.Sorted = true;
            this.cboObjCreate.SelectedIndexChanged += new System.EventHandler(this.cboObjCreate_SelectedIndexChanged);
            // 
            // selectButton
            // 
            resources.ApplyResources(this.selectButton, "selectButton");
            this.selectButton.Name = "selectButton";
            this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
            // 
            // newObjectButton
            // 
            resources.ApplyResources(this.newObjectButton, "newObjectButton");
            this.newObjectButton.Name = "newObjectButton";
            this.newObjectButton.Click += new System.EventHandler(this.newObjectButton_Click);
            // 
            // tabPoly
            // 
            this.tabPoly.BackColor = System.Drawing.Color.Silver;
            this.tabPoly.Controls.Add(this.wpGroup);
            this.tabPoly.Controls.Add(this.groupPolygons);
            resources.ApplyResources(this.tabPoly, "tabPoly");
            this.tabPoly.Name = "tabPoly";
            // 
            // wpGroup
            // 
            this.wpGroup.Controls.Add(this.checkBox3);
            this.wpGroup.Controls.Add(this.buttonSelWay);
            this.wpGroup.Controls.Add(this.label4);
            this.wpGroup.Controls.Add(this.txtWpDefault);
            this.wpGroup.Controls.Add(this.label3);
            this.wpGroup.Controls.Add(this.editWpButton);
            this.wpGroup.Controls.Add(this.wpNameBox);
            this.wpGroup.Controls.Add(this.wpAddButton);
            resources.ApplyResources(this.wpGroup, "wpGroup");
            this.wpGroup.Name = "wpGroup";
            this.wpGroup.TabStop = false;
            // 
            // checkBox3
            // 
            resources.ApplyResources(this.checkBox3, "checkBox3");
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // buttonSelWay
            // 
            resources.ApplyResources(this.buttonSelWay, "buttonSelWay");
            this.buttonSelWay.Name = "buttonSelWay";
            this.buttonSelWay.UseVisualStyleBackColor = true;
            this.buttonSelWay.Click += new System.EventHandler(this.buttonSelWay_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // txtWpDefault
            // 
            resources.ApplyResources(this.txtWpDefault, "txtWpDefault");
            this.txtWpDefault.Name = "txtWpDefault";
            this.txtWpDefault.TextChanged += new System.EventHandler(this.txtWpDefault_TextChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // editWpButton
            // 
            resources.ApplyResources(this.editWpButton, "editWpButton");
            this.editWpButton.Name = "editWpButton";
            this.editWpButton.Click += new System.EventHandler(this.renWpButton_Click);
            // 
            // wpNameBox
            // 
            resources.ApplyResources(this.wpNameBox, "wpNameBox");
            this.wpNameBox.Name = "wpNameBox";
            this.wpNameBox.TextChanged += new System.EventHandler(this.wpNameBox_TextChanged);
            // 
            // wpAddButton
            // 
            resources.ApplyResources(this.wpAddButton, "wpAddButton");
            this.wpAddButton.Name = "wpAddButton";
            this.wpAddButton.Click += new System.EventHandler(this.wpAddButton_Click);
            // 
            // groupPolygons
            // 
            this.groupPolygons.Controls.Add(this.buttonPolygonDelete);
            this.groupPolygons.Controls.Add(this.buttonPolygonNew);
            this.groupPolygons.Controls.Add(this.buttonEditPolygon);
            this.groupPolygons.Controls.Add(this.buttonPoints);
            this.groupPolygons.Controls.Add(this.listPolygons);
            resources.ApplyResources(this.groupPolygons, "groupPolygons");
            this.groupPolygons.Name = "groupPolygons";
            this.groupPolygons.TabStop = false;
            // 
            // buttonPolygonDelete
            // 
            resources.ApplyResources(this.buttonPolygonDelete, "buttonPolygonDelete");
            this.buttonPolygonDelete.Name = "buttonPolygonDelete";
            this.buttonPolygonDelete.Click += new System.EventHandler(this.buttonPolygonDelete_Click);
            // 
            // buttonPolygonNew
            // 
            resources.ApplyResources(this.buttonPolygonNew, "buttonPolygonNew");
            this.buttonPolygonNew.Name = "buttonPolygonNew";
            this.buttonPolygonNew.Click += new System.EventHandler(this.buttonPolygonNew_Click);
            // 
            // buttonEditPolygon
            // 
            resources.ApplyResources(this.buttonEditPolygon, "buttonEditPolygon");
            this.buttonEditPolygon.Name = "buttonEditPolygon";
            this.buttonEditPolygon.Click += new System.EventHandler(this.buttonEditPolygon_Click);
            // 
            // buttonPoints
            // 
            resources.ApplyResources(this.buttonPoints, "buttonPoints");
            this.buttonPoints.Name = "buttonPoints";
            this.buttonPoints.Click += new System.EventHandler(this.buttonPoints_Click);
            // 
            // listPolygons
            // 
            this.listPolygons.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.listPolygons.DropDownWidth = 120;
            this.listPolygons.FormattingEnabled = true;
            resources.ApplyResources(this.listPolygons, "listPolygons");
            this.listPolygons.Name = "listPolygons";
            this.listPolygons.Click += new System.EventHandler(this.listPolygons_Click);
            // 
            // checkBox2
            // 
            resources.ApplyResources(this.checkBox2, "checkBox2");
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // chkShowObj
            // 
            resources.ApplyResources(this.chkShowObj, "chkShowObj");
            this.chkShowObj.Checked = true;
            this.chkShowObj.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowObj.Name = "chkShowObj";
            this.chkShowObj.UseVisualStyleBackColor = true;
            // 
            // chkGrid
            // 
            resources.ApplyResources(this.chkGrid, "chkGrid");
            this.chkGrid.Name = "chkGrid";
            this.chkGrid.UseVisualStyleBackColor = true;
            // 
            // chkTileGrid
            // 
            resources.ApplyResources(this.chkTileGrid, "chkTileGrid");
            this.chkTileGrid.Name = "chkTileGrid";
            this.chkTileGrid.UseVisualStyleBackColor = true;
            this.chkTileGrid.CheckedChanged += new System.EventHandler(this.chkTileGrid_CheckedChanged);
            // 
            // scrollPanel
            // 
            resources.ApplyResources(this.scrollPanel, "scrollPanel");
            this.scrollPanel.Controls.Add(this.mapPanel);
            this.scrollPanel.Name = "scrollPanel";
            this.scrollPanel.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrollPanel_Scroll);
            // 
            // mapPanel
            // 
            resources.ApplyResources(this.mapPanel, "mapPanel");
            this.mapPanel.Name = "mapPanel";
            this.mapPanel.MouseLeave += new System.EventHandler(this.mapPanel_MouseLeave);
            this.mapPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.mapPanel_Paint);
            this.mapPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mapPanel_MouseMove);
            this.mapPanel.Leave += new System.EventHandler(this.mapPanel_Leave);
            this.mapPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mapPanel_MouseDown);
            this.mapPanel.Resize += new System.EventHandler(this.mapPanel_Resize);
            this.mapPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mapPanel_MouseUp);
            // 
            // contextMenu
            // 
            this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.contextMenuCopy,
            this.contextMenuPaste,
            this.contextMenuDelete,
            this.menuItem3,
            this.contextMenuProperties,
            this.contextMenuExportDefault,
            this.contextMenuExportBinary});
            // 
            // contextMenuCopy
            // 
            this.contextMenuCopy.Index = 0;
            resources.ApplyResources(this.contextMenuCopy, "contextMenuCopy");
            this.contextMenuCopy.Click += new System.EventHandler(this.contextMenuCopy_Click);
            // 
            // contextMenuPaste
            // 
            this.contextMenuPaste.Index = 1;
            resources.ApplyResources(this.contextMenuPaste, "contextMenuPaste");
            this.contextMenuPaste.Click += new System.EventHandler(this.contextMenuPaste_Click);
            // 
            // tmrInvalidate
            // 
            this.tmrInvalidate.Enabled = true;
            this.tmrInvalidate.Tick += new System.EventHandler(this.tmrInvalidate_Tick);
            // 
            // MapView
            // 
            this.Controls.Add(this.groupAdv);
            this.Controls.Add(this.scrollPanel);
            this.Controls.Add(this.statusBar);
            this.Name = "MapView";
            resources.ApplyResources(this, "$this");
            ((System.ComponentModel.ISupportInitialize)(this.statusLocation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusWall)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusTile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusObject)).EndInit();
            this.groupAdv.ResumeLayout(false);
            this.groupAdv.PerformLayout();
            this.tabMapTools.ResumeLayout(false);
            this.tabWalls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.wallGroup.ResumeLayout(false);
            this.tabTiles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.floorGroup.ResumeLayout(false);
            this.floorGroup.PerformLayout();
            this.tabObjects.ResumeLayout(false);
            this.tabObjects.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.objectGroup.ResumeLayout(false);
            this.tabPoly.ResumeLayout(false);
            this.wpGroup.ResumeLayout(false);
            this.wpGroup.PerformLayout();
            this.groupPolygons.ResumeLayout(false);
            this.scrollPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        public MapView()
        {
            InitializeComponent();
            MapPanelID = (uint)mapPanel.Handle.ToInt32();
            string[] cols = Enum.GetNames(typeof(tilecolors));
            int count = 0;
            Array vals = Enum.GetValues(typeof(tilecolors));
            foreach (ThingDb.Tile til in ThingDb.FloorTiles)
            {
               count = 0;
               foreach (string str in cols)
               {
                  if (str == til.Name)
                  {
                    til.col = Color.FromArgb((int)((uint)vals.GetValue(count)));
                    til.hascolor = true;
                    break;
                  }
                  count++;
               }
            }


            WidthMod = groupAdv.Width;//groupBox1.Width;
            foreach (string s in ThingDb.Things.Keys)
                cboObjCreate.Items.Add(s);
            cboObjCreate.SelectedIndex = 0;

            DeletePressed += new KeyEventHandler(MapView_DeletePressed);
            wallSelector.Mapview = this;
            wallSelector.picBox = this.pictureBox2;

            tileGraphic.Items.AddRange(ThingDb.FloorTiles.ToArray());
            //set default values
            tileGraphic.SelectedIndex = 0;
            tileVar.SelectedIndex = 0;

            cboEdgeIgnoreBox.Items.AddRange(ThingDb.FloorTiles.ToArray());
            //set default values
            cboEdgeIgnoreBox.SelectedIndex = 0;

            cboAutoEdge.Items.AddRange(ThingDb.EdgeTiles.ToArray());
            cboAutoEdge.SelectedIndex = 0;

            Map = new Map();//dummy map
            currentButton = selectButton;
            SetMode(Mode.SELECT);
            //CurrentMode = Mode.SELECT;

            secretFlagsBoxes.Items.AddRange(Enum.GetNames(typeof(Map.Wall.SecretFlags)));

            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Westwood\\Nox");
            if (key == null)
            {
                MessageBox.Show("Can not find the Nox directory in the registry. You can try reinstalling Nox to fix this.", "Error");
                Environment.Exit(1);
            }
        }

        ~MapView()
        {
            //System.IO.File.Delete(tempPath);
        }
        private void mapPanel_MouseDown(object sender, MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            Point tilePt = GetNearestTilePoint(pt);
            Point wallPt = GetNearestWallPoint(pt);
           // if (CurrentMode != Mode.MAKE_WAYPOINT && CurrentMode != Mode.EDIT_WAYPOINT)
            //    KeyStates.wp1 = null;

            if (e.Button.Equals(MouseButtons.Middle)/* && e.Clicks == 1*/)//if middle click
            {
                KeyStates.WheelClick = true;
                // KeyStates.lastX = mouseLocation.X;
                // KeyStates.lastY = mouseLocation.Y;
                if (CurrentMode == Mode.MAKE_FLOOR || CurrentMode == Mode.EDIT_EDGE)
                {
                    threeFloorBox.Checked = !threeFloorBox.Checked;
                }
               // else if (CurrentMode == Mode.EDIT_WAYPOINT)
                //{
                 //   Map.Waypoints.SetNameFromPoint(pt, wpNameBox.Text);
               // }
            }
            if (e.Button.Equals(MouseButtons.Right)/* && e.Clicks == 1*/)//if single right click
            {
                KeyStates.MRight = true;

                // Add context menu popup    
                if( SelectedObject != null && (CurrentMode == Mode.MAKE_OBJECT || CurrentMode == Mode.SELECT))
                    contextMenu.Show(mapPanel,mouseLocation);
                if( CurrentMode == Mode.MAKE_FLOOR )
                    RemoveTile(tilePt);
                else if (CurrentMode == Mode.EDIT_EDGE)
                {
                    RemoveEdge(tilePt);
                }
                else if (CurrentMode == Mode.MAKE_WAYPOINT)
                {                   
                    Map.Waypoint wp = Map.Waypoints.GetWPFromPoint(pt);
                    if (wp != null)
                    {
                        Map.Waypoints.Remove(wp);
                    }
                    //KeyStates.wp1 = null;
                }
                else if (CurrentMode == Mode.EDIT_WAYPOINT)
                {
                    Map.Waypoint wp = Map.Waypoints.GetWPFromPoint(pt);
                    if (wp != null && KeyStates.wp1 != null && KeyStates.wp1 != wp)
                    {
                        //wp.RemoveConnByNum(KeyStates.wp1);
                        KeyStates.wp1.RemoveConnByNum(wp);
                        KeyStates.wp1 = null;
                    }
                    else
                    {
                        KeyStates.wp1 = wp;
                    }
                }
            }
            if (e.Button.Equals(MouseButtons.Left)/* && e.Clicks == 1*/)//if single left click
            {
                KeyStates.MLeft = true;
                if (CurrentMode == Mode.SELECT)
                {
                    //dragging = Map.SelectObject(pointClicked) == SelectedObject;//only start "dragging" if this object has already been selected
                    if (SelectObject(pt) == SelectedObject)
                        dragging = true;
                    else
                    {
                        SelectedObject = SelectObject(pt);
                        KeyStates.MLeft = false;
                    }
                }
                else if (CurrentMode == Mode.EDIT_EDGE)
                {
                    EditEdge(tilePt);
                }
                else if (CurrentMode == Mode.MAKE_WALL)
                {
                    if (!Map.Walls.ContainsKey(wallPt))
                    {
                        Map.Walls.Add(wallPt, wallSelector.NewWall(wallPt));
                        Map.Wall.WallFacing facing = Map.Walls[wallPt].Facing;
                        if (
                            facing == Map.Wall.WallFacing.NORTH ||
                            facing == Map.Wall.WallFacing.WEST)
                        {
                            Map.Walls[wallPt].Variation = CreateVariationW(wallPt, (ushort)Map.Walls[wallPt].Variation, (ushort)Map.Walls[wallPt].Variations, true);
                        }
                        else
                        {
                            Map.Walls[wallPt].Variation = CreateVariationW(wallPt, (ushort)Map.Walls[wallPt].Variation, (ushort)Map.Walls[wallPt].Variations, false);
                        }
                        wallDrag = wallPt;
                        dragging = true;
                    }
                    else
                        Map.Walls.Remove(wallPt);
                }
                else if (CurrentMode == Mode.MAKE_OBJECT)
                {
                    string m_ExePath = Process.GetCurrentProcess().MainModule.FileName;
                    m_ExePath = Path.GetDirectoryName(m_ExePath);
                    Map.Object obj = (Map.Object)DefaultObject.Clone();

                    obj.Name = cboObjCreate.Items[cboObjCreate.SelectedIndex].ToString();
                    //boxMod.Clear();
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
                    //boxMod.Text = mod;

                    if (mod.Length > 0)
                    {
                        MemoryStream stream = new MemoryStream();
                        BinaryWriter wtr = new BinaryWriter(stream);
                        Regex bytes = new Regex("[0-9|a-f|A-F]{2}");
                        foreach (Match match in bytes.Matches(mod))
                            wtr.Write(Convert.ToByte(match.Value, 16));
                        obj.modbuf = stream.ToArray();
                    }
                    else
                        obj.modbuf = new byte[0];
                    //}
                    if (SnapGrid)//snap to grid when grid is on
                        pt = new Point((int)Math.Round((decimal)(pt.X / squareSize)) * squareSize, (int)Math.Round((decimal)(pt.Y / squareSize)) * squareSize);

                    if (SnapHalfGrid)//snap to half grid when grid is on
                        pt = new Point((int)Math.Round((decimal)((pt.X / (squareSize)) * squareSize) + squareSize / 2), (int)Math.Round((decimal)((pt.Y / (squareSize)) * squareSize) + squareSize / 2));

                    obj.Location = pt;
                    obj.Extent = DefaultObject.Extent + 1;
                    obj.UniqueID = IdCount++;
                    while (Map.Objects.extents.Contains(obj.Extent))
                        obj.Extent++;
                    Map.Objects.Add(obj);
                    unsafe
                    {
                        IntPtr ptr = Marshal.StringToHGlobalAnsi(obj.Name);
                        int val = obj.UniqueID;
                        if ((ThingDb.Things[obj.Name].Class & ThingDb.Thing.ClassFlags.DOOR) == ThingDb.Thing.ClassFlags.DOOR)
                        {
                            MainWindow.myMap.AddObject(ptr.ToPointer(), (int)obj.Location.X, (int)obj.Location.Y, val,obj.modbuf[0]);
                        }
                        else
                            MainWindow.myMap.AddObject(ptr.ToPointer(), (int)obj.Location.X, (int)obj.Location.Y, val,-1);
                    }
                }
                else if (CurrentMode == Mode.MAKE_WINDOW)
                {
                    if (Map.Walls.ContainsKey(wallPt))
                        Map.Walls[wallPt].Window = !Map.Walls[wallPt].Window;
                }
                else if (CurrentMode == Mode.MAKE_DESTRUCTABLE)
                {
                    if (Map.Walls.ContainsKey(wallPt))
                        Map.Walls[wallPt].Destructable = !Map.Walls[wallPt].Destructable;
                }

                else if (CurrentMode == Mode.MAKE_SECRET)
                {
                    if (Map.Walls.ContainsKey(wallPt))
                    {
                        Map.Walls[wallPt].Secret = !Map.Walls[wallPt].Secret;
                        Map.Walls[wallPt].Secret_flags = (uint)Map.Wall.SecretFlags.Unknown100;
                        foreach (String s in secretFlagsBoxes.CheckedItems)
                            Map.Walls[wallPt].Secret_flags = Map.Walls[wallPt].Secret_flags | (uint)Enum.Parse(typeof(Map.Wall.SecretFlags), s);
                    }
                }
                else if (CurrentMode == Mode.MAKE_WAYPOINT)
                {
                    int i;
                    for (i = 1; Map.Waypoints.num_wp.ContainsKey(i); i++) ;
                    Map.Waypoint wp = new Map.Waypoint(wpNameBox.Text, pt, i);
                    wp.enabled = 1;
                    Map.Waypoints.Add(wp);
                    Map.Waypoints.num_wp.Add(i, wp);
                    //if (chkAutoLink1.Checked && KeyStates.wp1 != null)
                    //{
                    //    wp.AddConnByNum(KeyStates.wp1, Convert.ToByte(txtWpDefault.Text));
                    //}
                    //if (chkAutoLink2.Checked && KeyStates.wp1 != null)
                    //{
                    //    KeyStates.wp1.AddConnByNum(wp, Convert.ToByte(txtWpDefault.Text));
                    //}
                    //KeyStates.wp1 = wp;
                }
                else if (CurrentMode == Mode.EDIT_WAYPOINT)
                {
                    Map.Waypoint wp = Map.Waypoints.GetWPFromPoint(pt);
                    if (wp != null && KeyStates.wp1 != null && wp != KeyStates.wp1)
                    {
                        //wp.AddConnByNum(KeyStates.wp1,Map.Waypoint.WaypointConnection.DefaultFlag);
                        KeyStates.wp1.AddConnByNum(wp, Convert.ToByte(txtWpDefault.Text));
                        KeyStates.wp1 = null;
                    }
                    else
                    {
                        KeyStates.wp1 = wp;
                    }
                }
                else if (CurrentMode == Mode.SELECT_WAYPOINT)
                {
                    Map.Waypoint wp = Map.Waypoints.GetWPFromPoint(pt);
                    if (wp != null && wp != KeyStates.wp1)
                    {
                        string oldname = wpNameBox.Text;
                        OverrideChecking = true;
                        checkBox3.Checked = Convert.ToBoolean(wp.enabled);
                        OverrideChecking = false;
                        wpNameBox.Text = wp.Name;
                        if( KeyStates.wp1 !=null)
                            KeyStates.wp1.Name = oldname;
                        KeyStates.wp1 = wp;
                        KeyStates.MLeft = false;
                    }
                }
                else if (CurrentMode == Mode.MAKE_FLOOR)
                {
                    AddTile(tilePt);
                }
                else if (CurrentMode == Mode.ADD_POLYGON_PT)
                {
                    Map.Polygon poly = (Map.Polygon)Map.Polygons[listPolygons.SelectedIndex];
                    if (poly != null)
                        poly.Points.Add(new PointF(pt.X, pt.Y));
                }
                mapPanel.Invalidate();
            }
        }
        public void RemoveObject(Point pt)
        {
            SelectedObject = SelectObject(pt);
            if (SelectedObject != null)
            {
                
                int val = SelectedObject.UniqueID;
                Map.Objects.Remove(SelectedObject);
                unsafe
                {
                    MainWindow.myMap.DeleteObject((int)val);
                }

            }
        }
        private void mapPanel_MouseMove(object sender, MouseEventArgs e)
        {

            mouseLocation = new Point(e.X, e.Y);
            Point tilePt = GetNearestTilePoint(mouseLocation);
            wallMouseLocation = new Point(mouseLocation.X / squareSize, mouseLocation.Y / squareSize);
            Map.Wall wall = Map.Walls.ContainsKey(GetNearestWallPoint(mouseLocation)) ? Map.Walls[GetNearestWallPoint(mouseLocation)] : null;
            Map.Tile tile = Map.Tiles.ContainsKey(GetNearestTilePoint(mouseLocation)) ? Map.Tiles[GetNearestTilePoint(mouseLocation)] : null;

            statusWall.Text = statusTile.Text = statusObject.Text = "";
            statusLocation.Text = WallCoors ? String.Format("X={0} Y={1}", GetNearestWallPoint(mouseLocation).X, GetNearestWallPoint(mouseLocation).Y) : String.Format("X={0} Y={1}", e.X, e.Y);

            if (wall != null)
                statusWall.Text = String.Format("{0} #{1}", wall.Material, wall.Variation);

            if (tile != null)
            {
                statusTile.Text += String.Format("{0}-0x{1:x2}", tile.Graphic, tile.Variation);
                if (tile.EdgeTiles.Count > 0)
                {
                    statusTile.Text += String.Format(" Edges({0}):", tile.EdgeTiles.Count);
                    foreach (Map.Tile.EdgeTile edge in tile.EdgeTiles)
                        statusTile.Text += String.Format(" {0}-0x{1:x2}-{2}-{3}", ThingDb.FloorTileNames[edge.Graphic], edge.Variation, edge.Dir, ThingDb.EdgeTileNames[edge.Edge]);
                }
                /*
                if (video != null)
                {
                    Bitmap bits = video.ExtractOne(tile.Variations[tile.Variation]);
                    if (bits != null)
                    {
                        bits.Save(tempPath, System.Drawing.Imaging.ImageFormat.Png);
                        bits.Dispose();
                        //tilePictureBox.Load(tempPath);
                    }
                }
                 */
            }

           // if (KeyStates.wp1 != null && CurrentMode == Mode.EDIT_WAYPOINT)
             //   statusObject.Text = String.Format("Name = {0} Flag = {1}", KeyStates.wp1.Name, KeyStates.wp1.enabled);
            if (CurrentMode == Mode.SELECT_WAYPOINT)
            {
                if (KeyStates.wp1 != null && KeyStates.MLeft)
                {
                    KeyStates.wp1.Point.X = mouseLocation.X; // Move the waypoint
                    KeyStates.wp1.Point.Y = mouseLocation.Y;
                    mapPanel.Invalidate(); // Repaint the screen
                }
            }

            if (SelectedObject != null)
            {
                statusObject.Text = String.Format("{0}", SelectedObject.Name);
                if (CurrentMode == Mode.SELECT && KeyStates.MLeft)
                {
                    SelectedObject.Location.X = mouseLocation.X; // Move the object
                    SelectedObject.Location.Y = mouseLocation.Y;
                    mapPanel.Invalidate(); // Repaint the screen
                }
            }
            if (dragging == true && (CurrentMode == Mode.MAKE_WALL || CurrentMode == Mode.SELECT || CurrentMode == Mode.MAKE_FLOOR))
            {
                mapPanel.Invalidate();
            }
            else if (KeyStates.MLeft && CurrentMode == Mode.MAKE_FLOOR)
            {
                AddTile(tilePt);
            }
            else if (KeyStates.MRight && CurrentMode == Mode.MAKE_FLOOR)
            {
                RemoveTile(tilePt);
            }
            else if (KeyStates.MLeft && CurrentMode == Mode.EDIT_EDGE)
            {
                EditEdge(tilePt);
            }
            else if (KeyStates.MRight && CurrentMode == Mode.EDIT_EDGE)
            {
                RemoveEdge(tilePt);
            }
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            SetMode(Mode.SELECT);
            //CurrentMode = Mode.SELECT;//set new mode
            currentButton = (Button)sender;//update current button
            mapPanel.Invalidate();
        }
        private void newObjectButton_Click(object sender, EventArgs e)
        {
            SetMode(Mode.MAKE_OBJECT);
            //CurrentMode = Mode.MAKE_OBJECT;//set new mode
            currentButton = (Button)sender;//update current button
            mapPanel.Invalidate();
        }

        private void destructableButton_Click(object sender, EventArgs e)
        {
            SetMode(Mode.MAKE_DESTRUCTABLE);
            //CurrentMode = Mode.MAKE_DESTRUCTABLE;//set new mode
            currentButton = (Button)sender;//update current button	
            mapPanel.Invalidate();
        }

        private void windowsButton_Click(object sender, EventArgs e)
        {
            SetMode(Mode.MAKE_WINDOW);
            //CurrentMode = Mode.MAKE_WINDOW;//set new mode
            currentButton = (Button)sender;//update current button	
            mapPanel.Invalidate();
        }

        private void buttonSecret_Click(object sender, EventArgs e)
        {
            SetMode(Mode.MAKE_SECRET);
            //CurrentMode = Mode.MAKE_SECRET;//set new mode
            currentButton = (Button)sender;//update current button	
            mapPanel.Invalidate();
        }

        private void floorButton_Click(object sender, EventArgs e)
        {
            SetMode(Mode.MAKE_FLOOR);
            //CurrentMode = Mode.MAKE_FLOOR;//set new mode
            currentButton = (Button)sender;//update current button	
            mapPanel.Invalidate();
        }

        private void contextMenuDelete_Click(object sender, EventArgs e)
        {
            if (SelectedObject != null)
            {
                int val = SelectedObject.UniqueID;
                Map.Objects.Remove(SelectedObject);
                unsafe
                {
                    MainWindow.myMap.DeleteObject((int)val);
                }
                mapPanel.Invalidate();
            }   
        }
        private void contextMenuExportDefault_Click(object sender, EventArgs e)
        {
            if (SelectedObject != null)
            {
                string mod = "";
                string m_ExePath = Process.GetCurrentProcess().MainModule.FileName;
                m_ExePath = Path.GetDirectoryName(m_ExePath);
                StreamWriter SR;
                SR = File.CreateText(m_ExePath + "\\scripts\\objects\\defaultmods\\" + SelectedObject.Name.ToLower() + ".txt");
                if (SR != null)
                {
                    SR.WriteLine((short)SelectedObject.Properties);
                    SR.WriteLine(SelectedObject.Terminator);
                    foreach (byte b in SelectedObject.modbuf)
                        mod += String.Format("{0:x2} ", b);
                    SR.Write(mod);
                }
                SR.Close();
            }
        }
        private void contextMenuExportBinary_Click(object sender, EventArgs e)
        {
            if (SelectedObject != null)
            {
                string m_ExePath = Process.GetCurrentProcess().MainModule.FileName;
                m_ExePath = Path.GetDirectoryName(m_ExePath);
                FileStream fs = File.Create(m_ExePath + "\\scripts\\objects\\" + SelectedObject.Name.ToLower() + ".bin");
                BinaryWriter SR = new BinaryWriter(fs); ;

                if (SR != null)
                {
                    SR.Write(SelectedObject.modbuf, 0, SelectedObject.modbuf.Length);
                }
                SR.Close();
            }
        }
        protected ObjectPropertiesDialog propDlg;
        protected ObjectEnchantDialog enchantDlg;
        protected DoorProperties doorDlg;
        private void contextMenuProperties_Click(object sender, EventArgs e)
        {
            propDlg = new ObjectPropertiesDialog();
            propDlg.Object = SelectedObject;
            if (propDlg.ShowDialog() == DialogResult.OK)//modifications will be effected when ok is pressed
            {
                if (SelectedObject != null)
                {
                    int val = SelectedObject.UniqueID;
                    unsafe
                    {
                        MainWindow.myMap.DeleteObject((int)val);
                        IntPtr ptr = Marshal.StringToHGlobalAnsi(SelectedObject.Name);
                        if ((ThingDb.Things[SelectedObject.Name].Class & ThingDb.Thing.ClassFlags.DOOR) == ThingDb.Thing.ClassFlags.DOOR)
                        {
                        MainWindow.myMap.AddObject(ptr.ToPointer(), (int)SelectedObject.Location.X, (int)SelectedObject.Location.Y, val, SelectedObject.modbuf[0]);
                        }
                        else
                        MainWindow.myMap.AddObject(ptr.ToPointer(), (int)SelectedObject.Location.X, (int)SelectedObject.Location.Y, val,-1);
                    }
                }
                mapPanel.Invalidate();
            }
        }

        private void mapPanel_MouseUp(object sender, MouseEventArgs e)
        {
            Point pointClicked = new Point(e.X, e.Y);
            if (e.Button.Equals(MouseButtons.Right))
            {
                KeyStates.MRight = false;
            }
            else if (e.Button.Equals(MouseButtons.Left))
            {
                if (SelectedObject != null && CurrentMode == Mode.SELECT && KeyStates.MLeft)
                {
                    if (SnapGrid)//snap to grid when grid is on
                        pointClicked = new Point((int)Math.Round((decimal)(pointClicked.X / squareSize)) * squareSize, (int)Math.Round((decimal)(pointClicked.Y / squareSize)) * squareSize);

                    if (SnapHalfGrid)//snap to half grid when grid is on
                        pointClicked = new Point((int)Math.Round((decimal)((pointClicked.X / (squareSize)) * squareSize) + squareSize / 2), (int)Math.Round((decimal)((pointClicked.Y / (squareSize)) * squareSize) + squareSize / 2));

                    SelectedObject.Location = pointClicked;
                    dragging = false;
                }
                KeyStates.MLeft = false;
            }
            else if (e.Button.Equals(MouseButtons.Middle))
            {
                KeyStates.WheelClick = false;
            }

            if (dragging && wallDrag != null && CurrentMode == Mode.MAKE_WALL)
            {
                pointClicked = GetNearestWallPoint(pointClicked);
                int length = (int)(Distance(wallDrag, wallMouseLocation)); // X and Y should be same length
                length = (int)Math.Sqrt((length * length) / 2);
                if (pointClicked.X >= wallDrag.X && pointClicked.Y < wallDrag.Y)
                    for (int i = 1; i < length; i++)
                    {
                        Point newPoint = new Point(wallDrag.X + i, wallDrag.Y - i);
                        Map.Wall newWall = wallSelector.NewWall(newPoint);
                        newWall.Facing = Map.Wall.WallFacing.NORTH;
                        if (!Map.Walls.ContainsKey(newPoint))
                            Map.Walls.Add(newPoint, newWall);
                    }

                else if (pointClicked.X >= wallDrag.X && pointClicked.Y >= wallDrag.Y)
                    for (int i = 1; i < length; i++)
                    {
                        Point newPoint = new Point(wallDrag.X + i, wallDrag.Y + i);
                        Map.Wall newWall = wallSelector.NewWall(newPoint);
                        newWall.Facing = Map.Wall.WallFacing.WEST;
                        if (!Map.Walls.ContainsKey(newPoint))
                            Map.Walls.Add(newPoint, newWall);
                    }
                else if (pointClicked.X < wallDrag.X && pointClicked.Y < wallDrag.Y)
                    for (int i = 1; i < length; i++)
                    {
                        Point newPoint = new Point(wallDrag.X - i, wallDrag.Y - i);
                        Map.Wall newWall = wallSelector.NewWall(newPoint);
                        newWall.Facing = Map.Wall.WallFacing.WEST;
                        if (!Map.Walls.ContainsKey(newPoint))
                            Map.Walls.Add(newPoint, newWall);
                    }
                else if (pointClicked.X < wallDrag.X && pointClicked.Y >= wallDrag.Y)
                    for (int i = 1; i < length; i++)
                    {
                        Point newPoint = new Point(wallDrag.X - i, wallDrag.Y + i);
                        Map.Wall newWall = wallSelector.NewWall(newPoint);
                        newWall.Facing = Map.Wall.WallFacing.NORTH;
                        if (!Map.Walls.ContainsKey(newPoint))
                            Map.Walls.Add(newPoint, newWall);
                    }
                dragging = false;
            }
            else if (dragging && tileDrag != null && CurrentMode == Mode.MAKE_FLOOR)
            {
                Point tilePt = GetNearestTilePoint(mouseLocation);
                Point topLeftPt;
                //int variation = 0;
                int w = tilePt.X - tileDrag.X, h = tilePt.Y - tileDrag.Y;
                if (w < 0)
                    topLeftPt = new Point(tileDrag.X + w, 0);
                else
                    topLeftPt = new Point(tileDrag.X, 0);
                if (h < 0)
                    topLeftPt = new Point(topLeftPt.X, tileDrag.Y + h);
                else
                    topLeftPt = new Point(topLeftPt.X, tileDrag.Y);
                int varCount = ThingDb.FloorTiles[tileGraphic.SelectedIndex].Variations.Count; //-1 because needs to start at 0
                int varSqrt = (int)Math.Sqrt(varCount);
                for (int i = 0; i <= h; i++) // i = y
                {
                    for (int k = (i - (((int)(i / varSqrt)) * varSqrt)); k <= w + (i - (((int)(i / varSqrt)) * varSqrt)); k++) // k = x
                    {
                        tilePt = GetNearestTilePoint(new Point((topLeftPt.X + (k - (i - (((int)(i / varSqrt)) * varSqrt)))) * squareSize, (topLeftPt.Y + i) * squareSize));
                        //variation = CreateVariation(tilePt, (ushort)tileVar.SelectedIndex, (byte)tileGraphic.SelectedIndex);
                        Map.Tile tile = new Map.Tile(tilePt, (byte)tileGraphic.SelectedIndex, CreateVariation(tilePt, (ushort)tileVar.SelectedIndex, (byte)tileGraphic.SelectedIndex));
                        if (!Map.Tiles.ContainsKey(tilePt) && tilePt.X >= 0 && tilePt.Y >= 0 && tilePt.X <= 255 && tilePt.Y <= 255)
                        {
                            unsafe
                            {
                               // col = Color.FromArgb();
                                IntPtr ptr = Marshal.StringToHGlobalAnsi(tile.Graphic);
                                Map.Tiles.Add(tilePt, tile);
                                MainWindow.myMap.AddTile(ptr.ToPointer(), tilePt.Y, tilePt.X, CreateVariation(tilePt, (ushort)tileVar.SelectedIndex, (byte)tileGraphic.SelectedIndex));
                            }
                        }
                    }
                }
            }

            dragging = false;
            mapPanel.Invalidate();
        }

        public Map.Object SelectObject(Point pt)
        {
            double closestDistance = Double.MaxValue;
            Map.Object closest = null;

            foreach (Map.Object obj in Map.Objects)
            {
                double distance = Math.Sqrt(Math.Pow(pt.X - obj.Location.X, 2) + Math.Pow(pt.Y - obj.Location.Y, 2));

                if (distance < (double)objectSelectionRadius && distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = obj;
                }
            }

            return closest;
        }

        protected BlendDialog blendDialog = new BlendDialog();
        private void buttonBlend_Click(object sender, EventArgs e)
        {
            blendDialog.ShowDialog();
        }

        public ushort GetVariation(ComboBox box)
        {
            return box.SelectedIndex == 0 ? (ushort)new Random().Next(box.Items.Count - 1) : Convert.ToUInt16(box.Text);
        }
        public static void RepopulateVariations(ComboBox box, int variations)
        {
            int oldNdx = box.SelectedIndex;
            box.Items.Clear();
            box.Items.Add("Random");
            for (int i = 0; i < variations; i++)
                box.Items.Add(String.Format("{0}", i));
            if (oldNdx < box.Items.Count && oldNdx >= 0)
                box.SelectedIndex = oldNdx;
            else
                box.SelectedIndex = 1;
        }

        private void tileGraphic_SelectedIndexChanged(object sender, EventArgs e)
        {
            RepopulateVariations(tileVar, ((ThingDb.Tile)((ComboBox)sender).SelectedItem).Variations.Count);           
            //KeyStates.CurTileKey = ThingDb.FloorTiles.FindIndex(tileGraphic.SelectedText);
           // RepopulateVariations(tileVar, ((ThingDb.Tile)((ComboBox)sender).SelectedItem).Variations.Count);
        }

        private void defaultButt_Click(object sender, EventArgs e)
        {
            //propDlg = new ObjectPropertiesDialog();
            //propDlg.Object = DefaultObject;
            //propDlg.ShowDialog();
        }


        private void buttonPolygonNew_Click(object sender, EventArgs e)
        {
            polyDlg.Polygon = null;
            if (polyDlg.ShowDialog() == DialogResult.OK && polyDlg.Polygon != null)
            {
                Map.Polygons.Add(polyDlg.Polygon);
                listPolygons.Items.Add(polyDlg.Polygon.Name);
                mapPanel.Invalidate();
            }
        }

        private void buttonEditPolygon_Click(object sender, EventArgs e)
        {
            polyDlg.Polygon = (Map.Polygon)Map.Polygons[listPolygons.SelectedIndex];
            if (polyDlg.ShowDialog() == DialogResult.OK && polyDlg.Polygon != null)
            {
                Map.Polygons.RemoveAt(listPolygons.SelectedIndex);
                Map.Polygons.Insert(listPolygons.SelectedIndex, polyDlg.Polygon);
                mapPanel.Invalidate();
            }
        }

        private void buttonPolygonDelete_Click(object sender, EventArgs e)
        {
            Map.Polygons.RemoveAt(listPolygons.SelectedIndex);
            mapPanel.Invalidate();
        }

        private void listPolygons_Click(object sender, EventArgs e)
        {
            listPolygons.Items.Clear();
            foreach (Map.Polygon poly in Map.Polygons)
                listPolygons.Items.Add(poly.Name);
        }

        //TODO: use e.ClipRectangle to limit drawing
        private void mapPanel_Paint(object sender, PaintEventArgs e)
        {
            if (Map == null)
                return;

            PointF nwCorner, neCorner, seCorner, swCorner, center;

            Graphics g = e.Graphics;
            Font drawFont = new Font("Arial", 10);
            //black out the panel to start out
            Size size = mapPanel.Size;

            
            //MainWindow.myMap.SetLoc(winX, winY);
           // MainWindow.myMap.Update_Window();
            //MainWindow.myMap.Render(/*((MainWindow)this.Parent.Parent.Parent).Magnify*/false);
            g.Clear(ColorLayout.Background);
            //g.FillRectangle(ColorLayout.Background, new Rectangle(new Point(0, 0), size));
            Pen pen;

            //draw one more top and left row wall than neccesary
            Rectangle clip = new Rectangle(e.ClipRectangle.Left - squareSize, e.ClipRectangle.Top - squareSize, e.ClipRectangle.Width + squareSize, e.ClipRectangle.Height + squareSize);
            //Bitmap floor = new Bitmap(clip.Width, clip.Height);
            //draw grid
            if (DrawGrid || chkGrid.Checked)
            {
                using (pen = new Pen(Color.Gray, gridThickness))
                {
                    //draw the grid sloppily (an extra screen's worth of lines along either axis)
                    for (int x = -squareSize * (size.Width / squareSize) - 3 * squareSize / 2 % (2 * squareSize); x < 2 * size.Width; x += 2 * squareSize)
                    {
                        int y = -3 * squareSize / 2 % (2 * squareSize);
                        g.DrawLine(pen, new Point(x - 1, y), new Point(y, x - 1));
                        g.DrawLine(pen, new Point(x, y), new Point(size.Width + x, size.Width + y));
                    }
                }
            }

            //draw floor
            if (CurrentMode == Mode.MAKE_FLOOR || chkTileGrid.Checked || CurrentMode == Mode.EDIT_EDGE)//only draw the floor when editing it
            {

                foreach (Map.Tile tile in Map.Tiles.Values)
                {

                    //int x = (int)(tile.Location.X * squareSize - squareSize / 2f);
                    int x = tile.Location.X * squareSize;
                    int y = tile.Location.Y * squareSize;
                    if (chkAltGrid.Checked)
                        pen = ColorLayout.Tiles2;
                    else
                        pen = ColorLayout.Tiles;

                    if (clip.Contains((int)x, (int)y))
                    {
                        /*if (video != null)
                        {
                            System.Drawing.Imaging.ImageAttributes imgAttr = new System.Drawing.Imaging.ImageAttributes();
                            //imgAttr.SetColorKey(Color.Black, Color.Black);
                            video.ExtractOneToPoint(tile.Variations[tile.Variation],ref floor,new Point(x-clip.X,y-clip.Y));
                            floor.Save(tempPath2, System.Drawing.Imaging.ImageFormat.Bmp);
                        }/*
                        /* Old floor lines */
                        center = new PointF(x + squareSize / 2f, y + (3 / 2f) * squareSize);
                        nwCorner = new PointF(x - squareSize / 2f, y + (3 / 2f) * squareSize);
                        neCorner = new PointF(nwCorner.X + squareSize, nwCorner.Y - squareSize);
                        swCorner = new PointF(nwCorner.X + squareSize, nwCorner.Y + squareSize);
                        seCorner = new PointF(neCorner.X + squareSize, neCorner.Y + squareSize);
                        Brush tembrush = floorBrush;
                        if( ThingDb.FloorTiles[tile.graphicId].hascolor )
                            tembrush = new SolidBrush(ThingDb.FloorTiles[tile.graphicId].col);

                        g.FillPolygon(tembrush, new PointF[] { nwCorner, neCorner, seCorner, swCorner });
                        g.DrawPolygon(pen, new PointF[] { nwCorner, neCorner, seCorner, swCorner });
                        

                        //draw the center dot
                        int diam = 2;// +tile.EdgeTiles.Count * 3;
                        PointF ellTL = new PointF(center.X - diam / 2f, center.Y - diam / 2f);
                        if (tile.EdgeTiles.Count > 0)
                            e.Graphics.FillEllipse(Brushes.YellowGreen, ellTL.X, ellTL.Y, diam, diam);
                        else
                            e.Graphics.DrawEllipse(Pens.Brown, ellTL.X, ellTL.Y, diam, diam);
                        
                        //g.DrawString(tile.Location.X.ToString() +":"+ tile.Location.Y.ToString(), new Font("Arial", 10), Brushes.Red, center.X, center.Y);

                  /////
                  /////
                  /////
                  /////
                  /////


                        // Draw edging constructs
                      /*  if (tile.EdgeTiles.Count > 0)
                        {
                            PointF ellTL2 = new PointF(x - (squareSize - 4) / 2f, y + (3 / 2f) * (squareSize - 4));
                            PointF ellTL3 = new PointF(ellTL2.X + (squareSize - 4), ellTL2.Y - (squareSize - 4));
                            if (ThingDb.FloorTiles[tile.graphicId].hascolor)
                                g.DrawLine(new Pen(ThingDb.FloorTiles[tile.graphicId].col, gridThickness * 4), ellTL2, ellTL3);
                            else
                                g.DrawLine(new Pen(Color.Aqua, gridThickness * 4), ellTL2, ellTL3);

                        }*/

                        PointF nwCorner2 = nwCorner;
                        PointF neCorner2 = neCorner;
                        PointF swCorner2 = swCorner;
                        PointF seCorner2 = seCorner;
                        
                        int i = 0;
                        for( i=0; i<tile.EdgeTiles.Count; i++)
                        {
                            int graphId = ((Map.Tile.EdgeTile)tile.EdgeTiles[i]).Graphic;
                            Color col = new Color();
                            if (ThingDb.FloorTiles[graphId].hascolor)
                                col = ThingDb.FloorTiles[graphId].col;
                            else
                                col = Color.Aqua;
                            nwCorner2 = nwCorner;//new PointF(x - squareSize / 2f, y + (3 / 2f) * squareSize);
                            neCorner2 = neCorner;// new PointF(nwCorner.X + squareSize, nwCorner.Y - squareSize);
                            swCorner2 = swCorner;// new PointF(nwCorner.X + squareSize, nwCorner.Y + squareSize);
                            seCorner2 = seCorner;// new PointF(neCorner.X + squareSize, neCorner.Y + squareSize);

                            switch (((Map.Tile.EdgeTile)tile.EdgeTiles[i]).Dir)
                            {
                                case Map.Tile.EdgeTile.Direction.North_08:
                                case Map.Tile.EdgeTile.Direction.North_0A:
                                case Map.Tile.EdgeTile.Direction.North:
                                    nwCorner2.X += 2;
                                    nwCorner2.Y += 2;
                                    neCorner2.X += 2;
                                    neCorner2.Y += 2;
                                    g.DrawLine(new Pen(col, gridThickness * 4), nwCorner2, neCorner2);
                                    break;
                                case Map.Tile.EdgeTile.Direction.South_07:
                                case Map.Tile.EdgeTile.Direction.South_09:
                                case Map.Tile.EdgeTile.Direction.South:
                                    swCorner2.X -= 2;
                                    swCorner2.Y -= 2;
                                    seCorner2.X -= 2;
                                    seCorner2.Y -= 2;
                                    g.DrawLine(new Pen(col, gridThickness * 4), swCorner2, seCorner2);
                                    break;
                                case Map.Tile.EdgeTile.Direction.East_D:
                                case Map.Tile.EdgeTile.Direction.East_E:
                                case Map.Tile.EdgeTile.Direction.East:
                                    neCorner.X -= 2;
                                    neCorner.Y += 2;
                                    seCorner.X -= 2;
                                    seCorner.Y += 2;
                                    g.DrawLine(new Pen(col, gridThickness * 4), neCorner, seCorner);
                                    break;
                                case Map.Tile.EdgeTile.Direction.West_02:
                                case Map.Tile.EdgeTile.Direction.West_03:
                                case Map.Tile.EdgeTile.Direction.West:
                                    nwCorner2.X += 2;
                                    nwCorner2.Y -= 2;
                                    swCorner2.X += 2;
                                    swCorner2.Y -= 2;
                                    g.DrawLine(new Pen(col, gridThickness * 4), nwCorner2, swCorner2);
                                    break;
                                case Map.Tile.EdgeTile.Direction.NE_Sides:
                                    neCorner.X -= 2;
                                    neCorner.Y += 2;
                                    seCorner.X -= 2;
                                    seCorner.Y += 2;
                                    g.DrawLine(new Pen(col, gridThickness * 4), neCorner, seCorner);
                                    nwCorner2 = nwCorner;
                                    neCorner2 = neCorner;
                                    swCorner2 = swCorner;
                                    seCorner2 = seCorner;
                                    nwCorner2.X += 2;
                                    nwCorner2.Y += 2;
                                    neCorner2.X += 2;
                                    neCorner2.Y += 2;
                                    g.DrawLine(new Pen(col, gridThickness * 4), nwCorner2, neCorner2);
                                    break;
                                case Map.Tile.EdgeTile.Direction.NW_Sides:
                                    nwCorner2.X += 2;
                                    nwCorner2.Y -= 2;
                                    swCorner2.X += 2;
                                    swCorner2.Y -= 2;
                                    g.DrawLine(new Pen(col, gridThickness * 4), nwCorner2, swCorner2);
                                    nwCorner2 = nwCorner;
                                    neCorner2 = neCorner;
                                    swCorner2 = swCorner;
                                    seCorner2 = seCorner;
                                    nwCorner2.X += 2;
                                    nwCorner2.Y += 2;
                                    neCorner2.X += 2;
                                    neCorner2.Y += 2;
                                    g.DrawLine(new Pen(col, gridThickness * 4), nwCorner2, neCorner2);
                                    break;
                                case Map.Tile.EdgeTile.Direction.SE_Sides:
                                    swCorner2.X -= 2;
                                    swCorner2.Y -= 2;
                                    seCorner2.X -= 2;
                                    seCorner2.Y -= 2;
                                    g.DrawLine(new Pen(col, gridThickness * 4), swCorner2, seCorner2);
                                    nwCorner2 = nwCorner;
                                    neCorner2 = neCorner;
                                    swCorner2 = swCorner;
                                    seCorner2 = seCorner;
                                    neCorner.X -= 2;
                                    neCorner.Y += 2;
                                    seCorner.X -= 2;
                                    seCorner.Y += 2;
                                    g.DrawLine(new Pen(col, gridThickness * 4), neCorner, seCorner);
                                    break;

                                case Map.Tile.EdgeTile.Direction.SW_Sides:
                                    swCorner2.X -= 2;
                                    swCorner2.Y -= 2;
                                    seCorner2.X -= 2;
                                    seCorner2.Y -= 2;
                                    g.DrawLine(new Pen(col, gridThickness * 4), swCorner2, seCorner2);
                                    nwCorner2 = nwCorner;
                                    neCorner2 = neCorner;
                                    swCorner2 = swCorner;
                                    seCorner2 = seCorner;
                                    nwCorner2.X += 2;
                                    nwCorner2.Y -= 2;
                                    swCorner2.X += 2;
                                    swCorner2.Y -= 2;
                                    g.DrawLine(new Pen(col, gridThickness * 4), nwCorner2, swCorner2);
                                    break;
                                case Map.Tile.EdgeTile.Direction.NE_Tip:
                                   /* //neCorner.X += 2;
                                    //neCorner.Y -= 2;
                                    PointF tem = neCorner2;
                                    tem.X -= 4;
                                    tem.Y += 4;
                                    PointF tem2 = neCorner2;
                                    tem.X += 4;
                                    tem2.Y += 4;
                                    g.FillPolygon(new SolidBrush(col), new PointF[] { neCorner2, tem, tem2 });
                                    //g.DrawLine(new Pen(col, gridThickness * 4), nwCorner, swCorner);
                                    */break;
                                case Map.Tile.EdgeTile.Direction.SE_Tip: break;
                                case Map.Tile.EdgeTile.Direction.SW_Tip: break;
                                case Map.Tile.EdgeTile.Direction.NW_Tip: break;

                                default: break;
                            }

                        }  


                  /////
                  /////
                  /////
                  /////
                  /////
                  /////


                    }
                }
                //floor.Save("c:\\file.bmp");
                //e.Graphics.DrawImage(floor, new Point(clip.X + 40, clip.Y + 40));
            }
            if (CurrentMode == Mode.MAKE_FLOOR || CurrentMode == Mode.EDIT_EDGE)
            {
                // Draw the overlay to show tile location
                Point pt = new Point(mouseLocation.X, mouseLocation.Y);
                Point tilePt = GetNearestTilePoint(pt);
                int squareSize2 = squareSize;
                if (threeFloorBox.Checked && CurrentMode == Mode.MAKE_FLOOR)
                {
                    squareSize2 *= 3;
                    tilePt.X -= 1;
                    tilePt.Y -= 3;
                }
                tilePt.X *= squareSize;
                tilePt.Y *= squareSize;


                center = new PointF(tilePt.X + squareSize / 2f, tilePt.Y + (3 / 2f) * squareSize);
                nwCorner = new PointF(tilePt.X - squareSize2 / 2f, tilePt.Y + (3 / 2f) * squareSize2);
                neCorner = new PointF(nwCorner.X + squareSize2, nwCorner.Y - squareSize2);
                swCorner = new PointF(nwCorner.X + squareSize2, nwCorner.Y + squareSize2);
                seCorner = new PointF(neCorner.X + squareSize2, neCorner.Y + squareSize2);
                if( CurrentMode == Mode.MAKE_FLOOR )
                    g.DrawPolygon(new Pen(Color.Yellow, gridThickness * 2), new PointF[] { nwCorner, neCorner, seCorner, swCorner });
                else if( CurrentMode == Mode.EDIT_EDGE )
                    g.DrawPolygon(new Pen(Color.Aqua, gridThickness * 2), new PointF[] { nwCorner, neCorner, seCorner, swCorner });
            }
            //draw walls in map editor
            Pen extentPen = new Pen(Color.Green, 1);
            Pen ambPen = new Pen(Color.LightGray, 1);
            Pen doorPen = new Pen(Color.Brown, 2);
            Pen objMoveablePen = new Pen(Color.OrangeRed, 1);
            Pen destructablePen = new Pen(Color.Red, wallThickness);
            Pen windowPen = new Pen(Color.Orange, wallThickness);
            Pen secretPen = new Pen(Color.Green, wallThickness);
            Pen invisiblePen = new Pen(Color.DarkGray, wallThickness);
            Pen wallPen = new Pen(ColorLayout.Walls, wallThickness);

            foreach (Map.Wall wall in Map.Walls.Values)
            {
                Point pt = wall.Location;
                int x = pt.X * squareSize, y = pt.Y * squareSize;
                Point txtPoint = new Point(x,y);
                if (clip.Contains(x, y))
                {
                    //TODO: how to draw if a destructable window? is this even possible?
                    if (wall.Destructable)
                        pen = destructablePen;
                    else if (wall.Window)
                        pen = windowPen;
                    else if (wall.Secret)
                        pen = secretPen;
                    else if (wall.Material.Contains("Invisible"))
                        pen = invisiblePen;
                    else
                        pen = wallPen;

                    center = new PointF(x + squareSize / 2f, y + squareSize / 2f);
                    Point nCorner = new Point(x, y);
                    Point sCorner = new Point(x + squareSize, y + squareSize);
                    Point wCorner = new Point(x + squareSize, y);
                    Point eCorner = new Point(x, y + squareSize);

                    Point nCornerUp = new Point(x, y - 40);
                    Point sCornerUp = new Point(x + squareSize, y + squareSize - 40);
                    Point wCornerUp = new Point(x + squareSize, y - 40);
                    Point eCornerUp = new Point(x, y + squareSize - 40);
                    PointF centerUp = new PointF(x + squareSize / 2f,( y + squareSize / 2f )-40);

                    switch (wall.Facing)
                    {
                        case Map.Wall.WallFacing.NORTH:
                            g.DrawLine(pen, wCorner, eCorner);
                            if (!DrawWalls3D)
                                break;
                            g.DrawLine(pen, wCornerUp, eCornerUp);
 
                            g.DrawLine(pen, wCornerUp, wCorner);
                            g.DrawLine(pen, eCornerUp, eCorner);
                            
                            break;
                        case Map.Wall.WallFacing.WEST:
                            g.DrawLine(pen, nCorner, sCorner);
                            if (!DrawWalls3D)
                                break;
                            g.DrawLine(pen, nCornerUp, sCornerUp);

                            g.DrawLine(pen, nCorner, nCornerUp);
                            g.DrawLine(pen, sCorner, sCornerUp);
                            break;
                        case Map.Wall.WallFacing.CROSS:
                            g.DrawLine(pen, wCorner, eCorner);//north wall
                            g.DrawLine(pen, nCorner, sCorner);//south wall
                            if (!DrawWalls3D)
                                break;
                            g.DrawLine(pen, wCornerUp, eCornerUp);//north wall
                            g.DrawLine(pen, nCornerUp, sCornerUp);//south wall

                            g.DrawLine(pen, wCorner, wCornerUp);
                            g.DrawLine(pen, nCorner, nCornerUp);
                            g.DrawLine(pen, sCorner, sCornerUp);
                            g.DrawLine(pen, eCorner, eCornerUp);

                            break;
                        case Map.Wall.WallFacing.NORTH_T:
                            g.DrawLine(pen, wCorner, eCorner);//north wall
                            g.DrawLine(pen, center, sCorner);//tail towards south
                            if (!DrawWalls3D)
                                break;
                            g.DrawLine(pen, wCornerUp, eCornerUp);//north wall
                            g.DrawLine(pen, centerUp, sCornerUp);//tail towards south

                            g.DrawLine(pen, wCorner, wCornerUp);
                            g.DrawLine(pen, eCorner, eCornerUp);
                            g.DrawLine(pen, sCorner, sCornerUp);
                            g.DrawLine(pen, center, centerUp);

                            break;
                        case Map.Wall.WallFacing.SOUTH_T:
                            g.DrawLine(pen, wCorner, eCorner);//north wall
                            g.DrawLine(pen, center, nCorner);//tail towards north
                            if (!DrawWalls3D)
                                break;
                            g.DrawLine(pen, wCornerUp, eCornerUp);//north wall
                            g.DrawLine(pen, centerUp, nCornerUp);//tail towards south

                            g.DrawLine(pen, wCorner, wCornerUp);
                            g.DrawLine(pen, eCorner, eCornerUp);
                            g.DrawLine(pen, nCorner, nCornerUp);
                            g.DrawLine(pen, center, centerUp);
                            break;
                        case Map.Wall.WallFacing.WEST_T:
                            g.DrawLine(pen, nCorner, sCorner);//west wall
                            g.DrawLine(pen, center, eCorner);//tail towards east
                            if (!DrawWalls3D)
                                break;
                            g.DrawLine(pen, nCornerUp, sCornerUp);//north wall
                            g.DrawLine(pen, centerUp, eCornerUp);//tail towards south

                            g.DrawLine(pen, nCorner, nCornerUp);
                            g.DrawLine(pen, sCorner, sCornerUp);
                            g.DrawLine(pen, eCorner, eCornerUp);
                            g.DrawLine(pen, center, centerUp);
                            break;
                        case Map.Wall.WallFacing.EAST_T:
                            g.DrawLine(pen, nCorner, sCorner);//west wall
                            g.DrawLine(pen, center, wCorner);//tail towards west
                            if (!DrawWalls3D)
                                break;
                            g.DrawLine(pen, nCornerUp, sCornerUp);//north wall
                            g.DrawLine(pen, centerUp, wCornerUp);//tail towards south

                            g.DrawLine(pen, wCorner, wCornerUp);
                            g.DrawLine(pen, nCorner, nCornerUp);
                            g.DrawLine(pen, sCorner, sCornerUp);
                            g.DrawLine(pen, center, centerUp);
                            break;
                        case Map.Wall.WallFacing.NE_CORNER:
                            g.DrawLine(pen, center, eCorner);
                            g.DrawLine(pen, center, sCorner);
                            if (!DrawWalls3D)
                                break;
                            g.DrawLine(pen, centerUp, eCornerUp);
                            g.DrawLine(pen, centerUp, sCornerUp);

                            g.DrawLine(pen, centerUp, center);
                            g.DrawLine(pen, eCornerUp, eCorner);
                            g.DrawLine(pen, sCornerUp, sCorner);
                            break;
                        case Map.Wall.WallFacing.NW_CORNER:
                            g.DrawLine(pen, center, wCorner);
                            g.DrawLine(pen, center, sCorner);
                            if (!DrawWalls3D)
                                break;
                            g.DrawLine(pen, centerUp, wCornerUp);
                            g.DrawLine(pen, centerUp, sCornerUp);

                            g.DrawLine(pen, centerUp, center);
                            g.DrawLine(pen, wCornerUp, wCorner);
                            g.DrawLine(pen, sCornerUp, sCorner);
                            break;
                        case Map.Wall.WallFacing.SW_CORNER:
                            g.DrawLine(pen, center, nCorner);
                            g.DrawLine(pen, center, wCorner);
                            if (!DrawWalls3D)
                                break;
                            g.DrawLine(pen, centerUp, nCornerUp);
                            g.DrawLine(pen, centerUp, wCornerUp);

                            g.DrawLine(pen, centerUp, center);
                            g.DrawLine(pen, wCornerUp, wCorner);
                            g.DrawLine(pen, nCornerUp, nCorner);
                            break;
                        case Map.Wall.WallFacing.SE_CORNER:
                            g.DrawLine(pen, center, nCorner);
                            g.DrawLine(pen, center, eCorner);

                            if (!DrawWalls3D)
                                break;
                            g.DrawLine(pen, centerUp, nCornerUp);
                            g.DrawLine(pen, centerUp, eCornerUp);

                            g.DrawLine(pen, centerUp, center);
                            g.DrawLine(pen, eCornerUp, eCorner);
                            g.DrawLine(pen, nCornerUp, nCorner);
                            break;
                        default:
                            g.DrawRectangle(pen, x, y, squareSize, squareSize);
                           
                            TextRenderer.DrawText(e.Graphics, "?", drawFont, nCorner, Color.Red);
                           // g.DrawString("?", new Font("Arial", 12), Brushes.Red, nCorner);
                            break;
                    }
                   // pen.Dispose();

                    //g.DrawString(wall.Minimap.ToString(), new Font("Arial", 10), Brushes.Red, x, y);
                    TextRenderer.DrawText(e.Graphics, wall.Minimap.ToString(), drawFont, txtPoint,Color.Red);
                }
            }

            //draw objects
            if (chkShowObj.Checked)
            {
                foreach (Map.Object oe in Map.Objects)
                {
                    PointF ptf = oe.Location;
                    if (clip.Contains((int)ptf.X, (int)ptf.Y))
                    {
                        float x = ptf.X, y = ptf.Y;

                        center = new PointF(x, y);
                        PointF topLeft = new PointF(center.X - objectSelectionRadius, center.Y - objectSelectionRadius);
                        if (SelectedObject != null && SelectedObject.Location.Equals(oe.Location))
                            pen = Pens.Green;
                        else
                            pen = ColorLayout.Objects;
                        g.DrawEllipse(pen, new RectangleF(topLeft, new Size(2 * objectSelectionRadius, 2 * objectSelectionRadius)));
                        
                        // If is a door
                        if ((ThingDb.Things[oe.Name].Class & ThingDb.Thing.ClassFlags.DOOR) == ThingDb.Thing.ClassFlags.DOOR)
                        {
                            if( oe.modbuf[0] == 0x00 ) // South
                            {
                                g.DrawLine(doorPen, new Point((int)center.X,(int)center.Y), new Point((int)center.X-20,(int)center.Y-20));

                                if (DrawWalls3D)
                                {
                                    g.DrawLine(doorPen, new Point((int)center.X, (int)center.Y - 40), new Point((int)center.X - 20, (int)center.Y - 60));
                                    g.DrawLine(doorPen, new Point((int)center.X, (int)center.Y), new Point((int)center.X - 20, (int)center.Y - 60));

                                    //g.DrawLine(pen, new Point((int)center.X, (int)center.Y), new Point((int)center.X, (int)center.Y - 40));
                                    g.DrawLine(doorPen, new Point((int)center.X - 20, (int)center.Y - 20), new Point((int)center.X - 20, (int)center.Y - 60));
                                }
                            }
                            else if (oe.modbuf[0] == 0x08 ) // West
                            {
                                g.DrawLine(doorPen, new Point((int)center.X, (int)center.Y), new Point((int)center.X + 20, (int)center.Y - 20));

                                if (DrawWalls3D)
                                {
                                    g.DrawLine(doorPen, new Point((int)center.X, (int)center.Y - 40), new Point((int)center.X + 20, (int)center.Y - 60));
                                    g.DrawLine(doorPen, new Point((int)center.X, (int)center.Y), new Point((int)center.X + 20, (int)center.Y - 60));

                                   // g.DrawLine(doorPen, new Point((int)center.X, (int)center.Y), new Point((int)center.X, (int)center.Y - 40));
                                    g.DrawLine(doorPen, new Point((int)center.X + 20, (int)center.Y - 20), new Point((int)center.X + 20, (int)center.Y - 60));
                                }
                            }
                            else if (oe.modbuf[0] == 0x10 ) // North
                            {
                                g.DrawLine(doorPen, new Point((int)center.X, (int)center.Y), new Point((int)center.X + 20, (int)center.Y + 20));

                                if (DrawWalls3D)
                                {
                                    g.DrawLine(doorPen, new Point((int)center.X, (int)center.Y - 40), new Point((int)center.X + 20, (int)center.Y - 20));
                                    g.DrawLine(doorPen, new Point((int)center.X, (int)center.Y), new Point((int)center.X + 20, (int)center.Y - 20));

                                    //g.DrawLine(doorPen, new Point((int)center.X, (int)center.Y), new Point((int)center.X, (int)center.Y - 40));
                                    g.DrawLine(doorPen, new Point((int)center.X + 20, (int)center.Y + 20), new Point((int)center.X + 20, (int)center.Y - 20));
                                }
                            }
                            else if (oe.modbuf[0] == 0x18) // East
                            {
                                g.DrawLine(doorPen, new Point((int)center.X, (int)center.Y), new Point((int)center.X - 20, (int)center.Y + 20));

                                if (DrawWalls3D)
                                {
                                    g.DrawLine(doorPen, new Point((int)center.X, (int)center.Y - 40), new Point((int)center.X - 20, (int)center.Y - 20));
                                    g.DrawLine(doorPen, new Point((int)center.X, (int)center.Y), new Point((int)center.X - 20, (int)center.Y - 20));

                                    //g.DrawLine(doorPen, new Point((int)center.X, (int)center.Y), new Point((int)center.X, (int)center.Y - 40));
                                    g.DrawLine(doorPen, new Point((int)center.X - 20, (int)center.Y + 20), new Point((int)center.X - 20, (int)center.Y - 20));
                                }
                            }
                        }
                        
                        if (ShowExtents)
                        {
                            if (!(!ShowAllExtents && ((ThingDb.Things[oe.Name].Flags & ThingDb.Thing.FlagsFlags.NO_COLLIDE) == ThingDb.Thing.FlagsFlags.NO_COLLIDE)))
                            {
                                if (ThingDb.Things[oe.Name].ExtentType == "CIRCLE")
                                {
                                    PointF t = new PointF(center.X - ThingDb.Things[oe.Name].ExtentX, center.Y - ThingDb.Things[oe.Name].ExtentX);
                                    PointF p = new PointF((center.X) - ThingDb.Things[oe.Name].ExtentX, (center.Y - (ThingDb.Things[oe.Name].ZSizeY)) - ThingDb.Things[oe.Name].ExtentX);

                                    Pen rotatePen;

                                    if (!((ThingDb.Things[oe.Name].Class & ThingDb.Thing.ClassFlags.IMMOBILE) == ThingDb.Thing.ClassFlags.IMMOBILE))
                                    {
                                        rotatePen = objMoveablePen;
                                    }
                                    else
                                        rotatePen = extentPen;

                                    PointF point1 = new PointF(t.X, t.Y);
                                    point1.Y += ThingDb.Things[oe.Name].ExtentX;
                                    PointF point2 = new PointF(p.X, p.Y);
                                    point2.Y += ThingDb.Things[oe.Name].ExtentX;

                                    g.DrawLine(rotatePen, point1, point2);

                                    point1.X += ThingDb.Things[oe.Name].ExtentX * 2;
                                    point2.X += ThingDb.Things[oe.Name].ExtentX * 2;

                                    g.DrawLine(rotatePen, point1, point2);

                                    g.DrawEllipse(rotatePen, new RectangleF(t, new Size(2 * ThingDb.Things[oe.Name].ExtentX, 2 * ThingDb.Things[oe.Name].ExtentX)));
                                    g.DrawEllipse(rotatePen, new RectangleF(p, new Size(2 * ThingDb.Things[oe.Name].ExtentX, 2 * ThingDb.Things[oe.Name].ExtentX)));
                                }
                                if (ThingDb.Things[oe.Name].ExtentType == "BOX")
                                {
                                    Point t = new Point((int)(center.X - (ThingDb.Things[oe.Name].ExtentX / 2)), (int)(center.Y - (ThingDb.Things[oe.Name].ExtentY / 2)));
                                    Point p = new Point((int)((center.X - (ThingDb.Things[oe.Name].ZSizeY / 2)) - (ThingDb.Things[oe.Name].ExtentX / 2)), (int)((center.Y - (ThingDb.Things[oe.Name].ZSizeY / 2)) - (ThingDb.Things[oe.Name].ExtentY / 2)));

                                    using (System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix())
                                    {
                                        m.RotateAt(45, center);
                                        g.Transform = m;
                                        Pen rotatePen = new Pen(Color.Green, 1);

                                        PointF point1 = new PointF(t.X, t.Y);
                                        PointF point2 = new PointF(p.X, p.Y);
                                        g.DrawLine(rotatePen, point1, point2);

                                        point1 = new PointF(t.X, t.Y);
                                        point2 = new PointF(p.X, p.Y);
                                        point1.Y += ThingDb.Things[oe.Name].ExtentY;
                                        point2.Y += ThingDb.Things[oe.Name].ExtentY;
                                        g.DrawLine(rotatePen, point1, point2);

                                        point1 = new PointF(t.X, t.Y);
                                        point2 = new PointF(p.X, p.Y);
                                        point1.X += ThingDb.Things[oe.Name].ExtentX;
                                        point2.X += ThingDb.Things[oe.Name].ExtentX;
                                        g.DrawLine(rotatePen, point1, point2);

                                        point1 = new PointF(t.X, t.Y);
                                        point2 = new PointF(p.X, p.Y);
                                        point1.X += ThingDb.Things[oe.Name].ExtentX;
                                        point2.X += ThingDb.Things[oe.Name].ExtentX;
                                        point1.Y += ThingDb.Things[oe.Name].ExtentY;
                                        point2.Y += ThingDb.Things[oe.Name].ExtentY;
                                        g.DrawLine(rotatePen, point1, point2);

                                        g.DrawRectangle(rotatePen, new Rectangle(t, new Size(ThingDb.Things[oe.Name].ExtentX, ThingDb.Things[oe.Name].ExtentY)));
                                        g.DrawRectangle(rotatePen, new Rectangle(p, new Size(ThingDb.Things[oe.Name].ExtentX, ThingDb.Things[oe.Name].ExtentY)));
                                        //g.ResetTransform();
                                        g.ResetTransform();
                                    }
                                }
                            }
                        }
                        if (!ShowObjectNames && ! (ShowExtents || ShowAllExtents))
                        {

                            TextRenderer.DrawText(e.Graphics, oe.Extent.ToString(), drawFont, new Point(Convert.ToInt32(topLeft.X),Convert.ToInt32(topLeft.Y)), Color.Purple);
                            //g.DrawString(oe.Extent.ToString(), new Font("Arial", 10), Brushes.Purple, topLeft);
                        }
                        else if (ShowObjectNames)
                           // g.DrawString(oe.Name, new Font("Arial", 10), Brushes.Green, topLeft);
                            TextRenderer.DrawText(e.Graphics, oe.Name, drawFont, new Point(Convert.ToInt32(topLeft.X), Convert.ToInt32(topLeft.Y)), Color.Green);

                       // ThingDb.Things.Keys.
                    }
                }
            }
            //draw polygons
            if (ShowPolygons)
            {
                foreach (Map.Polygon poly in Map.Polygons)
                {
                    pen = Pens.PaleGreen;
                    ArrayList points = new ArrayList();
                    foreach (PointF pt in poly.Points)
                        points.Add(new PointF(pt.X, pt.Y));
                    if (points.Count > 2)
                    {
                        points.Add(points[0]);//complete the loop
                        g.DrawLines(pen, (PointF[])points.ToArray(typeof(PointF)));
                    }
                }
            }
            //draw waypoints
            if (ShowWaypoints)
            {
                foreach (Map.Waypoint wp in Map.Waypoints)
                {
                    if (KeyStates.wp1 != null && wp == KeyStates.wp1)
                        pen = Pens.Aqua;
                    else
                        pen = Pens.Olive;
                    center = new PointF(wp.Point.X - objectSelectionRadius, wp.Point.Y - objectSelectionRadius);
                    g.DrawEllipse(pen, new RectangleF(center, new Size(2 * objectSelectionRadius, 2 * objectSelectionRadius)));

                    //code/idea from UndeadZeus
                    foreach (Map.Waypoint.WaypointConnection wpc in wp.connections)
                    {
                        g.DrawLine(pen, wp.Point.X, wp.Point.Y, wpc.wp.Point.X, wpc.wp.Point.Y);
                        foreach (Map.Waypoint.WaypointConnection wpwp in wpc.wp.connections)//Checks if the waypoint connection is connecting to wp
                        {
                            if (wpwp.wp.Equals(wp))
                            {
                                if (KeyStates.wp1 != null && wp == KeyStates.wp1)
                                    g.DrawLine(Pens.Aqua, wp.Point.X, wp.Point.Y, wpc.wp.Point.X, wpc.wp.Point.Y);
                                else
                                    g.DrawLine(Pens.Orange, wp.Point.X, wp.Point.Y, wpc.wp.Point.X, wpc.wp.Point.Y);
                                break;
                            }
                        }
                    }
                    if (wp.Name.Length > 0)
                      //  g.DrawString(wp.num + ":" + wp.Name, new Font("Arial", 8), Brushes.Purple, center);
                        TextRenderer.DrawText(e.Graphics, wp.num + ":" + wp.Name, drawFont, new Point(Convert.ToInt32(center.X), Convert.ToInt32(center.Y)), Color.Purple);
                    else
                        //g.DrawString(wp.num + "", new Font("Arial", 8), Brushes.Purple, center);
                        TextRenderer.DrawText(e.Graphics, wp.num + "", drawFont, new Point(Convert.ToInt32(center.X), Convert.ToInt32(center.Y)), Color.Purple);
                }
            }
            //draw drag line
            if (dragging == true)
            {
                switch (CurrentMode)
                {
                    case Mode.MAKE_WALL:
                        pen = Pens.Green;
                        int length = (int)Distance(wallDrag, wallMouseLocation);
                        int newX = wallDrag.X + (int)Math.Sqrt((length * length) / 2) * (wallMouseLocation.X >= wallDrag.X ? 1 : (-1));
                        int newY = wallDrag.Y + (int)Math.Sqrt((length * length) / 2) * (wallMouseLocation.Y >= wallDrag.Y ? 1 : (-1));
                        g.DrawLine(pen, wallDrag.X * 23, wallDrag.Y * 23, newX * 23, newY * 23);
                        break;
                    case Mode.SELECT:
                        if (SelectedObject != null)
                        {
                            pen = Pens.Green;
                            PointF topLeft = new PointF(mouseLocation.X - objectSelectionRadius, mouseLocation.Y - objectSelectionRadius);
                            g.DrawEllipse(pen, new RectangleF(topLeft, new Size(2 * objectSelectionRadius, 2 * objectSelectionRadius)));
                        }
                        break;
                    case Mode.MAKE_FLOOR:
                        pen = Pens.Green;
                        Point tilePt = GetNearestTilePoint(mouseLocation);
                        Point topLeftPt;
                        int w = tilePt.X - tileDrag.X, h = tilePt.Y - tileDrag.Y;
                        if (w < 0)
                            topLeftPt = new Point(tileDrag.X + w, 0);
                        else
                            topLeftPt = new Point(tileDrag.X, 0);
                        if (h < 0)
                            topLeftPt = new Point(topLeftPt.X, tileDrag.Y + h);
                        else
                            topLeftPt = new Point(topLeftPt.X, tileDrag.Y);
                        for (int i = 0; i <= w; i++)
                        {
                            for (int k = 0; k <= h; k++)
                            {
                                tilePt = GetNearestTilePoint(new Point((topLeftPt.X + i) * squareSize, (topLeftPt.Y + k) * squareSize));
                                tilePt = new Point(tilePt.X * squareSize, tilePt.Y * squareSize);
                                center = new PointF(tilePt.X + squareSize / 2f, tilePt.Y + (3 / 2f) * squareSize);
                                nwCorner = new PointF(tilePt.X - squareSize / 2f, tilePt.Y + (3 / 2f) * squareSize);
                                neCorner = new PointF(nwCorner.X + squareSize, nwCorner.Y - squareSize);
                                swCorner = new PointF(nwCorner.X + squareSize, nwCorner.Y + squareSize);
                                seCorner = new PointF(neCorner.X + squareSize, neCorner.Y + squareSize);
                                g.DrawPolygon(pen, new PointF[] { nwCorner, neCorner, seCorner, swCorner });
                            }
                        }
                        break;
                }
            }
        }

        private Point GetNearestTilePoint(Point pt)
        {
            pt.Offset(0, -squareSize);
            return GetNearestWallPoint(pt);
        }

        private Point GetNearestWallPoint(Point pt)
        {
            Point tl = new Point((pt.X / squareSize) * squareSize, (pt.Y / squareSize) * squareSize);
            if (tl.X / squareSize % 2 == tl.Y / squareSize % 2)
                return new Point(tl.X / squareSize, tl.Y / squareSize);
            else
            {
                Point left = new Point(tl.X, tl.Y + squareSize / 2);
                Point right = new Point(tl.X + squareSize, tl.Y + squareSize / 2);
                Point top = new Point(tl.X + squareSize / 2, tl.Y);
                Point bottom = new Point(tl.X + squareSize / 2, tl.Y + squareSize);
                Point closest = left;
                foreach (Point point in new Point[] { left, right, top, bottom })
                    if (Distance(point, pt) < Distance(closest, pt))
                        closest = point;

                if (closest == left)
                    return new Point(tl.X / squareSize - 1, tl.Y / squareSize);
                else if (closest == right)
                    return new Point(tl.X / squareSize + 1, tl.Y / squareSize);
                else if (closest == top)
                    return new Point(tl.X / squareSize, tl.Y / squareSize - 1);
                else
                    return new Point(tl.X / squareSize, tl.Y / squareSize + 1);
            }
        }

        private int Distance(Point a, Point b)
        {
            return (int)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        private void wpAddButton_Click(object sender, EventArgs e)
        {
            SetMode(Mode.MAKE_WAYPOINT);
            //CurrentMode = Mode.MAKE_WAYPOINT;//set new mode
            currentButton = (Button)sender;//update current button
            mapPanel.Invalidate();
        }


        private void renWpButton_Click(object sender, EventArgs e)
        {
            SetMode(Mode.EDIT_WAYPOINT);
           /* if (listWaypoints.SelectedItem != null)
            {
                WaypointPropertiesDialog wpd = new WaypointPropertiesDialog();
                wpd.wplist = Map.Waypoints;
                wpd.wpPub = (Map.Waypoint)listWaypoints.SelectedItem;
                wpd.ShowDialog();
                mapPanel.Invalidate();
            }*/
        }

        private void scrollPanel_Scroll(object sender, ScrollEventArgs e)
        {

            mapPanel.Invalidate(new Rectangle(scrollPanel.HorizontalScroll.Value, scrollPanel.VerticalScroll.Value, scrollPanel.Width, scrollPanel.Height));
            //winX = scrollPanel.HorizontalScroll.Value + WidthMod;
            //winY = scrollPanel.VerticalScroll.Value + scrollPanel.Height/2;
        }

        public void centerPoint(Point centerAt)
        {
            if (mapPanel.ClientRectangle.Contains(centerAt))
            {
                int Y = centerAt.Y - scrollPanel.Height / 2;
                int X = centerAt.X - scrollPanel.Width / 2;
                if (Y < 0)
                    Y = 0;
                if (X < 0)
                    X = 0;
                scrollPanel.VerticalScroll.Value = Y;
                scrollPanel.HorizontalScroll.Value = X;
                winX = centerAt.X- WidthMod;
                winY = centerAt.Y;
                mapPanel.Invalidate();
            }
        }
        private void buttonPoints_Click(object sender, EventArgs e)
        {
            if (listPolygons.SelectedIndex > -1)
            {
                SetMode(Mode.ADD_POLYGON_PT);
                //CurrentMode = Mode.ADD_POLYGON_PT;
            }
        }

        private void contextMenu_Popup(object sender, EventArgs e)
        {
            if (SelectedObject == null)
                foreach (MenuItem m in contextMenu.MenuItems)
                    m.Enabled = false;
            else
                foreach (MenuItem m in contextMenu.MenuItems)
                    m.Enabled = true;
        }

        private void contextMenuCopy_Click(object sender, EventArgs e)
        {
            if (SelectedObject != null)
                Clipboard.SetDataObject(SelectedObject, true);
        }

        private void contextMenuPaste_Click(object sender, EventArgs e)
        {
            if (Clipboard.GetDataObject().GetDataPresent(typeof(Map.Object)))
            {
                Map.Object o = (Map.Object)((Map.Object)Clipboard.GetDataObject().GetData(typeof(Map.Object))).Clone();
                o.Location = mouseLocation;
                Map.Objects.Add(o);
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Delete)
            {
                DeletePressed.Invoke(this, new KeyEventArgs(keyData));
                return true;
            }
            else
                return base.ProcessDialogKey(keyData);
        }

        void MapView_DeletePressed(object sender, KeyEventArgs e)
        {
            if (SelectedObject != null)
            {
                int val = SelectedObject.UniqueID;
                Map.Objects.Remove(SelectedObject);
                SelectedObject = null;
                unsafe
                {
                    MainWindow.myMap.DeleteObject((int)val);
                }
                mapPanel.Invalidate();
            }
        }

        public Bitmap MapToImage()
        {
            if (Map == null)
                return null;
            PointF nwCorner, neCorner, seCorner, swCorner, center;
            Bitmap bitmap = new Bitmap(5880, 5880);
            Graphics g = Graphics.FromImage(bitmap);
            //black out the panel to start out
            Size size = bitmap.Size;
            g.FillRectangle(Brushes.Black, new Rectangle(new Point(0, 0), size));
            Pen pen;
            //draw floor
            foreach (Map.Tile tile in Map.Tiles.Values)
            {
                pen = Pens.Yellow;
                int x = tile.Location.X * squareSize;
                int y = tile.Location.Y * squareSize;
               // if (video == null)
                //{
                    center = new PointF(x + squareSize / 2f, y + (3 / 2f) * squareSize);
                    nwCorner = new PointF(x - squareSize / 2f, y + (3 / 2f) * squareSize);
                    neCorner = new PointF(nwCorner.X + squareSize, nwCorner.Y - squareSize);
                    swCorner = new PointF(nwCorner.X + squareSize, nwCorner.Y + squareSize);
                    seCorner = new PointF(neCorner.X + squareSize, neCorner.Y + squareSize);
                    g.DrawPolygon(pen, new PointF[] { nwCorner, neCorner, seCorner, swCorner });


                    //draw the center dot
                    int diam = 2 + tile.EdgeTiles.Count * 3;
                    PointF ellTL = new PointF(center.X - diam / 2f, center.Y - diam / 2f);
                    if (tile.EdgeTiles.Count > 0)
                        g.FillEllipse(Brushes.YellowGreen, ellTL.X, ellTL.Y, diam, diam);
                    else
                        g.DrawEllipse(Pens.Brown, ellTL.X, ellTL.Y, diam, diam);

                    g.DrawString(tile.Variation.ToString(), new Font("Arial", 10), Brushes.Red, center.X, center.Y);
                   // TextRenderer.DrawText(e.Graphics, tile.Variation.ToString(), drawFont, center, Color.Red);
                // }
                //else
                  //  g.DrawImageUnscaled(video.ExtractOne(tile.Variations[tile.Variation]), x - (int)(0.5f * squareSize), y + (int)(0.5f * squareSize));
            }
            //floor.Save("c:\\file.bmp");
            //e.Graphics.DrawImage(floor, new Point(clip.X + 40, clip.Y + 40));

            //draw walls
            foreach (Map.Wall wall in Map.Walls.Values)
            {
                Point pt = wall.Location;
                int x = pt.X * squareSize, y = pt.Y * squareSize;
                //TODO: how to draw if a destructable window? is this even possible?
                if (wall.Destructable)
                    pen = new Pen(Color.Red, wallThickness);
                else if (wall.Window)
                    pen = new Pen(Color.Orange, wallThickness);
                else if (wall.Secret)
                    pen = new Pen(Color.Green, wallThickness);
                else
                    pen = new Pen(Color.White, wallThickness);
                center = new PointF(x + squareSize / 2f, y + squareSize / 2f);
                Point nCorner = new Point(x, y);
                Point sCorner = new Point(x + squareSize, y + squareSize);
                Point wCorner = new Point(x + squareSize, y);
                Point eCorner = new Point(x, y + squareSize);

                Point nCornerUp = new Point(x-50, y-50);
                Point sCornerUp = new Point(x + squareSize-50, y + squareSize-50);
                Point wCornerUp = new Point(x + squareSize-50, y-50);
                Point eCornerUp = new Point(x-50, y + squareSize-50);
                switch (wall.Facing)
                {
                    case Map.Wall.WallFacing.NORTH:
                        g.DrawLine(pen, wCorner, eCorner);
                        break;
                    case Map.Wall.WallFacing.WEST:
                        g.DrawLine(pen, nCorner, sCorner);
                        break;
                    case Map.Wall.WallFacing.CROSS:
                        g.DrawLine(pen, wCorner, eCorner);//north wall
                        g.DrawLine(pen, nCorner, sCorner);//south wall
                        break;
                    case Map.Wall.WallFacing.NORTH_T:
                        g.DrawLine(pen, wCorner, eCorner);//north wall
                        g.DrawLine(pen, center, sCorner);//tail towards south
                        break;
                    case Map.Wall.WallFacing.SOUTH_T:
                        g.DrawLine(pen, wCorner, eCorner);//north wall
                        g.DrawLine(pen, center, nCorner);//tail towards north
                        break;
                    case Map.Wall.WallFacing.WEST_T:
                        g.DrawLine(pen, nCorner, sCorner);//west wall
                        g.DrawLine(pen, center, eCorner);//tail towards east
                        break;
                    case Map.Wall.WallFacing.EAST_T:
                        g.DrawLine(pen, nCorner, sCorner);//west wall
                        g.DrawLine(pen, center, wCorner);//tail towards west
                        break;
                    case Map.Wall.WallFacing.NE_CORNER:
                        g.DrawLine(pen, center, eCorner);
                        g.DrawLine(pen, center, sCorner);
                        break;
                    case Map.Wall.WallFacing.NW_CORNER:
                        g.DrawLine(pen, center, wCorner);
                        g.DrawLine(pen, center, sCorner);
                        break;
                    case Map.Wall.WallFacing.SW_CORNER:
                        g.DrawLine(pen, center, nCorner);
                        g.DrawLine(pen, center, wCorner);
                        break;
                    case Map.Wall.WallFacing.SE_CORNER:
                        g.DrawLine(pen, center, nCorner);
                        g.DrawLine(pen, center, eCorner);
                        break;
                    default:
                        g.DrawRectangle(pen, x, y, squareSize, squareSize);
                        g.DrawString("?", new Font("Arial", 12), Brushes.Red, nCorner);
                        //TextRenderer.DrawText(e.Graphics, "?", drawFont, nCorner, Color.Red);
                        break;
                }
                pen.Dispose();

                g.DrawString(wall.Minimap.ToString(), new Font("Arial", 10), Brushes.Red, x, y);
                //TextRenderer.DrawText(e.Graphics,wall.Minimap.ToString(),drawfont
            }

            //draw objects
            foreach (Map.Object oe in Map.Objects)
            {
                PointF ptf = oe.Location;
                float x = ptf.X, y = ptf.Y;

                center = new PointF(x, y);
                PointF topLeft = new PointF(center.X - objectSelectionRadius, center.Y - objectSelectionRadius);
                if (SelectedObject != null && SelectedObject.Location.Equals(oe.Location))
                    pen = Pens.Green;
                else
                    pen = Pens.Blue;
                g.DrawEllipse(pen, new RectangleF(topLeft, new Size(2 * objectSelectionRadius, 2 * objectSelectionRadius)));
                g.DrawString(oe.Extent.ToString(), new Font("Arial", 10), Brushes.Purple, topLeft);
            }

            //draw polygons
            foreach (Map.Polygon poly in Map.Polygons)
            {
                pen = Pens.PaleGreen;
                ArrayList points = new ArrayList();
                foreach (PointF pt in poly.Points)
                    points.Add(new PointF(pt.X, pt.Y));
                if (points.Count > 2)
                {
                    points.Add(points[0]);//complete the loop
                    g.DrawLines(pen, (PointF[])points.ToArray(typeof(PointF)));
                }
            }
            //draw waypoints
            foreach (Map.Waypoint wp in Map.Waypoints)
            {
                pen = Pens.Olive;
                center = new PointF(wp.Point.X - objectSelectionRadius, wp.Point.Y - objectSelectionRadius);
                g.DrawEllipse(pen, new RectangleF(center, new Size(2 * objectSelectionRadius, 2 * objectSelectionRadius)));
                if (wp.Name.Length > 0)
                    g.DrawString(wp.Name, new Font("Arial", 8), Brushes.Purple, center);
            }
            return bitmap;
        }
        ushort CreateVariation(Point loc,ushort Vari,byte tile)
        {

                ushort Variation = Vari;
                if (AutoVari)
                {
                    int x = loc.X;
                    int y = loc.Y;
                    int cols = ThingDb.FloorTiles[tile].numCols;
                    int rows = ThingDb.FloorTiles[tile].numRows;

                    Variation = (ushort)(((x+y) / 2 % cols) + (((y % rows) + 1 + cols - ((x + y) / 2) % cols) % rows) * cols);
                    
                }
                return (Variation);
            
        }
        byte CreateVariationW(Point loc, ushort Vari, ushort varia,bool faced)
        {

            ushort Variation = Vari;
            if (wallSelector.AutoVariW && faced)
            {
                int x = loc.X;
                int y = loc.Y;

                int cols = varia;

                Variation = (ushort)(((x) % cols));// + (((y % rows) + 1 + cols - ((x + y) / 2) % cols) % rows) * cols);

            }
            else if (wallSelector.AutoVariW)
            {
                Variation = 0;
            }
            return ((byte)Variation);

        }

        private void tmrInvalidate_Tick(object sender, EventArgs e)
        {
            mapPanel.Invalidate();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            AutoVari = checkBox1.Checked;
        }

       /* private void colorBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            switch(colorBox.SelectedItem.ToString())
            {
                case "Gray":
                    floorBrush = Brushes.Gray; break;
                case "Green":
                    floorBrush = Brushes.Green; break;
                case "Pink":
                    floorBrush = Brushes.Pink; break;
                case "Plum":
                    floorBrush = Brushes.Plum; break;
                case "PowderBlue":
                    floorBrush = Brushes.PowderBlue; break;
                case "Red":
                    floorBrush = Brushes.Red; break;
                case "LightGray":
                    floorBrush = Brushes.LightGray; break;
                case "DarkGray":
                    floorBrush = Brushes.DarkGray; break;
                case "Yellow":
                    floorBrush = Brushes.Yellow; break;
                case "YellowGreen":
                    floorBrush = Brushes.YellowGreen; break;
                case "White":
                    floorBrush = Brushes.White; break;
                case "Blue":
                    floorBrush = Brushes.Blue; break;
                case "Black":
                    floorBrush = Brushes.Black; break;
                case "Brown":
                    floorBrush = Brushes.Brown; break;
                default: floorBrush = Brushes.Gray; break;
            }
            
        }*/

        private void wallSelector_Load(object sender, EventArgs e)
        {

        }
        public void DrawWallsToSurface(IntPtr hdc)
        {
            //Graphics g = Graphics.FromHdcInternal(hdc);
            //Graphics g = Graphics.FromHdc(hdc);
            //Pen pen;
            //PointF center;
            /*foreach (Map.Wall wall in Map.Walls.Values)
            {
                Point pt = wall.Location;
                int x = pt.X * squareSize, y = pt.Y * squareSize;
                //TODO: how to draw if a destructable window? is this even possible?
                if (wall.Destructable)
                    pen = new Pen(Color.Red, wallThickness);
                else if (wall.Window)
                    pen = new Pen(Color.Orange, wallThickness);
                else if (wall.Secret)
                    pen = new Pen(Color.Green, wallThickness);
                else
                    pen = new Pen(Color.White, wallThickness);
                center = new PointF(x + squareSize / 2f, y + squareSize / 2f);
                Point nCorner = new Point(x, y);
                Point sCorner = new Point(x + squareSize, y + squareSize);
                Point wCorner = new Point(x + squareSize, y);
                Point eCorner = new Point(x, y + squareSize);
                switch (wall.Facing)
                {
                    case Map.Wall.WallFacing.NORTH:
                        g.DrawLine(pen, wCorner, eCorner);
                        break;
                    case Map.Wall.WallFacing.WEST:
                        g.DrawLine(pen, nCorner, sCorner);
                        break;
                    case Map.Wall.WallFacing.CROSS:
                        g.DrawLine(pen, wCorner, eCorner);//north wall
                        g.DrawLine(pen, nCorner, sCorner);//south wall
                        break;
                    case Map.Wall.WallFacing.NORTH_T:
                        g.DrawLine(pen, wCorner, eCorner);//north wall
                        g.DrawLine(pen, center, sCorner);//tail towards south
                        break;
                    case Map.Wall.WallFacing.SOUTH_T:
                        g.DrawLine(pen, wCorner, eCorner);//north wall
                        g.DrawLine(pen, center, nCorner);//tail towards north
                        break;
                    case Map.Wall.WallFacing.WEST_T:
                        g.DrawLine(pen, nCorner, sCorner);//west wall
                        g.DrawLine(pen, center, eCorner);//tail towards east
                        break;
                    case Map.Wall.WallFacing.EAST_T:
                        g.DrawLine(pen, nCorner, sCorner);//west wall
                        g.DrawLine(pen, center, wCorner);//tail towards west
                        break;
                    case Map.Wall.WallFacing.NE_CORNER:
                        g.DrawLine(pen, center, eCorner);
                        g.DrawLine(pen, center, sCorner);
                        break;
                    case Map.Wall.WallFacing.NW_CORNER:
                        g.DrawLine(pen, center, wCorner);
                        g.DrawLine(pen, center, sCorner);
                        break;
                    case Map.Wall.WallFacing.SW_CORNER:
                        g.DrawLine(pen, center, nCorner);
                        g.DrawLine(pen, center, wCorner);
                        break;
                    case Map.Wall.WallFacing.SE_CORNER:
                        g.DrawLine(pen, center, nCorner);
                        g.DrawLine(pen, center, eCorner);
                        break;
                    default:
                        g.DrawRectangle(pen, x, y, squareSize, squareSize);
                        g.DrawString("?", new Font("Arial", 12), Brushes.Red, nCorner);
                        break;
                }
                pen.Dispose();

                g.DrawString(wall.Minimap.ToString(), new Font("Arial", 10), Brushes.Red, x, y);
            }*/

        }

        private void chkTileGrid_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            ShowObjectNames = checkBox2.Checked;
        }

     /*   private void chkSnapCenter_CheckedChanged(object sender, EventArgs e)
        {
            SnapGrid = chkSnapCenter.Checked;
            SnapHalfGrid = false;
        }*/

        private void mapPanel_Leave(object sender, EventArgs e)
        {
           // KeyStates.Reset(); // If the focus leaves the pane stop auto tiling
        }

        private void mapPanel_MouseLeave(object sender, EventArgs e)
        {
            //KeyStates.Reset(); // If the mouse leaves the pane stop auto tiling
        }

       /* private void chkSnapFull_CheckedChanged(object sender, EventArgs e)
        {
            SnapGrid = false;
            SnapHalfGrid = chkSnapFull.Checked;
        }*/
        //
        // Add tiles to the screen
        //
        private void AddTile(Point tilePt)
        {
            if (CurrentMode == Mode.MAKE_FLOOR)
            {
                int tileGraph = (((ThingDb.Tile)(tileGraphic.SelectedItem)).Id);
                Map.Tile tile = Map.Tiles.ContainsKey(tilePt) ? Map.Tiles[tilePt] : null;
                if (tile == null && !threeFloorBox.Checked)
                {
                    ushort variation = CreateVariation(tilePt, (ushort)(tileVar.SelectedIndex-1), (byte)tileGraph);
                    ArrayList newblends = new ArrayList();
                    if (!chkAutoEdge.Checked)
                    {
                        for (int ve = 0; ve < blendDialog.Blends.Count; ve++)
                        {
                            Map.Tile.EdgeTile old = (Map.Tile.EdgeTile)blendDialog.Blends[ve];
                            Map.Tile.EdgeTile tr = new Map.Tile.EdgeTile(old.Graphic, (ushort)CreateVariation(tilePt, (ushort)old.Variation, (byte)old.Graphic), old.Dir, old.Edge);
                            newblends.Add(tr);
                        }
                    }
                    tile = new Map.Tile(
                        tilePt,
                        (byte)tileGraph,//tileGraphic.SelectedIndex,
                        variation,
                        newblends
                        );
                    unsafe
                    {
                        Map.Tiles.Add(tilePt, tile);
                        if (chkAutoEdge.Checked)
                        {
                            AutoEdge(tilePt);
                        }
                        IntPtr ptr = Marshal.StringToHGlobalAnsi(tile.Graphic);
                        MainWindow.myMap.AddTile(ptr.ToPointer(), tilePt.Y, tilePt.X, tile.Variation);
                        tilePt.X++;
                        //tileDrag = tilePt;  // Re-enable to allow tile dragging again
                        //dragging = true;
                    }

                }
                else if(threeFloorBox.Checked == true)
                {
                    unsafe
                    {
                        Point pat = new Point();
                        int i = 0;

                        int cols = 3;//ThingDb.FloorTiles[(byte)tileGraphic.SelectedIndex].numCols;
                        int rows = 3;//ThingDb.FloorTiles[(byte)tileGraphic.SelectedIndex].numRows;
                        for (pat = tilePt; i < rows; i++, pat.X--, pat.Y++)
                        {
                            for (int j = 0; j < cols; j++)
                            {
                                Point pat2 = new Point();
                                pat2 = pat;
                                pat2.X += j * 1;
                                pat2.Y += j * 1;
                                //pat2.X -= 1;
                                pat2.Y -= 2;
                                ushort variation = CreateVariation(pat2, (ushort)(tileVar.SelectedIndex-1), (byte)tileGraph);
                                ArrayList newblends = new ArrayList();
                                if (!chkAutoEdge.Checked)
                                {
                                    for (int ve = 0; ve < blendDialog.Blends.Count; ve++)
                                    {
                                        Map.Tile.EdgeTile old = (Map.Tile.EdgeTile)blendDialog.Blends[ve];
                                        Map.Tile.EdgeTile tr = new Map.Tile.EdgeTile(old.Graphic, (ushort)CreateVariation(pat2, (ushort)old.Variation, (byte)old.Graphic), old.Dir, old.Edge);
                                        newblends.Add(tr);
                                    }
                                }
                                tile = new Map.Tile(
                                    new Point(pat2.X, pat2.Y),
                                    (byte)tileGraph,//tileGraphic.SelectedIndex,
                                    variation,
                                    newblends
                                    );
                                IntPtr ptr = Marshal.StringToHGlobalAnsi(tile.Graphic);
                                if (!Map.Tiles.ContainsKey(tile.Location))
                                {
                                    Map.Tiles.Add(tile.Location, tile);
                                    if (chkAutoEdge.Checked)
                                    {
                                        AutoEdge(tile.Location);
                                    }
                                    MainWindow.myMap.AddTile(ptr.ToPointer(), pat2.Y, pat2.X, variation);
                                }
                            }
                        }
                    }
                }
            }
        }
        private void RemoveTile(Point tilePt)
        {
            if (CurrentMode == Mode.MAKE_FLOOR )
            {
                Map.Tiles.Remove(tilePt);
                MainWindow.myMap.RemoveTile(tilePt.X, tilePt.Y);
                if (threeFloorBox.Checked)
                {
                    unsafe
                    {
                        Point pat = new Point();
                        int i = 0;
                        int cols = 3;
                        int rows = 3;
                        for (pat = tilePt; i < rows; i++, pat.X--, pat.Y++)
                        {
                            for (int j = 0; j < cols; j++)
                            {
                                Point pat2 = new Point();
                                pat2 = pat;
                                pat2.X += j * 1;
                                pat2.Y += j * 1;
                                pat2.Y -= 2;
                                MainWindow.myMap.RemoveTile(pat2.X, pat2.Y);
                                Map.Tiles.Remove(new Point(pat2.X, pat2.Y));
                            }
                        }
                    }
                }
            }
        }

        // Need to code this next
        private void AutoEdge(Point pt)
        {
            Point tilePt = pt;//GetNearestTilePoint(pt);
            //Map.Tile.EdgeTile.Direction EdgeDir;

            //tilePt = GetNearestTilePoint(tilePt);

            int x = 0; // Holding marker
            int y = 0; // Holding marker
            Point temTile = tilePt; // Tile location reset

                    temTile = tilePt;
                    temTile.X += x;
                    temTile.Y += y;

                    Map.Tile tesTile = Map.Tiles[tilePt];
                    Map.Tile.EdgeTile.Direction Edir = Map.Tile.EdgeTile.Direction.South;

                    Map.Tile.EdgeTile.Direction dir1;
                    Map.Tile.EdgeTile.Direction dir2;
                    Map.Tile.EdgeTile.Direction dir3;
                    DirSettings DirSetting = new DirSettings();
                    DirSetting.Clear();
                    Random random = new Random();
                    //int variation = 0;

                    ///////////////////////////////////////////////////////////////////////
                    // 
                    // East Direction If Statement
                    // 
                    ///////////////////////////////////////////////////////////////////////
                    dir1 = Map.Tile.EdgeTile.Direction.East;
                    dir2 = Map.Tile.EdgeTile.Direction.North;
                    dir3 = Map.Tile.EdgeTile.Direction.South;
                    if (IsTileFromDir(tilePt, dir1))
                    {
                        bool done = false;
                        Map.Tile te1 = Map.Tiles[GetTileFromDir(tilePt, dir1)];
                        if (tesTile.Graphic != te1.Graphic && !IsOnIgnore(te1.Graphic))
                        {
                            if (IsTileFromDir(tilePt, dir2) && 
                                !DirSetting.NE && 
                                !DirSetting.SE && 
                                !DirSetting.NW &&
                                !done)
                            {
                                Map.Tile te2 = Map.Tiles[GetTileFromDir(tilePt, dir2)];
                                if (te1.Graphic == te2.Graphic)
                                {
                                    DirSetting.NE = true;
                                    Edir = Map.Tile.EdgeTile.Direction.NE_Sides;
                                    done = true;
                                }
                            }
                            if (IsTileFromDir(tilePt, dir3) && 
                                !DirSetting.SE && 
                                !DirSetting.NE &&
                                !DirSetting.SW &&
                                !done)
                            {
                                Map.Tile te2 = Map.Tiles[GetTileFromDir(tilePt, dir3)];
                                if (te1.Graphic == te2.Graphic)
                                {
                                    DirSetting.SE = true;
                                    Edir = Map.Tile.EdgeTile.Direction.SE_Sides;
                                    done = true;
                                }
                            }
                            if( !done && !DirSetting.NE && !DirSetting.SE )
                            {
                                int num = random.Next(3);
                                switch (num)
                                {
                                    case 0: Edir = Map.Tile.EdgeTile.Direction.East_D; break;
                                    case 1: Edir = Map.Tile.EdgeTile.Direction.East_E; break;
                                    default: Edir = dir1; break;
                                }
                                DirSetting.E = true;
                                done = true;
                            }
                            if (done)
                            {                               
                                Map.Tiles[tilePt].EdgeTiles.Add(new Map.Tile.EdgeTile(
                                            te1.graphicId,
                                            GetEdgeVariation(cboAutoEdge.Text,te1.Variation),
                                            Edir,
                                            (byte)cboAutoEdge.SelectedIndex));
                            }
                        }
                    }
                    /////////////////////////////////////////////////////////////////////// 
                    ///////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////////////////////////////////////////
                    // 
                    // North Direction If Statement
                    // 
                    ///////////////////////////////////////////////////////////////////////
                    dir1 = Map.Tile.EdgeTile.Direction.North;
                    dir2 = Map.Tile.EdgeTile.Direction.West;
                    dir3 = Map.Tile.EdgeTile.Direction.East;
                    if (IsTileFromDir(tilePt, dir1))
                    {
                        bool done = false;
                        Map.Tile te1 = Map.Tiles[GetTileFromDir(tilePt, dir1)];
                        if (tesTile.Graphic != te1.Graphic && !IsOnIgnore(te1.Graphic))
                        {
                            if (IsTileFromDir(tilePt, dir2) &&
                                !DirSetting.NW &&
                                !DirSetting.NE &&
                                !DirSetting.SW &&
                                !done)
                            {
                                Map.Tile te2 = Map.Tiles[GetTileFromDir(tilePt, dir2)];
                                if (te1.Graphic == te2.Graphic)
                                {
                                    DirSetting.NW = true;
                                    Edir = Map.Tile.EdgeTile.Direction.NW_Sides;
                                    done = true;
                                }
                            }
                            if (IsTileFromDir(tilePt, dir3) &&
                                !DirSetting.NE &&
                                !DirSetting.NW &&
                                !DirSetting.SE &&
                                !done)
                            {
                                Map.Tile te2 = Map.Tiles[GetTileFromDir(tilePt, dir3)];
                                if (te1.Graphic == te2.Graphic)
                                {
                                    DirSetting.NE = true;
                                    Edir = Map.Tile.EdgeTile.Direction.NE_Sides;
                                    done = true;
                                }
                            }
                            if( !done && !DirSetting.NE && !DirSetting.NW)
                            {
                                int num = random.Next(3);
                                switch (num)
                                {
                                    case 0: Edir = Map.Tile.EdgeTile.Direction.North_08; break;
                                    case 1: Edir = Map.Tile.EdgeTile.Direction.North_0A; break;
                                    default: Edir = dir1; break;
                                }
                                DirSetting.N = true;
                                done = true;
                            }
                            if (done)
                            {
                                Map.Tiles[tilePt].EdgeTiles.Add(new Map.Tile.EdgeTile(
                                            te1.graphicId,
                                            GetEdgeVariation(cboAutoEdge.Text, te1.Variation),
                                            Edir,
                                            (byte)cboAutoEdge.SelectedIndex));
                            }
                        }
                    }
                    /////////////////////////////////////////////////////////////////////// 
                    ///////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////////////////////////////////////////
                    // 
                    // West Direction If Statement
                    // 
                    ///////////////////////////////////////////////////////////////////////
                    dir1 = Map.Tile.EdgeTile.Direction.West;
                    dir2 = Map.Tile.EdgeTile.Direction.South;
                    dir3 = Map.Tile.EdgeTile.Direction.North;
                    if (IsTileFromDir(tilePt, dir1))
                    {
                        bool done = false;
                        Map.Tile te1 = Map.Tiles[GetTileFromDir(tilePt, dir1)];
                        if (tesTile.Graphic != te1.Graphic && !IsOnIgnore(te1.Graphic))
                        {
                            if (IsTileFromDir(tilePt, dir2) &&
                                !DirSetting.NW &&
                                !DirSetting.SW &&
                                !DirSetting.SE &&
                                !done)
                            {
                                Map.Tile te2 = Map.Tiles[GetTileFromDir(tilePt, dir2)];
                                if (te1.Graphic == te2.Graphic)
                                {
                                    DirSetting.SW = true;
                                    Edir = Map.Tile.EdgeTile.Direction.SW_Sides;
                                    done = true;
                                }
                            }
                            if (IsTileFromDir(tilePt, dir3) &&
                                !DirSetting.NW &&
                                !DirSetting.NE &&
                                !DirSetting.SW &&
                                !done)
                            {
                                Map.Tile te2 = Map.Tiles[GetTileFromDir(tilePt, dir3)];
                                if (te1.Graphic == te2.Graphic)
                                {
                                    DirSetting.NW = true;
                                    Edir = Map.Tile.EdgeTile.Direction.NW_Sides;
                                    done = true;
                                }
                            }
                            if (!done && !DirSetting.NW && !DirSetting.SW)
                            {
                                int num = random.Next(3);
                                switch (num)
                                {
                                    case 0: Edir = Map.Tile.EdgeTile.Direction.West_02; break;
                                    case 1: Edir = Map.Tile.EdgeTile.Direction.West_03; break;
                                    default: Edir = dir1; break;
                                }
                                DirSetting.W = true;
                                done = true;
                            }
                            if (done)
                            {
                                Map.Tiles[tilePt].EdgeTiles.Add(new Map.Tile.EdgeTile(
                                            te1.graphicId,
                                            GetEdgeVariation(cboAutoEdge.Text, te1.Variation),
                                            Edir,
                                            (byte)cboAutoEdge.SelectedIndex));
                            }
                        }
                    }
                    /////////////////////////////////////////////////////////////////////// 
                    ///////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////////////////////////////////////////
                    // 
                    // South Direction If Statement
                    // 
                    ///////////////////////////////////////////////////////////////////////
                    dir1 = Map.Tile.EdgeTile.Direction.South;
                    dir2 = Map.Tile.EdgeTile.Direction.East;
                    dir3 = Map.Tile.EdgeTile.Direction.West;
                    if (IsTileFromDir(tilePt, dir1))
                    {
                        bool done = false;
                        Map.Tile te1 = Map.Tiles[GetTileFromDir(tilePt, dir1)];
                        if (tesTile.Graphic != te1.Graphic && !IsOnIgnore(te1.Graphic))
                        {
                            if (IsTileFromDir(tilePt, dir2) &&
                                !DirSetting.SE &&
                                !DirSetting.SW &&
                                !DirSetting.NE &&
                                !done)
                            {
                                Map.Tile te2 = Map.Tiles[GetTileFromDir(tilePt, dir2)];
                                if (te1.Graphic == te2.Graphic)
                                {
                                    DirSetting.SE = true;
                                    Edir = Map.Tile.EdgeTile.Direction.SE_Sides;
                                    done = true;
                                }
                            }
                            if (IsTileFromDir(tilePt, dir3) &&
                                !DirSetting.SW &&
                                !DirSetting.SE &&
                                !DirSetting.NW &&
                                !done)
                            {
                                Map.Tile te2 = Map.Tiles[GetTileFromDir(tilePt, dir3)];
                                if (te1.Graphic == te2.Graphic)
                                {
                                    DirSetting.SW = true;
                                    Edir = Map.Tile.EdgeTile.Direction.SW_Sides;
                                    done = true;
                                }
                            }
                            if (!done && !DirSetting.SW && !DirSetting.SE)
                            {
                                int num = random.Next(3);
                                switch (num)
                                {
                                    case 0: Edir = Map.Tile.EdgeTile.Direction.South_07; break;
                                    case 1: Edir = Map.Tile.EdgeTile.Direction.South_09; break;
                                    default: Edir = dir1; break;
                                }
                                DirSetting.S = true;
                                done = true;
                            }
                            if (done)
                            {
                                Map.Tiles[tilePt].EdgeTiles.Add(new Map.Tile.EdgeTile(
                                            te1.graphicId,
                                            GetEdgeVariation(cboAutoEdge.Text, te1.Variation),
                                            Edir,
                                            (byte)cboAutoEdge.SelectedIndex));
                            }
                        }
                    }
                    /////////////////////////////////////////////////////////////////////// 
                    ///////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////////////////////////////////////////
                    // 
                    // NE Tip Direction If Statement
                    // 
                    ///////////////////////////////////////////////////////////////////////
                    dir1 = Map.Tile.EdgeTile.Direction.NE_Tip;
                    if (IsTileFromDir(tilePt, dir1))
                    {
                        Map.Tile te1 = Map.Tiles[GetTileFromDir(tilePt, dir1)];
                        if (tesTile.Graphic != te1.Graphic && !IsOnIgnore(te1.Graphic))
                        {
                            if (!DirSetting.NE && !DirSetting.NW && !DirSetting.SE && !DirSetting.N && !DirSetting.E)
                            {
                                Edir = dir1;
                                Map.Tiles[tilePt].EdgeTiles.Add(new Map.Tile.EdgeTile(
                                                                te1.graphicId,
                                                                GetEdgeVariation(cboAutoEdge.Text, te1.Variation),
                                                                Edir,
                                                                (byte)cboAutoEdge.SelectedIndex));
                            }
                        }
                    }
                    /////////////////////////////////////////////////////////////////////// 
                    ///////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////////////////////////////////////////
                    // 
                    // NW Tip Direction If Statement
                    // 
                    ///////////////////////////////////////////////////////////////////////
                    dir1 = Map.Tile.EdgeTile.Direction.NW_Tip;
                    if (IsTileFromDir(tilePt, dir1))
                    {
                        Map.Tile te1 = Map.Tiles[GetTileFromDir(tilePt, dir1)];
                        if (tesTile.Graphic != te1.Graphic && !IsOnIgnore(te1.Graphic))
                        {
                            if (!DirSetting.NW && !DirSetting.NE && !DirSetting.SW && !DirSetting.N && !DirSetting.W)
                            {
                                Edir = dir1;
                                Map.Tiles[tilePt].EdgeTiles.Add(new Map.Tile.EdgeTile(
                                                                te1.graphicId,
                                                                GetEdgeVariation(cboAutoEdge.Text, te1.Variation),
                                                                Edir,
                                                                (byte)cboAutoEdge.SelectedIndex));
                            }
                        }
                    }
                    /////////////////////////////////////////////////////////////////////// 
                    ///////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////////////////////////////////////////
                    // 
                    // SE Tip Direction If Statement
                    // 
                    ///////////////////////////////////////////////////////////////////////
                    dir1 = Map.Tile.EdgeTile.Direction.SE_Tip;
                    if (IsTileFromDir(tilePt, dir1))
                    {
                        Map.Tile te1 = Map.Tiles[GetTileFromDir(tilePt, dir1)];
                        if (tesTile.Graphic != te1.Graphic && !IsOnIgnore(te1.Graphic))
                        {
                            if (!DirSetting.SE && !DirSetting.NE && !DirSetting.SW && !DirSetting.S && !DirSetting.E)
                            {
                                Edir = dir1;
                                Map.Tiles[tilePt].EdgeTiles.Add(new Map.Tile.EdgeTile(
                                                                te1.graphicId,
                                                                GetEdgeVariation(cboAutoEdge.Text, te1.Variation),
                                                                Edir,
                                                                (byte)cboAutoEdge.SelectedIndex));
                            }
                        }
                    }
                    /////////////////////////////////////////////////////////////////////// 
                    ///////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////////////////////////////////////////
                    // 
                    // SW Tip Direction If Statement
                    // 
                    ///////////////////////////////////////////////////////////////////////
                    dir1 = Map.Tile.EdgeTile.Direction.SW_Tip;
                    if (IsTileFromDir(tilePt, dir1))
                    {
                        Map.Tile te1 = Map.Tiles[GetTileFromDir(tilePt, dir1)];
                        if (tesTile.Graphic != te1.Graphic && !IsOnIgnore(te1.Graphic))
                        {
                            if (!DirSetting.SW && !DirSetting.NW && !DirSetting.SE && !DirSetting.W && !DirSetting.S)
                            {
                                Edir = dir1;
                                Map.Tiles[tilePt].EdgeTiles.Add(new Map.Tile.EdgeTile(
                                                                te1.graphicId,
                                                                GetEdgeVariation(cboAutoEdge.Text, te1.Variation),
                                                                Edir,
                                                                (byte)cboAutoEdge.SelectedIndex));
                            }
                        }
                    }
                    /////////////////////////////////////////////////////////////////////// 
                    ///////////////////////////////////////////////////////////////////////  
        }
        private ushort GetEdgeVariation(string EdgeName, int TileVari)
        {
            Random ran = new Random();
            switch (EdgeName)
            {
                case "MudEdge": return((ushort)ran.Next(4));
                default: return (ushort)TileVari;
            }
        }
        private bool IsTileFromDir(Point tilePt, Map.Tile.EdgeTile.Direction EdgeDir)
        {
            return((GetTileFromDir(tilePt,EdgeDir) != tilePt));
        }
        private Point GetTileFromDir(Point tilePt, Map.Tile.EdgeTile.Direction EdgeDir)
        {
            Point temPt = tilePt;

               switch(EdgeDir)
               {
                   case Map.Tile.EdgeTile.Direction.East:
                   case Map.Tile.EdgeTile.Direction.East_D:
                   case Map.Tile.EdgeTile.Direction.East_E:
                       {
                       temPt.X += 1;
                       temPt.Y += -1;
                       }break;

                   //case Map.Tile.EdgeTile.Direction.NE_Sides:
                    //   temPt.X +=
                     //  temPt.Y += 
                      // break;

                   case Map.Tile.EdgeTile.Direction.NE_Tip:
                       {
                       temPt.X += 0;
                       temPt.Y += -2;
                       }break;

                   case Map.Tile.EdgeTile.Direction.North:
                   case Map.Tile.EdgeTile.Direction.North_08:
                   case Map.Tile.EdgeTile.Direction.North_0A:
                       {
                       temPt.X += -1;
                       temPt.Y += -1;
                       }break;

                   //case Map.Tile.EdgeTile.Direction.NW_Sides:
                    //   temPt.X +=
                     //  temPt.Y += 
                      // break;

                   case Map.Tile.EdgeTile.Direction.NW_Tip:
                       {
                       temPt.X += -2;
                       temPt.Y += 0;
                       }break;

                  // case Map.Tile.EdgeTile.Direction.SE_Sides:
                   //    temPt.X +=
                    //   temPt.Y += 
                     //  break;

                   case Map.Tile.EdgeTile.Direction.SE_Tip:
                       {
                       temPt.X += 2;
                       temPt.Y += 0;
                       }break;

                   case Map.Tile.EdgeTile.Direction.South:
                   case Map.Tile.EdgeTile.Direction.South_07:
                   case Map.Tile.EdgeTile.Direction.South_09:
                       {
                       temPt.X += 1;
                       temPt.Y += 1;
                       }break;

                 //  case Map.Tile.EdgeTile.Direction.SW_Sides:
                  //     temPt.X +=
                   //    temPt.Y += 
                    //   break;

                   case Map.Tile.EdgeTile.Direction.SW_Tip:
                       {
                       temPt.X += 0;
                       temPt.Y += 2;
                       }break;

                   case Map.Tile.EdgeTile.Direction.West:
                   case Map.Tile.EdgeTile.Direction.West_02:
                   case Map.Tile.EdgeTile.Direction.West_03:
                       {
                       temPt.X += -1;
                       temPt.Y += 1;
                       }break;
                   default:break;
               }
               if (!Map.Tiles.ContainsKey(temPt))
               {
                   temPt = tilePt;
               }
               return temPt;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SetMode(Mode.EDIT_EDGE);
            //CurrentMode = Mode.EDIT_EDGE;//set new mode
            currentButton = (Button)sender;//update current button	
            mapPanel.Invalidate();
        }
        private bool IsOnIgnore(string t1)
        {
            if (!chkIgnoreOn.Checked)
                return false;

            if ( t1 == (((ThingDb.Tile)(cboEdgeIgnoreBox.SelectedItem)).Name))
                return true;
            else
                return false;

        }
        private void SetMode(Mode mode)
        {
            CurrentMode = mode;
            KeyStates.Reset();
        }
        private void RemoveEdge(Point tilePt)
        {
            if (Map.Tiles.ContainsKey(tilePt))
                Map.Tiles[tilePt].EdgeTiles.Clear();
        }
        private void EditEdge(Point tilePt)
        {
            if (chkAutoEdge.Checked)
            {
                if (Map.Tiles.ContainsKey(tilePt))
                {
                    Map.Tiles[tilePt].EdgeTiles.Clear();
                    AutoEdge(tilePt);
                }
            }
            else
            {
                if (Map.Tiles.ContainsKey(tilePt))
                {
                    Map.Tiles[tilePt].EdgeTiles.Clear();
                    ArrayList newblends = new ArrayList();
                    for (int ve = 0; ve < blendDialog.Blends.Count; ve++)
                    {
                        Map.Tile.EdgeTile old = (Map.Tile.EdgeTile)blendDialog.Blends[ve];
                        Map.Tile.EdgeTile tr = new Map.Tile.EdgeTile(old.Graphic, (ushort)CreateVariation(tilePt, (ushort)old.Variation, (byte)old.Graphic), old.Dir, old.Edge);
                        Map.Tiles[tilePt].EdgeTiles.Add(tr);
                    }
                }
            }
        }

        private void cboObjCreate_SelectedIndexChanged(object sender, EventArgs e)
        {
            unsafe
            {
                IntPtr ptr = Marshal.StringToHGlobalAnsi((string)cboObjCreate.SelectedItem);
                IntPtr ptr2 = Marshal.StringToHGlobalAnsi(Application.StartupPath + "\\object.bmp");
                MainWindow.myMap.SaveObjectToBMP(ptr.ToPointer(), ptr2.ToPointer());
                pictureBox1.ImageLocation = Application.StartupPath + "\\object.bmp";
            }

        }
        public static void SetWallBox(PictureBox box, string val)
        {
            /*unsafe
            {
                IntPtr ptr = Marshal.StringToHGlobalAnsi(val);
                IntPtr ptr2 = Marshal.StringToHGlobalAnsi(Application.StartupPath + "\\wall.bmp");
                MainWindow.myMap.SaveWallToBMP(ptr.ToPointer(), ptr2.ToPointer());
                box.ImageLocation = Application.StartupPath + "\\wall.bmp";
            }*/

        }

        private void buttonSelWay_Click(object sender, EventArgs e)
        {
            SetMode(Mode.SELECT_WAYPOINT);
        }

        private void wpNameBox_TextChanged(object sender, EventArgs e)
        {
            if (KeyStates.wp1 != null && CurrentMode == Mode.SELECT_WAYPOINT)
            {
                KeyStates.wp1.Name = wpNameBox.Text;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (OverrideChecking == true)
                OverrideChecking = false;
            else if (KeyStates.wp1 != null && CurrentMode == Mode.SELECT_WAYPOINT)
            {
                KeyStates.wp1.enabled = checkBox3.Checked ? 1 : 0;
                //KeyStates.wp1.enabled = Convert.ToInt16(checkBox3.Checked);
            }
        }

        private void txtWpDefault_TextChanged(object sender, EventArgs e)
        {

        }

        private void radFullSnap_CheckedChanged(object sender, EventArgs e)
        {
            SnapGrid = false;
            SnapHalfGrid = radFullSnap.Checked;
            //radCenterSnap.Checked = false;
            //radNoSnap.Checked = false;
        }

        private void radCenterSnap_CheckedChanged(object sender, EventArgs e)
        {
            SnapGrid = radCenterSnap.Checked;
            SnapHalfGrid = false;
            //radNoSnap.Checked = false;
            //radFullSnap.Checked = false;
        }

        private void radNoSnap_CheckedChanged(object sender, EventArgs e)
        {
            SnapGrid = false;
            SnapHalfGrid = false;
            //radCenterSnap.Checked = false;
            //radFullSnap.Checked = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            ShowExtents = false;
            ShowAllExtents = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ShowExtents = true;
            ShowAllExtents = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            ShowExtents = true;
            ShowAllExtents = true;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            ShowWaypoints = checkBox4.Checked;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            ShowPolygons = checkBox5.Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            DrawWalls3D = checkBox6.Checked;
        }

        private void mapPanel_Resize(object sender, EventArgs e)
        {
         /*   if (mapPanel.Width > 0 && mapPanel.Height > 0)
            {
                MainWindow.myMap.ReInit(mapPanel.Width, mapPanel.Height);
                MainWindow.myMap.Update_Window();
            }*/
        }

    }

}
