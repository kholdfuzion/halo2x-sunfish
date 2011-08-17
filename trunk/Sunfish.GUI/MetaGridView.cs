using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Sunfish.TagStructures;

namespace Sunfish.MetaEditor
{
    public partial class MetaGridView : UserControl
    {
        bool loaded = false;
        public Tag HaloTag;
        public TagBlock workingTagblock;
        DataGridViewCellStyle Disabled = new DataGridViewCellStyle()
        {
            BackColor = SystemColors.Control,
            ForeColor = SystemColors.GrayText,
            SelectionBackColor = SystemColors.Control,
            SelectionForeColor = SystemColors.GrayText,
        };

        public MetaGridView()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        public void LoadTag(Tag tag)
        {
            HaloTag = tag;
            workingTagblock = TagBlock.CreateInstance(tag.Type);
            workingTagblock.Deserialize(tag.TagStream, 0, 0);
            Enabled = false;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + string.Format(@"./MetaLayouts/{0}.xml", new Sunfish.ValueTypes.TagType(tag.Type).ToPathSafeString()));
            LoadTreeView(xDoc.LastChild.LastChild, workingTagblock);
            Enabled = true;
            loaded = true;
        }

        private void LoadTreeView(XmlNode parentnode, TagBlock workingTagblock)
        {
            treeView1.Nodes.Clear();
            TagHeaderTreeNode node = new TagHeaderTreeNode(workingTagblock.Name, parentnode, workingTagblock);
            treeView1.Nodes.Add(node);
            LoadTreeView(parentnode, workingTagblock, treeView1.Nodes[0]);
            treeView1.SelectedNode = treeView1.TopNode;
            treeView1.TopNode.Expand();
        }

        private void LoadTreeView(XmlNode parentnode, TagBlock workingTagblock, TreeNode node)
        {
            int offset = 0;
            foreach (XmlNode childnode in parentnode)
            {
                if (childnode.NodeType == XmlNodeType.Element)
                {
                    switch (childnode.Name.ToLower())
                    {
                        case "tagblock":
                            if (workingTagblock.Values == null) { return; }
                            foreach (Value value in workingTagblock.Values)
                            {
                                if (value.Offset == offset && value is TagBlockArray && (value as TagBlockArray).Length > 0)
                                {
                                    TagStructArrayTreeNode treenode = new TagStructArrayTreeNode(childnode.Attributes["name"].Value, childnode);
                                    for (int i = 0; i < (value as TagBlockArray).Length; i++)
                                    {
                                        TagStructTreeNode childtreenode = new TagStructTreeNode(string.Format("chunk:{0}", i), (value as TagBlockArray).TagBlocks[i]);
                                        LoadTreeView(childnode, (value as TagBlockArray).TagBlocks[i], childtreenode);
                                        treenode.Nodes.Add(childtreenode);
                                    }
                                    node.Nodes.Add(treenode);
                                    break;
                                }
                            }
                            offset += 8;
                            break;
                        case "taginstance":
                            offset += 8;
                            break;
                        case "tagclass":
                            offset += 4;
                            break;
                        case "tagindex":
                            offset += 4;
                            break;
                        case "stringid":
                            offset += 4;
                            break;
                        case "string":
                            offset += int.Parse(childnode.Attributes["byte-length"].Value);
                            break;
                        case "unused":
                            offset += int.Parse(childnode.Attributes["length"].Value);
                            break;
                        case "byte":
                            offset += sizeof(byte);
                            break;
                        case "short":
                            offset += sizeof(short);
                            break;
                        case "ushort":
                            offset += sizeof(ushort);
                            break;

                        case "int":
                            offset += sizeof(int);
                            break;
                        case "uint":
                            offset += sizeof(uint);
                            break;
                        case "float":
                        case "single":
                            offset += sizeof(float);
                            break;
                        case "enum":
                            switch (childnode.Attributes["base"].Value)
                            {
                                case "byte":
                                    offset += sizeof(byte);
                                    break;
                                case "short":
                                    offset += sizeof(short);
                                    break;
                                case "int":
                                    offset += sizeof(int);
                                    break;
                            }
                            break;
                        case "flags":
                            switch (childnode.Attributes["base"].Value)
                            {
                                case "byte":
                                    offset += sizeof(byte);
                                    break;
                                case "short":
                                    offset += sizeof(short);
                                    break;
                                case "int":
                                    offset += sizeof(int);
                                    break;
                            }
                            break;
                        default:
                            { }
                            break;
                    }
                }
            }
        }

