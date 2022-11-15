using System;
using System.Linq.Expressions;

namespace Assets.Tests.ZombuildCoreTestsUtils.Templates.Components.Dtos
{
    internal sealed class ExpressionWithTimes<T> : DtoWithTimesBase
    {
        internal Expression<Action<T>> Expression;
        internal object ResultExpected;
    }
}