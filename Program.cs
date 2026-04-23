using System.Diagnostics;
using System.Net.Sockets;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var host = "45.33.32.156";  // scanme.nmap.org
        var startingPort = 1;       // min: 0
        var endingPort = 65535;        // max: 65535

        var stopwatch = Stopwatch.StartNew();

        // await TraditionalPortScanner(host, startingPort, endingPort);

        stopwatch.Stop();

        Console.WriteLine($"\nTraditional Port Scanner: {stopwatch.ElapsedMilliseconds}ms");

        stopwatch.Restart();

        await ParallelProcessingPortScanner(host, startingPort, endingPort);

        stopwatch.Stop();

        Console.WriteLine($"Parallel Processing Port Scanner: {stopwatch.ElapsedMilliseconds}ms");
    }

    private static async Task ParallelProcessingPortScanner(string host, int startingPort, int endingPort)
    {
        var tasks = new List<Task>();

        for (var port = startingPort; port <= endingPort; port++)
        {
            tasks.Add(ScanPortAsync(host, port));
        }

        await Task.WhenAll(tasks);
    }

    private static async Task TraditionalPortScanner(string host, int startingPort, int endingPort)
    {
        for (var port = startingPort; port <= endingPort; port++)
        {
            await ScanPortAsync(host, port);
        }
    }

    private static async Task ScanPortAsync(string host, int port)
    {
        using (var client = new TcpClient())
        {
            var cancelToken = new CancellationTokenSource(1000);

            try
            {
                await client.ConnectAsync(host, port).WaitAsync(cancelToken.Token);
                if (client.Connected)
                {
                    Console.WriteLine($"Port {port} is open.");
                }
            }
            catch
            {

            }
        }
    }
}