        public void Save()
        {
            //SaveValues(workingTagblock, 0, dataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible));
        }

        void LoadValues(TagBlock tagBlock, int startIndex, int count)
        {
            int offset;
            for (int i = startIndex; i < startIndex + count; i++)
            {
                try
                {
                    if (dataGridView.Rows[i] is Row) { (dataGridView.Rows[i] as Row).Data = tagBlock.Data; }
                    offset = (dataGridView.Rows[i] as Row).Offset;
                    if (dataGridView.Rows[i] is StringRow)
                        dataGridView.Rows[i].Cells[2].Value = (dataGridView.Rows[i] as StringRow).Encoding.GetString(tagBlock.Data, offset, (dataGridView.Rows[i] as StringRow).Length).Trim(char.MinValue);
                    else if (dataGridView.Rows[i] is StringIdRow)
                    {
                        Sunfish.ValueTypes.StringId sid = BitConverter.ToInt32(tagBlock.Data, offset);
                        (dataGridView.Rows[i] as StringIdRow).Value = HaloTag.Strings[sid.Index];
                    }
                    else if (dataGridView.Rows[i] is TagDialogRow)
                    {
                        Sunfish.ValueTypes.TagType type = new Sunfish.ValueTypes.TagType(new byte[] { tagBlock.Data[offset], tagBlock.Data[offset + 1], tagBlock.Data[offset + 2], tagBlock.Data[offset + 3] });
                        Sunfish.ValueTypes.TagIndex index = BitConverter.ToInt32(tagBlock.Data, offset + 4);
                        (dataGridView.Rows[i] as TagDialogRow).Value = HaloTag.TagReferences[index.Index];
                    }
                    else if (dataGridView.Rows[i] is Row)
                    {
                        //Load byte
                        if (dataGridView.Rows[i].Cells[2].ValueType == typeof(byte))
                            dataGridView.Rows[i].Cells[2].Value = tagBlock.Data[offset];
                        //Load short
                        else if (dataGridView.Rows[i].Cells[2].ValueType == typeof(short))
                            dataGridView.Rows[i].Cells[2].Value = BitConverter.ToInt16(tagBlock.Data, offset);
                        //Load ushort
                        else if (dataGridView.Rows[i].Cells[2].ValueType == typeof(ushort))
                            dataGridView.Rows[i].Cells[2].Value = BitConverter.ToUInt16(tagBlock.Data, offset);
                        //Load int
                        else if (dataGridView.Rows[i].Cells[2].ValueType == typeof(int))
                            dataGridView.Rows[i].Cells[2].Value = BitConverter.ToInt32(tagBlock.Data, offset);
                        //Load uint
                        else if (dataGridView.Rows[i].Cells[2].ValueType == typeof(uint))
                            dataGridView.Rows[i].Cells[2].Value = BitConverter.ToUInt32(tagBlock.Data, offset);
                        //Load float
                        else if (dataGridView.Rows[i].Cells[2].ValueType == typeof(float))
                            dataGridView.Rows[i].Cells[2].Value = BitConverter.ToSingle(tagBlock.Data, offset);

                        else DisableRows(i, 1);

                    }
                    else DisableRows(i, 1);
                }
                catch { continue; }
            }
        }

