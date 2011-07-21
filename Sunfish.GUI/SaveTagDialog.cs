using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sunfish.GUI
{
    public class SaveTagDialog
    {
        SaveFileDialog dialog;

        public SaveTagDialog()
        {
            dialog.Filter = "Sunfish Tag File (*.sf) | *.sf";
            dialog.SupportMultiDottedExtensions = true;
            dialog.AddExtension = true;
        }
    }
}
