using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using AGT.Sprites;

namespace AGT.GameToolkit
{

    public sealed class AGT_Pawn: ICloneable
    {
        private int _texture_width;
        private int _texture_height;
        private Rectangle _collision_rect = Rectangle.Empty;

        private Vector3 _position;
        private Vector3 _rotation;
        private Vector3 _scaling;

        public Vector3 Position
        {
            get
            {
                lock (this)
                {
                    return _position;
                }
            }
        }

        public Vector3 Rotation
        {
            get
            {
                lock (this)
                {
                    return _rotation;
                }
            }
        }

        public Vector3 Scaling
        {
            get
            {
                lock (this)
                {
                    return _scaling;
                }
            }
        }

        private string _pawn_id;
        public string Id
        {
            get
            {
                lock (this)
                {
                    return _pawn_id;
                }
            }
        }

        private AGT_SpriteId _sprite_id;
        public AGT_SpriteId SpriteId
        {
            get
            {
                lock (this)
                {
                    return _sprite_id;
                }
            }
        }

        public int Width
        {
            get
            {
                lock (this)
                {
                    return _texture_width;
                }
            }
        }
        public int Height
        {
            get
            {
                return _texture_height;
            }
        }

        public int ScaledWidth
        {
            get
            {
                lock (this)
                {
                    return (int)(_texture_width * _scaling.X);
                }
            }
        }
        public int ScaledHeight
        {
            get
            {
                lock (this)
                {
                    return (int)(_texture_height * _scaling.Y);
                }
            }
        }

        public int ScaledBottom
        {
            get
            {
                lock (this)
                {
                    return (int)((_texture_height * _scaling.Y) + _position.Y);
                }
            }
        }
        public int ScaledRight
        {
            get
            {
                lock (this)
                {
                    return (int)((_texture_width * _scaling.X) + _position.X);
                }
            }
        }

        public static AGT_Pawn Empty = new AGT_Pawn();

        public bool HitTest(int mouse_x, int mouse_y)
        {
            return _collision_rect.Contains(mouse_x, mouse_y);
        }

        public bool RelativeHitTest(int mouse_x, int mouse_y, int offset_x, int offset_y)
        {
            Rectangle collision_rect = _collision_rect;
            collision_rect.X += offset_x;
            collision_rect.Y += offset_y;
            return collision_rect.Contains(mouse_x, mouse_y);
        }


        public AGT_Pawn()
        {
            _sprite_id = new AGT_SpriteId();
            _pawn_id = string.Empty;
            _position = _rotation = Vector3.Empty;
            _scaling = new Vector3(1, 1, 1);
            _collision_rect = Rectangle.Empty;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public static bool operator ==(AGT_Pawn lhs, AGT_Pawn rhs)
        {
            if ((lhs.Position == rhs.Position) &&
                (lhs.Rotation == lhs.Rotation) &&
                (lhs.Id == rhs.Id) &&
                (lhs.Height == rhs.Height) &&
                (lhs.Scaling == rhs.Scaling) &&
                (lhs.SpriteId == rhs.SpriteId))
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(AGT_Pawn lhs, AGT_Pawn rhs)
        {
            if ((lhs.Position == rhs.Position) &&
                (lhs.Rotation == lhs.Rotation) &&
                (lhs.Id == rhs.Id) &&
                (lhs.Height == rhs.Height) &&
                (lhs.Scaling == rhs.Scaling) &&
                (lhs.SpriteId == rhs.SpriteId))
            {
                return false;
            }
            return true;
        }


        public AGT_Pawn(string pawn_id, AGT_SpriteResource texture)
        {
            _position = new Vector3(0, 0, 0);
            _rotation = new Vector3(0, 0, 0);
            _scaling = new Vector3(1, 1, 1);
            _pawn_id = pawn_id;
            _sprite_id.Id = texture.TextureId;
            _collision_rect.Height = _texture_height = texture.Height;
            _collision_rect.Width = _texture_width = texture.Width;
        }

        public void SetSpriteReference(AGT_SpriteResource texture)
        {
            _sprite_id.Id = texture.TextureId;
            _collision_rect.Height = _texture_height = texture.Height;
            _collision_rect.Width = _texture_width = texture.Width;
        }
        public void SetPosition(Vector3 position)
        {
            SetPosition(position.X, position.Y, position.Z);
        }
        public void SetPosition(float x, float y, float z)
        {
            lock (this)
            {
                _position.X = x;
                _position.Y = y;
                _position.Z = z;
                _collision_rect.X = (int)(x - (ScaledWidth * .5));
                _collision_rect.Y = (int)(y - (ScaledHeight * .5));
            }
        }

        public void SetRotation(Vector3 rotation)
        {
            SetPosition(rotation.X, rotation.Y, rotation.Z);
        }
        public void SetRotation(float x, float y, float z)
        {
            lock (this)
            {
                _rotation.X = x;
                _rotation.Y = y;
                _rotation.Z = z;
            }
        }

        public void SetScaling(Vector3 scaling)
        {
            SetScaling(scaling.X, scaling.Y, scaling.Z);
        }
        public void SetScaling(float x, float y, float z)
        {
            lock (this)
            {
                _scaling.X = x;
                _scaling.Y = y;
                _scaling.Z = z;
                _collision_rect.Width = ScaledWidth;
                _collision_rect.Height = ScaledHeight;
            }
        }

        #region ICloneable Members

        public object Clone()
        {
            AGT_Pawn p = new AGT_Pawn();

            p._pawn_id = this.Id;
            p._sprite_id = this._sprite_id;
            p._position = this.Position;
            p._rotation = this.Rotation;
            p._scaling = this.Scaling;
            p._texture_height = this.Height;
            p._texture_width = this.Width;

            p._collision_rect.X = (int)this.Position.X;
            p._collision_rect.Y = (int)this.Position.Y;
            p._collision_rect.Height = this._texture_height;
            p._collision_rect.Width = this._texture_width;

            return (object)p;
        }

        #endregion
    }
}
