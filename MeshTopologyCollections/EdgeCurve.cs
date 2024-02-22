using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Vertex = System.Tuple<double, double, double>;

namespace MeshTopologyCollections
{
    /// <summary>
    /// Holds vertices in an ordered sequence as a curve.
    /// </summary>
    public class EdgeCurve : IDisposable
    {
        /// <summary>
        /// indexed collection of vertices
        /// </summary>
        protected TopologyVertexList _vertexList;
        /// <summary>
        /// index couples representing connections.
        /// </summary>
        protected Couples _edgeCouples;
        private void Reset()
        {
            _edgeCouples = new Couples();
        }
        /// <summary>
        /// creates a new EdgeCurve.
        /// </summary>
        /// <param name="vertexList">collection of vertices.</param>
        public EdgeCurve(TopologyVertexList vertexList)
        {
            _vertexList = vertexList;
            Reset();
        }
        /// <summary>
        /// returns one of the tips of this curve
        /// </summary>
        public VertexIndex Tip_1
        {
            get
            {
                return new VertexIndex(_edgeCouples.Tip_1);
            }
        }
        /// <summary>
        /// returns one of the tips of this curve
        /// </summary>
        public VertexIndex Tip_2
        {
            get
            {
                return new VertexIndex(_edgeCouples.Tip_2);
            }
        }
        /// <summary>
        /// returns vertex correspondin ot the VertexIndex, if none found return null.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public Vertex GetVertex(VertexIndex index)
        {
            return _vertexList.GetVertex(index);
        }
        /// <summary>
        /// VertexIndex items ordered in a curve sequence.
        /// </summary>
        public List<VertexIndex> VerticesFromTip_1 { get; set; }
        /// <summary>
        /// VertexIndex items ordered in a curve sequence.
        /// </summary>
        public List<VertexIndex> VerticesFromTip_2 { get; set; }
        /// <summary>
        /// the end point of an edge should be start point of another edge. An edge is a connection between two vertices.
        /// </summary>
        /// <param name="edges"></param>
        /// <returns>Returns false if the indices in the input list are not in a correct form.</returns>
        public bool Add(List<Tuple<VertexIndex, VertexIndex>> edges)
        {
            int size = edges.Count;
            for (int i = 0; i < size; i++)
            {
                if (!_edgeCouples.AddCouple(edges[i].Item1.Value, edges[i].Item2.Value))
                {
                    Reset();
                    return false;
                }
            }
            List<int> indicesFrom_1;
            if (!_edgeCouples.ConnectedIndicesFromTip_1(out indicesFrom_1))
            {
                Reset();
                return false;
            }
            List<int> indicesFrom_2;
            if (!_edgeCouples.ConnectedIndicesFromTip_2(out indicesFrom_2))
            {
                Reset();
                return false;
            }
            VerticesFromTip_1 = new List<VertexIndex>();
            for (int i = 0; i < indicesFrom_1.Count; i++)
            {
                VerticesFromTip_1.Add(new VertexIndex(indicesFrom_1[i]));
            }
            VerticesFromTip_2 = new List<VertexIndex>();
            for (int i = 0; i < indicesFrom_2.Count; i++)
            {
                VerticesFromTip_2.Add(new VertexIndex(indicesFrom_2[i]));
            }
            return true;
        }

        public void Dispose()
        {
            _edgeCouples = null;
        }

        /// <summary>
        /// represents connections as index couples
        /// </summary>
        public class Couples
        {
            int _coupleCount;
            internal Couples()
            {
                _coupleCount = 0;
                V1 = new Dictionary<int, int>();
                V2 = new Dictionary<int, int>();
            }
            private Dictionary<int, int> V1;
            private Dictionary<int, int> V2;

            int _tip_1 = -1;
            int _tip_2 = -1;
            internal int Tip_1
            {
                get
                {
                    if (_tip_1 == -1)
                    {
                        SetTips();
                    }
                    return _tip_1;
                }
            }
            internal int Tip_2
            {
                get
                {
                    if (_tip_2 == -1)
                    {
                        SetTips();
                    }
                    return _tip_2;
                }
            }
            /// <summary>
            /// Adds a new connection.
            /// </summary>
            /// <param name="v1"></param>
            /// <param name="v2"></param>
            /// <returns></returns>
            internal bool AddCouple(int v1, int v2)
            {
                /* * V1 and V2 are two connection dictionaries.
                 * * A key index shall only map to one index vice versa.
                 * * Chain of connections represents a curve points domain.
                 * */
                // v1 and v2 may be in a reverse order.
                if (V1.ContainsKey(v1) || V2.ContainsKey(v2))
                {
                    if (V1.ContainsKey(v2) || V2.ContainsKey(v1))
                    {
                        return false;
                    }
                    V1.Add(v2, v1);
                    V2.Add(v1, v2);
                }
                else
                {
                    V1.Add(v1, v2);
                    V2.Add(v2, v1);
                }                
                _coupleCount++;
                return true;
            }

            internal bool ConnectedIndicesFromTip_1(out List<int> indices)
            {
                indices = new List<int>();
                int next = Tip_1;
                indices.Add(next);
                for (int i = 0; i < _coupleCount; i++)
                {
                    if (!V1.TryGetValue(next, out next))
                    {
                        return false;
                    }
                    indices.Add(next);
                }
                return true;
            }
            internal bool ConnectedIndicesFromTip_2(out List<int> indices)
            {
                indices = new List<int>();
                int next = Tip_2;
                indices.Add(next);
                for (int i = 0; i < _coupleCount; i++)
                {
                    if (!V2.TryGetValue(next, out next))
                    {
                        return false;
                    }
                    indices.Add(next);
                }
                return true;
            }
            void SetTips()
            {
                int tip1 = -1;
                int tip2 = -1;
                foreach (KeyValuePair<int, int> item in V1)
                {
                    if (!V2.ContainsKey(item.Key))
                    {
                        tip1 = item.Key;
                        break;
                    }
                }
                foreach (KeyValuePair<int, int> item in V2)
                {
                    if (!V1.ContainsKey(item.Key))
                    {
                        tip2 = item.Key;
                        break;
                    }
                }
                (_tip_1, _tip_2) = Tuple.Create(tip1, tip2);
            }
            /// <summary>
            /// return -1 if no value is found
            /// </summary>
            /// <param name="v"></param>
            /// <returns></returns>
            internal int GetOther(int v)
            {
                int other;
                if (V1.TryGetValue(v, out other))
                {
                    return other;
                }
                if (V2.TryGetValue(v, out other))
                {
                    return other;
                }
                return -1;
            }
        }
    }
}
