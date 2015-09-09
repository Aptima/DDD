using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

using System.Drawing;
using Aptima.Asim.DDD.Client.Common.GLCore;
using Aptima.Asim.DDD.Client.Common.GLCore.CoreObjects;
using Aptima.Asim.DDD.Client.Common.GLCore.PathController;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

using Aptima.Asim.DDD.Client.Dialogs;
using Aptima.Asim.DDD.Client.Controller;

namespace Aptima.Asim.DDD.Client
{


    partial class MapPlayfield
    {
        Rectangle temp_rect = Rectangle.Empty;
        public bool DrawUnmanagedAssetLabels = true;
        public bool ScaleUnitWithMap = true;



        public override void OnSceneLoading(GameFramework g)
        {
            SceneMode = MODE.SCENE_RENDER;
        }
        public override void OnInitializeScene(GameFramework g)
        {
            this.Fonts.Add("Small", g.CANVAS.CreateFont(new System.Drawing.Font("Arial", 9, FontStyle.Bold)));
        }

        public override void OnRender(Canvas canvas)
        {

            if (Map != null)
            {
                Map.Draw(canvas);
            }

            DrawActiveZones(canvas);

            DDDObjects.Scale = Scale;
            if (!ScaleUnitWithMap)
            {
                DDDObjects.MinScale = 1;
            }
            else
            {
                DDDObjects.MinScale = .5f;
            }

            DDDObjects.MapPosition(Map.Position.X, Map.Position.Y);


            lock (this)
            {
                DDDObjects selected_target = GetSelectedTarget();
                DDDObjects selected_asset = GetSelectedObject();

                //draw range rings
                foreach (string obj_name in OrderedPlayfieldObjects)
                {
                    DDDObjects obj = (DDDObjects)GetMappableObject(obj_name);
                    if (obj != null)
                    {
                        obj.DrawRangeRings(canvas);
                    }
                }

                foreach (string obj_name in OrderedPlayfieldObjects)
                {
                    DDDObjects obj = (DDDObjects)GetMappableObject(obj_name);
                    if (obj != null)
                    {
                        if (obj == selected_asset)
                        { //display mouseover info

                            float xpos = UTM_Mapping.HorizontalPixelsToMeters((obj.ScreenCoordinates.X - Map.Position.X) / Scale);
                            float ypos = UTM_Mapping.VerticalPixelsToMeters((obj.ScreenCoordinates.Y - Map.Position.Y ) / Scale);
                            //float xpos = UTM_Mapping.HorizontalPixelsToMeters((obj.ScreenCoordinates.X + (obj.SpriteArea.Width / 2)) - Map.Position.X);
                            //float ypos = UTM_Mapping.VerticalPixelsToMeters((obj.ScreenCoordinates.Y + (obj.SpriteArea.Height / 2) - (Scale * 50)) - Map.Position.Y);

                            //Console.WriteLine("x: {0}; y: {1}", xpos, ypos);
                           // Console.WriteLine("x: {0}; y: {1}", xpos, ypos + (obj.SpriteArea.Height / 2) / Scale);

                            DDD_Global.Instance.SetRangeFinderDistance(xpos, ypos);
                            //get distance string 
                            string distanceFromSelected = DDD_Global.Instance.RangeFinderDistanceString;
                            int distance = -1;
                            int intensity = -1;
                            if (distanceFromSelected.Trim() != "")
                            {
                                distance = (int)Math.Round(Convert.ToDouble(distanceFromSelected));
                                intensity = obj.GetCapabilityRangeRingIntensity(obj.CurrentlySelectedCapability, true, distance);
                                if (intensity > 0)
                                {
                                    DDD_Global.Instance.RangeFinderIntensityString = String.Format("{0}", intensity);
                                }
                                else
                                {
                                    DDD_Global.Instance.RangeFinderIntensityString = string.Empty;
                                }
                            }
                            //check if a capability is selected, if so get ranges, and then use disance to calculate intensity.
                        }

                       // obj.DrawRangeRings(canvas); //moved above
                        obj.DrawUnmanagedAssetLabel = DrawUnmanagedAssetLabels;
                        obj.SetDiffuseColor();
                        if (obj.IsPathCalculatorRunning())
                        {
                            if (!obj.IsWeapon)
                            {
                                obj.HeadingColor = Color.DodgerBlue;
                            }
                            else
                            {
                                obj.HeadingColor = Color.Red;
                            }
                            obj.DrawHeading(canvas);
                        }
                        if ((obj != selected_asset) && (obj != selected_target))
                        {
                            obj.TextColor = Color.White;
                            obj.BorderColor = Color.Black;
                            obj.Draw(canvas, Fonts["Small"]);
                            obj.DrawSpecial(canvas, Fonts["Small"]);
                        }
                    }

                }


                //Render selected objects last.
                if (selected_target != null)
                {
                    //selected_asset.DiffuseColor = Color.Yellow;
                    selected_target.TextColor = Color.Yellow;
                    selected_target.BorderColor = Color.Yellow;
                    selected_target.Draw(canvas, Fonts["Small"]);
                    selected_target.DrawSpecial(canvas, Fonts["Small"]);
                }
                if (selected_asset != null)
                {
                    //selected_asset.DiffuseColor = Color.Yellow;
                    selected_asset.TextColor = Color.Yellow;
                    selected_asset.BorderColor = Color.Yellow;
                    selected_asset.Draw(canvas, Fonts["Small"]);
                    selected_asset.DrawSpecial(canvas, Fonts["Small"]);
                }

            }



            if (DrawDistanceLine)
            {
                canvas.DrawLine(Color.Lime, 1, LineStartLocation.X, LineStartLocation.Y, LineEndLocation.X, LineEndLocation.Y);
                double distance = Math.Sqrt(Math.Pow((LineEndLocation.X - LineStartLocation.X), 2) + Math.Pow((LineEndLocation.Y - LineStartLocation.Y), 2));
                Fonts["Small"].DrawText(null, 
                    string.Format("{0} m", Math.Round(UTM_Mapping.HorizontalPixelsToMeters((float)(distance/Scale)))), 
                    LineEndLocation.X+12, LineEndLocation.Y - 12, Color.Lime);
            }

        }

        public override void ObjectAddedUpdate(MappableObject obj)
        {
        }

        public override void ObjectRemovedUpdate(string object_id)
        {
        }

        /// <summary>
        ///  Moves object from one asset list to the other when ownership changes.
        /// </summary>
        /// <param name="obj_name"></param>
        public void SwitchOwnership(string object_id)
        {

        }

    }
}
