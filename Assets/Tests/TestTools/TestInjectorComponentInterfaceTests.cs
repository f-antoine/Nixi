using Moq;
using NixiTestTools;
using NUnit.Framework;
using ScriptExample.ComponentsWithInterface;
using Tests.Builders;

namespace Tests.TestTools
{
    internal sealed class TestInjectorComponentInterfaceTests
    {
        #region Component Interface
        [Test]
        public void Duck_ShouldBeFilled_FromInterfaceInjection_WithoutFieldName()
        {
            // Arrange
            Duck duck = DuckBuilder.Create().BuildDuck();

            TestInjector testInjector = new TestInjector(duck);
            testInjector.CheckAndInjectAll();

            Assert.That(duck.Wings, Is.Null);

            // Inject Mock
            Mock<IFlyBehavior> flyBehaviorMock = new Mock<IFlyBehavior>(MockBehavior.Strict);
            flyBehaviorMock.Setup(x => x.Height).Returns(22);
            testInjector.InjectField(flyBehaviorMock.Object);

            // Verify
            Assert.That(duck.Wings, Is.Not.Null);
            Assert.That(duck.Wings.Height, Is.EqualTo(22));

            flyBehaviorMock.Verify(x => x.Height, Times.Once);
            flyBehaviorMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Duck_ShouldThrowException_FromInterfaceInjection_WithTwoSameTypeFieldsButWithoutFieldName()
        {
            // Arrange
            Duck duck = DuckBuilder.Create().BuildDuck();

            TestInjector testInjector = new TestInjector(duck);
            testInjector.CheckAndInjectAll();

            Assert.That(duck.Pocket, Is.Null);

            // Inject Mock
            Mock<IDuckObjectContainer> duckBackPackMock = new Mock<IDuckObjectContainer>(MockBehavior.Strict);
            duckBackPackMock.Setup(x => x.NbSlot).Returns(12);
            Assert.Throws<TestInjectorException>(() => testInjector.InjectField(duckBackPackMock.Object));
        }

        [Test]
        public void Duck_ShouldBeFilled_FromInterfaceInjection_WithFieldName()
        {
            // Arrange
            Duck duck = DuckBuilder.Create().BuildDuck();

            TestInjector testInjector = new TestInjector(duck);
            testInjector.CheckAndInjectAll();

            Assert.That(duck.Pocket, Is.Null);
            Assert.That(duck.DuckCompanyBackPack, Is.Null);

            // Inject pocket Mock
            Mock<IDuckObjectContainer> duckPocketMock = new Mock<IDuckObjectContainer>(MockBehavior.Strict);
            duckPocketMock.Setup(x => x.NbSlot).Returns(12);
            testInjector.InjectField(duckPocketMock.Object, "pocket");

            // Inject duckCompanyBackPack Mock
            Mock<IDuckObjectContainer> duckCompanyBackPackMock = new Mock<IDuckObjectContainer>(MockBehavior.Strict);
            duckCompanyBackPackMock.Setup(x => x.NbSlot).Returns(256);
            testInjector.InjectField(duckCompanyBackPackMock.Object, "duckCompanyBackPack");

            // Verify
            Assert.That(duck.Pocket, Is.Not.Null);
            Assert.That(duck.DuckCompanyBackPack, Is.Not.Null);
            Assert.That(duck.Pocket.NbSlot, Is.EqualTo(12));
            Assert.That(duck.DuckCompanyBackPack.NbSlot, Is.EqualTo(256));

            duckPocketMock.Verify(x => x.NbSlot, Times.Once);
            duckCompanyBackPackMock.Verify(x => x.NbSlot, Times.Once);
            duckPocketMock.VerifyNoOtherCalls();
            duckCompanyBackPackMock.VerifyNoOtherCalls();
        }
        #endregion Component Interface
    }
}
