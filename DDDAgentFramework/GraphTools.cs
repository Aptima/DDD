using System;
using System.Collections.Generic;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;


namespace Aptima.Asim.DDD.DDDAgentFramework
{
    public class LocationGraph
    {
        public class LocationNode
        {
            private String m_name;
            private List<LocationNode> m_connectedNodes;
            public List<LocationNode> ConnectedNodes
            {
                get { return m_connectedNodes; }
            }
            public String Name
            {
                set { m_name = value; }
                get { return m_name; }
            }
            
            public void Connect(LocationNode n)
            {
                if (!ConnectedNodes.Contains(n))
                {
                    ConnectedNodes.Add(n);
                }
            }
            private LocationValue m_location;
            public LocationValue Location
            {
                get { return m_location; }
            }

            public LocationNode(String name, LocationValue loc)
            {
                m_location = loc;
                m_connectedNodes = new List<LocationNode>();
                Name = name;
            }
        }

        
        private Dictionary<String, LocationNode> m_nodes;
        public Dictionary<String, LocationNode> GraphNodes
        {
            get { return m_nodes; }
        }
        public LocationGraph()
        {
            m_nodes = new Dictionary<string, LocationNode>();
        }
        public void AddNode(LocationNode n)
        {
            m_nodes[n.Name] = n;
        }
        public void Connect(String fromName, String toName)
        {
            if (m_nodes.ContainsKey(fromName) && m_nodes.ContainsKey(toName))
            {
                m_nodes[fromName].Connect(m_nodes[toName]);
            }
        }
        public void BiConnect(String fromName, String toName)
        {
            if (m_nodes.ContainsKey(fromName) && m_nodes.ContainsKey(toName))
            {
                m_nodes[fromName].Connect(m_nodes[toName]);
                m_nodes[toName].Connect(m_nodes[fromName]);
            }
        }
        public void Connect(LocationNode fromNode, LocationNode toNode)
        {
            fromNode.Connect(toNode);
        }
        public void BiConnect(LocationNode fromNode, LocationNode toNode)
        {
            fromNode.Connect(toNode);
            toNode.Connect(fromNode);
        }
        public bool ContainesNode(String name)
        {
            return m_nodes.ContainsKey(name);
        }
        public LocationNode GetNode(String name)
        {
            if (m_nodes.ContainsKey(name))
            {
                return m_nodes[name];
            }
            else
            {
                return null;
            }
        }

        //public void ProximityConnect(Double distanceThreshold)
        //{
        //    List<String> workingList = new List<string>(m_nodes.Keys);

        //    while (workingList.Count > 0)
        //    {
        //        String currentNodeName = workingList[0];

        //        workingList.Remove(currentNodeName);
        //        foreach (String workingNodeName in workingList)
        //        {
        //            if (BehaviorHelper.LocationIsEqual(m_nodes[currentNodeName].Location, m_nodes[workingNodeName].Location, distanceThreshold))
        //            {
        //                BiConnect(currentNodeName, workingNodeName);
        //            }
        //        }
        //    }
        //}

        //public void IntersectConnect()
        //{
        //    List<String> workingList = new List<string>(m_nodes.Keys);
        //    while (workingList.Count > 0)
        //    {
        //        String currentNodeName = workingList[0];
        //        LocationNode currentNode = m_nodes[currentNodeName];
        //        workingList.Remove(currentNodeName);
        //        List<LocationNode> connectedNodes = new List<LocationNode>(currentNode.ConnectedNodes);
        //        foreach (LocationNode connectedNode in connectedNodes)
        //        {
        //            foreach (String workingNodeName in workingList)
        //            {
        //                LocationNode workingNode = m_nodes[workingNodeName];
        //                List<LocationNode> connectedNodes2 = new List<LocationNode>(workingNode.ConnectedNodes);
        //                foreach (LocationNode workingConnectedNode in connectedNodes2)
        //                {
        //                    LocationValue intersectLoc = BehaviorHelper.LineIntersect(currentNode.Location, connectedNode.Location,
        //                                                                              workingNode.Location, workingConnectedNode.Location);
        //                    if (intersectLoc != null)
        //                    {
        //                        LocationNode newNode = new LocationNode(String.Format("{0}_{1}_{2}_{3}",currentNode.Name,connectedNode.Name,
        //                                                                workingNode.Name, workingConnectedNode.Name), 
        //                                                                intersectLoc);
        //                        AddNode(newNode);
        //                        BiConnect(currentNode, newNode);
        //                        BiConnect(connectedNode, newNode);
        //                        BiConnect(workingNode, newNode);
        //                        BiConnect(workingConnectedNode, newNode);
        //                    }
        //                }

