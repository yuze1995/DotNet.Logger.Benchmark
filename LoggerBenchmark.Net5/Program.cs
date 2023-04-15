using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using dotNetTips.Spargine.Core.Logging;
using dotNetTips.Spargine.Extensions;
using Microsoft.Extensions.Logging;
using Perfolizer.Horology;

namespace LoggerBenchmark.Net5
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<LoggerBenchmark>();
        }
    }

    [MemoryDiagnoser]
    [MinColumn]
    [MaxColumn]
    [Config(typeof(Config))]
    public class LoggerBenchmark
    {
        private readonly ILogger<LoggerBenchmark> _logger = new LoggerFactory().CreateLogger<LoggerBenchmark>();

        private class Config : ManualConfig
        {
            public Config()
            {
                SummaryStyle = SummaryStyle.Default.WithTimeUnit(TimeUnit.Nanosecond);
            }
        }

        [Params(1, 10, 100, 1000)] public int Count { get; set; }

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
        public void LogBy_SpargineStatic()
        {
            Loop(() => LoggingHelper.FastLogger(_logger, LogLevel.Information, "Hello World!", default));
        }

        [Benchmark]
        public void LogBy_SpargineExtension()
        {
            var i = 0;
            while (i < Count)
            {
                _logger.FastLogger(LogLevel.Information, "Hello World!", default);
                i++;
            }
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
}