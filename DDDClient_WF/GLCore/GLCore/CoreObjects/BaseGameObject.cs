using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Aptima.Asim.DDD.Client.Common.GLCore.CoreObjects
{
    /// <summary>
    /// Base object for all GLCore shape primatives.
    /// </summary>
    public abstract class BaseGameObject
    {

        /// <summary>
        /// Lower left corner of bounding box.
        /// </summary>
        protected Vector3 min = Vector3.Empty;
        /// <summary>
        /// Upper right corner of bounding box.
        /// </summary>
        protected Vector3 max = Vector3.Empty;

        /// <summary>
        /// 3D vector position of this object.
        /// </summary>
        private Vector3 _position = Vector3.Empty;
        public Vector3 Position
        {
            get
            {
                return _position;
            }
        }
        /// <summary>
        /// 3D rotation of this object.
        /// </summary>
        private Vector3 _rotation = Vector3.Empty;
        public Vector3 Rotation
        {
            get
            {
                return _rotation;
            }
        }
        /// <summary>
        /// 3D Scaling of this object.
        /// </summary>
        private Vector3 _scaling = Vector3.Empty;
        public Vector3 Scaling
        {
            get
            {
                return _scaling;
            }
        }


        /// <summary>
        /// Gets this objects DirectX world matrix.  
        /// Position * RotationX * RotationY * RotationZ * Scaling.
        /// </summary>
        public Matrix WorldMtx
        {
            get
            {
                return (
                    Matrix.Translation(Position) *
                    Matrix.RotationX(Rotation.X) *
                    Matrix.RotationY(Rotation.Y) *
                    Matrix.RotationZ(Rotation.Z) *
                    Matrix.Scaling(this.Scaling)
                    );
            }
        }

        /// <summary>
        /// Determines if the 3D Ray (specified by near and far) Intersects the
        /// bounding box of this object.  Ray / Object intersection.
        /// </summary>
        /// <param name="near">3D position of the ray's near point.</param>
        /// <param name="far">3D position of the ray's far point.</param>
        /// <returns>True, if ray intersects the bounding volume,
        /// False, otherwise.</returns>
        public bool BoxBoundProbe(Vector3 near, Vector3 far)
        {
            return (Geometry.BoxBoundProbe(min, max, near, far));
        }

        public virtual void ClearPosition()
        {
            _position = Vector3.Empty;
        }
        public virtual void SetPosition(float x_pos, float y_pos, float z_pos)
        {
            _position.X = x_pos;
            _position.Y = y_pos;
            _position.Z = z_pos;
        }
        public virtual void ClearRotation()
        {
            _rotation = Vector3.Empty;
        }
        public virtual void SetRotation(float x_rotation, float y_rotation, float z_rotation)
        {
            _rotation.X = x_rotation;
            _rotation.Y = y_rotation;
            _rotation.Z = z_rotation;
        }

        public virtual void ClearScaling()
        {
            _scaling.X = 1;
            _scaling.Y = 1;
            _scaling.Z = 1;
        }
        /// <summary>
        /// Changes the X, Y, Z, scaling of this object.
        /// </summary>
        /// <param name="xScale">Percent X scaling.</param>
        /// <param name="yScale">Percent Y scaling.</param>
        /// <param name="zScale">Percent Z scaling.</param>
        public virtual void SetScale(float x_scale, float y_scale, float z_scale)
        {
            _scaling.X = x_scale;
            _scaling.Y = y_scale;
            _scaling.Z = z_scale;
        }

        /// <summary>
        /// One time initialization required by derived shape primatives.
        /// </summary>
        /// <param name="canvas"></param>
        public virtual void Initialize(Canvas canvas) { ;}
        
        /// <summary>
        /// Deprecated?
        /// One time texture initialization required by derived shape primatives.
        /// </summary>
        /// <param name="canvas"></param>
        public virtual void Texture(Canvas canvas) { ;}
        /// <summary>
        /// Deprecated?
        /// One time texture initialization required by derived shape primatives.
        /// </summary>
        /// <param name="texture">Texture object</param>
        /// <param name="width">width of texture</param>
        /// <param name="height">height of texture</param>
        public virtual void Texture(Texture texture, int width, int height) { ;}

        /// <summary>
        /// Textures the object using GLCore's GameTexture object.
        /// </summary>
        /// <param name="texture"></param>
        public void Texture(GameTexture texture)
        {
            this.Texture(texture.texture, texture.width, texture.height);
        }

        /// <summary>
        /// Draws the object to the Canvas.  This is called from the Canvas
        /// Render loop.
        /// </summary>
        /// <param name="canvas">GLCore Canvas object.</param>
        public abstract void Draw(Canvas canvas);



    }
}
