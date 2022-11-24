using System;
using System.Linq.Expressions;

namespace NixiTestTools.Components.Dtos
{
    internal sealed class ExpressionWithTimes<T> : DtoWithTimesBase
    {
        internal Expression<Action<T>> Expression;
        internal object ResultExpected;
    }
}