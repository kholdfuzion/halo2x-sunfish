using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Sunfish.Canvas
{
    public partial class MainForm : Form
    {
        OpenFileDialog openDialog;
        OpenFileDialog openTagDialog;
        SettingsForm settingsForm;

        Settings settings;
        List<Tag> LoadedTags;
        H2BitmapCollection LoadedTagMeta;

        int curTagIndex;
        int curBitmapIndex;

        int CurrentTagIndex
        {
            get { return curTagIndex; }
            set
            {
                curTagIndex = value; 
                if (value != -1)
                    LoadBitmapTag();
            }
        }
        int CurrentBitmapIndex
        {
            get { return curBitmapIndex; }
            set
            {
                curBitmapIndex = value; 
                if (value != -1)
                    LoadBitmapStream();
            }
        }

        public MainForm()
        {
            InitializeComponent();
            LoadedTags = new List<Tag>();
            xnaBitmapViewer2.RunGame();
            curTagIndex = -1;
            curBitmapIndex = -1;

            openDialog = new OpenFileDialog();
            openDialog.Filter = "DirectDrawSurface (*.dds)|*.dds";
            openDialog.Multiselect = false;
            openDialog.Title = "Open DirectDrawSurface File";

            openTagDialog = new OpenFileDialog();
            openTagDialog.Filter = "Halo 2 Tag (*.h2tag)|*.h2tag";
            openTagDialog.Multiselect = false;
            openTagDialog.Title = "Open Halo 2 Tag File";

            settingsForm = new SettingsForm(settings);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openTagDialog.ShowDialog() == DialogResult.OK)
            {
                string ext = Path.GetExtension(openTagDialog.FileName);
                if (ext == ".h2tag")
                {
                    Tag tag = new Tag(openTagDialog.FileName);
                    LoadedTags.Add(tag);
                    tagList.Items.Add(CleanFilename(openTagDialog.FileName, string.Empty));
                    tagList.SelectedIndex = LoadedTags.Count - 1;
                }
                //else if (ext == ".dds")
                //{
                //    FileStream File = new FileStream(ofd.FileName, FileMode.Open);
                //    //xnaBitmapViewer2.LoadTexture2D(File);
                //}
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string Folder = fbd.SelectedPath;
                settings.TagsDirectory = Folder;
                LoadTags(Folder);
            }
            fbd.Dispose();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tagList.SelectedIndex != -1)
                CurrentTagIndex = tagList.SelectedIndex;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            xnaBitmapViewer2.Opacity = toolStripButton1.Checked;
            settings.Opacity = toolStripButton1.Checked;
            settings.SaveSettings();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bitmapExplorer.SelectedIndices.Count > 0 && bitmapExplorer.SelectedIndices[0] != -1)
                CurrentBitmapIndex = bitmapExplorer.SelectedIndices[0];
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (File.Exists(Settings.Filename))
            {
                settings.LoadSettings();
                Setup();
            }
            else
            {
                settings = new Settings() { Opacity = false, TagsDirectory = string.Empty };
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            settings.Opacity = toolStripButton1.Checked;
            settings.SaveSettings();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                FileStream File = new FileStream(openDialog.FileName, FileMode.Open);
                Surface Surface = new Surface(File);
                ImportBitmap(Surface);
                RefreshBitmapInformation();
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settingsForm = new SettingsForm(settings);
            if(settingsForm.ShowDialog() == DialogResult.OK)
            {
                settings = settingsForm.Settings;
                Setup();
            }
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadedTags.Clear();
            tagList.Items.Clear();
            bitmapExplorer.Items.Clear();
            xnaBitmapViewer2.ClearTextures();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LoadedTags.Count > 0)
            {
                LoadedTags.RemoveAt(CurrentTagIndex);
                tagList.Items.RemoveAt(CurrentTagIndex);
                if (CurrentTagIndex < tagList.Items.Count)
                    tagList.SelectedIndex = CurrentTagIndex;
                else tagList.SelectedIndex = CurrentTagIndex - 1;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadedTags[CurrentTagIndex].Save();
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Tag t in LoadedTags)
                t.Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                LoadedTags[CurrentTagIndex].Save(sfd.FileName);
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                FileStream File = new FileStream(openDialog.FileName, FileMode.Open);
                byte[] Buffer = new byte[File.Length];
                File.Read(Buffer, 0, Buffer.Length);
                File.Close();
                DirectDrawSurfaceStream Surface = new DirectDrawSurfaceStream(Buffer);
                AddBitmap(Surface);
                RefreshBitmapInformation();
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            RemoveBitmap();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Halo 2 bitmap injector & extractor\n\rVersion: 1.1", "Sunfish Canvas", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
    }
}
