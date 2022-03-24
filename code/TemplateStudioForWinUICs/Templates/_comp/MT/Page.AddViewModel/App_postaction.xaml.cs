            .ConfigureServices((context, services) =>
            {
                // Views and ViewModels
//{[{
                services.AddTransient<wts.ItemNameViewModel>();
                services.AddTransient<wts.ItemNamePage>();
//}]}