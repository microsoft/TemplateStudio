using System;

namespace ProjectTypeApp.ProjectTypes.MasterDetail
{
    public class MenuItem
    {
        public MenuItem()
        {
            TargetType = typeof(DetailPage);
        }

		public int Id { get; set; }
		public string Title { get; set; }

		public Type TargetType { get; set; }
    }
}