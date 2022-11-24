using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace NixiTestTools.Extensions
{
    public static class MockExtensions
    {
        public static Mock<T> WithSetup<T>(this Mock<T> mockToSetup, Expression<Action<T>> expression)
            where T : class
        {
            mockToSetup.Setup(expression);
            return mockToSetup;
        }

        public static Mock<T> WithSetupAdd<T>(this Mock<T> mockToSetup, Action<T> expression)
            where T : class
        {
            mockToSetup.SetupAdd(expression);
            return mockToSetup;
        }

        public static Mock<T> WithSetupRemove<T>(this Mock<T> mockToSetup, Action<T> expression)
             where T : class
        {
            mockToSetup.SetupRemove(expression);
            return mockToSetup;
        }

        public static Mock<T> WithSetup<T, TResult>(this Mock<T> mockToSetup, Expression<Func<T, TResult>> expression, TResult result)
            where T : class
        {
            mockToSetup.Setup(expression).Returns(result);
            return mockToSetup;
        }

        internal static ISetup GetSetupIfExists<TMock>(this Mock<TMock> mock, Expression<Action<TMock>> expression)
            where TMock : class
        {
            Mock<TMock> mockComparer = new Mock<TMock>(MockBehavior.Strict).WithSetup(expression);
            return mock.Setups.SingleOrDefault(x => x.ToString() == mockComparer.Setups.Single().ToString());
        }

        internal static ISetup GetSetupAddIfExists<TMock>(this Mock<TMock> mock, Action<TMock> eventAction)
            where TMock : class
        {
            Mock<TMock> mockComparer = new Mock<TMock>(MockBehavior.Strict).WithSetupAdd(eventAction);
            return mock.Setups.SingleOrDefault(x => x.ToString() == mockComparer.Setups.Single().ToString());
        }

        internal static ISetup GetSetupRemoveIfExists<TMock>(this Mock<TMock> mock, Action<TMock> eventAction)
            where TMock : class
        {
            Mock<TMock> mockComparer = new Mock<TMock>(MockBehavior.Strict).WithSetupRemove(eventAction);
            return mock.Setups.SingleOrDefault(x => x.ToString() == mockComparer.Setups.Single().ToString());
        }
    }
}