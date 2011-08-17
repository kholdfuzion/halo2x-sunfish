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
using System.Runtime.Remoting.Messaging;

namespace Sunfish.GUI
{
    public partial class MainForm : Form
    {
        Project project;
        OpenFileDialog openMapDialog;
        OpenFileDialog openProjectDialog;
        SaveFileDialog saveTagDialog;

        SolutionExplorer Explorer;

        Dictionary<string, int> saveFilters;

        Benchmark mark = new Benchmark();
        Decompile dec;

        public MainForm()
        {
            testToolStripMenuItem_Click(null, null);

            if (Properties.Settings.Default.ProjectsDirectory == null)
                Properties.Settings.Default.ProjectsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Sunfish 2011\\Projects");
            if(!Directory.Exists(Properties.Settings.Default.ProjectsDirectory))
                Directory.CreateDirectory(Properties.Settings.Default.ProjectsDirectory);

            InitializeComponent();

            LoadDefaultWorkspace(); 

            #region Status Handler

            Globals.StatusChangeHandler s = new Globals.StatusChangeHandler(Status);
            Globals.StatusChanged += s;



            #endregion

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

            #region auto load
#if DEBUG
            project = new Project();
            project.Load(@"O:\Sunfish 2011\Projects\taco\taco.h2proj");
            LoadProject();
#endif
            #endregion
        }

        delegate void Decompile(Map m);


        private void LoadDefaultWorkspace()
        {
            Explorer = new SolutionExplorer();
            Explorer.treeView1.DoubleClick += new EventHandler(treeView1_DoubleClick);
            Explorer.Text = "Solution Explorer";
            PrimaryDock.ActiveDocumentChanged += new EventHandler(dockPanel1_ActiveDocumentChanged);
            Explorer.Show(this.PrimaryDock, WeifenLuo.WinFormsUI.Docking.DockState.DockRight);
        }

        void dockPanel1_ActiveDocumentChanged(object sender, EventArgs e)
        {
            if (Disposing) { return; }
            foreach (IDockContent content in PrimaryDock.Documents)
            {
                if (content == PrimaryDock.ActiveDocument) { content.OnActivated(null); }
                else content.OnDeactivate(null);
            }
            if (PrimaryDock.ActiveDocument is SunfishEditor) TagSaveOptions(true);
            else TagSaveOptions(false);
        }

        private void TagSaveOptions(bool enabled)
        {
            saveToolStripMenuItem.Enabled = enabled;
            saveAsToolStripMenuItem.Enabled = enabled;
        }

        void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (Explorer.treeView1.SelectedNode == null) return;
            else if (Explorer.treeView1.SelectedNode is ClassTreeNode)
                LoadTag(Explorer.treeView1.SelectedNode as ClassTreeNode);
        }

