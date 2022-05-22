namespace PostService.Common.Types;
public interface IStartupInitializer
{
    public void AddInitializer(IInitializer initializer);
    public Task InitializeAsync();
}
