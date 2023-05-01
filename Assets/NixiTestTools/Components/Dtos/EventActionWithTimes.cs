using System;

namespace NixiTestTools.Components.Dtos
{
    // TODO : Rename
    internal sealed class EventActionWithTimes<T> : DtoWithTimesBase
    {
        internal Action<T> EventAction;
        internal bool IsAdding;
    }
}