        private void LoadTag(ClassTreeNode node)
        {
            foreach (DockContent dc in this.PrimaryDock.Documents)
                if ((string)(dc.Tag) == node.Path)
                {
                    dc.Activate();
                    return;
                }
            switch (Sunfish.Tag.Path.GetTagType(node.Path))
            {

                case "scnr":
                    MetaTool mt = new MetaTool();
                    mt.Text = Explorer.treeView1.SelectedNode.Text;
                    mt.Show(this.PrimaryDock, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                    Sunfish.Tag scnr = new Tag(node.Path);
                    mt.LoadTag(scnr);
                    break;

                case "bitm":
                    BitmapTool bt = new BitmapTool();
                    bt.Text = Explorer.treeView1.SelectedNode.Text;
                    bt.Show(this.PrimaryDock, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                    break;

                case "mode":
                    ModelEditor modeEdit = new ModelEditor();
                    modeEdit.Text = Explorer.treeView1.SelectedNode.Text;
                    modeEdit.Show(this.PrimaryDock, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                    modeEdit.LoadTag(node.Path);
                    break;
                case "coll":
                    ModelEditor collEdit = new ModelEditor();
                    collEdit.Text = Explorer.treeView1.SelectedNode.Text;
                    collEdit.Show(this.PrimaryDock, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                    collEdit.LoadTag(node.Path);
                    break;
                default:
                    mt = new MetaTool();
                    mt.Text = Explorer.treeView1.SelectedNode.Text;
                    mt.Show(this.PrimaryDock, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                    Sunfish.Tag tag = new Tag(node.Path);
                    mt.LoadTag(tag);
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
                dec = new Decompile(g);
                dec.BeginInvoke(map, new AsyncCallback(done), this);
            }
        }

        public void g(Map map)
        {
            project.ImportMap(map);
            map.BaseStream.Close();
        }

        public void done(IAsyncResult ar)
        {
            AsyncResult result = (AsyncResult)ar;
            Decompile caller = (Decompile)result.AsyncDelegate;
            caller.EndInvoke(result);
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
            Explorer.LoadLayout(Path.Combine(project.RootDirectory, "user.settings"));

            project.OnImportBegin += new Project.BeginImport(project_OnImportBegin);
            project.OnImportCompleted += new Project.BeginImport(project_OnImportCompleted);
        }

        void project_OnImportCompleted()
        {
            BeginInvoke(new DoSomething(ImportComplete));
        }

        void project_OnImportBegin()
        {
            BeginInvoke(new DoSomething(ImportBegin));
        }

        delegate void DoSomething();
        void ImportBegin()
        {
            mark.Begin();
            this.Enabled = false;
            fileSystemWatcher1.EnableRaisingEvents = false;
        }
        void ImportComplete()
        {
            mark.End();
            this.Enabled = true;
            fileSystemWatcher1.EnableRaisingEvents = true;
            DisplaySourceFiles();
            MessageBox.Show(mark.Result);
        }

        private void CloseProject()
        {
            if (project != null)
            {
                project.Save();
                Explorer.SaveLayout(Path.Combine(project.RootDirectory, "user.settings"));
                project = null;
                this.Text = "Sunfish 2011";
                Explorer.treeView1.Nodes.Clear();
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
                if (project != null) { CloseProject(); }
                project = new Project();
                project.Load(openProjectDialog.FileName);
                LoadProject();
            }
        }

        private void DisplaySourceFiles()
        {
            if (project != null)
            {
                Explorer.treeView1.Nodes.Clear();
                Explorer.treeView1.Nodes.Add(String.Format("Solution \'{0}\'", project.Name));
                Explorer.treeView1.Nodes[0].Tag = project.SourceDirectory;
                Explorer.treeView1.BeginUpdate();
                foreach (string filename in project.SourceFiles)
                {
                    string relativeFilename = filename.Substring(project.SourceDirectory.Length);
                    string[] parts = relativeFilename.Split(new char[] { Path.DirectorySeparatorChar }); 
                    string parent = project.SourceDirectory;
                    CreateNodes(Explorer.treeView1.Nodes[0].Nodes, parts, 0, ref parent);
                }
                Explorer.treeView1.Nodes[0].Expand();
                Explorer.treeView1.EndUpdate();
                this.Text = project.Name + " - Sunfish 2011";
            }
        }

        private void UpdateSourceFiles(string filename)
        {
            Explorer.treeView1.BeginUpdate();
            string relativeFilename = filename.Substring(project.SourceDirectory.Length);
            string[] parts = relativeFilename.Split(new char[] { Path.DirectorySeparatorChar });
            string parent = project.SourceDirectory;
            CreateNodes(Explorer.treeView1.Nodes[0].Nodes, parts, 0, ref parent);
            Explorer.treeView1.EndUpdate();
        }

        private void RefreshSourceFiles()
        {
            if (project != null)
            {
                List<string> filenames = new List<string>(Directory.GetFiles(project.SourceDirectory, String.Format("*{0}", Sunfish.Tag.Path.Extension), SearchOption.AllDirectories));
                filenames.Sort();
                Explorer.treeView1.BeginUpdate();
                foreach (string filename in filenames)
                {
                    string relativeFilename = filename.Substring(project.SourceDirectory.Length);
                    string[] parts = relativeFilename.Split(new char[] { Path.DirectorySeparatorChar });
                    string parent = project.SourceDirectory;
                    CreateNodes(Explorer.treeView1.Nodes[0].Nodes, parts, 0, ref parent);
                }
                Explorer.treeView1.EndUpdate();
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
            if (this.PrimaryDock.ActiveDocument is SunfishEditor)
                (this.PrimaryDock.ActiveDocument as SunfishEditor).Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.PrimaryDock.ActiveDocument is SunfishEditor)
            {
                SunfishEditor sunDoc = (this.PrimaryDock.ActiveDocument as SunfishEditor);
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
            UpdateSourceFiles(e.FullPath);
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

        private void fileSystemWatcher1_Renamed(object sender, RenamedEventArgs e)
        {
            RefreshSourceFiles();
        }

        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (project != null)
            {
                string[] s = Directory.GetFiles(project.SourceDirectory);
                string[] d = Directory.GetDirectories(project.SourceDirectory);
                foreach (string ss in s)
                    File.Delete(ss);
                foreach (string ds in d)
                    Directory.Delete(ds, true);
            } DisplaySourceFiles();
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Sunfish.Better_Tag_Structures.Coll.CollisionModel cm = new Sunfish.Better_Tag_Structures.Coll.CollisionModel();
            //cm.Load(File.OpenRead(@"O:\Sunfish 2011\Projects\taco\source\objects\vehicles\warthog\warthog.coll.sf"), 512, -512);
        }
    }
}
