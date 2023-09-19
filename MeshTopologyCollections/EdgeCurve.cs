using System;
using System.Collections.Generic;
using System.Text;
using Vertex = System.Tuple<double, double, double>;

namespace MeshTopologyCollections
{
    /// <summary>
    /// Holds vertices in an ordered sequence as a curve.
    /// </summary>
    internal class EdgeCurve : IDisposable
    {
        protected TopologyVertexList _vertexList;
        protected VertexIndex _startPoint;
        protected Couples _edgeCouples;
        private void Reset()
        {
            _edgeCouples = new Couples();
            VerticesAsCurve = new List<VertexIndex>();
        }
        public EdgeCurve(VertexIndex startPoint, TopologyVertexList vertexList)
        {
            _startPoint = startPoint;
            _vertexList = vertexList;
            Reset();
        }
        /// <summary>
        /// VertexIndex items ordered in a curve sequence.
        /// </summary>
        public List<VertexIndex> VerticesAsCurve { get; set; }
        /// <summary>
        /// the end point of an edge should be start point of another edge.
        /// </summary>
        /// <param name="edges"></param>
        /// <returns>Returns false if the input list has not connected edges.</returns>
        public bool Add(List<Tuple<VertexIndex, VertexIndex>> edges)
        {
            bool res = false;
            int size = edges.Count;
            for(int i = 0;i < size; i++)
            {
                if(!_edgeCouples.AddCouple(edges[i].Item1.Value, edges[i].Item2.Value))
                {
                    Reset();
                    return false;
                }
            }
            
            int next = _startPoint.Value;
            VerticesAsCurve.Add(new VertexIndex(next));
            for (int i = 0; i< size; i++)
            {
                next = _edgeCouples.GetOther(next);
                if (next == -1)
                {
                    Reset();
                    return false;
                }
                VerticesAsCurve.Add(new VertexIndex(next));                
            }
            return res;
        }

        public void Dispose()
        {
            _edgeCouples = null;
            VerticesAsCurve = null;
        }

        internal class Couples
        {
            internal Couples()
            {
                V1 = new Dictionary<int, int>();
                V2 = new Dictionary<int, int>();
            }
            private Dictionary<int, int> V1;
            private Dictionary<int, int> V2;

            internal bool AddCouple(int v1, int v2)
            {
                if (V1.ContainsKey(v1)) return false;
                if (V2.ContainsKey(v2)) return false;
                V1.Add(v1, v2);
                V2.Add(v2, v1);
                return true;
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