        void SaveValues(int startIndex, int count)
        {
            for (int i = startIndex; i < startIndex + count; i++)
            {
                if (dataGridView.Rows[i] is StringRow)
                {
                    StringRow strrow = (dataGridView.Rows[i] as StringRow);
                    byte[] buffer = new byte[strrow.Length]; 
                    byte[] strbytes = strrow.Encoding.GetBytes(dataGridView.Rows[i].Cells[2].Value.ToString());
                    Array.Copy(strbytes, buffer, Clamp(strbytes.Length, strrow.Length));
                    Array.Copy(buffer, 0, strrow.Data, strrow.Offset, buffer.Length);
                }
                else if (dataGridView.Rows[i] is StringIdRow)
                {
                    string str = dataGridView.Rows[i].Cells[2].Value.ToString();
                    if (!HaloTag.Strings.Contains(str)) HaloTag.Strings.Add(str);
                }
                else if (dataGridView.Rows[i] is Row)
                    SaveValue(dataGridView.Rows[i] as Row);
            }
        }

        int Clamp(int length, int max)
        {
            if (length > max) return max;
            else return length;
        }

        void SaveValue(Row row)
        {
            if (row.Cells[2].ValueType == typeof(byte))
                row.Data[row.Offset] = (byte)row.Cells[2].Value;
            else if (row.Cells[2].ValueType == typeof(short))
                Array.Copy(BitConverter.GetBytes((short)row.Cells[2].Value), 0, row.Data, row.Offset, sizeof(short));
            else if (row.Cells[2].ValueType == typeof(ushort))
                Array.Copy(BitConverter.GetBytes((ushort)row.Cells[2].Value), 0, row.Data, row.Offset, sizeof(ushort));
            else if (row.Cells[2].ValueType == typeof(int))
                Array.Copy(BitConverter.GetBytes((int)row.Cells[2].Value), 0, row.Data, row.Offset, sizeof(int));
            else if (row.Cells[2].ValueType == typeof(uint))
                Array.Copy(BitConverter.GetBytes((uint)row.Cells[2].Value), 0, row.Data, row.Offset, sizeof(uint));
            else if (row.Cells[2].ValueType == typeof(float))
                Array.Copy(BitConverter.GetBytes((float)row.Cells[2].Value), 0, row.Data, row.Offset, sizeof(float));
        }

        DataGridViewRow[] LoadRows(XmlNode parentnode) { return LoadRows(parentnode, 0); }

