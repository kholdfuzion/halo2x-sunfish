﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;

namespace Sunfish.GUI
{
    public class SunfishEditor : DockContent
    {
        public Tag HaloTag;

        public virtual void Save()
        {
            Globals.Status = String.Format("{0} saved...", this.Text);
            HaloTag.Save();
        }

        public virtual void SaveAs(string filename)
        {
            HaloTag.Save(filename);
            this.Text = Path.ChangeExtension(Sunfish.Tag.Path.GetTagName(HaloTag.Filename), Sunfish.Tag.Path.GetTagType(HaloTag.Filename));
            this.Tag = filename;
        }

        public SunfishEditor()
        {
            base.DockAreas = DockAreas.Document;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SunfishEditor
            // 
            this.ClientSize = new System.Drawing.Size(340, 342);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SunfishEditor";
            this.ResumeLayout(false);

        }
    }
}
