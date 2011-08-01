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
using Sunfish.Mode;

namespace Sunfish.GUI
{
    public partial class ModelEditor : SunfishEditor
    {
        Renderer game;
        public Sunfish.Mode.Model Model;

        public int SelectedSection
        {
            get { return _selectedSection; }
            set { _selectedSection = value; if (SelectedSectionChanged != null) SelectedSectionChanged(this, new ValueEventArgs(value)); }
        }
        public int SelectedGroup
        {
            get { return _selectedGroup; }
            set { _selectedGroup = value; if (SelectedGroupChanged != null) SelectedGroupChanged(this, new ValueEventArgs(value)); }
        }

        int _selectedSection;
        int _selectedGroup;

        public event EventHandler<ValueEventArgs> SelectedSectionChanged;
        public event EventHandler<ValueEventArgs> SelectedGroupChanged;
        public class ValueEventArgs : EventArgs
        {
            public object Value;
            public ValueEventArgs(object value)
            {
                Value = value;
            }
        }

        public ModelEditor()
        {
            InitializeComponent();
        }

        internal void LoadTag(string filename)
        {
            this.Tag = filename;
            this.HaloTag = new Tag(filename);
            Model = new Sunfish.Mode.Model(HaloTag);
            Model.SelectedIndexChanged += new EventHandler(m_SelectedIndexChanged);
            foreach (Mode.Region r in Model.Regions)
                listBox1.Items.Add(r.name);
            game = new Renderer(this, xnaViewer1.Height, xnaViewer1.Width);
            xnaViewer1.RunGame(game);
        }

        void m_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        public override void Save()
        {
            HaloTag = Model.CreateTag(HaloTag.Filename);
            base.Save();
        }

