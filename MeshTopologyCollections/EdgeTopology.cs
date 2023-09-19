
using System.Collections.Generic;

namespace MeshTopologyCollections
{
    /// <summary>
    /// maps edge indexes with start and end point indexes. Creates a connection map
    /// </summary>
    internal class EdgeTopology
    {
        Dictionary<VertexIndex, EdgesOfPoint> _startPointToEdge;
        Dictionary<VertexIndex, EdgesOfPoint> _endPointToEdge;
        /// <summary>
        /// initialize an empty LfemEdgeList
        /// </summary>
        internal EdgeTopology()
        {
            _startPointToEdge = new Dictionary<VertexIndex, EdgesOfPoint>();
            _endPointToEdge = new Dictionary<VertexIndex, EdgesOfPoint>();
        }
        /// <summary>
        /// Registers the edge index with the start and end point indexes.
        /// </summary>
        /// <param name="edgeIndex"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        internal void RegisterEdge(int edgeIndex, VertexIndex startPoint, VertexIndex endPoint)
        {
            InsertEntity(ref _startPointToEdge, edgeIndex, startPoint, endPoint);
            InsertEntity(ref _endPointToEdge, edgeIndex, endPoint, startPoint);
        }
        void InsertEntity(ref Dictionary<VertexIndex, EdgesOfPoint> entityMap, int edgeIndex, VertexIndex refPoint, VertexIndex counterPoint)
        {
            EdgesOfPoint entities;
            if (entityMap.TryGetValue(refPoint, out entities))
            {
                entities.AddEdge(edgeIndex, counterPoint);
            }
            else
            {
                entities = new EdgesOfPoint();
                entities.AddEdge(edgeIndex, counterPoint);

                entityMap.Add(refPoint, entities);
            }
        }
        /// <summary>
        /// Checks if there is an edge starting with startIndex and ending with endIndex.
        /// İf an edge is found returns its index. Else checks in reverse.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns>Returns the index of the edge. -1 if not found.</returns>
        internal int GetEdgeIndex(VertexIndex startIndex, VertexIndex endIndex)
        {
            EdgesOfPoint edgesStarting;
            EdgesOfPoint edgesEnding;
            if (_startPointToEdge.TryGetValue(startIndex, out edgesStarting))
            {
                int edgeIndex = edgesStarting.GetEdgeIndexBy(endIndex);
                if (edgeIndex >= 0) return edgeIndex;
            }
            if (_endPointToEdge.TryGetValue(startIndex, out edgesEnding))
            {
                int edgeIndex = edgesEnding.GetEdgeIndexBy(endIndex);
                if (edgeIndex >= 0) return edgeIndex;
            }
            return -1;
        }
        class EdgesOfPoint
        {
            /// <summary>
            /// the index of the vertex of the other extend of the edge.
            /// </summary>
            internal List<int> CounterPoints { get; private set; }
            internal List<int> EdgeIndexes { get; private set; }
            internal EdgesOfPoint()
            {
                CounterPoints = new List<int>();
                EdgeIndexes = new List<int>();
            }
            internal void AddEdge(int edgeIndex, VertexIndex counterPoint )
            {
                EdgeIndexes.Add(edgeIndex);
                CounterPoints.Add(counterPoint.Value);
            }
            /// <summary>
            /// Returns the index of the edge. -1 if not found.
            /// </summary>
            /// <param name="counterPoint"></param>
            /// <returns></returns>
            internal int GetEdgeIndexBy(VertexIndex counterPoint)
            {
                int index = CounterPoints.IndexOf(counterPoint.Value);
                if(index == -1)
                {
                    return -1;
                }
                int edgeIndex = EdgeIndexes[index];
                return edgeIndex;

            }

        }
    }
}
