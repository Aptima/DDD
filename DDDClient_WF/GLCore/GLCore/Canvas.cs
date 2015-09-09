using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Aptima.Asim.DDD.Client.Common.GLCore.CoreObjects;

namespace Aptima.Asim.DDD.Client.Common.GLCore
{
    public enum MATRICES:int  { NONE = 0, PROJECTION = 1, VIEW = 2, WORLD = 3 };
    public enum DIRECTION:int {UP = 0, DOWN = 1, LEFT = 2, RIGHT = 3};

    public class Canvas
    {

        private static CustomVertex.TransformedColored[] line_pts = new CustomVertex.TransformedColored[2];
        private static CustomVertex.TransformedColored[] rect_pts = new CustomVertex.TransformedColored[4];
        private static CustomVertex.TransformedColored[] closed_rect_pts = new CustomVertex.TransformedColored[5];
        private CanvasOptions _CanvasOptions_;
        private Control _Target_;
        private Device _Device_;
        private Matrix[] _MTXTransforms = new Matrix[4];
        private Size _canvasSize = new Size();


        public Microsoft.DirectX.Direct3D.Material Material
        {
           
            get
            {
                return _Device_.Material;
            }
            set
            {
                _Device_.Material = value;
            }
        }

        public CanvasOptions Options
        {
            get
            {
                return _CanvasOptions_;
            }
            set
            {
                _CanvasOptions_ = value;
            }
        }
        public Control TargetControl
        {
            get
            {
                return _Target_;
            }
            set
            {
                _Target_ = value;
            }
        }
        public Matrix MtxProjection
        {
            get
            {
                return _MTXTransforms[(int)MATRICES.PROJECTION];
            }
            set
            {
                _MTXTransforms[(int)MATRICES.PROJECTION] = value;
                _Device_.Transform.Projection = value;

            }
        }
        public Matrix MtxView
        {
            get
            {
                return _MTXTransforms[(int)MATRICES.VIEW];
            }
            set
            {
                if (value != null)
                {
                    _MTXTransforms[(int)MATRICES.VIEW] = value;
                    _Device_.Transform.View = value;
                }
            }
        }
        public Matrix MtxWorld
        {
            get
            {
                return _MTXTransforms[(int)MATRICES.WORLD];
            }
            set
            {
                _MTXTransforms[(int)MATRICES.WORLD] = value;
                _Device_.Transform.World = value;
            }
        }
        public Size Size
        {
            get
            {
                if (_Target_ == null)
                {
                    throw new NullReferenceException("GetSize: null Target control.");
                }
                return _canvasSize;
            }
        }

        public Microsoft.DirectX.Direct3D.Viewport Viewport
        {
            get
            {
                return _Device_.Viewport;
            }
            set
            {
                _Device_.Viewport = value;
            }
        }

        public Canvas()
        {
            _Target_ = null;
            _CanvasOptions_ = new CanvasOptions();
        }

        public Canvas(Control target)
        {
            _Target_ = target;
            _CanvasOptions_ = new CanvasOptions();
            
            _Device_ = null;
        }

        public void Quit()
        {
            _Device_.Dispose();
        }

        private void _Device__DeviceReset(object sender, EventArgs e)
        {
            _Device_.Clear(ClearFlags.Target, _CanvasOptions_.BackgroundColor, 1.0f, 0);
            
            _Device_.RenderState.Lighting = false;
            _Device_.RenderState.CullMode = _CanvasOptions_.BackfaceCulling;
            _Device_.RenderState.Ambient = _CanvasOptions_.AmbientColor;
            
        }

        private PresentParameters GetPresentParameters()
        {
            PresentParameters parm = new PresentParameters();

            parm.Windowed = _CanvasOptions_.Windowed;
            
            switch (parm.Windowed)
            {
                case true:              
                    parm.SwapEffect = SwapEffect.Discard;
                    parm.PresentationInterval = PresentInterval.Default;
                    break;
                case false:
                    parm.SwapEffect = SwapEffect.Discard;
                    parm.PresentationInterval = PresentInterval.Immediate;

                    parm.EnableAutoDepthStencil = false;
                    parm.BackBufferCount = 1;
                    parm.BackBufferHeight = Microsoft.DirectX.Direct3D.Manager.Adapters.Default.CurrentDisplayMode.Height;
                    parm.BackBufferWidth = Microsoft.DirectX.Direct3D.Manager.Adapters.Default.CurrentDisplayMode.Width;
                    parm.BackBufferFormat = Microsoft.DirectX.Direct3D.Manager.Adapters.Default.CurrentDisplayMode.Format;
                    break;
            }

            return parm;
        }
        private CreateFlags GetCreateFlags()
        {
            CreateFlags flags;
            Caps cap = Microsoft.DirectX.Direct3D.Manager.GetDeviceCaps(
                Microsoft.DirectX.Direct3D.Manager.Adapters.Default.Adapter, 
                Microsoft.DirectX.Direct3D.DeviceType.Hardware
                );
           
            if (cap.DeviceCaps.SupportsHardwareTransformAndLight)
            {
                flags = CreateFlags.HardwareVertexProcessing;
                if (cap.DeviceCaps.SupportsPureDevice)
                {
                    flags |= CreateFlags.PureDevice;
                    flags |= CreateFlags.MultiThreaded;
                }
            }
            else
            {
                flags = CreateFlags.SoftwareVertexProcessing;
                flags |= CreateFlags.MultiThreaded;
            }

            return flags;

        }

        
        public void Render(Scene scene)
        {
 
            if (_Device_ == null)
            {
                this.InitializeCanvas();
            }
            if (_Device_.Lights.Count > 0)
            {
                _Device_.RenderState.Lighting = true;
            }
            else
            {
                _Device_.RenderState.Lighting = false;
            }


            _Device_.Clear(ClearFlags.Target, _CanvasOptions_.BackgroundColor, 1.0f, 0);

            if (scene != null)
            {
                _Device_.BeginScene();

                MtxView = scene.GetCameraView();

                scene.OnRender(this);

                _Device_.EndScene();
            }
            try
            {
                _Device_.Present();
            }
            catch (DeviceLostException)
            {

                Console.WriteLine("Device Lost @ {0}, {1}", DateTime.Now, scene.GetType().Name);
                RecoverLostDevice();
            }
        }
            
        


