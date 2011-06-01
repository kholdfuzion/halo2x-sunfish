using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;
using Sunfish.Canvas;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Windows.Forms;

namespace Sunfish.GUI
{
    public partial class ModelEditor : DockContent
    {
        Game1 game;
        Sunfish.Mode.Model m;

        public ModelEditor()
        {
            InitializeComponent();
        }

        internal void LoadTag(Tag tag)
        {
            m = new Sunfish.Mode.Model(tag);
            //m.Sections[0].Mesh.ExportWavefrontObject(m.Shaders);
            //WavefrontObject wfo = Wavefront.ParseWavefrontOBJFile(@"O:\import_test.obj");
            //m.Sections[0].Mesh.ImportWavefrontObject(wfo, m.BoundingBoxes[0]);
            //m.Sections[0].Mesh.ExportWavefrontObject(m.Shaders);
            //Tag newTag = m.CreateTag();
            //Model m2 = new Model(newTag);
            //newTag.Save(tag.Filename + "copy");
            foreach (Mode.Region r in m.Regions)
                listBox1.Items.Add(r.name);
            game = new Game1(m, xnaViewer1.Height, xnaViewer1.Width);
            xnaViewer1.RunGame(game);
        }

        private void ModelEditor_Resize(object sender, EventArgs e)
        {
            if (game != null)
            {
                game.ViewportHeight = this.xnaViewer1.Height;
                game.ViewportWidth = this.xnaViewer1.Width;
            }
        }

        private void ModelEditor_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            UpdateCamera(e);
        }

