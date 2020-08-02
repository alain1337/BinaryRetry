using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryRetry
{
    public class Tester
    {
        public int SuccessCount { get; private set; }
        public int FailCount { get; private set; }
        public int RecordsProcessed { get; private set; }

        public void Do(IList<TestData> data)
        {
            if (data.Any(d => d.Fail))
            {
                FailCount++;
                throw new Exception("Oops, told to fail");
            }

            SuccessCount++;
            RecordsProcessed += data.Count;
        }
    }

    [DebuggerDisplay("Fail: {Fail}")]
    public class TestData
    {
        public TestData(bool fail)
        {
            Fail = fail;
        }

        public bool Fail { get; }
    }
}
