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
    internal sealed class TestInjecterFieldInjectionTests
    {
        #region FieldInjection : InjectField without field name
        [Test]
        public void InjectFieldWithoutName_ShouldThrowException_WhenInterfaceNotReferencedInClass()
        {
            TestInjecter testInjecter = SorcererBuilder.Create().BuildTestInjecter();

            testInjecter.CheckAndInjectAll();

            Assert.Throws<TestInjecterException>(() => testInjecter.InjectField(new Mock<IList>().Object));
        }

        [Test]
        public void InjectFieldWithoutName_ShouldThrowException_WhenTwoSameInterfaceAndNoNameToDefine()
        {
            TestInjecter testInjecter = InjectableBuilder<BrokenSorcerer>.Create().BuildTestInjecter();

            testInjecter.CheckAndInjectAll();

            Assert.Throws<TestInjecterException>(() => testInjecter.InjectField(new Mock<IBrokenTestInterface>().Object));
        }

        [Test]
        public void InjectFieldWithoutName_ShouldFill()
        {
            // Init
            Sorcerer sorcerer = SorcererBuilder.Create().Build();
            Assert.That(sorcerer.TestInterface, Is.Null);

            // Inject
            TestInjecter testInjecter = new TestInjecter(sorcerer);
            testInjecter.CheckAndInjectAll();

            // Mock
            Mock<ITestInterface> testMock = new Mock<ITestInterface>(MockBehavior.Strict);
            testMock.SetupGet(x => x.ValueToRetrieve).Returns(4).Verifiable();
            testInjecter.InjectField(testMock.Object);

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
            TestInjecter testInjecter = new TestInjecter(sorcerer);
            testInjecter.CheckAndInjectAll();

            // Mock
            SO_Sorcerer soInfos = ScriptableObject.CreateInstance<SO_Sorcerer>();
            soInfos.CharaName = "SorcererCharaName";
            soInfos.ManaMax = 2000;
            testInjecter.InjectField(soInfos);

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
            TestInjecter testInjecter = SorcererBuilder.Create().BuildTestInjecter();

            testInjecter.CheckAndInjectAll();

            Assert.Throws<TestInjecterException>(() => testInjecter.InjectField(new Mock<IList>().Object, "anyName"));
        }

        [Test]
        public void InjectFieldWithName_ShouldThrowException_WhenInterfaceReferencedWithBadNameInClass()
        {
            TestInjecter testInjecter = SorcererBuilder.Create().BuildTestInjecter();

            testInjecter.CheckAndInjectAll();

            Assert.Throws<TestInjecterException>(() => testInjecter.InjectField(new Mock<ITestInterface>().Object, "anyName"));
        }

        [Test]
        public void InjectFieldWithName_ShouldFill()
        {
            // Init
            Sorcerer sorcerer = SorcererBuilder.Create().Build();
            Assert.That(sorcerer.TestInterface, Is.Null);

            // Inject
            TestInjecter testInjecter = new TestInjecter(sorcerer);
            testInjecter.CheckAndInjectAll();

            // Mock
            Mock<ITestInterface> testMock = new Mock<ITestInterface>(MockBehavior.Strict);
            testMock.SetupGet(x => x.ValueToRetrieve).Returns(4).Verifiable();
            testInjecter.InjectField(testMock.Object, "testInterface");

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
            TestInjecter testInjecter = new TestInjecter(brokenSorcerer);
            testInjecter.CheckAndInjectAll();

            // Mock First
            Mock<IBrokenTestInterface> testMock = new Mock<IBrokenTestInterface>(MockBehavior.Strict);
            testMock.SetupGet(x => x.ValueToRetrieve).Returns(1).Verifiable();
            testInjecter.InjectField(testMock.Object, "brokenTestInterface");

            // Mock Second
            Mock<IBrokenTestInterface> testMockSecond = new Mock<IBrokenTestInterface>(MockBehavior.Strict);
            testMockSecond.SetupGet(x => x.ValueToRetrieve).Returns(2).Verifiable();
            testInjecter.InjectField(testMockSecond.Object, "brokenTestInterfaceSecond");

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
            TestInjecter testInjecter = new TestInjecter(sorcerer);
            testInjecter.CheckAndInjectAll();

            // Mock
            SO_InventoryBag soInfosBag = ScriptableObject.CreateInstance<SO_InventoryBag>();

            // Assert
            Assert.Throws<TestInjecterException>(() => testInjecter.InjectField(soInfosBag));
        }

        [Test]
        public void InjectScriptableObjectMockName_ShouldFill()
        {
            // Init
            Sorcerer sorcerer = SorcererBuilder.Create().Build();
            Assert.That(sorcerer.FirstInventoryBagInfos, Is.Null);
            Assert.That(sorcerer.SecondInventoryBagInfos, Is.Null);

            // Inject
            TestInjecter testInjecter = new TestInjecter(sorcerer);
            testInjecter.CheckAndInjectAll();

            // Mock
            SO_InventoryBag soInfosBag = ScriptableObject.CreateInstance<SO_InventoryBag>();
            soInfosBag.BagName = "Pocket";
            soInfosBag.NbSlot = 14;
            testInjecter.InjectField(soInfosBag, "firstInventoryBagInfos");

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

            TestInjecter testInjecter = new TestInjecter(player);
            testInjecter.CheckAndInjectAll();

            Sorcerer sorcerer = testInjecter.GetComponent<Sorcerer>();

            Assert.That(sorcerer, Is.Not.Null);

            Skill skillMock = new GameObject().AddComponent<Skill>();
            Assert.Throws<TestInjecterException>(() => testInjecter.InjectField(skillMock, "anyName", sorcerer));
        }
        #endregion FieldInjection : InjectField with field name
    }
}
