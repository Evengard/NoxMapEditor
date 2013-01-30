namespace MapEditor
{
    partial class ModEditor
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
            this.txtval = new System.Windows.Forms.TextBox();
            this.lblname = new System.Windows.Forms.Label();
            this.lblextent = new System.Windows.Forms.Label();
            this.listPresets = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listLoop = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // txtval
            // 
            this.txtval.Location = new System.Drawing.Point(211, 319);
            this.txtval.Name = "txtval";
            this.txtval.Size = new System.Drawing.Size(190, 20);
            this.txtval.TabIndex = 2;
            this.txtval.Visible = false;
            this.txtval.TextChanged += new System.EventHandler(this.txtval_TextChanged);
            // 
            // lblname
            // 
            this.lblname.AutoSize = true;
            this.lblname.Location = new System.Drawing.Point(1, 0);
            this.lblname.Name = "lblname";
            this.lblname.Size = new System.Drawing.Size(66, 13);
            this.lblname.TabIndex = 6;
            this.lblname.Text = "ObjectName";
            // 
            // lblextent
            // 
            this.lblextent.AutoSize = true;
            this.lblextent.Location = new System.Drawing.Point(135, 0);
            this.lblextent.Name = "lblextent";
            this.lblextent.Size = new System.Drawing.Size(70, 13);
            this.lblextent.TabIndex = 8;
            this.lblextent.Text = "Object extent";
            // 
            // listPresets
            // 
            this.listPresets.FormattingEnabled = true;
            this.listPresets.Location = new System.Drawing.Point(209, 201);
            this.listPresets.Name = "listPresets";
            this.listPresets.Size = new System.Drawing.Size(192, 95);
            this.listPresets.TabIndex = 10;
            this.listPresets.SelectedIndexChanged += new System.EventHandler(this.listPresets_SelectedIndexChanged);
            this.listPresets.Click += new System.EventHandler(this.listPresets_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(208, 303);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Item Value";
            // 
            // listLoop
            // 
            this.listLoop.FormattingEnabled = true;
            this.listLoop.Location = new System.Drawing.Point(209, 22);
            this.listLoop.Name = "listLoop";
            this.listLoop.Size = new System.Drawing.Size(192, 147);
            this.listLoop.TabIndex = 13;
            this.listLoop.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listLoop_MouseClick);
            this.listLoop.DoubleClick += new System.EventHandler(this.listLoop_DoubleClick);
            this.listLoop.SelectedIndexChanged += new System.EventHandler(this.listLoop_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(209, 175);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 20);
            this.button1.TabIndex = 14;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(326, 175);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 20);
            this.button2.TabIndex = 15;
            this.button2.Text = "Del";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(4, 22);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(201, 317);
            this.treeView1.TabIndex = 16;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // ModEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 349);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listLoop);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listPresets);
            this.Controls.Add(this.lblextent);
            this.Controls.Add(this.lblname);
            this.Controls.Add(this.txtval);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ModEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ModEditor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ModEditor_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtval;
        private System.Windows.Forms.Label lblname;
        private System.Windows.Forms.Label lblextent;
        private System.Windows.Forms.ListBox listPresets;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listLoop;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TreeView treeView1;
    }
}