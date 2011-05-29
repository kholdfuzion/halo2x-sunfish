using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sunfish;
using Sunfish.TagStructures;

namespace WindowsFormsApplication1
{
    public partial class MainForm : Form
    {
        bool Loaded = false;

        Tag Tag;
        TagBlock TagBlock;

        public MainForm()
        {
            InitializeComponent();
        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txbFilename.Text = ofd.FileName;
                LoadTag();
            }
        }

        private void LoadTag()
        {
            //Create a new Tag object from file
            Tag = new Tag(txbFilename.Text);
            Loaded = true;
            DisplayTagInformation();
        }

        private void DisplayTagInformation()
        {
            if (Loaded)
            {
                txbTagInfo.Text =
string.Format(@"Meta Size: {0}
Raw Count: {1}
Raw Size: {2}
StringID Count: {3}
External Tag References: {4}", Tag.TagStream.Length, Tag.RawInfos.Length, Tag.RawStream.Length, Tag.StringIdNames.Count, Tag.TagReferences.Count);
            }
        }

        private void cmdLoadTagBlock_Click(object sender, EventArgs e)
        {
            if (Loaded)
            {
                //Create new TagBlock object from Tag object
                TagBlock = TagBlock.CreateFromTag(Tag);
                DisplayTagBlockInTreeView();
            }
        }

        private void DisplayTagBlockInTreeView()
        {
            tvTagBlock.Nodes.Clear();
            tvTagBlock.BeginUpdate();
            LoadTagBlockValuesAsNodes(tvTagBlock.Nodes, TagBlock);
            tvTagBlock.EndUpdate();
        }

        private void LoadTagBlockValuesAsNodes(TreeNodeCollection treeNodeCollection, TagBlock block)
        {
            //Add this TagBlock (chunk) to the Nodes
            treeNodeCollection.Add(block.ToString());
            int index = treeNodeCollection.Count - 1;
            treeNodeCollection[index].ContextMenuStrip = chunkMenu;
            //Add the TagBlock (chunk) object to the Tag to let use edit it directly from the node
            treeNodeCollection[index].Tag = block;
            //Values might be null, dealio
            if (block.Values == null) return;
            foreach (Value val in block.Values)
            {
                //the Values can be a bunch of things, we only want the ones that are TagBlockArrays (reflexives)
                if (val is TagBlockArray)
                {
                    treeNodeCollection[index].Nodes.Add(val.ToString());
                    treeNodeCollection[index].Nodes[treeNodeCollection[index].Nodes.Count - 1].ContextMenuStrip = reflexiveMenu;
                    //Add the TagBlockArray object (reflexive) to the Tag to let us edit it directly from the node
                    treeNodeCollection[index].Nodes[treeNodeCollection[index].Nodes.Count - 1].Tag = val;
                    //TagBlocks also might be null, dealio
                    if ((val as TagBlockArray).TagBlocks == null) continue;
                    foreach (TagBlock tagBlock in (val as TagBlockArray).TagBlocks)
                    {
                        //Recurse
                        LoadTagBlockValuesAsNodes(treeNodeCollection[index].Nodes[treeNodeCollection[index].Nodes.Count - 1].Nodes, tagBlock);
                    }
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tvTagBlock.SelectedNode.Tag is TagBlockArray)
            {
                TagBlockArray arr = (tvTagBlock.SelectedNode.Tag as TagBlockArray);
                //Add new 'Default' (blank) TagBlock (chunk) to this array (reflexive)
                arr.Add(arr.Default);
                //Hacky... fixes the addresses of the tagBlocks 
                //(I used this here to make the ui look good, but you only need to do this when you are done all the editing, or not, w/e)
                TagBlock.Update();
                DisplayTagBlockInTreeView();
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (tvTagBlock.SelectedNode.Tag is TagBlock && tvTagBlock.SelectedNode.Parent != null)
            {
                TagBlockArray arr = (tvTagBlock.SelectedNode.Parent.Tag as TagBlockArray);
                arr.Remove((TagBlock)tvTagBlock.SelectedNode.Tag);
                TagBlock.Update();
                DisplayTagBlockInTreeView();
            }
        }

        private void cmdSaveTagBlock_Click(object sender, EventArgs e)
        {
            TagBlock.Save(Tag);
            DisplayTagInformation();
        }

        private void cmdSaveTag_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Tag.Save(sfd.FileName);
                MessageBox.Show("Saved!");
            }
        }
    }
}
