using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;


namespace Aptima.Asim.DDD.Client.Common.GLCore
{
    public partial class CanvasOptions
    {
        private bool _bWindowed;
        public bool Windowed
        {
            get
            {
                return _bWindowed;
            }
            set
            {
                this._bWindowed = value;
            }
        }

        private ShadeMode _shader;
        public ShadeMode Shader
        {
            get
            {
                return _shader;
            }
            set
            {
                _shader = value;
            }
        }

        private Cull _culling;
        public Cull BackfaceCulling
        {
            get
            {
                return _culling;
            }
            set
            {
                _culling = value;
            }
        }

        private Color _background;
        public Color BackgroundColor
        {
            get
            {
                return _background;
            }
            set
            {
                _background = value;
            }
        }

        private Color _ambient;
        public Color AmbientColor
        {
            get
            {
                return _ambient;
            }
            set
            {
                _ambient = value;
            }
        }

        private DeviceType _device;
        public DeviceType Device
        {
            get
            {
                if (_device == DeviceType.NullReference)
                {
                    _device = DeviceType.Software;
                }
                return _device;
            }
            set
            {
                _device = value;
            }
        }

        public CanvasOptions()
        {
            this._bWindowed = true;
            this._shader = ShadeMode.Flat;
            this._culling = Cull.None;
            this._background = Color.Black;
            this._ambient = Color.White;
            this._device = DeviceType.Software;

        }
        

    }
}