        DataGridViewRow[] LoadRows(XmlNode parentnode, int depth)
        {
            List<DataGridViewRow> Rows = new List<DataGridViewRow>();
            int offset = 0;
            foreach(XmlNode childnode in parentnode.ChildNodes)
            {
                if (childnode.NodeType == XmlNodeType.Element)
                {
                    switch (childnode.Name.ToLower())
                    {
                        case "tagblock":
                            offset += 8;
                            break;
                        case "taginstance":
                            Rows.Add(new TagDialogRow(childnode.Attributes["name"].Value, offset, depth));
                            offset += 4;
                            break;
                        case "tagclass":
                            Rows.Add(new TextBoxRow(childnode.Attributes["name"].Value, typeof(Sunfish.ValueTypes.TagType), offset, depth));
                            offset += 4;
                            break;
                        case "tagindex":
                            Rows.Add(new TextBoxRow(childnode.Attributes["name"].Value, typeof(Sunfish.ValueTypes.TagIndex), offset, depth));
                            offset += 4;
                            break;
                        case "stringid":
                            Rows.Add(new StringIdRow(childnode.Attributes["name"].Value, offset, depth));
                            offset += 4;
                            break;
                        case "string":
                            Encoding e =Encoding.UTF8;
                            switch (childnode.Attributes["encoding"].Value)
                            {
                                case "utf-16":
                                    e = Encoding.Unicode;
                                    break;
                            }
                            StringRow newrow = new StringRow(childnode.Attributes["name"].Value, offset, depth, int.Parse(childnode.Attributes["byte-length"].Value), e);
                            (newrow.Cells[2] as DataGridViewTextBoxCell).MaxInputLength = int.Parse(childnode.Attributes["byte-length"].Value);
                            Rows.Add(newrow);
                            offset += int.Parse(childnode.Attributes["byte-length"].Value);
                            break;
                        case "unused":
                            offset += int.Parse(childnode.Attributes["length"].Value);
                            break;
                        case "byte":
                            Rows.Add(new TextBoxRow(childnode.Attributes["name"].Value, typeof(byte), offset, depth));
                            offset += sizeof(byte);
                            break;
                        case "short":
                            Rows.Add(new TextBoxRow(childnode.Attributes["name"].Value, typeof(short), offset, depth));
                            offset += sizeof(short);
                            break;
                        case "ushort":
                            Rows.Add(new TextBoxRow(childnode.Attributes["name"].Value, typeof(ushort), offset, depth));
                            offset += sizeof(ushort);
                            break;

                        case "int":
                            Rows.Add(new TextBoxRow(childnode.Attributes["name"].Value, typeof(int), offset, depth));
                            offset += sizeof(int);
                            break;
                        case "uint":
                            Rows.Add(new TextBoxRow(childnode.Attributes["name"].Value, typeof(uint), offset, depth));
                            offset += sizeof(uint);
                            break;
                        case "float":
                        case "single":
                            Rows.Add(new TextBoxRow(childnode.Attributes["name"].Value, typeof(float), offset, depth));
                            offset += sizeof(float);
                            break;
                        case "enum":
                            switch (childnode.Attributes["base"].Value)
                            {
                                case "byte":
                                    Rows.Add(new TextBoxRow(childnode.Attributes["name"].Value, typeof(byte), offset, depth));
                                    offset += sizeof(byte);
                                    break;
                                case "short":
                                    Rows.Add(new TextBoxRow(childnode.Attributes["name"].Value, typeof(short), offset, depth));
                                    offset += sizeof(short);
                                    break;
                                case "int":
                                    Rows.Add(new TextBoxRow(childnode.Attributes["name"].Value, typeof(int), offset, depth));
                                    offset += sizeof(int);
                                    break;
                            }
                            break;
                        case "flags":
                            switch (childnode.Attributes["base"].Value)
                            {
                                case "byte":
                                    Rows.Add(new TextBoxRow(childnode.Attributes["name"].Value, typeof(byte), offset, depth));
                                    offset += sizeof(byte);
                                    break;
                                case "short": Rows.Add(new TextBoxRow(childnode.Attributes["name"].Value, typeof(short), offset, depth));
                                    offset += sizeof(short);
                                    break;
                                case "int": Rows.Add(new TextBoxRow(childnode.Attributes["name"].Value, typeof(int), offset, depth));
                                    offset += sizeof(int);
                                    break;
                            }
                            break;
                        default:
                            { }
                            break;
                    }
                }
            }
            return Rows.ToArray();
        }

        void DisableRows(int startIndex, int count)
        {
            for (int i = startIndex; i < startIndex + count; i++)
            {
                dataGridView.Rows[i].ReadOnly = true;
                (dataGridView.Rows[i] as Row).Depth = -1;
            }
        }

