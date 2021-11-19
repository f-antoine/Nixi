using Nixi.Containers;
using NUnit.Framework;
using ScriptExample.Containers;

namespace Tests.Containers
{
    internal sealed class ContainerTests
    {
        [Test]
        public void ContainerShould_ThrowExceptionWhenNotMapped()
        {
            NixiContainer.Remove<ITestInterface>();
            Assert.Throws<NixiContainerException>(() => NixiContainer.Resolve<ITestInterface>());
        }

        [Test]
        public void ContainerShould_ThrowExceptionWhenLeftElementIsNotInterface()
        {
            NixiContainer.Remove<ITestInterface>();
            Assert.Throws<NixiContainerException>(() => NixiContainer.MapSingle<TestImplementation, TestImplementation>());
            Assert.Throws<NixiContainerException>(() => NixiContainer.MapTransient<TestImplementation, TestImplementation>());
        }

        [Test]
        public void ContainerShould_ThrowExceptionWhenAddingTwice()
        {
            // Double transient
            NixiContainer.Remove<ITestInterface>();
            NixiContainer.MapTransient<ITestInterface, TestImplementation>();
            Assert.Throws<NixiContainerException>(() => NixiContainer.MapTransient<ITestInterface, TestImplementation>());

            // Double singleton
            NixiContainer.Remove<ITestInterface>();
            NixiContainer.MapSingle<ITestInterface, TestImplementation>();
            Assert.Throws<NixiContainerException>(() => NixiContainer.MapSingle<ITestInterface, TestImplementation>());

            // Singleton then transient
            NixiContainer.Remove<ITestInterface>();
            NixiContainer.MapSingle<ITestInterface, TestImplementation>();
            Assert.Throws<NixiContainerException>(() => NixiContainer.MapTransient<ITestInterface, TestImplementation>());

            // Transient then singleton
            NixiContainer.Remove<ITestInterface>();
            NixiContainer.MapTransient<ITestInterface, TestImplementation>();
            Assert.Throws<NixiContainerException>(() => NixiContainer.MapSingle<ITestInterface, TestImplementation>());
        }

        [Test]
        public void ContainerShould_HandleTransientWithGoodType()
        {   
            // Clearing
            NixiContainer.Remove<ITestInterface>();

            // Retrieve transient first time
            NixiContainer.MapTransient<ITestInterface, TestImplementation>();
            ITestInterface instance = NixiContainer.Resolve<ITestInterface>();

            Assert.That(instance, Is.Not.Null);
            Assert.That(instance.ValueToRetrieve, Is.EqualTo(0));

            // Update transient
            instance.ValueToRetrieve = 2;

            // Retrieve another transient
            ITestInterface anotherInstance = NixiContainer.Resolve<ITestInterface>();
            Assert.That(instance.ValueToRetrieve, Is.EqualTo(2));
            Assert.That(anotherInstance.ValueToRetrieve, Is.EqualTo(0));
        }

        [Test]
        public void ContainerShould_HandleSingletonWithGoodType()
        {
            // Clearing
            NixiContainer.Remove<ITestInterface>();

            NixiContainer.MapSingle<ITestInterface, TestImplementation>();

            // Retrieve singleton first time
            ITestInterface instance = NixiContainer.Resolve<ITestInterface>();
            Assert.That(instance, Is.Not.Null);
            Assert.That(instance.ValueToRetrieve, Is.EqualTo(0));

            // Update singleton
            instance.ValueToRetrieve = 2;

            // Retrieve singleton second time
            ITestInterface sameInstance = NixiContainer.Resolve<ITestInterface>();
            
            Assert.That(instance.ValueToRetrieve, Is.EqualTo(2));
            Assert.That(sameInstance.ValueToRetrieve, Is.EqualTo(2));
        }

        [Test]
        public void ContainerShould_RegisterSingletonWithInstancePassed()
        {
            // Clearing
            NixiContainer.Remove<ITestInterface>();

            TestImplementation implementation = new TestImplementation();
            implementation.ValueToRetrieve = 14;

            NixiContainer.MapSingle<ITestInterface, TestImplementation>(implementation);

            // Retrieve singleton first time
            ITestInterface instance = NixiContainer.Resolve<ITestInterface>();
            Assert.That(instance, Is.Not.Null);
            Assert.That(instance.ValueToRetrieve, Is.EqualTo(14));

            // Update singleton
            instance.ValueToRetrieve = 8;

            // Retrieve singleton second time
            ITestInterface sameInstance = NixiContainer.Resolve<ITestInterface>();

            Assert.That(instance.ValueToRetrieve, Is.EqualTo(8));
            Assert.That(sameInstance.ValueToRetrieve, Is.EqualTo(8));
            Assert.That(implementation.ValueToRetrieve, Is.EqualTo(8));
        }

        [Test]
        public void ContainerShouldNotRegisterTwice_WhenSingletonAlreadyRegisteredWithInstancePassed()
        {
            // Clearing
            NixiContainer.Remove<ITestInterface>();

            TestImplementation implementation = new TestImplementation();
            NixiContainer.MapSingle<ITestInterface, TestImplementation>(implementation);

            Assert.Throws<NixiContainerException>(() => NixiContainer.MapSingle<ITestInterface, TestImplementation>(implementation));
            Assert.Throws<NixiContainerException>(() => NixiContainer.MapSingle<ITestInterface, TestImplementation>());
        }

        [Test]
        public void CheckIfRegistered_ShouldCheckCorrectlyForSingleton()
        {
            // Clearing
            NixiContainer.Remove<ITestInterface>();

            Assert.False(NixiContainer.CheckIfRegistered<ITestInterface>());
            Assert.False(NixiContainer.CheckIfRegistered(typeof(ITestInterface)));

            NixiContainer.MapSingle<ITestInterface, TestImplementation>();

            Assert.True(NixiContainer.CheckIfRegistered<ITestInterface>());
            Assert.True(NixiContainer.CheckIfRegistered(typeof(ITestInterface)));
        }

        [Test]
        public void CheckIfRegistered_ShouldCheckCorrectlyForSingletonWithInstance()
        {
            // Clearing
            NixiContainer.Remove<ITestInterface>();

            Assert.False(NixiContainer.CheckIfRegistered<ITestInterface>());
            Assert.False(NixiContainer.CheckIfRegistered(typeof(ITestInterface)));

            TestImplementation implementation = new TestImplementation();
            NixiContainer.MapSingle<ITestInterface, TestImplementation>(implementation);

            Assert.True(NixiContainer.CheckIfRegistered<ITestInterface>());
            Assert.True(NixiContainer.CheckIfRegistered(typeof(ITestInterface)));
        }

        [Test]
        public void CheckIfRegistered_ShouldCheckCorrectlyForTransients()
        {
            // Clearing
            NixiContainer.Remove<ITestInterface>();

            Assert.False(NixiContainer.CheckIfRegistered<ITestInterface>());
            Assert.False(NixiContainer.CheckIfRegistered(typeof(ITestInterface)));

            NixiContainer.MapTransient<ITestInterface, TestImplementation>();

            Assert.True(NixiContainer.CheckIfRegistered<ITestInterface>());
            Assert.True(NixiContainer.CheckIfRegistered(typeof(ITestInterface)));
        }

        [Test]
        public void CheckIfRegistered_ShouldThrowExceptionWhenNotInterface()
        {
            Assert.Throws<NixiContainerException>(() => NixiContainer.CheckIfRegistered<TestImplementation>());
            Assert.Throws<NixiContainerException>(() => NixiContainer.CheckIfRegistered(typeof(TestImplementation)));
        }
    }
}