        //            }
        //        }

                
        //    }
        //}

        void InsertByProximity(LocationNode newNode)
        {
            //find closest node
            List<String> workingList = new List<string>(m_nodes.Keys);
            String closest = workingList[0];
            double distance = BehaviorHelper.Distance(newNode.Location, m_nodes[closest].Location);
            workingList.Remove(closest);

            foreach (String n in workingList)
            {
                double thisDistance = BehaviorHelper.Distance(newNode.Location, m_nodes[n].Location);
                if (thisDistance < distance)
                {
                    closest = n;
                    distance = thisDistance;
                }
            }
            LocationNode closestNode = m_nodes[closest];

            LocationNode otherNode = null;
            LocationValue pointOnLine = null;
            double distanceToPointOnLine = -1;
            foreach (LocationNode node in closestNode.ConnectedNodes)
            {
                if (pointOnLine == null)
                {
                    otherNode = node;
                    pointOnLine = BehaviorHelper.ClosestPointOnLine(newNode.Location, closestNode.Location, node.Location);
                    distanceToPointOnLine = BehaviorHelper.Distance(newNode.Location, pointOnLine);
                }
                else
                {
                    LocationValue l = BehaviorHelper.ClosestPointOnLine(newNode.Location, closestNode.Location, node.Location);
                    double d = BehaviorHelper.Distance(newNode.Location, l);
                    if (d < distanceToPointOnLine)
                    {
                        otherNode = node;
                        pointOnLine = l;
                        distanceToPointOnLine = d;
                    }
                }
            }

            if (distanceToPointOnLine > 5 || true)
            {

                LocationNode insertNode = new LocationNode(String.Format("{0}_{1}_{2}", newNode.Name, closestNode.Name, otherNode.Name),
                                                           pointOnLine);
                AddNode(insertNode);
                BiConnect(insertNode.Name, closestNode.Name);
                BiConnect(insertNode.Name, otherNode.Name);
                AddNode(newNode);
                BiConnect(newNode.Name, insertNode.Name);
            }
            else
            {
                AddNode(newNode);
                BiConnect(newNode.Name, closestNode.Name);
                BiConnect(newNode.Name, otherNode.Name);
            }

        }

        public WaypointRoute GenerateWaypointRoute(String startNodeName, String endNodeName)
        {
            WaypointRoute r = new WaypointRoute();

            List<String> bestPath;
            Double distance;
            FindShortestPath(new List<string>(), startNodeName, endNodeName, out distance, out bestPath);
            //r.Add(new Waypoint(startNodeName, m_nodes[startNodeName].Location));
            foreach (String nodeName in bestPath)
            {
                r.Add(new Waypoint(nodeName, m_nodes[nodeName].Location));
            }

            return r;
        }

        public void FindShortestPath(List<String> path,String startName, String endName, out Double out_distance, out List<String> out_path)
        {
            out_path = new List<string>();
            out_distance = -1;
            path = new List<string>(path);

            path.Add(startName);

            LocationNode startNode = m_nodes[startName];
            List<LocationNode> childNodes = new List<LocationNode>(startNode.ConnectedNodes);

            List<String> bestPath = null;
            LocationNode bestNode = null;
            Double bestDistance = -1;
            foreach (LocationNode child in childNodes)
            {
                if (path.Contains(child.Name)) // ignore children that we have already been to
                {
                    continue;
                }
                if (child.Name == endName) // we've found the destination!
                {
                    out_path.Add(endName);
                    out_distance = BehaviorHelper.Distance(startNode.Location, child.Location);
                    return;
                }
                else
                {
                    List<String> tempPath = null;
                    Double tempDistance = -1;
                    FindShortestPath(path, child.Name, endName, out tempDistance, out tempPath);
                    if (tempDistance < 0)
                    {
                        continue;
                    }
                    else if (bestDistance < 0)
                    {
                        bestDistance = tempDistance;
                        bestPath = tempPath;
                        bestNode = child;
                    }
                    else if (tempDistance < bestDistance)
                    {
                        bestDistance = tempDistance;
                        bestPath = tempPath;
                        bestNode = child;
                    }
                }
            }
            if (bestDistance < 0)
            {
                return;
            }
            else
            {
                out_distance = BehaviorHelper.Distance(startNode.Location, m_nodes[bestPath[0]].Location) + bestDistance;
                //bestPath.Insert(0, startNode.Name);
                bestPath.Insert(0, bestNode.Name);
                out_path = bestPath;
            }
        }

