using System;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace Param_ItemNamespace.Views
{
    public sealed partial class GridViewPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        public GridViewPage()
        {
            InitializeComponent();
        }

        public ObservableCollection<Student> Source
        {
            get
            {
                // TODO UWPTemplates: Get your actual data to display in the grid
                // THe following random data is for demonstration purposes only
                var collection = new ObservableCollection<Student>();
                var rand = new Random();
                var surnames = new[] { "Thomas", "Andrews", "Lacey", "Patel", "Smith", "Jones", "Gates", "Campbell", "Williams", "Kadavy", "Franklin", "Gu", "Simpson", "Cooper" };
                var subjects = new[] { "English", "Maths", "Comp. Sci." };

                for (int i = 0; i < 10; i++)
                {
                    collection.Add(new Student
                    {
                        Name = $"{(char)rand.Next(65, 91)}. {surnames[rand.Next(0, surnames.Length)]}",
                        Age = rand.Next(10, 22),
                        Subject = subjects[rand.Next(0, subjects.Length)],
                        TestScrore = rand.Next(0, 100)
                    });
                }

                return collection;
            }
        }

        public class Student
        {
            public string Name { get; set; }
            public double Age { get; set; }
            public string Subject { get; set; }
            public int TestScrore { get; set; }
        }
    }
}
