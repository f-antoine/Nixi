using Nixi.Injections.Injectors;
using NixiTestTools;
using NUnit.Framework;
using ScriptExample.Characters.Broken;
using UnityEngine;

namespace Tests.Injections
{
    internal sealed class WrongNixiAttributesTests
    {
        [Test]
        public void TestInjector_ShouldThrowExceptionWhen_Injecting_WrongNixInjectComponentAttribute()
        {
            SorcererWithWrongNixInjectComponent wrongSorcerer = new GameObject().AddComponent<SorcererWithWrongNixInjectComponent>();

            TestInjector Injector = new TestInjector(wrongSorcerer);

            Assert.Throws<TestInjectorException>(() => Injector.CheckAndInjectAll());
        }

        [Test]
        public void TestInjector_ShouldThrowExceptionWhen_Injecting_WrongNixInjectAttribute()
        {
            SorcererWithWrongNixInject wrongSorcerer = new GameObject().AddComponent<SorcererWithWrongNixInject>();

            TestInjector Injector = new TestInjector(wrongSorcerer);

            Assert.Throws<TestInjectorException>(() => Injector.CheckAndInjectAll());
        }

        [Test]
        public void NixInjector_ShouldThrowExceptionWhen_Injecting_WrongNixInjectComponentAttribute()
        {
            SorcererWithWrongNixInjectComponent wrongSorcerer = new GameObject().AddComponent<SorcererWithWrongNixInjectComponent>();

            NixInjector Injector = new NixInjector(wrongSorcerer);

            Assert.Throws<NixInjectorException>(() => Injector.CheckAndInjectAll());
        }

        [Test]
        public void NixInjector_ShouldThrowExceptionWhen_Injecting_WrongNixInjectAttributeOnComponent()
        {
            SorcererWithWrongNixInject wrongSorcerer = new GameObject().AddComponent<SorcererWithWrongNixInject>();

            NixInjector Injector = new NixInjector(wrongSorcerer);

            Assert.Throws<NixInjectorException>(() => Injector.CheckAndInjectAll());
        }

        [Test]
        public void NixInjector_ShouldThrowExceptionWhen_Injecting_WrongNixInjectAttributeOnNotInterface()
        {
            SorcererWithWrongNixInjectNotInterface wrongSorcerer = new GameObject().AddComponent<SorcererWithWrongNixInjectNotInterface>();

            NixInjector Injector = new NixInjector(wrongSorcerer);

            Assert.Throws<NixInjectorException>(() => Injector.CheckAndInjectAll());
        }
    }
}
