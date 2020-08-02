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
        public void Basics()
        {
            Assert.AreEqual(0, Partitioner.Partition(0, 10).Count);
            Assert.AreEqual(1, Partitioner.Partition(1, 10).Count);
            Assert.AreEqual(9, Partitioner.Partition(9, 10).Count);
            Assert.AreEqual(10, Partitioner.Partition(10, 10).Count);

            var parts = Partitioner.Partition(11, 10);
            Assert.AreEqual(11, parts.Sum(p => p.Count));
            Assert.AreEqual(10, parts.Count);
            Assert.AreEqual(0, parts[0].Start);
            Assert.AreEqual(2, parts[9].Count);
            Assert.AreEqual(9, parts[9].Start);

            Assert.AreEqual(0, Partitioner.Partition(0, 2).Count);
            Assert.AreEqual(1, Partitioner.Partition(1, 2).Count);
            Assert.AreEqual(2, Partitioner.Partition(2, 2).Count);
            Assert.AreEqual(2, Partitioner.Partition(3, 2).Count);
        }
    }
}
