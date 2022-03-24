            .ConfigureServices((context, services) =>
            {
                // Views and ViewModels
//{[{
                services.AddTransient<wts.ItemNameDetailViewModel>();
                services.AddTransient<wts.ItemNameDetailPage>();
//}]}