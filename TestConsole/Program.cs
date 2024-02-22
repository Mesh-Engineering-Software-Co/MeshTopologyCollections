// See https://aka.ms/new-console-template for more information
using MeshTopologyCollections;

Console.WriteLine("Hello, World!");

var segments = new List<Tuple<VertexIndex, VertexIndex>>();
segments.Add(new Tuple<VertexIndex, VertexIndex>(0, 1));
segments.Add(new Tuple<VertexIndex, VertexIndex>(1, 2));
var edgeCurve = new EdgeCurve(new TopologyVertexList(3));
bool res = edgeCurve.Add(segments);

if (!res)
{
    Console.WriteLine(res);
}
