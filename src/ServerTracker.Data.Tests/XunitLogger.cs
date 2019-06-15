using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

//--------------------------------------------------
// Based on this StackOverflow answer: https://stackoverflow.com/a/46172875
//--------------------------------------------------

namespace ServerTracker.Data.Tests
{
    public class XunitLoggerProvider : ILoggerProvider
    {
        private ITestOutputHelper Output { get; }

        public XunitLoggerProvider(ITestOutputHelper output)
        {
            Output = output;
        }

        public ILogger CreateLogger(string categoryName) => new XunitLogger(categoryName, Output);

        public void Dispose()
        { }
    }

    public class XunitLogger : ILogger
    {
        private string CategoryName { get; }

        private ITestOutputHelper Output { get; }

        public XunitLogger(string categoryName, ITestOutputHelper output)
        {
            CategoryName = categoryName;
            Output = output;
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Output.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}][{logLevel}]({CategoryName}) {formatter(state, exception)}");
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NoopDisposable.Instance;
        }

        private class NoopDisposable : IDisposable
        {
            private static Lazy<NoopDisposable> LazyInstance { get; } = new Lazy<NoopDisposable>(() => new NoopDisposable());

            public static NoopDisposable Instance => LazyInstance.Value;

            public void Dispose()
            { }
        }
    }
}