        void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!loaded) return;
            else if (dataGridView.Rows[e.RowIndex] is Row)
            {
                SaveValues(e.RowIndex, 1);
            }

        }

        void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        void ApplyStyle()
        {
            uint color = 0xFFFFFFFF;
            int Dec = 0x00101200;
            for (int i = 0; i < dataGridView.RowCount; i++)
            {
                if ((dataGridView.Rows[i] as Row).Depth == -1)
                    dataGridView.Rows[i].DefaultCellStyle = Disabled;
                else dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb((int)(color - (Dec * (dataGridView.Rows[i] as Row).Depth)));
            }
        }

        void dataGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode is TagHeaderTreeNode)
            {
                dataGridView.Rows.Clear();
                dataGridView.Rows.AddRange(LoadRows((treeView1.SelectedNode as TagHeaderTreeNode).Values));
                LoadValues((treeView1.SelectedNode as TagHeaderTreeNode).TagBlock, 0, dataGridView.Rows.Count);
            }
            else if (treeView1.SelectedNode is TagStructArrayTreeNode)
            {
                dataGridView.Rows.Clear();
                dataGridView.Rows.AddRange(LoadRows((treeView1.SelectedNode as TagStructArrayTreeNode).Values));
                LoadValues((treeView1.SelectedNode.FirstNode as TagStructTreeNode).TagBlock, 0, dataGridView.Rows.Count);
            }
            else if (treeView1.SelectedNode is TagStructTreeNode)
            {
                dataGridView.Rows.Clear();
                dataGridView.Rows.AddRange(LoadRows((treeView1.SelectedNode.Parent as TagStructArrayTreeNode).Values));
                LoadValues((treeView1.SelectedNode as TagStructTreeNode).TagBlock, 0, dataGridView.Rows.Count);
            }
        }

        private void dataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                dataGridView.BeginEdit(true);
            }
            if (dataGridView.Rows[e.RowIndex] is TagDialogRow) { MessageBox.Show("Choose"); }
        }
    }

    class TagHeaderTreeNode : TreeNode
    {
        public TagBlock TagBlock;
        public XmlNode Values;
        public TagHeaderTreeNode(string name, XmlNode node, TagBlock tagblock)
            : base(name)
        {
            Values = node;
            TagBlock = tagblock;
        }
    }

    class TagStructArrayTreeNode : TreeNode
    {
        public XmlNode Values;
        public TagStructArrayTreeNode(string name, XmlNode node) : base(name) { Values = node; }
    }

    class TagStructTreeNode : TreeNode
    {
        public TagBlock TagBlock;
        public TagStructTreeNode(string name, TagBlock tagblock) : base(name) { TagBlock = tagblock; }
    }

    class EnumRow : Row
    {
        public EnumRow()
            : base()
        {
            Cells[2] = new ComboBoxCell();
        }
    }

    class ComboBoxCell : DataGridViewComboBoxCell
    {
        List<int> Values;
        List<string> Names;

        public ComboBoxCell()
            : base()
        {
            Values = new List<int>();
            Names = new List<string>();
        }

        public void SetCellValueFromEnumValue(int value)
        {
            int index = Values.IndexOf(value);
            if (index == -1)
            {
                AddEnumOption(value.ToString(), value);
                index = Values.Count - 1;
            }
            this.Value = Names[index];
        }

        public void AddEnumOption(string name, int value)
        {
            Values.Add(value);
            Names.Add(name);
            Items.Add(name);
        }
    }

    class TagDialogRow : TextBoxRow
    {
        public string Value { get { return this.Cells[2].Value.ToString(); } set { this.Cells[2].Value = value; } }

        public TagDialogRow(string name, int offset, int depth)
            : base(name, typeof(object), offset, depth) { }
    }

    class StringIdRow : TextBoxRow
    {
        public string Value { get { return this.Cells[2].Value.ToString(); } set { this.Cells[2].Value = value; } }

        public StringIdRow(string name, int offset, int depth)
            : base(name, typeof(Sunfish.ValueTypes.StringId), offset, depth) { }
    }

    class StringRow : TextBoxRow
    {
        public int Length;
        public Encoding Encoding;

        public StringRow(string name, int offset, int depth, int length, Encoding encoding)
            : base(name, typeof(string), offset, depth)
        {
            Length = length;
            Encoding = encoding;
        }
    }

    class TextBoxRow : Row
    {
        public TextBoxRow(string name, Type valuetype, int offset, int depth)
            : base(offset, depth)
        {
            Cells[0].Value = name;
            Cells[0].ReadOnly = true;
            Cells[1].Value = valuetype;
            Cells[1].ReadOnly = true;
            Cells[2].ValueType = valuetype;
            Cells[2].ReadOnly = false;
        }
    }

    class Row : DataGridViewRow
    {
        public byte[] Data;
        public int Offset;
        public int Depth;
        public Row()
        {
            Cells.AddRange(new DataGridViewCell[]{
                    new DataGridViewTextBoxCell(),
                    new DataGridViewTextBoxCell(),
                    new DataGridViewTextBoxCell(),
                });
        }

        public Row(int offset, int depth)
            : this()
        {
            Offset = offset;
            Depth = depth;
        }
    }
}