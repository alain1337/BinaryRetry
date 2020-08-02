using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryRetry
{
    internal static class Program
    {
        static void Main()
        {
            const int dataSize = 500_000;

            Console.WriteLine($"Data Size: {dataSize:N0}");
            Console.WriteLine();

            Console.WriteLine($"{"Fail Rate",18} {"Slice Size",18} {"Partitions",18} {"Executes",18} {"Elapsed",18}");
            foreach (var failRate in new[] { 0.01, 0.001, 0.0001 })
                foreach (var sliceSize in new[] { 100, 1_000, 10_000 })
                    foreach (var partitions in new[] { 2, 3, 5, 10 })
                        Test(dataSize, failRate, sliceSize, partitions);
            Console.WriteLine();

            //Console.WriteLine("Results:");
            //Console.WriteLine();
            //Console.WriteLine($"\tData             \t{testdata.Count,10:N0}");
            //Console.WriteLine($"\tSlice Size       \t{sliceSize,10:N0}");
            //Console.WriteLine();
            //Console.WriteLine($"\tFail Rate        \t{failRate,10:P2}");
            //Console.WriteLine($"\tSuccess Count    \t{tester.SuccessCount,10:N0}");
            //Console.WriteLine($"\tFail Count       \t{tester.FailCount,10:N0}");
            //Console.WriteLine($"\tRecords Processed\t{tester.RecordsProcessed,10:N0}");
            //Console.WriteLine();
            //Console.WriteLine($"\tExecutes         \t{brr.Executes,10:N0}");
            //Console.WriteLine($"\tFailed Records   \t{brr.FailedRecords.Count,10:N0}");
            //Console.WriteLine();
        }

        static void Test(int dataSize, double failRate, int sliceSize, int partitions)
        {
            var rnd = new Random();
            var testdata = Enumerable.Range(0, dataSize).Select(i => new TestData(rnd.NextDouble() < failRate)).ToList();
            var tester = new Tester();
            var br = new BinaryRetrier<TestData>(tester.Do, sliceSize, partitions);

            var sw = Stopwatch.StartNew();
            var brr = br.Execute(testdata);
            Console.WriteLine($"{failRate,18:P2} {sliceSize,18:N0} {partitions,18:N0} {brr.Executes,18:N0} {sw.Elapsed,18}");

            if (tester.RecordsProcessed + brr.FailedRecords.Count != dataSize)
                throw new Exception("Something doesn't sum up");
        }
    }
}
