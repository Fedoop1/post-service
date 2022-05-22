namespace PostService.Common.Types;
public class StartupInitializer : IStartupInitializer
{
    private List<IInitializer> initializers = new ();

    public void AddInitializer(IInitializer initializer)
    {
        ArgumentNullException.ThrowIfNull(initializer);

        this.initializers.Add(initializer);
    }

    public async Task InitializeAsync() =>
        await Task.WhenAll(this.initializers.Select(initializer => initializer.InitializeAsync()));
}
