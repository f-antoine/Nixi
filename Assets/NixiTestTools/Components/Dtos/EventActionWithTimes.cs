using System;

namespace NixiTestTools.Components.Dtos
{
    internal sealed class EventActionWithTimes<T> : DtoWithTimesBase
    {
        internal Action<T> EventAction;
        internal bool IsAdding;
    }
}