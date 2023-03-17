using ScriptExample.DummyElements;
using Moq;
using NixiTestTools;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests.TestTools
{
    // TODO : Next fix ? Recheck ?
    public sealed class MoqInjectableTestTemplateTests : MoqInjectableTestTemplate<DummyMoqInjectable>
    {
        #region General cases
        [Test]
        public void InitTest_ShouldResetTests()
        {
            int instanceId = MainTested.GetInstanceID();
            int injectorHash = MainInjector.GetHashCode();

            InitTests();

            Assert.AreNotEqual(instanceId, MainTested.GetInstanceID());
            Assert.AreNotEqual(injectorHash, MainInjector.GetHashCode());
        }

        [Test]
        public void GetMock_ShouldReturnMock_AndResetCorrectlyWithTestReset()
        {
            // Nothing init
            Assert.Throws<KeyNotFoundException>(() => GetMockBehavior<IDummyInterface>());

            // Inject and check call
            AddMockBehavior<IDummyInterface, int>(x => x.ValueToRetrieve, 4);
            Mock<IDummyInterface> dummyInterfaceMock = GetMockBehavior<IDummyInterface>();
            Assert.NotNull(dummyInterfaceMock);
            Assert.AreEqual(4, dummyInterfaceMock.Object.ValueToRetrieve);

            // Reset
            InitTests();
            Assert.Throws<KeyNotFoundException>(() => GetMockBehavior<IDummyInterface>());
        }

        [Test]
        public void GetMockWithName_ShouldReturnMockWithName_AndResetCorrectlyWithTestReset()
        {
            // Nothing init
            Assert.Throws<KeyNotFoundException>(() => GetMockBehavior<IOtherDummyInterface>("otherInterface"));
            Assert.Throws<KeyNotFoundException>(() => GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond"));

            // Inject and check call
            AddMockBehavior<IOtherDummyInterface, int>(x => x.ValueToRetrieve, 4, 1, "otherInterfaceSecond");
            Mock<IOtherDummyInterface> otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");

            Assert.Throws<KeyNotFoundException>(() => GetMockBehavior<IOtherDummyInterface>("otherInterface"));
            Assert.NotNull(otherMock);
            Assert.AreEqual(4, otherMock.Object.ValueToRetrieve);

            // Reset
            InitTests();
            Assert.Throws<KeyNotFoundException>(() => GetMockBehavior<IOtherDummyInterface>("otherInterface"));
            Assert.Throws<KeyNotFoundException>(() => GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond"));
        }

        [Test]
        public void FieldsWithSameType_ShouldBeMockedCorrectly()
        {
            // Nothing init
            Assert.Throws<KeyNotFoundException>(() => GetMockBehavior<IOtherDummyInterface>("otherInterface"));
            Assert.Throws<KeyNotFoundException>(() => GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond"));

            // Mock first for 2 calls
            AddMockBehavior<IOtherDummyInterface, int>(x => x.ValueToRetrieve, 111, 2, "otherInterface");
            Mock<IOtherDummyInterface> otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterface");

            // Mock second for 3 calls
            AddMockBehavior<IOtherDummyInterface, int>(x => x.ValueToRetrieve, 222, 3, "otherInterfaceSecond");
            Mock<IOtherDummyInterface> secondOtherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");

            Assert.Throws<MockException>(() => CheckAllVerify());
            Assert.AreEqual(111, otherMock.Object.ValueToRetrieve);
            Assert.AreEqual(111, otherMock.Object.ValueToRetrieve);

            Assert.Throws<MockException>(() => CheckAllVerify());
            Assert.AreEqual(222, secondOtherMock.Object.ValueToRetrieve);
            Assert.AreEqual(222, secondOtherMock.Object.ValueToRetrieve);
            Assert.AreEqual(222, secondOtherMock.Object.ValueToRetrieve);

            CheckAllVerify();
        }

        [Test]
        public void AddEmptyMockBehaviorTwice_ShouldThrowException()
        {
            AddEmptyMockBehavior<IDummyInterface>();

            // Check empty : no setup
            Mock<IDummyInterface> dummyInterfaceMock = GetMockBehavior<IDummyInterface>();
            Assert.AreEqual(0, dummyInterfaceMock.Setups.Count);
            Assert.NotNull(GetMockBehavior<IDummyInterface>());

            Assert.Throws<InvalidOperationException>(() => AddEmptyMockBehavior<IDummyInterface>());
        }

        [Test]
        public void AddEmptyMockBehavior_WithNameWhenSameType_ShouldStoreMock()
        {
            AddEmptyMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");

            Mock<IOtherDummyInterface> otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");
            Assert.NotNull(otherMock);

            Assert.Throws<KeyNotFoundException>(() => GetMockBehavior<IOtherDummyInterface>());
            Assert.Throws<KeyNotFoundException>(() => GetMockBehavior<IOtherDummyInterface>("otherInterface"));

            // Second time should throw exception
            Assert.Throws<InvalidOperationException>(() => AddEmptyMockBehavior<IOtherDummyInterface>("otherInterfaceSecond"));
        }

        [Test]
        public void AddEmptyMockBehavior_WithoutNameWhenSameType_ShouldThrowException()
        {
            Assert.Throws<TestInjectorException>(() => AddEmptyMockBehavior<IOtherDummyInterface>());
            Assert.Throws<TestInjectorException>(() => AddEmptyMockBehavior<IOtherDummyInterface>("doesNotExist"));
        }
        #endregion General cases

        #region Actions (mock without name)
        [Test]
        public void Action_AddMockBehaviorAction_ShouldRegisterMock_AndRegister_OneCall()
        {
            // Add one time Increment call required
            AddMockBehavior<IDummyInterface>(x => x.Increment());

            // Check
            Mock<IDummyInterface> dummyInterfaceMock = GetMockBehavior<IDummyInterface>();
            Assert.AreEqual(1, dummyInterfaceMock.Setups.Count);
            Assert.NotNull(dummyInterfaceMock);

            // Increment never called : exception
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Increment called once : no exception
            dummyInterfaceMock.Object.Increment();
            CheckAllVerify();
        }

        [Test]
        public void Action_AddMockBehaviorAction_ShouldRegisterMock_AndRegister_ManyCalls()
        {
            // Add one time Increment call required
            AddMockBehavior<IDummyInterface>(x => x.Increment());

            // Add 2 times Increment call required (3 total calls needed)
            AddMockBehavior<IDummyInterface>(x => x.Increment(), 2);

            // Checks
            Mock<IDummyInterface> dummyInterfaceMock = GetMockBehavior<IDummyInterface>();
            Assert.AreEqual(1, dummyInterfaceMock.Setups.Count);
            Assert.NotNull(dummyInterfaceMock);

            // Only one call : exception
            dummyInterfaceMock.Object.Increment();
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Second call : exception
            dummyInterfaceMock.Object.Increment();
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Third call : ok
            dummyInterfaceMock.Object.Increment();
            CheckAllVerify();

            // Add one more setup and call it twice
            AddMockBehavior<IDummyInterface>(x => x.Decrement(), 2);
            dummyInterfaceMock.Object.Decrement();
            dummyInterfaceMock.Object.Decrement();
            CheckAllVerify();

            Assert.AreEqual(2, dummyInterfaceMock.Setups.Count);
            dummyInterfaceMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Action_InitTests_ShouldClearAndRegisterAgain_ThenReplaceOldData()
        {
            AddMockBehavior<IDummyInterface>(x => x.Increment(), 2);
            AddMockBehavior<IDummyInterface>(x => x.Decrement(), 3);

            // Check it was registered
            Mock<IDummyInterface> dummyInterfaceMock = GetMockBehavior<IDummyInterface>();
            Assert.NotNull(dummyInterfaceMock.Object);
            Assert.AreEqual(2, dummyInterfaceMock.Setups.Count);

            // Clear
            InitTests();
            Assert.Throws<KeyNotFoundException>(() => GetMockBehavior<IDummyInterface>());

            // Check nothing wrong
            CheckAllVerify();

            // Register a second time
            AddMockBehavior<IDummyInterface>(x => x.Increment(), 1);

            dummyInterfaceMock = GetMockBehavior<IDummyInterface>();
            Assert.NotNull(dummyInterfaceMock.Object);
            Assert.AreEqual(1, dummyInterfaceMock.Setups.Count);

            // Never called
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Call and check
            dummyInterfaceMock.Object.Increment();
            CheckAllVerify();
        }
        #endregion Actions (mock without name)

        #region Funcs (mock without name)
        [Test]
        public void Func_AddMockBehavior_ShouldRegisterMock_AndRegister_OneActionCall()
        {
            // Add one time ValueToRetrieve call required
            AddMockBehavior<IDummyInterface, int>(x => x.ValueToRetrieve, 987);

            // Check
            Mock<IDummyInterface> dummyInterfaceMock = GetMockBehavior<IDummyInterface>();
            Assert.AreEqual(1, dummyInterfaceMock.Setups.Count);
            Assert.NotNull(dummyInterfaceMock);

            // ValueToRetrieve never called : exception
            Assert.Throws<MockException>(() => CheckAllVerify());

            // ValueToRetrieve called once : no exception
            Assert.AreEqual(987, dummyInterfaceMock.Object.ValueToRetrieve);
            CheckAllVerify();
        }

        [Test]
        public void Func_AddMockBehavior_ShouldRegisterMock_AndRegister_ManyCalls()
        {
            // Add one time ValueToRetrieve call required
            AddMockBehavior<IDummyInterface, int>(x => x.ValueToRetrieve, 987);

            // Add 2 times ValueToRetrieve call required (3 total calls needed)
            AddMockBehavior<IDummyInterface, int>(x => x.ValueToRetrieve, 987, 2);

            // Checks
            Mock<IDummyInterface> dummyInterfaceMock = GetMockBehavior<IDummyInterface>();
            Assert.AreEqual(1, dummyInterfaceMock.Setups.Count);
            Assert.NotNull(dummyInterfaceMock);

            // Only one call : exception
            Assert.AreEqual(987, dummyInterfaceMock.Object.ValueToRetrieve);
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Second call : exception
            Assert.AreEqual(987, dummyInterfaceMock.Object.ValueToRetrieve);
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Third call : ok
            Assert.AreEqual(987, dummyInterfaceMock.Object.ValueToRetrieve);
            CheckAllVerify();

            // Add one more setup and call it twice
            AddMockBehavior<IDummyInterface>(x => x.Decrement(), 2);
            dummyInterfaceMock.Object.Decrement();
            dummyInterfaceMock.Object.Decrement();
            CheckAllVerify();

            Assert.AreEqual(2, dummyInterfaceMock.Setups.Count);
            dummyInterfaceMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Func_InitTests_ShouldClearAndRegisterAgain_ThenReplaceOldData()
        {
            AddMockBehavior<IDummyInterface, int>(x => x.ValueToRetrieve, 987, 2);
            AddMockBehavior<IDummyInterface>(x => x.Decrement(), 3);

            // Check it was registered
            Mock<IDummyInterface> dummyInterfaceMock = GetMockBehavior<IDummyInterface>();
            Assert.NotNull(dummyInterfaceMock.Object);
            Assert.AreEqual(2, dummyInterfaceMock.Setups.Count);

            // Clear
            InitTests();
            Assert.Throws<KeyNotFoundException>(() => GetMockBehavior<IDummyInterface>());

            // Check nothing wrong
            CheckAllVerify();

            // Register a second time
            AddMockBehavior<IDummyInterface, int>(x => x.ValueToRetrieve, 321, 1);

            dummyInterfaceMock = GetMockBehavior<IDummyInterface>();
            Assert.NotNull(dummyInterfaceMock.Object);
            Assert.AreEqual(1, dummyInterfaceMock.Setups.Count);

            // Never called
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Call and check
            Assert.AreEqual(321, dummyInterfaceMock.Object.ValueToRetrieve);
            CheckAllVerify();
        }

        [Test]
        public void Func_AddMockBehavior_ShouldThrowException_WhenThereIsManyRegistrationWithDifferentReturns()
        {
            AddMockBehavior<IDummyInterface, int>(x => x.ValueToRetrieve, 987);
            Assert.Throws<InvalidOperationException>(() => AddMockBehavior<IDummyInterface, int>(x => x.ValueToRetrieve, 321));

            Mock<IDummyInterface> dummyInterfaceMock = GetMockBehavior<IDummyInterface>();
            Assert.AreEqual(987, dummyInterfaceMock.Object.ValueToRetrieve);

            AddMockBehavior<IDummyInterface, int>(x => x.ValueToRetrieve, 987);
            Assert.AreEqual(987, dummyInterfaceMock.Object.ValueToRetrieve);
        }
        #endregion Funcs (mock without name)

        #region Events (mock without name)
        [Test]
        public void EventAdd_AddMockBehaviorAction_ShouldRegisterMock_AndRegister_OneCall()
        {
            // Add one time EventAdd call required
            AddMockEventBehavior<IDummyInterface>(x => x.GotTriggered += It.IsAny<Action>(), true);

            // Check
            Mock<IDummyInterface> dummyInterfaceMock = GetMockBehavior<IDummyInterface>();
            Assert.AreEqual(1, dummyInterfaceMock.Setups.Count);
            Assert.NotNull(dummyInterfaceMock);

            // EventAdd never called : exception
            Assert.Throws<MockException>(() => CheckAllVerify());

            // EventAdd called once : no exception
            dummyInterfaceMock.Object.GotTriggered += null;

            CheckAllVerify();
        }

        [Test]
        public void EventRemove_AddMockBehaviorAction_ShouldRegisterMock_AndRegister_OneCall()
        {
            // Add one time EventRemove call required
            AddMockEventBehavior<IDummyInterface>(x => x.GotTriggered -= It.IsAny<Action>(), false);

            // Check
            Mock<IDummyInterface> dummyInterfaceMock = GetMockBehavior<IDummyInterface>();
            Assert.AreEqual(1, dummyInterfaceMock.Setups.Count);
            Assert.NotNull(dummyInterfaceMock);

            // EventRemove never called : exception
            Assert.Throws<MockException>(() => CheckAllVerify());

            // EventRemove called once : no exception
            dummyInterfaceMock.Object.GotTriggered -= null;

            CheckAllVerify();
        }

        [Test]
        public void EventAdd_AddMockBehaviorAction_ShouldRegisterMock_AndRegister_ManyCalls()
        {
            // Add one time EventAdd call required
            AddMockEventBehavior<IDummyInterface>(x => x.GotTriggered += It.IsAny<Action>(), true);

            // Add 2 times EventAdd call required (3 total calls needed)
            AddMockEventBehavior<IDummyInterface>(x => x.GotTriggered += It.IsAny<Action>(), true, 2);

            // Checks
            Mock<IDummyInterface> dummyInterfaceMock = GetMockBehavior<IDummyInterface>();
            Assert.AreEqual(1, dummyInterfaceMock.Setups.Count);
            Assert.NotNull(dummyInterfaceMock);

            // Only one call : exception
            dummyInterfaceMock.Object.GotTriggered += null;
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Second call : exception
            dummyInterfaceMock.Object.GotTriggered += null;
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Third call : ok
            dummyInterfaceMock.Object.GotTriggered += null;
            CheckAllVerify();

            // Add one more setup and call it twice
            AddMockBehavior<IDummyInterface>(x => x.Decrement(), 2);
            dummyInterfaceMock.Object.Decrement();
            dummyInterfaceMock.Object.Decrement();
            CheckAllVerify();

            Assert.AreEqual(2, dummyInterfaceMock.Setups.Count);
            dummyInterfaceMock.VerifyNoOtherCalls();
        }

        [Test]
        public void EventRemove_AddMockBehaviorAction_ShouldRegisterMock_AndRegister_ManyCalls()
        {
            // Add one time EventRemove call required
            AddMockEventBehavior<IDummyInterface>(x => x.GotTriggered -= It.IsAny<Action>(), false);

            // Add 2 times EventRemove call required (3 total calls needed)
            AddMockEventBehavior<IDummyInterface>(x => x.GotTriggered -= It.IsAny<Action>(), false, 2);

            // Checks
            Mock<IDummyInterface> dummyInterfaceMock = GetMockBehavior<IDummyInterface>();
            Assert.AreEqual(1, dummyInterfaceMock.Setups.Count);
            Assert.NotNull(dummyInterfaceMock);

            // Only one call : exception
            dummyInterfaceMock.Object.GotTriggered -= null;
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Second call : exception
            dummyInterfaceMock.Object.GotTriggered -= null;
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Third call : ok
            dummyInterfaceMock.Object.GotTriggered -= null;
            CheckAllVerify();

            // Add one more setup and call it twice
            AddMockBehavior<IDummyInterface>(x => x.Decrement(), 2);
            dummyInterfaceMock.Object.Decrement();
            dummyInterfaceMock.Object.Decrement();
            CheckAllVerify();

            Assert.AreEqual(2, dummyInterfaceMock.Setups.Count);
            dummyInterfaceMock.VerifyNoOtherCalls();
        }

        [Test]
        public void EventAdd_InitTests_ShouldClearAndRegisterAgain_ThenReplaceOldData()
        {
            AddMockEventBehavior<IDummyInterface>(x => x.GotTriggered -= It.IsAny<Action>(), false, 2);
            AddMockEventBehavior<IDummyInterface>(x => x.GotTriggered += It.IsAny<Action>(), true, 3);

            // Check it was registered
            Mock<IDummyInterface> dummyInterfaceMock = GetMockBehavior<IDummyInterface>();
            Assert.NotNull(dummyInterfaceMock.Object);
            Assert.AreEqual(2, dummyInterfaceMock.Setups.Count);

            // Clear
            InitTests();
            Assert.Throws<KeyNotFoundException>(() => GetMockBehavior<IDummyInterface>());

            // Check nothing wrong
            CheckAllVerify();

            // Register a second time
            AddMockEventBehavior<IDummyInterface>(x => x.GotTriggered += It.IsAny<Action>(), true);

            dummyInterfaceMock = GetMockBehavior<IDummyInterface>();
            Assert.NotNull(dummyInterfaceMock.Object);
            Assert.AreEqual(1, dummyInterfaceMock.Setups.Count);

            // Never called
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Call and check
            dummyInterfaceMock.Object.GotTriggered += null;
            CheckAllVerify();
        }

        [Test]
        public void EventRemove_InitTests_ShouldClearAndRegisterAgain_ThenReplaceOldData()
        {
            AddMockEventBehavior<IDummyInterface>(x => x.GotTriggered -= It.IsAny<Action>(), false, 2);
            AddMockEventBehavior<IDummyInterface>(x => x.GotTriggered += It.IsAny<Action>(), true, 3);

            // Check it was registered
            Mock<IDummyInterface> dummyInterfaceMock = GetMockBehavior<IDummyInterface>();
            Assert.NotNull(dummyInterfaceMock.Object);
            Assert.AreEqual(2, dummyInterfaceMock.Setups.Count);

            // Clear
            InitTests();
            Assert.Throws<KeyNotFoundException>(() => GetMockBehavior<IDummyInterface>());

            // Check nothing wrong
            CheckAllVerify();

            // Register a second time
            AddMockEventBehavior<IDummyInterface>(x => x.GotTriggered -= It.IsAny<Action>(), false);

            dummyInterfaceMock = GetMockBehavior<IDummyInterface>();
            Assert.NotNull(dummyInterfaceMock.Object);
            Assert.AreEqual(1, dummyInterfaceMock.Setups.Count);

            // Never called
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Call and check
            dummyInterfaceMock.Object.GotTriggered -= null;
            CheckAllVerify();
        }

        [Test]
        public void AddEvent_ShouldBeDifferentFromRemoveEvent()
        {
            AddMockEventBehavior<IDummyInterface>(x => x.GotTriggered += It.IsAny<Action>(), true);

            Mock<IDummyInterface> dummyInterfaceMock = GetMockBehavior<IDummyInterface>();
            Assert.Throws<MockException>(() => dummyInterfaceMock.Object.GotTriggered -= null);

            InitTests();
        }

        [Test]
        public void RemoveEvent_ShouldBeDifferentFromAddEvent()
        {
            AddMockEventBehavior<IDummyInterface>(x => x.GotTriggered -= It.IsAny<Action>(), false);

            Mock<IDummyInterface> dummyInterfaceMock = GetMockBehavior<IDummyInterface>();
            Assert.Throws<MockException>(() => dummyInterfaceMock.Object.GotTriggered += null);

            InitTests();
        }

        [Test]
        public void AddAndRemoveEvent_ShouldBeDifferents_WhenBothRegistered()
        {
            AddMockEventBehavior<IDummyInterface>(x => x.GotTriggered += It.IsAny<Action>(), true, 3);
            AddMockEventBehavior<IDummyInterface>(x => x.GotTriggered -= It.IsAny<Action>(), false, 2);

            Mock<IDummyInterface> dummyInterfaceMock = GetMockBehavior<IDummyInterface>();

            Assert.Throws<MockException>(() => CheckAllVerify());
            dummyInterfaceMock.Object.GotTriggered += null;
            dummyInterfaceMock.Object.GotTriggered += null;
            dummyInterfaceMock.Object.GotTriggered += null;

            Assert.Throws<MockException>(() => CheckAllVerify());
            dummyInterfaceMock.Object.GotTriggered -= null;
            dummyInterfaceMock.Object.GotTriggered -= null;

            CheckAllVerify();
        }

        [Test]
        public void AddAndRemoveEvent_ShouldBeDifferents_WhenBothRegistered_ForDistinctCases()
        {
            AddMockEventBehavior<IDummyInterface>(x => x.GotTriggered += It.IsAny<Action>(), true, 3);
            AddMockEventBehavior<IDummyInterface>(x => x.GotTriggered -= It.IsAny<Action>(), false, 2);

            AddMockEventBehavior<IDummyInterface>(x => x.GotTriggeredWithInt += null, true, 4);
            AddMockEventBehavior<IDummyInterface>(x => x.GotTriggeredWithInt -= null, false, 5);

            static void action(int arg) => Console.WriteLine("");
            AddMockEventBehavior<IDummyInterface>(x => x.GotTriggeredWithInt += action, true, 6);
            AddMockEventBehavior<IDummyInterface>(x => x.GotTriggeredWithInt -= action, false, 7);

            Mock<IDummyInterface> dummyInterfaceMock = GetMockBehavior<IDummyInterface>();

            Assert.Throws<MockException>(() => CheckAllVerify());
            dummyInterfaceMock.Object.GotTriggered += null;
            dummyInterfaceMock.Object.GotTriggered += null;
            dummyInterfaceMock.Object.GotTriggered += null;

            Assert.Throws<MockException>(() => CheckAllVerify());
            dummyInterfaceMock.Object.GotTriggered -= null;
            dummyInterfaceMock.Object.GotTriggered -= null;

            Assert.Throws<MockException>(() => CheckAllVerify());
            for (int i = 0; i++ < 4;)
                dummyInterfaceMock.Object.GotTriggeredWithInt += null;

            Assert.Throws<MockException>(() => CheckAllVerify());
            for (int i = 0; i++ < 5;)
                dummyInterfaceMock.Object.GotTriggeredWithInt -= null;

            Assert.Throws<MockException>(() => CheckAllVerify());
            for (int i = 0; i++ < 6;)
                dummyInterfaceMock.Object.GotTriggeredWithInt += action;

            Assert.Throws<MockException>(() => CheckAllVerify());
            for (int i = 0; i++ < 7;)
                dummyInterfaceMock.Object.GotTriggeredWithInt -= action;

            CheckAllVerify();
        }
        #endregion Events (mock without name)

        #region Actions (mock with name)
        [Test]
        public void WithNameAction_AddMockBehaviorAction_ShouldRegisterMock_AndRegister_OneCall()
        {
            // Add one time Increment call required
            AddMockBehavior<IOtherDummyInterface>(x => x.Increment(), 1, "otherInterfaceSecond");

            // Check
            Mock<IOtherDummyInterface> otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");
            Assert.AreEqual(1, otherMock.Setups.Count);
            Assert.NotNull(otherMock);

            // Increment never called : exception
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Increment called once : no exception
            otherMock.Object.Increment();
            CheckAllVerify();
        }

        [Test]
        public void WithNameAction_AddMockBehaviorAction_ShouldRegisterMock_AndRegister_ManyCalls()
        {
            // Add one time Increment call required
            AddMockBehavior<IOtherDummyInterface>(x => x.Increment(), 1, "otherInterfaceSecond");

            // Add 2 times Increment call required (3 total calls needed)
            AddMockBehavior<IOtherDummyInterface>(x => x.Increment(), 2, "otherInterfaceSecond");

            // Checks
            Mock<IOtherDummyInterface> otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");
            Assert.AreEqual(1, otherMock.Setups.Count);
            Assert.NotNull(otherMock);

            // Only one call : exception
            otherMock.Object.Increment();
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Second call : exception
            otherMock.Object.Increment();
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Third call : ok
            otherMock.Object.Increment();
            CheckAllVerify();

            // Add one more setup and call it twice
            AddMockBehavior<IOtherDummyInterface>(x => x.Decrement(), 2, "otherInterfaceSecond");
            otherMock.Object.Decrement();
            otherMock.Object.Decrement();
            CheckAllVerify();

            Assert.AreEqual(2, otherMock.Setups.Count);
            otherMock.VerifyNoOtherCalls();
        }

        [Test]
        public void WithNameAction_InitTests_ShouldClearAndRegisterAgain_ThenReplaceOldData()
        {
            AddMockBehavior<IOtherDummyInterface>(x => x.Increment(), 2, "otherInterfaceSecond");
            AddMockBehavior<IOtherDummyInterface>(x => x.Decrement(), 3, "otherInterfaceSecond");

            // Check it was registered
            Mock<IOtherDummyInterface> otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");
            Assert.NotNull(otherMock.Object);
            Assert.AreEqual(2, otherMock.Setups.Count);

            // Clear
            InitTests();
            Assert.Throws<KeyNotFoundException>(() => GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond"));

            // Check nothing wrong
            CheckAllVerify();

            // Register a second time
            AddMockBehavior<IOtherDummyInterface>(x => x.Increment(), 1, "otherInterfaceSecond");

            otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");
            Assert.NotNull(otherMock.Object);
            Assert.AreEqual(1, otherMock.Setups.Count);

            // Never called
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Call and check
            otherMock.Object.Increment();
            CheckAllVerify();
        }
        #endregion Actions (mock with name)

        #region Funcs (mock with name)
        [Test]
        public void WithNameFunc_AddMockBehavior_ShouldRegisterMock_AndRegister_OneActionCall()
        {
            // Add one time ValueToRetrieve call required
            AddMockBehavior<IOtherDummyInterface, int>(x => x.ValueToRetrieve, 987, 1, "otherInterfaceSecond");

            // Check
            Mock<IOtherDummyInterface> otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");
            Assert.AreEqual(1, otherMock.Setups.Count);
            Assert.NotNull(otherMock);

            // ValueToRetrieve never called : exception
            Assert.Throws<MockException>(() => CheckAllVerify());

            // ValueToRetrieve called once : no exception
            Assert.AreEqual(987, otherMock.Object.ValueToRetrieve);
            CheckAllVerify();
        }

        [Test]
        public void WithNameFunc_AddMockBehavior_ShouldRegisterMock_AndRegister_ManyCalls()
        {
            // Add one time ValueToRetrieve call required
            AddMockBehavior<IOtherDummyInterface, int>(x => x.ValueToRetrieve, 987, 1, "otherInterfaceSecond");

            // Add 2 times ValueToRetrieve call required (3 total calls needed)
            AddMockBehavior<IOtherDummyInterface, int>(x => x.ValueToRetrieve, 987, 2, "otherInterfaceSecond");

            // Checks
            Mock<IOtherDummyInterface> otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");
            Assert.AreEqual(1, otherMock.Setups.Count);
            Assert.NotNull(otherMock);

            // Only one call : exception
            Assert.AreEqual(987, otherMock.Object.ValueToRetrieve);
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Second call : exception
            Assert.AreEqual(987, otherMock.Object.ValueToRetrieve);
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Third call : ok
            Assert.AreEqual(987, otherMock.Object.ValueToRetrieve);
            CheckAllVerify();

            // Add one more setup and call it twice
            AddMockBehavior<IOtherDummyInterface>(x => x.Decrement(), 2, "otherInterfaceSecond");
            otherMock.Object.Decrement();
            otherMock.Object.Decrement();
            CheckAllVerify();

            Assert.AreEqual(2, otherMock.Setups.Count);
            otherMock.VerifyNoOtherCalls();
        }

        [Test]
        public void WithNameFunc_InitTests_ShouldClearAndRegisterAgain_ThenReplaceOldData()
        {
            AddMockBehavior<IOtherDummyInterface, int>(x => x.ValueToRetrieve, 987, 2, "otherInterfaceSecond");
            AddMockBehavior<IOtherDummyInterface>(x => x.Decrement(), 3, "otherInterfaceSecond");

            // Check it was registered
            Mock<IOtherDummyInterface> otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");
            Assert.NotNull(otherMock.Object);
            Assert.AreEqual(2, otherMock.Setups.Count);

            // Clear
            InitTests();
            Assert.Throws<KeyNotFoundException>(() => GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond"));

            // Check nothing wrong
            CheckAllVerify();

            // Register a second time
            AddMockBehavior<IOtherDummyInterface, int>(x => x.ValueToRetrieve, 321, 1, "otherInterfaceSecond");

            otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");
            Assert.NotNull(otherMock.Object);
            Assert.AreEqual(1, otherMock.Setups.Count);

            // Never called
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Call and check
            Assert.AreEqual(321, otherMock.Object.ValueToRetrieve);
            CheckAllVerify();
        }

        [Test]
        public void WithNameFunc_AddMockBehavior_ShouldThrowException_WhenThereIsManyRegistrationWithDifferentReturns()
        {
            AddMockBehavior<IOtherDummyInterface, int>(x => x.ValueToRetrieve, 987, 1, "otherInterfaceSecond");
            Assert.Throws<InvalidOperationException>(() => AddMockBehavior<IOtherDummyInterface, int>(x => x.ValueToRetrieve, 321, 1, "otherInterfaceSecond"));

            Mock<IOtherDummyInterface> otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");
            Assert.AreEqual(987, otherMock.Object.ValueToRetrieve);

            AddMockBehavior<IOtherDummyInterface, int>(x => x.ValueToRetrieve, 987, 1, "otherInterfaceSecond");
            Assert.AreEqual(987, otherMock.Object.ValueToRetrieve);
        }
        #endregion Funcs (mock with name)

        #region Events (mock with name)
        [Test]
        public void WithNameEventAdd_AddMockBehaviorAction_ShouldRegisterMock_AndRegister_OneCall()
        {
            // Add one time EventAdd call required
            AddMockEventBehavior<IOtherDummyInterface>(x => x.GotTriggered += It.IsAny<Action>(), true, 1, "otherInterfaceSecond");

            // Check
            Mock<IOtherDummyInterface> otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");
            Assert.AreEqual(1, otherMock.Setups.Count);
            Assert.NotNull(otherMock);

            // EventAdd never called : exception
            Assert.Throws<MockException>(() => CheckAllVerify());

            // EventAdd called once : no exception
            otherMock.Object.GotTriggered += null;

            CheckAllVerify();
        }

        [Test]
        public void WithNameEventRemove_AddMockBehaviorAction_ShouldRegisterMock_AndRegister_OneCall()
        {
            // Add one time EventRemove call required
            AddMockEventBehavior<IOtherDummyInterface>(x => x.GotTriggered -= It.IsAny<Action>(), false, 1, "otherInterfaceSecond");

            // Check
            Mock<IOtherDummyInterface> otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");
            Assert.AreEqual(1, otherMock.Setups.Count);
            Assert.NotNull(otherMock);

            // EventRemove never called : exception
            Assert.Throws<MockException>(() => CheckAllVerify());

            // EventRemove called once : no exception
            otherMock.Object.GotTriggered -= null;

            CheckAllVerify();
        }

        [Test]
        public void WithNameEventAdd_AddMockBehaviorAction_ShouldRegisterMock_AndRegister_ManyCalls()
        {
            // Add one time EventAdd call required
            AddMockEventBehavior<IOtherDummyInterface>(x => x.GotTriggered += It.IsAny<Action>(), true, 1, "otherInterfaceSecond");

            // Add 2 times EventAdd call required (3 total calls needed)
            AddMockEventBehavior<IOtherDummyInterface>(x => x.GotTriggered += It.IsAny<Action>(), true, 2, "otherInterfaceSecond");

            // Checks
            Mock<IOtherDummyInterface> otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");
            Assert.AreEqual(1, otherMock.Setups.Count);
            Assert.NotNull(otherMock);

            // Only one call : exception
            otherMock.Object.GotTriggered += null;
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Second call : exception
            otherMock.Object.GotTriggered += null;
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Third call : ok
            otherMock.Object.GotTriggered += null;
            CheckAllVerify();

            // Add one more setup and call it twice
            AddMockBehavior<IOtherDummyInterface>(x => x.Decrement(), 2, "otherInterfaceSecond");
            otherMock.Object.Decrement();
            otherMock.Object.Decrement();
            CheckAllVerify();

            Assert.AreEqual(2, otherMock.Setups.Count);
            otherMock.VerifyNoOtherCalls();
        }

        [Test]
        public void WithNameEventRemove_AddMockBehaviorAction_ShouldRegisterMock_AndRegister_ManyCalls()
        {
            // Add one time EventRemove call required
            AddMockEventBehavior<IOtherDummyInterface>(x => x.GotTriggered -= It.IsAny<Action>(), false, 1, "otherInterfaceSecond");

            // Add 2 times EventRemove call required (3 total calls needed)
            AddMockEventBehavior<IOtherDummyInterface>(x => x.GotTriggered -= It.IsAny<Action>(), false, 2, "otherInterfaceSecond");

            // Checks
            Mock<IOtherDummyInterface> otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");
            Assert.AreEqual(1, otherMock.Setups.Count);
            Assert.NotNull(otherMock);

            // Only one call : exception
            otherMock.Object.GotTriggered -= null;
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Second call : exception
            otherMock.Object.GotTriggered -= null;
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Third call : ok
            otherMock.Object.GotTriggered -= null;
            CheckAllVerify();

            // Add one more setup and call it twice
            AddMockBehavior<IOtherDummyInterface>(x => x.Decrement(), 2, "otherInterfaceSecond");
            otherMock.Object.Decrement();
            otherMock.Object.Decrement();
            CheckAllVerify();

            Assert.AreEqual(2, otherMock.Setups.Count);
            otherMock.VerifyNoOtherCalls();
        }

        [Test]
        public void WithNameEventAdd_InitTests_ShouldClearAndRegisterAgain_ThenReplaceOldData()
        {
            AddMockEventBehavior<IOtherDummyInterface>(x => x.GotTriggered -= It.IsAny<Action>(), false, 2, "otherInterfaceSecond");
            AddMockEventBehavior<IOtherDummyInterface>(x => x.GotTriggered += It.IsAny<Action>(), true, 3, "otherInterfaceSecond");

            // Check it was registered
            Mock<IOtherDummyInterface> otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");
            Assert.NotNull(otherMock.Object);
            Assert.AreEqual(2, otherMock.Setups.Count);

            // Clear
            InitTests();
            Assert.Throws<KeyNotFoundException>(() => GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond"));

            // Check nothing wrong
            CheckAllVerify();

            // Register a second time
            AddMockEventBehavior<IOtherDummyInterface>(x => x.GotTriggered += It.IsAny<Action>(), true, 1, "otherInterfaceSecond");

            otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");
            Assert.NotNull(otherMock.Object);
            Assert.AreEqual(1, otherMock.Setups.Count);

            // Never called
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Call and check
            otherMock.Object.GotTriggered += null;
            CheckAllVerify();
        }

        [Test]
        public void WithNameEventRemove_InitTests_ShouldClearAndRegisterAgain_ThenReplaceOldData()
        {
            AddMockEventBehavior<IOtherDummyInterface>(x => x.GotTriggered -= It.IsAny<Action>(), false, 2, "otherInterfaceSecond");
            AddMockEventBehavior<IOtherDummyInterface>(x => x.GotTriggered += It.IsAny<Action>(), true, 3, "otherInterfaceSecond");

            // Check it was registered
            Mock<IOtherDummyInterface> otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");
            Assert.NotNull(otherMock.Object);
            Assert.AreEqual(2, otherMock.Setups.Count);

            // Clear
            InitTests();
            Assert.Throws<KeyNotFoundException>(() => GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond"));

            // Check nothing wrong
            CheckAllVerify();

            // Register a second time
            AddMockEventBehavior<IOtherDummyInterface>(x => x.GotTriggered -= It.IsAny<Action>(), false, 1, "otherInterfaceSecond");

            otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");
            Assert.NotNull(otherMock.Object);
            Assert.AreEqual(1, otherMock.Setups.Count);

            // Never called
            Assert.Throws<MockException>(() => CheckAllVerify());

            // Call and check
            otherMock.Object.GotTriggered -= null;
            CheckAllVerify();
        }

        [Test]
        public void WithNameAddEvent_ShouldBeDifferentFromRemoveEvent()
        {
            AddMockEventBehavior<IOtherDummyInterface>(x => x.GotTriggered += It.IsAny<Action>(), true, 1, "otherInterfaceSecond");

            Mock<IOtherDummyInterface> otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");
            Assert.Throws<MockException>(() => otherMock.Object.GotTriggered -= null);

            InitTests();
        }

        [Test]
        public void WithNameRemoveEvent_ShouldBeDifferentFromAddEvent()
        {
            AddMockEventBehavior<IOtherDummyInterface>(x => x.GotTriggered -= It.IsAny<Action>(), false, 1, "otherInterfaceSecond");

            Mock<IOtherDummyInterface> otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");
            Assert.Throws<MockException>(() => otherMock.Object.GotTriggered += null);

            InitTests();
        }

        [Test]
        public void WithNameAddAndRemoveEvent_ShouldBeDifferents_WhenBothRegistered()
        {
            AddMockEventBehavior<IOtherDummyInterface>(x => x.GotTriggered += It.IsAny<Action>(), true, 3, "otherInterfaceSecond");
            AddMockEventBehavior<IOtherDummyInterface>(x => x.GotTriggered -= It.IsAny<Action>(), false, 2, "otherInterfaceSecond");

            Mock<IOtherDummyInterface> otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");

            Assert.Throws<MockException>(() => CheckAllVerify());
            otherMock.Object.GotTriggered += null;
            otherMock.Object.GotTriggered += null;
            otherMock.Object.GotTriggered += null;

            Assert.Throws<MockException>(() => CheckAllVerify());
            otherMock.Object.GotTriggered -= null;
            otherMock.Object.GotTriggered -= null;

            CheckAllVerify();
        }

        [Test]
        public void WithNameAddAndRemoveEvent_ShouldBeDifferents_WhenBothRegistered_ForDistinctCases()
        {
            AddMockEventBehavior<IOtherDummyInterface>(x => x.GotTriggered += It.IsAny<Action>(), true, 3, "otherInterfaceSecond");
            AddMockEventBehavior<IOtherDummyInterface>(x => x.GotTriggered -= It.IsAny<Action>(), false, 2, "otherInterfaceSecond");

            AddMockEventBehavior<IOtherDummyInterface>(x => x.GotTriggeredWithInt += null, true, 4, "otherInterfaceSecond");
            AddMockEventBehavior<IOtherDummyInterface>(x => x.GotTriggeredWithInt -= null, false, 5, "otherInterfaceSecond");

            static void action(int arg) => Console.WriteLine("");
            AddMockEventBehavior<IOtherDummyInterface>(x => x.GotTriggeredWithInt += action, true, 6, "otherInterfaceSecond");
            AddMockEventBehavior<IOtherDummyInterface>(x => x.GotTriggeredWithInt -= action, false, 7, "otherInterfaceSecond");

            Mock<IOtherDummyInterface> otherMock = GetMockBehavior<IOtherDummyInterface>("otherInterfaceSecond");

            Assert.Throws<MockException>(() => CheckAllVerify());
            otherMock.Object.GotTriggered += null;
            otherMock.Object.GotTriggered += null;
            otherMock.Object.GotTriggered += null;

            Assert.Throws<MockException>(() => CheckAllVerify());
            otherMock.Object.GotTriggered -= null;
            otherMock.Object.GotTriggered -= null;

            Assert.Throws<MockException>(() => CheckAllVerify());
            for (int i = 0; i++ < 4;)
                otherMock.Object.GotTriggeredWithInt += null;

            Assert.Throws<MockException>(() => CheckAllVerify());
            for (int i = 0; i++ < 5;)
                otherMock.Object.GotTriggeredWithInt -= null;

            Assert.Throws<MockException>(() => CheckAllVerify());
            for (int i = 0; i++ < 6;)
                otherMock.Object.GotTriggeredWithInt += action;

            Assert.Throws<MockException>(() => CheckAllVerify());
            for (int i = 0; i++ < 7;)
                otherMock.Object.GotTriggeredWithInt -= action;

            CheckAllVerify();
        }
        #endregion Events (mock with name)
    }
}
