using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace PostService.Common.Logging.Types;

public class AppConsoleFormatter : ConsoleFormatter
{
    private static readonly Dictionary<LogLevel, string> LogLevelStringMapper = new()
    {
        [LogLevel.Information] = "Info",
        [LogLevel.Warning] = "Warn",
        [LogLevel.Critical] = "Crit",
        [LogLevel.Error] = "Error",
        [LogLevel.None] = "None",
        [LogLevel.Trace] = "Trace",
        [LogLevel.Debug] = "Debug",
    };

    private static readonly Dictionary<LogLevel, (ConsoleColor foreground, ConsoleColor background)> LogLevelColorMapper = new()
    {
        [LogLevel.Information] = (ConsoleColor.Green, ConsoleColor.Black),
        [LogLevel.Warning] = (ConsoleColor.Yellow, ConsoleColor.White),
        [LogLevel.Critical] = (ConsoleColor.Magenta, ConsoleColor.White),
        [LogLevel.Error] = (ConsoleColor.Red, ConsoleColor.White),
        [LogLevel.None] = (ConsoleColor.White, ConsoleColor.Black),
        [LogLevel.Trace] = (ConsoleColor.Gray, ConsoleColor.Black),
        [LogLevel.Debug] = (ConsoleColor.DarkCyan, ConsoleColor.Black),
    };

    private ConsoleWrappingFormatterOptions formatterOptions;

    public AppConsoleFormatter(IOptionsMonitor<ConsoleWrappingFormatterOptions> options) : base(nameof(AppConsoleFormatter))
    {
        this.formatterOptions = options.CurrentValue;

        options.OnChange(this.ReloadFormatterOptions);
    }

    public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider scopeProvider, TextWriter textWriter)
    {
        var logMessage = logEntry.Formatter?.Invoke(logEntry.State, logEntry.Exception);

        if (logMessage is null) return;

        this.FormatMessage(logEntry, textWriter, logMessage);
    }

    private void FormatMessage<TState>(LogEntry<TState> logEntry, TextWriter writer, string message)
    {
        var builder = new StringBuilder(message.Length + 1);

        if (formatterOptions.ShowTimestamp)
        {
            builder.Append($"[{DateTime.Now.ToString("s")}] ");
        }

        if (formatterOptions.ShowAppName && !string.IsNullOrEmpty(formatterOptions.AppName))
        {
            builder.Append($"[{formatterOptions.AppName}] ");
        }

        if (formatterOptions.ShowSource)
        {
            builder.Append(formatterOptions.ShowEventId ? $"[{logEntry.Category}][{logEntry.EventId}] " : $"[{logEntry.Category}] ");
        }

        if (formatterOptions.ShowErrorLevel)
        {
            builder.Append($"[{LogLevelStringMapper[logEntry.LogLevel]}] ");
        }

        builder.Append(message);

        if (formatterOptions.UseCustomColors &&
            (formatterOptions.ColorBehavior == LoggerColorBehavior.Enabled ||
             (formatterOptions.ColorBehavior == LoggerColorBehavior.Default && !Console.IsOutputRedirected)))
        {
            var (foreground, background) = LogLevelColorMapper[logEntry.LogLevel];
            writer.WriteWithColor(builder.ToString(), background, foreground, formatterOptions.SingleLine);
            return;
        }

        if (formatterOptions.SingleLine)
        {
            writer.Write(builder);
            return;
        }

        writer.WriteLine(builder);

    }

    private void ReloadFormatterOptions(ConsoleWrappingFormatterOptions formatterOptions) => this.formatterOptions = formatterOptions;
}
