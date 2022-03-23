            if (App.MainWindow.Content == null)
            {
//{[{
                _shell = App.GetService<ShellPage>();
//}]}
                App.MainWindow.Content = _shell ?? new Frame();
            }