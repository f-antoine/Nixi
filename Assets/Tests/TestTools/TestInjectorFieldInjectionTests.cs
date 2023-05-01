using Moq;
using NixiTestTools;
using NUnit.Framework;
using ScriptExample.Characters;
using ScriptExample.Characters.Broken;
using ScriptExample.Characters.ScriptableObjects;
using ScriptExample.Containers;
using ScriptExample.Containers.Broken;
using ScriptExample.Players;
using System.Collections;
using Tests.Builders;
using UnityEngine;

namespace Tests.TestTools
{
    internal sealed class TestInjectorFieldInjectionTests
    {
        #region FieldInjection : InjectField without field name
        [Test]
        public void InjectFieldWithoutName_ShouldThrowException_WhenInterfaceNotReferencedInClass()
        {
            TestInjector testInjector = SorcererBuilder.Create().BuildTestInjector();

            testInjector.CheckAndInjectAll();

            Assert.Throws<TestInjectorException>(() => testInjector.InjectField(new Mock<IList>().Object));
        }

        [Test]
        public void InjectFieldWithoutName_ShouldThrowException_WhenTwoSameInterfaceAndNoNameToDefine()
        {
            TestInjector testInjector = InjectableBuilder<BrokenSorcerer>.Create().BuildTestInjector();

            testInjector.CheckAndInjectAll();

            Assert.Throws<TestInjectorException>(() => testInjector.InjectField(new Mock<IBrokenTestInterface>().Object));
        }

        [Test]
        public void InjectFieldWithoutName_ShouldFill()
        {
            // Init
            Sorcerer sorcerer = SorcererBuilder.Create().Build();
            Assert.That(sorcerer.TestInterface, Is.Null);

            // Inject
            TestInjector testInjector = new TestInjector(sorcerer);
            testInjector.CheckAndInjectAll();

            // Mock
            Mock<ITestInterface> testMock = new Mock<ITestInterface>(MockBehavior.Strict);
            testMock.SetupGet(x => x.ValueToRetrieve).Returns(4).Verifiable();
            testInjector.InjectField(testMock.Object);

            // Asserts
            Assert.That(sorcerer.TestInterface, Is.Not.Null);
            Assert.That(sorcerer.TestInterface.ValueToRetrieve, Is.EqualTo(4));
            testMock.VerifyGet(x => x.ValueToRetrieve, Times.Once);
        }

        [Test]
        public void InjectScriptableObjectMockWithoutName_ShouldFill()
        {
            // Init
            Sorcerer sorcerer = SorcererBuilder.Create().Build();
            Assert.That(sorcerer.SOInfos, Is.Null);

            // Inject
            TestInjector testInjector = new TestInjector(sorcerer);
            testInjector.CheckAndInjectAll();

            // Mock
            SO_Sorcerer soInfos = ScriptableObject.CreateInstance<SO_Sorcerer>();
            soInfos.CharaName = "SorcererCharaName";
            soInfos.ManaMax = 2000;
            testInjector.InjectField(soInfos);

            // Asserts
            Assert.That(sorcerer.SOInfos, Is.Not.Null);
            Assert.That(sorcerer.SOInfos.CharaName, Is.EqualTo(soInfos.CharaName));
            Assert.That(sorcerer.SOInfos.ManaMax, Is.EqualTo(soInfos.ManaMax));
        }
        #endregion FieldInjection : InjectField without field name

        #region FieldInjection : InjectField with field name
        [Test]
        public void InjectFieldWithName_ShouldThrowException_WhenInterfaceNotReferencedInClass()
        {
            TestInjector testInjector = SorcererBuilder.Create().BuildTestInjector();

            testInjector.CheckAndInjectAll();

            Assert.Throws<TestInjectorException>(() => testInjector.InjectField(new Mock<IList>().Object, "anyName"));
        }

        [Test]
        public void InjectFieldWithName_ShouldThrowException_WhenInterfaceReferencedWithBadNameInClass()
        {
            TestInjector testInjector = SorcererBuilder.Create().BuildTestInjector();

            testInjector.CheckAndInjectAll();

            Assert.Throws<TestInjectorException>(() => testInjector.InjectField(new Mock<ITestInterface>().Object, "anyName"));
        }

        [Test]
        public void InjectFieldWithName_ShouldFill()
        {
            // Init
            Sorcerer sorcerer = SorcererBuilder.Create().Build();
            Assert.That(sorcerer.TestInterface, Is.Null);

            // Inject
            TestInjector testInjector = new TestInjector(sorcerer);
            testInjector.CheckAndInjectAll();

            // Mock
            Mock<ITestInterface> testMock = new Mock<ITestInterface>(MockBehavior.Strict);
            testMock.SetupGet(x => x.ValueToRetrieve).Returns(4).Verifiable();
            testInjector.InjectField(testMock.Object, "testInterface");

            // Asserts
            Assert.That(sorcerer.TestInterface, Is.Not.Null);
            Assert.That(sorcerer.TestInterface.ValueToRetrieve, Is.EqualTo(4));
            testMock.VerifyGet(x => x.ValueToRetrieve, Times.Once);
        }

