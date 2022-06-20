﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Fluent;
using Microsoft.Xaml.Behaviors;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Models;

namespace Param_RootNamespace.Behaviors;

// See how to add new Tabs and new groups in Home Tab from your pages https://github.com/microsoft/TemplateStudio/blob/main/docs/WPF/projectTypes/ribbon.md
public class RibbonTabsBehavior : Behavior<Ribbon>
{
    private INavigationService _navigationService;

    public static readonly DependencyProperty IsHomeTabProperty = DependencyProperty.RegisterAttached(
        "IsHomeTab", typeof(bool), typeof(RibbonTabsBehavior), new PropertyMetadata(default(bool)));

    public static void SetIsHomeTab(DependencyObject element, bool value)
        => element.SetValue(IsHomeTabProperty, value);

    public static bool GetIsHomeTab(DependencyObject element)
        => (bool)element.GetValue(IsHomeTabProperty);

    public static bool GetIsTabFromPage(RibbonTabItem item)
        => (bool)item.GetValue(IsTabFromPageProperty);

    public static void SetIsTabFromPage(RibbonTabItem item, bool value)
        => item.SetValue(IsTabFromPageProperty, value);

    public static readonly DependencyProperty IsTabFromPageProperty =
        DependencyProperty.RegisterAttached("IsTabFromPage", typeof(bool), typeof(RibbonTabItem), new PropertyMetadata(false));

    public static bool GetIsGroupFromPage(RibbonGroupBox item)
        => (bool)item.GetValue(IsGroupFromPageProperty);

    public static void SetIsGroupFromPage(RibbonGroupBox item, bool value)
        => item.SetValue(IsGroupFromPageProperty, value);

    public static readonly DependencyProperty IsGroupFromPageProperty =
        DependencyProperty.RegisterAttached("IsGroupFromPage", typeof(bool), typeof(RibbonGroupBox), new PropertyMetadata(false));

    public static RibbonPageConfiguration GetPageConfiguration(Page item)
        => (RibbonPageConfiguration)item.GetValue(PageConfigurationProperty);

    public static void SetPageConfiguration(Page item, RibbonPageConfiguration value)
        => item.SetValue(PageConfigurationProperty, value);

    public static readonly DependencyProperty PageConfigurationProperty =
        DependencyProperty.Register("PageConfiguration", typeof(RibbonPageConfiguration), typeof(Page), new PropertyMetadata(new RibbonPageConfiguration()));

    public void Initialize(INavigationService navigationService)
    {
        _navigationService = navigationService;
        _navigationService.Navigated += OnNavigated;
    }

    public void Unsubscribe()
    {
        if (_navigationService != null)
        {
            _navigationService.Navigated -= OnNavigated;
        }
    }

    private void OnNavigated(object sender, Type pageType)
    {
        var frame = sender as Frame;
        if (frame != null && frame.Content is Page page)
        {
            UpdateTabs(page);
        }
    }

    private void UpdateTabs(Page page)
    {
        if (page != null)
        {
            var config = GetPageConfiguration(page);
            SetupHomeGroups(config.HomeGroups);
            SetupTabs(config.Tabs);
        }
    }

    private void SetupHomeGroups(Collection<RibbonGroupBox> homeGroups)
    {
        var homeTab = AssociatedObject.Tabs.FirstOrDefault(GetIsHomeTab);
        if (homeTab == null)
        {
            return;
        }

        for (int i = homeTab.Groups.Count - 1; i >= 0; i--)
        {
            if (GetIsGroupFromPage(homeTab.Groups[i]))
            {
                homeTab.Groups.RemoveAt(i);
            }
        }

        foreach (var group in homeGroups)
        {
            if (GetIsGroupFromPage(group))
            {
                homeTab.Groups.Add(group);
            }
        }
    }

    private void SetupTabs(Collection<RibbonTabItem> tabs)
    {
        for (int i = AssociatedObject.Tabs.Count - 1; i >= 0; i--)
        {
            if (GetIsTabFromPage(AssociatedObject.Tabs[i]))
            {
                AssociatedObject.Tabs.RemoveAt(i);
            }
        }

        foreach (var tab in tabs)
        {
            if (GetIsTabFromPage(tab))
            {
                AssociatedObject.Tabs.Add(tab);
            }
        }
    }
}
