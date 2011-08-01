using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;

namespace Sunfish.GUI
{
    public partial class SolutionExplorer : DockContent
    {
        public SolutionExplorer()
        {
            InitializeComponent();
        }

        public bool SaveLayout(string filename)
        {
            List<bool> states = new List<bool>();
            ProcessNodeStates(treeView1.Nodes, states);
            bool result = true;
            try
            {
                using (BinaryWriter bw = new BinaryWriter(File.Create(filename)))
                    foreach (bool b in states) { bw.Write(b); }
            }
            catch { result = false; }
            return result;
        }

        public void LoadLayout(string filename)
        {
            List<bool> states = new List<bool>();
            try
            {
                using (BinaryReader br = new BinaryReader(File.OpenRead(filename)))
                    while (br.BaseStream.Position < br.BaseStream.Length) states.Add(br.ReadBoolean());
                int index = -1;
                SetNodeStates(treeView1.Nodes, states, ref index);
            }
            catch {}
        }

        private void SetNodeStates(TreeNodeCollection treeNodeCollection, List<bool> states, ref int index)
        {
            for (int i = 0; i < treeNodeCollection.Count; i++)
            {
                index++;
                if (index > states.Count) return;
                if (states[index] == true)
                {
                    treeNodeCollection[i].Expand();
                    SetNodeStates(treeNodeCollection[i].Nodes, states, ref index);
                }
            }
        }

        private void ProcessNodeStates(TreeNodeCollection treeNodeCollection, List<bool> states)
        {
            foreach (TreeNode node in treeNodeCollection)
                if (node.IsExpanded)
                {
                    states.Add(true);
                    ProcessNodeStates(node.Nodes, states);
                }
                else states.Add(false);
        }
    }

    public class ResourceTreeNode : TreeNode {
        public string Path;
    }

    public class FolderTreeNode : ResourceTreeNode
    {

    }

    public class ClassTreeNode : ResourceTreeNode { }

    public class TagInformation
    {
        public string Path;

        public TagInformation(string path)
        {
            Path = path;
        }
    }
}
