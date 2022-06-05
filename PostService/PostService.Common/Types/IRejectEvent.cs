namespace PostService.Common.Types;
public interface IRejectEvent : IEvent
{
    public string Reason { get; set; }
}
