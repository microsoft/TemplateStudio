using Param_RootNamespace.Contracts.Services;

namespace Param_RootNamespace.Services;

public class MockPageService : IPageService
{
    private readonly Dictionary<string, Type> _pages = new();

    public MockPageService()
    {
    }
    public Type GetPageType(string key)
    {
        return null;
    }
}
