using Nixi.Injections.Injectors;
using NUnit.Framework;
using ScriptExample.Genericity.Classes;
using ScriptExample.Genericity.Classes.SecondLevel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Tests.Injections
{
    internal sealed class EnumerableComponentBinderTests
    {
        EnumerableComponentBinder binder;

        [SetUp]
        public void Init()
        {
            binder = new EnumerableComponentBinder();
        }

        [Test]
        public void ChangeType_ShouldChange_Enumerable_Into_ArrayOfObjects()
        {
            // Prepare
            List<int> integers = new List<int> { 3, 2, 1 };

            // Bind
            object result = binder.ChangeType(integers.AsEnumerable(), typeof(int[]), CultureInfo.InvariantCulture);

            // Check is array
            Type resultType = result.GetType();
            Assert.True(resultType.IsArray);

            // Check length
            Array array = result as Array;
            Assert.AreEqual(integers.Count, array.Length);

            // Check same elements
            System.Collections.IEnumerable arrayEnumerable = result as System.Collections.IEnumerable;
            System.Collections.IEnumerator arrayEnumerator = arrayEnumerable.GetEnumerator();
            for (int i = 0; i < integers.Count; i++)
            {
                arrayEnumerator.MoveNext();
                Assert.AreEqual(typeof(int), arrayEnumerator.Current.GetType());
                Assert.AreEqual(integers[i], arrayEnumerator.Current);
            }
        }

        [Test]
        public void ChangeType_ShouldChange_Enumerable_Into_ListOfObjects()
        {
            // Prepare
            List<int> integers = new List<int> { 3, 2, 1 };

            // Bind
            object result = binder.ChangeType(integers.AsEnumerable(), typeof(List<int>), CultureInfo.InvariantCulture);

            // Check is List<int>
            Type resultType = result.GetType();
            Assert.True(resultType.IsGenericType);

            Type resultGenericTypeDefinition = resultType.GetGenericTypeDefinition();
            Assert.True(typeof(List<>).IsAssignableFrom(resultGenericTypeDefinition));
            Assert.AreEqual(typeof(int), resultType.GetGenericArguments()[0]);
            
            // Check values
            List<int> resultList = result as List<int>;
            Assert.AreEqual(integers.Count, resultList.Count);

            for (int i = 0; i < integers.Count; i++)
            {
                Assert.AreEqual(integers[i], resultList[i]);
            }
        }

        [Test]
        public void ChangeType_ShouldReturn_SameEnumerable()
        {
            // Prepare
            List<int> integers = new List<int> { 3, 2, 1 };

            // Bind
            object result = binder.ChangeType(integers.AsEnumerable(), typeof(IEnumerable<int>), CultureInfo.InvariantCulture);

            // Check is Enumerable<int>
            Type resultType = result.GetType();
            Assert.True(resultType.IsGenericType);
            Assert.True(result is IEnumerable<int>);

            // Check values
            IEnumerable<int> resultEnumerable = result as IEnumerable<int>;
            Assert.AreEqual(integers.Count, resultEnumerable.Count());

            for (int i = 0; i < integers.Count; i++)
            {
                Assert.AreEqual(integers[i], resultEnumerable.Skip(i).First());
            }
        }

        [Test]
        public void ChangeType_ShouldThrowException_WhenNotArrayAndNotGenericType()
        {
            Assert.Throws<NotImplementedException>(() => binder.ChangeType(123, typeof(IEnumerable<int>), CultureInfo.InvariantCulture));
        }

        [Test]
        public void ChangeType_ShouldThrowException_WhenGenericTypeButNotListOrEnumerable()
        {
            Assert.Throws<NotImplementedException>(() => binder.ChangeType(new GenericClass<int>(), typeof(IEnumerable<int>), CultureInfo.InvariantCulture));
        }

        [Test]
        public void ChangeType_ShouldThrowException_WhenGenericTypeButTwoGenericParameter()
        {
            Assert.Throws<NotImplementedException>(() => binder.ChangeType(new MultipleGenericClass<int, float>(), typeof(IEnumerable<int>), CultureInfo.InvariantCulture));
        }
    }
}