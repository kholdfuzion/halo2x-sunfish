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
    public partial class MetaGridView : DataGridView
    {
        bool loaded = false;
        TagBlock workingTagblock;
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
            //System.Windows.Forms.DataGridViewTextBoxColumn Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //System.Windows.Forms.DataGridViewTextBoxColumn Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //System.Windows.Forms.DataGridViewTextBoxColumn Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //System.Windows.Forms.DataGridViewTextBoxColumn Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //System.Windows.Forms.DataGridViewTextBoxColumn Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            //this.SuspendLayout();
            //// 
            //// Column1
            //// 
            //Column1.HeaderText = "Column1";
            //Column1.Name = "Column1";
            //// 
            //// Column5
            //// 
            //Column5.HeaderText = "Column5";
            //Column5.Name = "Column5";
            //// 
            //// Column2
            //// 
            //Column2.HeaderText = "Column2";
            //Column2.Name = "Column2";
            //// 
            //// Column3
            //// 
            //Column3.HeaderText = "Column3";
            //Column3.Name = "Column3";
            //// 
            //// Column4
            //// 
            //Column4.HeaderText = "Column4";
            //Column4.Name = "Column4";
            //// 
            //// MetaGridView
            //// 
            //AllowUserToAddRows = false;
            //AllowUserToDeleteRows = false;
            //Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            //Column1,
            //Column5,
            //Column2,
            //Column3,
            //Column4});
        }

        public void LoadTag(Tag tag)
        {
            workingTagblock = TagBlock.CreateInstance(tag.Type);
            workingTagblock.Deserialize(tag.TagStream, 0, 0);
            XmlReader xmlReader = XmlReader.Create(@"./MetaLayouts/scnr.xml");
            Enabled = false;
            Rows.AddRange(LoadRows(xmlReader));
            LoadValues(workingTagblock, 0, Rows.GetRowCount(DataGridViewElementStates.Visible));
            Enabled = true;
            loaded = true;
            ApplyStyle();
        }

        public void Save()
        {
            SaveValues(workingTagblock, 0, Rows.GetRowCount(DataGridViewElementStates.Visible));
        }

        private void LoadValues(TagBlock tagBlock, int startIndex, int count)
        {
            int offset;
            for (int i = startIndex; i < startIndex + count; i++)
            {
                offset = (int)Rows[i].Cells[3].Value;
                if (Rows[i] is MetaGridViewTagBlockRow)
                {
                    int valueOffset = 0;
                    if (tagBlock.Values == null) continue;
                    foreach (Value value in tagBlock.Values)
                    {
                        if (value.Offset == offset)
                        {
                            if ((value as TagBlockArray).Length > 0)
                            {
                                int index = 0;
                                (Rows[i].Cells[2] as DataGridViewComboBoxCell).ValueType = typeof(int);
                                (Rows[i].Cells[2] as DataGridViewComboBoxCell).Items.Clear();
                                while (index < (value as TagBlockArray).Length)
                                {
                                    (Rows[i].Cells[2] as DataGridViewComboBoxCell).Items.Add(index);
                                    index++;
                                }
                                (Rows[i].Cells[2] as DataGridViewComboBoxCell).Value = 0;
                                (Rows[i] as MetaGridViewTagBlockRow).tagBlockArray = (value as TagBlockArray);
                                LoadValues((Rows[i] as MetaGridViewTagBlockRow).tagBlockArray.TagBlocks[0], i + 1, (Rows[i] as MetaGridViewTagBlockRow).Length);
                            }
                            else
                            {
                                DisableRows(i, (Rows[i] as MetaGridViewTagBlockRow).Length + 1);
                            }
                            break;
                        }
                        valueOffset += value.Size;
                    }
                    i += (Rows[i] as MetaGridViewTagBlockRow).Length;
                }
                else if (Rows[i] is MetaGridViewRow)
                {
                    //Load byte
                    if (Rows[i].Cells[2].ValueType == typeof(byte))
                        Rows[i].Cells[2].Value = tagBlock.Data[offset];
                    //Load short
                    else if (Rows[i].Cells[2].ValueType == typeof(short))
                        Rows[i].Cells[2].Value = BitConverter.ToInt16(tagBlock.Data, offset);
                    //Load ushort
                    else if (Rows[i].Cells[2].ValueType == typeof(ushort))
                        Rows[i].Cells[2].Value = BitConverter.ToUInt16(tagBlock.Data, offset);
                    //Load int
                    else if (Rows[i].Cells[2].ValueType == typeof(int))
                        Rows[i].Cells[2].Value = BitConverter.ToInt32(tagBlock.Data, offset);
                    //Load uint
                    else if (Rows[i].Cells[2].ValueType == typeof(uint))
                        Rows[i].Cells[2].Value = BitConverter.ToUInt32(tagBlock.Data, offset);
                    //Load float
                    else if (Rows[i].Cells[2].ValueType == typeof(float))
                        Rows[i].Cells[2].Value = BitConverter.ToSingle(tagBlock.Data, offset);

                }
            }
        }

        private void SaveValues(TagBlock tagBlock, int startIndex, int count)
        {
            for (int i = startIndex; i < startIndex + count; i++)
            {
                if (Rows[i] is MetaGridViewTagBlockRow)
                {
                    int valueOffset = 0;
                    foreach (Value value in tagBlock.Values)
                    {
                        if (value.Offset == (int)Rows[i].Cells[3].Value)
                        {
                            if ((value as TagBlockArray).Length > 0)
                            {
                                int index = (int)Rows[i].Cells[2].Value;
                                SaveValues((Rows[i] as MetaGridViewTagBlockRow).tagBlockArray.TagBlocks[index], i + 1, (Rows[i] as MetaGridViewTagBlockRow).Length);
                            }
                            break;
                        }
                        valueOffset += value.Size;
                    }
                    i += (Rows[i] as MetaGridViewTagBlockRow).Length;
                }
                else if (Rows[i] is MetaGridViewRow)
                {
                    if (Rows[i].Cells[2].ValueType == typeof(byte))
                    {
                        tagBlock.Data[(int)Rows[i].Cells[3].Value] = (byte)Rows[i].Cells[2].Value;
                    }
                    else if (Rows[i].Cells[2].ValueType == typeof(short))
                    {
                        tagBlock.Data = SetTagBlockData(tagBlock.Data, BitConverter.GetBytes((short)Rows[i].Cells[2].Value), (int)Rows[i].Cells[3].Value);
                    }
                    else if (Rows[i].Cells[2].ValueType == typeof(ushort))
                    {
                        tagBlock.Data = SetTagBlockData(tagBlock.Data, BitConverter.GetBytes((ushort)Rows[i].Cells[2].Value), (int)Rows[i].Cells[3].Value);
                    }
                    else if (Rows[i].Cells[2].ValueType == typeof(int))
                    {
                        tagBlock.Data = SetTagBlockData(tagBlock.Data, BitConverter.GetBytes((int)Rows[i].Cells[2].Value), (int)Rows[i].Cells[3].Value);
                    }
                    else if (Rows[i].Cells[2].ValueType == typeof(float))
                    {
                        tagBlock.Data = SetTagBlockData(tagBlock.Data, BitConverter.GetBytes((float)Rows[i].Cells[2].Value), (int)Rows[i].Cells[3].Value);
                    }
                }
            }
        }

        private byte[] SetTagBlockData(byte[] tagBlockData, byte[] buffer, int startIndex)
        {
            for (int index = 0; index < buffer.Length; index++)
                tagBlockData[startIndex + index] = buffer[index];
            return tagBlockData;
        }

        private DataGridViewRow[] LoadRows(XmlReader xmlReader) { return LoadRows(xmlReader, 0); }

        private DataGridViewRow[] LoadRows(XmlReader xmlReader, int depth)
        {

            List<DataGridViewRow> Rows = new List<DataGridViewRow>();
            xmlReader.Read();
            int offset = 0;
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    switch (xmlReader.Name.ToLower())
                    {
                        case "tagblock":
                            Rows.Add(new MetaGridViewTagBlockRow());
                            Rows[Rows.Count - 1].Cells[0].Value = xmlReader.GetAttribute("name");
                            Rows[Rows.Count - 1].Cells[1].ValueType = typeof(TagBlockArray);
                            Rows[Rows.Count - 1].Cells[3].Value = offset;
                            Rows[Rows.Count - 1].Cells[4].Value = depth + 1;
                            DataGridViewRow[] tagBlockRows = LoadRows(xmlReader.ReadSubtree(), depth + 1);
                            xmlReader.Skip();
                            (Rows[Rows.Count - 1] as MetaGridViewTagBlockRow).Length = tagBlockRows.Length;
                            Rows.AddRange(tagBlockRows);
                            offset += 8;
                            break;
                        case "unused":
                            int unusedCount = int.Parse(xmlReader.GetAttribute("count"));
                            offset += unusedCount;
                            break;
                        case "byte":
                            Rows.Add(new MetaGridViewRow());
                            Rows[Rows.Count - 1].Cells[0].Value = xmlReader.GetAttribute("name");
                            Rows[Rows.Count - 1].Cells[1].Value = "byte";
                            Rows[Rows.Count - 1].Cells[2].ValueType = typeof(byte);
                            Rows[Rows.Count - 1].Cells[3].Value = offset;
                            Rows[Rows.Count - 1].Cells[4].Value = depth;
                            offset += 1;
                            break;
                        case "short":
                            Rows.Add(new MetaGridViewRow());
                            Rows[Rows.Count - 1].Cells[0].Value = xmlReader.GetAttribute("name");
                            Rows[Rows.Count - 1].Cells[1].Value = "short";
                            Rows[Rows.Count - 1].Cells[2].ValueType = typeof(short);
                            Rows[Rows.Count - 1].Cells[3].Value = offset;
                            Rows[Rows.Count - 1].Cells[4].Value = depth;
                            offset += 2;
                            break;
                        case "ushort":
                            Rows.Add(new MetaGridViewRow());
                            Rows[Rows.Count - 1].Cells[0].Value = xmlReader.GetAttribute("name");
                            Rows[Rows.Count - 1].Cells[1].Value = "ushort";
                            Rows[Rows.Count - 1].Cells[2].ValueType = typeof(ushort);
                            Rows[Rows.Count - 1].Cells[3].Value = offset;
                            Rows[Rows.Count - 1].Cells[4].Value = depth;
                            offset += 2;
                            break;
                        case "enum16":
                            Rows.Add(new MetaGridViewEnumRow());
                            Rows[Rows.Count - 1].Cells[0].Value = xmlReader.GetAttribute("name");
                            Rows[Rows.Count - 1].Cells[1].Value = "enum16";
                            Rows[Rows.Count - 1].Cells[2].ValueType = typeof(string);
                            Rows[Rows.Count - 1].Cells[3].Value = offset;
                            Rows[Rows.Count - 1].Cells[4].Value = depth;
                            XmlReader xmlReaderOptions = xmlReader.ReadSubtree();
                            int value = 0;
                            while (xmlReaderOptions.Read())
                            {
                                if (xmlReaderOptions.NodeType == XmlNodeType.Element)
                                {
                                    if (xmlReaderOptions.Name.ToLower() == "option")
                                    {
                                        string name = xmlReaderOptions.GetAttribute("name");
                                        try { value = int.Parse(xmlReaderOptions.GetAttribute("value")); }
                                        catch (ArgumentNullException) { }
                                        finally { }
                                        (Rows[Rows.Count - 1].Cells[2] as MetaGridViewEnumComboBoxCell).AddEnumOption(name, value);
                                        value++;
                                    }
                                }
                            }
                            xmlReader.Skip();
                            offset += 2;
                            break;
                        case "int":
                            Rows.Add(new MetaGridViewRow());
                            Rows[Rows.Count - 1].Cells[0].Value = xmlReader.GetAttribute("name");
                            Rows[Rows.Count - 1].Cells[1].Value = "int";
                            Rows[Rows.Count - 1].Cells[2].ValueType = typeof(int);
                            Rows[Rows.Count - 1].Cells[3].Value = offset;
                            Rows[Rows.Count - 1].Cells[4].Value = depth;
                            offset += 4;
                            break;
                        case "uint":
                            Rows.Add(new MetaGridViewRow());
                            Rows[Rows.Count - 1].Cells[0].Value = xmlReader.GetAttribute("name");
                            Rows[Rows.Count - 1].Cells[1].Value = "uint";
                            Rows[Rows.Count - 1].Cells[2].ValueType = typeof(uint);
                            Rows[Rows.Count - 1].Cells[3].Value = offset;
                            Rows[Rows.Count - 1].Cells[4].Value = depth;
                            offset += 4;
                            break;
                        case "float":
                            Rows.Add(new MetaGridViewRow());
                            Rows[Rows.Count - 1].Cells[0].Value = xmlReader.GetAttribute("name");
                            Rows[Rows.Count - 1].Cells[1].Value = "single";
                            Rows[Rows.Count - 1].Cells[2].ValueType = typeof(float);
                            Rows[Rows.Count - 1].Cells[3].Value = offset;
                            Rows[Rows.Count - 1].Cells[4].Value = depth;
                            offset += 4;
                            break;
                    }
                }
            }
            return Rows.ToArray();
        }

        private void DisableRows(int startIndex, int count)
        {
            for (int i = startIndex; i < startIndex + count; i++)
            {
                Rows[i].ReadOnly = true;
                Rows[i].Cells[4].Value = -1;
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!loaded) return;
            if (Rows[e.RowIndex] is MetaGridViewTagBlockRow)
            {
                if (e.ColumnIndex == 2)
                {
                    LoadValues((Rows[e.RowIndex] as MetaGridViewTagBlockRow).tagBlockArray.TagBlocks[(int)Rows[e.RowIndex].Cells[e.ColumnIndex].Value], e.RowIndex + 1, (Rows[e.RowIndex] as MetaGridViewTagBlockRow).Length);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                BeginEdit(false);
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (CurrentCell.ColumnIndex == 2)
            {
                if (e.Control is DataGridViewComboBoxEditingControl)
                {
                    DataGridViewComboBoxEditingControl control = e.Control as DataGridViewComboBoxEditingControl;
                    control.SelectedValueChanged += new EventHandler(control_SelectedValueChanged);
                }
            }
        }

        void control_SelectedValueChanged(object sender, EventArgs e)
        {
            if (CurrentRow is MetaGridViewTagBlockRow) ;
            LoadValues((CurrentRow as MetaGridViewTagBlockRow).tagBlockArray.TagBlocks[(int)(sender as DataGridViewComboBoxEditingControl).SelectedItem], CurrentRow.Index + 1, (CurrentRow as MetaGridViewTagBlockRow).Length);
        }

        private void ApplyStyle()
        {
            uint color = 0xFFFFFFFF;
            int Dec = 0x00101200;
            for (int i = 0; i < RowCount; i++)
            {
                if ((int)Rows[i].Cells[4].Value == -1)
                    Rows[i].DefaultCellStyle = Disabled;
                else Rows[i].DefaultCellStyle.BackColor = Color.FromArgb((int)(color - (Dec * (int)Rows[i].Cells[4].Value)));
            }
        }

        private class MetaGridViewEnumRow : MetaGridViewRow
        {
            public MetaGridViewEnumRow()
                : base()
            {
                Cells[2] = new MetaGridViewEnumComboBoxCell();
            }
        }

        private class MetaGridViewEnumComboBoxCell : DataGridViewComboBoxCell
        {
            List<int> Values;
            List<string> Names;

            public MetaGridViewEnumComboBoxCell()
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

        private class MetaGridViewTagBlockRow : MetaGridViewRow
        {
            public TagBlockArray tagBlockArray;
            public int Length;

            public MetaGridViewTagBlockRow()
                : base()
            {
                Cells[2] = new DataGridViewComboBoxCell();
                (Cells[2] as DataGridViewComboBoxCell).DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            }
        }

        private class MetaGridViewRow : DataGridViewRow
        {
            public MetaGridViewRow()
            {
                Cells.AddRange(new DataGridViewCell[]{
                    new DataGridViewTextBoxCell(),
                    new DataGridViewTextBoxCell(),
                    new DataGridViewTextBoxCell(),
                    new DataGridViewTextBoxCell(),
                    new DataGridViewTextBoxCell(),});
            }
        }
    }
}