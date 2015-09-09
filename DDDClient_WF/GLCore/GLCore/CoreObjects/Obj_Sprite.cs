using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Aptima.Asim.DDD.Client.Common.GLCore.CoreObjects
{
    /// <summary>
    /// GLCore Sprite primative.  A Sprite is a DirectX Bitmap object, differs 
    /// from a textured plane in that this is strictly 2D and requires no Mesh data.
    /// </summary>
    public class Obj_Sprite: BaseGameObject
    {

        /// <summary>
        /// Sets the Diffuse color or Tint of the object's bitmap.
        /// </summary>
        public Color Diffuse
        {
            get
            {
                return _diffuse_color;
            }
            set
            {
                _diffuse_color = value;
            }
        }
        /// <summary>
        /// Center the object.
        /// </summary>
        public bool Centered = false;
        /// <summary>
        /// Gets/set the DirectX sprite flags for rendering. See DirectX Docs for
        /// more info on SpriteFlags.
        /// </summary>
        public SpriteFlags Flags
        {
            get
            {
                return _flags;
            }
            set
            {
                _flags = value;
            }
        }
        /// <summary>
        /// Gets the width of the texture.
        /// </summary>
        public float TextureWidth
        {
            get
            {
                return _texture_width;
            }
        }
        /// <summary>
        /// Gets the height of the texture.
        /// </summary>
        public float TextureHeight
        {
            get
            {
                return _texture_height;
            }
        }



        #region Private Members
        private RectangleF _internal_rect = RectangleF.Empty;
        private string _filename;
        private SpriteFlags _flags;
        private int _texture_width;
        private int _texture_height;
        private float _XScaleCoeff;
        private float _YScaleCoeff;
        /// <summary>
        /// DirectX sprite object, see DirectX Docs for more info.
        /// </summary>
        private Sprite _sprite;
        /// <summary>
        /// DirectX Texture object, see DirectX Docs for more info.
        /// </summary>
        private Texture _texture;
        /// <summary>
        /// Tints, the object with this select color upon selection.
        /// </summary>
        private Color _diffuse_color;
        private Vector3 Zrotation = new Vector3(0, 0, 1);
        private Vector3 center = Vector3.Empty;
        #endregion

        public Obj_Sprite(string filename, SpriteFlags flags)
        {
            _filename = filename;
            _flags = flags;
            ClearPosition();
        }
        public Obj_Sprite(SpriteFlags flags)
        {
            _filename = string.Empty;
            _flags = flags;
            ClearPosition();
        }

        public override void SetPosition(float xpos, float ypos, float zpos)
        {
            base.SetPosition(xpos, ypos, zpos);
            if (!Centered)
            {
                _internal_rect.X = Position.X;
                _internal_rect.Y = Position.Y;
            }
            else
            {
                _internal_rect.X = (Position.X - (Scaling.X * TextureWidth / 2));
                _internal_rect.Y = (Position.Y - (Scaling.Y * TextureHeight / 2));
            }
        }
        public Rectangle ToRectangle()
        {
            return Rectangle.Truncate(_internal_rect);
        }
        public RectangleF ToRectangleF()
        {
            return _internal_rect;
        }

        public override void Initialize(Canvas canvas)
        {
            _sprite = canvas.CreateSprite();
            _diffuse_color = Color.White;
        }


        public override void Texture(Canvas canvas)
        {
            int hardware_width;
            int hardware_height;

            if (_filename.Length > 0)
            {
                _texture = canvas.LoadTexture(_filename, out _texture_width, out _texture_height);
                _internal_rect.Width = _texture_width;
                _internal_rect.Height = _texture_height;

                using (Surface s = _texture.GetSurfaceLevel(0))
                {
                    hardware_height = s.Description.Height;
                    hardware_width = s.Description.Width;
                }
                _XScaleCoeff = (float)_texture_width / (float)hardware_width;
                _YScaleCoeff = (float)_texture_height / (float)hardware_height;
                SetScale(1, 1, 1);

            }
        }

        public override void Texture(Texture texture, int width, int height)
        {
            int hardware_width;
            int hardware_height;

            if (texture != null)
            {
                _texture = texture;
                _texture_width = width;
                _texture_height = height;

                using (Surface s = _texture.GetSurfaceLevel(0))
                {
                    hardware_height = s.Description.Height;
                    hardware_width = s.Description.Width;
                }

                _XScaleCoeff = (float)_texture_width / (float)hardware_width;
                _YScaleCoeff = (float)_texture_height / (float)hardware_height;

                SetScale(1, 1, 1);

            }
        }





        public override void SetScale(float pct_xScale, float pct_yScale, float pct_zScale)
        {
            // Hardware changes aspect ratio, therefore to display properly we must multiply
            // by the appropriate ScaleCoeff to maintain original aspect ratio.
            base.SetScale(_XScaleCoeff * pct_xScale,  _YScaleCoeff * pct_yScale, pct_zScale);

            _internal_rect.Width = (int)(TextureWidth * (pct_xScale));
            _internal_rect.Height = (int)(TextureHeight * (pct_yScale));

        }

        public void DrawIcon(Canvas canvas, float x, float y)
        {
            _sprite.Begin(this.Flags);

            _sprite.Transform = Matrix.Scaling(Scaling.X, Scaling.Y, Scaling.Z) *
                            Matrix.Translation(x, y, 0); 

            _sprite.Draw(_texture, Vector3.Empty, Vector3.Empty, Color.White.ToArgb());
            
            _sprite.End();
        }


        public override void Draw(Canvas canvas)
        {
            _sprite.Begin(this.Flags);

            // Sprite is 2 dimensional, only concerned with rotations about the Z access.
            if (Rotation.Z > 0)
            {
                // To rotate: Rotation is performed about the sprite origin (0,0).  To rotate the sprite about its CENTER 
                //   rotation we must translate the sprite to the origin (0,0).  Perform the rotation, then translate it
                //   to its original position.

                // Move sprite so its center is over point 0,0.
                _sprite.Transform = Matrix.Translation(-(TextureWidth / 2),  -(TextureHeight / 2), 0) *
                                           Matrix.RotationAxis(Zrotation, Rotation.Z);

                // Move sprite back to original position and finish scaling and translating.
                _sprite.Transform *= Matrix.Translation((TextureWidth / 2), (TextureHeight / 2), 0) *
                                           Matrix.Scaling(Scaling.X, Scaling.Y, Scaling.Z) * 
                                           Matrix.Translation(_internal_rect.X, _internal_rect.Y, Position.Z);
            }
            else
            {
                // If no rotation required, simplify the translation, just scale and position.
                _sprite.Transform = Matrix.Scaling(Scaling.X, Scaling.Y, Scaling.Z) * 
                                            Matrix.Translation(_internal_rect.X, _internal_rect.Y, Position.Z); 
            }

            _sprite.Draw(_texture, Vector3.Empty, Vector3.Empty, _diffuse_color.ToArgb());
            _sprite.End();
        }

        public void Draw(Canvas canvas, Rectangle clip_area)
        {
            _sprite.Begin(this.Flags);

            // Sprite is 2 dimensional, only concerned with rotations about the Z access.
            if (Rotation.Z > 0)
            {
                // To rotate: Rotation is performed about the sprite origin (0,0).  To rotate the sprite about its CENTER 
                //   rotation we must translate the sprite to the origin (0,0).  Perform the rotation, then translate it
                //   to its original position.

                // Move sprite so its center is over point 0,0.
                _sprite.Transform = Matrix.Translation(-(TextureWidth / 2), -(TextureHeight / 2), 0) *
                                           Matrix.RotationAxis(Zrotation, Rotation.Z);

                // Move sprite back to original position and finish scaling and translating.
                _sprite.Transform *= Matrix.Translation((TextureWidth / 2), (TextureHeight / 2), 0) *
                                           Matrix.Scaling(Scaling.X, Scaling.Y, Scaling.Z) *
                                           Matrix.Translation(_internal_rect.X, _internal_rect.Y, Position.Z);
            }
            else
            {
                // If no rotation required, simplify the translation, just scale and position.
                _sprite.Transform = Matrix.Scaling(Scaling.X, Scaling.Y, Scaling.Z) *
                                            Matrix.Translation(_internal_rect.X, _internal_rect.Y, Position.Z);
            }

            _sprite.Draw(_texture,  clip_area, Vector3.Empty, Vector3.Empty, _diffuse_color.ToArgb());
            _sprite.End();
        }


    }
}
