using Assets.Tests.ZombuildCoreTestsUtils.Extensions;
using Assets.Tests.ZombuildCoreTestsUtils.Templates.Components.Dtos;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Assets.Tests.ZombuildCoreTestsUtils.Templates.Components.DelegateHandlers
{
    internal sealed class ExpressionHandler<TMock>
        where TMock : class
    {
        private readonly List<ExpressionWithTimes<TMock>> expressionsWithTimes = new List<ExpressionWithTimes<TMock>>();
        private readonly Mock<TMock> mock;

        public ExpressionHandler(Mock<TMock> mock)
        {
            this.mock = mock;
        }

        internal void AddExpression(Expression<Action<TMock>> mockExpression, int nbTimes = 1)
        {
            ISetup setupFound = mock.GetSetupIfExists(mockExpression);

            if (setupFound != null)
            {
                ExpressionWithTimes<TMock> expressionFound = GetRegisteredExpression(setupFound);
                expressionFound.NbTimes += nbTimes;
            }
            else
            {
                mock.Setup(mockExpression).Verifiable();
                RegisterExpression(mockExpression, mock.Setups.Last(), nbTimes);
            }
        }

        internal void AddExpression<TResult>(Expression<Func<TMock, TResult>> mockExpression, TResult result, int nbTimes = 1)
        {
            Expression<Action<TMock>> actionExpression = Expression.Lambda<Action<TMock>>(mockExpression.Body, mockExpression.Parameters);

            ISetup setupFound = mock.GetSetupIfExists(actionExpression);

            if (setupFound != null)
            {
                ExpressionWithTimes<TMock> expressionFound = GetRegisteredExpression(setupFound);
                if (expressionFound.ResultExpected.GetHashCode() != result.GetHashCode()) // TODO5 : Check if registered as sequence ?
                {
                    throw new InvalidOperationException($"Cannot register twice expression : {mockExpression}, with different result." +
                                $"{Environment.NewLine} Origin return : {expressionFound.ResultExpected}" +
                                $"{Environment.NewLine} New return: {result}");
                }

                expressionFound.NbTimes += nbTimes;
            }
            else
            {
                mock.Setup(mockExpression).Returns(result);
                RegisterExpression(actionExpression, mock.Setups.Last(), nbTimes, result);
            }
        }

        // TODO5 : Tests
        internal void AddExpressionSequence<TResult>(Expression<Func<TMock, TResult>> mockExpression, params TResult[] results)
        {
            Expression<Action<TMock>> actionExpression = Expression.Lambda<Action<TMock>>(mockExpression.Body, mockExpression.Parameters);

            ISetup setupFound = mock.GetSetupIfExists(actionExpression);

            if (setupFound != null)
            {
                throw new InvalidOperationException($"Cannot register twice expression with sequence : {mockExpression}.");
            }
            else
            {
                Moq.Language.ISetupSequentialResult<TResult> sequence = mock.SetupSequence(mockExpression);
                foreach (TResult result in results)
                {
                    sequence = sequence.Returns(result);
                }
                RegisterExpression(actionExpression, mock.Setups.Last(), results.Length); // TODO5 : Check for no result here ?
            }
        }

        private ExpressionWithTimes<TMock> GetRegisteredExpression(ISetup setup)
        {
            return expressionsWithTimes.SingleOrDefault(x => x.SetupRegistered.ToString() == setup.ToString());
        }

        private void RegisterExpression(Expression<Action<TMock>> expression, ISetup setupRegistered, int nbTimes, object result = null)
        {
            expressionsWithTimes.Add(new ExpressionWithTimes<TMock>
            {
                SetupRegistered = setupRegistered,
                Expression = expression,
                NbTimes = nbTimes,
                ResultExpected = result
            });
        }

        internal void VerifyAll()
        {
            foreach (ExpressionWithTimes<TMock> expressionWithTimes in expressionsWithTimes)
            {
                mock.Verify(expressionWithTimes.Expression, expressionWithTimes.Times);
            }
        }
    }
}