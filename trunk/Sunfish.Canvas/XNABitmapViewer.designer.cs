namespace Sunfish.Canvas
{
    partial class XNABitmapViewer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel = new System.Windows.Forms.Panel();
            this.xnaViewer = new XNAViewer.XNAViewer();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.AutoScroll = true;
            this.panel.BackColor = System.Drawing.Color.Silver;
            this.panel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel.Controls.Add(this.xnaViewer);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(250, 250);
            this.panel.TabIndex = 1;
            this.panel.Resize += new System.EventHandler(this.panel1_Resize);
            // 
            // xnaViewer
            // 
            this.xnaViewer.BackColor = System.Drawing.Color.Silver;
            this.xnaViewer.Location = new System.Drawing.Point(25, 25);
            this.xnaViewer.Margin = new System.Windows.Forms.Padding(0);
            this.xnaViewer.Name = "xnaViewer";
            this.xnaViewer.Size = new System.Drawing.Size(200, 200);
            this.xnaViewer.TabIndex = 1;
            this.xnaViewer.Resize += new System.EventHandler(this.xnaViewer_Resize);
            // 
            // XNABitmapViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel);
            this.Name = "XNABitmapViewer";
            this.Size = new System.Drawing.Size(250, 250);
            this.panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private XNAViewer.XNAViewer xnaViewer;
    }
}
