using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using System.Windows.Forms;

namespace Sunfish.Canvas
{
    public partial class XNABitmapViewer : UserControl
    {
        BitmapEditorGame Game;

        public GraphicsDevice Device { get { return Game.GraphicsDevice; } }

        public bool Opacity { get { return Game.Opacity; } set { Game.Opacity = value; } }

        public XNAViewer.XNAViewer Viewer
        {
            get { return this.xnaViewer; }
            set { this.xnaViewer = value; }
        }

        public void LoadTexture2D(Texture2D texture, int height, int width)
        {
            Game.Textures.Clear();
            Game.Textures.Add(texture);
            Viewer.Width = texture.Width;
            Viewer.Height = texture.Height;
            Game.SourceRectangle = new Rectangle(0, 0, width, height);
            Game.Mode = BitmapEditorGame.PreviewMode.Texture2D;
        }

        public void LoadCubemap(Texture2D[] faces)
        {
            Game.Textures.Clear();
            Game.Textures.AddRange(faces);
            Viewer.Width = faces[0].Width * 4;
            Viewer.Height = faces[0].Height * 3;
            Game.Mode = BitmapEditorGame.PreviewMode.Cubemap;
        }

        public XNABitmapViewer()
        {
            InitializeComponent();
            Game = new BitmapEditorGame();
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            //if (xnaViewer.Height < panel.Height)
            //    xnaViewer.Top = (panel.Height - xnaViewer.Height) / 2;
            //else xnaViewer.Top = 0;
            //if (xnaViewer.Width < panel.Width)
            //    xnaViewer.Left = (panel.Width - xnaViewer.Width) / 2;
            //else xnaViewer.Left = 0;
            Game.ViewportHeight = xnaViewer.Height;
            Game.ViewportWidth = xnaViewer.Width;
        }

        public void RunGame()
        {
            Viewer.RunGame(Game);
        }

        public void ClearTextures()
        {
        }

        private void xnaViewer_Resize(object sender, EventArgs e)
        {
            panel1_Resize(sender, e);
        }
    }
}
