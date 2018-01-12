using Microsoft.WindowsAzure.MobileServices;

namespace EasyTablesPoc.Models
{
    public abstract class EasyTableBase
    {
        public string Id { get; set; }

        [Version]
        public string Version { get; set; }
    }
}
