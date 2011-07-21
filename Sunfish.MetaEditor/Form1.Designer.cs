namespace Sunfish.MetaEditor
{
    partial class Form1
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
            this.reflexive1 = new Sunfish.MetaEditor.Reflexive();
            this.SuspendLayout();
            // 
            // reflexive1
            // 
            this.reflexive1.Location = new System.Drawing.Point(147, 140);
            this.reflexive1.Name = "reflexive1";
            this.reflexive1.ReflexiveName = "reflexive1";
            this.reflexive1.Size = new System.Drawing.Size(354, 181);
            this.reflexive1.TabIndex = 0;
            this.reflexive1.Text = "reflexive1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 481);
            this.Controls.Add(this.reflexive1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Reflexive reflexive1;
    }
}

