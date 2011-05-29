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
    public partial class SolutionExplorer : DockContent
    {
        public SolutionExplorer()
        {
            InitializeComponent();
        }
    }

    public class FolderTreeNode : TreeNode
    {
    }

    public class ClassTreeNode : TreeNode { }

    public class TagInformation
    {
        public string Path;

        public TagInformation(string path)
        {
            Path = path;
        }
    }
}
