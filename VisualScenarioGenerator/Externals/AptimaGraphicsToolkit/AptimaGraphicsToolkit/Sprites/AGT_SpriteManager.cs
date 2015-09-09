using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using System.Reflection;

using AGT.GameToolkit;

namespace AGT.Sprites
{
    public struct AGT_SpriteId
    {
        public string Id;
        public static AGT_SpriteId Empty = new AGT_SpriteId(string.Empty);

        public AGT_SpriteId(string id)
        {
            Id = id;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            AGT_SpriteId s = (AGT_SpriteId)obj;
            return ((s.Id == Id));
        }
        public static bool operator ==(AGT_SpriteId lhs, AGT_SpriteId rhs)
        {
            return (lhs.Id == rhs.Id);
        }

        public static bool operator !=(AGT_SpriteId lhs, AGT_SpriteId rhs)
        {
            return (lhs.Id != rhs.Id);
        }
    }


    public class AGT_SpriteManager: IDisposable
    {
        private Device _device_;
        private RectangleF _sprite_rectangle = RectangleF.Empty;

        private Sprite _sprite_resource = null;
        private Dictionary<string, AGT_SpriteResource> _texture_list = new Dictionary<string, AGT_SpriteResource>();

        private AGT_SpriteId _unknown_id = AGT_SpriteId.Empty;

        public int Count
        {
            get
            {
                return _texture_list.Count;
            }
        }

        public AGT_SpriteManager(Device d)
        {
            if (d == null)
            {
                throw new ArgumentNullException("Null device passed");
            }
            _device_ = d;
            _sprite_resource = new Sprite(d);
            using (Bitmap b = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("AGT.images.Unknown.png")))
            {
                _unknown_id = AddResource("AGT.images.Unknown.png", b, 0, 0, 0);
                SetCenter(_unknown_id, (float)(b.Width * .5f), (float)(b.Height * .5f), 0);
            }

        }

        public float X = 0;
        public float Y = 0;
        public float Scale = 1f;

        public AGT_SpriteId AddResource(string id, string file, float x, float y, float z, bool rotate)
        {
            if (System.IO.File.Exists(file))
            {
                AGT_SpriteResource t = new AGT_SpriteResource();
                t.TextureId = id;
                t.CanRotate = rotate;
                t.Texture = TextureLoader.FromFile(_device_, file);
                t.Position.X = x;
                t.Position.Y = y;
                t.Position.Z = z;
                using (Surface s = t.Texture.GetSurfaceLevel(0))
                {
                    t.Width = s.Description.Width;
                    t.Height = s.Description.Height;
                }
                using (Image b = Bitmap.FromFile(file))
                {
                    t.Aspect.X = (float)b.Width / (float)t.Width;
                    t.Aspect.Y = (float)b.Height / (float)t.Height;
                    t.BitmapWidth = b.Width;
                    t.BitmapHeight = b.Height;
                }
                lock (this)
                {
                    if (!_texture_list.ContainsKey(id))
                    {
                        _texture_list.Add(id, t);
                    }
                }
                return new AGT_SpriteId(id);
            }
            else
            {
                throw new ArgumentNullException("Null bitmap passed");
            }
        }

        public AGT_SpriteId AddResource(string id, System.Drawing.Bitmap bitmap, float x, float y, float z, bool rotate)
        {
            if (bitmap != null)
            {
                AGT_SpriteResource t = new AGT_SpriteResource();
                t.TextureId = id;
                t.CanRotate = rotate;
                t.Texture = Texture.FromBitmap(_device_, bitmap, Usage.None, Pool.Managed);
                t.Position.X = x;
                t.Position.Y = y;
                t.Position.Z = z;
                t.Aspect.X = ((float)bitmap.Width / (float)bitmap.Height);
                t.Aspect.Y = ((float)bitmap.Width / (float)bitmap.Height);
                t.BitmapHeight = bitmap.Height;
                t.BitmapWidth = bitmap.Width;
                using (Surface s = t.Texture.GetSurfaceLevel(0))
                {
                    t.Width = s.Description.Width;
                    t.Height = s.Description.Height;
                }

                lock (this)
                {
                    if (!_texture_list.ContainsKey(id))
                    {
                        _texture_list.Add(id, t);
                    }
                }
                return new AGT_SpriteId(id);
            }
            else
            {
                throw new ArgumentNullException("Null bitmap passed");
            }
        }

        public AGT_SpriteId AddResource(string id, System.Drawing.Bitmap bitmap, float x, float y, float z)
        {
            return AddResource(id, bitmap, x, y, z, false);
        }

