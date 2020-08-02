using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryRetry
{
    public static class Partitioner
    {
        static readonly IList<Partition> _emptyPartitions = new List<Partition>().AsReadOnly();

        public static IList<Partition> ByCount(int count, int partitions)
        {
            if (partitions < 1)
                throw new ArgumentOutOfRangeException(nameof(partitions) + " must be > 0");
            if (count < 1)
                return _emptyPartitions;

            var parts = new List<Partition>();
            var partSize = Math.Max(count / partitions, 1);
            for (var p = 0; p < partitions; p++)
            {
                var s = p * partSize;
                if (s >= count)
                    break;

                if (p == partitions - 1) // Last? Then add rest
                    parts.Add(new Partition(s, count - s));
                else
                    parts.Add(new Partition(s, Math.Min(partSize, count - s)));
            }

            return parts.AsReadOnly();
        }

        public static IList<Partition> ByMaxSize(int count, int maxSize)
        {
            if (maxSize < 1)
                throw new ArgumentOutOfRangeException(nameof(maxSize) + " must be > 0");
            if (count < 1)
                return _emptyPartitions;

            var parts = new List<Partition>();
            var s = 0;
            while (s <= count - 1)
            {
                var c = Math.Min(maxSize, count - s);
                parts.Add(new Partition(s, c));
                s += maxSize;
            }
            return parts.AsReadOnly();
        }
    }

    [DebuggerDisplay("{Start}+{Count}")]
    public class Partition
    {
        public Partition(int start, int count)
        {
            Start = start;
            Count = count;
        }

        public int Start { get; }
        public int Count { get; }
    }
}
