using System;
using System.Linq.Expressions;

namespace NixiTestTools.Components.Dtos
{
    // TODO : Rename
    internal sealed class ExpressionWithTimes<T> : DtoWithTimesBase
    {
        internal Expression<Action<T>> Expression;
        internal object ResultExpected;
    }
}