        public AGT_SpriteId AddResource(string id, AGT_PointList pointlist)
        {
            return AddResource(id, pointlist.ToBitmap(_device_), pointlist.Location.X + Math.Abs(X), pointlist.Location.Y + Math.Abs(Y), 0);
        }
        public AGT_SpriteResource GetTextureDefinition(AGT_SpriteId id)
        {
            lock (this)
            {
                if (IsValidId(id))
                {
                    return new AGT_SpriteResource(_texture_list[id.Id]);
                }
                return new AGT_SpriteResource(_texture_list[_unknown_id.Id]);
            }
        }
        public int GetTextureWidth(AGT_SpriteId id)
        {
            lock (this)
            {
                if (IsValidId(id))
                {
                    return _texture_list[id.Id].Width;
                }
                return -1;
            }
        }
        public int GetTextureHeight(AGT_SpriteId id)
        {
            lock (this)
            {
                if (IsValidId(id))
                {
                    return _texture_list[id.Id].Height;
                }
                return -1;
            }
        }
        public int GetBitmapWidth(AGT_SpriteId id)
        {
            lock (this)
            {
                if (IsValidId(id))
                {
                    return _texture_list[id.Id].BitmapWidth;
                }
                return -1;
            }
        }
        public int GetBitmapHeight(AGT_SpriteId id)
        {
            lock (this)
            {
                if (IsValidId(id))
                {
                    return _texture_list[id.Id].BitmapHeight;
                }
                return -1;
            }
        }

        private bool IsValidId(AGT_SpriteId id)
        {
            return _texture_list.ContainsKey(id.Id);
        }
        public void SetRotation(AGT_SpriteId id, float rx, float ry, float rz)
        {
            lock (this)
            {
                if (IsValidId(id))
                {
                    _texture_list[id.Id].Rotation.X = rx;
                    _texture_list[id.Id].Rotation.Y = ry;
                    _texture_list[id.Id].Rotation.Z = rz;
                }
            }
        }
        public void SetRotation(AGT_SpriteId id, Vector3 rotation)
        {
            SetRotation(id, rotation.X, rotation.Y, rotation.Z);
        }
        public void SetTextureScale(AGT_SpriteId id, float sx, float sy, float sz)
        {
            lock (this)
            {
                if (IsValidId(id))
                {
                    _texture_list[id.Id].SetScale(sx, sy);
                }
            }
        }
        public void SetTexutureScale(AGT_SpriteId id, Vector3 scale)
        {
            SetTextureScale(id, scale.X, scale.Y, scale.Z);
        }
        public void SetPosition(AGT_SpriteId id, float x, float y, float z)
        {
            lock (this)
            {
                if (IsValidId(id))
                {
                    _texture_list[id.Id].Position.X = x;
                    _texture_list[id.Id].Position.Y = y;
                    _texture_list[id.Id].Position.Z = z;
                }
            }
        }

        public void SetPosition(AGT_SpriteId id, Vector3 position)
        {
            SetPosition(id, position.X, position.Y, position.Z);
        }
        public void SetCenter(AGT_SpriteId id, float x, float y, float z)
        {
            lock (this)
            {
                if (IsValidId(id))
                {
                    _texture_list[id.Id].Center.X = x;
                    _texture_list[id.Id].Center.Y = y;
                    _texture_list[id.Id].Center.Z = z;
                }
            }
        }
        public void SetCenter(AGT_SpriteId id, Vector3 center)
        {
            SetCenter(id, center.X, center.Y, center.Z);
        }

        public bool CanRotate(AGT_SpriteId id)
        {
            lock (this)
            {
                if (IsValidId(id))
                {
                    return _texture_list[id.Id].CanRotate;
                }
                return false;
            }
        }
        public void Remove(AGT_SpriteId id)
        {
            if (IsValidId(id))
            {
                lock (this)
                {
                    _texture_list.Remove(id.Id);
                }
            }
        }
        public void ClearAll()
        {
            lock (this)
            {
                _texture_list.Clear();
            }
        }

        public void BatchDraw(SpriteFlags flags)
        {
            _sprite_resource.Begin(flags);
            lock (this)
            {
                foreach (AGT_SpriteResource t in _texture_list.Values)
                {
                    t.DrawRelative(_sprite_resource, Scale, X, Y, 0, Color.White.ToArgb());
                }
            }
            _sprite_resource.End();
        }

        public void Begin(SpriteFlags flags)
        {
            _sprite_resource.Begin(flags);
        }
        public void Draw(AGT_SpriteId id, Color diffuse)
        {
            if (_texture_list.ContainsKey(id.Id))
            {
                _texture_list[id.Id].DrawRelative(_sprite_resource, Scale, X, Y, 0, diffuse.ToArgb());
            }
            else
            {
                _texture_list[_unknown_id.Id].DrawRelative(_sprite_resource, Scale, X, Y, 0, diffuse.ToArgb());
            }
        }
        public void End()
        {
            _sprite_resource.End();
        }

        public void DumpToConsole()
        {
            Console.WriteLine("SpriteManagerDump");
            foreach (string s in _texture_list.Keys)
            {
                Console.WriteLine("{0}", s);
            }
        }


        #region IDisposable Members

        public void Dispose()
        {
            if (_texture_list != null)
            {
                foreach (AGT_SpriteResource t in _texture_list.Values)
                {
                    t.Texture.Dispose();
                }
                _texture_list.Clear();
            }
        }

        #endregion
    }

}
