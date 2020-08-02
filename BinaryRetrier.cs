using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryRetry
{
    public class BinaryRetrier<T>
    {
        public Action<IList<T>> Action { get; }

        public BinaryRetrier(Action<IList<T>> action)
        {
            Action = action;
        }

        public BinaryRetryResult<T> Execute(IList<T> data, int sliceSize)
        {
            var results = new BinaryRetryResult<T>();
            for (var i = 0;; i++)
            {
                var slice = data.Skip(i * sliceSize).Take(sliceSize).ToList();
                if (slice.Count == 0)
                    return results;
                try
                {
                    Execute(slice, results);
                }
                catch
                {
                    // Ignore
                }
            }
        }

        void Execute(IList<T> data, BinaryRetryResult<T> results)
        {
            if (data.Count == 0)
                return;

            results.Executes++;
            try
            {
                Action(data);
                return;
            }
            catch
            {
                if (data.Count == 1)
                {
                    results.FailedRecords.AddRange(data);
                    return;
                }
            }

            Execute(data.Take(data.Count / 2).ToList(), results);
            Execute(data.Skip(data.Count / 2).ToList(), results);
        }
    }

    public class BinaryRetryResult<T>
    {
        public int Executes { get; internal set; }
        public List<T> FailedRecords { get; } = new List<T>();
    }
}