        #region ICanvas Members
        public void InitializeCanvas()
        {

            _Device_ = new Microsoft.DirectX.Direct3D.Device(
                Microsoft.DirectX.Direct3D.Manager.Adapters.Default.Adapter,
                Microsoft.DirectX.Direct3D.DeviceType.Hardware,
                _Target_,
                this.GetCreateFlags(),
                this.GetPresentParameters());

            if (Options.Windowed)
            {
                _canvasSize.Width = _Target_.ClientSize.Width;
                _canvasSize.Height = _Target_.ClientSize.Height;
            }
            else
            {
                _canvasSize.Width = _Device_.DisplayMode.Width;
                _canvasSize.Height = _Device_.DisplayMode.Height;
            }

            line_pts[0] = new CustomVertex.TransformedColored();
            line_pts[1] = new CustomVertex.TransformedColored();

            rect_pts[0] = new CustomVertex.TransformedColored();
            rect_pts[1] = new CustomVertex.TransformedColored();
            rect_pts[2] = new CustomVertex.TransformedColored();
            rect_pts[3] = new CustomVertex.TransformedColored();

            closed_rect_pts[0] = new CustomVertex.TransformedColored();
            closed_rect_pts[1] = new CustomVertex.TransformedColored();
            closed_rect_pts[2] = new CustomVertex.TransformedColored();
            closed_rect_pts[3] = new CustomVertex.TransformedColored();
            closed_rect_pts[4] = new CustomVertex.TransformedColored();

            _Device_.DeviceReset += new EventHandler(_Device__DeviceReset);
            _Device_.DeviceResizing += new CancelEventHandler(_Device__DeviceResizing);
            _Device__DeviceReset(this, null);
        }
        private void RecoverLostDevice()
        {
            try
            {
                _Device_.TestCooperativeLevel(); //let's check what the state of the device is, if we can reset the device or not.
            }
            catch (DeviceLostException)
            {
            }
            catch (DeviceNotResetException) //The device can be reset
            {
                try
                {
                    _Device_.Reset(this.GetPresentParameters()); //Reset the device.
                }
                catch (DeviceLostException)
                {
                    Application.ExitThread();
                }
            }
        }
        private void _Device__DeviceResizing(object sender, CancelEventArgs e)
        {
            if ((_Target_.ClientSize.Width == 0) || (_Target_.ClientSize.Height == 0))
            {
                e.Cancel = true;
            }
            _canvasSize.Width = _Target_.ClientSize.Width;
            _canvasSize.Height = _Target_.ClientSize.Height;
        }





        public Mesh CreateTeapot()
        {
            if (_Device_ != null)
            {
                return Mesh.Teapot(_Device_);
            }
            else
            {
                throw new NullReferenceException("Null Device");
            }
        }
        public Sprite CreateSprite()
        {
            if (_Device_ != null)
            {
                return new Sprite(_Device_);
            }
            else
            {
                throw new NullReferenceException("Null Device");
            }
        }
        public Microsoft.DirectX.Direct3D.Font CreateFont(System.Drawing.Font font)
        {
            return new Microsoft.DirectX.Direct3D.Font(_Device_, font);
        }


        public Texture CreateTexture(Bitmap bmp, out int width, out int height)
        {
            try
            {
                Console.WriteLine("Creating texture: Height: {0}; Width: {1}", bmp.Height, bmp.Width);
                width = bmp.Width;
                height = bmp.Height;

                return new Texture(_Device_, bmp, Usage.None, Pool.Managed);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Failed creating texture");
                throw new ArgumentException(e.Message);
            }
        }
        
