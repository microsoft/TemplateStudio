        private void OnLoaded(object sender, RoutedEventArgs e)
        {
//^^
//{[{

            ShellMenuBarSettingsButton.AddHandler(UIElement.PointerPressedEvent, new PointerEventHandler(ShellMenuBarSettingsButton_PointerPressed), true);
            ShellMenuBarSettingsButton.AddHandler(UIElement.PointerReleasedEvent, new PointerEventHandler(ShellMenuBarSettingsButton_PointerReleased), true);
//}]}
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
//^^
//{[{

            ShellMenuBarSettingsButton.RemoveHandler(UIElement.PointerPressedEvent, (PointerEventHandler)ShellMenuBarSettingsButton_PointerPressed);
            ShellMenuBarSettingsButton.RemoveHandler(UIElement.PointerReleasedEvent, (PointerEventHandler)ShellMenuBarSettingsButton_PointerReleased);
//}]}
        }

//^^
//{[{
        private void ShellMenuBarSettingsButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            AnimatedIcon.SetState((UIElement)sender, "PointerOver");
        }

        private void ShellMenuBarSettingsButton_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            AnimatedIcon.SetState((UIElement)sender, "Pressed");
        }

        private void ShellMenuBarSettingsButton_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            AnimatedIcon.SetState((UIElement)sender, "Normal");
        }

        private void ShellMenuBarSettingsButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            AnimatedIcon.SetState((UIElement)sender, "Normal");
        }
//}]}
    }
}
