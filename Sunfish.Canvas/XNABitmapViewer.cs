using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Visual_Simplicity.Raw_Editors.Games;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;

namespace Sunfish.Canvas
{
    public partial class XNABitmapViewer : UserControl
    {
        BitmapEditorGame Game;

        public bool Opacity { get { return Game.Opacity; } set { Game.Opacity = value; } }

        public XNAViewer.XNAViewer Viewer
        {
            get { return this.xnaViewer; }
            set { this.xnaViewer = value; }
        }

        public void LoadTexture(Stream stream)
        {
            Game.Mode = BitmapEditorGame.PreviewMode.None;
            stream.Position = 0;
            Game.Texture = Texture2D.FromFile(Game.GraphicsDevice, stream);
            Viewer.Height = Game.Texture.Height;
            Viewer.Width = Game.Texture.Width;
            panel1_Resize(this, EventArgs.Empty);
            Game.Mode = BitmapEditorGame.PreviewMode.Plane;
        }

        public XNABitmapViewer()
        {
            InitializeComponent();
            Game = new BitmapEditorGame();
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            if (xnaViewer.Height < panel.Height)
                xnaViewer.Top = (panel.Height - xnaViewer.Height) / 2;
            else xnaViewer.Top = 0;
            if (xnaViewer.Width < panel.Width)
                xnaViewer.Left = (panel.Width - xnaViewer.Width) / 2;
            else xnaViewer.Left = 0;
        }

        public void RunGame()
        {
            Viewer.RunGame(Game);
        }

        internal void UnloadTexture()
        {
            Game.Texture = new Texture2D(Game.GraphicsDevice, 1, 1);
        }

        private void xnaViewer_Resize(object sender, EventArgs e)
        {
            panel1_Resize(sender, e);
        }
    }
}
