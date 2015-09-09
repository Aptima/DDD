using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using System.Drawing;
using Aptima.Asim.DDD.Client.Common.GLCore;
using Aptima.Asim.DDD.Client.Common.GLCore.Controls;
using Aptima.Asim.DDD.Client.Common.GLCore.CoreObjects;
using Aptima.Asim.DDD.Client.Common.GLCore.PathController;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;

using Aptima.Asim.DDD.Client.Dialogs;
using Aptima.Asim.DDD.Client.Controller;

namespace Aptima.Asim.DDD.Client
{

    public enum ScrollState : int { NONE = 0, UP = 1, DOWN = 2, LEFT = 3, RIGHT = 4 }
    public enum MenuMode: int {DEFAULT = 0, ATTACK = 1, SUBPLATFORM = 2}

    class DDD_HeadsUpDisplay : WindowManager
    {

        #region Private Members

        public MenuMode Mode = MenuMode.DEFAULT;
        private bool IsPaused = false;
        private ICommand _commands;

        private MapPlayfield MapScene;
        private Obj_Sprite Map = null;

        private Canvas _Canvas_ = null;

        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelSlider Throttle;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel StatusPanel;

        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelMenu AttackMenu;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel AttackPanel;

        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelMenu SubPlatformMenu;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel SubPlatformPanel;

        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelMenu AssetMenu;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelMenu MiscAssetMenu;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel AssetPanel;

        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelMenu VulnerabilityMenu;

        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel PlayfieldPanel;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel ButtonPanel;

        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelStaticButton LegendBtn;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelStaticButton GameControlBtn;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelStaticButton MapControlsBtn;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelStaticButton MissionInfoBtn;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelStaticButton TaskInfoBtn;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelStaticButton ViewportBtn;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelStaticButton CancelAttackBtn;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelStaticButton CancelSubPBtn;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelStaticButton DeSelectBtn;

        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel LeftScrollPanel;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel RightScrollPanel;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel DownScrollPanel;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel UpScrollPanel;

        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelDynamicButton LeftScrollBtn;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelDynamicButton RightScrollBtn;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelDynamicButton UpScrollBtn;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelDynamicButton DownScrollBtn;

        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelTextRegion MsgWindowPanel;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelTextRegion ChatWindowPanel;

        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelLabel ObjectIDLbl;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelLabel ObjectClassLbl;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelLabel ObjectStateLbl;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelLabel LocationLbl;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelLabel MaxSpeedLbl;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelLabel DestinationLbl;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelProgressLabel FuelAmountLbl;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelLabel FuelCapacityLbl;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelLabel TtdLbl;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelLabel SpeedLbl;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelLabel AltitudeLbl;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelLabel VulnerabilityLbl;

        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelLabel AttackModeLbl;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelLabel AttackHeaderLbl;

        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelLabel SubPModeLbl;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelLabel SubPHeaderLbl;

        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelLabel AssetModeLbl;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelLabel AssetHeaderLbl;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelLabel MiscAssetHeaderLbl;

        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelLabel MapNameLbl;


        private Aptima.Asim.DDD.Client.Common.GLCore.Window MsgWindow;
        private Aptima.Asim.DDD.Client.Common.GLCore.Window ChatWindow;

        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelSceneRegion Playfield;
        private string ScoreHeader = string.Empty;

        private ScrollState SCROLL = ScrollState.NONE;


        private Rectangle background;
        private Color background_color = Color.FromArgb(130, 135, 138);
        private Material background_material = new Material();

        private Rectangle frame;
        private Color frame_color = Color.FromArgb(178, 178, 178);
        private Material frame_color_material = new Material();
        
        private Rectangle score;
        private Color score_color = Color.FromArgb(63, 63, 63);
        private Material score_color_material = new Material();

        private Color border_color = Color.FromArgb(150, 150, 160);

        private Rectangle time_rect;

        private Rectangle information_rect;

        private Rectangle button_panel;
        private Rectangle playfield_panel;

        private Rectangle up_arrow;
        private Rectangle down_arrow;
        private Rectangle left_arrow;
        private Rectangle right_arrow;

        private string[] _no_vulnerabilities = new string[] { "No Vulnerabilities" };

        private int ColorStep = 8;

        private string Time = "00:00:00";
        #endregion




        public DDD_HeadsUpDisplay(IGameControl game_control, ICommand commands): base(game_control)
        {
            _commands = commands;
            background_material.Diffuse = background_color;
            frame_color_material.Diffuse = frame_color;
            score_color_material.Diffuse = score_color;
        }
        public void ResetVunerabilities()
        {
            VulnerabilityMenu.Reset();
        }
        #region Scene Events
        public override void OnSceneLoading(GameFramework g)
        {
            SceneMode = MODE.SCENE_RENDER;
        }

        public void InitializeObjects(ViewProMotionUpdate update)
        {
            if (!Sprites.ContainsKey(update.Icon))
            {
                update.Icon = "ImageLib.Unknown.png";
            }
            DDDObjects obj = new DDDObjects();
            obj.ObjectID = update.ObjectId;
            obj.PlayerID = update.PlayerId;
            obj.OwnerID = update.OwnerID;
            obj.CapabilityAndWeapons = null;
            obj.TextBoxColor = Color.FromArgb(update.PlayerColor);
            obj.SetPosition(update.StartX, update.StartY, 0);
            obj.Throttle = 1;
            obj.MaxSpeed = update.MaxSpeed;
            obj.FuelAmount = 0;
            obj.FuelCapacity = 0;
            obj.IsAttacking = update.IsWeapon;
            obj.Altitude = update.StartZ;

            if (!DDD_Global.Instance.nc.IsConnected())
            {
                switch (update.ObjectId)
                {
                    case "object1":
                        obj.PlayerID = "Demo Player";
                        obj.OwnerID = DDD_Global.Instance.PlayerID;
                        obj.CapabilityAndWeapons = new string[] { "Null Capability", "Null Weapon1", "Null Weapon2", "Null Weapon3" };
                        obj.SubPlatforms = new string[] { "Pilot 1", "Navigator 1", "Black Box" };
                        obj.Vulnerabilities = new string[] { "Superman", "Batman", "The Flash" };
                        obj.MaxSpeed = 24;
                        obj.FuelAmount = 100;
                        obj.FuelCapacity = 200;
                        obj.Altitude = 12;
                        obj.SetSprite(Sprites[update.Icon]);
                        obj.DrawWithRotation = false;


                        MapScene.AddMappableObject(update.ObjectId, obj);
                        break;
                    case "object2":
                        obj.PlayerID = "Demo Player";
                        obj.OwnerID = DDD_Global.Instance.PlayerID;
                        obj.CapabilityAndWeapons = new string[] { "Null Capability", "Null Weapon" };
                        obj.Vulnerabilities = new string[] { "Aquaman", "Green Lantern", "Robin" };
                        obj.MaxSpeed = 24;
                        obj.FuelAmount = 275;
                        obj.FuelCapacity = 300;
                        obj.Altitude = 0;
                        obj.SetSprite(Sprites[update.Icon]);
                        obj.DrawWithRotation = false;


                        MapScene.AddMappableObject(update.ObjectId, obj);
                        break;
                    case "object3":
                        obj.PlayerID = "Red";
                        obj.OwnerID = DDD_Global.Instance.PlayerID;
                        obj.MaxSpeed = 24;
                        obj.FuelAmount = 48;
                        obj.FuelCapacity = 50;
                        obj.Altitude = 0;
                        obj.SetSprite(Sprites[update.Icon]);
                        obj.DrawWithRotation = false;


                        MapScene.AddMappableObject(update.ObjectId, obj);
                        break;
                    case "object4":
                        obj.PlayerID = "Red";
                        obj.OwnerID = DDD_Global.Instance.PlayerID;
                        obj.CapabilityAndWeapons = new string[] { "Null Capability", "Null Weapon" };
                        obj.MaxSpeed = 24;
                        obj.FuelAmount = 49;
                        obj.FuelCapacity = 50;
                        obj.Altitude = 0;
                        obj.SetSprite(Sprites[update.Icon]);
                        obj.DrawWithRotation = false;


                        MapScene.AddMappableObject(update.ObjectId, obj);
                        break;
                    case "object5":
                        obj.PlayerID = "Red";
                        obj.OwnerID = DDD_Global.Instance.PlayerID;
                        obj.CapabilityAndWeapons = new string[] { "Null Capability", "Null Weapon" };
                        obj.MaxSpeed = 24;
                        obj.FuelAmount = 50;
                        obj.FuelCapacity = 50;
                        obj.Altitude = 0;
                        obj.SetSprite(Sprites[update.Icon]);
                        obj.DrawWithRotation = false;

                        MapScene.AddMappableObject(update.ObjectId, obj);
                        break;
                    default:
                        obj.PlayerID = "Demo Player";
                        obj.OwnerID = "Demo Player";
                        obj.CapabilityAndWeapons = new string[] { "Null Capability", "Null Weapon1", "Null Weapon2", "Null Weapon3" };
                        obj.MaxSpeed = 24;
                        obj.FuelAmount = 100;
                        obj.FuelCapacity = 200;
                        obj.Altitude = 12;
                        obj.SetSprite(Sprites[update.Icon]);
                        obj.DrawWithRotation = false;


                        MapScene.AddMappableObject(update.ObjectId, obj);
                        break;

                }
            }
            else
            {
                if (!MapScene.ContainsMapObject(update.ObjectId))
                {
                    obj.DrawWithRotation = false;
                    obj.SetSprite(Sprites[update.Icon]);
                    MapScene.AddMappableObject(update.ObjectId, obj);
                    //obj.CanSelect = false;
                    //MapScene.AddMappableObject(update.ObjectId, obj);
                    //((DDDObjects)MapScene.GetMappableObject(update.ObjectId)).Throttle = update.Throttle;
                    //MapScene.MoveMapObject(
                    //        update.ObjectId,
                    //        update.StartX,
                    //        update.StartY,
                    //        update.StartZ,
                    //        update.DestinationX,
                    //        update.DestinationY,
                    //        update.DestinationZ,
                    //        UTM_Mapping.VelocityToPixels((float)(update.Throttle * update.MaxSpeed)));

                    //break;
                }                    
            }
            
            AssetMenu.Reset();
            MiscAssetMenu.Reset();

        }

