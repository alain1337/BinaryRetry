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

        public void Do(IList<TestData> data, int start, int count)
        {
            for (var pos=0; pos < count; pos++)
            {
                if (data[start+pos].Fail)
                {
                    FailCount++;
                    throw new Exception("Oops, told to fail");
                }
            }

            SuccessCount++;
            RecordsProcessed += count;
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
