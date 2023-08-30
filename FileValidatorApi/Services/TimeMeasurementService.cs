using System.Diagnostics;

namespace FileValidatorApi.Services;

public interface ITimeMeasurementService
{
    void Start();
    void Stop(string message);
}

public class TimeMeasurementService : ITimeMeasurementService
{
    private readonly Stopwatch stopwatch = new();

    public void Start()
    {
        stopwatch.Reset();
        stopwatch.Start();
    }

    public void Stop(string message)
    {
        stopwatch.Stop();
        Console.WriteLine($"{message} - took {stopwatch.Elapsed}");
    }
}