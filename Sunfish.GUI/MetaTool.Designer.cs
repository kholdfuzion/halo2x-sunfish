namespace Sunfish.GUI
{
    partial class MetaTool
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.metaGridView1 = new Sunfish.MetaEditor.MetaGridView();
            this.SuspendLayout();
            // 
            // metaGridView1
            // 
            this.metaGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metaGridView1.Location = new System.Drawing.Point(0, 0);
            this.metaGridView1.Name = "metaGridView1";
            this.metaGridView1.Size = new System.Drawing.Size(292, 273);
            this.metaGridView1.TabIndex = 0;
            // 
            // MetaTool
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.metaGridView1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MetaTool";
            this.ResumeLayout(false);

        }

        #endregion

        private Sunfish.MetaEditor.MetaGridView metaGridView1;

    }
}