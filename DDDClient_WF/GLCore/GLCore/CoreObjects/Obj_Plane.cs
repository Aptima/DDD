using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Aptima.Asim.DDD.Client.Common.GLCore.CoreObjects
{
    /// <summary>
    /// Base class for a textured plane, 3D object.
    /// Not currently used, currently using Sprite objects instead of textured planes.
    /// </summary>
    public class Obj_Plane:BaseGameObject
    {
        /// <summary>
        /// Vertices (corners) of the textured plane.
        /// </summary>
        private CustomVertex.PositionNormalTextured[] verts;
        /// <summary>
        /// Filename of the texture to use
        /// </summary>
        private string _filename;
        /// <summary>
        /// A direct x texture object that will house the file.
        /// </summary>
        private Texture _texture;
        /// <summary>
        /// A direct X material object.
        /// </summary>
        private Material material;


        private int _texture_width;
        /// <summary>
        /// Returns the width of the plane's texture object.
        /// </summary>
        public int Width
        {
            get
            {
                return _texture_width;
            }
        }

        private int _texture_height;
        /// <summary>
        /// Returns the height of the plane's texture object.
        /// </summary>
        public int Height
        {
            get
            {
                return _texture_height;
            }
        }


        /// <summary>
        /// Plane object constructor.  Defines the texture filename the displayed
        /// diffuse color.  A texture will be tinted with the diffuse color.
        /// </summary>
        /// <param name="filename">Texture file.</param>
        /// <param name="diffuse">Texture tint.</param>
        public Obj_Plane(string filename, Color diffuse)
        {
           
            _filename = filename;
            _texture = null;
            material = new Material();
            material.Diffuse = diffuse;

        }
        /// <summary>
        /// Plan object constructor.  Defines the Texture object to use, and the
        /// diffuse color.  A texture will be tinted with the diffuse color.
        /// </summary>
        /// <param name="texture">Texture object (DirectX).</param>
        /// <param name="diffuse">Texture tint.</param>
        public Obj_Plane(Texture texture, Color diffuse)
        {
            _filename = string.Empty;
            _texture = texture;
            material = new Material();
            material.Diffuse = diffuse;

        }

        /// <summary>
        /// Creates a texture from a specified filename (if specified).  Creates a plane
        /// mesh (DirectX mesh object).  Mesh size is the texture size.
        /// </summary>
        /// <param name="canvas">A GLCore Canvas object.</param>
        public override void Initialize(Canvas canvas)
        {

            if (_filename.Length > 0)
            {
                _texture = canvas.LoadTexture(_filename, out _texture_width, out _texture_height);
            }

            verts = new CustomVertex.PositionNormalTextured[4];

            verts[0] = new CustomVertex.PositionNormalTextured(new Vector3(0, _texture_height, 0f), new Vector3(1, 1, 1), 0, 0);
            verts[1] = new CustomVertex.PositionNormalTextured(new Vector3(0, 0, 0), new Vector3(1, 1, 1), 0, 1);
            verts[2] = new CustomVertex.PositionNormalTextured(new Vector3(_texture_width, _texture_height, 0), new Vector3(1, 1, 1), 1, 0);
            verts[3] = new CustomVertex.PositionNormalTextured(new Vector3(_texture_width, 0, 0), new Vector3(1, 1, 1), 1, 1);

            Geometry.ComputeBoundingBox(verts, CustomVertex.PositionNormalTextured.Format, out min, out max);


        }

        /// <summary>
        /// Notifies the GLCore Canvas to use this object's material and texture.
        /// </summary>
        /// <param name="canvas">A GLCore texture object.</param>
        public override void Texture(Canvas canvas)
        {
            canvas.Material = material;
            canvas.SetTexture(0, _texture);
                
        }
        /// <summary>
        /// Draws a textured Plane mesh to a GLCore canvas object.
        /// </summary>
        /// <param name="canvas"></param>
        public override void Draw(Canvas canvas)
        {
            canvas.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, CustomVertex.PositionNormalTextured.Format, verts);              
        }

    }
}
