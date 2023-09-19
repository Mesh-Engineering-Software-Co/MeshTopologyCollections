namespace MeshTopologyCollections
{
    /// <summary>
    /// index for vertex list.
    /// </summary>
    public struct VertexIndex
    {
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
        public static implicit operator VertexIndex(int i)
        {
            return new VertexIndex(i);
        }
        public struct PanelIndex
        {
            public int Value { get; private set; }
            public PanelIndex(int index)
            {
                Value = index;
            }
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        public override string ToString()
        {
            return $"Value:{this.Value.ToString()};NewlyAdded:{NewlyAdded}";
        }
    }
}
