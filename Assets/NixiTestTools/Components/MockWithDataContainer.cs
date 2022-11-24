using NixiTestTools.Components.DelegateHandlers;
using Moq;
using System;
using System.Linq.Expressions;

namespace NixiTestTools.Components
{
    internal class MockWithDataContainer<TMock> : MockWithDataContainerBase
        where TMock : class
    {
        #region Accessors
        private readonly ExpressionHandler<TMock> expressionHandler;
        private readonly EventActionHandler<TMock> eventActionHandler;

        internal override Mock Mock => MockToInject;
        internal Mock<TMock> MockToInject { get; private set; }

        private readonly Type typeRegistered;
        internal override Type TypeRegistered => typeRegistered;

        private readonly string fieldName;
        internal override string FieldName => fieldName;
        #endregion Accessors

        #region Methods
        internal MockWithDataContainer(string fieldName = "")
        {
            MockToInject = new Mock<TMock>(MockBehavior.Strict);
            MockToInject.Name = fieldName;
            typeRegistered = typeof(TMock);
            this.fieldName = fieldName;

            expressionHandler = new ExpressionHandler<TMock>(MockToInject);
            eventActionHandler = new EventActionHandler<TMock>(MockToInject);
        }

        internal override void AddExpression<TInput>(Expression<Action<TInput>> expression, int nbTimes = 1)
        {
            Expression<Action<TMock>> mockExpression = expression as Expression<Action<TMock>>;
            expressionHandler.AddExpression(mockExpression, nbTimes);
        }

        internal override void AddExpression<TInput, TResult>(Expression<Func<TInput, TResult>> expression, TResult result, int nbTimes = 1)
        {
            Expression<Func<TMock, TResult>> mockExpression = expression as Expression<Func<TMock, TResult>>;
            expressionHandler.AddExpression(mockExpression, result, nbTimes);
        }

        // TODO5 : Tests
        internal override void AddExpressionSequence<TInput, TResult>(Expression<Func<TInput, TResult>> expression, params TResult[] results)
        {
            Expression<Func<TMock, TResult>> mockExpression = expression as Expression<Func<TMock, TResult>>;
            expressionHandler.AddExpressionSequence(mockExpression, results);
        }

        internal override void AddExpressionEvent<TInput>(Action<TInput> eventAction, bool isAdding, int nbTimes = 1)
        {
            Action<TMock> mockEventAction = eventAction as Action<TMock>;
            eventActionHandler.AddExpressionEvent(mockEventAction, isAdding, nbTimes);
        }

        internal override void VerifyAll()
        {
            expressionHandler.VerifyAll();
            eventActionHandler.VerifyAll();
        }

        internal override void VerifyAllNoOtherCalls()
        {
            MockToInject.VerifyNoOtherCalls();
        }
        #endregion Methods
    }
}