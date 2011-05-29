namespace WindowsFormsApplication1
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.txbFilename = new System.Windows.Forms.TextBox();
            this.cmdBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txbTagInfo = new System.Windows.Forms.TextBox();
            this.tvTagBlock = new System.Windows.Forms.TreeView();
            this.label3 = new System.Windows.Forms.Label();
            this.cmdLoadTagBlock = new System.Windows.Forms.Button();
            this.reflexiveMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.chunkMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.label4 = new System.Windows.Forms.Label();
            this.cmdSave = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cmdSaveTagBlock = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.reflexiveMenu.SuspendLayout();
            this.chunkMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // txbFilename
            // 
            this.txbFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbFilename.Location = new System.Drawing.Point(12, 29);
            this.txbFilename.Name = "txbFilename";
            this.txbFilename.Size = new System.Drawing.Size(362, 20);
            this.txbFilename.TabIndex = 0;
            // 
            // cmdBrowse
            // 
            this.cmdBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowse.Location = new System.Drawing.Point(380, 27);
            this.cmdBrowse.Name = "cmdBrowse";
            this.cmdBrowse.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowse.TabIndex = 1;
            this.cmdBrowse.Text = "Browse...";
            this.cmdBrowse.UseVisualStyleBackColor = true;
            this.cmdBrowse.Click += new System.EventHandler(this.cmdBrowse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "1. Load a Tag here";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "2. Display Tag Information here";
            // 
            // txbTagInfo
            // 
            this.txbTagInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbTagInfo.Location = new System.Drawing.Point(15, 69);
            this.txbTagInfo.Multiline = true;
            this.txbTagInfo.Name = "txbTagInfo";
            this.txbTagInfo.Size = new System.Drawing.Size(441, 90);
            this.txbTagInfo.TabIndex = 4;
            // 
            // tvTagBlock
            // 
            this.tvTagBlock.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvTagBlock.Location = new System.Drawing.Point(12, 216);
            this.tvTagBlock.Name = "tvTagBlock";
            this.tvTagBlock.Size = new System.Drawing.Size(443, 130);
            this.tvTagBlock.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "3. Load Tag into a TagBlock";
            // 
            // cmdLoadTagBlock
            // 
            this.cmdLoadTagBlock.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdLoadTagBlock.Location = new System.Drawing.Point(215, 165);
            this.cmdLoadTagBlock.Name = "cmdLoadTagBlock";
            this.cmdLoadTagBlock.Size = new System.Drawing.Size(240, 23);
            this.cmdLoadTagBlock.TabIndex = 7;
            this.cmdLoadTagBlock.Text = "Load a TagBlock from a Tag";
            this.cmdLoadTagBlock.UseVisualStyleBackColor = true;
            this.cmdLoadTagBlock.Click += new System.EventHandler(this.cmdLoadTagBlock_Click);
            // 
            // reflexiveMenu
            // 
            this.reflexiveMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.reflexiveMenu.Name = "reflexiveMenu";
            this.reflexiveMenu.ShowImageMargin = false;
            this.reflexiveMenu.Size = new System.Drawing.Size(72, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(71, 22);
            this.toolStripMenuItem1.Text = "Add";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // chunkMenu
            // 
            this.chunkMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3});
            this.chunkMenu.Name = "contextMenuStrip1";
            this.chunkMenu.Size = new System.Drawing.Size(108, 26);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(107, 22);
            this.toolStripMenuItem3.Text = "Delete";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 200);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(183, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "4. Do some basic chunkcloning here:";
            // 
            // cmdSave
            // 
            this.cmdSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSave.Location = new System.Drawing.Point(215, 400);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(240, 23);
            this.cmdSave.TabIndex = 12;
            this.cmdSave.Text = "Save As...";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSaveTag_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 358);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(154, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "5. Save TagBlock back to Tag";
            // 
            // cmdSaveTagBlock
            // 
            this.cmdSaveTagBlock.Location = new System.Drawing.Point(215, 353);
            this.cmdSaveTagBlock.Name = "cmdSaveTagBlock";
            this.cmdSaveTagBlock.Size = new System.Drawing.Size(240, 23);
            this.cmdSaveTagBlock.TabIndex = 14;
            this.cmdSaveTagBlock.Text = "Save TagBlock";
            this.cmdSaveTagBlock.UseVisualStyleBackColor = true;
            this.cmdSaveTagBlock.Click += new System.EventHandler(this.cmdSaveTagBlock_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 405);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "6. Save Tag here";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 438);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmdSaveTagBlock);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.cmdLoadTagBlock);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txbTagInfo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tvTagBlock);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdBrowse);
            this.Controls.Add(this.txbFilename);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Sunfish Example 1";
            this.reflexiveMenu.ResumeLayout(false);
            this.chunkMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txbFilename;
        private System.Windows.Forms.Button cmdBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txbTagInfo;
        private System.Windows.Forms.TreeView tvTagBlock;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button cmdLoadTagBlock;
        private System.Windows.Forms.ContextMenuStrip reflexiveMenu;
        private System.Windows.Forms.ContextMenuStrip chunkMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button cmdSaveTagBlock;
        private System.Windows.Forms.Label label6;
    }
}

