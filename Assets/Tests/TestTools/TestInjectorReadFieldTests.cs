using Moq;
using NixiTestTools;
using NUnit.Framework;
using ScriptExample.Characters;
using ScriptExample.ComponentsWithEnumerable;
using ScriptExample.Containers;
using ScriptExample.FieldReading;
using System;
using Tests.Builders;
using UnityEngine;

namespace Tests.TestTools
{
    internal sealed class TestInjectorReadFieldTests
    {
        FieldsToRead injectable;
        TestInjector testInjector;

        [SetUp]
        public void InitTests()
        {
            injectable = InjectableBuilder<FieldsToRead>.Create().Build();
            testInjector = new TestInjector(injectable);
            testInjector.CheckAndInjectAll();
        }

        #region ReadField without field name
        [Test]
        public void ReadField_ShouldThrowException_WhenNotReferenced()
        {
            Exception exception = Assert.Throws<TestInjectorException>(() => testInjector.ReadField<List>());
            StringAssert.Contains("no field with type", exception.Message);
        }

        [Test]
        public void ReadField_ShouldThrowException_WhenNotReferencedWithNixiDecoratorOrSerializeFieldAttribute()
        {
            Exception exception = Assert.Throws<TestInjectorException>(() => testInjector.ReadField<decimal>());
            StringAssert.Contains("no field with type", exception.Message);
        }

        [Test]
        public void ReadField_ShouldThrowException_OnComponentFieldDecoratedWithNixiAttribute()
        {
            Exception exception = Assert.Throws<TestInjectorException>(() => testInjector.ReadField<Sorcerer>());
            StringAssert.Contains("no field with type", exception.Message);
        }

        [Test]
        public void ReadField_ShouldThrowException_WhenTwoSameOnSerializedField_ButNoNameToFind()
        {
            Exception exception = Assert.Throws<TestInjectorException>(() => testInjector.ReadField<int>());
            StringAssert.Contains("multiple fields with type", exception.Message);
        }

        [Test]
        public void ReadField_ShouldThrowException_WhenTwoSameOnNixiContainerField_ButNoNameToFind()
        {
            Exception interfaceException = Assert.Throws<TestInjectorException>(() => testInjector.ReadField<ITestInterface>());
            StringAssert.Contains("multiple fields with type", interfaceException.Message);
        }

        [Test]
        public void ReadField_ShouldReturnFieldValue_OnSerializedField()
        {
            string singleString = testInjector.ReadField<string>();
            Assert.AreEqual(string.Empty, singleString);

            injectable.SingleString = "ToFind";
            singleString = testInjector.ReadField<string>();
            Assert.AreEqual("ToFind", singleString);
        }

        [Test]
        public void ReadField_ShouldReturnFieldValue_OnNixiContainerField()
        {
            ISecondTestInterface singleSecondTestInterface = testInjector.ReadField<ISecondTestInterface>();
            Assert.Null(singleSecondTestInterface);

            Mock<ISecondTestInterface> interfaceMock = new Mock<ISecondTestInterface>(MockBehavior.Strict);
            interfaceMock.Setup(x => x.ValueToRetrieve).Returns(8);

            injectable.SingleSecondTestInterface = interfaceMock.Object;
            singleSecondTestInterface = testInjector.ReadField<ISecondTestInterface>();
            Assert.NotNull(singleSecondTestInterface);
            Assert.AreEqual(8, singleSecondTestInterface.ValueToRetrieve);

            interfaceMock.Verify(x => x.ValueToRetrieve, Times.Once);
            interfaceMock.VerifyNoOtherCalls();
        }

        [Test]
        public void ReadField_OnComponentFieldNotDecoratedWithNixiAttribute_ShouldReturnFieldValue()
        {
            Fruit singleFruit = testInjector.ReadField<Fruit>();
            Assert.Null(singleFruit);

            injectable.SingleFruit = new GameObject().AddComponent<Fruit>();
            singleFruit = testInjector.ReadField<Fruit>();
            Assert.NotNull(singleFruit);

            injectable.SingleFruit.name = "NewName";
            Assert.AreEqual("NewName", singleFruit.name);
            Assert.AreEqual(singleFruit.GetInstanceID(), injectable.SingleFruit.GetInstanceID());

            // Full example with inject before
            Fruit fruitToInject = new GameObject().AddComponent<Fruit>();
            fruitToInject.ChangeWeight(41);
            testInjector.InjectField(fruitToInject);

            Fruit fruitRetrieved = testInjector.ReadField<Fruit>();
            Assert.AreEqual(41, fruitRetrieved.Weight);
            Assert.AreEqual(fruitRetrieved.GetInstanceID(), injectable.SingleFruit.GetInstanceID());
            Assert.AreNotEqual(fruitRetrieved.GetInstanceID(), singleFruit.GetInstanceID());
        }

        [Test]
        public void ReadField_OnInterfaceComponentFieldDecoratedWithNixiAttribute_ShouldReturnFieldValue()
        {
            IFruit iFruit = testInjector.ReadField<IFruit>();
            Assert.Null(iFruit);

            Mock<IFruit> fruitMock = new Mock<IFruit>(MockBehavior.Strict);
            fruitMock.Setup(x => x.Weight).Returns(21);
            injectable.SingleIFruit = fruitMock.Object;

            iFruit = testInjector.ReadField<IFruit>();
            Assert.NotNull(iFruit);

            Assert.AreEqual(21, iFruit.Weight);
            fruitMock.Verify(x => x.Weight, Times.Once);
            fruitMock.VerifyNoOtherCalls();
        }
        #endregion ReadField without field name

