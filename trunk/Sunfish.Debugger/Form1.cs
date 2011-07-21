using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Sunfish.Developmental;
using Sunfish.TagStructures;
using System.Diagnostics;
using System.Threading;
using System.Runtime.Remoting.Messaging;

namespace Sunfish.Debugger
{
    public partial class Form1 : Form
    {
        Map map;
        int tagIndex;
        List<string> cell0;
        List<string> cell1;
        List<int> cell2;
        List<Sector> Sectors;

        public Form1()
        {
            InitializeComponent();

        }

        //        private void Form1_Load(object sender, EventArgs e)
        //        {

        //        }

        Report DoShit(int index, Debugger deb, Map map)
        {
            return deb.DebugTag(new Sunfish.ValueTypes.TagIndex(index), map);
        }

        public delegate Report Debug(int index, Debugger deb, Map map);

        //        public delegate void AddTagInformation(string tagname);

        //        public void AddTagInfo(string tagname)
        //        {
        //            dataGridView1.Rows.Add();
        //            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = tagname;
        //            dataGridView1.Rows[dataGridView1.RowCount - 1].DefaultCellStyle.BackColor = Color.Green;
        //            Sectors.Clear();
        //        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripComboBox1.Items.Clear();
            dataGridView1.Rows.Clear();
            Debugger deb = new Debugger();
            Blocks.ReloadBlocks();
            string dir = @"E:\Users\root\Documents\Sunfish 2011\Projects\headlong2\bin";
            string[] paths = Directory.GetFiles(dir, "*.map");
            List<Header> Headers = new List<Header>();
            Debug debug = new Debug(DoShit);
            foreach (string path in paths)
            {
                FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.ReadWrite, 512, FileOptions.SequentialScan);
                map = new Map(file);
                if (map.Header.Type == Sunfish.Header.MapType.Multiplayer)
                {
                    for (tagIndex = 0; tagIndex < map.Index.TagInfoCount; tagIndex++)
                    {
                        deb.DebugTag(tagIndex, map);
                    }
                }
                List<string> shit = new List<string>();
                List<Sunfish.ValueTypes.StringId> nigg = new List<Sunfish.ValueTypes.StringId>();
                List<int> fuck = new List<int>();
                foreach (string s in map.StringIdNames)
                {
                    if (!deb.Strings.Contains(s))
                    {
                        shit.Add(s);
                        Sunfish.ValueTypes.StringId sf = new Sunfish.ValueTypes.StringId() { Index = (short)map.StringIdNames.IndexOf(s), Length = (sbyte)Encoding.UTF8.GetByteCount(s) };

                        nigg.Add(sf);
                        fuck.Add((int)sf);
                    }
                }
                int findAddress = 29814640;
                for (tagIndex = 0; tagIndex < map.Index.TagInfoCount; tagIndex++)
                {
                    int offset, length;
                    if (map.Index.TagEntries[tagIndex].Type == "sbsp" || map.Index.TagEntries[tagIndex].Type == "ltmp")
                    {
                        offset = map.Index.TagEntries[tagIndex].VirtualAddress - map.PrimaryMagic; length = map.Index.TagEntries[tagIndex].Length;
                    }
                    else
                    {
                        offset = map.Index.TagEntries[tagIndex].VirtualAddress - map.SecondaryMagic; length = map.Index.TagEntries[tagIndex].Length;
                    }
                    if (findAddress >= offset && findAddress < offset + length)
                    { }
                }
            }
        }

        //        void deb_OnStartDebuggingTag(object obj, Debugger.ReadTagEventArgs e)
        //        {
        //            BeginInvoke(new AddTagInformation(AddTagInfo), e.tagname);
        //        }

        void DoneShit(IAsyncResult result)
        {
            AsyncResult ar = (AsyncResult)result;
            Debug caller = (Debug)ar.AsyncDelegate;
            Report r = caller.EndInvoke(result);
            BeginInvoke(new SectionHandler(HandleSector), r.Sectors.ToArray());
        }

        delegate void SectionHandler(Sector[] sectors);

        void HandleSector(Sector[] sectors)
        {
            int nextAddress = sectors[0].StartAddress;
            for (int i = 0; i < sectors.Length; i++)
            {
                bool b = false;
                if (!sectors[i].IsInternal)
                    b = true;
                for (int j = 0; j < i; j++)
                    if (sectors[j].StartAddress == sectors[i].StartAddress)
                        b = true;

                List<int> indices = new List<int>();
                Block cur = sectors[i].Block;
                StringBuilder s = new StringBuilder();
                if (cur.Parent == null) { s = new StringBuilder("root"); }
                else
                {
                    while (cur.Parent != null)
                    {
                        indices.Add(cur.Index);
                        cur = cur.Parent;
                    }
                    indices.Reverse();
                    for (int j = 0; j < indices.Count - 1; j++)
                        s.Append(indices[j].ToString() + " - ");
                    s.Append(indices[indices.Count - 1]);
                }
                dataGridView1.Rows.Add("Sector", sectors[i].StartAddress, sectors[i].Length, sectors[i].Alignment, sectors[i].NextOffset, sectors[i].StartAddress - nextAddress, s);
                if (Sunfish.Padding.Pad(nextAddress, sectors[i].Alignment) != sectors[i].StartAddress && !b)
                {
                    dataGridView1.Rows[dataGridView1.RowCount - 1].DefaultCellStyle.BackColor = Color.Red;
                    toolStripComboBox1.Items.Add(dataGridView1.RowCount - 1);
                }
                if (!b)
                    nextAddress = sectors[i].NextOffset;
            }
        }

        //        int nextAddress;
        //        void deb_OnSectorRead(object obj, Debugger.ReadSectorArgs e)
        //        {
        //            BeginInvoke(new SectionHandler(HandleSector), e.Sector);
        //        }

        //        void deb_OnAddressError(object obj, Debugger.AddressingErrorArgs e)
        //        {
        //            //if (e.Type == Debugger.AddressingErrorArgs.ErrorType.InvalidAlignment)
        //            //{
        //            //    List<int> indices = new List<int>();
        //            //    Block cur = e.Block;
        //            //    StringBuilder s = new StringBuilder();
        //            //    if (cur.Parent == null) { s = new StringBuilder("root"); }
        //            //    else
        //            //    {
        //            //        while (cur.Parent != null)
        //            //        {
        //            //            indices.Add(cur.Index);
        //            //            cur = cur.Parent;
        //            //        }
        //            //        indices.Reverse();
        //            //        for (int i = 0; i < indices.Count - 1; i++)
        //            //            s.Append(indices[i].ToString() + " - ");
        //            //        s.Append(indices[indices.Count - 1]);
        //            //    }
        //            //    cell0.Add(String.Format("Unreferenced bytes in {0} in tag {1}, Offset {2}, Length {3}", map.Header.Name.Trim('\0'), Path.ChangeExtension(map.Tagnames[tagIndex], map.Index.TagEntries[tagIndex].Type), e.Offset, e.Length));
        //            //    cell1.Add(s.ToString());
        //            //    cell2.Add(e.Offset);
        //            //}
        //        }

        //        void deb_OnError(object obj, Debugger.ValueErrorArgs e)
        //        {
        //            //switch (e.Type)
        //            //{
        //            //    case Debugger.ValueErrorArgs.ErrorType.InvalidStringReference:
        //            //        toolStripComboBox1.Items.Add(dataGridView1.RowCount);
        //            //        List<int> indices = new List<int>();
        //            //        Block cur = e.Block;
        //            //        StringBuilder s = new StringBuilder();
        //            //        if (cur.Parent == null) { s = new StringBuilder("root"); }
        //            //        else
        //            //        {
        //            //            while (cur.Parent != null)
        //            //            {
        //            //                indices.Add(cur.Index);
        //            //                cur = cur.Parent;
        //            //            }
        //            //            indices.Reverse();
        //            //            for (int i = 0; i < indices.Count - 1; i++)
        //            //                s.Append(indices[i].ToString() + " - ");
        //            //            s.Append(indices[indices.Count - 1]);
        //            //        }
        //            //        dataGridView1.Rows.Add();
        //            //        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = String.Format("Invalid StringReference value in {0} in tag {1}, reflexive {2}", map.Header.Name.Trim('\0'), Path.ChangeExtension(map.Tagnames[tagIndex], map.Index.TagEntries[tagIndex].Type), s);
        //            //        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = e.Offset;
        //            //        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = string.Format("Data:[{0}]", BitConverter.ToString(BitConverter.GetBytes((int)e.Value)));
        //            //        dataGridView1.Rows[dataGridView1.RowCount - 1].DefaultCellStyle.BackColor = Color.Purple;
        //            //        break;
        //            //    case Debugger.ValueErrorArgs.ErrorType.InvalidTagType:
        //            //        toolStripComboBox1.Items.Add(dataGridView1.RowCount);
        //            //        indices = new List<int>();
        //            //        cur = e.Block;
        //            //        s = new StringBuilder();
        //            //        if (cur.Parent == null) { s = new StringBuilder("root"); }
        //            //        else
        //            //        {
        //            //            while (cur.Parent != null)
        //            //            {
        //            //                indices.Add(cur.Index);
        //            //                cur = cur.Parent;
        //            //            }
        //            //            indices.Reverse();
        //            //            for (int i = 0; i < indices.Count - 1; i++)
        //            //                s.Append(indices[i].ToString() + " - ");
        //            //            s.Append(indices[indices.Count - 1]);
        //            //        }
        //            //        dataGridView1.Rows.Add();
        //            //        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = String.Format("Invalid TagType value in {0} in tag {1}, reflexive {2}", map.Header.Name.Trim('\0'), Path.ChangeExtension(map.Tagnames[tagIndex], map.Index.TagEntries[tagIndex].Type), s);
        //            //        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = e.Offset;
        //            //        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = string.Format("Data:[{0}]", e.Value.ToString());
        //            //        dataGridView1.Rows[dataGridView1.RowCount - 1].DefaultCellStyle.BackColor = Color.Yellow;
        //            //        break;
        //            //    case Debugger.ValueErrorArgs.ErrorType.InvalidTagIndex:
        //            //        toolStripComboBox1.Items.Add(dataGridView1.RowCount);
        //            //        indices = new List<int>();
        //            //        cur = e.Block;
        //            //        s = new StringBuilder();
        //            //        if (cur.Parent == null) { s = new StringBuilder("root"); }
        //            //        else
        //            //        {
        //            //            while (cur.Parent != null)
        //            //            {
        //            //                indices.Add(cur.Index);
        //            //                cur = cur.Parent;
        //            //            }
        //            //            indices.Reverse();
        //            //            for (int i = 0; i < indices.Count - 1; i++)
        //            //                s.Append(indices[i].ToString() + " - ");
        //            //            s.Append(indices[indices.Count - 1]);
        //            //        }
        //            //        dataGridView1.Rows.Add();
        //            //        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = String.Format("Invalid TagIndex value in {0} in tag {1}, reflexive {2}", map.Header.Name.Trim('\0'), Path.ChangeExtension(map.Tagnames[tagIndex], map.Index.TagEntries[tagIndex].Type), s);
        //            //        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = e.Offset;
        //            //        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = string.Format("Data:[{0}]", BitConverter.ToString(BitConverter.GetBytes((int)e.Value)));
        //            //        dataGridView1.Rows[dataGridView1.RowCount - 1].DefaultCellStyle.BackColor = Color.Orange;
        //            //        break;
        //            //}
        //        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(dataGridView1.CurrentCell.Value.ToString(), TextDataFormat.Text);
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo i = (sender as DataGridView).HitTest(e.X, e.Y);
            if (i.Type == DataGridViewHitTestType.Cell)
            {
                (sender as DataGridView).SelectionMode = DataGridViewSelectionMode.CellSelect;
                (sender as DataGridView).MultiSelect = false;
                (sender as DataGridView).Rows[i.RowIndex].Cells[i.ColumnIndex].Selected = true;
            }
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripComboBox1.SelectedIndex > -1) dataGridView1.FirstDisplayedScrollingRowIndex = (int)toolStripComboBox1.Items[toolStripComboBox1.SelectedIndex];
        }

        private void compareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = @"E:\Users\root\Documents\Sunfish 2011\Projects\headlong\bin";
           Map rebuild = new Map(new FileStream(Path.Combine(path, "headlong.map"), FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.ReadWrite));
            Map original = new Map(new FileStream(Path.Combine(path, "headlong.map"), FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.ReadWrite));
           // List<string> missingStrings = new List<string>();
           // List<int> StringReferences = new List<int>();
           // foreach (string str in original.StringIdNames)
           // {
           //     if (!rebuild.StringIdNames.Contains(str))
           //     {
           //         missingStrings.Add(str);
           //         StringReferences.Add(new Sunfish.ValueTypes.StringReference((short)original.StringIdNames.IndexOf(str), (sbyte)Encoding.UTF8.GetByteCount(str)));
           //     }
           // }
           // int findAddress = 16543774;
           // for (tagIndex = 0; tagIndex < original.Index.TagInfoCount; tagIndex++)
           // {
           //     int offset, length;
           //     if (original.Index.TagEntries[tagIndex].Type == "sbsp" || original.Index.TagEntries[tagIndex].Type == "ltmp")
           //     {
           //         offset = original.Index.TagEntries[tagIndex].VirtualAddress - original.PrimaryMagic; length = original.Index.TagEntries[tagIndex].Length;
           //     }
           //     else
           //     {
           //         offset = original.Index.TagEntries[tagIndex].VirtualAddress - original.SecondaryMagic; length = original.Index.TagEntries[tagIndex].Length;
           //     }
           //     if (findAddress >= offset && findAddress < offset + length)
           //     { }
           // } 
           // rebuild.BaseStream.Close();
           // original.BaseStream.Close();
           }
    }
}