        [Test]
        public void InjectFieldWithName_ShouldFillMoreComplicated()
        {
            // Init
            BrokenSorcerer brokenSorcerer = InjectableBuilder<BrokenSorcerer>.Create().Build();
            Assert.That(brokenSorcerer.BrokenTestInterface, Is.Null);
            Assert.That(brokenSorcerer.BrokenTestInterfaceSecond, Is.Null);

            // Inject
            TestInjector testInjector = new TestInjector(brokenSorcerer);
            testInjector.CheckAndInjectAll();

            // Mock First
            Mock<IBrokenTestInterface> testMock = new Mock<IBrokenTestInterface>(MockBehavior.Strict);
            testMock.SetupGet(x => x.ValueToRetrieve).Returns(1).Verifiable();
            testInjector.InjectField(testMock.Object, "brokenTestInterface");

            // Mock Second
            Mock<IBrokenTestInterface> testMockSecond = new Mock<IBrokenTestInterface>(MockBehavior.Strict);
            testMockSecond.SetupGet(x => x.ValueToRetrieve).Returns(2).Verifiable();
            testInjector.InjectField(testMockSecond.Object, "brokenTestInterfaceSecond");

            // Asserts First
            Assert.That(brokenSorcerer.BrokenTestInterface, Is.Not.Null);
            Assert.That(brokenSorcerer.BrokenTestInterface.ValueToRetrieve, Is.EqualTo(1));
            testMock.VerifyGet(x => x.ValueToRetrieve, Times.Once);

            // Asserts Second
            Assert.That(brokenSorcerer.BrokenTestInterfaceSecond, Is.Not.Null);
            Assert.That(brokenSorcerer.BrokenTestInterfaceSecond.ValueToRetrieve, Is.EqualTo(2));
            testMockSecond.VerifyGet(x => x.ValueToRetrieve, Times.Once);
        }

        [Test]
        public void InjectScriptableObjectMock_ShouldThrowWhenTwoSameScriptableObjectType()
        {
            // Init
            Sorcerer sorcerer = SorcererBuilder.Create().Build();

            // Inject
            TestInjector testInjector = new TestInjector(sorcerer);
            testInjector.CheckAndInjectAll();

            // Mock
            SO_InventoryBag soInfosBag = ScriptableObject.CreateInstance<SO_InventoryBag>();

            // Assert
            Assert.Throws<TestInjectorException>(() => testInjector.InjectField(soInfosBag));
        }

        [Test]
        public void InjectScriptableObjectMockName_ShouldFill()
        {
            // Init
            Sorcerer sorcerer = SorcererBuilder.Create().Build();
            Assert.That(sorcerer.FirstInventoryBagInfos, Is.Null);
            Assert.That(sorcerer.SecondInventoryBagInfos, Is.Null);

            // Inject
            TestInjector testInjector = new TestInjector(sorcerer);
            testInjector.CheckAndInjectAll();

            // Mock
            SO_InventoryBag soInfosBag = ScriptableObject.CreateInstance<SO_InventoryBag>();
            soInfosBag.BagName = "Pocket";
            soInfosBag.NbSlot = 14;
            testInjector.InjectField(soInfosBag, "firstInventoryBagInfos");

            // Asserts
            Assert.That(sorcerer.FirstInventoryBagInfos, Is.Not.Null);
            Assert.That(sorcerer.SecondInventoryBagInfos, Is.Null);
            Assert.That(sorcerer.FirstInventoryBagInfos.BagName, Is.EqualTo(soInfosBag.BagName));
            Assert.That(sorcerer.FirstInventoryBagInfos.NbSlot, Is.EqualTo(soInfosBag.NbSlot));
        }

        [Test]
        public void InjectScriptableObjectMockName_ShouldThrowException_WhenTypeIsRepresentedButNotFieldName()
        {
            Player player = PlayerBuilder.Create().Build();

            TestInjector testInjector = new TestInjector(player);
            testInjector.CheckAndInjectAll();

            Sorcerer sorcerer = testInjector.GetComponent<Sorcerer>();

            Assert.That(sorcerer, Is.Not.Null);

            Skill skillMock = new GameObject().AddComponent<Skill>();
            Assert.Throws<TestInjectorException>(() => testInjector.InjectField(skillMock, "anyName", sorcerer));
        }
        #endregion FieldInjection : InjectField with field name
    }
}
