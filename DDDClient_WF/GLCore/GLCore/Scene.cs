using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Aptima.Asim.DDD.Client.Common.GLCore.CoreObjects;

namespace Aptima.Asim.DDD.Client.Common.GLCore
{
    public class Scene: IDeviceInput
    {
        public enum MODE:int {SCENE_LOAD = 0, SCENE_RENDER  = 1, SCENE_FINISHED = 2, SCENE_ERROR = 3};


        protected Dictionary<string, Obj_Sprite> Sprites = new Dictionary<string, Obj_Sprite>();
        protected Dictionary<string, Microsoft.DirectX.Direct3D.Font> Fonts = new Dictionary<string, Microsoft.DirectX.Direct3D.Font>();

        private int _current_zoom_level = 8;
        protected float[] _zoom_levels = { .15f, .25f, .35f, .45f, .55f, .65f, .75f, .85f, 1 };
        private float _scale = 1;
        public float Scale
        {
            get
            {
                //return _zoom_levels[_current_zoom_level];
                return _scale;
            }
            set
            {
                _scale = value;
            }
        }
        public float MinScale
        {
            get
            {
                return _zoom_levels[0];
            }
        }
        private float _maxZoom = 3;
        public float MaxZoom
        {
            get
            {
                return _maxZoom;
            }
            //set
            //{
            //    _maxZoom = value;
            //}
        }
        public MODE SceneMode = MODE.SCENE_LOAD;

        protected bool _initialized;
        public bool Initialized
        {
            get
            {
                return _initialized;
            }
        }


        protected Camera ViewCamera;

        private Matrix _perspective = Matrix.Zero;
        public Matrix Perspective
        {
            get
            {
                return _perspective;
            }
        }

        protected void CreatePerspectiveProjection(float field_of_view, float aspect_ratio, float near, float far)
        {
            _perspective = Matrix.PerspectiveFovLH(field_of_view, aspect_ratio, near, far);
        }
        protected void CreateOrthoProjection(float width, float height, float near, float far)
        {
            _perspective = Matrix.OrthoLH(width, height, near, far);
        }

        protected Obj_Sprite CreateSprite(string name, SpriteFlags flags)
        {
            int index = Sprites.Count;
            Sprites.Add(name, new Obj_Sprite(SpriteFlags.SortTexture | SpriteFlags.AlphaBlend));
            return Sprites[name];
        }
 

        public Scene() {
            _initialized = false;
            ViewCamera = new Camera();
        }
        public Matrix GetCameraView()
        {
            return ViewCamera.LookAtLH();
        }

        public void Initialize(GameFramework g)
        {
            if (!_initialized)
            {
                OnInitializeSceneProjection(g);
                g.CANVAS.MtxProjection = _perspective;

                OnInitializeCameraView();
                g.CANVAS.MtxView = GetCameraView();

                OnInitializeScene(g);

                _initialized = true;
            }
        }

        public void ZoomIn()
        {
            if (_current_zoom_level < (_zoom_levels.Length - 1))
            {
                _current_zoom_level++;
            }

            if (!OnZoom())
            {
                _current_zoom_level--;
            }

        }
        public void ZoomOut()
        {
            if (_current_zoom_level > 0)
            {
                _current_zoom_level--;
            }

            if (!OnZoom())
            {
                _current_zoom_level ++;
            }
        }
        public void Zoom(int magnification_level)
        {
            if (magnification_level > _zoom_levels.Length - 1)
            {
                _current_zoom_level = _zoom_levels.Length - 1;
                return;
            }
            if (magnification_level < 0)
            {
                _current_zoom_level = 0;
                return;
            }
            _current_zoom_level = magnification_level;
        }

        public int GetMagnificationLevel()
        {
            return _current_zoom_level;
        }
        public void Pan(float xpos, float ypos)
        {
            OnPan(xpos, ypos);
        }



        public virtual void OnInitializeSceneProjection(GameFramework g)
        {
        }
        public virtual void OnInitializeCameraView()
        {
        }
        public virtual void OnInitializeScene(GameFramework g)
        {
        }
        public virtual void OnSceneLoading(GameFramework g)
        {
        }
        
        public virtual void OnBeforeRender(GameFramework g)
        {
        }
        public virtual void OnRender(Canvas canvas)
        {
        }
        public virtual void OnAfterRender(GameFramework g)
        {
        }

        public virtual void OnSceneCleanup(GameFramework g)
        {
        }


        public virtual bool OnZoom()
        {
            return true;
        }
        public virtual void OnZoom(int magnification_level)
        {
        }


        protected virtual void OnPan(float xpos, float ypos)
        {
        }

        

        #region IDeviceInput Members

        public virtual void OnMouseClick(object sender, MouseEventArgs e)
        {
        }

        public virtual void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        public virtual void OnMouseDown(object sender, MouseEventArgs e)
        {
        }

        public virtual void OnMouseMove(object sender, MouseEventArgs e)
        {
        }

        public virtual void OnMouseUp(object sender, MouseEventArgs e)
        {
        }

        public virtual void OnMouseWheel(object sender, MouseEventArgs e)
        {
        }

        public virtual void OnKeyDown(object sender, KeyEventArgs e)
        {
        }

        public virtual void OnKeyPress(object sender, KeyPressEventArgs e)
        {
        }

        public virtual void OnKeyUp(object sender, KeyEventArgs e)
        {
        }

        #endregion
    }
}
