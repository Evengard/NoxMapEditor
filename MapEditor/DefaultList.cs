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
    public partial class DefaultList : Form
    {
        Map.ObjectTable table;
        public DefaultList(Map.ObjectTable tabl)
        {
            table = tabl;
            InitializeComponent();
            foreach (ThingDb.Thing tng in ThingDb.Things.Values)
            {
                listDefault.Items.Add(tng.Name.ToLower());
            }
            foreach (string str in Directory.GetFiles(Application.StartupPath + "\\scripts\\objects\\defaultmods\\"))
            {
                if (str == "Update.txt")
                { }
                else
                {
                    string[] strs = str.Split("\\".ToCharArray());
                    string str2 = strs[strs.GetLength(0) - 1];
                    str2 = str2.Remove(str2.Length - 4);
                    if (listDefault.Items.Contains(str2.ToLower()))
                    {
                        listDefault.Items.Remove(str2.ToLower());
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileStream file = File.Create(Application.StartupPath + "\\scripts\\objects\\defaultmods\\Update.txt");
            StreamWriter rW = new StreamWriter(file);
            foreach (string str in Directory.GetFiles(Application.StartupPath + "\\scripts\\objects\\defaultmods\\"))
            {
                string[] strs = str.Split("\\".ToCharArray());
                string str2 = strs[strs.GetLength(0) - 1];
                str2 = str2.Remove(str2.Length - 4);
                if (str2 == "Update.txt")
                { }
                else
                    rW.Write(str2 + ".txt;" + DateTime.Now + ";false;" + "        'dfsdf\n");
            }
            rW.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int count = 0;
            foreach (Map.Object obj in table)
            {
                if (listDefault.Items.Contains(obj.Name.ToLower()))
                {
                    listDefault.Items.Remove(obj.Name.ToLower());
                    string mod = "";
                    string m_ExePath = Process.GetCurrentProcess().MainModule.FileName;
                    m_ExePath = Path.GetDirectoryName(m_ExePath);
                    StreamWriter SR;
                    SR = File.CreateText(m_ExePath + "\\scripts\\objects\\defaultmods\\" + obj.Name.ToLower() + ".txt");
                    if (SR != null)
                     {
                          SR.WriteLine((short)obj.Properties);
                         SR.WriteLine(obj.Terminator);
                        foreach (byte b in obj.modbuf)
                          mod += String.Format("{0:x2} ", b);
                      SR.Write(mod);
                     }
                    SR.Close();
                    count++;
                 }
            }
            txtTotal.Text = count.ToString();
        }
    }
}