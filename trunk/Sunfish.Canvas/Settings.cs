using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms.Design;
using System.Drawing.Design;

namespace Sunfish.Canvas
{

    [Serializable]
    public struct Settings
    {
        public const string Filename = "settings.bin";

        [DescriptionAttribute("Default directory used to load bitmap tags on startup")]
        [CategoryAttribute("Settings")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string TagsDirectory { get { return tagsDirectory; } set { tagsDirectory = value; } }

        [DescriptionAttribute("Defines whether to render bitmap alpha-channel or not.")]
        [CategoryAttribute("Settings")]
        public bool Opacity { get { return opacity; } set { opacity = value; } }

        string tagsDirectory;
        bool opacity;

        public void SaveSettings()
        {
            MemoryStream ms = new MemoryStream(Marshal.SizeOf(this));
            BinaryFormatter bitFormatter = new BinaryFormatter();
            bitFormatter.Serialize(ms, this);
            FileStream fs = new FileStream(Filename, FileMode.Create);
            fs.Write(ms.ToArray(), 0, (int)ms.Length);
            fs.Close();
        }

        public void  LoadSettings()
        {
            FileStream fs = new FileStream(Filename, FileMode.Open);
            BinaryFormatter binFormatter = new BinaryFormatter();
            this = (Settings)(binFormatter.Deserialize(fs));
        }
    }
}
