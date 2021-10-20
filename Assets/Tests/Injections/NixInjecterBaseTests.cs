using Assets.ScriptExample.ErrorMultiAttributes;
using Nixi.Containers;
using Nixi.Injections;
using Nixi.Injections.Injecters;
using NUnit.Framework;
using ScriptExample.Containers;
using System;
using Tests.Builders;
using UnityEngine;

namespace Assets.Tests.Injections
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

        [TestCase(typeof(FieldWithMono))]
        [TestCase(typeof(FieldWithMonoFromMethod))]
        [TestCase(typeof(FieldWithMonoFromMethodRoot))]
        [TestCase(typeof(MonoFromMethodWithMonoFromMethodRoot))]
        [TestCase(typeof(MonoWithMonoFromMethod))]
        [TestCase(typeof(MonoWithMonoFromMethodRoot))]
        [TestCase(typeof(AllMonoAttributes))]
        public void InjectFieldWithMultiple_ShouldThrowException(Type type)
        {
            GameObject gameObject = new GameObject("any", type);
            MonoBehaviourInjectable monoBehaviourInjectable = gameObject.GetComponent(type) as MonoBehaviourInjectable;

            NixInjecter injecter = new NixInjecter(monoBehaviourInjectable);

            Assert.Throws<NixInjecterException>(() => injecter.CheckAndInjectAll());
        }
    }
}