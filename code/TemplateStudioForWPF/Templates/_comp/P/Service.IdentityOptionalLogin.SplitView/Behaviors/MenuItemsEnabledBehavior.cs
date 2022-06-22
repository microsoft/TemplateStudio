﻿using System.Windows;
using MahApps.Metro.Controls;
using Microsoft.Xaml.Behaviors;

namespace Param_RootNamespace.Behaviors;

public class MenuItemsEnabledBehavior : Behavior<HamburgerMenu>
{
    public bool IsEnabled
    {
        get { return (bool)GetValue(IsEnabledProperty); }
        set { SetValue(IsEnabledProperty, value); }
    }

    public static readonly DependencyProperty IsEnabledProperty =
        DependencyProperty.Register(
            nameof(IsEnabled),
            typeof(bool),
            typeof(MenuItemsEnabledBehavior),
            new PropertyMetadata(true, OnPropertyChanged));

    public Func<HamburgerMenuItem, bool> ApplyTo
    {
        get { return (Func<HamburgerMenuItem, bool>)GetValue(ApplyToProperty); }
        set { SetValue(ApplyToProperty, value); }
    }

    public static readonly DependencyProperty ApplyToProperty =
        DependencyProperty.Register(
            nameof(ApplyTo),
            typeof(Func<HamburgerMenuItem, bool>),
            typeof(MenuItemsEnabledBehavior),
            new PropertyMetadata(null, OnPropertyChanged));

    private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        => ((MenuItemsEnabledBehavior)d).RestrictItems();

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.Loaded += OnLoaded;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.Loaded -= OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        RestrictItems();
    }

    private void RestrictItems()
    {
        if (AssociatedObject != null)
        {
            foreach (HamburgerMenuItem menuItem in AssociatedObject.Items)
            {
                if (ApplyTo != null && ApplyTo(menuItem))
                {
                    menuItem.IsEnabled = IsEnabled;
                }
            }
        }
    }
}
