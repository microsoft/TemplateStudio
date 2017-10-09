namespace Param_ItemNamespace.ViewModels
{
    public class wts.ItemNameViewModel : ViewModelBase
    {
        public IMediaPlaybackSource Source
        {
            get { return _source; }
//{--{
            set { Set(ref _source, value); }//}--}
//^^
//{[{
            set { SetProperty(ref _source, value); }
//}]}
        }

        private string _posterSource;

        public string PosterSource
        {
            get { return _posterSource; }
//{--{
            set { Set(ref _posterSource, value); }//}--}
//^^
//{[{
            set { SetProperty(ref _posterSource, value); }
//}]}
        }

    }
}
