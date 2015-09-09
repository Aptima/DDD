using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Aptima.Asim.DDD.Client.Common.GLCore
{
    public class Camera
    {
        private Vector3 _v3LookAt;

        public Vector3 LookAt
        {
            get
            {
                return _v3LookAt;
            }
            set
            {
                _v3LookAt = value;
            }
        }
        private bool ShouldSerializeLookAt()
        {
            return true;
        }
       

        private Vector3 _v3Up;
        public Vector3 Up
        {
            get
            {
                return _v3Up;
            }
            set
            {
                _v3Up = value;
            }
        }
        private bool ShouldSerializeUp()
        {
            return true;
        }


        private Vector3 _v3Position;
        public Vector3 Position
        {
            get
            {
                return _v3Position;
            }
            set
            {
                _v3Position = value;
            }
        }
        private bool ShouldSerializePosition()
        {
            return true;
        }
        public Camera()
        {
            _v3LookAt = new Vector3(0, 0, 0);
            _v3Position = new Vector3(0, 0, -10);
            _v3Up = new Vector3(0, 1, 0);
        }

        public Camera(Vector3 positon, Vector3 look_at, Vector3 up)
        {
            this._v3Position = positon;
            this._v3LookAt = look_at;
            this._v3Up = up;
        }

        public void ChangePosition(Vector3 position)
        {
            _v3Position.Add(position);
        }
        public void ChangePosition(float x, float y, float z)
        {
            ChangePosition(new Vector3(x, y, z));
        }

        public void ChangeDirection(Vector3 direction)
        {
            _v3LookAt.Add(direction);
        }
        public void ChangeDirection(float x, float y, float z)
        {
            ChangeDirection(new Vector3(x, y, z));
        }

        public Matrix LookAtLH()
        {
            return Matrix.LookAtLH(_v3Position, _v3LookAt, _v3Up);
        }
        public Matrix LookAtRH()
        {
            return Matrix.LookAtRH(_v3Position, _v3LookAt, _v3Up);
        }


    }

}
