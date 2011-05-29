using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace Sunfish.Canvas
{
    public class BitmapEditorGame : EditorGame
    {
        SpriteBatch spiteBatch;
        Texture2D backgroundTexture;
        List<Texture2D> textures = new List<Texture2D>();
        BasicEffect effect;

        public PreviewMode Mode { get; set; }
        public List<Texture2D> Textures
        {
            get { return textures; }
            set
            {
                textures = value;
            }
        }
        public Color BackgroundColor { get; set; }
        public int ViewportWidth { get { return width; } set { if (value > 0) width = value; } }
        public int ViewportHeight { get { return height; } set { if (value > 0) height = value; } }
        public bool Opacity { get; set; }

        int width = 1;
        int height = 1;

        public Rectangle SourceRectangle { get; set; }

        public enum PreviewMode
        {
            None,
            Texture2D,
            Cubemap,
        }

        public BitmapEditorGame()
        { BackgroundColor = Color.Silver; }

        protected override void Initialize()
        {
            Mode = PreviewMode.None;
            spiteBatch = new SpriteBatch(GraphicsDevice);
            LoadBackgroundTexture();
            base.Initialize();
        }

        private void LoadBackgroundTexture()
        {
            FileStream File = new FileStream(System.Windows.Forms.Application.StartupPath + "\\resources\\transparancy.png", FileMode.Open, FileAccess.Read, FileShare.None);
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
            SpriteBlendMode blendMode = Opacity == true ? SpriteBlendMode.AlphaBlend : SpriteBlendMode.None;
            switch (Mode)
            {
                case PreviewMode.Texture2D:
                    if (Textures != null)
                    {
                        lock (Textures)
                        {
                            spiteBatch.Begin(blendMode, SpriteSortMode.Immediate, SaveStateMode.None);
                            GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
                            GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
                            spiteBatch.Draw(backgroundTexture, SourceRectangle, SourceRectangle, Color.White);

                            if (Textures.Count > 0)
                                spiteBatch.Draw(Textures[0], Vector2.Zero, SourceRectangle, Color.White);

                            spiteBatch.End();
                        }
                    }
                    break;
                case PreviewMode.Cubemap:
                    if (Textures != null)
                    {
                        lock (Textures)
                        {
                            if (Textures.Count == 6)
                            {
                                int size = textures[0].Width;
                                spiteBatch.Begin(blendMode, SpriteSortMode.Immediate, SaveStateMode.None);
                                GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
                                GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;

                                spiteBatch.Draw(backgroundTexture, new Rectangle(0, 0, size * 4, size * 3), new Rectangle(0, 0, size * 4, size * 3), Color.White);

                                spiteBatch.Draw(Textures[4], new Rectangle(size * 2, 0, size, size), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0);
                                spiteBatch.Draw(Textures[2], new Rectangle(0, size, size, size), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally, 0);
                                spiteBatch.Draw(Textures[1], new Rectangle(size * 1 + (size / 2), size + (size / 2), size, size), null, Color.White, (float)-(Math.PI / 2), new Vector2(size / 2, size / 2), SpriteEffects.None, 0);
                                spiteBatch.Draw(Textures[3], new Rectangle(size * 2, size, size, size), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0);
                                spiteBatch.Draw(Textures[0], new Rectangle(size * 3 + (size / 2), size + (size / 2), size, size), null, Color.White, (float)(Math.PI / 2), new Vector2(size / 2, size / 2), SpriteEffects.None, 0);
                                spiteBatch.Draw(Textures[5], new Rectangle(size * 2, size * 2, size, size), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally, 0);
                            }
                            spiteBatch.End();
                        }
                    }
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
