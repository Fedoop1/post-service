namespace PostService.Common.Seq.Types;

public class SeqOptionsNotDefinedException : Exception
{
    public SeqOptionsNotDefinedException(string message) : base(message) { }
}
