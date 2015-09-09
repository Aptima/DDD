using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using System.IO;
using System.Drawing;

using System.Reflection;

namespace AGT.Sprites
{

    public class AGT_SystemImages: IDisposable
    {
        private AGT_SpriteManager _sprites = null;

        private AGT_SpriteId _endpoint = new AGT_SpriteId("AGT.images.Endpoint.png");
        private AGT_SpriteId _waypoint = new AGT_SpriteId("AGT.images.Waypoint.png");
        private AGT_SpriteId _cursor_engage = new AGT_SpriteId("AGT.images.Cursor-Engage.png");
        private AGT_SpriteId _cursor_move = new AGT_SpriteId("AGT.images.Cursor-Move.png");
        private AGT_SpriteId _cursor_undock = new AGT_SpriteId("AGT.images.Cursor-Undock.png");
        private AGT_SpriteId _cursor_select = new AGT_SpriteId("AGT.images.Cursor.png");
        private AGT_SpriteId _waypoint_add = new AGT_SpriteId("AGT.images.Waypoint-Add.png");
        private AGT_SpriteId _waypoint_move = new AGT_SpriteId("AGT.images.Waypoint-Move.png");
        private AGT_SpriteId _waypoint_delete = new AGT_SpriteId("AGT.images.Waypoint-Delete.png");


        public AGT_SpriteId Waypoint
        {
            get
            {
                return _waypoint;
            }
        }
        public AGT_SpriteId Cursor_Engage
        {
            get
            {
                return _cursor_engage;
            }
        }
        public AGT_SpriteId Cursor_Move
        {
            get
            {
                return _cursor_move;
            }
        }
        public AGT_SpriteId Cursor_Undock
        {
            get
            {
                return _cursor_undock;
            }
        }
        public AGT_SpriteId Cursor_Select
        {
            get
            {
                return _cursor_select;
            }
        }
        public AGT_SpriteId Waypoint_Add
        {
            get
            {
                return _waypoint_add;
            }
        }
        public AGT_SpriteId Waypoint_Move
        {
            get
            {
                return _waypoint_move;
            }
        }
        public AGT_SpriteId Waypoint_Delete
        {
            get
            {
                return _waypoint_delete;
            }
        }


        public AGT_SystemImages(Microsoft.DirectX.Direct3D.Device d)
        {

            _sprites = new AGT_SpriteManager(d);

            using (Bitmap b = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(_waypoint.Id)))
            {
                _sprites.AddResource(_waypoint.Id, b, 0, 0, 0);
                _sprites.SetCenter(_waypoint, (float)(b.Width * .5f), (float)(b.Height * .5f), 0);
            }
            using (Bitmap b = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(_cursor_engage.Id)))
            {
                _sprites.AddResource(_cursor_engage.Id, b, 0, 0, 0);
                _sprites.SetCenter(_cursor_engage, 0, 0, 0);
            }
            using (Bitmap b = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(_cursor_move.Id)))
            {
                _sprites.AddResource(_cursor_move.Id, b, 0, 0, 0);
                _sprites.SetCenter(_cursor_move, 0, 0, 0);
            }
            using (Bitmap b = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(_cursor_undock.Id)))
            {
                _sprites.AddResource(_cursor_undock.Id, b, 0, 0, 0);
                _sprites.SetCenter(_cursor_undock, 0, 0, 0);
            }
            using (Bitmap b = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(_cursor_select.Id)))
            {
                _sprites.AddResource(_cursor_select.Id, b, 0, 0, 0);
                _sprites.SetCenter(_cursor_select, 0, 0, 0);
            }
            using (Bitmap b = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(_waypoint_add.Id)))
            {
                _sprites.AddResource(_waypoint_add.Id, b, 0, 0, 0);
                _sprites.SetCenter(_waypoint_add, 0, 0, 0);
            }
            using (Bitmap b = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(_waypoint_move.Id)))
            {
                _sprites.AddResource(_waypoint_move.Id, b, 0, 0, 0);
                _sprites.SetCenter(_waypoint_move, 0, 0, 0);
            }
            using (Bitmap b = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(_waypoint_delete.Id)))
            {
                _sprites.AddResource(_waypoint_delete.Id, b, 0, 0, 0);
                _sprites.SetCenter(_waypoint_delete, 0, 0, 0);
            }

        }



        public void DrawSingle(AGT_SpriteId id, float x, float y, float z, Color color)
        {
            if (_sprites != null)
            {
                _sprites.SetPosition(id, x, y, z);

                _sprites.Begin(SpriteFlags.AlphaBlend);
                _sprites.Draw(id, color);
                _sprites.End();
            }
        }
        public void DrawSingle(AGT_SpriteId id, float x, float y, float z)
        {
            DrawSingle(id, x, y, z, Color.White);
        }


        public void Begin()
        {
            if (_sprites != null)
            {
                _sprites.Begin(SpriteFlags.AlphaBlend);
            }
        }


        public void End()
        {
            if (_sprites != null)
            {
                _sprites.End();
            }
        }


        public void Draw(AGT_SpriteId id, float x, float y, float z, Color color)
        {
            if (_sprites != null)
            {
                _sprites.SetPosition(id, x, y, z);

                _sprites.Draw(id, color);
            }
        }


        public void Draw(AGT_SpriteId id, float x, float y, float z)
        {
            Draw(id, x, y, z, Color.White);
        }


        public AGT_SpriteResource GetTextureDefinition(AGT_SpriteId id)
        {
            if (_sprites != null)
            {
                return _sprites.GetTextureDefinition(id);
            }
            return null;
        }


        public void SetPosition(AGT_SpriteId id, float x, float y, float z)
        {
            if (_sprites != null)
            {
                _sprites.SetPosition(id, x, y, z);
            }
        }

        public void SetRotation(AGT_SpriteId id, float x, float y, float z)
        {
            if (_sprites != null)
            {
                _sprites.SetRotation(id, x, y, z);
            }
        }

        public void SetScaling(AGT_SpriteId id, float x, float y, float z)
        {
            if (_sprites != null)
            {
                _sprites.SetTextureScale(id, x, y, z);
            }
        }

        public void SetCenter(AGT_SpriteId id, float x, float y, float z)
        {
            if (_sprites != null)
            {
                _sprites.SetCenter(id, x, y, z);
            }
        }



        #region IDisposable Members

        public void Dispose()
        {
            if (_sprites != null)
            {
                _sprites.Dispose();
            }
        }

        #endregion
    }
}
