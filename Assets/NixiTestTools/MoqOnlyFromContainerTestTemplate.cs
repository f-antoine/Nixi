using NixiTestTools.Components;
using Moq;
using Nixi.Injections;
using NixiTestTools;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NixiTestTools
{
    public abstract class MoqOnlyFromContainerTestTemplate<TInjectable> : InjectableOnlyFromContainerTestTemplate<TInjectable>
        where TInjectable : OnlyFromContainerInjectable
    {
        private readonly List<MockWithDataContainerBase> mockContainers = new List<MockWithDataContainerBase>();

        [SetUp]
        public virtual void InitTests()
        {
            ResetTemplate();
            mockContainers.Clear();
        }

        [TearDown]
        public void CheckAllVerify()
        {
            foreach (MockWithDataContainerBase container in mockContainers)
            {
                container.VerifyAll();
            }

            // Check all before all no other calls
            foreach (MockWithDataContainerBase container in mockContainers)
            {
                container.VerifyAllNoOtherCalls();
            }
        }

        #region Methods
        protected Mock<T> GetMock<T>(string fieldName = "")
            where T : class
        {
            MockWithDataContainerBase mockContainerFound = GetMockContainer<T>(fieldName);

            if (mockContainerFound == null)
            {
                string errorMsg = $"Cannot find Mock with type {typeof(T)}";
                if (!string.IsNullOrEmpty(fieldName))
                {
                    errorMsg += $" and with fieldName {fieldName}";
                }
                throw new KeyNotFoundException(errorMsg);
            }

            return mockContainerFound.Mock as Mock<T>;
        }

        protected void AddEmptyMockBehavior<T>(string fieldName = "")
            where T : class
        {
            if (GetMockContainer<T>(fieldName) == null)
            {
                MockWithDataContainer<T> newContainer = new MockWithDataContainer<T>(fieldName);
                mockContainers.Add(newContainer);

                InjectFieldWithOrWithoutFieldName(fieldName, newContainer);
            }
            else
            {
                throw new InvalidOperationException("Cannot AddMock without expression twice");
            }
        }

        protected void AddMockBehavior<T>(Expression<Action<T>> expression, int nbTimes = 1, string fieldName = "")
            where T : class
        {
            MockWithDataContainerBase mockContainerFound = GetMockContainer<T>(fieldName);

            if (mockContainerFound == null)
            {
                MockWithDataContainer<T> newContainer = new MockWithDataContainer<T>(fieldName);
                newContainer.AddExpression(expression, nbTimes);
                mockContainers.Add(newContainer);

                InjectFieldWithOrWithoutFieldName(fieldName, newContainer);
            }
            else
            {
                mockContainerFound.AddExpression(expression, nbTimes);
            }
        }

        protected void AddMockBehavior<T, TResult>(Expression<Func<T, TResult>> expression, TResult result, int nbTimes = 1, string fieldName = "")
            where T : class
        {
            MockWithDataContainerBase mockContainerFound = GetMockContainer<T>(fieldName);

            if (mockContainerFound == null)
            {
                MockWithDataContainer<T> newContainer = new MockWithDataContainer<T>(fieldName);
                newContainer.AddExpression(expression, result, nbTimes);
                mockContainers.Add(newContainer);

                InjectFieldWithOrWithoutFieldName(fieldName, newContainer);
            }
            else
            {
                mockContainerFound.AddExpression(expression, result, nbTimes);
            }
        }

        protected void AddMockEventBehavior<T>(Action<T> expression, bool isAdding, int nbTimes = 1, string fieldName = "")
            where T : class
        {
            MockWithDataContainerBase mockContainerFound = GetMockContainer<T>(fieldName);

            if (mockContainerFound == null)
            {
                MockWithDataContainer<T> newContainer = new MockWithDataContainer<T>(fieldName);
                newContainer.AddExpressionEvent(expression, isAdding, nbTimes);
                mockContainers.Add(newContainer);

                InjectFieldWithOrWithoutFieldName(fieldName, newContainer);
            }
            else
            {
                mockContainerFound.AddExpressionEvent(expression, isAdding, nbTimes);
            }
        }

        private MockWithDataContainerBase GetMockContainer<T>(string fieldName = "")
        {
            return mockContainers.SingleOrDefault(x => x.TypeRegistered == typeof(T) && x.FieldName == fieldName);
        }

        private void InjectFieldWithOrWithoutFieldName<T>(string fieldName, MockWithDataContainer<T> newContainer) where T : class
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                MainInjector.InjectField(newContainer.MockToInject.Object);
            }
            else
            {
                MainInjector.InjectField(fieldName, newContainer.MockToInject.Object);
            }
        }
        #endregion Methods
    }
}