using System;

namespace Assets.Tests.ZombuildCoreTestsUtils.Templates.Components.Dtos
{
    internal sealed class EventActionWithTimes<T> : DtoWithTimesBase
    {
        internal Action<T> EventAction;
        internal bool IsAdding;
    }
}