using System.Reflection;
using PostService.Common.RabbitMq.Types;
using PostService.Common.Types;
using PostService.Operations.Services;

namespace PostService.Operations.Extensions.RabbitMq;
public static class OperationsRabbitMqExtensions
{
    public static void AddGenericEventHandler(this WebApplicationBuilder webBuilder) =>
        webBuilder.Services.AddSingleton(typeof(IEventHandler<>), typeof(GenericEventHandler<>));

    public static IBusSubscriber SubscribeToAllEvents<TEvent>(this IBusSubscriber busSubscriber, IEnumerable<Type> exclude, Func<IEvent, MessageException, IRejectEvent> onError = null) where TEvent : IEvent
    {
        var events = Assembly.GetExecutingAssembly().DefinedTypes
            .Where(type => type.IsClass && type.IsAssignableTo(typeof(TEvent)))
            .Where(type => !exclude.Contains(type));

        foreach (var @event in events)
        {
            busSubscriber.GetType().GetMethod(nameof(IBusSubscriber.SubscribeEvent))!.MakeGenericMethod(@event)
                .Invoke(busSubscriber, new object?[] { onError });
        }

        return busSubscriber;
    }

    public static IBusSubscriber SubscribeToAllEvents(this IBusSubscriber busSubscriber, IEnumerable<Type> exclude) =>
        busSubscriber.SubscribeToAllEvents<IEvent>(exclude);
}
