using Nixi.Containers;
using NUnit.Framework;
using ScriptExample.Characters;
using ScriptExample.Containers;
using ScriptExample.ContainersWithParameters;
using Tests.Builders;

namespace Tests.Containers
{
    internal sealed class ContainerTests
    {
        [Test]
        public void ContainerShould_ThrowExceptionWhenNotMapped()
        {
            NixiContainer.RemoveMap<ITestInterface>();
            Assert.Throws<NixiContainerException>(() => NixiContainer.Resolve<ITestInterface>());
        }

        [Test]
        public void ContainerShould_ThrowExceptionWhenLeftElementIsNotInterface()
        {
            NixiContainer.RemoveMap<ITestInterface>();
            Assert.Throws<NixiContainerException>(() => NixiContainer.MapSingle<TestImplementation, TestImplementation>());
            Assert.Throws<NixiContainerException>(() => NixiContainer.MapTransient<TestImplementation, TestImplementation>());
        }

        [Test]
        public void ContainerShould_ThrowExceptionWhenAddingTwice()
        {
            // Double transient
            NixiContainer.RemoveMap<ITestInterface>();
            NixiContainer.MapTransient<ITestInterface, TestImplementation>();
            Assert.Throws<NixiContainerException>(() => NixiContainer.MapTransient<ITestInterface, TestImplementation>());

            // Double singleton
            NixiContainer.RemoveMap<ITestInterface>();
            NixiContainer.MapSingle<ITestInterface, TestImplementation>();
            Assert.Throws<NixiContainerException>(() => NixiContainer.MapSingle<ITestInterface, TestImplementation>());

            // Singleton then transient
            NixiContainer.RemoveMap<ITestInterface>();
            NixiContainer.MapSingle<ITestInterface, TestImplementation>();
            Assert.Throws<NixiContainerException>(() => NixiContainer.MapTransient<ITestInterface, TestImplementation>());

            // Transient then singleton
            NixiContainer.RemoveMap<ITestInterface>();
            NixiContainer.MapTransient<ITestInterface, TestImplementation>();
            Assert.Throws<NixiContainerException>(() => NixiContainer.MapSingle<ITestInterface, TestImplementation>());
        }

        [Test]
        public void ContainerShould_HandleTransientWithGoodType()
        {
            // Clearing
            NixiContainer.RemoveMap<ITestInterface>();

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
            NixiContainer.RemoveMap<ITestInterface>();

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
            NixiContainer.RemoveMap<ITestInterface>();

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
            NixiContainer.RemoveMap<ITestInterface>();

            TestImplementation implementation = new TestImplementation();
            NixiContainer.MapSingle<ITestInterface, TestImplementation>(implementation);

            Assert.Throws<NixiContainerException>(() => NixiContainer.MapSingle<ITestInterface, TestImplementation>(implementation));
            Assert.Throws<NixiContainerException>(() => NixiContainer.MapSingle<ITestInterface, TestImplementation>());
        }

        [Test]
        public void CheckIfRegistered_ShouldCheckCorrectlyForSingleton()
        {
            // Clearing
            NixiContainer.RemoveMap<ITestInterface>();

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
            NixiContainer.RemoveMap<ITestInterface>();

            Assert.False(NixiContainer.CheckIfRegistered<ITestInterface>());
            Assert.False(NixiContainer.CheckIfRegistered(typeof(ITestInterface)));

            TestImplementation implementation = new TestImplementation();
            NixiContainer.MapSingle<ITestInterface, TestImplementation>(implementation);

            ITestInterface resolved = NixiContainer.Resolve<ITestInterface>();

            Assert.NotNull(resolved);
            Assert.True(NixiContainer.CheckIfRegistered<ITestInterface>());
            Assert.True(NixiContainer.CheckIfRegistered(typeof(ITestInterface)));
        }

