using System.Data.Common;
using System.Runtime.CompilerServices;

namespace MeshTopologyCollections
{
    /// <summary>
    /// index for vertex list.
    /// </summary>
    public struct VertexIndex
    {
        /// <summary>
        /// Index value of the vertex
        /// </summary>
        public int Value { get; private set; }
        /// <summary>
        /// true if this vertex is added to a collection for the first time. This property shall be handled by the collection that creates the vertex.
        /// </summary>
        public bool NewlyAdded { get; internal set; }
        /// <summary>
        /// cretes a VertexIndex with given integer value and newlyAdded as false.
        /// </summary>
        /// <param name="index"></param>
        public VertexIndex(int index)
        {
            Value = index;
            NewlyAdded = false;
        }
        /// <summary>
        /// newlyAdded is true only if the vertex is returned by VertexList directly after the vertex is added as a new vertex.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="newlyAdded"></param>
        public VertexIndex(int index, bool newlyAdded)
        {
            Value = index;
            NewlyAdded = true;
        }
        public struct PanelIndex
        {
            public int Value { get; private set; }
            public PanelIndex(int index)
            {
                Value = index;
            }
        }
        /// <summary>
        /// returns <see cref="Value"/>
        /// </summary>
        /// <param name="index"></param>
        public static implicit operator int(VertexIndex index)
        {
            return index.Value;
        }
        /// <summary>
        /// converts integer to <see cref="VertexIndex"/>
        /// </summary>
        /// <param name="index"></param>
        public static implicit operator VertexIndex(int index)
        {
            return new VertexIndex(index);
        }
        /// <summary>
        /// value based equality.
        /// </summary>
        /// <param name="vertIndex"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool operator ==(VertexIndex vertIndex, int index)
        {
            return vertIndex.Equals(index);
        }
        /// <summary>
        /// value based non-equality.
        /// </summary>
        /// <param name="vertIndex"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool operator !=(VertexIndex vertIndex, int index)
        {
            return !vertIndex.Equals(index);
        }
        /// <summary>
        /// value based equality.
        /// </summary>
        /// <param name="vertIndex"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool operator ==(VertexIndex vertIndex, VertexIndex index)
        {
            return vertIndex.Equals(index);
        }
        /// <summary>
        /// value based non-equality.
        /// </summary>
        /// <param name="vertIndex"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool operator !=(VertexIndex vertIndex, VertexIndex index)
        {
            return !vertIndex.Equals(index);
        }
        /// <summary>
        /// Value based equality.
        /// can be used against <see cref="int"/> and <see cref="VertexIndex"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override bool Equals(object index)
        {
            if (index is VertexIndex)
            {
                return Value == ((VertexIndex)index).Value;
            }
            if (index is int)
            {
                return Value == (int)index;
            }
            return false;
        }
        /// <summary>
        /// returns the hashcode of the <see cref="Value"/>
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        /// <summary>
        /// Outputs -> "Value:{<see cref="Value"/>};NewlyAdded:{<see cref="NewlyAdded"/>}"
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Value:{this.Value.ToString()};NewlyAdded:{NewlyAdded}";
        }
    }
}
