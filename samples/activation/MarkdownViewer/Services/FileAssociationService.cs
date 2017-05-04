using MarkdownViewer.Activation;
using MarkdownViewer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace MarkdownViewer.Services
{
    internal class FileAssociationService : ActivationHandler<File​Activated​Event​Args>
    {
        protected override async Task HandleInternalAsync(File​Activated​Event​Args args)
        {
            var file = args.Files.FirstOrDefault();

            NavigationService.Navigate(typeof(MarkdownPage), file);

            await Task.CompletedTask;
        }
    }
}
