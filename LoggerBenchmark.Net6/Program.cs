using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using DotNetTips.Spargine.Core.Logging;
using Microsoft.Extensions.Logging;
using Perfolizer.Horology;

BenchmarkRunner.Run<LoggerBenchmark>();

[MemoryDiagnoser]
[MinColumn]
[MaxColumn]
[Config(typeof(Config))]
public class LoggerBenchmark
{
    private readonly ILogger<LoggerBenchmark> _logger;

    public LoggerBenchmark()
    {
        _logger = new LoggerFactory().CreateLogger<LoggerBenchmark>();
    }

    [Params(1, 10, 100, 1000)] public int Count { get; set; }

    private class Config : ManualConfig
    {
        public Config()
        {
            SummaryStyle = SummaryStyle.Default.WithTimeUnit(TimeUnit.Nanosecond);
        }
    }

    [Benchmark]
    public void LogBy_DefaultExtension()
    {
        Loop(() => _logger.LogInformation("Hello World!"));
    }

    [Benchmark]
    public void LogBy_LogMessageDefine_WithSharedMethod()
    {
        Loop(() => _loggerMessage(_logger, "Hello World!", default!));
    }

    [Benchmark]
    public void LogBy_LoggerMessageDefine_WithLocalMethod()
    {
        Action<ILogger, string, Exception> loggerMessage = LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(id: 0, nameof(loggerMessage)),
            "{Message}");

        Loop(() => loggerMessage(_logger, "Hello World!", default!));
    }

    [Benchmark]
    public void LogBy_LogMessageAttribute()
    {
        Loop(() => _logger.LogInformationExt("Hello World!"));
    }

    [Benchmark]
    public void LogBy_SpargineStatic()
    {
        Loop(() => EasyLogger.LogInformation(_logger, "Hello World!"));
    }

    private readonly Action<ILogger, string, Exception> _loggerMessage =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            eventId: new EventId(id: 0, name: nameof(_loggerMessage)),
            formatString: "{Message}");

    private void Loop(Action action)
    {
        var i = 0;
        while (i < Count)
        {
            action?.Invoke();
            i++;
        }
    }
}

public static partial class Log
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "{Message}")]
    public static partial void LogInformationExt(this ILogger logger, string Message);
}