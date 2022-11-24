using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NixiTestTools.Extensions
{
    public static class MockEnumerableExtensions
    {
        public static List<T> ConvertToObjects<T>(this List<Mock<T>> mocks)
            where T : class
        {
            return mocks.Select(x => x.Object).ToList();
        }

        public static IEnumerable<T> ConvertToObjects<T>(this IEnumerable<Mock<T>> mocks)
            where T : class
        {
            return mocks.Select(x => x.Object);
        }

        public static T[] ConvertToObjects<T>(this Mock<T>[] mocks)
            where T : class
        {
            return mocks.Select(x => x.Object).ToArray();
        }

        public static void SetupAll<T>(this IEnumerable<Mock<T>> mocks, Expression<Action<T>> expression)
            where T : class
        {
            foreach (var element in mocks)
            {
                element.Setup(expression);
            }
        }

        public static void VerifyAll<T>(this IEnumerable<Mock<T>> mocks, Expression<Action<T>> expression, Times times)
            where T : class
        {
            foreach (var element in mocks)
            {
                element.Verify(expression, times);
            }
        }

        public static void VerifyAll<T, TResult>(this IEnumerable<Mock<T>> mocks, Expression<Func<T, TResult>> expression, Times times)
            where T : class
        {
            foreach (var element in mocks)
            {
                element.Verify(expression, times);
            }
        }

        public static void VerifyAllNoOtherCalls<T>(this IEnumerable<Mock<T>> mocks)
            where T : class
        {
            foreach (var element in mocks)
            {
                element.VerifyNoOtherCalls();
            }
        }
    }
}