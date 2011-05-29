using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Sunfish.Mode;
using System.IO;
using Sunfish.Canvas;

namespace Sunfish.GUI
{
    public partial class ModelEditor : DockContent
    {
        public ModelEditor()
        {
            InitializeComponent();
        }

        internal void LoadTag(Tag tag)
        {
            Model m = new Model(tag);
            //m.Sections[0].Mesh.ExportWavefrontObject(m.Shaders);
            //WavefrontObject wfo = Wavefront.ParseWavefrontOBJFile(@"O:\import_test.obj");
            //m.Sections[0].Mesh.ImportWavefrontObject(wfo, m.BoundingBoxes[0]);
            //byte[] buffer = m.Sections[0].Mesh.Serialize(m.Sections[0], m.BoundingBoxes[0]);
            //BinaryWriter bw = new BinaryWriter(File.Create(@"O:\test.mode.raw.bin"));
            //bw.Write(buffer);
            //bw.Close();
            Tag newTag = m.CreateTag();
            Model m2 = new Model(newTag);
        }
    }
}