        public override void OnInitializeScene(GameFramework g)
        {
            Size DisplaySize = g.CANVAS.Size;

            BindGameController();

            this.Fonts.Add("Large", g.CANVAS.CreateFont(new System.Drawing.Font("MS Sans Serif", 14)));
            this.Fonts.Add("Medium", g.CANVAS.CreateFont(new System.Drawing.Font("MS Sans Serif", 12)));
            this.Fonts.Add("Small", g.CANVAS.CreateFont(new System.Drawing.Font("MS Sans Serif", 8)));

            int bottom = 130;

            background.X = 0;
            background.Y = 0;
            background.Height = DisplaySize.Height;
            background.Width = DisplaySize.Width;
            SetRootWindow(background);

            score.X = 0;
            score.Y = 0;
            score.Width = background.Width;
            score.Height = 40;

            time_rect.X = score.Width - 100;
            time_rect.Y = 0;
            time_rect.Width = 100;
            time_rect.Height = 40;

            information_rect.X = background.Right - 200;
            information_rect.Y = score.Bottom + 1;
            information_rect.Width = 200;
            information_rect.Height = background.Height - (score.Height + bottom);

            button_panel.X = 0;
            button_panel.Y = 41;
            button_panel.Height = 20;
            button_panel.Width = background.Width - information_rect.Width;
            
            frame.X = 0;
            frame.Y = button_panel.Bottom + 1;
            frame.Height = background.Height - (score.Height + bottom + button_panel.Height);
            frame.Width = background.Width - information_rect.Width;

            playfield_panel.X = frame.X + 20;
            playfield_panel.Y = frame.Y + 20;
            playfield_panel.Width = frame.Width - 40;
            playfield_panel.Height = frame.Height - 40;


            #region StatusPanel
            StatusPanel = new Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel(
                                                information_rect.X + 1,
                                                information_rect.Y,
                                                information_rect.Right - 1,
                                                information_rect.Y + information_rect.Height);
            StatusPanel.Layout = PanelLayout.VerticalFree;
            StatusPanel.Background = Window.BackgroundColor;
            StatusPanel.Hide = true;

            ObjectIDLbl = (PanelLabel)StatusPanel.BindPanelControl(new PanelLabel("Object Name", 0, 0, StatusPanel.ClientArea.Width, 50));
            ObjectIDLbl.BackgroundColor = Color.Black;
            ObjectIDLbl.ForegroundColor = Color.Yellow;
            ObjectIDLbl.Font = Fonts["Large"];

            PanelLabel b = (PanelLabel)StatusPanel.BindPanelControl(new PanelLabel(0, 0, StatusPanel.ClientArea.Width, 12));
            b.BackgroundColor = StatusPanel.Background;
            b.Font = Fonts["Medium"];

            b = (PanelLabel)StatusPanel.BindPanelControl(new PanelLabel("Class Name", 0, 0, StatusPanel.ClientArea.Width, 25));
            b.BackgroundColor = Color.Black;
            b.Font = Fonts["Medium"];
            ObjectClassLbl = (PanelLabel)StatusPanel.BindPanelControl(new PanelLabel(0, 0, StatusPanel.ClientArea.Width, 25));
            ObjectClassLbl.BackgroundColor = StatusPanel.Background;
            ObjectClassLbl.Font = Fonts["Small"];

            b = (PanelLabel)StatusPanel.BindPanelControl(new PanelLabel("State", 0, 0, StatusPanel.ClientArea.Width, 25));
            b.BackgroundColor = Color.Black;
            b.Font = Fonts["Medium"];
            ObjectStateLbl = (PanelLabel)StatusPanel.BindPanelControl(new PanelLabel(0, 0, StatusPanel.ClientArea.Width, 25));
            ObjectStateLbl.BackgroundColor = StatusPanel.Background;
            ObjectStateLbl.Font = Fonts["Small"];

            b = (PanelLabel)StatusPanel.BindPanelControl(new PanelLabel("Location", 0, 0, StatusPanel.ClientArea.Width, 25));
            b.BackgroundColor = Color.Black;
            b.Font = Fonts["Medium"];
            LocationLbl = (PanelLabel)StatusPanel.BindPanelControl(new PanelLabel(0, 0, StatusPanel.ClientArea.Width, 25));
            LocationLbl.ForegroundColor = Color.Lime;
            LocationLbl.BackgroundColor = StatusPanel.Background;
            LocationLbl.Font = Fonts["Small"];

            AltitudeLbl = (PanelLabel)StatusPanel.BindPanelControl(new PanelLabel(0, 0, StatusPanel.ClientArea.Width, 25));
            AltitudeLbl.BackgroundColor = StatusPanel.Background;
            AltitudeLbl.Font = Fonts["Small"];

            b = (PanelLabel)StatusPanel.BindPanelControl(new PanelLabel("Destination", 0, 0, StatusPanel.ClientArea.Width, 25));
            b.BackgroundColor = Color.Black;
            b.Font = Fonts["Medium"];
            DestinationLbl = (PanelLabel)StatusPanel.BindPanelControl(new PanelLabel(0, 0, StatusPanel.ClientArea.Width, 25));
            DestinationLbl.ForegroundColor = Color.Lime;
            DestinationLbl.BackgroundColor = StatusPanel.Background;
            DestinationLbl.Font = Fonts["Small"];

            TtdLbl = (PanelLabel)StatusPanel.BindPanelControl(new PanelLabel(0, 0, StatusPanel.ClientArea.Width, 25));
            TtdLbl.ForegroundColor = Color.Lime;
            TtdLbl.BackgroundColor = StatusPanel.Background;
            TtdLbl.Font = Fonts["Medium"];

            b = (PanelLabel)StatusPanel.BindPanelControl(new PanelLabel("Speed", 0, 0, StatusPanel.ClientArea.Width, 25));
            b.BackgroundColor = Color.Black;
            b.Font = Fonts["Medium"];
            SpeedLbl = (PanelLabel)StatusPanel.BindPanelControl(new PanelLabel(0, 0, StatusPanel.ClientArea.Width, 25));
            SpeedLbl.BackgroundColor = StatusPanel.Background;
            SpeedLbl.Font = Fonts["Small"];

            b = (PanelLabel)StatusPanel.BindPanelControl(new PanelLabel("Maximum Speed", 0, 0, StatusPanel.ClientArea.Width, 25));
            b.BackgroundColor = Color.Black;
            b.Font = Fonts["Medium"];
            MaxSpeedLbl = (PanelLabel)StatusPanel.BindPanelControl(new PanelLabel(0, 0, StatusPanel.ClientArea.Width, 25));
            MaxSpeedLbl.BackgroundColor = StatusPanel.Background;
            MaxSpeedLbl.Font = Fonts["Small"];

            b = (PanelLabel)StatusPanel.BindPanelControl(new PanelLabel("Throttle", 0, 0, StatusPanel.ClientArea.Width, 25));
            b.BackgroundColor = Color.Black;
            b.Font = Fonts["Medium"];

            Throttle = (PanelSlider)StatusPanel.BindPanelControl(new PanelSlider(0, 0, StatusPanel.ClientArea.Width,30));
            Throttle.BackgroundColor = StatusPanel.Background;
            Throttle.Font = Fonts["Small"];
            Throttle.Handler = new PanelSliderChangeHandler(SliderHandler);

            b = (PanelLabel)StatusPanel.BindPanelControl(new PanelLabel("Fuel Amount", 0, 0, StatusPanel.ClientArea.Width, 25));
            b.BackgroundColor = Color.Black;
            b.Font = Fonts["Medium"];
            FuelAmountLbl = (PanelProgressLabel)StatusPanel.BindPanelControl(new PanelProgressLabel(0, 0, StatusPanel.ClientArea.Width, 25));
            FuelAmountLbl.BackgroundColor = StatusPanel.Background;
            FuelAmountLbl.Font = Fonts["Small"];

            b = (PanelLabel)StatusPanel.BindPanelControl(new PanelLabel("Fuel Capacity", 0, 0, StatusPanel.ClientArea.Width, 25));
            b.BackgroundColor = Color.Black;
            b.Font = Fonts["Medium"];
            FuelCapacityLbl = (PanelLabel)StatusPanel.BindPanelControl(new PanelLabel(0, 0, StatusPanel.ClientArea.Width, 25));
            FuelCapacityLbl.BackgroundColor = StatusPanel.Background;
            FuelCapacityLbl.Font = Fonts["Small"];

            b = (PanelLabel)StatusPanel.BindPanelControl(new PanelLabel("Vulnerabilities", 0, 0, StatusPanel.ClientArea.Width, 25));
            b.BackgroundColor = Color.Black;
            b.Font = Fonts["Medium"];
            VulnerabilityMenu = (PanelMenu)StatusPanel.BindPanelControl(new PanelMenu(Fonts["Medium"], string.Empty, 0, 0, StatusPanel.ClientArea.Width, 75, null));
            VulnerabilityMenu.BackgroundColor = Color.Black;
            VulnerabilityMenu.ForegroundColor = Window.CaptionColor;
            VulnerabilityMenu.Font = Fonts["Medium"];

            b = (PanelLabel)StatusPanel.BindPanelControl(new PanelLabel(0, 0, StatusPanel.ClientArea.Width, 12));
            b.BackgroundColor = StatusPanel.Background;
            b.Font = Fonts["Medium"];

            DeSelectBtn = new PanelStaticButton(0, 0, StatusPanel.ClientArea.Width, 25);
            DeSelectBtn.Text = "Cancel / DeSelect";
            DeSelectBtn.MouseClick = new MouseEventHandler(this.DeSelectBtnHandler);
            DeSelectBtn.Vertical = true;
            DeSelectBtn.Font = Fonts["Medium"];

            StatusPanel.BindPanelControl(DeSelectBtn);


            #endregion

            #region AssetPanel
            AssetPanel = new Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel(
                                    information_rect.X + 1,
                                    information_rect.Y,
                                    information_rect.Right - 1,
                                    information_rect.Y + information_rect.Height);
            AssetPanel.Layout = PanelLayout.VerticalFree;
            AssetPanel.Background = Window.BackgroundColor;

            AssetPanel.Hide = true;

            AssetModeLbl = (PanelLabel)AssetPanel.BindPanelControl(new PanelLabel("Asset Menu", 0, 0, AssetPanel.ClientArea.Width, 50));
            AssetModeLbl.BackgroundColor = Color.Black;
            AssetModeLbl.ForegroundColor = Color.Yellow;
            AssetModeLbl.Font = Fonts["Large"];

            AssetHeaderLbl = (PanelLabel)AssetPanel.BindPanelControl(new PanelLabel("Managed Assets ...", 0, 0, AssetPanel.ClientArea.Width, 25));
            AssetHeaderLbl.BackgroundColor = Color.Black;
            AssetHeaderLbl.Font = Fonts["Medium"];

            AssetMenu = new PanelMenu(Fonts["Medium"], string.Empty, 0, 0, AssetPanel.ClientArea.Width, AssetHeaderLbl.ClientArea.Bottom,
                new PanelMenuSelectHandler(AssetHandler));
            AssetMenu.SetHeight(information_rect.Height / 3);
            AssetMenu.BackgroundColor = AssetPanel.Background;
            AssetPanel.BindPanelControl(AssetMenu);

            MiscAssetHeaderLbl = (PanelLabel)AssetPanel.BindPanelControl(new PanelLabel("UnManaged Assets ...", 0, 0, AssetPanel.ClientArea.Width, 25));
            MiscAssetHeaderLbl.BackgroundColor = Color.Black;
            MiscAssetHeaderLbl.Font = Fonts["Medium"];

            MiscAssetMenu = new PanelMenu(Fonts["Medium"], string.Empty, 0, 0, AssetPanel.ClientArea.Width, MiscAssetHeaderLbl.ClientArea.Bottom,
                new PanelMenuSelectHandler(AssetHandler));
            MiscAssetMenu.SetHeight(information_rect.Height / 3);
            MiscAssetMenu.BackgroundColor = AssetPanel.Background;
            AssetPanel.BindPanelControl(MiscAssetMenu);

            #endregion
            
            #region AttackPanel
            AttackPanel = new Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel(
                                                information_rect.X + 1,
                                                information_rect.Y,
                                                information_rect.Right - 1,
                                                information_rect.Y + information_rect.Height);
            AttackPanel.Layout = PanelLayout.VerticalFree;
            AttackPanel.Background = Window.BackgroundColor;
            AttackPanel.Hide = true;

            AttackModeLbl = (PanelLabel) AttackPanel.BindPanelControl(new PanelLabel("Object Name", 0, 0, AttackPanel.ClientArea.Width, 50));
            AttackModeLbl.BackgroundColor = Color.Black;
            AttackModeLbl.ForegroundColor = Color.Yellow;
            AttackModeLbl.Font = Fonts["Large"];

            AttackHeaderLbl = (PanelLabel)AttackPanel.BindPanelControl(new PanelLabel("Choose Weapon ...", 0, 0, AttackPanel.ClientArea.Width, 25));
            AttackHeaderLbl.BackgroundColor = Color.Black;
            AttackHeaderLbl.Font = Fonts["Medium"];


            AttackMenu = new PanelMenu(Fonts["Medium"], string.Empty, 0, 0, AttackPanel.ClientArea.Width, AttackModeLbl.ClientArea.Bottom,  
                new PanelMenuSelectHandler(AttackHandler));
            AttackMenu.SetHeight(information_rect.Height / 2);
            AttackMenu.BackgroundColor = AttackPanel.Background;
            AttackPanel.BindPanelControl(AttackMenu);


            CancelAttackBtn = new PanelStaticButton(0, 0, AttackMenu.ClientArea.Width, 25);
            CancelAttackBtn.Text = "Cancel";
            CancelAttackBtn.MouseClick = new MouseEventHandler(this.CancelAttackBtnHandler);
            CancelAttackBtn.Vertical = true;
            CancelAttackBtn.Font = Fonts["Medium"];

            AttackPanel.BindPanelControl(CancelAttackBtn);
            #endregion AttackPanel
            
            #region SubPPanel
            SubPlatformPanel = new Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel(
                                                information_rect.X + 1,
                                                information_rect.Y,
                                                information_rect.Right - 1,
                                                information_rect.Y + information_rect.Height);
            SubPlatformPanel.Layout = PanelLayout.VerticalFree;
            SubPlatformPanel.Background = Window.BackgroundColor;
            SubPlatformPanel.Hide = true;

            SubPModeLbl = (PanelLabel)SubPlatformPanel.BindPanelControl(new PanelLabel("Object Name", 0, 0, SubPlatformPanel.ClientArea.Width, 50));
            SubPModeLbl.BackgroundColor = Color.Black;
            SubPModeLbl.ForegroundColor = Color.Yellow;
            SubPModeLbl.Font = Fonts["Large"];

            SubPHeaderLbl = (PanelLabel)SubPlatformPanel.BindPanelControl(new PanelLabel("Choose Sub-Platform ...", 0, 0, SubPlatformPanel.ClientArea.Width, 25));
            SubPHeaderLbl.BackgroundColor = Color.Black;
            SubPHeaderLbl.Font = Fonts["Medium"];


            SubPlatformMenu = new PanelMenu(Fonts["Medium"], string.Empty, 0, 0, AttackPanel.ClientArea.Width, AttackModeLbl.ClientArea.Bottom,
                new PanelMenuSelectHandler(SubPHandler));
            SubPlatformMenu.SetHeight(information_rect.Height / 2);
            SubPlatformMenu.BackgroundColor = SubPlatformPanel.Background;
            SubPlatformPanel.BindPanelControl(SubPlatformMenu);


            CancelSubPBtn = new PanelStaticButton(0, 0, SubPlatformMenu.ClientArea.Width, 25);
            CancelSubPBtn.Text = "Cancel SubP";
            CancelSubPBtn.MouseClick = new MouseEventHandler(this.CancelSubPBtnHandler);
            CancelSubPBtn.Vertical = true;
            CancelSubPBtn.Font = Fonts["Medium"];

            SubPlatformPanel.BindPanelControl(CancelSubPBtn);
            #endregion AttackPanel
            
            #region Misc UI

            PlayfieldPanel = CreatePanel(playfield_panel.X, playfield_panel.Y, playfield_panel.Right, playfield_panel.Bottom);
            ButtonPanel = CreatePanel(button_panel.X, button_panel.Y, button_panel.Right, button_panel.Bottom);
            ButtonPanel.Background = StatusPanel.Background;
            ButtonPanel.Layout = PanelLayout.HorizontalFree;

            MapNameLbl = (PanelLabel)ButtonPanel.BindPanelControl(
                new PanelLabel("Map: " + DDD_Global.Instance.MapName.Substring(0, DDD_Global.Instance.MapName.Length - 4)
                , 0, 0, ButtonPanel.ClientArea.Width, ButtonPanel.ClientArea.Height));
            MapNameLbl.BackgroundColor = ButtonPanel.Background;
            MapNameLbl.Font = Fonts["Medium"];

            

            MsgWindow = CreateWindow(g.CANVAS.CreateFont(new System.Drawing.Font("MS Sans Serif", 12)),
                "Messages", 0, frame.Bottom + 2, (int)(background.Width / 2), background.Bottom);

            MsgWindowPanel = (PanelTextRegion)MsgWindow.BindPanelControl(new Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelTextRegion(g.CANVAS.CreateFont(new System.Drawing.Font("MS Sans Serif", 10))
               , 0, frame.Bottom + 2, (int)(background.Width / 2), background.Bottom));

            MsgWindowPanel.Sticky = true;


            ChatWindow = CreateWindow(g.CANVAS.CreateFont(new System.Drawing.Font("MS Sans Serif", 12)), "Chat",
                MsgWindowPanel.ClientArea.Right + 2,
                frame.Bottom + 2,
                background.Right - 2,
                background.Bottom);

            ChatWindowPanel = (PanelTextRegion)ChatWindow.BindPanelControl(new Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelTextRegion(g.CANVAS.CreateFont(new System.Drawing.Font("MS Sans Serif", 10)),
                MsgWindowPanel.ClientArea.Right + 2,
                frame.Bottom + 2,
                background.Right - 2,
                background.Bottom));

            ChatWindowPanel.HasInput = true;
            ChatWindowPanel.Sticky = true;
            ChatWindowPanel.OnTextChat = new TextChatEventHandler(this.TextChatHandler);


            LeftScrollPanel = CreatePanel(frame.X, playfield_panel.Y, playfield_panel.X - 1, playfield_panel.Bottom);
            LeftScrollBtn = (PanelDynamicButton)LeftScrollPanel.BindPanelControl(new Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelDynamicButton(frame.X, playfield_panel.Y, playfield_panel.X - 1, playfield_panel.Bottom));
            LeftScrollBtn.MouseDown = new MouseEventHandler(this.LeftScroll);
            LeftScrollBtn.MouseUp = new MouseEventHandler(this.StopScroll);
            left_arrow.X = LeftScrollPanel.ClientArea.X + (LeftScrollPanel.ClientArea.Width / 2) - 6;
            left_arrow.Y = LeftScrollPanel.ClientArea.Top + (LeftScrollPanel.ClientArea.Height / 2) - 6;
            left_arrow.Width = 12;
            left_arrow.Height = 12;


            RightScrollPanel = CreatePanel(playfield_panel.Right + 1, playfield_panel.Y, frame.Right, playfield_panel.Bottom);
            RightScrollBtn = (PanelDynamicButton)RightScrollPanel.BindPanelControl(new Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelDynamicButton(playfield_panel.Right + 1, playfield_panel.Y, frame.Right, playfield_panel.Bottom));
            RightScrollBtn.MouseDown = new MouseEventHandler(this.RightScroll);
            RightScrollBtn.MouseUp = new MouseEventHandler(this.StopScroll);
            right_arrow.X = RightScrollPanel.ClientArea.X + (RightScrollPanel.ClientArea.Width / 2) - 6;
            right_arrow.Y = RightScrollPanel.ClientArea.Top + (RightScrollPanel.ClientArea.Height / 2) - 6;
            right_arrow.Width = 12;
            right_arrow.Height = 12;


            DownScrollPanel = CreatePanel(playfield_panel.X, playfield_panel.Bottom + 1, playfield_panel.Right, frame.Bottom);
            DownScrollBtn = (PanelDynamicButton)DownScrollPanel.BindPanelControl(new Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelDynamicButton(playfield_panel.X, playfield_panel.Bottom + 1, playfield_panel.Right, frame.Bottom));
            DownScrollBtn.MouseDown = new MouseEventHandler(this.DownScroll);
            DownScrollBtn.MouseUp = new MouseEventHandler(this.StopScroll);
            down_arrow.X = DownScrollPanel.ClientArea.X + (DownScrollPanel.ClientArea.Width / 2) - 6;
            down_arrow.Y = DownScrollPanel.ClientArea.Top + (DownScrollPanel.ClientArea.Height / 2) - 6;
            down_arrow.Width = 12;
            down_arrow.Height = 12;


            UpScrollPanel = CreatePanel(playfield_panel.X, frame.Top, playfield_panel.Right, playfield_panel.Top - 1);
            UpScrollBtn = (PanelDynamicButton)UpScrollPanel.BindPanelControl(new Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelDynamicButton(playfield_panel.X, frame.Top, playfield_panel.Right, playfield_panel.Top - 1));
            UpScrollBtn.MouseDown = new MouseEventHandler(this.UpScroll);
            UpScrollBtn.MouseUp = new MouseEventHandler(this.StopScroll);
            up_arrow.X = UpScrollPanel.ClientArea.X + (UpScrollPanel.ClientArea.Width / 2) - 6;
            up_arrow.Y = UpScrollPanel.ClientArea.Top + (UpScrollPanel.ClientArea.Height / 2) - 6;
            up_arrow.Width = 12;
            up_arrow.Height = 12;
            #endregion


            foreach (string str in g.Textures.Keys)
            {
                if (str.CompareTo("MAP") != 0)
                {
                    Obj_Sprite s = CreateSprite(str, SpriteFlags.AlphaBlend);
                    s.Initialize(g.CANVAS);
                    s.Texture(g.Textures[str]);
                }
            }
                
            Map = CreateSprite("MAP", SpriteFlags.SortTexture | SpriteFlags.AlphaBlend);
            Map.Initialize(g.CANVAS);
            Map.Texture(g.GetTexture("MAP"));
            Map.Diffuse = Color.Gray; 



            MapScene = new MapPlayfield(g.CANVAS, Map, PlayfieldPanel.ClientArea, _commands);
            Playfield = (PanelSceneRegion)PlayfieldPanel.BindPanelControl(
                new Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelSceneRegion(playfield_panel.X, playfield_panel.Y, playfield_panel.Right, playfield_panel.Bottom, MapScene)
                );
            Playfield.Sticky = false;
            Playfield.MouseClick = new MouseEventHandler(MapScene.OnMouseClick);
            Playfield.MouseWheel = new MouseEventHandler(MapScene.OnMouseWheel);
            Playfield.MouseDown = new MouseEventHandler(MapScene.OnMouseDown);
            Playfield.MouseUp = new MouseEventHandler(MapScene.OnMouseUp);
            Playfield.MouseMove = new MouseEventHandler(MapScene.OnMouseMove);


            Map.SetPosition(PlayfieldPanel.ClientArea.X, PlayfieldPanel.ClientArea.Y, 0);
            MapScene.OnInitializeScene(g);


            if (!DDD_Global.Instance.nc.IsConnected())
            {
                ViewProMotionUpdate update = new ViewProMotionUpdate();

                update.ObjectId = "object1";
                update.Throttle = 1;
                update.StartX = 600;
                update.StartY = 600;
                update.StartZ = 0;
                update.PlayerColor = Color.White.ToArgb();
                update.Icon = "ImageLib.Persuade.Blue.BTTR.png";
                InitializeObjects(update);

                update.ObjectId = "object2";
                update.Throttle = 1;
                update.StartX = 630;
                update.StartY = 630;
                update.StartZ = 0;
                update.PlayerColor = Color.Green.ToArgb();
                update.Icon = "ImageLib.Persuade.Blue.CA.png";
                InitializeObjects(update);

                update.ObjectId = "object1b";
                update.Throttle = 1;
                update.StartX = 660;
                update.StartY = 660;
                update.StartZ = 0;
                update.PlayerColor = Color.Yellow.ToArgb();
                update.Icon = "ImageLib.Persuade.Blue.CH47.png";
                InitializeObjects(update);

                update.ObjectId = "object1c";
                update.Throttle = 1;
                update.StartX = 690;
                update.StartY = 690;
                update.StartZ = 0;
                update.PlayerColor = Color.Blue.ToArgb();
                update.Icon = "ImageLib.Persuade.Blue.CMBT-ENGR-CO.png";
                InitializeObjects(update);

                update.ObjectId = "object1d";
                update.Throttle = 1;
                update.StartX = 720;
                update.StartY = 720;
                update.StartZ = 0;
                update.PlayerColor = Color.Black.ToArgb();
                update.Icon = "ImageLib.Persuade.Blue.DSM-TRP.png";
                InitializeObjects(update);

                update.ObjectId = "object1e";
                update.Throttle = 1;
                update.StartX = 750;
                update.StartY = 750;
                update.StartZ = 0;
                update.PlayerColor = Color.Red.ToArgb();
                update.Icon = "ImageLib.Persuade.Blue.FA-BTTR.png";
                InitializeObjects(update);

                //update.ObjectId = update.OwnerID = update.PlayerId = "object2";
                //update.Throttle = 1;
                //update.StartX = 30;
                //update.StartY = 30;
                //update.StartZ = 0;
                //InitializeObjects(update);

                //update.ObjectId = update.OwnerID = update.PlayerId = "object3";
                //update.Throttle = 1;
                //update.StartX = 50;
                //update.StartY = 50;
                //update.StartZ = 0;
                //InitializeObjects(update);

                //update.ObjectId = update.OwnerID = update.PlayerId = "object4";
                //update.Throttle = 1;
                //update.StartX = 70;
                //update.StartY = 70;
                //update.StartZ = 0;
                //InitializeObjects(update);

                //update.ObjectId = update.OwnerID = update.PlayerId = "object5";
                //update.Throttle = 1;
                //update.StartX = 90;
                //update.StartY = 90;
                //update.StartZ = 0;
                //InitializeObjects(update);

              
                //MsgWindowPanel.AddText("UTM Northing units: " + UTM_Mapping.VerticalMetersPerPixel);
                //MsgWindowPanel.AddText("UTM Easting units: " + UTM_Mapping.HorizonalMetersPerPixel);
                MsgWindowPanel.AddText(DDD_Global.Instance.ScenarioDescription);
                //MsgWindowPanel.AddText("Demo Mode...");
            }
            else
            {
                //MsgWindowPanel.AddText("UTM Northing units: " + UTM_Mapping.VerticalMetersPerPixel);
                //MsgWindowPanel.AddText("UTM Easting units: " + UTM_Mapping.HorizonalMetersPerPixel);
                MsgWindowPanel.AddText(DDD_Global.Instance.ScenarioDescription);
                //MsgWindowPanel.AddText("Connected to server...");
            }

            ScoreHeader = string.Format("Scenario: {0}             Player: {1}", DDD_Global.Instance.ScenarioName, DDD_Global.Instance.PlayerID);
            _commands.HandshakeInitializeGUIDone(DDD_Global.Instance.PlayerID);

        }
                
