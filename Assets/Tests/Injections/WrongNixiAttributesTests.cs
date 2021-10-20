using Assets.ScriptExample.Characters.Broken;
using Nixi.Injections.Injecters;
using NixiTestTools;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Tests.Injections
{
    internal sealed class WrongNixiAttributesTests
    {
        [Test]
        public void TestInjecter_ShouldThrowExceptionWhen_Injecting_WrongNixInjectComponentAttribute()
        {
            SorcererWithWrongNixInjectComponent wrongSorcerer = new GameObject().AddComponent<SorcererWithWrongNixInjectComponent>();

            TestInjecter injecter = new TestInjecter(wrongSorcerer);

            Assert.Throws<NixInjecterException>(() => wrongSorcerer.BuildInjections(injecter));
        }

        [Test]
        public void TestInjecter_ShouldThrowExceptionWhen_Injecting_WrongNixInjectAttribute()
        {
            SorcererWithWrongNixInject wrongSorcerer = new GameObject().AddComponent<SorcererWithWrongNixInject>();

            TestInjecter injecter = new TestInjecter(wrongSorcerer);

            Assert.Throws<NixInjecterException>(() => wrongSorcerer.BuildInjections(injecter));
        }

        [Test]
        public void NixInjecter_ShouldThrowExceptionWhen_Injecting_WrongNixInjectComponentAttribute()
        {
            SorcererWithWrongNixInjectComponent wrongSorcerer = new GameObject().AddComponent<SorcererWithWrongNixInjectComponent>();

            NixInjecter injecter = new NixInjecter(wrongSorcerer);

            Assert.Throws<NixInjecterException>(() => wrongSorcerer.BuildInjections(injecter));
        }

        [Test]
        public void NixInjecter_ShouldThrowExceptionWhen_Injecting_WrongNixInjectAttributeOnComponent()
        {
            SorcererWithWrongNixInject wrongSorcerer = new GameObject().AddComponent<SorcererWithWrongNixInject>();

            NixInjecter injecter = new NixInjecter(wrongSorcerer);

            Assert.Throws<NixInjecterException>(() => wrongSorcerer.BuildInjections(injecter));
        }

        [Test]
        public void NixInjecter_ShouldThrowExceptionWhen_Injecting_WrongNixInjectAttributeOnNotInterface()
        {
            SorcererWithWrongNixInjectNotInterface wrongSorcerer = new GameObject().AddComponent<SorcererWithWrongNixInjectNotInterface>();

            NixInjecter injecter = new NixInjecter(wrongSorcerer);

            Assert.Throws<NixInjecterException>(() => wrongSorcerer.BuildInjections(injecter));
        }
    }
}
