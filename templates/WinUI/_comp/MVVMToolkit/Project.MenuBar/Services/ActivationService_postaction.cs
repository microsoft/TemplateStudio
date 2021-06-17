            if (App.MainWindow.Content == null)
            {
//{[{
                _shell = Ioc.Default.GetService<ShellPage>();
//}]}
                App.MainWindow.Content = _shell ?? new Frame();
            }