        public override void OnRender(Canvas canvas)
        {
            Rectangle PlayfieldRect = PlayfieldPanel.ClientArea;
            Rectangle SpriteRect = Rectangle.Empty;
            if (Map != null)
            {
                SpriteRect = Map.ToRectangle();
            }

            switch (SCROLL)
            {
                case ScrollState.LEFT:
                    SpriteRect.X = (int)(SpriteRect.X + 10);
                    if (SpriteRect.Contains(PlayfieldRect))
                    {
                        MapScene.Pan(10, 0);
                    }
                    break;
                case ScrollState.RIGHT:
                    SpriteRect.X = (int)(SpriteRect.X - 10);
                    if (SpriteRect.Contains(PlayfieldRect))
                    {
                        MapScene.Pan(-10, 0);
                    }
                    break;
                case ScrollState.UP:
                    SpriteRect.Y = (int)(SpriteRect.Y + 10);
                    if (SpriteRect.Contains(PlayfieldRect))
                    {
                        MapScene.Pan(0, 10);
                    }
                    break;
                case ScrollState.DOWN:
                    SpriteRect.Y = (int)(SpriteRect.Y - 10);
                    if (SpriteRect.Contains(PlayfieldRect))
                    {
                        MapScene.Pan(0, -10);
                    }
                    break;
            }


            canvas.DrawFillRect(background, background_material);
            canvas.DrawFillRect(frame, frame_color_material);
            canvas.DrawRect(frame, border_color);
            canvas.DrawRect(PlayfieldPanel.ClientArea, Color.Green);
            canvas.DrawFillRect(score, score_color_material);
            canvas.DrawFillRect(information_rect, score_color_material);
            
            canvas.DrawFillTri(left_arrow, background_material, DIRECTION.LEFT);
            canvas.DrawFillTri(right_arrow, background_material, DIRECTION.RIGHT);
            canvas.DrawFillTri(up_arrow, background_material, DIRECTION.UP);
            canvas.DrawFillTri(down_arrow, background_material, DIRECTION.DOWN);

            if (MapScene.SelectionCount() == 0)
            {
                Mode = MenuMode.DEFAULT;
            }
            switch (Mode)
            {
                case MenuMode.DEFAULT:
                    DrawMoveMode(canvas);
                    break;
                case MenuMode.ATTACK:
                    DrawAttackMode(canvas);
                    break;
                case MenuMode.SUBPLATFORM:
                    DrawSubPMode(canvas);
                    break;
            }

            Fonts["Large"].DrawText(null, ScoreHeader, score, DrawTextFormat.VerticalCenter, Color.Yellow);
            if (!IsPaused)
            {
                Fonts["Large"].DrawText(null, Time, time_rect, DrawTextFormat.VerticalCenter, Color.Yellow);
            }
            else
            {
                Fonts["Large"].DrawText(null, "Paused", time_rect, DrawTextFormat.VerticalCenter, Color.Yellow);
            }


            base.OnRender(canvas);

        }

        
        public override void OnSceneCleanup(GameFramework g)
        {
            UnbindGameController();
            Fonts["Large"].Dispose();
            Fonts["Medium"].Dispose();
        }
        #endregion

