using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Sunfish.Canvas
{
    public class EditorGame : Game
    {
        GraphicsDeviceManager graphics;

        public event EventHandler OnInitialize;
        public event EventHandler OnUpdate;

        public bool AcceptInput { get; set; }

        public EditorGame()
        { graphics = new GraphicsDeviceManager(this); }

        protected override void Initialize()
        {
            if (OnInitialize != null) OnInitialize.Invoke(this, null);
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (OnUpdate != null) OnUpdate.Invoke(this, null);
            base.Update(gameTime);
        }
    }
}
