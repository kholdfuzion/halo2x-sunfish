using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace Visual_Simplicity.Raw_Editors.Games
{
    public class BitmapEditorGame : EditorGame
    {
        SpriteBatch spiteBatch;
        Texture2D backgroundTexture;
        Texture2D texture;
        BasicEffect effect;

        public PreviewMode Mode { get; set; }
        public Texture2D Texture
        {
            get { return texture; }
            set
            {
                texture = value;
                ViewportWidth = value.Width;
                ViewportHeight = value.Height;
            }
        }
        public Color BackgroundColor { get; set; }
        public int ViewportWidth { get { return width; } set { if (value > 0) width = value; } }
        public int ViewportHeight { get { return height; } set { if (value > 0) height = value; } }
        public bool Opacity { get; set; }

        int width = 1;
        int height = 1;

        public enum PreviewMode
        {
            None,
            Plane,
            Cube,
            Sphere,
        }

        public BitmapEditorGame()
        { BackgroundColor = Color.Silver; }

        protected override void Initialize()
        {
            Mode = PreviewMode.Plane;
            spiteBatch = new SpriteBatch(GraphicsDevice);
            LoadBackgroundTexture();
            base.Initialize();
        }

        private void LoadBackgroundTexture()
        {
            FileStream File = new FileStream(System.Windows.Forms.Application.StartupPath + "\\transparancy.png", FileMode.Open, FileAccess.Read, FileShare.None);
            backgroundTexture = Texture2D.FromFile(GraphicsDevice, File);
            File.Close();
        }

        private void InitializeBasicEffect()
        {
            effect = new BasicEffect(GraphicsDevice, null);
            effect.Alpha = 1.0f;
            effect.DiffuseColor = new Vector3(1.0f, 0.0f, 1.0f);
            effect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
            effect.SpecularPower = 5.0f;
            effect.AmbientLightColor = new Vector3(0.75f, 0.75f, 0.75f);

            effect.DirectionalLight0.Enabled = true;
            effect.DirectionalLight0.DiffuseColor = Vector3.One;
            effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1.0f, -1.0f, -1.0f));
            effect.DirectionalLight0.SpecularColor = Vector3.One;

            effect.DirectionalLight1.Enabled = true;
            effect.DirectionalLight1.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f);
            effect.DirectionalLight1.Direction = Vector3.Normalize(new Vector3(-1.0f, -1.0f, 1.0f));
            effect.DirectionalLight1.SpecularColor = new Vector3(0.5f, 0.5f, 0.5f);

            effect.LightingEnabled = false;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GraphicsDevice.Viewport.Width != ViewportWidth || GraphicsDevice.Viewport.Height != ViewportHeight)
            {
                PresentationParameters newParams = GraphicsDevice.PresentationParameters;
                newParams.BackBufferWidth = ViewportWidth;
                newParams.BackBufferHeight = ViewportHeight;
                GraphicsDevice.Reset(newParams);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackgroundColor);
            switch (Mode)
            {
                case PreviewMode.Plane: 
                    if (Texture != null)
                    {
                        lock (Texture)
                        {
                            SpriteBlendMode blendMode = Opacity == true ? SpriteBlendMode.AlphaBlend : SpriteBlendMode.None;
                            spiteBatch.Begin(blendMode, SpriteSortMode.Immediate, SaveStateMode.None);

                            GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
                            GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
                            spiteBatch.Draw(backgroundTexture, new Rectangle(0, 0, ViewportWidth, ViewportHeight), new Rectangle(0, 0, ViewportWidth, ViewportHeight), Color.White);
                            spiteBatch.Draw(Texture, Vector2.Zero, Color.White);

                            spiteBatch.End();
                        }
                    }
                    break;
                case PreviewMode.Cube:
                    break;
                case PreviewMode.Sphere:
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
