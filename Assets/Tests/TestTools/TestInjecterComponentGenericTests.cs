using Assets.ScriptExample.Genericity.Classes;
using Assets.ScriptExample.Genericity.Classes.SecondLevel;
using Assets.ScriptExample.Genericity.Interfaces;
using Assets.ScriptExample.Genericity.Interfaces.SecondLevel;
using Assets.Tests.Builders;
using Nixi.Injections;
using NixiTestTools;
using NUnit.Framework;
using System;
using UnityEngine;

namespace Tests.TestTools
{
    internal sealed class TestInjecterComponentGenericTests
    {
        [TestCase(typeof(GenericInterfaceExample))]
        [TestCase(typeof(GenericInterfaceExampleChild))]
        [TestCase(typeof(GenericInterfaceExampleParent))]
        [TestCase(typeof(GenericInterfaceExampleRoot))]
        [TestCase(typeof(GenericInterfaceExampleRootChild))]
        public void NonEnumerable_GenericInterfaceWithOnlyOneGenericArgument_InjectedAsComponent_ShouldThrowException(Type injectableTypeToBuild)
        {
            Component component = CompoBuilderWithExpliciteType.Create().Build(injectableTypeToBuild);
            MonoBehaviourInjectable injectable = component as MonoBehaviourInjectable;

            TestInjecter injecter = new TestInjecter(injectable);

            Assert.Throws<TestInjecterException>(() => injecter.CheckAndInjectAll());
        }

        [TestCase(typeof(MultipleGenericInterfaceExample))]
        [TestCase(typeof(MultipleGenericInterfaceExampleChild))]
        [TestCase(typeof(MultipleGenericInterfaceExampleParent))]
        [TestCase(typeof(MultipleGenericInterfaceExampleRoot))]
        [TestCase(typeof(MultipleGenericInterfaceExampleRootChild))]
        public void NonEnumerable_GenericInterfaceWithManyGenericArgument_InjectedAsComponent_ShouldThrowException(Type injectableTypeToBuild)
        {
            Component component = CompoBuilderWithExpliciteType.Create().Build(injectableTypeToBuild);
            MonoBehaviourInjectable injectable = component as MonoBehaviourInjectable;

            TestInjecter injecter = new TestInjecter(injectable);

            Assert.Throws<TestInjecterException>(() => injecter.CheckAndInjectAll());
        }

        [TestCase(typeof(GenericClassExample))]
        [TestCase(typeof(GenericClassExampleChild))]
        [TestCase(typeof(GenericClassExampleParent))]
        [TestCase(typeof(GenericClassExampleRoot))]
        [TestCase(typeof(GenericClassExampleRootChild))]
        public void GenericClass_WithOnlyOneGenericArgument_InjectedAsComponent_ShouldThrowException(Type injectableTypeToBuild)
        {
            Component component = CompoBuilderWithExpliciteType.Create().Build(injectableTypeToBuild);
            MonoBehaviourInjectable injectable = component as MonoBehaviourInjectable;

            TestInjecter injecter = new TestInjecter(injectable);

            Assert.Throws<TestInjecterException>(() => injecter.CheckAndInjectAll());
        }

        [TestCase(typeof(MultipleGenericClassExample))]
        [TestCase(typeof(MultipleGenericClassExampleChild))]
        [TestCase(typeof(MultipleGenericClassExampleParent))]
        [TestCase(typeof(MultipleGenericClassExampleRoot))]
        [TestCase(typeof(MultipleGenericClassExampleRootChild))]
        public void GenericClass_GenericInterfaceWithManyGenericArgument_InjectedAsComponent_ShouldThrowException(Type injectableTypeToBuild)
        {
            Component component = CompoBuilderWithExpliciteType.Create().Build(injectableTypeToBuild);
            MonoBehaviourInjectable injectable = component as MonoBehaviourInjectable;

            TestInjecter injecter = new TestInjecter(injectable);

            Assert.Throws<TestInjecterException>(() => injecter.CheckAndInjectAll());
        }
    }
}