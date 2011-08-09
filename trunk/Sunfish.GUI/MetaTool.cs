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
    public partial class MetaTool : SunfishEditor
    {
        public MetaTool()
        {
            InitializeComponent();
        }

        internal void LoadTag(Tag tag)
        {
            this.Tag = tag.Filename;
            this.HaloTag = tag;
            this.metaGridView1.LoadTag(tag);
        }

        public override void Save()
        {
            this.metaGridView1.workingTagblock.Serialize(HaloTag.TagStream, 0);
            base.Save();
        }
    }
}
