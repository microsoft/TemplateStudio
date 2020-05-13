public bool SetTheme(AppTheme? theme = null)
{
    if (currentTheme == null || currentTheme.Name != theme.ToString())
    {
//^^
//{[{
        Fluent.ThemeManager.ChangeTheme(Application.Current, $"{theme}.Blue");
//}]}
        App.Current.Properties["Theme"] = theme.ToString();
    }
}