        #region ScrollHandler
        public void StopScroll(object sender, MouseEventArgs e)
        {
            SCROLL = ScrollState.NONE;
            if (UpScrollBtn != null)
            {
                UpScrollBtn.Selected = false;
            }
            if (DownScrollBtn != null)
            {
                DownScrollBtn.Selected = false;
            }
            if (LeftScrollBtn != null)
            {
                LeftScrollBtn.Selected = false;
            }
            if (RightScrollBtn != null)
            {
                RightScrollBtn.Selected = false;
            }
            if (PlayfieldPanel != null)
            {
                PlayfieldPanel.Selected = true;
            }
        }
        public void LeftScroll(object sender, MouseEventArgs e)
        {
            SCROLL = ScrollState.LEFT;
        }
        public void RightScroll(object sender, MouseEventArgs e)
        {
            SCROLL = ScrollState.RIGHT;
        }
        public void UpScroll(object sender, MouseEventArgs e)
        {
            SCROLL = ScrollState.UP;
        }
        public void DownScroll(object sender, MouseEventArgs e)
        {
            SCROLL = ScrollState.DOWN;
        }
        #endregion

        #region DDD Update Handler
        public void Exit(object sender, MouseEventArgs e)
        {
            if (DDD_Global.Instance.nc != null)
            {
                if (DDD_Global.Instance.nc.IsConnected())
                {
                    DDD_Global.Instance.nc.Disconnect();
                }
            }

        }

