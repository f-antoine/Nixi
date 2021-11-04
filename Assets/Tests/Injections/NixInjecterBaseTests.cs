using ScriptExample.ErrorMultiAttributes;
using Nixi.Containers;
using Nixi.Injections;
using Nixi.Injections.Injecters;
using NUnit.Framework;
using ScriptExample.Containers;
using System;
using Tests.Builders;
using UnityEngine;

namespace Tests.Injections
{
    internal sealed class NixInjecterBaseTests
    {
        [SetUp]
        public void InitTests()
        {
            NixiContainer.Remove<ITestInterface>();
            NixiContainer.MapSingle<ITestInterface, TestImplementation>();
        }

        [Test]
        public void InjectTwice_ShouldThrowException()
        {
            NixInjecter nixInjecter = SorcererBuilder.Create().WithSkill().WithChildSkill().BuildNixInjecter();

            nixInjecter.CheckAndInjectAll();

            Assert.Throws<NixInjecterException>(() => nixInjecter.CheckAndInjectAll());
        }

        [TestCase(typeof(FieldContainerWithCompo))]
        [TestCase(typeof(FieldWithCompoFromMethod))]
        [TestCase(typeof(FieldWithCompoFromMethodRoot))]
        [TestCase(typeof(CompoFromMethodWithCompoFromMethodRoot))]
        [TestCase(typeof(CompoWithCompoFromMethodRoot))]
        [TestCase(typeof(AllCompoAttributes))]
        [TestCase(typeof(FieldContainerWithFieldTestMock))]
        [TestCase(typeof(FieldTestMockWithCompo))]
        [TestCase(typeof(CompoWithCompoList))]
        public void InjectFieldWithMultiple_ShouldThrowException(Type type)
        {
            GameObject gameObject = new GameObject("any", type);
            MonoBehaviourInjectable monoBehaviourInjectable = gameObject.GetComponent(type) as MonoBehaviourInjectable;

            NixInjecter injecter = new NixInjecter(monoBehaviourInjectable);

            Assert.Throws<NixInjecterException>(() => injecter.CheckAndInjectAll());
        }
    }
}