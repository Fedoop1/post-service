using System.Reflection;
using EasyNetQ;
using IMessage = PostService.Common.Types.IMessage;

namespace PostService.Common.RabbitMq.Types;

internal class TypeNameSerializer : ITypeNameSerializer
{
    public string Serialize(Type type) => type.Name;

    public Type DeSerialize(string typeName) => (Assembly.GetEntryAssembly()?.DefinedTypes
            .Where(type => type.IsClass && type.IsAssignableTo(typeof(IMessage))) ?? Array.Empty<TypeInfo>())
        .FirstOrDefault(message => message.Name.Equals(typeName)) ?? throw new RabbitMqExtensionException(
            $"Message with type {typeName} doesn't exist in {Assembly.GetEntryAssembly()?.GetName().Name} assembly");


}
