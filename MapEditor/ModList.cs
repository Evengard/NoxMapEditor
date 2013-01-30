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

namespace MapEditor
{
    public partial class ModList : Form
    {
        public ModList()
        {
            InitializeComponent();
            foreach (ThingDb.Thing tng in ThingDb.Things.Values)
            {
                listObjects.Items.Add(tng.Name.ToLower());
            }

            foreach (string str in Directory.GetFiles(Application.StartupPath + "\\scripts\\objects\\modeditors\\"))
            {
                if (str == "Update.txt")
                { }
                else
                {
                    string[] strs = str.Split("\\".ToCharArray());
                    string str2 = strs[strs.GetLength(0)-1];
                    str2 = str2.Remove(str2.Length - 4);
                    if(listObjects.Items.Contains(str2.ToLower()))
                    {
                        listObjects.Items.Remove(str2);
                    }
                    
                    listContents.Items.Add(str2);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listObjects.SelectedItem != null)
            {
                listContents.Items.Insert(0,listObjects.SelectedItem);
                File.Create(Application.StartupPath + "\\scripts\\objects\\modeditors\\" + listObjects.SelectedItem + ".txt");
                listObjects.Items.Remove(listObjects.SelectedItem);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listContents.SelectedItem != null)
            {
                listObjects.Items.Insert(0, listContents.SelectedItem);
                File.Delete(Application.StartupPath + "\\scripts\\objects\\modeditors\\" + listContents.SelectedItem + ".txt");
                listContents.Items.Remove(listContents.SelectedItem);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listContents.SelectedItem != null)
            {
                StreamReader rR = File.OpenText(Application.StartupPath + "\\scripts\\objects\\modeditors\\" + listContents.SelectedItem + ".txt");
                textBox1.Text = rR.ReadToEnd();
                rR.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listContents.SelectedItem != null)
            {
                FileStream file = File.Create(Application.StartupPath + "\\scripts\\objects\\modeditors\\" + listContents.SelectedItem + ".txt");
                StreamWriter rW = new StreamWriter(file);
                rW.Write(textBox1.Text);
                rW.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FileStream file = File.Create(Application.StartupPath + "\\scripts\\objects\\modeditors\\Update.txt");
            StreamWriter rW = new StreamWriter(file);
            foreach (string str in listContents.Items )
            {
                rW.Write(str + ".txt;" + DateTime.Now + ";false;" + "        'dfsdf\n");
            }
            rW.Close();
        }
    }
}