        public override void SaveAs(string filename)
        {
            HaloTag = Model.CreateTag(filename);
            base.SaveAs(filename);
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
            this.
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
            comboBox1.Items.Clear();
            if (SelectedSection >= 0)
            {
                comboBox1.Items.Add("All");
                for (int i = 0; i < game.Model.Sections[SelectedSection].mesh.Groups.Length; i++)
                {
                    comboBox1.Items.Add(i);
                    comboBox2.Items.Clear();
                    comboBox2.Items.Add(game.Model.Sections[SelectedSection].mesh.Groups[i].ShaderIndex);
                }
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            game.LevelOfDetail = trackBar1.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (game.SelectedIndex == -1) { return; }
            //OpenFileDialog ofd = new OpenFileDialog();
            //if (ofd.ShowDialog() == DialogResult.OK)
            //{
            //    Mode.WavefrontObject wfo = Sunfish.Mode.Wavefront.ParseWavefrontOBJFile(ofd.FileName);
            //    xnaViewer1.PauseGame();
            //    Model.Sections[Model.Regions[game.SelectedIndex].permutation[0].indices[game.LevelOfDetail]].mesh.ImportWavefrontObject(wfo, Model.Space);
            //    xnaViewer1.ResumeGame();
            //}
        }

        struct Triangle
        {
            public Vector3 A, B, C;

            public Triangle(Vector3 a, Vector3 b, Vector3 c)
            { A = a; B = b; C = c; }

            public void FlipVertexOrder()
            {
                Vector3 t = B;
                B = C;
                C = t;
            }

            public bool IsDegenerate()
            { return (B - A == Vector3.Zero || C - A == Vector3.Zero || B - C == Vector3.Zero); }

            public Nullable<float> Intersects(Ray ray)
            {
                if (IsDegenerate()) return null;
                Vector3 N, N1, N2;
                float d, d1, d2;

                Vector3 AB = B - A;
                Vector3 AC = C - A;

                N = Vector3.Cross(AB, AC);
                d = Vector3.Dot(N, A);

                float t = -((Vector3.Dot(N, ray.Position) + d) / Vector3.Dot(N, ray.Direction));

                N1 = (Vector3.Cross(AC, N)) / (N.LengthSquared());
                d1 = Vector3.Dot(-N1, A);

                N2 = (Vector3.Cross(N, AB)) / (N.LengthSquared());
                d2 = Vector3.Dot(-N2, A);

                Vector3 P = ray.Position + (t * ray.Direction);
                float u = Vector3.Dot(N1, P) + d1;
                float v = Vector3.Dot(N2, P) + d2;

                float det = Vector3.Dot(ray.Direction, N);
                float t1 = d - Vector3.Dot(ray.Position, N);
                Vector3 P1 = det * ray.Position + t1 * ray.Direction;
                float u1 = Vector3.Dot(P1, N1) + det * d1;
                float v1 = Vector3.Dot(P1, N2) + det * d2;

                //float gg = Vector3.Dot(N1, b) + d1;

                //if(Math.Sign(t1) == Math.Sign(det * 
                //Math.Sign(t) = Math.Sign(det * tmax − t)
                bool bb = (Math.Sign(u1) == Math.Sign(det - u1) && Math.Sign(v1) == Math.Sign(det - u1 - v1));
                if (!bb) return null;

                return t1 * (1.0f / det);
            }
        }

        private void ModelEditor_MouseClick(object sender, MouseEventArgs e)
        {
            Ray r = Camera.GetMouseRay(new Vector2(e.X, e.Y), game.GraphicsDevice.Viewport, game.camera);
            BoundingBox box = new BoundingBox(new Vector3(Model.Space.X.Min, Model.Space.Y.Min, Model.Space.Z.Min),
                new Vector3(Model.Space.X.Max, Model.Space.Y.Max, Model.Space.Z.Max));
            if (e.Button == MouseButtons.Left)
            {
                if (r.Intersects(box) != null)
                {
                    Nullable<float> last = null;
                    int index = -1;
                    for (int i = 0; i < game.IsRendered.Length; i++)
                    {
                        if (!game.IsRendered[i]) continue;
                        for (int g = 0; g < game.Model.Sections[i].mesh.Groups.Length; g++)
                        {
                            if (r.Intersects(game.Model.Sections[i].mesh.Groups[g].BoundingBox) != null)
                            {
                                for (int indice = game.Model.Sections[i].mesh.Groups[g].IndiceStart;
                                    indice < game.Model.Sections[i].mesh.Groups[g].IndiceStart + game.Model.Sections[i].mesh.Groups[g].IndiceCount - 2;
                                    indice++)
                                {
                                    Triangle Temp = new Triangle(game.Model.Sections[i].mesh.Vertices[game.Model.Sections[i].mesh.Indices[indice + 0]].Position,
                                        game.Model.Sections[i].mesh.Vertices[game.Model.Sections[i].mesh.Indices[indice + 1]].Position,
                                        game.Model.Sections[i].mesh.Vertices[game.Model.Sections[i].mesh.Indices[indice + 2]].Position);
                                    Nullable<float> distance = Temp.Intersects(r);
                                    if (distance.HasValue && (distance < last || !last.HasValue))
                                    {
                                        index = i;
                                        last = distance; break;
                                    }
                                }
                            }
                        }
                    }
                    ChangeSelection(index);
                }
                else ChangeSelection(-1);
            }
        }

        private void ChangeSelection(int index)
        {
            SelectedSection = index;
            for (int sindex = 0; sindex < Model.Sections.Length; sindex++)
                for (int i = 0; i < Model.Sections[sindex].mesh.Vertices.Length; i++)
                    if (sindex == index) Model.Sections[sindex].mesh.Vertices[i].Color = Color.DarkRed;
                    else Model.Sections[sindex].mesh.Vertices[i].Color = Color.DarkGray;

            listBox1.ClearSelected();
            foreach (Mode.Region region in Model.Regions)
            {
                if (region.Contains(index))
                {
                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        if (listBox1.Items[i].ToString() == region.name)
                        {
                            if (listBox1.SelectedIndex != i) listBox1.SelectedIndex = i;
                        }
                    }
                }
            }
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedGroup = comboBox1.SelectedIndex == -1 ? -1 : comboBox1.SelectedIndex - 1;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            game.FillMode = toolStripButton2.Checked ? FillMode.WireFrame : FillMode.Solid;
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
    public class Renderer : Microsoft.Xna.Framework.Game
    {
        public int LevelOfDetail { get { return _levelOfDetail; } set { _levelOfDetail = value; ChangeLevelOfDetail(); } }
        int _levelOfDetail = 5;
        private void ChangeLevelOfDetail()
        {
            if (Model != null)
            {
                IsRendered = new bool[Model.Sections.Length];
                for (int i = 0; i < Model.Regions.Length; i++)
                    IsRendered[Model.Regions[i].permutation[0].indices[LevelOfDetail]] = true;
            }
        }

        public int SelectedSection = -1;
        public int SelectedGroup = -1;
        SpriteFont font;
        public Camera camera;
        public Sunfish.Mode.Model Model;
        public bool[] IsRendered;
        BasicEffect effect;
        GraphicsDeviceManager DeviceManager;
        SpriteBatch spriteBatch;
        VertexDeclaration myVertexDeclaration;
        public FillMode FillMode { get; set; }
        public bool EdgedFaces { get; set; }

        int width, height;
        public int ViewportWidth { get { return width; } set { if (value > 0) width = value; } }
        public int ViewportHeight { get { return height; } set { if (value > 0) height = value; } }

        public Renderer(ModelEditor editor, int height, int width)
        {
            this.width = width;
            this.height = height;
            this.Model = editor.Model;
            editor.SelectedSectionChanged += new EventHandler<ModelEditor.ValueEventArgs>(editor_SelectedSectionChanged);
            editor.SelectedGroupChanged += new EventHandler<ModelEditor.ValueEventArgs>(editor_SelectedGroupChanged);
            LevelOfDetail = 5;
            DeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        void editor_SelectedGroupChanged(object sender, ModelEditor.ValueEventArgs e)
        {
            SelectedGroup = e.Value is int ? (int)e.Value : -1;
        }

        void editor_SelectedSectionChanged(object sender, ModelEditor.ValueEventArgs e)
        {
            SelectedSection = e.Value is int ? (int)e.Value : -1;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // DeviceManager.PreferredBackBufferFormat = SurfaceFormat.Bgr555;
            DeviceManager.PreferredBackBufferWidth = width;
            DeviceManager.PreferredBackBufferHeight = height;
            DeviceManager.ApplyChanges();
            IsMouseVisible = true;
            base.Initialize();
            GraphicsDevice.VertexDeclaration = new VertexDeclaration(GraphicsDevice, Mode.Vertex.VertexElements);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            camera = new Camera(GraphicsDevice);
            Vector3 mid = new Vector3((Model.Space.X.Min + Model.Space.X.Max) / 2, (Model.Space.Y.Min + Model.Space.Y.Max) / 2, (Model.Space.Z.Min + Model.Space.Z.Max) / 2);
            camera.LookAt(new Vector3(mid.X, mid.Y, mid.Z));
            camera.Zoom = 20f;
            camera.Transformations *= Matrix.CreateFromAxisAngle(Vector3.UnitZ, (float)-(Math.PI + (Math.PI / 4)));
            camera.CalculateFrame();
            camera.Transformations *= Matrix.CreateFromAxisAngle(camera.Right, (float)-(Math.PI / 5));
            camera.CalculateFrame();
            font = Content.Load<SpriteFont>("debug");
            myVertexDeclaration = new VertexDeclaration(GraphicsDevice, VertexPositionColor.VertexElements);
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
                GraphicsDevice.RenderState.FillMode = FillMode;
                GraphicsDevice.VertexDeclaration = new VertexDeclaration(GraphicsDevice, Mode.Vertex.VertexElements);
            }

            camera.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.RenderState.CullMode = CullMode.CullClockwiseFace;
            GraphicsDevice.Clear(new Color(125, 125, 125));
            effect.CurrentTechnique = effect.Techniques[0];
            effect.View = camera.View;
            effect.Projection = camera.Projection;
            effect.World = Matrix.Identity;
            effect.LightingEnabled = true;
            effect.PreferPerPixelLighting = true;

            if (Model != null)
            {
                #region Render Meshes

                GraphicsDevice.RenderState.FillMode = FillMode;

                effect.Begin();
                for (int i = 0; i < Model.Sections.Length; i++)
                {
                    if (!IsRendered[i]) continue;

                    for (int g = 0; g < Model.Sections[i].mesh.Groups.Length; g++)
                    {
                        if (SelectedSection == i && SelectedGroup == -1) effect.DiffuseColor = new Vector3(1, 0, 0);
                        else if (SelectedSection == i && g == SelectedGroup) effect.DiffuseColor = new Vector3(1, 0, 0);
                        else effect.DiffuseColor = Color.Gray.ToVector3();

                        foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                        {
                            pass.Begin();

                            GraphicsDevice.DrawUserIndexedPrimitives<Sunfish.Mode.Vertex>(PrimitiveType.TriangleStrip,
                                Model.Sections[i].mesh.Vertices, 0, Model.Sections[i].mesh.Vertices.Length,
                                Model.Sections[i].mesh.Indices, Model.Sections[i].mesh.Groups[g].IndiceStart, Model.Sections[i].mesh.Groups[g].IndiceCount - 2);

                            pass.End();
                        }
                    }
                }
                effect.End();

                if (EdgedFaces)
                {
                    GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;
                    effect.Begin();
                    for (int i = 0; i < Model.Sections.Length; i++)
                    {
                        if (!IsRendered[i]) continue;
                        for (int g = 0; g < Model.Sections[i].mesh.Groups.Length; g++)
                        {
                            if (SelectedSection == i && SelectedGroup == -1) effect.DiffuseColor = Color.DarkRed.ToVector3();
                            else if (SelectedSection == i && g == SelectedGroup) effect.DiffuseColor = Color.DarkRed.ToVector3();
                            else if (SelectedSection == i && g != SelectedGroup) effect.DiffuseColor = Color.White.ToVector3();
                            else effect.DiffuseColor = new Vector3(0.1f, 0.1f, 0.1f);

                            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                            {
                                pass.Begin();

                                GraphicsDevice.DrawUserIndexedPrimitives<Sunfish.Mode.Vertex>(PrimitiveType.TriangleStrip,
                                    Model.Sections[i].mesh.Vertices, 0, Model.Sections[i].mesh.Vertices.Length,
                                    Model.Sections[i].mesh.Indices, Model.Sections[i].mesh.Groups[g].IndiceStart, Model.Sections[i].mesh.Groups[g].IndiceCount - 2);

                                pass.End();
                            }
                        }
                    }
                    effect.End();
                }

                if (SelectedSection >= 0)
                {
                    GraphicsDevice.RenderState.FillMode = FillMode.Solid;
                    effect.LightingEnabled = false;
                    effect.Begin();
                    effect.DiffuseColor = Color.White.ToVector3();
                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Begin();
                        BoundingBoxModel bbm = new BoundingBoxModel(Model.Sections[SelectedSection].BoundingBox);
                        bbm.Draw(GraphicsDevice);
                        pass.End();
                    }
                    effect.End();
                }

                #endregion

                //if (false)
                //{
                //    //GraphicsDevice.RenderState.CullMode = CullMode.None;
                //    //effect.DiffuseColor = Color.CornflowerBlue.ToVector3();
                //    //effect.LightingEnabled = false;
                //    //for (int i = 0; i < Model.Sections.Length; i++)
                //    //{
                //    //    if (!IsRendered[i]) continue;
                //    //    Mode.Mesh mesh = Model.Sections[i].mesh;

                //    //    GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;

                //    //    effect.Begin();

                //    //    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                //    //    {
                //    //        pass.Begin();

                //    //        GraphicsDevice.VertexDeclaration = new VertexDeclaration(DeviceManager.GraphicsDevice, Mode.Vertex.VertexElements);
                //    //        foreach (Mode.Group g in mesh.Groups)
                //    //        {
                //    //            BoundingBox
                //    //            RectangleModel rm = new RectangleModel(g.BoundingBox);
                //    //            GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, rm.Corners, 0, rm.Corners.Length, rm.Indices, 0, rm.Indices.Length / 3);
                //    //        }
                //    //        pass.End();
                //    //    }

                //    //    effect.End();
                //    //}
                //    //GraphicsDevice.RenderState.CullMode = CullMode.CullClockwiseFace;
                //}

                //GraphicsDevice.RenderState.FillMode = FillMode.Solid;

                //#endregion

                //#region Render Normals

                //if (false)
                //{
                //    for (int i = 0; i < Model.Sections.Length; i++)
                //    {
                //        if (!IsRendered[i]) continue;
                //        Mode.Mesh mesh = Model.Sections[i].mesh;
                //        effect.DiffuseColor = new Vector3(1, 0, 0);
                //        effect.Begin();

                //        foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                //        {
                //            pass.Begin();

                //            GraphicsDevice.VertexDeclaration = new VertexDeclaration(DeviceManager.GraphicsDevice, Mode.Vertex.VertexElements);
                //            foreach (Sunfish.Mode.Vertex vert in mesh.Vertices)
                //            {
                //                //VertexPositionColor[] vertexList = new VertexPositionColor[] { new VertexPositionColor(vert.Position(), Color.Pink), new VertexPositionColor((vert.Position + (vert.Normal * 0.1f)), Color.Pink) };
                //                //GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertexList, 0, 1);
                //            }

                //            pass.End();
                //        }

                //        effect.End();
                //    }
                //}

                //#endregion

                //#region Render Markers

                //if (false)
                //{
                //    effect.LightingEnabled = false;
                //    effect.DiffuseColor = Color.Yellow.ToVector3();
                //    effect.Begin();

                //    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                //    {
                //        pass.Begin();

                //        GraphicsDevice.VertexDeclaration = new VertexDeclaration(DeviceManager.GraphicsDevice, Mode.Vertex.VertexElements);

                //        foreach (Mode.MarkerGroup mg in Model.MarkerGroups)
                //        {
                //            foreach (Mode.Marker m in mg.Markers)
                //            {
                //                MarkerModel mm = MarkerModel.GetMarkerModel(0.01f, new Vector3(m.Translation.X, m.Translation.Y, m.Translation.Z), new Quaternion(m.Rotation.X, m.Rotation.Y, m.Rotation.Z, m.Rotation.W));
                //                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, mm.Vertices, 0, 9, mm.TriangleList, 0, mm.TriangleList.Length / 3);
                //                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, mm.Vertices, 0, 11, mm.LineList, 0, 1);
                //            }
                //        }

                //        pass.End();
                //    }

                //    effect.End();
                //}

                //#endregion

                //#region Render Nodes

                //effect.LightingEnabled = true;
                //effect.DiffuseColor = Color.White.ToVector3();
                //effect.VertexColorEnabled = true;

                //BoneModel bm = new BoneModel(Model.Nodes);
                //foreach (NodeModel m in bm.NodeModels)
                //    m.Draw(GraphicsDevice, effect);

                //effect.LightingEnabled = true;
                //effect.VertexColorEnabled = false;

                base.Draw(gameTime);

                //#endregion
            }
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
        public List<NodeModel> NodeModels;

        public BoneModel(Mode.Node[] nodes)
        {
            NodeModels = new List<NodeModel>(nodes.Length);
            NodeModels.Add(new NodeModel(nodes[0]));
            if (nodes[0].FirstChildNodeIndex > -1)
                CreateNode(ref nodes, nodes[0].FirstChildNodeIndex, NodeModels[0]);
        }

        private void CreateNode(ref Sunfish.Mode.Node[] nodes, short currentNode, NodeModel parent)
        {
            //if (p == 6)
            //{
            //}
            //Matrix m = Matrix.CreateTranslation(new Vector3(vector3.X, vector3.Y, vector3.Z));
            //m *= Matrix.CreateFromQuaternion(new Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W));
            //m *= Matrix.CreateTranslation(new Vector3(nodes[p].Translation.X, nodes[p].Translation.Y, nodes[p].Translation.Z));
            ////m *= Matrix.CreateFromQuaternion(new Quaternion(nodes[p].Rotation.X, nodes[p].Rotation.Y, nodes[p].Rotation.Z, nodes[p].Rotation.W));


            //Vector3 v = Vector3.Transform(Vector3.Zero, m);


            NodeModel node = new NodeModel(nodes[currentNode]);
            //node.Rotation *= new Quaternion(nodes[p].Rotation.X, nodes[p].Rotation.Y, nodes[p].Rotation.Z, nodes[p].Rotation.W);
            //node.Position = v;

            //NodeModels.Add(NodeModel.Join(parent, node));
            NodeModels.Add(node); 
            parent.AddChild(node);
            //triangleList.AddRange(new short[] { nodes[p].ParentNodeIndex, p });
            //Vertices[p] = new VertexPositionColor(v, Color.DarkBlue);
            if (nodes[currentNode].FirstSiblingNodeIndex > -1)
                CreateNode(ref nodes, nodes[currentNode].FirstSiblingNodeIndex, parent);

            if (nodes[currentNode].FirstChildNodeIndex > -1)
            {
                //Quaternion q;
                //Vector3 s, t;
                //m.Decompose(out s, out q, out t);
                CreateNode(ref nodes, nodes[currentNode].FirstChildNodeIndex, node);
            }
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

    public class NodeModel
    {
        NodeModel Parent;
        NodeModel NextSibling;
        NodeModel FirstChild;

        #region World Information 
        
        public Vector3 LocalUp;
        public Vector3 LocalLeft;
        public Vector3 LocalForward;
        public float Scale;
        public Vector3 Position { get { return Parent == null ? _position : Parent.Position + _position; } set { _position = value; CalculateWorldMatrix(); } }
        Vector3 _position;

        public Quaternion Rotation { get { return Parent == null ? _rotation : Parent.Rotation * _rotation; } set { _rotation = value; CalculateWorldMatrix(); } }
        Quaternion _rotation;

        public Vector3 Scales { get { return _scales; } set { _scales = value; } }
        Vector3 _scales;

        private void CalculateWorldMatrix()
        {
            _world = Matrix.CreateFromQuaternion(Rotation) * Matrix.CreateTranslation(Position);
            //if (Parent != null)
            //    _world  ;
            //else _world *= Matrix.CreateScale(Scales) * Matrix.CreateTranslation(Position);
        }

        Vector3 joinTranslation { get { return Vector3.Transform(Vertices[0].Position, World); ; } }
        Vector3 endTranslation { get { return Vector3.Transform(Vertices[5].Position, World); ; } }

        public Matrix World { get { CalculateWorldMatrix(); return _world; } set { _world = value; } }
        Matrix _world;

        #endregion

        VertexPositionColor[] Vertices;
        short[] Indices;

        public NodeModel()
            : this(0.035f, 0.035f, 0.025f) { }

        public NodeModel(Mode.Node node)
            : this() {
                _rotation = node.Rotation.ToXnaQuaternion();
                _position = node.Translation.ToXnaVector3();
                Scale = node.Scale;
                LocalUp = node.InverseUp.ToXnaVector3();
                LocalLeft = node.InverseLeft.ToXnaVector3();
                LocalForward = node.InverseForward.ToXnaVector3();
               //this.CalculateWorldMatrix();
        }

        public NodeModel(NodeModel parent)
            : this()
        {
            Rotation = parent.Rotation;
            Position = parent.Position;
        }

        public NodeModel(float size)
            : this(size, size, size) { }

        public NodeModel(float width, float length, float height)
        {
            Scales = new Vector3(width, length, height);
            Vertices = new VertexPositionColor[6];
            Vertices[0] = new VertexPositionColor(new Vector3(0.0f, 0.0f, height), Color.White);

            Vertices[1] = new VertexPositionColor(new Vector3(0.0f, -length, 0.0f), Color.White);
            Vertices[2] = new VertexPositionColor(new Vector3(width, 0.0f, 0.0f), Color.White);
            Vertices[3] = new VertexPositionColor(new Vector3(0.0f, length, 0.0f), Color.White);
            Vertices[4] = new VertexPositionColor(new Vector3(-width, -0.0f, 0.0f), Color.White);

            Vertices[5] = new VertexPositionColor(new Vector3(0.0f, 0.0f, -height), Color.White);

            Indices = new short[]{
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


        public void AddChild(NodeModel child)
        {
            FirstChild = child;
            child.Parent = this;
        }

        public static NodeModel Join(NodeModel a, NodeModel b)
        {
            return new NodeModel(a.joinTranslation, b.endTranslation);
        }

        public NodeModel(Vector3 A, Vector3 B)
            :this()
        {
            Vector3 AB = B -A;
            float length = AB.Length();
            _scales = new Vector3(0.04f, length / 2, 0.04f);
            _position = A + (0.5f * AB);
            Vector3 direction = AB;
            direction.Normalize();
            float roll = (float)Math.Acos(Vector3.Dot(Vector3.UnitY, direction));
            _rotation = Quaternion.CreateFromAxisAngle(Vector3.Cross(Vector3.UnitY, direction), roll);
            CalculateWorldMatrix();
        }

        static NodeModel()
        {
            
        }

        public void Draw(GraphicsDevice device, BasicEffect effect)
        {
            CalculateWorldMatrix();
            effect.World = World;
            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, Vertices, 0, Vertices.Length, Indices, 0, Indices.Length / 3);
                //VertexPositionColor[] lineVerts = new VertexPositionColor[6]{  new VertexPositionColor( -Vector3.One, Color.Blue), 
                //     new VertexPositionColor(-Vector3.One+LocalForward , Color.Blue),
                //     new VertexPositionColor( -Vector3.One, Color.Red), 
                // new VertexPositionColor(-Vector3.One+ LocalLeft , Color.Red),
                // new VertexPositionColor( -Vector3.One, Color.Green), 
                //new VertexPositionColor(Vector3.Transform((Vector3.One +LocalUp),Quaternion.Inverse( Rotation)) , Color.Green), };
                //device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, lineVerts, 0, lineVerts.Length, new short[] { 0, 1, 2, 3, 4, 5 }, 0, 3);
                pass.End();
            }
            effect.End();
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

    public class BoundingBoxModel
    {
        public VertexPositionColor[] Vertices;
        public Vector3 Dimensions;
        public short[] Indices = new short[]
        {
            0,1,
            0,2,
            0,3,
            4,5,
            4,6,
            4,7,
            8,9,
            8,10,
            8,11,
            12,13,
            12,14,
            12,15,
        };

        public BoundingBoxModel(BoundingBox b)
        {
            Vector3[] vs = b.GetCorners();
            VertexPositionColor[] Corners = new VertexPositionColor[vs.Length];
            for (int i = 0; i < vs.Length; i++)
                Corners[i] = new VertexPositionColor(vs[i], Color.BlueViolet);
            //Vertices = Corners;
            //Indices = new short[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            Vertices = new VertexPositionColor[Corners.Length * 4];
            Dimensions = b.Max - b.Min;
            int c = 0;
            for (int i = 0; i < Vertices.Length; i += 4)
            {
                float x, y, z;
                x = Corners[c].Position.X + (Dimensions.X / 6) > b.Max.X ? Corners[c].Position.X - (Dimensions.X / 6) : Corners[c].Position.X + (Dimensions.X / 6);
                y = Corners[c].Position.Y + (Dimensions.Y / 6) > b.Max.Y ? Corners[c].Position.Y - (Dimensions.Y / 6) : Corners[c].Position.Y + (Dimensions.Y / 6);
                z = Corners[c].Position.Z + (Dimensions.Z / 6) > b.Max.Z ? Corners[c].Position.Z - (Dimensions.Z / 6) : Corners[c].Position.Z + (Dimensions.Z / 6);
                Vertices[i + 0] = Corners[c];
                Vertices[i + 1] = new VertexPositionColor(
                    new Vector3(x, Corners[c].Position.Y, Corners[c].Position.Z),
                    Corners[c].Color
                    );
                Vertices[i + 2] = new VertexPositionColor(
                    new Vector3(Corners[c].Position.X, y, Corners[c].Position.Z),
                    Corners[c].Color
                    );
                Vertices[i + 3] = new VertexPositionColor(
                    new Vector3(Corners[c].Position.X, Corners[c].Position.Y, z),
                    Corners[c].Color
                    );
                c++;
            }
            Indices = new short[Vertices.Length / 4 * 3 * 2];
            short index = 0;
            for (short i = 0; i < Indices.Length; i++)
            {
                if (i % 6 == 0 && i > 0) index++;
                if (i % 2 == 0)
                    Indices[i] = (short)(index - (index % 4));
                else { Indices[i] = ++index; }
            }
        }

        public void Draw(GraphicsDevice device)
        {
            device.RenderState.PointSize = 12.5f;
            device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, Vertices, 0, Vertices.Length, Indices, 0, Indices.Length / 2);
            //device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.PointList, Vertices, 0, Vertices.Length, Indices, 0, Indices.Length);
        }

    }
}