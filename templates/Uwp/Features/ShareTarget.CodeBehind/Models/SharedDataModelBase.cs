namespace Param_ItemNamespace.Models
{
    public abstract class SharedDataModelBase
    {
        public string DataFormat { get; set; }

        public string PageTitle { get; set; }

        public string Title { get; set; }

        public SharedDataModelBase()
        {
        }
    }
}
