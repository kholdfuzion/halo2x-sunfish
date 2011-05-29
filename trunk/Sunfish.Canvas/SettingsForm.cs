using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sunfish.Canvas
{
    public partial class SettingsForm : Form
    {
        public Settings Settings { get; set; }
        public SettingsForm(Settings settings)
        {
            Settings = settings;
            InitializeComponent();
            propertyGrid1.SelectedObject = Settings;
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            Settings = (Settings)propertyGrid1.SelectedObject;
        }
    }
}
