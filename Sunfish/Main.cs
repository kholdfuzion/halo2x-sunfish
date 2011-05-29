using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using Sunfish.TagStructures;
using System.Runtime.InteropServices;
using Sunfish.Developmental;

namespace Sunfish
{
    public partial class Main : Form
    {
        string filename2 = @"E:\Users\root\Documents\Halo 2 Modding\Working Maps\headlong.map";
        string filename1 = @"E:\Users\root\Documents\Halo 2 Modding\Working Maps\headlong_rebuild.map";

        public Main()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Benchmark mark = new Benchmark();
            mark.Begin();
            FileStream mapFile = new FileStream(filename1, FileMode.Create);
            using (mapFile)
            {
                Compiler compiler = new Compiler(mapFile);
                compiler.CompileFromScenario(@"scenarios\multi\headlong\headlong.scnr");
            }
            mark.End();
            MessageBox.Show(string.Format("Done: {0}", mark.Result));
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            StaticBenchmark.Begin();
            FileStream file = new FileStream(filename2, FileMode.Open);
            MemoryStream ms;
            using (file)
            {
                ms = new MemoryStream((int)file.Length);
                Buffering.Copy(file, ms);
                file.Close();
            }
            Map m = new Map(ms);
            Decompiler decompiler = new Decompiler(m);
            foreach (Map.TagIndex.TagInfo Entry in m.Index.TagEntries)
            {
                if (Entry.Type == "sbsp" || Entry.Type == "ltmp")
                    decompiler.Decompile(Entry, m.PrimaryMagic);
                else
                    decompiler.Decompile(Entry, m.SecondaryMagic);
            }
            StaticBenchmark.End();
            MessageBox.Show(string.Format("Finished in: {0}", StaticBenchmark.Result), "Decompile Successful");
        }
    }
}
