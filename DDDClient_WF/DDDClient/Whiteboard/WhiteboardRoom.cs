using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Drawing;

using Aptima.Asim.DDD.Client.Controller;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace Aptima.Asim.DDD.Client.Whiteboard
{
    // Summary:
    //     Defines whiteboard drawing modes.
    public enum DrawModes : int
    {
        Selection = 1,
        Line = 2,
        Arrow = 3,
        Circle = 4,
        Text = 5
    }

    public class LineObject
    {
        private DrawModes mode;

        public DrawModes Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
            }
        }

        private LocationValue start;

        public LocationValue Start
        {
            get { return start; }
            set { start = value; }
        }

        private LocationValue end;

        public LocationValue End
        {
            get { return end; }
            set { end = value; }
        }
        private double width;

        public double Width
        {
            get { return width; }
            set { width = value; }
        }

        private double originalScale;

        public double OriginalScale
        {
            get { return originalScale; }
            set { originalScale = value; }
        }

        private int color;

        public int Color
        {
            get { return color; }
            set { color = value; }
        }

        private string text;

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        private string objectID;

        public string ObjectID
        {
            get
            {
                return objectID;
            }
            set
            {
                objectID = value;
            }
        }

        private Polygon boundingPolygon;

        public Polygon BoundingPolygon
        {
            get { return boundingPolygon; }
            set { boundingPolygon = value; }
        }

        private bool objectSelected;

        public bool ObjectSelected
        {
            get { return objectSelected; }
            set { objectSelected = value; }
        }

        private float innerRadius;

        public float InnerRadius
        {
            get { return innerRadius; }
            set { innerRadius = value; }
        }

        private float outterRadius;

        public float OutterRadius
        {
            get { return outterRadius; }
            set { outterRadius = value; }
        }
    }

    public class ScreenViewInfo
    {
        private int originX;

        public int OriginX
        {
            get
            {
                return originX;
            }
            set
            {
                originX = value;
            }
        }

        private int originY;

        public int OriginY
        {
            get
            {
                return originY;
            }
            set
            {
                originY = value;
            }
        }

        private int screenSizeWidth;

        public int ScreenSizeWidth
        {
            get
            {
                return screenSizeWidth;
            }
            set
            {
                screenSizeWidth = value;
            }
        }

        private int screenSizeHeight;

        public int ScreenSizeHeight
        {
            get
            {
                return screenSizeHeight;
            }
            set
            {
                screenSizeHeight = value;
            }
        }

        private double screenZoom;

        public double ScreenZoom
        {
            get
            {
                return screenZoom;
            }
            set
            {
                screenZoom = value;
            }
        }

        public ScreenViewInfo(int originX, int originY, int screenSizeWidth,
                                      int screenSizeHeight, double screenZoom)
        {
            OriginX = originX;
            OriginY = originY;
            ScreenSizeWidth = screenSizeWidth;
            ScreenSizeHeight = screenSizeHeight;
            ScreenZoom = screenZoom;
        }
    }

    public class WhiteboardRoom
    {
        private System.Object _threadLock = new object();
        private List<object> drawingCommands;

        private Object screenViewLock = new object();
        private Dictionary<string, ScreenViewInfo> screenViewMap = new Dictionary<string, ScreenViewInfo>();

        private Object screenViewStackLock = new object();
        private Stack<ScreenViewInfo> screenViewStack = new Stack<ScreenViewInfo>();

        private Object otherWBRoomLock = new object();
        private Dictionary<string, WhiteboardRoom> otherWBRooms = new Dictionary<string, WhiteboardRoom>();

        private DrawModes drawMode = DrawModes.Selection;
        private GUIController _Controller;
        private string _name = null;
        private Object drawingCommandLock = new Object();
        private List<string> _membership_list = null;
        private int numObjectsSelected = 0;

        public GUIController Controller
        {
            get
            {
                return _Controller;
            }
            set
            {
                _Controller = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public DrawModes DrawMode
        {
            get
            {
                return drawMode;
            }
            set
            {
                drawMode = value;
            }
        }

        private int drawColor = Color.Red.ToArgb();

        public int DrawColor
        {
            get { return drawColor; }
            set { drawColor = value; }
        }

        private int drawPointSize = 1;

        public int DrawPointSize
        {
            get { return drawPointSize; }
            set { drawPointSize = value; }
        }

        private string drawText = "";

        public string DrawText
        {
            get { return drawText; }
            set { drawText = value; }
        }

        public List<string> MembershipList
        {
            get
            {
                return _membership_list;
            }
            set
            {
                _membership_list = value;
            }
        }

        private bool isRoomOwner = false;

        public bool IsRoomOwner
        {
            get { return isRoomOwner; }
            set { isRoomOwner = value; }
        }

        // Structure that stores the results of the PolygonCollision function
        public struct PolygonCollisionResult
        {
            public bool WillIntersect; // Are the polygons going to intersect forward in time?
            public bool Intersect; // Are the polygons currently intersecting
            public Vector MinimumTranslationVector; // The translation to apply to polygon A to push the polygons appart.
        }

        // Check if polygon A is going to collide with polygon B for the given velocity
        public PolygonCollisionResult PolygonCollision(Polygon polygonA, Polygon polygonB, Vector velocity)
        {
            PolygonCollisionResult result = new PolygonCollisionResult();
            result.Intersect = true;
            result.WillIntersect = true;

            int edgeCountA = polygonA.Edges.Count;
            int edgeCountB = polygonB.Edges.Count;
            float minIntervalDistance = float.PositiveInfinity;
            Vector translationAxis = new Vector();
            Vector edge;

            // Loop through all the edges of both polygons
            for (int edgeIndex = 0; edgeIndex < edgeCountA + edgeCountB; edgeIndex++)
            {
                if (edgeIndex < edgeCountA)
                {
                    edge = polygonA.Edges[edgeIndex];
                }
                else
                {
                    edge = polygonB.Edges[edgeIndex - edgeCountA];
                }

                // ===== 1. Find if the polygons are currently intersecting =====

                // Find the axis perpendicular to the current edge
                Vector axis = new Vector(-edge.Y, edge.X);
                axis.Normalize();

                // Find the projection of the polygon on the current axis
                float minA = 0; float minB = 0; float maxA = 0; float maxB = 0;
                ProjectPolygon(axis, polygonA, ref minA, ref maxA);
                ProjectPolygon(axis, polygonB, ref minB, ref maxB);

                // Check if the polygon projections are currentlty intersecting
                if (IntervalDistance(minA, maxA, minB, maxB) > 0) result.Intersect = false;

                // ===== 2. Now find if the polygons *will* intersect =====

                // Project the velocity on the current axis
                float velocityProjection = axis.DotProduct(velocity);

                // Get the projection of polygon A during the movement
                if (velocityProjection < 0)
                {
                    minA += velocityProjection;
                }
                else
                {
                    maxA += velocityProjection;
                }

                // Do the same test as above for the new projection
                float intervalDistance = IntervalDistance(minA, maxA, minB, maxB);
                if (intervalDistance > 0) result.WillIntersect = false;

                // If the polygons are not intersecting and won't intersect, exit the loop
                if (!result.Intersect && !result.WillIntersect) break;

                // Check if the current interval distance is the minimum one. If so store
                // the interval distance and the current distance.
                // This will be used to calculate the minimum translation vector
                intervalDistance = Math.Abs(intervalDistance);
                if (intervalDistance < minIntervalDistance)
                {
                    minIntervalDistance = intervalDistance;
                    translationAxis = axis;

                    Vector d = polygonA.Center - polygonB.Center;
                    if (d.DotProduct(translationAxis) < 0) translationAxis = -translationAxis;
                }
            }

            // The minimum translation vector can be used to push the polygons appart.
            // First moves the polygons by their velocity
            // then move polygonA by MinimumTranslationVector.
            if (result.WillIntersect) result.MinimumTranslationVector = translationAxis * minIntervalDistance;

            return result;
        }

        // Calculate the distance between [minA, maxA] and [minB, maxB]
        // The distance will be negative if the intervals overlap
        public float IntervalDistance(float minA, float maxA, float minB, float maxB)
        {
            if (minA < minB)
            {
                return minB - maxA;
            }
            else
            {
                return minA - maxB;
            }
        }

        // Calculate the projection of a polygon on an axis and returns it as a [min, max] interval
        public void ProjectPolygon(Vector axis, Polygon polygon, ref float min, ref float max)
        {
            // To project a point on an axis use the dot product
            float d = axis.DotProduct(polygon.Points[0]);
            min = d;
            max = d;
            for (int i = 0; i < polygon.Points.Count; i++)
            {
                d = polygon.Points[i].DotProduct(axis);
                if (d < min)
                {
                    min = d;
                }
                else
                {
                    if (d > max)
                    {
                        max = d;
                    }
                }
            }
        }

        public Array GetCurrentDrawingCommands()
        {
            return drawingCommands.ToArray();
        }

        public WhiteboardRoom(string name, List<string> membership_list, GUIController controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException("Cannot pass a null GUIController");
            }
            _Controller = controller;
            _name = name;
            _membership_list = membership_list;
            drawingCommands = new List<object>();
        }

        private void GetLineBoundingPoly(float x1, float y1, float x2, float y2,
                                         float width, float originalScale,
                                         out Polygon boundingPolygon)
        {
            float minDist;
            
            width = width / 2;
            if ((width * originalScale) < 5) 
            {
                minDist = 5 / originalScale;
            }
            else
            {
                minDist = width;
            }

            float angle = (float)Math.Atan2(y2 - y1, x2 - x1);
            float t2sina1 = minDist * (float)Math.Sin(angle);
            float t2cosa1 = minDist * (float)Math.Cos(angle);

            boundingPolygon = new Polygon();
            boundingPolygon.Points.Add(new Vector(x1 + t2sina1, y1 - t2cosa1));
            boundingPolygon.Points.Add(new Vector(x2 + t2sina1, y2 - t2cosa1));
            boundingPolygon.Points.Add(new Vector(x2 - t2sina1, y2 + t2cosa1));
            boundingPolygon.Points.Add(new Vector(x1 - t2sina1, y1 + t2cosa1));
            boundingPolygon.Offset(0, 0);
            boundingPolygon.BuildEdges();
        }

        private void GetCircleBoundingPoly(float x1, float y1, float x2, float y2,
                                         float width, float originalScale,
                                         out Polygon boundingPolygon, out float outterRadius,
                                         out float innerRadius)
        {
            float minDist;

            width = width / 2;
            if ((width * originalScale) < 5)
            {
                minDist = 5 / originalScale;
            }
            else
            {
                minDist = width;
            }

            float radius = (float)Math.Sqrt(Math.Pow((double)Math.Abs(x1 - x2), 2) + Math.Pow((double)Math.Abs(y1 - y2), 2));
            radius += minDist;

            boundingPolygon = new Polygon();
            boundingPolygon.Points.Add(new Vector(x1 + radius, y1 - radius));
            boundingPolygon.Points.Add(new Vector(x1 + radius, y1 + radius));
            boundingPolygon.Points.Add(new Vector(x1 - radius, y1 + radius));
            boundingPolygon.Points.Add(new Vector(x1 - radius, y1 - radius));
            boundingPolygon.Offset(0, 0);
            boundingPolygon.BuildEdges();

            // Setup the radius selection range for a circle
            outterRadius = radius;
            innerRadius = radius - (2 * minDist);
            if (innerRadius < 0)
            {
                innerRadius = 0;
            }
        }

        public bool CircleSelectedByRect(LineObject lineObject, RectangleF selectionRect)
        {
            PointF circleDistance = new PointF();
            float cornerDistance;
            float testRadius;
            bool intersectsOutterCir = false;
            bool testResult;

            if ((lineObject == null) || (lineObject.Mode != DrawModes.Circle))
            {
                return false;
            }
            if ((lineObject.InnerRadius < 0) || (lineObject.OutterRadius <= 0) || (lineObject.InnerRadius > lineObject.OutterRadius))
            {
                return false;
            }

            // Test for outter circle intersection (needs to be true to return true)
            testRadius = lineObject.OutterRadius;
            testResult = false;
            circleDistance.X = Math.Abs((float) lineObject.Start.X - selectionRect.X - selectionRect.Width / 2);
            circleDistance.Y = Math.Abs((float) lineObject.Start.Y - selectionRect.Y - selectionRect.Height / 2);

            if (circleDistance.X > (selectionRect.Width / 2 + testRadius)) {
                testResult = false;
            }
            else if (circleDistance.Y > (selectionRect.Height / 2 + testRadius)) {
                testResult = false;
            }
            else if (circleDistance.X <= (selectionRect.Width / 2)) {
                testResult = true;
            }
            else if (circleDistance.Y <= (selectionRect.Height / 2))
            {
                testResult = true;
            }
            else
            {
                cornerDistance = (float)Math.Sqrt(Math.Pow((circleDistance.X - selectionRect.Width / 2), 2) +
                                      Math.Pow((circleDistance.Y - selectionRect.Height / 2), 2));

                testResult = (cornerDistance <= testRadius);
            }

            if (testResult)
            {
                // Test for inner circle intersection (needs to be false or part of the rect
                // must be outside the inner circle to return true)
                testRadius = lineObject.InnerRadius;
                testResult = false;
                circleDistance.X = Math.Abs((float)lineObject.Start.X - selectionRect.X - selectionRect.Width / 2);
                circleDistance.Y = Math.Abs((float)lineObject.Start.Y - selectionRect.Y - selectionRect.Height / 2);

                if (circleDistance.X > (selectionRect.Width / 2 + testRadius))
                {
                    testResult = true;
                }
                else if (circleDistance.Y > (selectionRect.Height / 2 + testRadius))
                {
                    testResult = true;
                }
                else if (circleDistance.X <= (selectionRect.Width / 2))
                {
                    testResult = true;
                }
                else if (circleDistance.Y <= (selectionRect.Height / 2))
                {
                    testResult = true;
                }
                else
                {
                    cornerDistance = (float)Math.Sqrt(Math.Pow((circleDistance.X - selectionRect.Width / 2), 2) +
                                          Math.Pow((circleDistance.Y - selectionRect.Height / 2), 2));

                    testResult = (cornerDistance > testRadius);
                }
            }

            return testResult;
        }

        public bool AddLine(string objectID, DrawModes mode, LocationValue start, LocationValue end, double width, double oringinalScale, int color, string text)
        {
            LineObject line = new LineObject();
            line.Mode = mode;
            line.Start = start;
            line.End = end;
            line.Width = width;
            line.OriginalScale = oringinalScale;
            line.Color = color;
            line.Text = text;
            line.ObjectID = objectID;
            line.BoundingPolygon = null;
            line.ObjectSelected = false;
            line.InnerRadius = 0.0f;
            line.OutterRadius = 0.0f;

            string user_id;

            if (objectID.LastIndexOf('_') >= 0)
            {
                user_id = objectID.Substring(0, objectID.LastIndexOf('_'));
            }
            else
            {
                return false;
            }

            // If this is not a circle, create a bounding polygon for this object
            Polygon newBoundingPoly = null;
            if ((line.Mode == DrawModes.Line) || (line.Mode == DrawModes.Arrow))
            {
                GetLineBoundingPoly((float) line.Start.X, (float) line.Start.Y, (float) line.End.X, (float) line.End.Y,
                    (float) line.Width, (float) line.OriginalScale, out newBoundingPoly);
                line.BoundingPolygon = newBoundingPoly;
            }
            else if (line.Mode == DrawModes.Circle)
            {
                float innerRadius;
                float outterRadius;

                GetCircleBoundingPoly((float)line.Start.X, (float)line.Start.Y, (float)line.End.X, (float)line.End.Y,
                    (float)line.Width, (float)line.OriginalScale, out newBoundingPoly,
                    out outterRadius, out innerRadius);
                line.BoundingPolygon = newBoundingPoly;
                line.InnerRadius = innerRadius;
                line.OutterRadius = outterRadius;
            }

            if (string.Compare(user_id, DDD_Global.Instance.PlayerID) == 0)
            {
                lock (drawingCommandLock)
                {
                    drawingCommands.Add(line);
                }
            }
            else
            {
                drawingCommands.Add(line);
            }

            return true;
        }

        public bool Clear(string user_id)
        {
            lock (drawingCommandLock)
            {
                // Only clear this users drawing objects
                for (int i = drawingCommands.Count - 1; i >= 0; i--)
                {
                    if (drawingCommands[i] is LineObject)
                    {
                        LineObject drawingObject = (LineObject)drawingCommands[i];
                        string objectUserID;

                        if ((drawingObject.ObjectID != null) && (drawingObject.ObjectID.LastIndexOf('_') >= 0))
                        {
                            objectUserID = drawingObject.ObjectID.Substring(0, drawingObject.ObjectID.LastIndexOf('_'));
                            if (string.Compare(objectUserID, user_id) == 0)
                            {
                                // Clear this drawing object
                                drawingCommands.RemoveAt(i);
                            }
                        }
                    }
                }
            }

            return true;
        }

        public bool ClearAll()
        {
            lock (drawingCommandLock)
            {
                drawingCommands.Clear();
            }
            return true;
        }

        public string GetMyLastObjectID()
        {
            string objectID = null;

            if (drawingCommands.Count > 0)
            {
                lock (drawingCommandLock)
                {
                    for(int i = drawingCommands.Count - 1; i >= 0; i--)
                    {
                        if (drawingCommands[i] is LineObject)
                        {
                            LineObject drawingObject = (LineObject)drawingCommands[i];
                            string user_id;

                            if ((drawingObject.ObjectID != null) && (drawingObject.ObjectID.LastIndexOf('_') >= 0))
                            {
                                user_id = drawingObject.ObjectID.Substring(0, drawingObject.ObjectID.LastIndexOf('_'));
                                if (string.Compare(user_id, DDD_Global.Instance.PlayerID) == 0)
                                {
                                    objectID = drawingObject.ObjectID;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return objectID;
        }

        public bool Undo(string object_id)
        {
            bool retVal = false;
            LineObject curDrawingObject;

            lock (drawingCommandLock)
            {
                for (int i = drawingCommands.Count - 1; i >= 0; i--)
                {
                    if (drawingCommands[i] is LineObject)
                    {
                        curDrawingObject = (LineObject) drawingCommands[i];
                        if (string.Compare(curDrawingObject.ObjectID, object_id) == 0)
                        {
                            drawingCommands.RemoveAt(i);
                            retVal = true;
                            break;
                        }
                    }
                }
            }

            return retVal;
        }

        public bool UpdateScreenView(string user_id, int originX, int originY, int screenSizeWidth,
                                      int screenSizeHeight, double screenZoom)
        {
            ScreenViewInfo screenViewInfo = new ScreenViewInfo(originX, originY, screenSizeWidth,
                                      screenSizeHeight, screenZoom);

            if ((MembershipList == null) || (!MembershipList.Contains(user_id)))
            {
                return false;
            }

            lock (screenViewLock)
            {
                if (!screenViewMap.ContainsKey(user_id))
                {
                    screenViewMap.Add(user_id, screenViewInfo);
                    return true;
                }
                else
                {
                    screenViewMap[user_id] = screenViewInfo;
                    return false;
                }
            }
        }

        public bool GetScreenViewInfo(string user_id, ref int originX, ref int originY, ref int screenSizeWidth,
            ref int screenSizeHeight, ref double screenZoom)
        {
            lock (screenViewLock)
            {
                if (!screenViewMap.ContainsKey(user_id))
                {
                    return false;
                }
                else
                {
                    ScreenViewInfo screenViewInfo = screenViewMap[user_id];

                    originX = screenViewInfo.OriginX;
                    originY = screenViewInfo.OriginY;
                    screenSizeWidth = screenViewInfo.ScreenSizeWidth;
                    screenSizeHeight = screenViewInfo.ScreenSizeHeight;
                    screenZoom = screenViewInfo.ScreenZoom;

                    return true;
                }
            }
        }

        public bool SaveScreenView()
        {
            ScreenViewInfo screenViewInfo;

            // Get this users current view information
            lock (screenViewLock)
            {
                if (!screenViewMap.ContainsKey(DDD_Global.Instance.PlayerID))
                {
                    return false;
                }
                else
                {
                    screenViewInfo = screenViewMap[DDD_Global.Instance.PlayerID];
                }
            }

            // Add this view to the view stack
            lock (screenViewStackLock)
            {
                screenViewStack.Push(screenViewInfo);
            }

            return true;
        }

        public bool PopScreenViewInfo(ref int originX, ref int originY, ref int screenSizeWidth,
            ref int screenSizeHeight, ref double screenZoom)
        {
            // Pop a view off the view stack if it is available
            lock (screenViewStackLock)
            {
                ScreenViewInfo screenViewInfo = null;

                try
                {
                    screenViewInfo = screenViewStack.Pop();
                }
                catch (Exception exc)
                {
                }

                if (screenViewInfo == null)
                {
                    return false;
                }

                originX = screenViewInfo.OriginX;
                originY = screenViewInfo.OriginY;
                screenSizeWidth = screenViewInfo.ScreenSizeWidth;
                screenSizeHeight = screenViewInfo.ScreenSizeHeight;
                screenZoom = screenViewInfo.ScreenZoom;

                return true;
            }
        }

        public bool ScreenViewInfoEmpty()
        {
            return (screenViewStack.Count == 0);
        }

        public void AddOtherRoom(string room_name, WhiteboardRoom wbRoom)
        {
            lock (otherWBRoomLock)
            {
                if (!otherWBRooms.ContainsKey(room_name))
                {
                    otherWBRooms.Add(room_name, wbRoom);
                }
            }
        }

        public void RemoveOtherRoom(string room_name)
        {
            lock (otherWBRoomLock)
            {
                if (otherWBRooms.ContainsKey(room_name))
                {
                    otherWBRooms.Remove(room_name);
                }
            }
        }

        public List<string> GetOtherWBRoomNames()
        {
            lock (otherWBRoomLock)
            {
                if ((otherWBRooms != null) && (otherWBRooms.Keys != null))
                {
                    return new List<string>(otherWBRooms.Keys);
                }
            }

            return null;
        }

        public WhiteboardRoom GetOtherWBRoom(string room_name)
        {
            lock (otherWBRoomLock)
            {
                if (otherWBRooms.ContainsKey(room_name))
                {
                    return otherWBRooms[room_name];
                }
            }

            return null;
        }

        public void ClearSelectionList()
        {
            lock (drawingCommandLock)
            {
                foreach (Object command in drawingCommands)
                {
                    if (command is LineObject)
                    {
                        ((LineObject)command).ObjectSelected = false;
                    }
                }
                numObjectsSelected = 0;
            }
        }

        private bool IsAllowedSelection(LineObject lineObject)
        {
            string user_id;
            string objectID = lineObject.ObjectID;

            if (IsRoomOwner)
            {
                return true;
            }

            if (objectID.LastIndexOf('_') >= 0)
            {
                user_id = objectID.Substring(0, objectID.LastIndexOf('_'));
            }
            else
            {
                return false;
            }

            return (string.Compare(user_id, DDD_Global.Instance.PlayerID) == 0);
        }

        public void AddSelection(RectangleF selectionRect, bool areaSelection)
        {
            Polygon selectionPolygon = null;
            LineObject lineObject;
            PolygonCollisionResult result;
            LineObject prevSelectedObject = null;

            // Create Selection Polygon
            selectionPolygon = new Polygon();
            selectionPolygon.Points.Add(new Vector(selectionRect.Left, selectionRect.Top));
            selectionPolygon.Points.Add(new Vector(selectionRect.Right, selectionRect.Top));
            selectionPolygon.Points.Add(new Vector(selectionRect.Right, selectionRect.Bottom));
            selectionPolygon.Points.Add(new Vector(selectionRect.Left, selectionRect.Bottom));
            selectionPolygon.Offset(0, 0);
            selectionPolygon.BuildEdges();

            if (areaSelection)
            {
                // Select an area of objects
                lock (drawingCommandLock)
                {
                    foreach (Object command in drawingCommands)
                    {
                        if (command is LineObject)
                        {
                            lineObject = command as LineObject;

                            if ((lineObject.BoundingPolygon != null) && (lineObject.Mode != DrawModes.Circle))
                            {
                                result = PolygonCollision(lineObject.BoundingPolygon, selectionPolygon, new Vector(0, 0));
                                if ((result.Intersect) && (IsAllowedSelection(lineObject)))
                                {
                                    lineObject.ObjectSelected = true;
                                    numObjectsSelected++;
                                }
                            }
                            else if (lineObject.Mode == DrawModes.Circle)
                            {
                                if ((CircleSelectedByRect(lineObject, selectionRect)) && (IsAllowedSelection(lineObject)))
                                {
                                    lineObject.ObjectSelected = true;
                                    numObjectsSelected++;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                // Select a single object
                if (numObjectsSelected > 1)
                {
                    ClearSelectionList();
                }
                else if (numObjectsSelected == 1)
                {
                    // Locate the previously selected object
                    lock (drawingCommandLock)
                    {
                        foreach (Object command in drawingCommands)
                        {
                            if ((command is LineObject) && ((LineObject) command).ObjectSelected)
                            {
                                prevSelectedObject = command as LineObject;
                                break;
                            }
                        }
                    }
                }

                LineObject selectedObject = null;
                LineObject firstSelectedObject = null;
                bool prevSelectionFound = false;

                lock (drawingCommandLock)
                {
                    foreach (Object command in drawingCommands)
                    {
                        if (command is LineObject)
                        {
                            bool objectSelected = false;
                            lineObject = command as LineObject;

                            if ((lineObject.BoundingPolygon != null) && (lineObject.Mode != DrawModes.Circle))
                            {
                                result = PolygonCollision(lineObject.BoundingPolygon, selectionPolygon, new Vector(0, 0));
                                if ((result.Intersect) && (IsAllowedSelection(lineObject)))
                                {
                                    objectSelected = true;
                                }
                            }
                            else if (lineObject.Mode == DrawModes.Circle)
                            {
                                if ((CircleSelectedByRect(lineObject, selectionRect)) && (IsAllowedSelection(lineObject)))
                                {
                                    objectSelected = true;
                                }
                            }

                            if (objectSelected)
                            {
                                if (firstSelectedObject == null)
                                {
                                    firstSelectedObject = lineObject;
                                }

                                if ((prevSelectionFound) && (lineObject != prevSelectedObject))
                                {
                                    selectedObject = lineObject;
                                    break;
                                }

                                if (lineObject == prevSelectedObject)
                                {
                                    selectedObject = lineObject;
                                    prevSelectionFound = true;
                                }
                            }
                        }
                    }
                }

                if ((selectedObject == prevSelectedObject) || (selectedObject == null))
                {
                    selectedObject = firstSelectedObject;
                }

                ClearSelectionList();

                if ((selectedObject != null) && (IsAllowedSelection(selectedObject)))
                {
                    selectedObject.ObjectSelected = true;
                    numObjectsSelected++;
                }
            }
        }

        public bool ObjectsSelected()
        {
            return numObjectsSelected > 0;
        }

        public void DeleteSelectionObjects()
        {
            lock (drawingCommandLock)
            {
                foreach (Object command in drawingCommands)
                {
                    if (command is LineObject)
                    {
                        LineObject lineObject = (LineObject)command;
                        if (lineObject.ObjectSelected)
                        {
                            lineObject.ObjectSelected = false;

                            // Determine if the user owns this object
                            if (!IsAllowedSelection(lineObject))
                            {
                                continue;
                            }

                            _Controller.WhiteboardUndoRequest(((LineObject)command).ObjectID, DDD_Global.Instance.PlayerID, Name);
                        }
                    }
                }
                numObjectsSelected = 0;
            }
        }

    }
}