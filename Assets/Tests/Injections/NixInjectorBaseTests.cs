using Nixi.Containers;
using Nixi.Injections;
using Nixi.Injections.Injectors;
using NUnit.Framework;
using ScriptExample.Containers;
using ScriptExample.ErrorMultiAttributes;
using System;
using Tests.Builders;
using UnityEngine;

namespace Tests.Injections
{
    internal sealed class NixInjectorBaseTests
    {
        [SetUp]
        public void InitTests()
        {
            NixiContainer.RemoveMap<ITestInterface>();
            NixiContainer.MapSingleton<ITestInterface, TestImplementation>();
        }

        [Test]
        public void InjectTwice_ShouldThrowException()
        {
            NixInjector nixInjector = SorcererBuilder.Create().WithSkill().WithChildSkill().BuildNixInjector();

            nixInjector.CheckAndInjectAll();

            Assert.Throws<NixInjectorException>(() => nixInjector.CheckAndInjectAll());
        }

        [TestCase(typeof(FieldContainerWithCompo))]
        [TestCase(typeof(FieldWithCompoFromMethod))]
        [TestCase(typeof(FieldWithCompoFromMethodRoot))]
        [TestCase(typeof(CompoFromMethodWithCompoFromMethodRoot))]
        [TestCase(typeof(CompoWithCompoFromMethodRoot))]
        [TestCase(typeof(AllCompoAttributes))]
        [TestCase(typeof(FieldContainerWithSerializeField))]
        [TestCase(typeof(SerializeFieldWithCompo))]
        [TestCase(typeof(CompoWithCompoList))]
        public void InjectFieldWithMultiple_ShouldThrowException(Type type)
        {
            GameObject gameObject = new GameObject("any", type);
            MonoBehaviourInjectable injectable = gameObject.GetComponent(type) as MonoBehaviourInjectable;

            NixInjector Injector = new NixInjector(injectable);

            Assert.Throws<NixInjectorException>(() => Injector.CheckAndInjectAll());
        }
    }
}