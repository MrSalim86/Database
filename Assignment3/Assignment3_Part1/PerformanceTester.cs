
using System.Diagnostics;

namespace Assignment3_Part1
{
    public class PerformanceTester
    {
        public static string RunOptimisticTest()
        {
            int concurrentUsers = 50;
            var tasks = new List<Task>();
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            for (int i = 0; i < concurrentUsers; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var service = new TournamentService();
                    await service.UpdateTournamentOptimisticAsync(1, DateTime.Now.AddDays(1));
                }));
            }

            Task.WaitAll(tasks.ToArray());
            stopwatch.Stop();

            return $"[OCC] Total time for {concurrentUsers} concurrent operations: {stopwatch.ElapsedMilliseconds} ms";
        }

        public static string RunPessimisticTest()
        {
            int concurrentUsers = 50;
            var tasks = new List<Task>();
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            for (int i = 0; i < concurrentUsers; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var service = new TournamentService();
                    await service.UpdateMatchPessimisticAsync(1, 4);  // fx altid sæt winner_id = 5
                }));
            }

            Task.WaitAll(tasks.ToArray());
            stopwatch.Stop();

            return $"[PCC] Total time for {concurrentUsers} concurrent operations: {stopwatch.ElapsedMilliseconds} ms";
        } 
    }
}