        #region ReadField with field name
        [Test]
        public void ReadFieldWithName_ShouldThrowException_WhenNotReferenced()
        {
            Exception exception = Assert.Throws<TestInjectorException>(() => testInjector.ReadField<List>("any"));
            StringAssert.Contains("no field with type", exception.Message);
        }

        [Test]
        public void ReadFieldWithName_ShouldThrowException_WhenNotReferencedWithNixiDecoratorOrSerializeFieldAttribute()
        {
            Exception exception = Assert.Throws<TestInjectorException>(() => testInjector.ReadField<decimal>("any"));
            StringAssert.Contains("no field with type", exception.Message);
        }

        [Test]
        public void ReadFieldWithName_ShouldThrowException_OnComponentFieldDecoratedWithNixiAttribute()
        {
            Exception exception = Assert.Throws<TestInjectorException>(() => testInjector.ReadField<Sorcerer>("Sorcerer"));
            StringAssert.Contains("no field with type", exception.Message);
        }

        [Test]
        public void ReadFieldWithName_ShouldThrowException_WhenReferenced_ButBadName()
        {
            Exception exception = Assert.Throws<TestInjectorException>(() => testInjector.ReadField<int>("any"));
            StringAssert.Contains($"field with type {typeof(int).Name} was/were found", exception.Message);
        }

        [Test]
        public void ReadFieldWithName_ShouldReturnFieldValue_OnSerializedField()
        {
            int firstInt = testInjector.ReadField<int>("FirstInt");
            Assert.AreEqual(0, firstInt);
            Assert.AreEqual(0, injectable.SecondInt);

            injectable.FirstInt = 4;
            firstInt = testInjector.ReadField<int>("FirstInt");
            Assert.AreEqual(4, firstInt);
            Assert.AreEqual(0, injectable.SecondInt);
        }

        [Test]
        public void ReadFieldWithName_ShouldReturnFieldValue_OnNixiContainerField()
        {
            ITestInterface firstInterface = testInjector.ReadField<ITestInterface>("FirstInterface");
            Assert.Null(firstInterface);
            Assert.Null(injectable.SecondInterface);

            Mock<ITestInterface> interfaceMock = new Mock<ITestInterface>(MockBehavior.Strict);
            interfaceMock.Setup(x => x.ValueToRetrieve).Returns(77);
            injectable.FirstInterface = interfaceMock.Object;

            firstInterface = testInjector.ReadField<ITestInterface>("FirstInterface");

            Assert.AreEqual(77, firstInterface.ValueToRetrieve);
            Assert.Null(injectable.SecondInterface);

            interfaceMock.Verify(x => x.ValueToRetrieve, Times.Once);
            interfaceMock.VerifyNoOtherCalls();
        }

        [Test]
        public void ReadFieldWithName_OnComponentFieldNotDecoratedWithNixiAttribute_ShouldReturnFieldValue()
        {
            Leprechaun firstLeprechaun = testInjector.ReadField<Leprechaun>("FirstLeprechaun");
            Assert.Null(firstLeprechaun);
            Assert.Null(injectable.SecondLeprechaun);

            injectable.FirstLeprechaun = new GameObject().AddComponent<Leprechaun>();
            firstLeprechaun = testInjector.ReadField<Leprechaun>("FirstLeprechaun");
            Assert.NotNull(firstLeprechaun);

            injectable.FirstLeprechaun.ChangeAngerLevel(21);
            Assert.AreEqual(21, firstLeprechaun.AngerLevel);
            Assert.AreEqual(firstLeprechaun.GetInstanceID(), injectable.FirstLeprechaun.GetInstanceID());
            Assert.Null(injectable.SecondLeprechaun);

            // Full example with inject before
            Leprechaun leprechaunToInjectIntoSecond = new GameObject().AddComponent<Leprechaun>();
            leprechaunToInjectIntoSecond.ChangeAngerLevel(32);
            testInjector.InjectField(leprechaunToInjectIntoSecond, "SecondLeprechaun");

            Leprechaun secondLeprechaunRetrieved = testInjector.ReadField<Leprechaun>("SecondLeprechaun");
            Assert.AreEqual(32, secondLeprechaunRetrieved.AngerLevel);
            Assert.AreEqual(secondLeprechaunRetrieved.GetInstanceID(), injectable.SecondLeprechaun.GetInstanceID());
            Assert.AreEqual(firstLeprechaun.GetInstanceID(), injectable.FirstLeprechaun.GetInstanceID());
        }

        [Test]
        public void ReadFieldWithName_OnInterfaceComponentFieldDecoratedWithNixiAttribute_ShouldReturnFieldValue()
        {
            ILeprechaun iLeprechaun = testInjector.ReadField<ILeprechaun>("FirstILeprechaun");
            Assert.Null(iLeprechaun);
            Assert.Null(injectable.SecondILeprechaun);

            Mock<ILeprechaun> leprechaunMock = new Mock<ILeprechaun>(MockBehavior.Strict);
            leprechaunMock.Setup(x => x.AngerLevel).Returns(52);
            injectable.FirstILeprechaun = leprechaunMock.Object;

            iLeprechaun = testInjector.ReadField<ILeprechaun>("FirstILeprechaun");
            Assert.NotNull(iLeprechaun);
            Assert.Null(injectable.SecondILeprechaun);

            Assert.AreEqual(52, iLeprechaun.AngerLevel);
            leprechaunMock.Verify(x => x.AngerLevel, Times.Once);
        }
        #endregion ReadField with field name
    }
}
