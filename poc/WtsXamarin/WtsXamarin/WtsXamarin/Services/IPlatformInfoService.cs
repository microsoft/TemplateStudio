namespace WtsXamarin.Services
{
    public interface IPlatformInfoService
    {
        string AppName { get; }
        string AppVersion { get; }
    }
}
