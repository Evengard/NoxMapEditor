using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NoxShared;

namespace NoxMapEditor
{
	public partial class ExportDialog : Form
	{
		private Map map;
		public Map pMap {
		    get
            {
                return map;
            }
            set
            {
                map = value;
            }
		}

		public ExportDialog()
		{
			InitializeComponent();
		}

		private void categoryList_SelectedIndexChanged(object sender, EventArgs e)
		{
			itemList.Items.Clear();
			switch (((ListBox)sender).SelectedItem.ToString()){
				case "Objects":
					itemList.Items.AddRange(map.Objects.ToArray());
					break;
				case "Tiles":
					foreach(Map.Tile t in map.Tiles.Values)
						itemList.Items.Add(t);
					break;
				case "Walls":
					foreach (Map.Wall w in map.Walls.Values)
						itemList.Items.Add(w);
					break;
				case "Polygons":
					itemList.Items.AddRange(map.Polygons.ToArray());
					break;
				case "Scripts":
					itemList.Items.AddRange(map.Scripts.Funcs.ToArray());
					break;
				case "Groups":
					foreach (Map.Group g in map.Groups.Values)
						itemList.Items.Add(g);
					break;
			}
		}

		private void exportButton_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "XML Document|*.xml";
			sfd.DefaultExt = "xml";
			sfd.AddExtension = true;
			sfd.OverwritePrompt = true;
			sfd.ShowDialog();
			System.IO.Stream stream = sfd.OpenFile();
			foreach (Object o in itemList.SelectedItems)
				XML.exportClassToXml(o, stream);
			stream.Close();
		}
	}
}