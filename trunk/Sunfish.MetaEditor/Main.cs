using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Sunfish.TagStructures;

namespace Sunfish.MetaEditor
{
    public partial class Main : Form
    {
        string filename = @"E:\Users\root\Documents\Halo 2 Modding\Working Maps\headlong.map";
        public Main()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
        }

        private void openTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filename = ofd.FileName;
                Text = filename;
                Tag t = new Tag(filename);
                metaGridView1.LoadTag(t);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            metaGridView1.Save();
        }
    }
}
