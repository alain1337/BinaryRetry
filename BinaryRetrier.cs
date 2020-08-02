using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryRetry
{
    public class BinaryRetrier<T>
    {
        public Action<IList<T>,int,int> Action { get; }
        public int SliceSize { get; }
        public int RetryPartitions { get; }

        public BinaryRetrier(Action<IList<T>, int, int> action, int sliceSize, int retryPartitions)
        {
            Action = action;
            SliceSize = sliceSize;
            RetryPartitions = retryPartitions;
        }

        public BinaryRetryResult<T> Execute(IList<T> data)
        {
            var results = new BinaryRetryResult<T>();
            foreach (var slice in Partitioner.ByMaxSize(data.Count, SliceSize))
            {
                try
                {
                    Execute(data, slice.Start, slice.Count, results);
                }
                catch
                {
                    // Ignore
                }
            }

            return results;
        }

        void Execute(IList<T> data, int start, int count, BinaryRetryResult<T> results)
        {
            if (count == 0)
                return;

            results.Executes++;
            try
            {
                Action(data, start, count);
                return;
            }
            catch
            {
                if (count == 1)
                {
                    results.FailedRecords.Add(data[start]);
                    return;
                }
            }

            foreach (var part in Partitioner.ByCount(count, RetryPartitions))
                Execute(data, start+part.Start, part.Count, results);
        }
    }

    public class BinaryRetryResult<T>
    {
        public int Executes { get; internal set; }
        public List<T> FailedRecords { get; } = new List<T>();
    }
}
