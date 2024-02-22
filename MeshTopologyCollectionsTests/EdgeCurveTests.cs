using Microsoft.VisualStudio.TestTools.UnitTesting;
using MeshTopologyCollections;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeshTopologyCollections.Tests
{
    [TestClass()]
    public class EdgeCurveTests
    {
        [TestMethod()]
        public void EdgeCurveTest()
        {
            var segments = new List<Tuple<VertexIndex, VertexIndex>>();
            segments.Add(new Tuple<VertexIndex, VertexIndex>(0, 1));
            segments.Add(new Tuple<VertexIndex, VertexIndex>(1, 2));
            segments.Add(new Tuple<VertexIndex, VertexIndex>(2, 3));
            var edgeCurve = new EdgeCurve(new TopologyVertexList(3));
            bool res = edgeCurve.Add(segments);
            Assert.Fail();
            Assert.Equals(res, true);
        }
    }
}