        public void TextChatHandler(object obj, TextChatEventArgs e)
        {
            _commands.TextChatRequest(DDD_Global.Instance.PlayerID, e.Message, string.Empty, string.Empty);
        }

        public void RemoveObject(string object_id)
        {
            if (MapScene.ContainsMapObject(object_id))
            {
                //MsgWindowPanel.AddText(string.Format("RemoveObject: id = {0}", object_id));
                MapScene.RemoveMapObject(object_id);
            }
            AssetMenu.Reset();
            MiscAssetMenu.Reset();
            
        }

        public void TimeTick(string time)
        {
            Time = time;
        }

        public void SystemMessageUpdate(string message, int argbColor)
        {
            MsgWindowPanel.AddText(Color.FromArgb(argbColor), message);
        }

        public void MoveUpdateObjects(ViewProMotionUpdate update)
        {
            
            lock (this)
            {
                if (MapScene.ContainsMapObject(update.ObjectId))
                {
                    DDDObjects obj = (DDDObjects)MapScene.GetMappableObject(update.ObjectId);

                    obj.Hide = update.HideObject;

                    if (!DDD_Global.Instance.nc.IsConnected())
                    {
                        obj.Throttle = update.Throttle;
                        obj.MaxSpeed = 24;
                        MapScene.MoveMapObject(update.ObjectId, update.DestinationX, update.DestinationY, update.DestinationZ, ((float)(update.Throttle * obj.MaxSpeed * DDD_Global.Instance.GameSpeed)));
                    }
                    else
                    {
                        // Don't update throttle here, since it will always be 0.
                        obj.MaxSpeed = update.MaxSpeed;

                        MapScene.MoveMapObject(
                            update.ObjectId,
                            update.StartX,
                            update.StartY,
                            update.StartZ,
                            update.DestinationX,
                            update.DestinationY,
                            update.DestinationZ,
                            UTM_Mapping.VelocityToPixels((float)(update.Throttle * update.MaxSpeed * DDD_Global.Instance.GameSpeed)));
                        
                    }
                }
            }
        }
                