        private void ModelEditor_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            UpdateCamera(e);
        }

        private void UpdateCamera(System.Windows.Forms.KeyEventArgs e)
        {
            game.cameraControls.Ctrl = e.Control;
            game.cameraControls.Alt = e.Alt;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            game.SelectedIndex = listBox1.SelectedIndex;            
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            game.LevelOfDetail = trackBar1.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (game.SelectedIndex == -1) { return; }
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Mode.WavefrontObject wfo = Sunfish.Mode.Wavefront.ParseWavefrontOBJFile(ofd.FileName);
                
                m.Sections[m.Regions[game.SelectedIndex].permutation[0].indices[game.LevelOfDetail]].Mesh.ImportWavefrontObject(wfo, m.BoundingBoxes[0]);
                xnaViewer1.PauseGame();
                game.UpdateModel(m);
                xnaViewer1.ResumeGame();
            }
        }
    }

    public class CameraControls
    {
        public bool Ctrl = false;
        public bool Alt = false;
        MouseState mouseState;
        MouseState lastMouseState;

        public void Update(Camera camera)
        {
            Vector3 Translation = Vector3.Zero;
            mouseState = Mouse.GetState(); 
            if (mouseState.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Ctrl && Alt)
            {
                float Zoom = mouseState.Y - lastMouseState.Y;
                Zoom *= 0.2f;
                camera.ZoomToTarget(Zoom);
            }
            else if (mouseState.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Ctrl)
            {
                float hRot = (float)(lastMouseState.X - mouseState.X) / 180f;
                float vRot = (float)(lastMouseState.Y - mouseState.Y) / 180f;
                Vector3 rots = new Vector3(hRot, vRot, 0);
                if (vRot != 0 || hRot != 0)
                    camera.Orbit(rots);
            }
            else if (mouseState.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Alt)
            {
                Vector2 move = new Vector2(mouseState.X - lastMouseState.X, mouseState.Y - lastMouseState.Y);
                camera.MoveCamera(move);
            }
            lastMouseState = mouseState;
        }

        public void DrawInformation(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.DrawString(font, "Mouse Wheel Value: " + mouseState.ScrollWheelValue.ToString(), new Vector2(10, 10), Color.Red);
            spriteBatch.DrawString(font, "X: " + mouseState.X.ToString(), new Vector2(10, 25), Color.Red);
            spriteBatch.DrawString(font, "Y: " + mouseState.Y.ToString(), new Vector2(10, 40), Color.Red);
        }
    }

    public class Camera
    {
        Vector3 camUp;
        Vector3 camRight;
        Vector3 camForward;
        Vector3 camRotations;
        Vector3 camPosition;
        Matrix camTransformations;
        float camZoom;
        public Matrix viewMatrix;
        public Matrix projectionMatrix;
        GraphicsDevice device;

        public Camera(GraphicsDevice graphicsDevice)
        {
            this.device = graphicsDevice;
            this.camZoom = 5f;
            this.camForward = Vector3.Forward;
            this.camUp = Vector3.Up;
            this.camRight = Vector3.Right;
            this.camRotations = Vector3.Zero;
            this.camTransformations = Matrix.Identity;
            CalculateFrame();
            Update();
        }

        public void LookAt(Vector3 target)
        {
            //camTarget = target;
            // Vector3 orig = camTarget - camPosition;
            camTransformations *= Matrix.CreateTranslation(target);
            CalculateFrame();
        }

        public void CalculateFrame()
        {

            //Quaternion q = Quaternion.CreateFromYawPitchRoll(camRotations.X, camRotations.Y, camRotations.Z);
            Quaternion q;
            Vector3 s, t;
            camTransformations.Decompose(out s, out q, out t);
            this.camForward = Vector3.Transform(Vector3.UnitY, q);
            this.camForward.Normalize();
            this.camUp = Vector3.Transform(Vector3.UnitZ, q);
            camUp.Normalize();
            this.camRight = Vector3.Transform(Vector3.UnitX, q);
            camRight.Normalize();
            this.camPosition = Vector3.Multiply(Vector3.Multiply(camForward, -0.1f), camZoom) + t;
        }

        public void Strafe(Vector3 translation)
        {
            /*
             * up - y
             * right - x
             * target - z
             */

            CalculateFrame();
            Vector3 x = Vector3.Multiply(camRight, translation.X);
            Vector3 y = Vector3.Multiply(camUp, translation.Y);
            //camTarget = Vector3.Add(camTarget, x);
            camPosition = Vector3.Add(camPosition, x);
            //camTarget = Vector3.Add(camTarget, y);
            camPosition = Vector3.Add(camPosition, y);
        }

        public void Orbit(Vector3 rots)
        {
            CalculateFrame();
            Matrix quat1 = Matrix.CreateFromAxisAngle(Vector3.UnitZ, rots.X);//Matrix.CreateFromYawPitchRoll(rots.X, 0, 0);
            Vector3 perpendicularAxis = camRight;
            Matrix quat2 = Matrix.CreateFromAxisAngle(perpendicularAxis, rots.Y);
            quat1 = quat2 * quat1;
            camTransformations *= quat1;
            Vector3 pos = Vector3.Transform(camPosition, quat1);
            camPosition = pos;
            CalculateFrame();
        }

        public void MoveCamera(Vector2 translation)
        {
            Update();
            CalculateFrame(); 
            Quaternion q;
            Vector3 s, t;
            camTransformations.Decompose(out s, out q, out t);
            float distanceFromCameraTarget = (camPosition - t).Length();
            float farClippingRange = 300f;
            float percentageWorld = distanceFromCameraTarget / farClippingRange;

            Vector3 PercentageScreen = new Vector3(translation.X, translation.Y, 0);
            Vector3 Right = device.Viewport.Unproject(new Vector3(0, 250, 1), projectionMatrix, viewMatrix, Matrix.Identity);
            Vector3 Left = device.Viewport.Unproject(new Vector3(500, 250, 1), projectionMatrix, viewMatrix, Matrix.Identity);
            Vector3 Top = device.Viewport.Unproject(new Vector3(250, 0, 1), projectionMatrix, viewMatrix, Matrix.Identity);
            Vector3 Bottom = device.Viewport.Unproject(new Vector3(250, 500, 1), projectionMatrix, viewMatrix, Matrix.Identity);

            Vector3 HorizontalTranslation = Vector3.Multiply((Left - Right), percentageWorld);
            Vector3 VerticalTranslation = Vector3.Multiply((Bottom - Top), percentageWorld);
            HorizontalTranslation *= -(PercentageScreen.X / 500);
            VerticalTranslation *= -(PercentageScreen.Y / 500);
            camTransformations *= Matrix.CreateTranslation(HorizontalTranslation);
            camTransformations *= Matrix.CreateTranslation(VerticalTranslation);
        }

        public void ZoomToTarget(float distanceToZoom)
        {
            CalculateFrame();
            camZoom += distanceToZoom;
            if (camZoom < 0.01f) camZoom = 0.01f;
            CalculateFrame();
        }

        public void Update()
        {
            viewMatrix = Matrix.CreateLookAt(camPosition, camPosition + camForward, camUp);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 1.0f, 300.0f);
        }

        public void DrawInformation(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.DrawString(font, "Camera position: " + camPosition.ToString(), new Vector2(10, 115), Color.Red);
            spriteBatch.DrawString(font, "Camera Up: " + camUp.ToString(), new Vector2(10, 130), Color.Red);
        }
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public int LevelOfDetail = 5;
        public int SelectedIndex = -1;
        SpriteFont font;
        public CameraControls cameraControls;
        public Camera camera;
        public Sunfish.Mode.Model Model;
        BasicEffect effect;
        GraphicsDevice device;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        VertexDeclaration myVertexDeclaration;

        int width, height;
        public int ViewportWidth { get { return width; } set { if (value > 0) width = value; } }
        public int ViewportHeight { get { return height; } set { if (value > 0) height = value; } }

        public Game1(Sunfish.Mode.Model model, int height, int width)
        {
            this.width = width;
            this.height = height;
            this.Model = model;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            device = graphics.GraphicsDevice;
            camera = new Camera(device);
            Vector3 mid = new Vector3((Model.BoundingBoxes[0].X.Min + Model.BoundingBoxes[0].X.Max) / 2, (Model.BoundingBoxes[0].Y.Min + Model.BoundingBoxes[0].Y.Max) / 2, (Model.BoundingBoxes[0].Z.Min + Model.BoundingBoxes[0].Z.Max) / 2);
            camera.LookAt(new Vector3(mid.X, mid.Y, mid.Z));
            camera.ZoomToTarget(20f);
            cameraControls = new CameraControls();
            font = Content.Load<SpriteFont>("debug");
            myVertexDeclaration = new VertexDeclaration(device, VertexPositionColor.VertexElements);
            IntializeEffect();
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        void IntializeEffect()
        {
            effect = new BasicEffect(GraphicsDevice, null);
            effect.EnableDefaultLighting();
            effect.DiffuseColor = Color.Gray.ToVector3();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GraphicsDevice.Viewport.Width != ViewportWidth || GraphicsDevice.Viewport.Height != ViewportHeight)
            {
                PresentationParameters newParams = GraphicsDevice.PresentationParameters;
                newParams.BackBufferWidth = ViewportWidth;
                newParams.BackBufferHeight = ViewportHeight;
                GraphicsDevice.Reset(newParams);
            }
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                this.Exit();
            cameraControls.Update(camera);
            camera.Update();
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(125, 125, 125));
            GraphicsDevice.RenderState.CullMode = CullMode.CullClockwiseFace;
            effect.CurrentTechnique = effect.Techniques[0];
            effect.View = camera.viewMatrix;
            effect.Projection = camera.projectionMatrix;
            effect.World = Matrix.Identity;


            if (Model != null)
            {
                for (int i = 0; i < Model.Regions.Length; i++)
                {
                    Mode.Mesh mesh = Model.Sections[Model.Regions[i].permutation[0].indices[LevelOfDetail]].Mesh;
                    if (i == SelectedIndex) effect.DiffuseColor = new Vector3(1, 0, 0);
                    else effect.DiffuseColor = Color.Gray.ToVector3();

                    effect.Begin();

                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Begin();

                        device.VertexDeclaration = new VertexDeclaration(graphics.GraphicsDevice, Mode.Vertex.VertexElements);
                        foreach (Mode.Group g in mesh.Groups)
                            device.DrawUserIndexedPrimitives<Sunfish.Mode.Vertex>(PrimitiveType.TriangleStrip, mesh.Vertexlist, 0, mesh.Vertices.Length, mesh.Indices, g.IndiceStart, g.IndiceCount - 2);

                        pass.End();
                    }

                    effect.End();
                }
            }

            //spriteBatch.Begin();
            //cameraControls.DrawInformation(spriteBatch, font);
            //camera.DrawInformation(spriteBatch, font);
            //spriteBatch.End();

            base.Draw(gameTime);
        }

        internal void UpdateModel(Sunfish.Mode.Model m)
        {
            Model = m;
        }
    }
}
