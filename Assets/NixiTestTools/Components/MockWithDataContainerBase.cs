using Moq;
using System;
using System.Linq.Expressions;

namespace NixiTestTools.Components
{
    // TODO : Check naming
    internal abstract class MockWithDataContainerBase
    {
        internal abstract Type TypeRegistered { get; }
        internal abstract Mock Mock { get; }
        internal abstract string FieldName { get; }

        internal abstract void AddExpression<TInput>(Expression<Action<TInput>> expression, int nbTimes = 1);
        internal abstract void AddExpression<TInput, TResult>(Expression<Func<TInput, TResult>> expression, TResult result, int nbTimes = 1);
        internal abstract void AddExpressionSequence<TInput, TResult>(Expression<Func<TInput, TResult>> expression, params TResult[] results);
        internal abstract void AddExpressionEvent<TInput>(Action<TInput> eventAction, bool isAdding, int nbTimes = 1);
        internal abstract void VerifyAll();
        internal abstract void VerifyAllNoOtherCalls();
    }
}