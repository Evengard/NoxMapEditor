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
	public partial class WaypointPropertiesDialog : Form
	{

		public Map.WaypointList wplist
		{
			get
			{
				return wpList;
			}
			set
			{
				wpList = value;
				wpBox.Items.Clear();
				foreach (Map.Waypoint w in wpList)
					wpBox.Items.Add(w);
			}
		}
		private Map.WaypointList wpList;
		public Map.Waypoint wpPub
		{
			get
			{
				return wp;
			}
			set
			{
				wp = value;
				nameText.Text = wp.Name;
				enabledCheck.Checked = wp.enabled == 1;
				connList.Items.Clear();
				foreach (Map.Waypoint.WaypointConnection c in wp.connections)
					connList.Items.Add(c);
			}
		}
		private Map.Waypoint wp;

		public WaypointPropertiesDialog()
		{
			InitializeComponent();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			wp.Name = nameText.Text;
			wp.enabled = enabledCheck.Checked ? 1 : 0;
			wp.connections.Clear();
			wp.connections.AddRange(connList.Items);
			Close();
		}

		private void addButton_Click(object sender, EventArgs e)
		{
			connList.Items.Add(new Map.Waypoint.WaypointConnection((Map.Waypoint)wpBox.SelectedItem, Byte.Parse(flagText.Text)));
			wpBox.Text = null;
		}

		private void delButton_Click(object sender, EventArgs e)
		{
			if(connList.SelectedItem != null)
				connList.Items.Remove(connList.SelectedItem);
		}

        private void enabledCheck_CheckedChanged(object sender, EventArgs e)
        {

        }
	}
}