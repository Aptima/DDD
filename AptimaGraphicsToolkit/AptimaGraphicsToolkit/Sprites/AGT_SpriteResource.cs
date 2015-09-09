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
    public sealed class AGT_SpriteResource
    {
        public string TextureId;
        public Texture Texture;
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scaling;
        public Vector3 Center;
        public Vector2 Aspect;
        public bool CanRotate;
        public int Width;
        public int Height;
        public int BitmapWidth;
        public int BitmapHeight;

        public AGT_SpriteResource()
        {
            TextureId = string.Empty;
            Texture = null;
            CanRotate = false;
            Position = new Vector3(0, 0, 0);
            Rotation = new Vector3(0, 0, 0);
            Center = new Vector3(0, 0, 0);
            Aspect = new Vector2(0, 0);
            Scaling = new Vector3(1f, 1f, 1f);
        }

        public AGT_SpriteResource(AGT_SpriteResource t)
        {
            TextureId = t.TextureId;
            Texture = t.Texture;
            Position = t.Position;
            Rotation = t.Rotation;
            Scaling = t.Scaling;
            Center = t.Center;
            Width = t.Width;
            Height = t.Height;
           
            CanRotate = t.CanRotate;
            Aspect = t.Aspect;
        }

        public void SetScale(float x_scale, float y_scale)
        {
            //Scaling.X = (float)(((float)this.Aspect.X * (float)x_scale) * (float)this.Width);
            //Scaling.Y = (float)(((float)this.Aspect.Y * (float)y_scale) * (float)this.Height);
            Scaling.X = (float)this.Aspect.X * (float)x_scale;
            Scaling.Y = (float)this.Aspect.Y * (float)y_scale;
            Scaling.Z = 1;            
        }
        public void Draw(Sprite sprite_resource, int diffuse)
        {
            sprite_resource.Transform = 
                Matrix.Scaling(Scaling) *
                Matrix.RotationX(Rotation.X) *
                Matrix.RotationY(Rotation.Y) *
                Matrix.RotationZ(Rotation.Z) *
                Matrix.Translation(Position);
            sprite_resource.Draw(Texture, Center, Vector3.Empty, diffuse);
        }

        public void Draw(Sprite sprite_resource)
        {
            Draw(sprite_resource, Color.White.ToArgb());
        }

        public void DrawRelative(Sprite sprite_resource, float relative_scale, float x_offset, float y_offset, float z_offset, int diffuse)
        {
            Vector3 v = Vector3.Empty;
            v.X = (Position.X * relative_scale) + x_offset;
            v.Y = (Position.Y * relative_scale) + y_offset;
            v.Z = (Position.Z * relative_scale) + z_offset;
            
            sprite_resource.Transform =
                Matrix.Scaling(Scaling) *
                Matrix.RotationX(Rotation.X) *
                Matrix.RotationY(Rotation.Y) *
                Matrix.RotationZ(Rotation.Z) *
                Matrix.Translation(v);
            sprite_resource.Draw(Texture, Center, Vector3.Empty, diffuse);
        }
    }

}
