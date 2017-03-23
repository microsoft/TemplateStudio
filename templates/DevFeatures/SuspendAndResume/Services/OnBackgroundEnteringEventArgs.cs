using System;

namespace ItemNamespace.Services
{
    public class OnBackgroundEnteringEventArgs : EventArgs
    {
        public SuspensionState SuspensionState { get; set; }

        public Type Target { get; private set; }

        public OnBackgroundEnteringEventArgs(SuspensionState suspensionState, Type target) : base()
        {
            SuspensionState = suspensionState;
            Target = target;
        }
    }

    public class SuspensionState
    {
        public Object Data { get; set; }

        public DateTime SuspensionDate { get; set; }
    }
}
