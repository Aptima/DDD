using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

using System.Drawing;
using GameLib;
using GameLib.GameObjects;
using GameLib.PathController;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using DDD.CommonComponents.NetworkTools;
using DDD.CommonComponents.SimulationEventTools;
using DDD.CommonComponents.DataTypeTools;

namespace TestGame
{
    class GIS_Scene: Scene
    {
        private Path_Linear linearPath;
        private string message;

        List<Obj_Sprite> Objects;

        private NetworkClient connection;

        private string texture_file;
        private Obj_Sprite background;

        private float pixel_areaX;
        private float pixel_areaY;

        Microsoft.DirectX.Direct3D.Font _myFont;

        //public bool drawline;
        //public Vector2 lineStart;
        //public Vector2 lineEnd;

        public GIS_Scene(string file, float areaX, float areaY)
        {
            texture_file = file;
            pixel_areaX = areaX;
            pixel_areaY = areaY;
            linearPath = new Path_Linear();
            Objects = new List<Obj_Sprite>();

        }

        public void Exiting()
        {
            if (connection.IsConnected())
            {
                connection.Disconnect();
            }
        }

        public override void InitializeCameraView()
        {
            return;
        }
        public override void InitializeSceneProjection(ICanvas canvas)
        {
            return;
        }

        public override void InitializeScene(ICanvas canvas)
        {
            // Connect with simulator
            connection = new NetworkClient();
            if (!connection.Connect("ganberg2", 9999))
            {
                throw new ApplicationException("Unable to establish connection with host.");
            }
            else
            {
                connection.Subscribe("TimeTick");
                connection.Subscribe("ViewProUpdate");
            }

            // Create and initialize Gameboard.
            background = new Obj_Sprite(texture_file, SpriteFlags.SortTexture | SpriteFlags.AlphaBlend);
            background.Initialize(canvas);
            background.Position = new Vector3(0, 0, 0);
            background.Rotation = new Vector3(0, 0, 0);
            background.Scaling = new Vector3(1, 1, 1);
            background.Texture(canvas);

            // Initialize and create Font;
            this._myFont = canvas.CreateFont(new System.Drawing.Font("Arial", 10));
            message = string.Empty;

            //f16 = new Obj_Sprite("f16.png", SpriteFlags.AlphaBlend);
            //f16.Initialize(canvas);
            //f16.Texture(canvas);
            //f16.Position = new Vector3(0, 0, 0);
            //f16.Rotation = new Vector3(0, 0, 0);
            //f16.Scaling = new Vector3(1, 1, 1);

        }

        
        public override void RenderScene(ICanvas canvas)
        {
            background.Draw(canvas);


            //linearPath.CalculateNewPosition(ref f16.Position);
            foreach (Obj_Sprite sprite in Objects)
            {
                sprite.Draw(canvas);
            }
            ReadViewProEvents(canvas);

            //if (drawline)
            //{
            //    canvas.DrawLine(Color.Black, 2, lineStart, lineEnd);
            //}
        } 

        public override void Pan(Vector3 offset)
        {
            offset.X *= 1 / this.Scale;
            offset.Y *= 1 / this.Scale;

            // Cycle through Scene's objects and update position.
            background.Position.Add(offset);
            //f16.Position.Add(offset);
            if (linearPath.IsRunning())
            {
                linearPath.StartPosition.Add(offset);
            }
        }

        public override void Zoom(bool zoom_in)
        {
            if (zoom_in)
            {
                ZoomIn();
            }
            else
            {
                ZoomOut();
            }

            // Cycle through Scene's objects and update scale.
            background.Scaling = new Vector3(Scale, Scale, 0);
            //f16.Scaling = new Vector3(Scale, Scale, 0);
        }

        public float MouseDistance(Vector3 mouse_distance)
        {
            float distanceX = mouse_distance.X * (1/Scale) * pixel_areaX;
            float distanceY = mouse_distance.Y * (1/Scale) * pixel_areaY;

            return (float)Math.Sqrt(Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2));
        }


        public bool Select(Vector2 point)
        {
            //float scaledWidth = (f16.Width*f16.Scaling.X)/2;
            //float scaledHeight = (f16.Height*f16.Scaling.Y)/2;
            //float scaledRadius = ((scaledWidth > scaledHeight) ? scaledWidth : scaledHeight);

            //Vector2 center =  new Vector2(f16.Position.X+scaledWidth, f16.Position.Y+scaledHeight);
            //point.X *= (1 / Scale);
            //point.Y *= (1 / Scale);

            //center.Subtract(point);

            //if (center.Length() >= scaledRadius)
            //{
            //    return false;
            //}
            //f16.ToggleSelect();
            return true;
        }

        public void MoveSelected(Vector2 point)
        {
            //if (f16.IsSelected)
            //{
            //    if (!linearPath.IsRunning())
            //    {
            //        point.X *= (1 / Scale);
            //        point.Y *= (1 / Scale);

            //        linearPath.InitializeLineFormula(f16.Position, new Vector3(point.X, point.Y, 0), 24);
            //    }
            //    Console.WriteLine("F16 {0},{1} to destination {2},{3}", f16.Position.X, f16.Position.Y, point.X, point.Y);
            //}
        }


        public Obj_Sprite GetObject(int id, ICanvas canvas)
        {
            if (id > (Objects.Count - 1))
            {
                Objects.Add(new Obj_Sprite("f16.png", SpriteFlags.AlphaBlend));
                Objects[id].Initialize(canvas);
                Objects[id].Texture(canvas);
                Objects[id].Rotation = new Vector3(0, 0, 0);
                Objects[id].Scaling = new Vector3(Scale, Scale, 0);
            }
            return Objects[id];
        }


        public void ReadViewProEvents(ICanvas canvas)
        {
            if (!connection.IsConnected())
            {
                return;
            }
            List<SimulationEvent> events = connection.GetEvents();

            foreach (SimulationEvent vpEvent in events)
            {
                switch (vpEvent.eventType)
                {
                    case "TimeTick":
                        break;
                    case "ViewProUpdate":
                        string id = ((StringValue)vpEvent.parameters["ObjectID"]).value;
                        if (id != null)
                        {
                            Obj_Sprite sprite = GetObject(Int32.Parse(id), canvas);
                            sprite.Position.X = (float)((LocationValue)vpEvent.parameters["Location"]).X;
                            sprite.Position.Y = (float)((LocationValue)vpEvent.parameters["Location"]).Y;
                            sprite.Position.Z = (float)((LocationValue)vpEvent.parameters["Location"]).Z;
                        }
                        break;
                }
            }
            

        }
    
    }
}