        static public LocationGraph GenerateRouteGraph(String startName,LocationValue startLocation, String endName, LocationValue endLocation, List<WaypointRoute> routes)
        {
            LocationGraph result = new LocationGraph();

            double distanceThreshold = 1.0;

            //List<String> routeNames = new List<string>(this.Keys);
            String name;
            for (int i = 0; i < routes.Count; i++)
            {
                name = routes[i].Name;
                WaypointRoute route = routes[i];
                LocationGraph.LocationNode lastNode = null;
                foreach (Waypoint wp in route)
                {
                    LocationGraph.LocationNode node = new LocationGraph.LocationNode(wp.Name, wp.Location);
                    result.AddNode(node);
                    if (lastNode != null)
                    {
                        result.BiConnect(lastNode, node);
                    }
                    lastNode = node;
                }

                if (i > 0)
                {
                    WaypointRoute lastRoute = routes[i - 1];
                    Boolean done = false;
                    for (int j = 1; j < lastRoute.Count; j++)
                    {
                        if (done) break;
                        for (int k = 1; k < route.Count; k++)
                        {
                            if (done) break;
                            Waypoint lastRouteP1 = lastRoute[j - 1];
                            Waypoint lastRouteP2 = lastRoute[j];
                            Waypoint nextRouteP1 = route[k - 1];
                            Waypoint nextRouteP2 = route[k];
                            LocationValue intersect = BehaviorHelper.LineIntersect(lastRouteP1.Location, lastRouteP2.Location,
                                                                                   nextRouteP1.Location, nextRouteP2.Location);
                            /*if (intersect != null)
                            {
                                string newName = String.Format("Intersection_{0}_{1}_{2}_{3}", lastRouteP1.Name, lastRouteP2.Name, nextRouteP1.Name, nextRouteP2.Name);
                                LocationGraph.LocationNode intersectNode = new LocationGraph.LocationNode(newName, intersect);
                                result.AddNode(intersectNode);
                                result.BiConnect(intersectNode.Name, lastRouteP1.Name);
                                result.BiConnect(intersectNode.Name, lastRouteP2.Name);
                                result.BiConnect(intersectNode.Name, nextRouteP1.Name);
                                result.BiConnect(intersectNode.Name, nextRouteP2.Name);
                            }
                            else*/ 
                            if (BehaviorHelper.Distance(lastRouteP1.Location, nextRouteP1.Location) < distanceThreshold)
                            {
                                result.BiConnect(lastRouteP1.Name, nextRouteP1.Name);
                                done = true;
                            }
                            else if (BehaviorHelper.Distance(lastRouteP1.Location, nextRouteP2.Location) < distanceThreshold)
                            {
                                result.BiConnect(lastRouteP1.Name, nextRouteP2.Name);
                                done = true;
                            }
                            else if (BehaviorHelper.Distance(lastRouteP2.Location, nextRouteP1.Location) < distanceThreshold)
                            {
                                result.BiConnect(lastRouteP2.Name, nextRouteP1.Name);
                                done = true;
                            }
                            else if (BehaviorHelper.Distance(lastRouteP2.Location, nextRouteP2.Location) < distanceThreshold)
                            {
                                result.BiConnect(lastRouteP2.Name, nextRouteP2.Name);
                                done = true;
                            }
                        }
                    }
                }
            }
            

            //result.ProximityConnect(10);
            //result.IntersectConnect();

            LocationNode startNode = new LocationNode(startName, startLocation);
            LocationNode endNode = new LocationNode(endName, endLocation);

            result.InsertByProximity(startNode);
            result.InsertByProximity(endNode);

            return result;
        }
    }
}
