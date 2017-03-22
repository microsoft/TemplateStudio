private IEnumerable<ActivationHandler> GetActivationHandlers()
{
    yield return Singleton<Services.SuspendAndResumeService>.Instance;
}