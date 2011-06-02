using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sunfish;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace Sunfish.GUI
{
    public partial class MainForm : Form
    {
        Project project;
        OpenFileDialog openMapDialog;
        OpenFileDialog openProejctDialog;

        SolutionExplorer f;

        Dictionary<string, Tag> Tags = new Dictionary<string, Tag>();


        public MainForm()
        {
            Properties.Settings.Default.ProjectsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Sunfish 2011\\Projects");
            if(!Directory.Exists(Properties.Settings.Default.ProjectsDirectory))
                Directory.CreateDirectory(Properties.Settings.Default.ProjectsDirectory);
            InitializeComponent();
            Globals.StatusChangeHandler s = new Globals.StatusChangeHandler(Status);
            Globals.StatusChanged += s;

            #region Dialogs Initialization

            openMapDialog = new OpenFileDialog();
            openMapDialog.Filter = "Halo 2 Cache File (*.map) | *.map";

            openProejctDialog = new OpenFileDialog();
            openProejctDialog.Filter = "Sunfish Project File (*.h2proj) | *.h2proj";
            openProejctDialog.InitialDirectory = Properties.Settings.Default.ProjectsDirectory;

            #endregion


            f = new SolutionExplorer();
            f.treeView1.DoubleClick += new EventHandler(treeView1_DoubleClick);
            f.Text = "Solution Explorer";
            f.Show(this.dockPanel1,  WeifenLuo.WinFormsUI.Docking.DockState.DockRight);
            this.dockPanel1.ActiveDocumentChanged += new EventHandler(dockPanel1_ActiveDocumentChanged);

            project = Project.Load(@"E:\Users\root\Documents\Sunfish 2011\Projects\Yogurt\Yogurt.h2proj");
            LoadProject();
        }

        void dockPanel1_ActiveDocumentChanged(object sender, EventArgs e)
        {
            if (Disposing) { return; }
            foreach(IDockContent content in dockPanel1.Documents)
            {
                if (content == dockPanel1.ActiveDocument) { content.OnActivated(null); }
                else content.OnDeactivate(null);
            }
        }

        void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (f.treeView1.SelectedNode == null) return;
            else if (f.treeView1.SelectedNode.Tag is TagInformation)
                LoadTag(f.treeView1.SelectedNode.Tag as TagInformation);
        }

        private void LoadTag(TagInformation tagInformation)
        {
            if (Tags.ContainsKey(tagInformation.Path))
            {
                foreach (DockContent dc in this.dockPanel1.Documents)
                    if (dc.Tag == Tags[tagInformation.Path])
                    {
                        dc.Activate();
                        break;
                    }
            }
            else
            {
                switch (Sunfish.Tag.Path.GetTagType(tagInformation.Path))
                {

                    case "scnr":
                        MetaTool mt = new MetaTool();
                        mt.Text = f.treeView1.SelectedNode.Text;
                        mt.Show(this.dockPanel1, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                        Sunfish.Tag scnr = new Tag(Path.Combine(project.SourceDirectory, (f.treeView1.SelectedNode.Tag as TagInformation).Path));
                        mt.LoadTag(scnr);
                        Tags.Add(tagInformation.Path, scnr);
                        break;

                    case "bitm":
                        BitmapTool bt = new BitmapTool();
                        bt.Text = f.treeView1.SelectedNode.Text;
                        bt.Show(this.dockPanel1, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                        break;

                    case "mode":
                        ModelEditor modeEdit = new ModelEditor();
                        modeEdit.Text = f.treeView1.SelectedNode.Text;
                        modeEdit.Show(this.dockPanel1, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                        Sunfish.Tag mode = new Tag(Path.Combine(project.SourceDirectory, (f.treeView1.SelectedNode.Tag as TagInformation).Path));
                        modeEdit.LoadTag(mode);
                        break;
                }
            }
        }

        public void Status(string message)
        { lblStatus.Text = message; }

        private void mapToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            if (openMapDialog.ShowDialog() == DialogResult.OK)
            {
                Map map = new Map(File.Open(openMapDialog.FileName, FileMode.Open));
                project.ImportMap(map);
                DisplaySourceFiles();
            }
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateFileDialog cfd = new CreateFileDialog("New Project");
            if (cfd.ShowDialog() == DialogResult.OK)
            {
                project = Project.Create(Path.Combine(Properties.Settings.Default.ProjectsDirectory, cfd.FileName), cfd.FileName);
                LoadProject();
            }
        }

        private void LoadProject()
        {
            DisplaySourceFiles();
            importToolStripMenuItem.Enabled = true;
            closeProjectToolStripMenuItem.Enabled = true;
        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openProejctDialog.ShowDialog() == DialogResult.OK)
            {
                project = Project.Load(openProejctDialog.FileName);
                LoadProject();
            }
        }

        private void DisplaySourceFiles()
        {
            if (project != null)
            {
                f.treeView1.Nodes.Clear();
                f.treeView1.Nodes.Add(String.Format("Solution \'{0}\'", project.Name));
                f.treeView1.BeginUpdate();
                foreach (string filename in project.SourceFiles)
                {
                    string[] parts = filename.Split(new char[] { Path.DirectorySeparatorChar });
                    CreateNodes(f.treeView1.Nodes[0].Nodes, parts, 0);
                }
                f.treeView1.Nodes[0].Expand();
                f.treeView1.EndUpdate();
                this.Text = project.Name + " - Sunfish 2011";
            }
        }

        private void CreateNodes(TreeNodeCollection treeNodeCollection, string[] parts, int p)
        {
            if (!treeNodeCollection.ContainsKey(parts[p]))
            {
                string extension = Path.GetExtension(parts[p]);
                switch (extension)
                {
                    case "":
                        treeNodeCollection.Add(parts[p], Path.ChangeExtension(parts[p], null));
                        treeNodeCollection[treeNodeCollection.IndexOfKey(parts[p])].Tag = parts[p];
                        break;
                    case Sunfish.Tag.Path.Extension:
                        FolderTreeNode ftn = new FolderTreeNode();
                        ftn.Text = Path.ChangeExtension(parts[p], null);
                        ftn.Name = parts[p];
                        string path = string.Empty;
                        foreach (string s in parts)
                            path = Path.Combine(path, s);
                        ftn.Tag = new TagInformation(path);
                        treeNodeCollection.Add(ftn);
                        break;
                }
            }
            if (parts.Length > p + 1)
                CreateNodes(treeNodeCollection[treeNodeCollection.IndexOfKey(parts[p])].Nodes, parts, p + 1);
        }

        private void closeProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseProject();
        }

        private void CloseProject()
        {
            if (project != null)
            {
                project.Save();
                project = null;
                this.Text = "Sunfish 2011";
                f.treeView1.Nodes.Clear();
                importToolStripMenuItem.Enabled = false; 
                closeProjectToolStripMenuItem.Enabled = false;
            }
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            project.Compile();
        }
    }
}
