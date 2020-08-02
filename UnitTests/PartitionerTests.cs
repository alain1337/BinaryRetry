using System;
using System.Linq;
using BinaryRetry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class PartitionerTests
    {
        [TestMethod]
        public void CountBasics()
        {
            Assert.AreEqual(0, Partitioner.ByCount(0, 10).Count);
            Assert.AreEqual(1, Partitioner.ByCount(1, 10).Count);
            Assert.AreEqual(9, Partitioner.ByCount(9, 10).Count);
            Assert.AreEqual(10, Partitioner.ByCount(10, 10).Count);

            var parts = Partitioner.ByCount(11, 10);
            Assert.AreEqual(11, parts.Sum(p => p.Count));
            Assert.AreEqual(10, parts.Count);
            Assert.AreEqual(0, parts[0].Start);
            Assert.AreEqual(2, parts[9].Count);
            Assert.AreEqual(9, parts[9].Start);

            Assert.AreEqual(0, Partitioner.ByCount(0, 2).Count);
            Assert.AreEqual(1, Partitioner.ByCount(1, 2).Count);
            Assert.AreEqual(2, Partitioner.ByCount(2, 2).Count);
            Assert.AreEqual(2, Partitioner.ByCount(3, 2).Count);
        }

        [TestMethod]
        public void SizeBasics()
        {
            Assert.AreEqual(1, Partitioner.ByMaxSize(500, 1000).Count);
            Assert.AreEqual(1, Partitioner.ByMaxSize(1000, 1000).Count);
            Assert.AreEqual(2, Partitioner.ByMaxSize(1001, 1000).Count);

            var parts = Partitioner.ByMaxSize(1500, 1000);
            Assert.AreEqual(2, parts.Count);
            Assert.AreEqual(0, parts[0].Start);
            Assert.AreEqual(1000, parts[0].Count);
            Assert.AreEqual(1000, parts[1].Start);
            Assert.AreEqual(500, parts[1].Count);
        }
    }
}