        public void AttributeUpdateObjects(ViewProAttributeUpdate update)
        {
            lock (this)
            {
                if (MapScene.ContainsMapObject(update.ObjectId))
                {
                    DDDObjects obj = (DDDObjects)MapScene.GetMappableObject(update.ObjectId);
                    if (obj != null)
                    {
                        if (update.IconName.Length > 0)
                        {
                            if (Sprites.ContainsKey(update.IconName))
                            {
                                obj.SetSprite(Sprites[update.IconName]);
                            }
                            else
                            {
                                obj.SetSprite(Sprites["ImageLib.Unknown.png"]);
                            }

                        }
                        if (update.CapabilityAndWeapons != null)
                        {
                            obj.CapabilityAndWeapons = update.CapabilityAndWeapons;
                        }
                        if (update.SubPlatforms != null)
                        {
                            obj.SubPlatforms = update.SubPlatforms;
                        }
                        if (update.Vulnerabilities != null)
                        {
                            obj.Vulnerabilities = update.Vulnerabilities;
                        }
                        if (update.FuelAmount > 0)
                        {
                            obj.FuelAmount = (float)update.FuelAmount;
                        }
                        if (update.FuelCapacity > 0)
                        {
                            obj.FuelCapacity = (float)update.FuelCapacity;
                        }
                        obj.PlayerID = update.PlayerId;

                        if (update.ClassName != null)
                        {
                            if (update.ClassName.Length > 0)
                            {
                                obj.ClassName = update.ClassName;
                            }
                        }
                        if (update.State.Length > 0)
                        {
                            obj.State = update.State;
                        }

                        if (obj.OwnerID != update.OwnerId)
                        {
                            obj.OwnerID = update.OwnerId;
                            MapScene.SwitchOwnership(obj.ObjectID);
                        }
                    }
                }
            }
        }

        public void ViewProStopObjectUpdate(string objectID)
        {
            if (MapScene.ContainsMapObject(objectID))
            {
                MapScene.GetMappableObject(objectID).StopPathCalculator();
            }
        }

        public void TextChatRequest(string user_id, string message, string target_id)
        {
            lock (this)
            {
                ChatWindowPanel.AddText(message);
            }
        }

        public void PauseGame()
        {
            IsPaused = true;
            MapScene.Pause();

        }

        public void ResumeGame()
        {
            IsPaused = false;
            MapScene.Resume();
        }

