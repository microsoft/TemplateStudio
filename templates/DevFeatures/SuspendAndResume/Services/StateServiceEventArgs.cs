using System;

namespace ItemNamespace.Services
{
    public delegate void SaveStateEventHandler(object sender, SaveStateEventArgs e);

    public delegate void RestoreStateEventHandler(object sender, RestoreStateEventArgs e);

    public class RestoreStateEventArgs : EventArgs
    {
        public Object PageState { get; private set; }

        public RestoreStateEventArgs(Object pageState) : base()
        {
            PageState = pageState;
        }
    }

    public class SaveStateEventArgs : EventArgs
    {
        public Object PageState { get; set; }

        public Type Page { get; private set; }

        public SaveStateEventArgs(Object pageState, Type page) : base()
        {
            PageState = pageState;
            Page = page;
        }
    }
}
