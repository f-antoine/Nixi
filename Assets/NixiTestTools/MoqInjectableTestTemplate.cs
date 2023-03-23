using Moq;
using Nixi.Injections;
using NixiTestTools.Components;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NixiTestTools
{
    // TODO : Tests
    // TODO : Refacto
    // TODO : Comments
    // TODO3 : Separate MockBehavior from MockContainer, with a special base container ? One inject, other not
    // TODO3 : Test/check everything for Nixi v2
    public abstract class MoqInjectableTestTemplate<TInjectable> : InjectableTestTemplate<TInjectable>
        where TInjectable : MonoBehaviourInjectable
    {
        // TODO3 : Separate MockBehavior from MockContainer, with a special base container ? One inject, other not
        private readonly List<MockWithDataContainerBase> mockBehaviorContainers = new List<MockWithDataContainerBase>();
        private readonly List<MockWithDataContainerBase> mockContainers = new List<MockWithDataContainerBase>();

        public override void InitTests()
        {
            mockBehaviorContainers.Clear();
            mockContainers.Clear();
            base.InitTests();
        }

        [TearDown]
        public void CheckAllVerify()
        {
            foreach (MockWithDataContainerBase container in mockBehaviorContainers)
            {
                container.VerifyAll();
            }

            foreach (MockWithDataContainerBase container in mockContainers)
            {
                container.VerifyAll();
            }

            // Check all before all no other calls
            foreach (MockWithDataContainerBase container in mockBehaviorContainers)
            {
                container.VerifyAllNoOtherCalls();
            }

            foreach (MockWithDataContainerBase container in mockContainers)
            {
                container.VerifyAllNoOtherCalls();
            }
        }

        #region Methods MockBehavior
        protected Mock<T> GetMockBehavior<T>()
            where T : class
        {
            return GetMockBehavior<T>("");
        }

        protected Mock<T> GetMockBehavior<T>(string fieldName = "")
            where T : class
        {
            MockWithDataContainerBase mockBehaviorContainerFound = GetMockBehaviorContainer<T>(fieldName);

            if (mockBehaviorContainerFound == null)
            {
                string errorMsg = $"Cannot find MockBehavior with type {typeof(T)}";
                if (!string.IsNullOrEmpty(fieldName))
                {
                    errorMsg += $" and with fieldName {fieldName}";
                }
                throw new KeyNotFoundException(errorMsg);
            }

            return mockBehaviorContainerFound.Mock as Mock<T>;
        }

        protected void AddEmptyMockBehavior<T>(string fieldName = "")
            where T : class
        {
            if (GetMockBehaviorContainer<T>(fieldName) == null)
            {
                MockWithDataContainer<T> newContainer = new MockWithDataContainer<T>(fieldName);
                mockBehaviorContainers.Add(newContainer);

                InjectFieldWithOrWithoutFieldName(fieldName, newContainer);
            }
            else
            {
                throw new InvalidOperationException("Cannot AddMockBehavior without expression twice");
            }
        }

        protected void AddMockBehavior<T>(Expression<Action<T>> expression, int nbTimes = 1, string fieldName = "")
            where T : class
        {
            MockWithDataContainerBase mockBehaviorContainerFound = GetMockBehaviorContainer<T>(fieldName);

            if (mockBehaviorContainerFound == null)
            {
                MockWithDataContainer<T> newContainer = new MockWithDataContainer<T>(fieldName);
                newContainer.AddExpression(expression, nbTimes);
                mockBehaviorContainers.Add(newContainer);

                InjectFieldWithOrWithoutFieldName(fieldName, newContainer);
            }
            else
            {
                mockBehaviorContainerFound.AddExpression(expression, nbTimes);
            }
        }

        // TODO3 : Rebuild Expression to avoid closure problem ?
        protected void AddMockBehavior<T, TResult>(Expression<Func<T, TResult>> expression, TResult result, int nbTimes = 1, string fieldName = "")
            where T : class
        {
            MockWithDataContainerBase mockBehaviorContainerFound = GetMockBehaviorContainer<T>(fieldName);

            if (mockBehaviorContainerFound == null)
            {
                MockWithDataContainer<T> newContainer = new MockWithDataContainer<T>(fieldName);
                newContainer.AddExpression(expression, result, nbTimes);
                mockBehaviorContainers.Add(newContainer);

                InjectFieldWithOrWithoutFieldName(fieldName, newContainer);
            }
            else
            {
                mockBehaviorContainerFound.AddExpression(expression, result, nbTimes);
            }
        }

        // TODO5 : Tests
        protected void AddMockBehaviorSequence<T, TResult>(Expression<Func<T, TResult>> expression, params TResult[] results)
            where T : class
        {
            AddMockBehaviorSequence(expression, "", results);
        }

        // TODO5 : Merge both AddMockBehaviorSequence ?
        protected void AddMockBehaviorSequence<T, TResult>(Expression<Func<T, TResult>> expression, string fieldName = "", params TResult[] results)
            where T : class
        {
            MockWithDataContainerBase mockBehaviorContainerFound = GetMockBehaviorContainer<T>(fieldName);

            if (mockBehaviorContainerFound == null)
            {
                MockWithDataContainer<T> newContainer = new MockWithDataContainer<T>(fieldName);
                newContainer.AddExpressionSequence(expression, results);
                mockBehaviorContainers.Add(newContainer);

                InjectFieldWithOrWithoutFieldName(fieldName, newContainer);
            }
            else
            {
                mockBehaviorContainerFound.AddExpressionSequence(expression, results);
            }
        }

        protected void AddMockEventBehavior<T>(Action<T> expression, bool isAdding, int nbTimes = 1, string fieldName = "")
            where T : class
        {
            MockWithDataContainerBase mockBehaviorContainerFound = GetMockBehaviorContainer<T>(fieldName);

            if (mockBehaviorContainerFound == null)
            {
                MockWithDataContainer<T> newContainer = new MockWithDataContainer<T>(fieldName);
                newContainer.AddExpressionEvent(expression, isAdding, nbTimes);
                mockBehaviorContainers.Add(newContainer);

                InjectFieldWithOrWithoutFieldName(fieldName, newContainer);
            }
            else
            {
                mockBehaviorContainerFound.AddExpressionEvent(expression, isAdding, nbTimes);
            }
        }

        private MockWithDataContainerBase GetMockBehaviorContainer<T>(string fieldName = "")
        {
            return mockBehaviorContainers.SingleOrDefault(x => x.TypeRegistered == typeof(T) && x.FieldName == fieldName);
        }

        private void InjectFieldWithOrWithoutFieldName<T>(string fieldName, MockWithDataContainer<T> newContainer) where T : class
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                MainInjector.InjectField(newContainer.MockToInject.Object);
            }
            else
            {
                MainInjector.InjectField(newContainer.MockToInject.Object, fieldName);
            }
        }
        #endregion Methods MockBehavior

        #region Methods Mock
        protected Mock<T> GetMock<T>()
            where T : class
        {
            return GetMock<T>("");
        }

        protected Mock<T> GetMock<T>(string identifier = "")
            where T : class
        {
            MockWithDataContainerBase mockBehaviorContainerFound = GetMockContainer<T>(identifier);

            if (mockBehaviorContainerFound == null)
            {
                return null;
            }

            return mockBehaviorContainerFound.Mock as Mock<T>;
        }

        protected Mock<T> AddEmptyMock<T>(string identifier = "")
            where T : class
        {
            if (GetMockContainer<T>(identifier) == null)
            {
                MockWithDataContainer<T> newContainer = new MockWithDataContainer<T>(identifier);
                mockContainers.Add(newContainer);
                return newContainer.MockToInject; // TODO3 : Rename, not MockToInject because two different use cases
            }
            else
            {
                throw new InvalidOperationException("Cannot AddMock without expression twice");
            }
        }

        protected Mock<T> AddMock<T>(Expression<Action<T>> expression, int nbTimes = 1, string identifier = "")
            where T : class
        {
            MockWithDataContainerBase mockContainerFound = GetMockContainer<T>(identifier);

            if (mockContainerFound == null)
            {
                MockWithDataContainer<T> newContainer = new MockWithDataContainer<T>(identifier);
                newContainer.AddExpression(expression, nbTimes);
                mockContainers.Add(newContainer);

                // TODO3 : Rename, not MockToInject because two different use cases
                // TODO3 : Should be returned in any circumstance
                return newContainer.MockToInject;
            }
            else
            {
                mockContainerFound.AddExpression(expression, nbTimes);
                return mockContainerFound.Mock as Mock<T>; // TODO3 : Check if cast ok, should be returned in any circumstance
            }
        }

        // TODO3 : Rebuild Expression to avoid closure problem ?
        protected Mock<T> AddMock<T, TResult>(Expression<Func<T, TResult>> expression, TResult result, int nbTimes = 1, string identifier = "")
            where T : class
        {
            MockWithDataContainerBase mockContainerFound = GetMockContainer<T>(identifier);

            if (mockContainerFound == null)
            {
                MockWithDataContainer<T> newContainer = new MockWithDataContainer<T>(identifier);
                newContainer.AddExpression(expression, result, nbTimes);
                mockContainers.Add(newContainer);

                // TODO3 : Rename, not MockToInject because two different use cases
                // TODO3 : Should be returned in any circumstance
                return newContainer.MockToInject;
            }
            else
            {
                mockContainerFound.AddExpression(expression, result, nbTimes);
                return mockContainerFound.Mock as Mock<T>; // TODO3 : Check if cast ok, should be returned in any circumstance
            }
        }

        // TODO5 : Tests
        protected Mock<T> AddMockSequence<T, TResult>(Expression<Func<T, TResult>> expression, params TResult[] results)
            where T : class
        {
            return AddMockSequence(expression, "", results);
        }

        // TODO5 : Merge both AddMockBehaviorSequence ?
        protected Mock<T> AddMockSequence<T, TResult>(Expression<Func<T, TResult>> expression, string identifier = "", params TResult[] results)
            where T : class
        {
            MockWithDataContainerBase mockContainerFound = GetMockContainer<T>(identifier);

            if (mockContainerFound == null)
            {
                MockWithDataContainer<T> newContainer = new MockWithDataContainer<T>(identifier);
                newContainer.AddExpressionSequence(expression, results);
                mockContainers.Add(newContainer);

                // TODO3 : Rename, not MockToInject because two different use cases
                // TODO3 : Should be returned in any circumstance
                return newContainer.MockToInject;
            }
            else
            {
                mockContainerFound.AddExpressionSequence(expression, results);
                return mockContainerFound.Mock as Mock<T>; // TODO3 : Check if cast ok, should be returned in any circumstance
            }
        }

        protected Mock<T> AddMockEvent<T>(Action<T> expression, bool isAdding, int nbTimes = 1, string identifier = "")
            where T : class
        {
            MockWithDataContainerBase mockContainerFound = GetMockContainer<T>(identifier);

            if (mockContainerFound == null)
            {
                MockWithDataContainer<T> newContainer = new MockWithDataContainer<T>(identifier);
                newContainer.AddExpressionEvent(expression, isAdding, nbTimes);
                mockContainers.Add(newContainer);

                // TODO3 : Rename, not MockToInject because two different use cases
                // TODO3 : Should be returned in any circumstance
                return newContainer.MockToInject;
            }
            else
            {
                mockContainerFound.AddExpressionEvent(expression, isAdding, nbTimes);
                return mockContainerFound.Mock as Mock<T>; // TODO3 : Check if cast ok, should be returned in any circumstance
            }
        }

        // TODO3 : Challeng identifier notion because we are not injecting the field, is that the best choice ?
        private MockWithDataContainerBase GetMockContainer<T>(string identifier = "")
        {
            return mockContainers.SingleOrDefault(x => x.TypeRegistered == typeof(T) && x.FieldName == identifier);
        }
        #endregion Methods Mock
    }
}