        public void AttackUpdate(string attacker, string target, int start_time, int end_time)
        {
            try
            {
                lock (this)
                {
                    DDDObjects att = (DDDObjects)MapScene.GetMappableObject(attacker);
                    DDDObjects obj = (DDDObjects)MapScene.GetMappableObject(target);
                    if (obj != null)
                    {
                        if ((end_time - start_time) > 0)
                        {
                            obj.IsBeingAttacked = true;
                            obj.EngagementTimer = (end_time - start_time) / 1000;
                            if (att != null)
                            {
                                att.EngagementTimer = obj.EngagementTimer;
                                att.IsAttacking = true;
                                obj.Attackers.Add(att);
                            }
                        }
                        else
                        {
                            obj.IsBeingAttacked = false;
                            obj.EngagementTimer = 0;
                            obj.Attackers.Clear();
                            if (att != null)
                            {
                                att.IsAttacking = false;
                            }
                        }
                    }
                }
            }

            catch (Exception e)
            {
                MessageBox.Show("AttackUpdate" + e.Message);
            }
        }
        #endregion

        
        #region Keyboard And Mouse Handlers
        public override void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    UpScroll(sender, null);
                    break;
                case Keys.Down:
                    DownScroll(sender, null);
                    break;
                case Keys.Left:
                    LeftScroll(sender, null);
                    break;
                case Keys.Right:
                    RightScroll(sender, null);
                    break;
                default:
                    base.OnKeyDown(sender, e);
                    break;
            }
        }

        public override void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1:
                    Mode = MenuMode.DEFAULT;
                    VulnerabilityMenu.ClearSelection();
                    break;
                case Keys.F2:
                    Mode = MenuMode.ATTACK;
                    AttackMenu.ClearSelection();
                    break;
                case Keys.F3:
                    Mode = MenuMode.SUBPLATFORM;
                    SubPlatformMenu.ClearSelection();
                    break;
                case Keys.Escape:
                    MapScene.DeselectAll();
                    MapScene.EnterMoveMode();
                    break;
                case Keys.L:
                    if (!e.Control)
                    {
                        goto default;
                    }
                    if ((Map.Diffuse.R + ColorStep) <= 255)
                    {
                        Map.Diffuse = Color.FromArgb(Map.Diffuse.R + ColorStep, Map.Diffuse.G + ColorStep, Map.Diffuse.B + ColorStep);
                    }
                    break;
                case Keys.D:
                    if (!e.Control)
                    {
                        goto default;
                    }
                    if ((Map.Diffuse.R - ColorStep) >= 0)
                    {
                        Map.Diffuse = Color.FromArgb(Map.Diffuse.R - ColorStep, Map.Diffuse.G - ColorStep, Map.Diffuse.B - ColorStep);
                    }
                    break;
                case Keys.Z:
                    if (!e.Control)
                    {
                        goto default;
                    }
                    MapScene.ZoomIn();
                    break;
                case Keys.X:
                    if (!e.Control)
                    {
                        goto default;
                    }
                    MapScene.ZoomOut();
                    break;
                case Keys.Up:
                    StopScroll(sender, null);
                    break;
                case Keys.Down:
                    StopScroll(sender, null);
                    break;
                case Keys.Left:
                    StopScroll(sender, null);
                    break;
                case Keys.Right:
                    StopScroll(sender, null);
                    break;
                default:
                    base.OnKeyUp(sender, e);
                    break;
            }
        }

        public override void OnMouseClick(object sender, MouseEventArgs e)
        {
            // Hack to catch chat window before Status panel.  Chat window can obscure statuspanel.
            if (!ChatWindow.ClientArea.Contains(e.Location))
            {
                if (StatusPanel.ClientArea.Contains(e.Location) && (Mode == MenuMode.DEFAULT) && !(StatusPanel.Hide))
                {
                    StatusPanel.OnMouseClick(sender, e);
                    return;
                }

                if (CancelAttackBtn.ClientArea.Contains(e.Location) && (Mode == MenuMode.ATTACK))
                {
                    CancelAttackBtn.OnMouseClick(sender, e);
                    return;
                }
                if (CancelSubPBtn.ClientArea.Contains(e.Location) && (Mode == MenuMode.SUBPLATFORM))
                {
                    CancelSubPBtn.OnMouseClick(sender, e);
                    return;
                }
            }


            base.OnMouseClick(sender, e);
        }

        public override void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (!IsPaused)
            {
                // Hack to catch chat window before Status panel.  Chat window can obscure statuspanel.
                if (!ChatWindow.ClientArea.Contains(e.Location))
                {

                    if (AssetMenu.ClientArea.Contains(e.Location) && (MapScene.Mode == MapModes.MOVE) && (!AssetPanel.Hide))
                    {
                        AssetPanel.OnMouseUp(sender, e);
                        return;
                    }
                    if (MiscAssetMenu.ClientArea.Contains(e.Location) && (MapScene.Mode == MapModes.MOVE) && (!AssetPanel.Hide))
                    {
                        AssetPanel.OnMouseUp(sender, e);
                        return;
                    }
                    if (AttackPanel.ClientArea.Contains(e.Location) && (Mode == MenuMode.ATTACK))
                    {
                        AttackPanel.OnMouseUp(sender, e);
                        return;
                    }
                    if (SubPlatformPanel.ClientArea.Contains(e.Location) && (Mode == MenuMode.SUBPLATFORM))
                    {
                        SubPlatformPanel.OnMouseUp(sender, e);
                        return;
                    }

                    if (StatusPanel.ClientArea.Contains(e.Location) && (MapScene.Mode == MapModes.MOVE))
                    {
                        StatusPanel.OnMouseUp(sender, e);
                        return;
                    }
                }
            }

            base.OnMouseUp(sender, e);

        }
        
        public override void OnMouseWheel(object sender, MouseEventArgs e)
        {
            base.OnMouseWheel(sender, e);
        }

        public override void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            base.OnKeyPress(sender, e);
        }

        public override void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            DDDObjects obj = MapScene.GetSelectedObject();

            if ((e.Button == MouseButtons.Left) && (obj != null))
            {
                if (obj.HitTest(e.X, e.Y))
                {
                    switch (Mode)
                    {
                        case MenuMode.DEFAULT:
                            Mode = MenuMode.ATTACK;
                            AttackMenu.ClearSelection();
                            break;
                        case MenuMode.ATTACK:
                            MapScene.Mode = MapModes.MOVE;
                            Mode = MenuMode.SUBPLATFORM;
                            SubPlatformMenu.ClearSelection();
                            break;
                        case MenuMode.SUBPLATFORM:
                            Mode = MenuMode.DEFAULT;
                            break;
                    }
                }
            }
            return;
        }

        public override void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (!IsPaused)
            {
                // Hack to catch chat window before Status panel.  Chat window can obscure statuspanel.
                if (!ChatWindow.ClientArea.Contains(e.Location))
                {
                    if (AssetMenu.ClientArea.Contains(e.Location) && (Mode == MenuMode.DEFAULT) && (!AssetPanel.Hide))
                    {
                        AssetPanel.OnMouseDown(sender, e);
                        return;
                    }
                    if (MiscAssetMenu.ClientArea.Contains(e.Location) && (Mode == MenuMode.DEFAULT) && (!AssetPanel.Hide))
                    {
                        AssetPanel.OnMouseDown(sender, e);
                        return;
                    }
                    if (AttackPanel.ClientArea.Contains(e.Location) && (Mode == MenuMode.ATTACK))
                    {
                        AttackPanel.OnMouseDown(sender, e);
                        return;
                    }
                    if (SubPlatformPanel.ClientArea.Contains(e.Location) && (Mode == MenuMode.SUBPLATFORM))
                    {
                        SubPlatformPanel.OnMouseDown(sender, e);
                        return;
                    }
                }
            }

            base.OnMouseDown(sender, e);
        }

        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            StopScroll(sender, e);
            base.OnMouseMove(sender, e);
        }

        #endregion

        #region ControlHandlers
        public void AssetHandler(int item, string itemstr)
        {
            Rectangle r = Rectangle.FromLTRB(MapScene.ClientArea.Left, MapScene.ClientArea.Top, MapScene.ClientArea.Right, MapScene.ClientArea.Bottom);
            r.Intersect(PlayfieldPanel.ClientArea);

            MapScene.Select(itemstr);
            DDDObjects obj = MapScene.GetSelectedObject();

            if (!r.Contains((int)obj.Position.X, (int)obj.Position.Y))
            {
                CenterMap(obj.Position.X, obj.Position.Y);
            }

            AssetMenu.Selected = false;
            AssetPanel.Selected = false;
            PlayfieldPanel.Selected = true;
        }
        public void AttackHandler(int item, string itemstr)
        {
            Console.WriteLine("{0}, {1}", item, itemstr);

                MapScene.GetSelectedObject().CurrentCapabilityAndWeapon = item;
                MapScene.EnterAttackMode();
        }

        public void SubPHandler(int item, string itemstr)
        {
            Console.WriteLine("{0}, {1}", item, itemstr);

                MapScene.GetSelectedObject().CurrentSubplatform = item;
                MapScene.EnterSubPlatformMode();
        }
        
        public void CancelAttackBtnHandler(object sender, MouseEventArgs e)
        {
            MapScene.EnterMoveMode();
            Mode = MenuMode.DEFAULT;

            CancelAttackBtn.Selected = false;
            PlayfieldPanel.Selected = true;
        }
        public void CancelSubPBtnHandler(object sender, MouseEventArgs e)
        {
            MapScene.EnterMoveMode();
            Mode = MenuMode.DEFAULT;

            CancelSubPBtn.Selected = false;
            PlayfieldPanel.Selected = true;
        }
        public void DeSelectBtnHandler(object sender, MouseEventArgs e)
        {
            MapScene.DeselectAll();
            DeSelectBtn.Selected = false;
            PlayfieldPanel.Selected = true;
        }
        public void SliderHandler(double slider_value)
        {
            DDDObjects obj = MapScene.GetSelectedObject();
            if (obj != null)
            {
                obj.Throttle = slider_value;
                obj.VelocityStr = "*" + obj.VelocityStr + "*";
            }
        }
        #endregion


        #region HeadsUpDisplay Support 
        /// <summary>
        /// Centers the currently displayed map around an X,Y position.
        /// Used when selecting assets from the Asset Menu ... Map recenters
        /// itself to the asset's location.
        /// </summary>
        /// <param name="obj_x"></param>
        /// <param name="obj_y"></param>
        public void CenterMap(float obj_x, float obj_y)
        {
            Rectangle PlayfieldRect = PlayfieldPanel.ClientArea;

            float new_x = (obj_x - ((PlayfieldRect.Width / 2))) * MapScene.Scale;
            float new_y = (obj_y - ((PlayfieldRect.Height / 2))) * MapScene.Scale;

            float max_extent_Y = (Map.TextureHeight * MapScene.Scale) - PlayfieldRect.Height;
            float max_extent_X = (Map.TextureWidth * MapScene.Scale) - PlayfieldRect.Width;
            float min_extent_Y = PlayfieldRect.Y;
            float min_extent_X = PlayfieldRect.X;

            if (new_x >= max_extent_X)
            {
                new_x = max_extent_X;
            }
            if (new_x <= min_extent_X)
            {
                new_x = 0;
            }
            if (new_y >= max_extent_Y)
            {
                new_y = max_extent_Y;
            }
            if (new_y <= min_extent_Y)
            {
                new_y = 0;
            }

            MapScene.SetMapPosition(PlayfieldRect.X, PlayfieldRect.Y);
            MapScene.Pan(-new_x, -new_y);
        }
       
        public void DrawAttackMode(Canvas c)
        {
            DDDObjects obj = MapScene.GetSelectedObject();

            if (obj != null)
            {
                AttackModeLbl.Text = obj.ObjectID;

                AttackHeaderLbl.SetPosition(AttackPanel.ClientArea.X, AttackModeLbl.ClientArea.Bottom + 12);

                AttackMenu.SetPosition(AttackPanel.ClientArea.X, AttackHeaderLbl.ClientArea.Bottom + 5);
                AttackMenu.LayoutMenuOptions(obj.CapabilityAndWeapons, PanelLayout.VerticalFree);
                if (obj.CapabilityAndWeapons != null)
                {
                    if (obj.CapabilityAndWeapons.Length == 0)
                    {
                        Fonts["Large"].DrawText(null, "No Weapons.", AttackMenu.ClientArea.X + 2, AttackMenu.ClientArea.Y + 2, Color.Yellow);
                    }
                }
                else
                {
                    Fonts["Large"].DrawText(null, "No Weapons.", AttackMenu.ClientArea.X + 2, AttackMenu.ClientArea.Y + 2, Color.Yellow);
                }

                CancelAttackBtn.SetPosition(AttackMenu.ClientArea.X, AttackMenu.ClientArea.Bottom + 12);
                CancelAttackBtn.SetWidth(AttackMenu.ClientArea.Width);
                CancelAttackBtn.SetHeight(25);

                AttackPanel.Hide = false;
                AttackPanel.OnRender(c);
            }
        }
        public void DrawSubPMode(Canvas c)
        {
            DDDObjects obj = MapScene.GetSelectedObject();

            if (obj != null)
            {
                SubPModeLbl.Text = obj.ObjectID;

                SubPHeaderLbl.SetPosition(SubPlatformPanel.ClientArea.X, SubPModeLbl.ClientArea.Bottom + 12);

                SubPlatformMenu.SetPosition(SubPlatformPanel.ClientArea.X, SubPHeaderLbl.ClientArea.Bottom + 5);
                SubPlatformMenu.LayoutMenuOptions(obj.SubPlatforms, PanelLayout.VerticalFree);
                if (obj.SubPlatforms != null)
                {
                    if (obj.SubPlatforms.Length == 0)
                    {
                        Fonts["Large"].DrawText(null, "No Subplatforms.", SubPlatformMenu.ClientArea.X + 2, SubPlatformMenu.ClientArea.Y + 2, Color.Yellow);
                    }
                }
                else
                {
                    Fonts["Large"].DrawText(null, "No Subplatforms.", SubPlatformMenu.ClientArea.X + 2, SubPlatformMenu.ClientArea.Y + 2, Color.Yellow);
                }

                CancelSubPBtn.SetPosition(SubPlatformMenu.ClientArea.X, SubPlatformMenu.ClientArea.Bottom + 12);
                CancelSubPBtn.SetWidth(SubPlatformMenu.ClientArea.Width);
                CancelSubPBtn.SetHeight(25);

                SubPlatformPanel.Hide = false;
                SubPlatformPanel.OnRender(c);
            }

        }

        public void DrawMoveMode(Canvas c)
        {
            DDDObjects obj = MapScene.GetSelectedObject();

            if (obj != null)
            {
                Throttle.Hide = false;
                Throttle.SliderPosition = obj.Throttle;

                ObjectIDLbl.Text = obj.ObjectID;
                ObjectClassLbl.Text = obj.ClassName;
                ObjectStateLbl.Text = obj.State;

                AltitudeLbl.Text = obj.AltitudeStr;

                if (obj.IsPathCalculatorRunning())
                {
                    LocationLbl.Text = "In Motion ...";
                    DestinationLbl.Text = obj.DestinationStr;
                    DestinationLbl.ForegroundColor = LocationLbl.ForegroundColor;
                    SpeedLbl.Text = obj.VelocityStr;
                    DestinationLbl.ForegroundColor = LocationLbl.ForegroundColor;
                    TtdLbl.Text = obj.TTDStr; 
                    TtdLbl.ForegroundColor = LocationLbl.ForegroundColor;
                }
                else
                {
                    LocationLbl.Text = obj.PositionStr;
                    DestinationLbl.Text = "-";
                    DestinationLbl.ForegroundColor = ObjectClassLbl.ForegroundColor;
                    TtdLbl.Text = "-";
                    TtdLbl.ForegroundColor = ObjectClassLbl.ForegroundColor;
                    SpeedLbl.Text = "-";
                    SpeedLbl.ForegroundColor = ObjectClassLbl.ForegroundColor;
                }

                MaxSpeedLbl.Text = obj.MaxSpeedStr;
                if (obj.FuelAmount <= 0)
                {
                    FuelAmountLbl.Text = "0%";
                    FuelAmountLbl.ProgressColor = Window.CaptionColor;
                    FuelAmountLbl.Progress = 0;
                }
                else
                {
                    FuelAmountLbl.Text = obj.FuelAmountStr;
                    FuelAmountLbl.ProgressColor = Window.CaptionColor;
                    FuelAmountLbl.Progress = obj.FuelAmount / obj.FuelCapacity;
                }
                if (obj.FuelCapacity <= 0)
                {
                    FuelCapacityLbl.Text = "0";
                }
                else
                {
                    FuelCapacityLbl.Text = obj.FuelCapacityStr;
                }
                StatusPanel.Hide = false;
                AssetPanel.Hide = true;

                if (obj.Vulnerabilities != null)
                {
                    if (obj.Vulnerabilities.Length == 0)
                    {
                        VulnerabilityMenu.LayoutMenuOptions(_no_vulnerabilities, PanelLayout.VerticalFree);
                    }
                    else
                    {
                        VulnerabilityMenu.LayoutMenuOptions(obj.Vulnerabilities, PanelLayout.VerticalFree);
                    }
                }
                else
                {
                    VulnerabilityMenu.LayoutMenuOptions(_no_vulnerabilities, PanelLayout.VerticalFree);
                }
                StatusPanel.OnRender(c);
            }
            else
            {
                AssetMenu.ClearSelection();
                MiscAssetMenu.ClearSelection();
                StatusPanel.Hide = true;
                DeSelectBtn.Selected = false;
                AssetHeaderLbl.SetPosition(AssetPanel.ClientArea.X, AssetModeLbl.ClientArea.Bottom + 12);
                AssetMenu.SetPosition(AssetPanel.ClientArea.X, AssetHeaderLbl.ClientArea.Bottom + 5);
                AssetMenu.LayoutMenuOptions(MapScene.Assets, PanelLayout.VerticalFree);

                MiscAssetHeaderLbl.SetPosition(AssetPanel.ClientArea.X, AssetMenu.ClientArea.Bottom + 12);
                MiscAssetMenu.SetPosition(AssetPanel.ClientArea.X, MiscAssetHeaderLbl.ClientArea.Bottom + 5);
                MiscAssetMenu.LayoutMenuOptions(MapScene.MiscAssets, PanelLayout.VerticalFree);

                AssetPanel.Hide = false;
                AssetPanel.OnRender(c);
            }

        }


        #endregion

    }
}
