using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace terrain_generator
{
    public partial class Form1 : Form
    {
        private Device device;
        private VertexBuffer vb;
        private IndexBuffer ib;

        private static int terWidth = 5;
        private static int terLength = 5;

        private static int vertCount = terWidth * terLength;
        private static int indCount = (terWidth - 1) * (terLength - 1) * 6;

        private Vector3 cameraPosition, cameraLookAt, cameraUp;

        CustomVertex.PositionColored[] verts;
        private static int[] indices;

        public Form1()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);

            InitializeComponent();
            InitializeGraphics();
        }

        private void InitializeGraphics()
        {
            // Setup PresentParameters
            PresentParameters pp = new PresentParameters();
            pp.Windowed = true;
            pp.SwapEffect = SwapEffect.Discard;

            pp.EnableAutoDepthStencil = true;
            pp.AutoDepthStencilFormat = DepthFormat.D16;

            device = new Device(0, DeviceType.Hardware, this, CreateFlags.HardwareVertexProcessing, pp);

            // generate indices and vertices
            GenerateVertex();
            GenerateIndex();

            // setup VertexBuffer
            vb = new VertexBuffer(typeof(CustomVertex.PositionColored), vertCount, device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionColored.Format, Pool.Default);
            vb.Created += new EventHandler(OnVertexBufferCreate);
            OnVertexBufferCreate(vb, null);

            // setup IndexBuffer
            ib = new IndexBuffer(typeof(int), indCount, device, Usage.WriteOnly, Pool.Default);
            ib.Created += new EventHandler(OnIndexBufferCreate);
            OnIndexBufferCreate(ib, null);

            // setup camera position vectors
            cameraPosition = new Vector3(2, 4.5f, -3.5f);
            cameraLookAt = new Vector3(2, 3.5f,-2.5f);
            cameraUp = new Vector3(0, 1, 0);
        }

        private void OnIndexBufferCreate(object sender, EventArgs e)
        {
            IndexBuffer buffer = (IndexBuffer)sender;

            buffer.SetData(indices, 0, LockFlags.None);
        }

            private void OnVertexBufferCreate(object sender, EventArgs e)
        {
            VertexBuffer buffer = (VertexBuffer)sender;

            buffer.SetData(verts, 0, LockFlags.None);
        }

        private void SetupCamera()
        {
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI/4, this.Width/this.Height, 1.0f, 100.0f);
            device.Transform.View = Matrix.LookAtLH(cameraPosition, cameraLookAt, cameraUp);

            device.RenderState.Lighting = false;
            device.RenderState.CullMode = Cull.CounterClockwise;
            device.RenderState.FillMode = FillMode.WireFrame;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1, 0);

            SetupCamera();
            device.BeginScene();

            device.VertexFormat = CustomVertex.PositionColored.Format;
            device.SetStreamSource(0, vb, 0);
            device.Indices = ib;

            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertCount, 0, indCount/3);

            device.EndScene();
            device.Present();

            this.Invalidate();
        }

        private void GenerateVertex()
        {
            verts = new CustomVertex.PositionColored[vertCount];

            int k = 0;

            for (int z = 0; z < terWidth; z++)
            {
                for (int x = 0; x < terLength; x++)
                {
                    verts[k].Position = new Vector3(x, 0, z);
                    verts[k].Color = Color.White.ToArgb();

                    k++;
                }
            }
        }

        private void GenerateIndex()
        {
            indices = new int[indCount];

            int k = 0;
            int length = 0;

            for (int i = 0; i < indCount; i +=6 )
            {
                // first triangle
                indices[i] = k;
                indices[i + 1] = k + terLength;
                indices[i + 2] = k + terLength + 1;

                indices[i + 3] = k;
                indices[i + 4] = k + terLength + 1;
                indices[i + 5] = k + 1;

                k++;
                length++;

                if (length == terLength - 1)
                {
                    length = 0;
                    k++;
                }
            }
        }
    }
}
