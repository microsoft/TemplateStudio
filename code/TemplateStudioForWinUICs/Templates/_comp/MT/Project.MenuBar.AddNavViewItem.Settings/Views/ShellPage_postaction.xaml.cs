﻿        private void OnLoaded(object sender, RoutedEventArgs e)
        {
//^^
//{[{

            SettingsButton.AddHandler(UIElement.PointerPressedEvent,
                new PointerEventHandler(SettingsButton_PointerPressed), true);
            SettingsButton.AddHandler(UIElement.PointerReleasedEvent,
                new PointerEventHandler(SettingsButton_PointerReleased), true);
//}]}
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
//^^
//{[{

            SettingsButton.RemoveHandler(UIElement.PointerPressedEvent,
                (PointerEventHandler)SettingsButton_PointerPressed);
            SettingsButton.RemoveHandler(UIElement.PointerReleasedEvent,
                (PointerEventHandler)SettingsButton_PointerReleased);
//}]}
        }

//^^
//{[{
        private void SettingsButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            AnimatedIcon.SetState((UIElement)sender, "PointerOver");
        }

        private void SettingsButton_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            AnimatedIcon.SetState((UIElement)sender, "Pressed");
        }

        private void SettingsButton_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            AnimatedIcon.SetState((UIElement)sender, "Normal");
        }

        private void SettingsButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            AnimatedIcon.SetState((UIElement)sender, "Normal");
        }
//}]}
    }
}
