using Moq;
using Nixi.Containers;
using NixiTestTools;
using NUnit.Framework;
using ScriptExample.ComponentsWithEnumerable;
using ScriptExample.Containers;
using ScriptExample.OnlyFromContainer.TestInjector;
using System;

namespace Tests.TestTools
{
    // TODO : Update here
    internal sealed class TestInjectorOnlyFromContainerTests : InjectableOnlyFromContainerTestTemplate<AllInjectorCases>
    {
        protected override Func<AllInjectorCases> MainTestedConstructionMethod => () => new AllInjectorCases(false);

        [SetUp]
        public void InitTests()
        {
            NixiContainer.RemoveMap<ITestInterface>();
            NixiContainer.MapSingleton<ITestInterface, TestImplementation>();

            ResetTemplate();
        }

        #region InjectField
        [Test]
        public void InjectField_ShouldInject_UniqueFieldWithType()
        {
            Assert.Null(MainTested.TestInterface);

            Mock<ITestInterface> interfaceMock = new Mock<ITestInterface>(MockBehavior.Strict);
            interfaceMock.Setup(x => x.ValueToRetrieve).Returns(34);
            MainInjector.InjectField(interfaceMock.Object);

            // Checks
            Assert.NotNull(MainTested.TestInterface);
            Assert.AreEqual(34, MainTested.TestInterface.ValueToRetrieve);
            interfaceMock.Verify(x => x.ValueToRetrieve, Times.Once);
            interfaceMock.VerifyNoOtherCalls();
        }

        [Test]
        public void InjectField_ShouldThrowException_WhenNoneFoundWithType()
        {
            Assert.Throws<TestInjectorOnlyFromContainerException>(() => MainInjector.InjectField(45));
        }

        [Test]
        public void InjectField_ShouldThrowException_WhenManyFieldWithTypeFound()
        {
            Assert.Null(MainTested.FirstFruit);
            Assert.Null(MainTested.SecondFruit);

            Mock<IFruit> interfaceMock = new Mock<IFruit>(MockBehavior.Strict);
            Assert.Throws<TestInjectorOnlyFromContainerException>(() => MainInjector.InjectField(interfaceMock.Object));

            Assert.Null(MainTested.FirstFruit);
            Assert.Null(MainTested.SecondFruit);
        }

        [Test]
        public void InjectFieldWithName_ShouldInject_UniqueFieldWithTypeAndName()
        {
            Assert.Null(MainTested.FirstFruit);
            Assert.Null(MainTested.SecondFruit);

            Mock<IFruit> interfaceMock = new Mock<IFruit>(MockBehavior.Strict);
            interfaceMock.Setup(x => x.Weight).Returns(99);
            MainInjector.InjectField("SecondFruit" ,interfaceMock.Object);

            // Checks
            Assert.Null(MainTested.FirstFruit);
            Assert.NotNull(MainTested.SecondFruit);
            Assert.AreEqual(99, MainTested.SecondFruit.Weight);
            interfaceMock.Verify(x => x.Weight, Times.Once);
            interfaceMock.VerifyNoOtherCalls();
        }

        [Test]
        public void InjectFieldWithName_ShouldThrowException_WhenTypeNotFound()
        {
            Assert.Throws<TestInjectorOnlyFromContainerException>(() => MainInjector.InjectField("cannotFind", 45));
        }

        [Test]
        public void InjectFieldWithName_ShouldThrowException_WhenFoundWithType_ButNotWithName()
        {
            Mock<IFruit> interfaceMock = new Mock<IFruit>(MockBehavior.Strict);
            Assert.Throws<TestInjectorOnlyFromContainerException>(() => MainInjector.InjectField("cannotFind", interfaceMock.Object));
        }
        #endregion InjectField 

        #region ReadField
        [Test]
        public void ReadField_ShouldReturn_UniqueFieldWithType()
        {
            MainTested.TestInterface = new TestImplementation { ValueToRetrieve = 123 };

            ITestInterface instanceReturned = MainInjector.ReadField<ITestInterface>();

            Assert.AreEqual(123, instanceReturned.ValueToRetrieve);
        }

        [Test]
        public void ReadField_ShouldThrowException_WhenNoneFoundWithType()
        {
            Assert.Throws<TestInjectorOnlyFromContainerException>(() => MainInjector.ReadField<int>());
        }

        [Test]
        public void ReadField_ShouldThrowException_WhenManyFieldWithTypeFound()
        {
            Assert.Throws<TestInjectorOnlyFromContainerException>(() => MainInjector.ReadField<IFruit>());
        }

        [Test]
        public void ReadFieldWithName_ShouldReturn_UniqueFieldWithTypeAndName()
        {
            Mock<IFruit> secondFruitMock = new Mock<IFruit>(MockBehavior.Strict);
            secondFruitMock.Setup(x => x.Weight).Returns(82);
            MainTested.SecondFruit = secondFruitMock.Object;

            IFruit secondFruit = MainInjector.ReadField<IFruit>("SecondFruit");

            Assert.AreEqual(82, secondFruit.Weight);
            secondFruitMock.Verify(x => x.Weight, Times.Once);
            secondFruitMock.VerifyNoOtherCalls();
        }

        [Test]
        public void ReadFieldWithName_ShouldThrowException_WhenTypeNotFound()
        {
            Assert.Throws<TestInjectorOnlyFromContainerException>(() => MainInjector.ReadField<int>("cannotFind"));
        }

        [Test]
        public void ReadFieldWithName_ShouldThrowException_WhenFoundWithType_ButNotWithName()
        {
            Mock<IFruit> secondFruitMock = new Mock<IFruit>(MockBehavior.Strict);
            MainTested.SecondFruit = secondFruitMock.Object;

            Assert.Throws<TestInjectorOnlyFromContainerException>(() => MainInjector.ReadField<IFruit>("cannotFind"));
        }
        #endregion ReadField        
    }
}