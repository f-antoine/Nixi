using Moq;
using Nixi.Injections.Injectors;
using NixiTestTools;
using NUnit.Framework;
using ScriptExample.Characters;
using ScriptExample.Characters.ScriptableObjects;
using ScriptExample.Containers;
using UnityEngine;

namespace Tests.Injections
{
    internal sealed class SimpleInjectionTests : InjectableTestTemplate<Sorcerer>
    {
        [Test]
        public void GameObjectInjection_ShouldRetrieveGameObjectElements()
        {
            Skill sorcererSkill = MainInjector.GetComponent<Skill>("magicSkill");
            Skill characterSkill = MainInjector.GetComponent<Skill>("attackSkill");

            Assert.That(sorcererSkill.name, Is.EqualTo("SorcererChildGameObjectName"));
            Assert.That(characterSkill.name, Is.EqualTo(MainTested.name));
        }

        [Test]
        public void GameObjectInjection_ShouldThrowExceptionWhenReBuildInjections()
        {   
            Assert.Throws<NixInjectorException>(() => MainInjector.CheckAndInjectAll());
        }

        [Test]
        public void FieldInjection_ShouldLoadMock_WhenUniqueInterface()
        {
            // Arrange
            Mock<ITestInterface> testMock = new Mock<ITestInterface>(MockBehavior.Strict);
            testMock.SetupGet(x => x.ValueToRetrieve).Returns(4).Verifiable();

            // Act + check value returned is same as injected
            ITestInterface interfaceMockInjected = MainInjector.InjectField(testMock.Object);

            // Assert
            Assert.That(MainTested.TestInterface.ValueToRetrieve, Is.EqualTo(4));
            Assert.That(interfaceMockInjected.ValueToRetrieve, Is.EqualTo(4));

            testMock.VerifyGet(x => x.ValueToRetrieve, Times.Exactly(2));
            testMock.VerifyNoOtherCalls();
        }

        [Test]
        public void FieldInjection_ShouldLoadMock_WithName()
        {
            // Arrange
            Mock< ITestInterface> testMock = new Mock<ITestInterface>(MockBehavior.Strict);
            testMock.SetupGet(x => x.ValueToRetrieve).Returns(4).Verifiable();

            // Act + check value returned is same as injected
            ITestInterface interfaceMockInjected = MainInjector.InjectField(testMock.Object, "testInterface");

            // Assert
            Assert.That(MainTested.TestInterface.ValueToRetrieve, Is.EqualTo(4));
            Assert.That(interfaceMockInjected.ValueToRetrieve, Is.EqualTo(4));

            testMock.VerifyGet(x => x.ValueToRetrieve, Times.Exactly(2));
            testMock.VerifyNoOtherCalls();
        }

        [Test]
        public void FieldInjection_ShouldLoadScriptable_WhenUniqueTypeOfField()
        {
            Assert.That(MainTested.SOInfos, Is.Null);

            SO_Sorcerer soInfosToInject = ScriptableObject.CreateInstance<SO_Sorcerer>();
            soInfosToInject.CharaName = "TestCharaName";
            soInfosToInject.ManaMax = 20;

            MainInjector.InjectField(soInfosToInject);

            Assert.That(MainTested.SOInfos, Is.Not.Null);
            Assert.That(MainTested.SOInfos.CharaName, Is.EqualTo(soInfosToInject.CharaName));
            Assert.That(MainTested.SOInfos.ManaMax, Is.EqualTo(soInfosToInject.ManaMax));
        }

        [Test]
        public void FieldInjection_ShouldLoadScriptable_WithName()
        {
            Assert.That(MainTested.FirstInventoryBagInfos, Is.Null);
            Assert.That(MainTested.SecondInventoryBagInfos, Is.Null);

            SO_InventoryBag soInventoryBag = ScriptableObject.CreateInstance<SO_InventoryBag>();
            soInventoryBag.BagName = "Pocket";
            soInventoryBag.NbSlot = 9;

            MainInjector.InjectField(soInventoryBag, "firstInventoryBagInfos");

            Assert.That(MainTested.SecondInventoryBagInfos, Is.Null);
            Assert.That(MainTested.FirstInventoryBagInfos, Is.Not.Null);
            Assert.That(MainTested.FirstInventoryBagInfos.BagName, Is.EqualTo(soInventoryBag.BagName));
            Assert.That(MainTested.FirstInventoryBagInfos.NbSlot, Is.EqualTo(soInventoryBag.NbSlot));
        }
    }
}