using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Sunfish.GUI
{
    public partial class MetaTool : DockContent
    {
        public MetaTool()
        {
            InitializeComponent();
        }

        internal void LoadTag(Tag tag)
        {
            this.Tag = tag;
            metaGridView1.LoadTag(tag);
        }
    }
}
