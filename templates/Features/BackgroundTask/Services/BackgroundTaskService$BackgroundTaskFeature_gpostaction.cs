private static IEnumerable<BackgroundTask> CreateInstances()
{
    var backgroundTasks = new List<BackgroundTask>();
    //^^
    //{[{
    backgroundTasks.Add(new BackgroundTaskFeature());
    //}]}

    return backgroundTasks;
}