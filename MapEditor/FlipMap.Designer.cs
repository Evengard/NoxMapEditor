namespace NoxMapEditor
{
    partial class FlipMap
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
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.chkFlipWall = new System.Windows.Forms.CheckBox();
            this.chkFlipOb = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.minimapGroup = new System.Windows.Forms.TextBox();
            this.labelMinimapGroup = new System.Windows.Forms.Label();
            this.chkAdv = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(44, 82);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(154, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Divide1";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(44, 111);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(154, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "Divide2";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(217, 64);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "Before using this control keep in mind that the tiles and waypoints are still bug" +
                "gy, for the time being, please do not use on maps with tiles or waypoints. I wil" +
                "l try to fix this soon.";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // chkFlipWall
            // 
            this.chkFlipWall.AutoSize = true;
            this.chkFlipWall.Location = new System.Drawing.Point(48, 139);
            this.chkFlipWall.Name = "chkFlipWall";
            this.chkFlipWall.Size = new System.Drawing.Size(71, 17);
            this.chkFlipWall.TabIndex = 5;
            this.chkFlipWall.Text = "Flip Walls";
            this.chkFlipWall.UseVisualStyleBackColor = true;
            // 
            // chkFlipOb
            // 
            this.chkFlipOb.AutoSize = true;
            this.chkFlipOb.Location = new System.Drawing.Point(48, 162);
            this.chkFlipOb.Name = "chkFlipOb";
            this.chkFlipOb.Size = new System.Drawing.Size(81, 17);
            this.chkFlipOb.TabIndex = 6;
            this.chkFlipOb.Text = "Flip Objects";
            this.chkFlipOb.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(48, 244);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(104, 21);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 228);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Copied Walls Material";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // minimapGroup
            // 
            this.minimapGroup.Location = new System.Drawing.Point(91, 271);
            this.minimapGroup.Name = "minimapGroup";
            this.minimapGroup.Size = new System.Drawing.Size(28, 20);
            this.minimapGroup.TabIndex = 17;
            // 
            // labelMinimapGroup
            // 
            this.labelMinimapGroup.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelMinimapGroup.Location = new System.Drawing.Point(46, 271);
            this.labelMinimapGroup.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.labelMinimapGroup.Name = "labelMinimapGroup";
            this.labelMinimapGroup.Size = new System.Drawing.Size(56, 28);
            this.labelMinimapGroup.TabIndex = 18;
            this.labelMinimapGroup.Text = "Minimap Group";
            // 
            // chkAdv
            // 
            this.chkAdv.AutoSize = true;
            this.chkAdv.Location = new System.Drawing.Point(48, 199);
            this.chkAdv.Name = "chkAdv";
            this.chkAdv.Size = new System.Drawing.Size(136, 17);
            this.chkAdv.TabIndex = 19;
            this.chkAdv.Text = "Use Advanced Options";
            this.chkAdv.UseVisualStyleBackColor = true;
            // 
            // FlipMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(240, 302);
            this.Controls.Add(this.chkAdv);
            this.Controls.Add(this.minimapGroup);
            this.Controls.Add(this.labelMinimapGroup);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.chkFlipOb);
            this.Controls.Add(this.chkFlipWall);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Name = "FlipMap";
            this.Text = "FlipMap";
            this.Load += new System.EventHandler(this.FlipMap_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox chkFlipWall;
        private System.Windows.Forms.CheckBox chkFlipOb;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox minimapGroup;
        private System.Windows.Forms.Label labelMinimapGroup;
        private System.Windows.Forms.CheckBox chkAdv;
    }
}