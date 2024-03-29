﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

namespace Param_RootNamespace.ViewModels
{
    // TODO: Remove this sample page when/if it's not needed.
    // This page is an sample of how to launch a specific page in response to a protocol launch and pass it a value.
    // It is expected that you will delete this page once you have changed the handling of a protocol launch to meet
    // your needs and redirected to another of your pages.
    public class SchemeActivationSampleViewModel : ViewModelBase
    {
        public ObservableCollection<string> Parameters { get; } = new ObservableCollection<string>();

        public SchemeActivationSampleViewModel()
        {
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameters = e?.Parameter as Dictionary<string, string>;
            if (parameters != null)
            {
                Initialize(parameters);
            }
        }

        public void Initialize(Dictionary<string, string> parameters)
        {
            Parameters.Clear();
            foreach (var param in parameters)
            {
                if (param.Key == "ticks" && long.TryParse(param.Value, out long ticks))
                {
                    var dateTime = new DateTime(ticks);
                    Parameters.Add($"{param.Key}: {dateTime.ToString()}");
                }
                else
                {
                    Parameters.Add($"{param.Key}: {param.Value}");
                }
            }
        }
    }
}
