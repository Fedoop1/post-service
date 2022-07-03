using Microsoft.Extensions.Logging.Console;

namespace PostService.Common.Logging.Types;

public class ConsoleWrappingFormatterOptions : SimpleConsoleFormatterOptions
{
    public string AppName { get; set; }
    public bool ShowTimestamp { get; set; }
    public bool ShowAppName { get; set; }
    public bool ShowErrorLevel { get; set; }
    public bool UseCustomColors { get; set; }
    public bool ShowSource { get; set; }
    public bool ShowEventId { get; set; }
}
