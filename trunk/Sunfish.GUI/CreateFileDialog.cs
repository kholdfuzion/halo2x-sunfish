using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Sunfish.GUI
{
    public partial class CreateFileDialog : Form
    {
        public CreateFileDialog(string title)
        {
            InitializeComponent();
            this.Text = title;
        }

        public string FileName { get { return this.textBox1.Text; } }

        private void cmdAccept_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory(Path.Combine(Properties.Settings.Default.ProjectsDirectory, FileName));
        }
    }
}
