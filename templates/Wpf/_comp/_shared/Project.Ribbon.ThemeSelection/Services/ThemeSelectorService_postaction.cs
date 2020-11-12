public void SetTheme(AppTheme theme)
{
//^^
//{[{
    Fluent.ThemeManager.ChangeTheme(Application.Current, $"{theme}.Blue");
//}]}
    App.Current.Properties["Theme"] = theme.ToString();
}