        public Texture CreateTexture(System.IO.Stream stream, out int width, out int height)
        {
            try
            {
                Bitmap bmp = new Bitmap(stream);
                width = bmp.Width;
                height = bmp.Height;

                return new Texture(_Device_, bmp, Usage.None, Pool.Managed);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message);
            }
        }
        
        
        
        
        public Texture LoadTexture(string filename, out int width, out int height)
        {
            try
            {
                if (System.IO.File.Exists(filename))
                {
                    Bitmap bmp = new Bitmap(filename);
                    width = bmp.Width;
                    height = bmp.Height;

                    return TextureLoader.FromFile(_Device_, filename);
                }
                else
                {
                    throw new Exception("Couldn't locate file: " + filename);
                }
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message);
            }
        }




        public void AddLight(LightType type, Vector3 position, Vector3 direction, float range, float falloff, Color diffuse)
        {
            int index = _Device_.Lights.Count;
            
            _Device_.Lights[index].Type = type;
            _Device_.Lights[index].Diffuse = diffuse;
            _Device_.Lights[index].Position = position;
            _Device_.Lights[index].Range = range;
            _Device_.Lights[index].Falloff = falloff;

            _Device_.Lights[index].Update();       // Let Direct3D know about the light
            _Device_.Lights[index].Enabled = true; // Turn it on

        }



        public void ClearDevice(ClearFlags flag, Color color, float zdepth, int stencil)
        {
            _Device_.Clear(flag, color, zdepth, stencil);
        }


        public void SetTexture(int stage, Texture texture)
        {
            _Device_.SetTexture(stage, texture);
        }
        public void SetAlphaBlend(bool state)
        {
            switch (state)
            {
                case true:
                    _Device_.RenderState.SourceBlend = Blend.SourceAlpha;

                    _Device_.RenderState.DestinationBlend = Blend.InvSourceAlpha;
                    _Device_.RenderState.AlphaBlendEnable = true;
                    break;
                case false:
                    _Device_.RenderState.AlphaBlendEnable = false;
                    break;
            }
        }
        public void SetFillMode(FillMode mode)
        {
            _Device_.SetRenderState(RenderStates.FillMode, (int)mode);
        }
        public void SetWorldMatrix(Matrix mtx)
        {
            MtxWorld = mtx;
        }



        #region DrawFunctions
        public void DrawUserPrimitives(PrimitiveType primative_type, int primative_count, VertexFormats format, object vertex_data)
        {
            _Device_.VertexFormat = format;
            _Device_.DrawUserPrimitives(primative_type, primative_count, vertex_data);
        }

        public void DrawLine(Color linecolor, int width, float x1, float y1, float x2, float y2)
        {
            int color = linecolor.ToArgb();

            DrawLine(color, width, x1, y1, x2, y2);
        }

        public void DrawLine(int linecolor, int width, float x1, float y1, float x2, float y2)
        {
            if (width <= 1)
            {
                // Standard 1 point thick line
                line_pts[0].X = x1;
                line_pts[0].Y = y1;
                line_pts[0].Z = 0;
                line_pts[0].Rhw = 1;
                line_pts[0].Color = linecolor;

                line_pts[1].X = x2;
                line_pts[1].Y = y2;
                line_pts[1].Z = 0;
                line_pts[1].Rhw = 1;
                line_pts[1].Color = linecolor;

                this.DrawUserPrimitives(PrimitiveType.LineList, 1, CustomVertex.TransformedColored.Format, line_pts);
            }
            else
            {
                float angle = (float) Math.Atan2(y2 - y1, x2 - x1);
                float t2sina1 = width / 2 * (float) Math.Sin(angle);
                float t2cosa1 = width / 2 * (float) Math.Cos(angle);

                // Draw thick line
                closed_rect_pts[0].X = x1 + t2sina1;
                closed_rect_pts[0].Y = y1 - t2cosa1;
                closed_rect_pts[0].Z = 0;
                closed_rect_pts[0].Rhw = 1;
                closed_rect_pts[0].Color = linecolor;

                closed_rect_pts[1].X = x2 + t2sina1;
                closed_rect_pts[1].Y = y2 - t2cosa1;
                closed_rect_pts[1].Z = 0;
                closed_rect_pts[1].Rhw = 1;
                closed_rect_pts[1].Color = linecolor;

                closed_rect_pts[2].X = x1 - t2sina1;
                closed_rect_pts[2].Y = y1 + t2cosa1;
                closed_rect_pts[2].Z = 0;
                closed_rect_pts[2].Rhw = 1;
                closed_rect_pts[2].Color = linecolor;

                closed_rect_pts[3].X = x2 - t2sina1;
                closed_rect_pts[3].Y = y2 + t2cosa1;
                closed_rect_pts[3].Z = 0;
                closed_rect_pts[3].Rhw = 1;
                closed_rect_pts[3].Color = linecolor;

                this.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, CustomVertex.TransformedColored.Format, closed_rect_pts);
            }
        }

        public void DrawCircle(Color linecolor, int width, float x1, float y1, float x2, float y2)
        {
            int color = linecolor.ToArgb();

            DrawCircle(color, width, x1, y1, x2, y2);
        }

        public void DrawCircle(int linecolor, int width, float x1, float y1, float x2, float y2)
        {
            int iVertices = 300;
            int iVertex = 0;
            float prevX, prevY, curX, curY, extendedX, extendedY = 0f;
            float angle = (float) Math.Atan2(y2 - y1, x2 - x1);
            float t2sina1;
            float t2cosa1;
            //float t2sina2;
            //float t2cosa2;
            float pointsToExtendLine = (width - 1) / 2;

            if (width < 1)
            {
                return;
            }

            // Calculate the radius length
            float radius = (float) Math.Sqrt(Math.Pow((double) Math.Abs(x1 - x2), 2) + Math.Pow((double) Math.Abs(y1 -y2), 2));
            float fComplete;

            /* Calculate each vertex position */
            prevX = (float) (x1 + ((float)radius * Math.Cos(0)));
            prevY = (float) (y1 + ((float)radius * Math.Sin(0)));
            for (iVertex = 1; iVertex <= iVertices; iVertex ++)
            {
                /* Percentage of circle already drawn */
                fComplete = (float)iVertex / (float)iVertices;
                curX = (float)(x1 + ((float)radius * Math.Cos(2 * Math.PI * fComplete)));
                curY = (float) (y1 + ((float)radius * Math.Sin(2 * Math.PI * fComplete)));

                angle = (float) Math.Atan2(curY - prevY, curX - prevX);
                t2sina1 = pointsToExtendLine * (float)Math.Sin(angle);
                t2cosa1 = pointsToExtendLine * (float)Math.Cos(angle);
                //t2sina2 = pointsToExtendLine * (float)Math.Sin(angle);
                //t2cosa2 = pointsToExtendLine * (float)Math.Cos(angle);

                extendedX = curX + t2cosa1;
                extendedY = curY + t2sina1;

                DrawLine(linecolor, width, prevX, prevY, extendedX, extendedY);

                prevX = curX;
                prevY = curY;
            }
        }

        public void DrawArrow(Color linecolor, int width, float x1, float y1, float x2, float y2)
        {
            int color = linecolor.ToArgb();

            DrawArrow(color, width, x1, y1, x2, y2);
        }

        public void DrawArrow(int linecolor, int width, float x1, float y1, float x2, float y2)
        {
            float arrowPointMult = 15f;

            if (width <= 1)
            {
                // Standard 1 point thick line
                line_pts[0].X = x1;
                line_pts[0].Y = y1;
                line_pts[0].Z = 0;
                line_pts[0].Rhw = 1;
                line_pts[0].Color = linecolor;

                line_pts[1].X = x2;
                line_pts[1].Y = y2;
                line_pts[1].Z = 0;
                line_pts[1].Rhw = 1;
                line_pts[1].Color = linecolor;

                this.DrawUserPrimitives(PrimitiveType.LineList, 1, CustomVertex.TransformedColored.Format, line_pts);

                float angle = (float)Math.Atan2(y2 - y1, x2 - x1);
                float t2sina1 = arrowPointMult * (float)Math.Sin(angle - ((float)Math.PI / 4f));
                float t2cosa1 = arrowPointMult * (float)Math.Cos(angle - ((float)Math.PI / 4f));
                float t2sina2 = arrowPointMult * (float)Math.Sin(angle - 3f * ((float)Math.PI / 4f));
                float t2cosa2 = arrowPointMult * (float)Math.Cos(angle - 3f * ((float)Math.PI / 4f));

                line_pts[0].X = x2;
                line_pts[0].Y = y2;
                line_pts[0].Z = 0;
                line_pts[0].Rhw = 1;
                line_pts[0].Color = linecolor;

                line_pts[1].X = x2 + t2sina1;
                line_pts[1].Y = y2 - t2cosa1;
                line_pts[1].Z = 0;
                line_pts[1].Rhw = 1;
                line_pts[1].Color = linecolor;

                this.DrawUserPrimitives(PrimitiveType.LineList, 1, CustomVertex.TransformedColored.Format, line_pts);

                line_pts[1].X = x2 + t2sina2;
                line_pts[1].Y = y2 - t2cosa2;
                line_pts[1].Z = 0;
                line_pts[1].Rhw = 1;
                line_pts[1].Color = linecolor;

                this.DrawUserPrimitives(PrimitiveType.LineList, 1, CustomVertex.TransformedColored.Format, line_pts);

            }
            else
            {
                DrawLine(linecolor, width, x1, y1, x2, y2);

                float angle = (float)Math.Atan2(y2 - y1, x2 - x1);
                float arrowHeadScale = width / 4;

                if (arrowHeadScale < 1)
                {
                    arrowHeadScale = 1;
                }

                float t2sina1 = arrowPointMult * arrowHeadScale * (float)Math.Sin(angle - ((float)Math.PI / 4f));
                float t2cosa1 = arrowPointMult * arrowHeadScale * (float)Math.Cos(angle - ((float)Math.PI / 4f));
                float t2sina2 = arrowPointMult * arrowHeadScale * (float)Math.Sin(angle - 3f * ((float)Math.PI / 4f));
                float t2cosa2 = arrowPointMult * arrowHeadScale * (float)Math.Cos(angle - 3f * ((float)Math.PI / 4f));
                float t2sina3 = width / 3 * (float)Math.Sin(angle);
                float t2cosa3 = width / 3 * (float)Math.Cos(angle);

                DrawLine(linecolor, width, x2 - t2sina3, y2 + t2cosa3, x2 + t2sina1, y2 - t2cosa1);
                DrawLine(linecolor, width, x2 + t2sina3, y2 - t2cosa3, x2 + t2sina2, y2 - t2cosa2);
            }
        }

        public void DrawShape(List<CustomVertex.TransformedColored> shape, float Scale, float xpos, float ypos, bool closed)
        {
            CustomVertex.TransformedColored[] shape_pts = shape.ToArray();
            for (int i = 0; i < shape_pts.Length; i++)
            {
                shape_pts[i].X *= Scale;
                shape_pts[i].X += xpos;

                shape_pts[i].Y *= Scale;
                shape_pts[i].Y += ypos;
            }

            if (!closed)
            {
                this.DrawUserPrimitives(PrimitiveType.LineStrip, shape_pts.Length - 1, CustomVertex.TransformedColored.Format, shape_pts);
            }
            else
            {
                this.DrawUserPrimitives(PrimitiveType.LineStrip, shape_pts.Length - 1, CustomVertex.TransformedColored.Format, shape_pts);
            }
        }

        public void DrawFillRect(Rectangle rectangle, Material material)
        {
            int color = material.Diffuse.ToArgb();
            Material = material;

            rect_pts[0].X = rectangle.Left;
            rect_pts[0].Y = rectangle.Top;
            rect_pts[0].Z = 0;
            rect_pts[0].Rhw = 1;
            rect_pts[0].Color = color;

            rect_pts[1].X = rectangle.Left;
            rect_pts[1].Y = rectangle.Bottom;
            rect_pts[1].Z = 0;
            rect_pts[1].Rhw = 1;
            rect_pts[1].Color = color;

            rect_pts[2].X = rectangle.Right;
            rect_pts[2].Y = rectangle.Top;
            rect_pts[2].Z = 0;
            rect_pts[2].Rhw = 1;
            rect_pts[2].Color = color;

            rect_pts[3].X = rectangle.Right;
            rect_pts[3].Y = rectangle.Bottom;
            rect_pts[3].Z = 0;
            rect_pts[3].Rhw = 1;
            rect_pts[3].Color = color;

            SetAlphaBlend(true);
            this.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, CustomVertex.TransformedColored.Format, rect_pts);
            SetAlphaBlend(false);
        }
        public void DrawFillRect(RectangleF rectangle, Material material)
        {
            int color = material.Diffuse.ToArgb();         
            Material = material;

            rect_pts[0].X = rectangle.Left;
            rect_pts[0].Y = rectangle.Top;
            rect_pts[0].Z = 0;
            rect_pts[0].Rhw = 1;
            rect_pts[0].Color = color;

            rect_pts[1].X = rectangle.Left;
            rect_pts[1].Y = rectangle.Bottom;
            rect_pts[1].Z = 0;
            rect_pts[1].Rhw = 1;
            rect_pts[1].Color = color;

            rect_pts[2].X = rectangle.Right;
            rect_pts[2].Y = rectangle.Top;
            rect_pts[2].Z = 0;
            rect_pts[2].Rhw = 1;
            rect_pts[2].Color = color;

            rect_pts[3].X = rectangle.Right;
            rect_pts[3].Y = rectangle.Bottom;
            rect_pts[3].Z = 0;
            rect_pts[3].Rhw = 1;
            rect_pts[3].Color = color;

            SetAlphaBlend(true);
            this.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, CustomVertex.TransformedColored.Format, rect_pts);
            SetAlphaBlend(false);
        }

        public void DrawRect(Rectangle rectangle, Color linecolor)
        {
            int color = linecolor.ToArgb();

            closed_rect_pts[0].X = rectangle.Left;
            closed_rect_pts[0].Y = rectangle.Top;
            closed_rect_pts[0].Z = 0;
            closed_rect_pts[0].Rhw = 1;
            closed_rect_pts[0].Color = color;

            closed_rect_pts[1].X = rectangle.Right;
            closed_rect_pts[1].Y = rectangle.Top;
            closed_rect_pts[1].Z = 0;
            closed_rect_pts[1].Rhw = 1;
            closed_rect_pts[1].Color = color;

            closed_rect_pts[2].X = rectangle.Right;
            closed_rect_pts[2].Y = rectangle.Bottom;
            closed_rect_pts[2].Z = 0;
            closed_rect_pts[2].Rhw = 1;
            closed_rect_pts[2].Color = color;

            closed_rect_pts[3].X = rectangle.Left;
            closed_rect_pts[3].Y = rectangle.Bottom;
            closed_rect_pts[3].Z = 0;
            closed_rect_pts[3].Rhw = 1;
            closed_rect_pts[3].Color = color;

            closed_rect_pts[4].X = rectangle.Left;
            closed_rect_pts[4].Y = rectangle.Top;
            closed_rect_pts[4].Z = 0;
            closed_rect_pts[4].Rhw = 1;
            closed_rect_pts[4].Color = color;

            this.DrawUserPrimitives(PrimitiveType.LineStrip, 4, CustomVertex.TransformedColored.Format, closed_rect_pts);
        }
        public void DrawRect(RectangleF rectangle, Color linecolor)
        {
            int color = linecolor.ToArgb();

            closed_rect_pts[0].X = rectangle.Left;
            closed_rect_pts[0].Y = rectangle.Top;
            closed_rect_pts[0].Z = 0;
            closed_rect_pts[0].Rhw = 1;
            closed_rect_pts[0].Color = color;

            closed_rect_pts[1].X = rectangle.Right;
            closed_rect_pts[1].Y = rectangle.Top;
            closed_rect_pts[1].Z = 0;
            closed_rect_pts[1].Rhw = 1;
            closed_rect_pts[1].Color = color;

            closed_rect_pts[2].X = rectangle.Right;
            closed_rect_pts[2].Y = rectangle.Bottom;
            closed_rect_pts[2].Z = 0;
            closed_rect_pts[2].Rhw = 1;
            closed_rect_pts[2].Color = color;

            closed_rect_pts[3].X = rectangle.Left;
            closed_rect_pts[3].Y = rectangle.Bottom;
            closed_rect_pts[3].Z = 0;
            closed_rect_pts[3].Rhw = 1;
            closed_rect_pts[3].Color = color;

            closed_rect_pts[4].X = rectangle.Left;
            closed_rect_pts[4].Y = rectangle.Top;
            closed_rect_pts[4].Z = 0;
            closed_rect_pts[4].Rhw = 1;
            closed_rect_pts[4].Color = color;

            this.DrawUserPrimitives(PrimitiveType.LineStrip, 4, CustomVertex.TransformedColored.Format, closed_rect_pts);
        }

        public void DrawFillTri(Rectangle rectangle, Material material, DIRECTION direction)
        {
            int color = material.Diffuse.ToArgb();
            Material = material;

            switch (direction)
            {
                case DIRECTION.UP:
                    rect_pts[0].X = rectangle.Left;
                    rect_pts[0].Y = rectangle.Bottom;
                    rect_pts[0].Z = 0;
                    rect_pts[0].Rhw = 1;
                    rect_pts[0].Color = color;

                    rect_pts[1].X = (rectangle.Width / 2) + rectangle.Left;
                    rect_pts[1].Y = rectangle.Top;
                    rect_pts[1].Z = 0;
                    rect_pts[1].Rhw = 1;
                    rect_pts[1].Color = color;

                    rect_pts[2].X = rectangle.Right;
                    rect_pts[2].Y = rectangle.Bottom;
                    rect_pts[2].Z = 0;
                    rect_pts[2].Rhw = 1;
                    rect_pts[2].Color = color;
                    break;
                case DIRECTION.DOWN:
                    rect_pts[0].X = rectangle.Left;
                    rect_pts[0].Y = rectangle.Top;
                    rect_pts[0].Z = 0;
                    rect_pts[0].Rhw = 1;
                    rect_pts[0].Color = color;

                    rect_pts[1].X = (rectangle.Width / 2) + rectangle.Left;
                    rect_pts[1].Y = rectangle.Bottom;
                    rect_pts[1].Z = 0;
                    rect_pts[1].Rhw = 1;
                    rect_pts[1].Color = color;

                    rect_pts[2].X = rectangle.Right;
                    rect_pts[2].Y = rectangle.Top;
                    rect_pts[2].Z = 0;
                    rect_pts[2].Rhw = 1;
                    rect_pts[2].Color = color;
                    break;
                case DIRECTION.LEFT:
                    rect_pts[0].X = rectangle.Right;
                    rect_pts[0].Y = rectangle.Top;
                    rect_pts[0].Z = 0;
                    rect_pts[0].Rhw = 1;
                    rect_pts[0].Color = color;

                    rect_pts[1].X = rectangle.Left;
                    rect_pts[1].Y = rectangle.Top + (rectangle.Height / 2);
                    rect_pts[1].Z = 0;
                    rect_pts[1].Rhw = 1;
                    rect_pts[1].Color = color;

                    rect_pts[2].X = rectangle.Right;
                    rect_pts[2].Y = rectangle.Bottom;
                    rect_pts[2].Z = 0;
                    rect_pts[2].Rhw = 1;
                    rect_pts[2].Color = color;
                    break;
                case DIRECTION.RIGHT:
                    rect_pts[0].X = rectangle.Left;
                    rect_pts[0].Y = rectangle.Top;
                    rect_pts[0].Z = 0;
                    rect_pts[0].Rhw = 1;
                    rect_pts[0].Color = color;

                    rect_pts[1].X = rectangle.Right;
                    rect_pts[1].Y = rectangle.Top + (rectangle.Height / 2);
                    rect_pts[1].Z = 0;
                    rect_pts[1].Rhw = 1;
                    rect_pts[1].Color = color;

                    rect_pts[2].X = rectangle.Left;
                    rect_pts[2].Y = rectangle.Bottom;
                    rect_pts[2].Z = 0;
                    rect_pts[2].Rhw = 1;
                    rect_pts[2].Color = color;
                    break;

            }

            SetAlphaBlend(true);
            this.DrawUserPrimitives(PrimitiveType.TriangleStrip, 1, CustomVertex.TransformedColored.Format, rect_pts);
            SetAlphaBlend(false);
        }
        public void DrawFillTri(RectangleF rectangle, Material material, DIRECTION direction)
        {
            int color = material.Diffuse.ToArgb();
            Material = material;

            switch (direction)
            {
                case DIRECTION.UP:
                    rect_pts[0].X = rectangle.Left;
                    rect_pts[0].Y = rectangle.Bottom;
                    rect_pts[0].Z = 0;
                    rect_pts[0].Rhw = 1;
                    rect_pts[0].Color = color;

                    rect_pts[1].X = (rectangle.Width / 2) + rectangle.Left;
                    rect_pts[1].Y = rectangle.Top;
                    rect_pts[1].Z = 0;
                    rect_pts[1].Rhw = 1;
                    rect_pts[1].Color = color;

                    rect_pts[2].X = rectangle.Right;
                    rect_pts[2].Y = rectangle.Bottom;
                    rect_pts[2].Z = 0;
                    rect_pts[2].Rhw = 1;
                    rect_pts[2].Color = color;
                    break;
                case DIRECTION.DOWN:
                    rect_pts[0].X = rectangle.Left;
                    rect_pts[0].Y = rectangle.Top;
                    rect_pts[0].Z = 0;
                    rect_pts[0].Rhw = 1;
                    rect_pts[0].Color = color;

                    rect_pts[1].X = (rectangle.Width / 2) + rectangle.Left;
                    rect_pts[1].Y = rectangle.Bottom;
                    rect_pts[1].Z = 0;
                    rect_pts[1].Rhw = 1;
                    rect_pts[1].Color = color;

                    rect_pts[2].X = rectangle.Right;
                    rect_pts[2].Y = rectangle.Top;
                    rect_pts[2].Z = 0;
                    rect_pts[2].Rhw = 1;
                    rect_pts[2].Color = color;
                    break;
                case DIRECTION.LEFT:
                    rect_pts[0].X = rectangle.Right;
                    rect_pts[0].Y = rectangle.Top;
                    rect_pts[0].Z = 0;
                    rect_pts[0].Rhw = 1;
                    rect_pts[0].Color = color;

                    rect_pts[1].X = rectangle.Left;
                    rect_pts[1].Y = rectangle.Top + (rectangle.Height / 2);
                    rect_pts[1].Z = 0;
                    rect_pts[1].Rhw = 1;
                    rect_pts[1].Color = color;

                    rect_pts[2].X = rectangle.Right;
                    rect_pts[2].Y = rectangle.Bottom;
                    rect_pts[2].Z = 0;
                    rect_pts[2].Rhw = 1;
                    rect_pts[2].Color = color;
                    break;
                case DIRECTION.RIGHT:
                    rect_pts[0].X = rectangle.Left;
                    rect_pts[0].Y = rectangle.Top;
                    rect_pts[0].Z = 0;
                    rect_pts[0].Rhw = 1;
                    rect_pts[0].Color = color;

                    rect_pts[1].X = rectangle.Right;
                    rect_pts[1].Y = rectangle.Top + (rectangle.Height / 2);
                    rect_pts[1].Z = 0;
                    rect_pts[1].Rhw = 1;
                    rect_pts[1].Color = color;

                    rect_pts[2].X = rectangle.Left;
                    rect_pts[2].Y = rectangle.Bottom;
                    rect_pts[2].Z = 0;
                    rect_pts[2].Rhw = 1;
                    rect_pts[2].Color = color;
                    break;

            }

            SetAlphaBlend(true);
            this.DrawUserPrimitives(PrimitiveType.TriangleStrip, 1, CustomVertex.TransformedColored.Format, rect_pts);
            SetAlphaBlend(false);
        }

        public void DrawTri(Rectangle rectangle, Color linecolor, DIRECTION direction)
        {
            int color = linecolor.ToArgb();

            switch (direction)
            {
                case DIRECTION.UP:
                    closed_rect_pts[0].X = rectangle.Left;
                    closed_rect_pts[0].Y = rectangle.Bottom;
                    closed_rect_pts[0].Z = 0;
                    closed_rect_pts[0].Rhw = 1;
                    closed_rect_pts[0].Color = color;

                    closed_rect_pts[1].X = (rectangle.Width / 2) + rectangle.Left;
                    closed_rect_pts[1].Y = rectangle.Top;
                    closed_rect_pts[1].Z = 0;
                    closed_rect_pts[1].Rhw = 1;
                    closed_rect_pts[1].Color = color;

                    closed_rect_pts[2].X = rectangle.Right;
                    closed_rect_pts[2].Y = rectangle.Bottom;
                    closed_rect_pts[2].Z = 0;
                    closed_rect_pts[2].Rhw = 1;
                    closed_rect_pts[2].Color = color;

                    closed_rect_pts[3].X = rectangle.Left;
                    closed_rect_pts[3].Y = rectangle.Bottom;
                    closed_rect_pts[3].Z = 0;
                    closed_rect_pts[3].Rhw = 1;
                    closed_rect_pts[3].Color = color;
                    break;
                case DIRECTION.DOWN:
                    closed_rect_pts[0].X = rectangle.Left;
                    closed_rect_pts[0].Y = rectangle.Top;
                    closed_rect_pts[0].Z = 0;
                    closed_rect_pts[0].Rhw = 1;
                    closed_rect_pts[0].Color = color;

                    closed_rect_pts[1].X = (rectangle.Width / 2) + rectangle.Left;
                    closed_rect_pts[1].Y = rectangle.Bottom;
                    closed_rect_pts[1].Z = 0;
                    closed_rect_pts[1].Rhw = 1;
                    closed_rect_pts[1].Color = color;

                    closed_rect_pts[2].X = rectangle.Right;
                    closed_rect_pts[2].Y = rectangle.Top;
                    closed_rect_pts[2].Z = 0;
                    closed_rect_pts[2].Rhw = 1;
                    closed_rect_pts[2].Color = color;

                    closed_rect_pts[3].X = rectangle.Left;
                    closed_rect_pts[3].Y = rectangle.Top;
                    closed_rect_pts[3].Z = 0;
                    closed_rect_pts[3].Rhw = 1;
                    closed_rect_pts[3].Color = color;
                    break;
                case DIRECTION.LEFT:
                    closed_rect_pts[0].X = rectangle.Right;
                    closed_rect_pts[0].Y = rectangle.Top;
                    closed_rect_pts[0].Z = 0;
                    closed_rect_pts[0].Rhw = 1;
                    closed_rect_pts[0].Color = color;

                    closed_rect_pts[1].X = rectangle.Left;
                    closed_rect_pts[1].Y = rectangle.Top + (rectangle.Height/2);
                    closed_rect_pts[1].Z = 0;
                    closed_rect_pts[1].Rhw = 1;
                    closed_rect_pts[1].Color = color;

                    closed_rect_pts[2].X = rectangle.Right;
                    closed_rect_pts[2].Y = rectangle.Bottom;
                    closed_rect_pts[2].Z = 0;
                    closed_rect_pts[2].Rhw = 1;
                    closed_rect_pts[2].Color = color;

                    closed_rect_pts[3].X = rectangle.Right;
                    closed_rect_pts[3].Y = rectangle.Top;
                    closed_rect_pts[3].Z = 0;
                    closed_rect_pts[3].Rhw = 1;
                    closed_rect_pts[3].Color = color;
                    break;
                case DIRECTION.RIGHT:
                    closed_rect_pts[0].X = rectangle.Left;
                    closed_rect_pts[0].Y = rectangle.Top;
                    closed_rect_pts[0].Z = 0;
                    closed_rect_pts[0].Rhw = 1;
                    closed_rect_pts[0].Color = color;

                    closed_rect_pts[1].X = rectangle.Right;
                    closed_rect_pts[1].Y = rectangle.Top + (rectangle.Height / 2);
                    closed_rect_pts[1].Z = 0;
                    closed_rect_pts[1].Rhw = 1;
                    closed_rect_pts[1].Color = color;

                    closed_rect_pts[2].X = rectangle.Left;
                    closed_rect_pts[2].Y = rectangle.Bottom;
                    closed_rect_pts[2].Z = 0;
                    closed_rect_pts[2].Rhw = 1;
                    closed_rect_pts[2].Color = color;

                    closed_rect_pts[3].X = rectangle.Left;
                    closed_rect_pts[3].Y = rectangle.Top;
                    closed_rect_pts[3].Z = 0;
                    closed_rect_pts[3].Rhw = 1;
                    closed_rect_pts[3].Color = color;
                    break;

            }

            this.DrawUserPrimitives(PrimitiveType.LineStrip, 3, CustomVertex.TransformedColored.Format, closed_rect_pts);
        }
        public void DrawTri(RectangleF rectangle, Color linecolor, DIRECTION direction)
        {
            int color = linecolor.ToArgb();

            switch (direction)
            {
                case DIRECTION.UP:
                    closed_rect_pts[0].X = rectangle.Left;
                    closed_rect_pts[0].Y = rectangle.Bottom;
                    closed_rect_pts[0].Z = 0;
                    closed_rect_pts[0].Rhw = 1;
                    closed_rect_pts[0].Color = color;

                    closed_rect_pts[1].X = (rectangle.Width / 2) + rectangle.Left;
                    closed_rect_pts[1].Y = rectangle.Top;
                    closed_rect_pts[1].Z = 0;
                    closed_rect_pts[1].Rhw = 1;
                    closed_rect_pts[1].Color = color;

                    closed_rect_pts[2].X = rectangle.Right;
                    closed_rect_pts[2].Y = rectangle.Bottom;
                    closed_rect_pts[2].Z = 0;
                    closed_rect_pts[2].Rhw = 1;
                    closed_rect_pts[2].Color = color;

                    closed_rect_pts[3].X = rectangle.Left;
                    closed_rect_pts[3].Y = rectangle.Bottom;
                    closed_rect_pts[3].Z = 0;
                    closed_rect_pts[3].Rhw = 1;
                    closed_rect_pts[3].Color = color;
                    break;
                case DIRECTION.DOWN:
                    closed_rect_pts[0].X = rectangle.Left;
                    closed_rect_pts[0].Y = rectangle.Top;
                    closed_rect_pts[0].Z = 0;
                    closed_rect_pts[0].Rhw = 1;
                    closed_rect_pts[0].Color = color;

                    closed_rect_pts[1].X = (rectangle.Width / 2) + rectangle.Left;
                    closed_rect_pts[1].Y = rectangle.Bottom;
                    closed_rect_pts[1].Z = 0;
                    closed_rect_pts[1].Rhw = 1;
                    closed_rect_pts[1].Color = color;

                    closed_rect_pts[2].X = rectangle.Right;
                    closed_rect_pts[2].Y = rectangle.Top;
                    closed_rect_pts[2].Z = 0;
                    closed_rect_pts[2].Rhw = 1;
                    closed_rect_pts[2].Color = color;

                    closed_rect_pts[3].X = rectangle.Left;
                    closed_rect_pts[3].Y = rectangle.Top;
                    closed_rect_pts[3].Z = 0;
                    closed_rect_pts[3].Rhw = 1;
                    closed_rect_pts[3].Color = color;
                    break;
                case DIRECTION.LEFT:
                    closed_rect_pts[0].X = rectangle.Right;
                    closed_rect_pts[0].Y = rectangle.Top;
                    closed_rect_pts[0].Z = 0;
                    closed_rect_pts[0].Rhw = 1;
                    closed_rect_pts[0].Color = color;

                    closed_rect_pts[1].X = rectangle.Left;
                    closed_rect_pts[1].Y = rectangle.Top + (rectangle.Height / 2);
                    closed_rect_pts[1].Z = 0;
                    closed_rect_pts[1].Rhw = 1;
                    closed_rect_pts[1].Color = color;

                    closed_rect_pts[2].X = rectangle.Right;
                    closed_rect_pts[2].Y = rectangle.Bottom;
                    closed_rect_pts[2].Z = 0;
                    closed_rect_pts[2].Rhw = 1;
                    closed_rect_pts[2].Color = color;

                    closed_rect_pts[3].X = rectangle.Right;
                    closed_rect_pts[3].Y = rectangle.Top;
                    closed_rect_pts[3].Z = 0;
                    closed_rect_pts[3].Rhw = 1;
                    closed_rect_pts[3].Color = color;
                    break;
                case DIRECTION.RIGHT:
                    closed_rect_pts[0].X = rectangle.Left;
                    closed_rect_pts[0].Y = rectangle.Top;
                    closed_rect_pts[0].Z = 0;
                    closed_rect_pts[0].Rhw = 1;
                    closed_rect_pts[0].Color = color;

                    closed_rect_pts[1].X = rectangle.Right;
                    closed_rect_pts[1].Y = rectangle.Top + (rectangle.Height / 2);
                    closed_rect_pts[1].Z = 0;
                    closed_rect_pts[1].Rhw = 1;
                    closed_rect_pts[1].Color = color;

                    closed_rect_pts[2].X = rectangle.Left;
                    closed_rect_pts[2].Y = rectangle.Bottom;
                    closed_rect_pts[2].Z = 0;
                    closed_rect_pts[2].Rhw = 1;
                    closed_rect_pts[2].Color = color;

                    closed_rect_pts[3].X = rectangle.Left;
                    closed_rect_pts[3].Y = rectangle.Top;
                    closed_rect_pts[3].Z = 0;
                    closed_rect_pts[3].Rhw = 1;
                    closed_rect_pts[3].Color = color;
                    break;

            }

            this.DrawUserPrimitives(PrimitiveType.LineStrip, 3, CustomVertex.TransformedColored.Format, closed_rect_pts);
        }

        public void DrawProgressBar(Rectangle bar, Material background, Material foreground, double progress)
        {
            DrawFillRect(bar, background);
            bar.Height -= 2;
            bar.X++;
            bar.Y++;
            bar.Width = (int)(bar.Width * progress)-1;
            DrawFillRect(bar, foreground);
        }
        public void DrawProgressBar(RectangleF bar, Material background, Material foreground, double progress)
        {
            DrawFillRect(bar, background);
            bar.Height -= 2;
            bar.X++;
            bar.Y++;
            bar.Width = (int)(bar.Width * progress) - 1;
            DrawFillRect(bar, foreground);
        }
        #endregion


        public Vector3 Unproject(Vector3 mouse, Matrix object_Mtx)
        {
            return Vector3.Unproject(mouse, _Device_.Viewport, MtxProjection, MtxView, object_Mtx);
        }
        

        #endregion
    }
}
