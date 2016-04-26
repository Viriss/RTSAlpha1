using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTS_alpha1
{
    public partial class Form1 : Form
    {
        public int GridSize = 32;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Engine.Init(8, 8);
            DrawGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Engine.Update();
            DrawGrid();
        }

        private void DrawGrid()
        {
            Image img = new Bitmap(Engine.Width * GridSize + 1, Engine.Height * GridSize + 1);
            Graphics g = Graphics.FromImage(img);
            StringFormat drawFormat = new StringFormat();
            Rectangle rect;
            Font fnt = new Font("Calibri", 16, FontStyle.Bold);

            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;
            
            foreach (oNode node in Engine.Nodes)
            {
                rect = new Rectangle(node.X * GridSize, node.Y * GridSize, GridSize, GridSize);

                g.DrawRectangle(Pens.Black, rect);
                if (node.LocationGuid != Guid.Empty)
                {
                    oLocation loc = Engine.FindLocationByGuid(node.LocationGuid);
                    switch (loc.LocationType)
                    {
                        case LocationType.IronForge:
                            g.DrawString("F", fnt, Brushes.Firebrick, rect, drawFormat);
                            break;
                        case LocationType.IronMine:
                            g.DrawString("M", fnt, Brushes.SkyBlue, rect, drawFormat);
                            break;
                    }
                }
            }

            foreach (oUnit u in Engine.Units)
            {
                drawFormat.Alignment = StringAlignment.Near;
                drawFormat.LineAlignment = StringAlignment.Near;
                fnt = new Font("Calibri", 12, FontStyle.Regular);

                rect = new Rectangle(u.X * GridSize, u.Y * GridSize, GridSize, GridSize);
                g.DrawString(u.Name, fnt, Brushes.Black, rect, drawFormat);

                if (u._destination != null)
                {
                    rect = new Rectangle(u._destination.X * GridSize, u._destination.Y * GridSize, GridSize, GridSize);
                    g.DrawEllipse(Pens.Blue, rect);
                }


                drawFormat.Alignment = StringAlignment.Far;
                drawFormat.LineAlignment = StringAlignment.Near;
                fnt = new Font("Calibri", 8, FontStyle.Regular);
                foreach (oPathStep p in u._path)
                {
                    rect = new Rectangle(p.X * GridSize, p.Y * GridSize, GridSize, GridSize);
                    g.DrawString(p.Distance.ToString(), fnt, Brushes.Red, rect, drawFormat);
                }
            }


            pictureBox1.BackgroundImage = img;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DrawGrid();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Engine.Init(8, 8);
            DrawGrid();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = checkBox1.Checked;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Engine.Update();
            DrawGrid();
        }
    }
}
