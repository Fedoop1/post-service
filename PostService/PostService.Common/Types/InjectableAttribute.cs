using PostService.Common.Enums;

namespace PostService.Common.Types;

[AttributeUsage(AttributeTargets.Interface)]
public class InjectableAttribute : Attribute
{
    public Scope Scope { get; }

    public InjectableAttribute(Scope scope = Scope.Scoped)
    {
        this.Scope = scope;
    }
    
}
