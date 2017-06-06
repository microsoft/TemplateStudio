using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Templates.UI;
using System.Windows;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class NewItemSetupViewModel : Observable
    {
        private string _header;
        public string Header
        {
            get => _header;
            set => SetProperty(ref _header, value);
        }

        private string _itemName;
        public string ItemName
        {
            get => _itemName;
            set => SetProperty(ref _itemName, value);
        }

        public ObservableCollection<ItemsGroupViewModel<TemplateInfoViewModel>> TemplateGroups { get; } = new ObservableCollection<ItemsGroupViewModel<TemplateInfoViewModel>>();

        //private TemplateInfoViewModel _templateSelected;
        //public TemplateInfoViewModel TemplateSelected
        //{
        //    get { return _templateSelected; }
        //    set
        //    {
        //        SetProperty(ref _templateSelected, value);
        //        UpdateItemName();
        //    }
        //}

        public NewItemSetupViewModel()
        {

        }

        public void Initialize()
        {
            var templates = GenContext.ToolBox.Repo.Get(t =>
                                                  (t.GetTemplateType() == TemplateType.Page || t.GetTemplateType() == TemplateType.Feature)
                                                  && t.GetFrameworkList().Contains(MainViewModel.Current.ConfigFramework)
                                                  && t.GetTemplateType() == MainViewModel.Current.ConfigTemplateType
                                                  && t.GetRightClickEnabled())
                                                  .Select(t => new TemplateInfoViewModel(t));


            var groups = templates.GroupBy(t => t.Group).Select(gr => new ItemsGroupViewModel<TemplateInfoViewModel>(gr.Key as string, gr.ToList(), OnSelectedItemChanged)).OrderBy(gr => gr.Title);

            TemplateGroups.Clear();
            TemplateGroups.AddRange(groups);
            if (TemplateGroups.Any())
            {
                var group = TemplateGroups.First();
                group.SelectedItem = group.Templates.First();
                MainViewModel.Current.NoContentVisibility = Visibility.Collapsed;
                MainViewModel.Current.EnableGoForward();
            }
            else
            {
                MainViewModel.Current.NoContentVisibility = Visibility.Visible;
            }
            UpdateHeader(templates.Count());
        }

        private void OnSelectedItemChanged(ItemsGroupViewModel<TemplateInfoViewModel> group)
        {
            foreach (var gr in TemplateGroups)
            {
                if (gr.Name == group.Name)
                {
                    var template = gr.SelectedItem as TemplateInfoViewModel;
                    if (template != null)
                    {
                        UpdateItemName(template);
                    }                    
                }
                else
                {
                    gr.CleanSelected();
                }

            }
        }

        private void UpdateHeader(int templatesCount)
        {
            if (MainViewModel.Current.ConfigTemplateType == TemplateType.Page)
            {
                Header = String.Format(StringRes.GroupPageHeader_SF, templatesCount);
            }
            else if (MainViewModel.Current.ConfigTemplateType == TemplateType.Feature)
            {
                Header = String.Format(StringRes.GroupFeatureHeader_SF, templatesCount);
            }
        }

        private void UpdateItemName(TemplateInfoViewModel item)
        {
            var validators = new List<Validator>() { new ReservedNamesValidator() };
            if (item.IsItemNameEditable)
            {
                validators.Add(new DefaultNamesValidator());
            }
            ItemName = Naming.Infer(item.DefaultName, validators);
        }
    }
}
