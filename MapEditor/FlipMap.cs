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
    public partial class FlipMap : Form
    {
        public MapView view;
        public FlipMap()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int mapsize = 255 * 23;
            foreach (Map.Wall wall in view.Map.Walls.Values)
            {
                byte val = (byte)wall.Location.X;
                val = (byte)~val;
                wall.Location.X = (int)val-1;
                switch (wall.Facing)
                {
                    case Map.Wall.WallFacing.NORTH:
                        wall.Facing = Map.Wall.WallFacing.WEST; break;
                    case Map.Wall.WallFacing.WEST:
                        wall.Facing = Map.Wall.WallFacing.NORTH; break;

                    case Map.Wall.WallFacing.NORTH_T:
                        wall.Facing = Map.Wall.WallFacing.WEST_T; break;
                    case Map.Wall.WallFacing.WEST_T:
                        wall.Facing = Map.Wall.WallFacing.NORTH_T; break;

                    case Map.Wall.WallFacing.EAST_T:
                        wall.Facing = Map.Wall.WallFacing.SOUTH_T; break;
                    case Map.Wall.WallFacing.SOUTH_T:
                        wall.Facing = Map.Wall.WallFacing.EAST_T; break;

                    case Map.Wall.WallFacing.SE_CORNER:
                        wall.Facing = Map.Wall.WallFacing.NW_CORNER; break;
                    case Map.Wall.WallFacing.NW_CORNER:
                        wall.Facing = Map.Wall.WallFacing.SE_CORNER; break;
                }
            }

            /*foreach (Map.Polygon poly in view.Map.Polygons)
            {
                foreach (PointF pt in poly.Points)
                {
                    float val = pt.X;
                    val = (float)((float)(mapsize) - (float)(val));
                    pt.X = (float)val;
                }
            }*/
            /* foreach (Map.Tile til in view.Map.Tiles.Values)
            {
                byte val = (byte)til.Location.X;
                val = (byte)~val;
                til.Location.X = (int)val-1;
                // foreach (Map.Tile.EdgeTile edg in til.EdgeTiles)
                // {
                //     edg.Dir = Map.Tile.EdgeTile.Direction.
                // }
            }*/

            foreach (Map.Waypoint wpt in view.Map.Waypoints)
            {
                float val = wpt.Point.X;
                val = (float)((float)(mapsize) - (float)(val));
                wpt.Point.X = (float)val;
            }
            foreach (Map.Object tng in view.Map.Objects)
            {
                float val = tng.Location.X;
                val = (float)((float)(mapsize) - (float)(val));
                tng.Location.X = (float)val;  
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int mapsize = 255 * 23;
            foreach (Map.Wall wall in view.Map.Walls.Values)
            {
                byte val = (byte)wall.Location.Y;
                val = (byte)~val;
                wall.Location.Y = (int)val - 1;
                switch (wall.Facing)
                {
                    case Map.Wall.WallFacing.NORTH:
                        wall.Facing = Map.Wall.WallFacing.WEST; break;
                    case Map.Wall.WallFacing.WEST:
                        wall.Facing = Map.Wall.WallFacing.NORTH; break;

                    case Map.Wall.WallFacing.NORTH_T:
                        wall.Facing = Map.Wall.WallFacing.EAST_T; break;
                    case Map.Wall.WallFacing.EAST_T:
                        wall.Facing = Map.Wall.WallFacing.NORTH_T; break;

                    case Map.Wall.WallFacing.WEST_T:
                        wall.Facing = Map.Wall.WallFacing.SOUTH_T; break;
                    case Map.Wall.WallFacing.SOUTH_T:
                        wall.Facing = Map.Wall.WallFacing.WEST_T; break;

                    case Map.Wall.WallFacing.NE_CORNER:
                        wall.Facing = Map.Wall.WallFacing.SW_CORNER; break;
                    case Map.Wall.WallFacing.SW_CORNER:
                        wall.Facing = Map.Wall.WallFacing.NE_CORNER; break;
                }
            }

            /*foreach (Map.Polygon poly in view.Map.Polygons)
            {
                foreach (PointF pt in poly.Points)
                {
                    float val = pt.X;
                    val = (float)((float)(mapsize) - (float)(val));
                    pt.X = (float)val;
                }
            }*/
           /* foreach (Map.Tile til in view.Map.Tiles.Values)
            {
                byte val = (byte)til.Location.Y;
                val = (byte)~val;
                til.Location.Y = (int)val - 1;
                // foreach (Map.Tile.EdgeTile edg in til.EdgeTiles)
                // {
                //     edg.Dir = Map.Tile.EdgeTile.Direction.
                // }
            }*/
            foreach (Map.Waypoint wpt in view.Map.Waypoints)
            {
                float val = wpt.Point.Y;
                val = (float)((float)(mapsize) - (float)(val));
                wpt.Point.Y = (float)val;
            }
            foreach (Map.Object tng in view.Map.Objects)
            {
                float val = tng.Location.Y;
                val = (float)((float)(mapsize) - (float)(val));
                tng.Location.Y = (float)val;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            button4.Enabled = false;
            /*
 * This function copies all of the items and objects and flips them. 
 * 
 * 
 */

            // Flip the walls
            if (chkFlipWall.Checked)
            {
                //int mapsize = 255 * 23;
                Map.WallMap temap = new Map.WallMap();
                foreach (Map.Wall wall in view.Map.Walls.Values)
                {
                    if (wall == null)
                        break;
                    Map.Wall wal = new Map.Wall(wall.Location, wall.Facing, wall.matId);

                    wal.Location.Y = wall.Location.X;
                    wal.Location.X = wall.Location.Y;

                    if (chkAdv.Checked)
                    {
                        wal.matId = (byte)comboBox1.SelectedIndex;
                        wal.Minimap = Convert.ToByte(minimapGroup.Text);
                    }
                    switch (wal.Facing)
                    {
                        case Map.Wall.WallFacing.EAST_T:
                            wal.Facing = Map.Wall.WallFacing.WEST_T; break;
                        case Map.Wall.WallFacing.WEST_T:
                            wal.Facing = Map.Wall.WallFacing.EAST_T; break;

                        case Map.Wall.WallFacing.NE_CORNER:
                            wal.Facing = Map.Wall.WallFacing.NW_CORNER; break;
                        case Map.Wall.WallFacing.SE_CORNER:
                            wal.Facing = Map.Wall.WallFacing.SW_CORNER; break;
                        case Map.Wall.WallFacing.SW_CORNER:
                            wal.Facing = Map.Wall.WallFacing.SE_CORNER; break;
                        case Map.Wall.WallFacing.NW_CORNER:
                            wal.Facing = Map.Wall.WallFacing.NE_CORNER; break;
                    }
                    temap.Add(wal.Location, wal);
                }
                foreach (Map.Wall wa in temap.Values)
                {
                    if (!view.Map.Walls.ContainsKey(wa.Location))
                    {
                        view.Map.Walls.Add(wa.Location, wa);
                    }
                }
            }
            // Flip the waypoints now
            /*Map.WaypointList wp = new Map.WaypointList();
            foreach (Map.Waypoint wpt in view.Map.Waypoints)
            {
                Map.Waypoint wt = new Map.Waypoint(wpt.Name,wpt.Point,wpt.num);
                wt = wpt;
                float val = wt.Point.Y;
                val = (float)((float)(mapsize) - (float)(val));
                
                wt.Point.Y = (float)val;
                wp.Add(wt);
            }
            foreach (Map.Waypoint wpt in wp)
            {
                view.Map.Waypoints.Add(wpt);
            }*/

            // Flip the objects now
            if (chkFlipOb.Checked)
            {
                Map.ObjectTable tng2 = new Map.ObjectTable();
                foreach (Map.Object tng in view.Map.Objects)
                {
                    Map.Object tn = new Map.Object(tng.Name,tng.Location);
                    tn.Properties = tng.Properties;
                    tn.modbuf = (byte[])tng.modbuf.Clone();
                    tn.Extent = 0;

                    tn.Location.Y = tng.Location.X;
                    tn.Location.X = tng.Location.Y;

                    tng2.Add(tn);
                }
                foreach (Map.Object tng in tng2)
                {
                    view.Map.Objects.Add(tng);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            button4.Enabled = false;
            /*
* This function copies all of the items and objects and flips them. 
* 
* 
*/

            // Flip the walls
            if (chkFlipWall.Checked)
            {
                //int mapsize = 255 * 23;
                Map.WallMap temap = new Map.WallMap();
                foreach (Map.Wall wall in view.Map.Walls.Values)
                {
                    Map.Wall wal = new Map.Wall(wall.Location, wall.Facing, wall.matId);
                    wal.Location.Y = (255 - wall.Location.X)+1;
                    wal.Location.X = (255 - wall.Location.Y)+1;
                    if (chkAdv.Checked)
                    {
                        wal.matId = (byte)comboBox1.SelectedIndex;
                        wal.Minimap = Convert.ToByte(minimapGroup.Text);
                    }
                    switch (wal.Facing)
                    {
                        case Map.Wall.WallFacing.NORTH_T:
                            wal.Facing = Map.Wall.WallFacing.SOUTH_T; break;
                        case Map.Wall.WallFacing.SOUTH_T:
                            wal.Facing = Map.Wall.WallFacing.NORTH_T; break;

                        case Map.Wall.WallFacing.NE_CORNER:
                            wal.Facing = Map.Wall.WallFacing.SE_CORNER; break;
                        case Map.Wall.WallFacing.SE_CORNER:
                            wal.Facing = Map.Wall.WallFacing.NE_CORNER; break;
                        case Map.Wall.WallFacing.SW_CORNER:
                            wal.Facing = Map.Wall.WallFacing.NW_CORNER; break;
                        case Map.Wall.WallFacing.NW_CORNER:
                            wal.Facing = Map.Wall.WallFacing.SW_CORNER; break;
                    }
                    temap.Add(wal.Location, wal);
                }
                foreach (Map.Wall wa in temap.Values)
                {
                    if (!view.Map.Walls.ContainsKey(wa.Location))
                    {
                        view.Map.Walls.Add(wa.Location, wa);
                    }
                }
            }
            // Flip the waypoints now
            /*Map.WaypointList wp = new Map.WaypointList();
            foreach (Map.Waypoint wpt in view.Map.Waypoints)
            {
                Map.Waypoint wt = new Map.Waypoint(wpt.Name,wpt.Point,wpt.num);
                wt = wpt;
                float val = wt.Point.Y;
                val = (float)((float)(mapsize) - (float)(val));
                
                wt.Point.Y = (float)val;
                wp.Add(wt);
            }
            foreach (Map.Waypoint wpt in wp)
            {
                view.Map.Waypoints.Add(wpt);
            }*/

            // Flip the objects now
            if (chkFlipOb.Checked)
            {
                Map.ObjectTable tng2 = new Map.ObjectTable();
                foreach (Map.Object tng in view.Map.Objects)
                {
                    Map.Object tn = new Map.Object();
                    tn.Properties = tng.Properties;
                    tn.modbuf = (byte[])tng.modbuf.Clone();
                    tn.Extent = 0;

                    tn.Location.Y = ((256*23) - tng.Location.X)+23;
                    tn.Location.X = ((256*23) - tng.Location.Y)+23;

                    tng2.Add(tn);
                }
                foreach (Map.Object tng in tng2)
                {
                    view.Map.Objects.Add(tng);
                }
            }
        }

        private void FlipMap_Load(object sender, EventArgs e)
        {
            button3.Enabled = true;
            button4.Enabled = true;
            comboBox1.Items.AddRange(ThingDb.Walls.ToArray());
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}