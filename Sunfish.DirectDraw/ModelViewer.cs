using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Sunfish.DirectDraw
{
    public partial class ModelViewer : System.Windows.Forms.UserControl
    {
        Game1 game = new Game1();
        public ModelViewer()
        {
            InitializeComponent();
            Viewer = new XNAViewer.XNAViewer();
        }

        public XNAViewer.XNAViewer Viewer
        {
            get;
            set;
        }

        public void Run()
        {
            Viewer.RunGame(game);
        }
    }

    public class Cube
    {
        public short[] indices;
        public VertexPositionColor[] vertices;

        public Cube()
        {
            vertices = new VertexPositionColor[8];
            vertices[0] = new VertexPositionColor(new Vector3(-10f, 0f, 10f), Color.White);
            vertices[1] = new VertexPositionColor(new Vector3(-10f, 0f, -10f), Color.Red);
            vertices[2] = new VertexPositionColor(new Vector3(10f, 0f, -10f), Color.Yellow);
            vertices[3] = new VertexPositionColor(new Vector3(10f, 0f, 10f), Color.Green);
            vertices[4] = new VertexPositionColor(new Vector3(-10f, 20f, 10f), Color.Blue);
            vertices[5] = new VertexPositionColor(new Vector3(10f, 20f, 10f), Color.Purple);
            vertices[6] = new VertexPositionColor(new Vector3(10f, 20f, -10f), Color.Orange);
            vertices[7] = new VertexPositionColor(new Vector3(-10f, 20f, -10f), Color.Violet);
            indices = new short[36] 
            { 
                /*
f 1 2 3
f 3 4 1 
f 5 6 7 
f 7 8 5 
f 1 4 6 
f 6 5 1 
f 4 3 7 
f 7 6 4
f 3 2 8 
f 8 7 3 
f 2 1 5 
f 5 8 2 
                 */
                0, 1, 2, 
                2, 3, 0, 
                4, 5, 6, 
                6, 7, 4, 
                0, 3, 5, 
                5, 4, 0, 
                3, 2, 6, 
                6, 5, 3, 
                2, 1, 7, 
                7, 6, 2, 
                1, 0, 4, 
                4, 7, 1 
            };
        }
    }
    public class CameraControls
    {
        public bool KeyboardControlled = false;
        public bool MouseControlled = true;
        KeyboardState keyState;
        MouseState mouseState;
        int lastScrollWheelValue;
        Point lastMouseLocation;

        public void Update(Camera camera)
        {
            Vector3 Translation = Vector3.Zero;
            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            float zoom = 0f;
            if (KeyboardControlled)
            {
                if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
                {
                    Translation.X -= 0.2f;
                }
                if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
                {
                    Translation.X += 0.2f;
                }
                if (keyState.IsKeyDown(Keys.PageUp) || keyState.IsKeyDown(Keys.X))
                {
                    Translation.Y += 0.2f;
                }
                if (keyState.IsKeyDown(Keys.PageDown) || keyState.IsKeyDown(Keys.Z))
                {
                    Translation.Y -= 0.2f;
                }
                if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
                {
                    Translation.Z -= 0.2f;
                }
                if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
                {
                    Translation.Z += 0.2f;
                }
            }
            if (MouseControlled)
            {

                if (mouseState.MiddleButton == ButtonState.Pressed && (keyState.IsKeyDown(Keys.RightControl) || keyState.IsKeyDown(Keys.LeftControl)))
                {

                    float hRot = (float)(lastMouseLocation.X - mouseState.X) / 180f;
                    float vRot = (float)(lastMouseLocation.Y - mouseState.Y) / 180f;
                    Vector3 rots = new Vector3(hRot, vRot, 0);
                    if (vRot != 0 || hRot != 0)
                        camera.Orbit(rots);

                }
                else if (mouseState.MiddleButton == ButtonState.Pressed)
                {
                    Vector3 Start = new Vector3(lastMouseLocation.X, lastMouseLocation.Y, 0);
                    Vector3 End = new Vector3(mouseState.X, mouseState.Y, 0);
                    if (Start != Vector3.Zero || End != Vector3.Zero)
                        camera.MoveCamera(Start, End);
                }

                if (keyState.IsKeyDown(Keys.Add))
                {
                    zoom += 1;
                }

                if (keyState.IsKeyDown(Keys.Subtract))
                {
                    zoom -= 1;
                }

                if (keyState.IsKeyDown(Keys.Z))
                {
                    camera.LookAt(new Vector3(0, 10, 0));
                }

                if (lastScrollWheelValue != mouseState.ScrollWheelValue)
                {
                    zoom = (float)(mouseState.ScrollWheelValue - lastScrollWheelValue) / 10f;
                    lastScrollWheelValue = mouseState.ScrollWheelValue;
                }
            }
            camera.ZoomToTarget(zoom);
            camera.Strafe(Translation); lastMouseLocation = new Point(mouseState.X, mouseState.Y);
        }

        public void DrawInformation(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.DrawString(font, "Mouse Wheel Value: " + mouseState.ScrollWheelValue.ToString(), new Vector2(10, 10), Color.Red);
            spriteBatch.DrawString(font, "X: " + mouseState.X.ToString(), new Vector2(10, 25), Color.Red);
            spriteBatch.DrawString(font, "Y: " + mouseState.Y.ToString(), new Vector2(10, 40), Color.Red);
            spriteBatch.DrawString(font, "X1: " + lastMouseLocation.X.ToString(), new Vector2(10, 55), Color.Red);
            spriteBatch.DrawString(font, "Y2: " + lastMouseLocation.Y.ToString(), new Vector2(10, 70), Color.Red);
        }
    }

    public class Camera
    {
        Vector3 camPosition;
        Vector3 camTarget;
        Vector3 camUp;
        Vector3 camRight;
        Vector3 camForward;
        Vector3 camRotations;
        public Matrix viewMatrix;
        public Matrix projectionMatrix;
        GraphicsDevice device;

        public Camera(GraphicsDevice graphicsDevice)
        {
            this.device = graphicsDevice;
            this.camPosition = new Vector3(0, 0, 50);
            this.camTarget = new Vector3(0, 0, 0);
            this.camForward = Vector3.Forward;
            this.camUp = Vector3.Up;
            this.camRight = Vector3.Right;
            this.camRotations = Vector3.Zero;
            CalculateFrame();
            Update();
        }

        public void LookAt(Vector3 target)
        {
            camTarget = target;
        }

        public void CalculateFrame()
        {
            Quaternion q = Quaternion.CreateFromYawPitchRoll(camRotations.X, camRotations.Y, camRotations.Z);
            this.camForward = Vector3.Transform(Vector3.Forward, q);
            this.camUp = Vector3.Transform(Vector3.Up, q);
            this.camRight = Vector3.Transform(Vector3.Right, q);
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
            camTarget = Vector3.Add(camTarget, x);
            camPosition = Vector3.Add(camPosition, x);
            camTarget = Vector3.Add(camTarget, y);
            camPosition = Vector3.Add(camPosition, y);
        }

        public void Orbit(Vector3 rots)
        {
            CalculateFrame();
            Matrix quat1 = Matrix.CreateFromYawPitchRoll(rots.X, 0, 0);
            Vector3 perpendicularAxis = camRight;
            Matrix quat2 = Matrix.CreateFromAxisAngle(perpendicularAxis, rots.Y);
            quat1 = quat2 * quat1;
            camRotations.X += rots.X;
            camRotations.Y += rots.Y;
            Vector3 pos = Vector3.Transform(camPosition, quat1);
            camPosition = pos;
            CalculateFrame();
        }

        public void MoveCamera(Vector3 mousePointInitial, Vector3 mousePointFinal)
        {
            Update();
            CalculateFrame();
            float distanceFromCameraTarget = (camPosition - camTarget).Length();
            float farClippingRange = 300f;
            float percentageWorld = distanceFromCameraTarget / farClippingRange;

            Vector3 PercentageScreen = mousePointFinal - mousePointInitial;
            Vector3 Right = device.Viewport.Unproject(new Vector3(0, 250, 1), projectionMatrix, viewMatrix, Matrix.Identity);
            Vector3 Left = device.Viewport.Unproject(new Vector3(500, 250, 1), projectionMatrix, viewMatrix, Matrix.Identity);
            Vector3 Top = device.Viewport.Unproject(new Vector3(250, 0, 1), projectionMatrix, viewMatrix, Matrix.Identity);
            Vector3 Bottom = device.Viewport.Unproject(new Vector3(250, 500, 1), projectionMatrix, viewMatrix, Matrix.Identity);

            Vector3 HorizontalTranslation = Vector3.Multiply((Left - Right), percentageWorld);
            Vector3 VerticalTranslation = Vector3.Multiply((Bottom - Top), percentageWorld);
            HorizontalTranslation *= -(PercentageScreen.X / 500);
            VerticalTranslation *= -(PercentageScreen.Y / 500);
            camTarget += HorizontalTranslation;
            camPosition += HorizontalTranslation;
            camTarget += VerticalTranslation;
            camPosition += VerticalTranslation;
        }

        public void ZoomToTarget(float distanceToZoom)
        {
            Vector3 AngleofZoom = Vector3.Subtract(camTarget, camPosition);
            AngleofZoom.Normalize();
            Vector3 Translation = Vector3.Multiply(AngleofZoom, distanceToZoom);
            camPosition = Vector3.Add(camPosition, Translation);
        }

        public void Update()
        {
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, camUp);
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
        SpriteFont font;
        CameraControls cameraControls;
        Camera camera;
        Cube cube;
        BasicEffect effect;
        GraphicsDevice device;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        VertexDeclaration myVertexDeclaration;

        public Game1()
        {
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
            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 500;
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
            cameraControls = new CameraControls();
            font = Content.Load<SpriteFont>("debug");
            myVertexDeclaration = new VertexDeclaration(device, VertexPositionColor.VertexElements);
            IntializeEffect();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            cube = new Cube();
            // TODO: use this.Content to load your game content here
        }

        void IntializeEffect()
        {
            effect = new BasicEffect(device, null);
            effect.Alpha = 1.0F;
            effect.DiffuseColor = new Vector3(1f, 1f, 1f);
            effect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
            effect.SpecularPower = 0.5f;
            effect.AmbientLightColor = new Vector3(0.75f, 0.75f, 0.75f);
            effect.DirectionalLight0.Enabled = false;
            effect.DirectionalLight0.DiffuseColor = Vector3.One;
            effect.DirectionalLight0.Direction =
                Vector3.Normalize(new Vector3(1.0f, -1.0f, -1.0f));
            effect.DirectionalLight0.SpecularColor = Vector3.One;

            effect.DirectionalLight1.Enabled = false;
            effect.DirectionalLight1.DiffuseColor =
                new Vector3(0.5f, 0.5f, 0.5f);
            effect.DirectionalLight1.Direction =
                Vector3.Normalize(new Vector3(-1.0f, -1.0f, 1.0f));
            effect.DirectionalLight1.SpecularColor =
                new Vector3(0.5f, 0.5f, 0.5f);
            effect.LightingEnabled = false;
            effect.VertexColorEnabled = true;
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
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
            GraphicsDevice.Clear(Color.Blue);


            device.RenderState.CullMode = CullMode.CullClockwiseFace;

            effect.CurrentTechnique = effect.Techniques[0];
            effect.View = camera.viewMatrix;
            effect.Projection = camera.projectionMatrix;
            effect.World = Matrix.Identity;

            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();

                device.VertexDeclaration = myVertexDeclaration;
                device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, cube.vertices, 0, cube.vertices.Length, cube.indices, 0, cube.indices.Length / 3);

                pass.End();
            }
            effect.End();

            device.RenderState.FillMode = FillMode.Solid;

            spriteBatch.Begin();
            cameraControls.DrawInformation(spriteBatch, font);
            camera.DrawInformation(spriteBatch, font);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
