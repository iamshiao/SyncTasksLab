using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncTasksLab
{
    class Program
    {
        public static void Main(string[] args)
        {
            ConcurrentBag<string> uncheckBlobs = new ConcurrentBag<string>();
            for (int i = 0; i < 1008; i++)
            {
                uncheckBlobs.Add($"blob {i + 1}");
            }
            Stopwatch watch = new Stopwatch();
            watch.Start();
            // m-1
            //var tasks = uncheckBlobs.Select(item =>
            //    ProcessBlob(item)
            //);

            // m-2
            //List<Task> tasks = new List<Task>();
            //Parallel.ForEach(uncheckBlobs, new ParallelOptions { MaxDegreeOfParallelism = 100 }, blob =>
            //{
            //    string blobName;
            //    uncheckBlobs.TryTake(out blobName);
            //    tasks.Add(ProcessBlob(blobName));
            //});

            // m-3
            var tasks = uncheckBlobs.Select(async item =>
                await ProcessBlob(item)
            );

            // m-5
            //using (var e = new CountdownEvent(uncheckBlobs.Count))
            //{
            //    Parallel.For(0, uncheckBlobs.Count, new ParallelOptions { MaxDegreeOfParallelism = 100 }, async i =>
            //    {
            //        string blobName;
            //        uncheckBlobs.TryTake(out blobName);
            //        await ProcessBlob(blobName);
            //        e.Signal();
            //    });

            //    e.Wait();
            //}

            // test record
            //   1000 10000
            //1  5834 30788
            //2  6464 21342
            //2a 5984 21362
            //3  5907 33767
            //4       413570 X
            //5  6197 35069

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("===============================================================All finished");
            watch.Stop();
            Console.WriteLine(watch.Elapsed.TotalMilliseconds);

            Console.ReadLine();
        }

        static async void MainAsync()
        {

        }

        static async Task ProcessBlob(string name)
        {
            await ValidateBlob(name);
            await CalculateHash(name);
        }

        static async Task ValidateBlob(string name)
        {
            Console.WriteLine($"ValidateBlob {name}");
            await Task.Delay(TimeSpan.FromSeconds(1));
            name += "v";
            Console.WriteLine("Finished ValidateBlob");
        }

        static async Task CalculateHash(string name)
        {
            Console.WriteLine($"CalculateHash {name}");
            await Task.Delay(TimeSpan.FromSeconds(2));
            name += "c";
            Console.WriteLine("Finished CalculateHash");
        }
    }
}
