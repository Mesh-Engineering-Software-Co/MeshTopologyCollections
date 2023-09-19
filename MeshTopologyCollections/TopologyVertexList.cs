using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vertex = System.Tuple<double, double, double>;

namespace MeshTopologyCollections
{
    /// <summary>
    /// a specially managed collection of vertices. Keeps close vertices as the same vertex according to the tolerance value.
    /// </summary>
    public class TopologyVertexList : IEnumerable<KeyValuePair<VertexIndex, Vertex>>
    {
        SortedList<string, Vertex> _verticeChecker;
        Dictionary<VertexIndex, Vertex> _vertices;
        Dictionary<Vertex, VertexIndex> _indexes;
        int _counter;
        int _precision;
        /// <summary>
        /// Precision of this collection as number of digits after decimal seperator in ten base.
        /// </summary>
        public int Precision { get => _precision; }
        /// <summary>
        /// number of vertices in the collection
        /// </summary>
        public int Count { get => _vertices.Count; }
        /// <summary>
        /// Precision is used when a new item is added. 
        /// If the difference of the values of the point item is not greater then an already adde item,
        /// then the new item is accepted as already added.
        /// </summary>
        /// <param name="precision">Precision in digits of decimal system, number of digits after decimal seperator.</param>
        public TopologyVertexList(int precision)
        {
            _precision = precision;
            _verticeChecker = new SortedList<string, Vertex>();
            _vertices = new Dictionary<VertexIndex, Vertex>();
            _indexes = new Dictionary<Vertex, VertexIndex>();
            _counter = 0;
        }
        /// <summary>
        /// If the difference of the coordinates of the point is not greater then precision, a vertex added before,
        /// then the new item is accepted as already added and returns the index of already added item.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public VertexIndex AddVertex(Vertex point)
        {
            string formatter = $"F{_precision}";
            string proxy = $"{point.Item1.ToString(formatter)};" +
                $"{point.Item2.ToString(formatter)};" +
                $"{point.Item3.ToString(formatter)};";
            if (_verticeChecker.ContainsKey(proxy))
            {
                var tup = _verticeChecker[proxy];
                return _indexes[tup];
            }
            else
            {
                var index = new VertexIndex(_counter++);
                _vertices.Add(index, point);
                _verticeChecker.Add(proxy, point);
                _indexes.Add(point, index);
                return new VertexIndex(index.Value, true);
            }
        }
        /// <summary>
        /// CAUTION! Before calling this method, be sure that the vertex you are removing is not being used anywhere!
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool RemoveVertex(VertexIndex index)
        {
            bool status = false;
            if (_vertices.ContainsKey(index))
            {
                var point = _vertices[index];
                string proxy = GetStringProxy(point);
                status = true;
                status &= _verticeChecker.ContainsKey(proxy);
                status &= _indexes.ContainsKey(point);
                if (status)
                {
                    _vertices.Remove(index);
                    _verticeChecker.Remove(proxy);
                    _indexes.Remove(point);
                }
            }
            return status;
        }
        /// <summary>
        /// returns the item corresponing to the index. Throws dictionary exception if no item is found.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vertex GetVertex(VertexIndex index)
        {
            return _vertices[index];
        }
        public Vertex this[VertexIndex key]
        {
            get
            {
                return GetVertex(key);
            }
        }
        /// <summary>
        /// gets the index of the item with X, Y, Z coordiantes by converting to the args into Tuple.Create(X,Y,Z).
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        /// <returns></returns>
        public VertexIndex IndexOf(double X, double Y, double Z)
        {
            var tup = Tuple.Create(X, Y, Z);
            return IndexOf(tup);
        }
        /// <summary>
        /// gets the index of the point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public VertexIndex IndexOf(Vertex point)
        {
            string proxy = GetStringProxy(point);
            if (_verticeChecker.ContainsKey(proxy))
            {
                var tup = _verticeChecker[proxy];
                return _indexes[tup];
            }
            return new VertexIndex(-1);
        }
        /// <summary>
        /// returns the vertices in this collection as a list of tuples.
        /// </summary>
        /// <returns></returns>
        public List<Vertex> GetVertices()
        {
            return _vertices.Values.ToList();
        }
        private string GetStringProxy(Vertex point)
        {
            string formatter = $"F{_precision}";
            string proxy = $"{point.Item1.ToString(formatter)};" +
                $"{point.Item2.ToString(formatter)};" +
                $"{point.Item3.ToString(formatter)};";
            return proxy;
        }
        #region Enumaration
        public IEnumerator<KeyValuePair<VertexIndex, Vertex>> GetEnumerator()
        {
            return Enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Enumerator;
        }

        IEnumerator<KeyValuePair<VertexIndex, Vertex>> Enumerator
        {
            get => new LfemVertexEnum(this, _vertices.Keys.ToList());
        }
        #endregion
    }
    /// <summary>
    /// Enumerator of the TopologyVertexList.
    /// </summary>
    public class LfemVertexEnum : IEnumerator<KeyValuePair<VertexIndex, Vertex>>
    {
        private List<VertexIndex> _indexes;
        private TopologyVertexList _vertexList;
        int _i;
        internal LfemVertexEnum(TopologyVertexList vertexList, List<VertexIndex> indexes)
        {
            _indexes = indexes;
            _vertexList = vertexList;
            _i = -1;
        }
        public KeyValuePair<VertexIndex, Vertex> Current => new KeyValuePair<VertexIndex, Vertex>(_indexes[_i], _vertexList.GetVertex(_indexes[_i]));
        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }
        public void Dispose()
        {
            _vertexList = null;
        }

        public bool MoveNext()
        {
            _i++;
            return _i < _indexes.Count;
        }

        public void Reset()
        {
            _i = -1;
        }
    }
}
