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
        OpenFileDialog openProjectDialog;
        SaveFileDialog saveTagDialog;

        SolutionExplorer f;

        Dictionary<string, int> saveFilters;

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

            openProjectDialog = new OpenFileDialog();
            openProjectDialog.Filter = "Sunfish Project File (*.h2proj) | *.h2proj";
            openProjectDialog.InitialDirectory = Properties.Settings.Default.ProjectsDirectory;

            saveTagDialog = new SaveFileDialog();
            saveTagDialog.Filter = "Sunfish Tag File (*.sf) | *.sf";
            saveFilters = new Dictionary<string, int>(Index.Types.Length);
            for (int i = 0; i < Index.Types.Length; i++)
            {
                saveFilters.Add(Index.Types[i].ToString(), i + 2);
                saveTagDialog.Filter += String.Format("|Sunfish {0} File (*.{1}.sf) | *.{1}.sf", Index.Types[i].ToString(), Index.Types[i].ToPathSafeString());
            }
            saveTagDialog.SupportMultiDottedExtensions = true;
            saveTagDialog.AddExtension = true;

            #endregion


            f = new SolutionExplorer();
            f.treeView1.DoubleClick += new EventHandler(treeView1_DoubleClick);
            f.Text = "Solution Explorer";
            f.Show(this.dockPanel1,  WeifenLuo.WinFormsUI.Docking.DockState.DockRight);
            this.dockPanel1.ActiveDocumentChanged += new EventHandler(dockPanel1_ActiveDocumentChanged);

            project = Project.Load(@"E:\Users\root\Documents\Sunfish 2011\Projects\frebuild\frebuild.h2proj");
            LoadProject();
        }

        void dockPanel1_ActiveDocumentChanged(object sender, EventArgs e)
        {
            if (Disposing) { return; }
            foreach (IDockContent content in dockPanel1.Documents)
            {
                if (content == dockPanel1.ActiveDocument) { content.OnActivated(null); }
                else content.OnDeactivate(null);
            }
            if (dockPanel1.ActiveDocument is SunfishDocument) TagSaveOptions(true);
            else TagSaveOptions(false);
        }

        private void TagSaveOptions(bool enabled)
        {
            saveToolStripMenuItem.Enabled = enabled;
            saveAsToolStripMenuItem.Enabled = enabled;
        }

        void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (f.treeView1.SelectedNode == null) return;
            else if (f.treeView1.SelectedNode is ClassTreeNode)
                LoadTag(f.treeView1.SelectedNode as ClassTreeNode);
        }

        private void LoadTag(ClassTreeNode node)
        {
            foreach (DockContent dc in this.dockPanel1.Documents)
                if ((string)(dc.Tag) == node.Path)
                {
                    dc.Activate();
                    return;
                }
            switch (Sunfish.Tag.Path.GetTagType(node.Path))
            {

                case "scnr":
                    MetaTool mt = new MetaTool();
                    mt.Text = f.treeView1.SelectedNode.Text;
                    mt.Show(this.dockPanel1, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                    Sunfish.Tag scnr = new Tag(Path.Combine(project.SourceDirectory, (f.treeView1.SelectedNode.Tag as TagInformation).Path));
                    mt.LoadTag(scnr);
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
                    modeEdit.LoadTag(node.Path);
                    break;
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
                map.BaseStream.Close();
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
            projectToolStripMenuItem.Enabled = true;
            projectToolStripMenuItem.Visible = true;
            closeProjectToolStripMenuItem.Enabled = true;
            buildToolStripMenuItem.Enabled = true;
            buildToolStripMenuItem.Visible = true;
            Directory.SetCurrentDirectory(project.SourceDirectory);
            fileSystemWatcher1.Path = project.SourceDirectory;
        }

        private void CloseProject()
        {
            if (project != null)
            {
                project.Save();
                project = null;
                this.Text = "Sunfish 2011";
                f.treeView1.Nodes.Clear();
                projectToolStripMenuItem.Enabled = false;
                projectToolStripMenuItem.Visible = false;
                buildToolStripMenuItem.Enabled = false;
                buildToolStripMenuItem.Visible = false;
                closeProjectToolStripMenuItem.Enabled = false;
            }
        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openProjectDialog.ShowDialog() == DialogResult.OK)
            {
                project = Project.Load(openProjectDialog.FileName);
                LoadProject();
            }
        }

        private void DisplaySourceFiles()
        {
            if (project != null)
            {
                List<string> filenames = new List<string>(Directory.GetFiles(project.SourceDirectory, String.Format("*{0}", Sunfish.Tag.Path.Extension), SearchOption.AllDirectories));
                filenames.Sort();
                f.treeView1.Nodes.Clear();
                f.treeView1.Nodes.Add(String.Format("Solution \'{0}\'", project.Name));
                f.treeView1.Nodes[0].Tag = project.SourceDirectory;
                f.treeView1.BeginUpdate();
                foreach (string filename in filenames)
                {
                    string relativeFilename = filename.Substring(project.SourceDirectory.Length);
                    string[] parts = relativeFilename.Split(new char[] { Path.DirectorySeparatorChar }); 
                    string parent = project.SourceDirectory;
                    CreateNodes(f.treeView1.Nodes[0].Nodes, parts, 0, ref parent);
                }
                f.treeView1.Nodes[0].Expand();
                f.treeView1.EndUpdate();
                this.Text = project.Name + " - Sunfish 2011";
            }
        }

        private void RefreshSourceFiles()
        {
            if (project != null)
            {
                List<string> filenames = new List<string>(Directory.GetFiles(project.SourceDirectory, String.Format("*{0}", Sunfish.Tag.Path.Extension), SearchOption.AllDirectories));
                filenames.Sort();
                f.treeView1.BeginUpdate();
                foreach (string filename in filenames)
                {
                    string relativeFilename = filename.Substring(project.SourceDirectory.Length);
                    string[] parts = relativeFilename.Split(new char[] { Path.DirectorySeparatorChar });
                    string parent = project.SourceDirectory;
                    CreateNodes(f.treeView1.Nodes[0].Nodes, parts, 0, ref parent);
                }
                f.treeView1.EndUpdate();
            }
        }

        private void CreateNodes(TreeNodeCollection treeNodeCollection, string[] parts, int index, ref string parent)
        {
            if (!treeNodeCollection.ContainsKey(parts[index]))
            {
                string extension = Path.GetExtension(parts[index]);
                ResourceTreeNode node;
                switch (extension)
                {
                    case "":
                        node = new FolderTreeNode();
                        node.Text = Path.ChangeExtension(parts[index], null);
                        node.Name = parts[index];
                        string path = string.Empty;
                        for (int i = 0; i <= index; i++)
                            path = Path.Combine(path, parts[i]);
                        node.Path = Path.Combine(parent, path);
                        treeNodeCollection.Add(node);
                        break;
                    case Sunfish.Tag.Path.Extension:
                        node = new ClassTreeNode();
                        node.Text = Path.ChangeExtension(parts[index], null);
                        node.Name = parts[index];
                        path = string.Empty;
                        foreach (string s in parts)
                            path = Path.Combine(path, s);
                        node.Path = Path.Combine(parent, path);
                        treeNodeCollection.Add(node);
                        break;
                }
            }
            if (parts.Length > index + 1)
                CreateNodes(treeNodeCollection[treeNodeCollection.IndexOfKey(parts[index])].Nodes, parts, index + 1, ref parent);
        }

        private void closeProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseProject();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dockPanel1.ActiveDocument is SunfishDocument)
                (this.dockPanel1.ActiveDocument as SunfishDocument).Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dockPanel1.ActiveDocument is SunfishDocument)
            {
                SunfishDocument sunDoc = (this.dockPanel1.ActiveDocument as SunfishDocument);
                string tagname = sunDoc.HaloTag.Filename;
                DirectoryInfo dir = Directory.GetParent(tagname);
                saveTagDialog.InitialDirectory = dir.ToString();
                saveTagDialog.FilterIndex = saveFilters[sunDoc.HaloTag.Type];
                if (saveTagDialog.ShowDialog() != DialogResult.OK) { return; }
                string filename = saveTagDialog.FileName;
                sunDoc.SaveAs(filename);
            }
        }

        private void fileSystemWatcher1_Created(object sender, FileSystemEventArgs e)
        {
            project.ImportTag(e.FullPath);
            RefreshSourceFiles();
        }

        private void releaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (project != null)
            {
                ProjectCompiler pc = new ProjectCompiler();
                pc.Compile(this.project, true);
            }
        }

        private void debugToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (project != null)
            {
                ProjectCompiler pc = new ProjectCompiler();
                pc.Compile(this.project, false);
            }
        }
    }
}
