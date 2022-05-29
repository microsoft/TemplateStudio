            .ConfigureServices((context, services) =>
            {
                // Views and ViewModels
//{[{
                services.AddTransient<Param_ItemNameDetailViewModel>();
                services.AddTransient<Param_ItemNameDetailPage>();
//}]}