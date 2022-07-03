namespace PostService.Common.Logging.Types;

public static class ConsoleColorExtensions
{
    private const string DefaultForegroundColor = "\x1B[39m\x1B[22m";
    private const string DefaultBackgroundColor = "\x1B[49m";

    private static string GetForegroundColorEscapeCode(ConsoleColor color) =>
        color switch
        {
            ConsoleColor.Black => "\x1B[30m",
            ConsoleColor.DarkRed => "\x1B[31m",
            ConsoleColor.DarkGreen => "\x1B[32m",
            ConsoleColor.DarkYellow => "\x1B[33m",
            ConsoleColor.DarkBlue => "\x1B[34m",
            ConsoleColor.DarkMagenta => "\x1B[35m",
            ConsoleColor.DarkCyan => "\x1B[36m",
            ConsoleColor.Gray => "\x1B[37m",
            ConsoleColor.Red => "\x1B[1m\x1B[31m",
            ConsoleColor.Green => "\x1B[1m\x1B[32m",
            ConsoleColor.Yellow => "\x1B[1m\x1B[33m",
            ConsoleColor.Blue => "\x1B[1m\x1B[34m",
            ConsoleColor.Magenta => "\x1B[1m\x1B[35m",
            ConsoleColor.Cyan => "\x1B[1m\x1B[36m",
            ConsoleColor.White => "\x1B[1m\x1B[37m",

            _ => DefaultForegroundColor
        };
    private static string GetBackgroundColorEscapeCode(ConsoleColor color) =>
        color switch
        {
            ConsoleColor.Black => "\x1B[40m",
            ConsoleColor.DarkRed => "\x1B[41m",
            ConsoleColor.DarkGreen => "\x1B[42m",
            ConsoleColor.DarkYellow => "\x1B[43m",
            ConsoleColor.DarkBlue => "\x1B[44m",
            ConsoleColor.DarkMagenta => "\x1B[45m",
            ConsoleColor.DarkCyan => "\x1B[46m",
            ConsoleColor.Gray => "\x1B[47m",

            _ => DefaultBackgroundColor
        };

    // Order:
    //   1. Background color
    //   2. Foreground color
    //   3. Message
    //   4. Reset foreground color
    //   5. Reset background color
    public static void WriteWithColor(this TextWriter writer, string message, ConsoleColor backgroundColor,
        ConsoleColor foregroundColor, bool singleLine = false)
    {
        var coloredMessage =
            $"{GetBackgroundColorEscapeCode(backgroundColor)}{GetForegroundColorEscapeCode(foregroundColor)} {message} {DefaultForegroundColor}{DefaultBackgroundColor}";

        Action<string> writeFn = singleLine ? writer.Write : writer.WriteLine;

        writeFn(coloredMessage);
    }
}
