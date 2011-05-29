using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Sunfish.Mode
{
    public partial class RenderDevice : UserControl
    {
        public Device Device;  

        public RenderDevice()
        {
            InitializeComponent();
        }

        public void InitializeDevice()
        {
            PresentParameters presentParameters = new PresentParameters();
            presentParameters.Windowed = true;
            presentParameters.SwapEffect = SwapEffect.Discard;
            Device = new Device(0, DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, presentParameters);
        }
    }

    public struct D3DVertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 Texture;
        public static readonly VertexFormats Format = VertexFormats.Position | VertexFormats.Normal | VertexFormats.Texture1;
    }
}
