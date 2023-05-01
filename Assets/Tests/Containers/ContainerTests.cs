using Nixi.Containers;
using NUnit.Framework;
using ScriptExample.Characters;
using ScriptExample.ComponentsWithInterface;
using ScriptExample.Containers;
using ScriptExample.ContainersWithParameters;
using System;
using Tests.Builders;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Tests.Containers
{
    internal sealed class ContainerTests
    {
        [SetUp]
        public void InitTests()
        {
            NixiContainer.RemoveMap<ITestInterface>();
            NixiContainer.RemoveMap<IContainerWithOneParameter>();
            NixiContainer.RemoveMap<IContainerWithTwoParameters>();
        }

        [Test]
        public void ContainerShould_ThrowExceptionWhenNotMapped()
        {
            Assert.Throws<NixiContainerException>(() => NixiContainer.ResolveMap<ITestInterface>());
        }

        [Test]
        public void ContainerShould_ThrowExceptionWhenLeftElementIsNotInterface()
        {
            Assert.Throws<NixiContainerException>(() => NixiContainer.MapSingleton<TestImplementation, TestImplementation>());
            Assert.Throws<NixiContainerException>(() => NixiContainer.MapTransient<TestImplementation, TestImplementation>());
        }

        [Test]
        public void ContainerShould_ThrowExceptionWhenImplementationIsMonoBehaviour_ButWithoutImplementationPassed()
        {
            NixiContainer.RemoveMap<IFlyBehavior>();
            Assert.Throws<NixiContainerException>(() => NixiContainer.MapTransient<IFlyBehavior, DuckWings>());
            Assert.Throws<NixiContainerException>(() => NixiContainer.MapSingleton<IFlyBehavior, DuckWings>());
        }

        [Test]
        public void ContainerShould_ThrowExceptionWhenAddingTwice()
        {
            // Double transient
            NixiContainer.MapTransient<ITestInterface, TestImplementation>();
            Assert.Throws<NixiContainerException>(() => NixiContainer.MapTransient<ITestInterface, TestImplementation>());

            // Double singleton
            NixiContainer.RemoveMap<ITestInterface>();
            NixiContainer.MapSingleton<ITestInterface, TestImplementation>();
            Assert.Throws<NixiContainerException>(() => NixiContainer.MapSingleton<ITestInterface, TestImplementation>());

            // Singleton then transient
            NixiContainer.RemoveMap<ITestInterface>();
            NixiContainer.MapSingleton<ITestInterface, TestImplementation>();
            Assert.Throws<NixiContainerException>(() => NixiContainer.MapTransient<ITestInterface, TestImplementation>());

            // Transient then singleton
            NixiContainer.RemoveMap<ITestInterface>();
            NixiContainer.MapTransient<ITestInterface, TestImplementation>();
            Assert.Throws<NixiContainerException>(() => NixiContainer.MapSingleton<ITestInterface, TestImplementation>());
        }

        [Test]
        public void ContainerShould_HandleTransientWithGoodType()
        {
            // Retrieve transient first time
            NixiContainer.MapTransient<ITestInterface, TestImplementation>();
            ITestInterface instance = NixiContainer.ResolveMap<ITestInterface>();

            Assert.That(instance, Is.Not.Null);
            Assert.That(instance.ValueToRetrieve, Is.EqualTo(0));

            // Update transient
            instance.ValueToRetrieve = 2;

            // Retrieve another transient
            ITestInterface anotherInstance = NixiContainer.ResolveMap<ITestInterface>();
            Assert.That(instance.ValueToRetrieve, Is.EqualTo(2));
            Assert.That(anotherInstance.ValueToRetrieve, Is.EqualTo(0));
        }

        [Test]
        public void ContainerShould_HandleSingletonWithGoodType()
        {
            NixiContainer.MapSingleton<ITestInterface, TestImplementation>();

            // Retrieve singleton first time
            ITestInterface instance = NixiContainer.ResolveMap<ITestInterface>();
            Assert.That(instance, Is.Not.Null);
            Assert.That(instance.ValueToRetrieve, Is.EqualTo(0));

            // Update singleton
            instance.ValueToRetrieve = 2;

            // Retrieve singleton second time
            ITestInterface sameInstance = NixiContainer.ResolveMap<ITestInterface>();

            Assert.That(instance.ValueToRetrieve, Is.EqualTo(2));
            Assert.That(sameInstance.ValueToRetrieve, Is.EqualTo(2));
        }

        [Test]
        public void ContainerShould_RegisterSingletonWithInstancePassed()
        {
            TestImplementation implementation = new TestImplementation();
            implementation.ValueToRetrieve = 14;

            NixiContainer.MapSingletonWithImplementation<ITestInterface, TestImplementation>(implementation);

            // Retrieve singleton first time
            ITestInterface instance = NixiContainer.ResolveMap<ITestInterface>();
            Assert.That(instance, Is.Not.Null);
            Assert.That(instance.ValueToRetrieve, Is.EqualTo(14));

            // Update singleton
            instance.ValueToRetrieve = 8;

            // Retrieve singleton second time
            ITestInterface sameInstance = NixiContainer.ResolveMap<ITestInterface>();

            Assert.That(instance.ValueToRetrieve, Is.EqualTo(8));
            Assert.That(sameInstance.ValueToRetrieve, Is.EqualTo(8));
            Assert.That(implementation.ValueToRetrieve, Is.EqualTo(8));
        }

        [Test]
        public void ContainerShould_OverrideWhenWithRegisterSingletonWithADifferentInstancePassed()
        {
            NixiContainer.MapSingletonWithImplementation<ITestInterface, TestImplementation>(new TestImplementation
            {
                ValueToRetrieve = 14
            });

            // Retrieve singleton first time
            ITestInterface instance = NixiContainer.ResolveMap<ITestInterface>();
            Assert.That(instance, Is.Not.Null);
            Assert.That(instance.ValueToRetrieve, Is.EqualTo(14));

            // Override
            NixiContainer.MapSingletonWithImplementation<ITestInterface, TestImplementation>(new TestImplementation
            {
                ValueToRetrieve = 9
            }, true);

            // Retrieve singleton overriden second time
            instance = NixiContainer.ResolveMap<ITestInterface>();
            Assert.That(instance, Is.Not.Null);
            Assert.That(instance.ValueToRetrieve, Is.EqualTo(9));
        }

        [Test]
        public void ContainerShouldNotRegisterTwice_WhenSingletonAlreadyRegisteredWithInstancePassed()
        {
            TestImplementation implementation = new TestImplementation();
            NixiContainer.MapSingletonWithImplementation<ITestInterface, TestImplementation>(implementation);

            Assert.Throws<NixiContainerException>(() => NixiContainer.MapSingletonWithImplementation<ITestInterface, TestImplementation>(implementation));
            Assert.Throws<NixiContainerException>(() => NixiContainer.MapSingleton<ITestInterface, TestImplementation>());
        }

        [Test]
        public void CheckIfRegistered_ShouldCheckCorrectlyForSingleton()
        {
            Assert.False(NixiContainer.CheckIfMappingRegistered<ITestInterface>());

            NixiContainer.MapSingleton<ITestInterface, TestImplementation>();

            Assert.True(NixiContainer.CheckIfMappingRegistered<ITestInterface>());
        }

        [Test]
        public void CheckIfRegistered_ShouldCheckCorrectlyForSingletonWithInstance()
        {
            Assert.False(NixiContainer.CheckIfMappingRegistered<ITestInterface>());

            TestImplementation implementation = new TestImplementation();
            NixiContainer.MapSingletonWithImplementation<ITestInterface, TestImplementation>(implementation);

            ITestInterface resolved = NixiContainer.ResolveMap<ITestInterface>();

            Assert.NotNull(resolved);
            Assert.True(NixiContainer.CheckIfMappingRegistered<ITestInterface>());
        }

        [Test]
        public void CheckIfRegistered_ShouldCheckCorrectlyForTransients()
        {
            Assert.False(NixiContainer.CheckIfMappingRegistered<ITestInterface>());

            NixiContainer.MapTransient<ITestInterface, TestImplementation>();

            Assert.True(NixiContainer.CheckIfMappingRegistered<ITestInterface>());
        }

        [Test]
        public void CheckIfRegistered_ShouldThrowExceptionWhenNotInterface()
        {
            Assert.Throws<NixiContainerException>(() => NixiContainer.CheckIfMappingRegistered<TestImplementation>());
        }

        #region Container with parameters
        [Test]
        public void SingletonShouldReturnSameInstance_AndSameParameter()
        {
            // Arrange
            Sorcerer sorcererParameter = InjectableBuilder<Sorcerer>.Create().Build();
            NixiContainer.MapSingleton<IContainerWithOneParameter, ContainerWithOneParameter>(sorcererParameter);

            // Act
            IContainerWithOneParameter resolved = NixiContainer.ResolveMap<IContainerWithOneParameter>();
            IContainerWithOneParameter resolvedSecond = NixiContainer.ResolveMap<IContainerWithOneParameter>();

            // Check
            Assert.AreEqual(resolved.SorcererFromParameter.GetInstanceID(), sorcererParameter.GetInstanceID());
            Assert.AreEqual(resolvedSecond.SorcererFromParameter.GetInstanceID(), sorcererParameter.GetInstanceID());
            Assert.AreEqual(resolved, resolvedSecond);
        }

        [Test]
        public void TransientShouldReturnDifferentInstance_AndSameParameter()
        {
            // Arrange
            Sorcerer sorcererParameter = InjectableBuilder<Sorcerer>.Create().Build();
            NixiContainer.MapTransient<IContainerWithOneParameter, ContainerWithOneParameter>(sorcererParameter);

            // Act
            IContainerWithOneParameter resolved = NixiContainer.ResolveMap<IContainerWithOneParameter>();
            IContainerWithOneParameter resolvedSecond = NixiContainer.ResolveMap<IContainerWithOneParameter>();

            // Check
            Assert.AreEqual(resolved.SorcererFromParameter.GetInstanceID(), sorcererParameter.GetInstanceID());
            Assert.AreEqual(resolvedSecond.SorcererFromParameter.GetInstanceID(), sorcererParameter.GetInstanceID());
            Assert.AreNotEqual(resolved, resolvedSecond);
        }

        [Test]
        public void SingletonShouldReturnSameInstance_AndSameTwoParameters()
        {
            // Arrange
            Sorcerer sorcererParameter = InjectableBuilder<Sorcerer>.Create().Build();
            NixiContainer.MapSingleton<IContainerWithTwoParameters, ContainerWithTwoParameters>(sorcererParameter, 4);

            // Act
            IContainerWithTwoParameters resolved = NixiContainer.ResolveMap<IContainerWithTwoParameters>();
            IContainerWithTwoParameters resolvedSecond = NixiContainer.ResolveMap<IContainerWithTwoParameters>();

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
            // Arrange
            Sorcerer sorcererParameter = InjectableBuilder<Sorcerer>.Create().Build();
            NixiContainer.MapTransient<IContainerWithTwoParameters, ContainerWithTwoParameters>(sorcererParameter, 3);

            // Act
            IContainerWithTwoParameters resolved = NixiContainer.ResolveMap<IContainerWithTwoParameters>();
            IContainerWithTwoParameters resolvedSecond = NixiContainer.ResolveMap<IContainerWithTwoParameters>();

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