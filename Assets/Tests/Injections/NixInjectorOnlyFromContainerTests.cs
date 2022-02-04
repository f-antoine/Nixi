using Nixi.Containers;
using Nixi.Injections.Injectors;
using NUnit.Framework;
using ScriptExample.Containers;
using ScriptExample.OnlyFromContainer;

namespace Tests.Injections
{
    internal sealed class NixInjectorOnlyFromContainerTests
    {
        [SetUp]
        public void InitTests()
        {
            NixiContainer.RemoveMap<ITestInterface>();
            NixiContainer.MapSingleton<ITestInterface, TestImplementation>();
        }

        [Test]
        public void InjectShould_FieldBothFields()
        {
            SimpleOnlyFromContainer injectable = new SimpleOnlyFromContainer();
            injectable.SetValueToRetrieve(321);

            Assert.NotNull(injectable.TestInterface);
            Assert.AreEqual(321, injectable.TestInterface.ValueToRetrieve);
            Assert.AreEqual(321, injectable.ValueToRetrieveFromPrivateInterface);
        }

        [Test]
        public void InjectShould_FieldNothing()
        {
            NoFieldToInjectOnlyFromContainer injectable = new NoFieldToInjectOnlyFromContainer();

            Assert.Null(injectable.TestInterface);
        }

        [Test]
        public void InjectShould_ThrowException_WhenManyNixiDecorator()
        {
            Assert.Throws<NixInjectorException>(() => new ManyDecoratorErrorOnlyFromContainer());
        }

        [Test]
        public void InjectShould_ThrowException_WhenWrongNixiDecorator()
        {
            Assert.Throws<NixInjectorException>(() => new WrongDecoratorOnlyFromContainer());
        }

        [Test]
        public void InjectShould_ThrowException_WhenSerializeField()
        {
            Assert.Throws<NixInjectorException>(() => new WrongSerializeFieldOnlyFromContainer());
        }
    }
}