        [Test]
        public void CheckIfRegistered_ShouldCheckCorrectlyForTransients()
        {
            // Clearing
            NixiContainer.RemoveMap<ITestInterface>();

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

        #region Container with parameters
        [Test]
        public void SingletonShouldReturnSameInstance_AndSameParameter()
        {
            // Cleaning
            NixiContainer.RemoveMap<IContainerWithOneParameter>();

            // Arrange
            Sorcerer sorcererParameter = InjectableBuilder<Sorcerer>.Create().Build();
            NixiContainer.MapSingle<IContainerWithOneParameter, ContainerWithOneParameter>(sorcererParameter);

            // Act
            IContainerWithOneParameter resolved = NixiContainer.Resolve<IContainerWithOneParameter>();
            IContainerWithOneParameter resolvedSecond = NixiContainer.Resolve<IContainerWithOneParameter>();

            // Check
            Assert.AreEqual(resolved.SorcererFromParameter.GetInstanceID(), sorcererParameter.GetInstanceID());
            Assert.AreEqual(resolvedSecond.SorcererFromParameter.GetInstanceID(), sorcererParameter.GetInstanceID());
            Assert.AreEqual(resolved, resolvedSecond);
        }

        [Test]
        public void TransientShouldReturnDifferentInstance_AndSameParameter()
        {
            // Cleaning
            NixiContainer.RemoveMap<IContainerWithOneParameter>();

            // Arrange
            Sorcerer sorcererParameter = InjectableBuilder<Sorcerer>.Create().Build();
            NixiContainer.MapTransient<IContainerWithOneParameter, ContainerWithOneParameter>(sorcererParameter);

            // Act
            IContainerWithOneParameter resolved = NixiContainer.Resolve<IContainerWithOneParameter>();
            IContainerWithOneParameter resolvedSecond = NixiContainer.Resolve<IContainerWithOneParameter>();

            // Check
            Assert.AreEqual(resolved.SorcererFromParameter.GetInstanceID(), sorcererParameter.GetInstanceID());
            Assert.AreEqual(resolvedSecond.SorcererFromParameter.GetInstanceID(), sorcererParameter.GetInstanceID());
            Assert.AreNotEqual(resolved, resolvedSecond);
        }

        [Test]
        public void SingletonShouldReturnSameInstance_AndSameTwoParameters()
        {
            // Cleaning
            NixiContainer.RemoveMap<IContainerWithTwoParameters>();

            // Arrange
            Sorcerer sorcererParameter = InjectableBuilder<Sorcerer>.Create().Build();
            NixiContainer.MapSingle<IContainerWithTwoParameters, ContainerWithTwoParameters>(sorcererParameter, 4);

            // Act
            IContainerWithTwoParameters resolved = NixiContainer.Resolve<IContainerWithTwoParameters>();
            IContainerWithTwoParameters resolvedSecond = NixiContainer.Resolve<IContainerWithTwoParameters>();

            // Check
            Assert.AreEqual(resolved.SorcererFromParameter.GetInstanceID(), sorcererParameter.GetInstanceID());
            Assert.AreEqual(4, resolved.IntFromSecondParameter);
            Assert.AreEqual(resolvedSecond.SorcererFromParameter.GetInstanceID(), sorcererParameter.GetInstanceID());
            Assert.AreEqual(4, resolvedSecond.IntFromSecondParameter);
            Assert.AreEqual(resolved, resolvedSecond);
        }

        [Test]
        public void TransientShouldReturnDifferentInstance_AndSameTwoParameters()
        {
            // Cleaning
            NixiContainer.RemoveMap<IContainerWithTwoParameters>();

            // Arrange
            Sorcerer sorcererParameter = InjectableBuilder<Sorcerer>.Create().Build();
            NixiContainer.MapTransient<IContainerWithTwoParameters, ContainerWithTwoParameters>(sorcererParameter, 3);

            // Act
            IContainerWithTwoParameters resolved = NixiContainer.Resolve<IContainerWithTwoParameters>();
            IContainerWithTwoParameters resolvedSecond = NixiContainer.Resolve<IContainerWithTwoParameters>();

            // Check
            Assert.AreEqual(resolved.SorcererFromParameter.GetInstanceID(), sorcererParameter.GetInstanceID());
            Assert.AreEqual(3, resolved.IntFromSecondParameter);
            Assert.AreEqual(resolvedSecond.SorcererFromParameter.GetInstanceID(), sorcererParameter.GetInstanceID());
            Assert.AreEqual(3, resolvedSecond.IntFromSecondParameter);
            Assert.AreNotEqual(resolved, resolvedSecond);
        }
        #endregion Container with parameters
    }
}