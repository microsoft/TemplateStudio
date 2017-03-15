using System;

namespace ItemNamespace.Services
{
    public delegate void OnBackgroundEnteringEventHandler(object sender, OnBackgroundEnteringEventArgs e);

    public class OnBackgroundEnteringEventArgs : EventArgs
    {
        public SuspensionState SuspensionState { get; set; }

        public Type Page { get; private set; }

        public OnBackgroundEnteringEventArgs(SuspensionState suspensionState, Type page) : base()
        {
            SuspensionState = suspensionState;
            Page = page;
        }
    }

    public class SuspensionState
    {
        public Object Data { get; set; }

        public DateTime SuspensionDate { get; set; }
    }
}
