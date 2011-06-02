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
            game.camera.Ctrl = e.Control;
            game.camera.Alt = e.Alt;
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
                xnaViewer1.PauseGame();
                m.Sections[m.Regions[game.SelectedIndex].permutation[0].indices[game.LevelOfDetail]].Mesh.ImportWavefrontObject(wfo, m.BoundingBoxes[0]);
                xnaViewer1.ResumeGame();
            }
        }

        private void ModelEditor_MouseClick(object sender, MouseEventArgs e)
        {
            Ray r = Camera.GetMouseRay(new Vector2(e.X, e.Y), game.GraphicsDevice.Viewport, game.camera);
            BoundingBox box = new BoundingBox(new Vector3(m.BoundingBoxes[0].X.Min, m.BoundingBoxes[0].Y.Min, m.BoundingBoxes[0].Z.Min),
                new Vector3(m.BoundingBoxes[0].X.Max, m.BoundingBoxes[0].Y.Max, m.BoundingBoxes[0].Z.Max));
            if (r.Intersects(box) == null) listBox1.ClearSelected();
        }

        protected override void OnActivated(EventArgs e)
        {
            if (game == null) return;
            xnaViewer1.ResumeGame();
            base.OnActivated(e);
        }

        protected override void OnDeactivate(EventArgs e)
        {
            if (game == null) return;
            xnaViewer1.PauseGame();
            base.OnDeactivate(e);
        }

        private void ModelEditor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((System.Windows.Forms.Keys)e.KeyChar == System.Windows.Forms.Keys.M)
            {
                if (game.GraphicsDevice.RenderState.FillMode == FillMode.WireFrame)
                    game.GraphicsDevice.RenderState.FillMode = FillMode.Solid;
                else game.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;
            }
        }
    }

    public class Camera
    {
        public Vector3 Up;
        public Vector3 Right;
        public Vector3 Forward;
        public Vector3 Position;
        public Matrix Transformations;
        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.01f) { zoom = 0.01f; } CalculatePosition(); }
        }
        public Matrix View;
        public Matrix Projection;

        GraphicsDevice GraphicsDevice;
        
        public bool Ctrl = false;
        public bool Alt = false;

        float zoom;
        MouseState mouseState;
        MouseState lastMouseState;

        public void Update()
        {
            mouseState = Mouse.GetState();
            if (mouseState.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if (Ctrl && Alt)
                {
                    Zoom += (mouseState.Y - lastMouseState.Y) * 0.2f;
                }
                else if (Ctrl)
                {
                    float hRot = (float)(lastMouseState.X - mouseState.X) / 180f;
                    float vRot = (float)(lastMouseState.Y - mouseState.Y) / 180f;
                    Vector3 rots = new Vector3(hRot, vRot, 0);
                    if (vRot != 0 || hRot != 0)
                        Orbit(rots);
                }
                else if (Alt)
                {
                    Vector2 move = new Vector2(mouseState.X - lastMouseState.X, mouseState.Y - lastMouseState.Y);
                    Move(move);
                }
            }
            lastMouseState = mouseState;

            View = Matrix.CreateLookAt(Position, Position + Forward, Up);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1.0f, 300.0f);
        }

        public static Ray GetMouseRay(Vector2 mousePosition, Viewport viewport, Camera camera)
        {
            Vector3 nearPoint = new Vector3(mousePosition, 0);
            Vector3 farPoint = new Vector3(mousePosition, 1);

            nearPoint = viewport.Unproject(nearPoint, camera.Projection, camera.View, Matrix.Identity);
            farPoint = viewport.Unproject(farPoint, camera.Projection, camera.View, Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            return new Ray(nearPoint, direction);
        }

        public Camera(GraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;
            this.Zoom = 5f;
            this.Forward = Vector3.Forward;
            this.Up = Vector3.Up;
            this.Right = Vector3.Right;
            this.Transformations = Matrix.Identity;
        }

        public void LookAt(Vector3 target)
        {
            Transformations *= Matrix.CreateTranslation(target);
            CalculateFrame();
        }

        public void CalculateFrame()
        {
            Quaternion rot;
            Vector3 scale, trans;
            Transformations.Decompose(out scale, out rot, out trans);
            this.Forward = Vector3.Transform(Vector3.UnitY, rot);
            this.Forward.Normalize();
            this.Up = Vector3.Transform(Vector3.UnitZ, rot);
            this.Up.Normalize();
            this.Right = Vector3.Transform(Vector3.UnitX, rot);
            this.Right.Normalize();
            CalculatePosition();
        }

        private void CalculatePosition()
        {
            this.Position = Vector3.Multiply(Vector3.Multiply(Forward, -0.1f), Zoom) + Transformations.Translation;
        }

        public void Orbit(Vector3 rots)
        {
            Matrix quat1 = Matrix.CreateFromAxisAngle(Vector3.UnitZ, rots.X);
            Matrix quat2 = Matrix.CreateFromAxisAngle(Right, rots.Y);
            Transformations *= quat2 * quat1;
            CalculateFrame();
        }

        public void Move(Vector2 translation)
        {
            float distanceFromCameraTarget = (Position - Forward).Length();
            float farClippingRange = 300f;
            float percentageWorld = distanceFromCameraTarget / farClippingRange;

            Vector3 PercentageScreen = new Vector3(translation.X, translation.Y, 0);
            Vector3 Right = GraphicsDevice.Viewport.Unproject(new Vector3(0, 250, 1), Projection, View, Matrix.Identity);
            Vector3 Left = GraphicsDevice.Viewport.Unproject(new Vector3(500, 250, 1), Projection, View, Matrix.Identity);
            Vector3 Top = GraphicsDevice.Viewport.Unproject(new Vector3(250, 0, 1), Projection, View, Matrix.Identity);
            Vector3 Bottom = GraphicsDevice.Viewport.Unproject(new Vector3(250, 500, 1), Projection, View, Matrix.Identity);

            Vector3 HorizontalTranslation = Vector3.Multiply((Left - Right), percentageWorld);
            Vector3 VerticalTranslation = Vector3.Multiply((Bottom - Top), percentageWorld);
            HorizontalTranslation *= -(PercentageScreen.X / 500);
            VerticalTranslation *= -(PercentageScreen.Y / 500);
            Transformations *= Matrix.CreateTranslation(HorizontalTranslation);
            Transformations *= Matrix.CreateTranslation(VerticalTranslation);
            CalculateFrame();
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
            camera.Zoom = 20f;
            camera.Transformations *= Matrix.CreateFromAxisAngle(Vector3.UnitZ, (float)-(Math.PI + (Math.PI / 4)));
            camera.CalculateFrame();
            camera.Transformations *= Matrix.CreateFromAxisAngle(camera.Right, (float)-(Math.PI / 5));
            camera.CalculateFrame();
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
                FillMode mode = GraphicsDevice.RenderState.FillMode;
                GraphicsDevice.Reset(newParams);
                GraphicsDevice.RenderState.FillMode = mode;
            }

            camera.Update();

            base.Update(gameTime);
        }

        int fps;
        float total;
        int FramesPerSecond;

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(125, 125, 125));
            GraphicsDevice.RenderState.CullMode = CullMode.CullClockwiseFace;
            effect.CurrentTechnique = effect.Techniques[0];
            effect.View = camera.View;
            effect.Projection = camera.Projection;
            effect.World = Matrix.Identity;
            effect.LightingEnabled = true;

            if (Model != null)
            {
                #region Render Meshes

                if (true)
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

                #endregion

                #region Render Markers

                if (false)
                {
                    effect.LightingEnabled = false;
                    effect.DiffuseColor = Color.Yellow.ToVector3();
                    effect.Begin();

                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Begin();

                        device.VertexDeclaration = new VertexDeclaration(graphics.GraphicsDevice, Mode.Vertex.VertexElements);

                        foreach (Mode.MarkerGroup mg in Model.MarkerGroups)
                        {
                            foreach (Mode.Marker m in mg.Markers)
                            {
                                MarkerModel mm = MarkerModel.GetMarkerModel(0.01f, new Vector3(m.Translation.X, m.Translation.Y, m.Translation.Z), new Quaternion(m.Rotation.X, m.Rotation.Y, m.Rotation.Z, m.Rotation.W));
                                device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, mm.Vertices, 0, 9, mm.TriangleList, 0, mm.TriangleList.Length / 3);
                                device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, mm.Vertices, 0, 11, mm.LineList, 0, 1);
                            }
                        }

                        pass.End();
                    }

                    effect.End();
                }

                #endregion

                #region Render Nodes

                effect.LightingEnabled = false;
                effect.DiffuseColor = Color.Yellow.ToVector3();

                device.VertexDeclaration = new VertexDeclaration(graphics.GraphicsDevice, Mode.Vertex.VertexElements);

                foreach (Mode.Node node in Model.Nodes)
                {
                    effect.Begin();

                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Begin();

                        BoneModel bm = new BoneModel(Model.Nodes);
                        if (bm.TriangleList.Length > 1)
                            device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, bm.Vertices, 0, bm.Vertices.Length, bm.TriangleList, 0, bm.TriangleList.Length / 2);

                        pass.End();
                    }

                    effect.End();
                }

                #endregion
            }

            base.Draw(gameTime);
        }
    }

    public class VertexPositionNormalColor
    {
        Vector3 Position;
        Vector3 Normal;
        Color Color;

        public VertexPositionNormalColor(Vector3 pos, Vector3 norm, Color col)
        {
            Position = pos;
            Normal = norm;
            Color = col;
        }

        public static int SizeInBytes { get { return 12 + 12 + 4; } }

        public static readonly Microsoft.Xna.Framework.Graphics.VertexElement[] VertexElements =
            new Microsoft.Xna.Framework.Graphics.VertexElement[] 
            {
                new Microsoft.Xna.Framework.Graphics.VertexElement(0, 0, Microsoft.Xna.Framework.Graphics.VertexElementFormat.Vector3,
                        Microsoft.Xna.Framework.Graphics.VertexElementMethod.Default,
                        Microsoft.Xna.Framework.Graphics.VertexElementUsage.Position, 0),
                new Microsoft.Xna.Framework.Graphics.VertexElement(0, sizeof(float) * 3,
                        Microsoft.Xna.Framework.Graphics.VertexElementFormat.Vector3, Microsoft.Xna.Framework.Graphics.VertexElementMethod.Default,
                       Microsoft.Xna.Framework.Graphics.VertexElementUsage.Normal, 0),
                new Microsoft.Xna.Framework.Graphics.VertexElement(0, (sizeof(float) * 3) + (sizeof(float)* 3),
                        Microsoft.Xna.Framework.Graphics.VertexElementFormat.Color, Microsoft.Xna.Framework.Graphics.VertexElementMethod.Default,
                       Microsoft.Xna.Framework.Graphics.VertexElementUsage.Color, 0)     
            };
    }

    public class BoneModel
    {
        public VertexPositionColor[] Vertices;
        public short[] TriangleList;

        public BoneModel(Mode.Node[] nodes)
        {
            Vertices = new VertexPositionColor[nodes.Length];
            List<short> triangleList = new List<short>(0);
            Vertices[0] = new VertexPositionColor(new Vector3(nodes[0].Translation.X, nodes[0].Translation.Y, nodes[0].Translation.Z), Color.Green);
            if (nodes[0].FirstChildNodeIndex > -1)
                CreateNode(triangleList, nodes, nodes[0].FirstChildNodeIndex, new Quaternion(nodes[0].Rotation.X, nodes[0].Rotation.Y, nodes[0].Rotation.Z, nodes[0].Rotation.W), new Vector3(nodes[0].Translation.X, nodes[0].Translation.Y, nodes[0].Translation.Z));
            TriangleList = triangleList.ToArray();
        }

        private void CreateNode(List<short> triangleList, Sunfish.Mode.Node[] nodes, short p, Quaternion quaternion, Vector3 vector3)
        {
            if (p == 6)
            {
            }
            Matrix m = Matrix.CreateTranslation(new Vector3(vector3.X, vector3.Y, vector3.Z));
            m *= Matrix.CreateFromQuaternion(new Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W));
            m *= Matrix.CreateTranslation(new Vector3(nodes[p].Translation.X, nodes[p].Translation.Y, nodes[p].Translation.Z));
            //m *= Matrix.CreateFromQuaternion(new Quaternion(nodes[p].Rotation.X, nodes[p].Rotation.Y, nodes[p].Rotation.Z, nodes[p].Rotation.W));
            Vector3 v = Vector3.Transform(Vector3.Zero, m);
            triangleList.AddRange(new short[] { nodes[p].ParentNodeIndex, p });
            Vertices[p] = new VertexPositionColor(v, Color.DarkBlue);
            if (nodes[p].FirstSiblingNodeIndex > -1)
                CreateNode(triangleList, nodes, nodes[p].FirstSiblingNodeIndex, quaternion, vector3);

            if (nodes[p].FirstChildNodeIndex > -1)
            {
                Quaternion q;
                Vector3 s, t;
                m.Decompose(out s, out q, out t);
                CreateNode(triangleList, nodes, nodes[p].FirstChildNodeIndex, quaternion * new Quaternion(nodes[p].Rotation.X, nodes[p].Rotation.Y, nodes[p].Rotation.Z, nodes[p].Rotation.W), t);
            }
        }

        public BoneModel(Mode.Node node1)
        {
            Vertices = new VertexPositionColor[6];
            //Vertices[0] = new VertexPositionColor(new Vector3(0.0f, 0.0f, 0.05f), Color.White);
            //Vertices[1] = new VertexPositionColor(new Vector3(-0.0002f, -0.0705f, 0.0f), Color.White);
            //Vertices[2] = new VertexPositionColor(new Vector3(0.0705f, 0.0002f, 0.0f), Color.Yellow);
            //Vertices[3] = new VertexPositionColor(new Vector3(0.0002f, 0.0705f, 0.0f), Color.Yellow);
            //Vertices[4] = new VertexPositionColor(new Vector3(-0.0705f, -0.0002f, 0.0f), Color.Yellow);
            //Vertices[5] = new VertexPositionColor(new Vector3(0.0f, 0.0f, -0.05f), Color.Yellow);

            for (int i = 0; i < Vertices.Length; i++)
            {
                Matrix m = Matrix.CreateTranslation(new Vector3(node1.Translation.X, node1.Translation.Y, node1.Translation.Z));
                //m *= Matrix.CreateFromQuaternion(new Quaternion(node1.Rotation.X, node1.Rotation.Y, node1.Rotation.Z, node1.Rotation.W));
                Vertices[i].Position = Vector3.Transform(Vertices[i].Position, m);
            }

            TriangleList = new short[]
            {
                0,1,2,
                0,2,3,
                0,3,4,
                0,4,1,
                1,5,2,
                2,5,3,
                3,5,4,
                4,5,1,
            };
        }

        public static MarkerModel GetMarkerModel(float scale, Vector3 position, Quaternion rotation)
        {
            MarkerModel mm = new MarkerModel();


            Matrix m = Matrix.CreateScale(scale);
            m *= Matrix.CreateFromQuaternion(rotation);
            m *= Matrix.CreateTranslation(position);
            for (int i = 0; i < mm.Vertices.Length; i++)
                mm.Vertices[i].Position = Vector3.Transform(mm.Vertices[i].Position, m);
            return mm;
        }
    }

    public class MarkerModel
    {
        public VertexPositionColor[] Vertices;
        public short[] TriangleList;
        public short[] LineList;

        public MarkerModel()
        {
            Vertices = new VertexPositionColor[11];
            Vertices[0] = new VertexPositionColor(new Vector3(0.5f, 0.0f, 10.0f), Color.Yellow);
            Vertices[1] = new VertexPositionColor(new Vector3(0.3536f, 0.3536f, 10.0f), Color.Yellow);
            Vertices[2] = new VertexPositionColor(new Vector3(0.0f, 0.0f, 12.0f), Color.Yellow);
            Vertices[3] = new VertexPositionColor(new Vector3(0.0f, 0.5f, 10.0f), Color.Yellow);
            Vertices[4] = new VertexPositionColor(new Vector3(-0.3536f, 0.3536f, 10.0f), Color.Yellow);
            Vertices[5] = new VertexPositionColor(new Vector3(-0.5f, 0.0f, 10.0f), Color.Yellow);
            Vertices[6] = new VertexPositionColor(new Vector3(-0.3536f, -0.3536f, 10.0f), Color.Yellow);
            Vertices[7] = new VertexPositionColor(new Vector3(0.0f, -0.5f, 10.0f), Color.Yellow);
            Vertices[8] = new VertexPositionColor(new Vector3(0.3536f, -0.3536f, 10.0f), Color.Yellow);

            Vertices[9] = new VertexPositionColor(new Vector3(0.0f, 0.0f, 10.0f), Color.Yellow);
            Vertices[10] = new VertexPositionColor(new Vector3(0.0f, 0.0f, 0.0f), Color.Yellow);

            TriangleList = new short[]
            {
                0,1,2,
                1,3,2,
                3,4,2,
                4,5,2,
                5,6,2,
                6,7,2,
                7,8,2,
                8,0,2,
                7,6,5,
                5,4,3,
                3,1,0,
                5,3,0,
                7,5,0,
                8,7,0,
            };

            LineList = new short[] { 9, 10 };
        }

        public static MarkerModel GetMarkerModel(float scale, Vector3 position, Quaternion rotation)
        {
            MarkerModel mm = new MarkerModel();
            

            Matrix m = Matrix.CreateScale(scale);
            m *= Matrix.CreateFromQuaternion(rotation);
            m *= Matrix.CreateTranslation(position);
            for (int i = 0; i < mm.Vertices.Length; i++)
                mm.Vertices[i].Position = Vector3.Transform(mm.Vertices[i].Position, m);
            return mm;
        }
    }
}
