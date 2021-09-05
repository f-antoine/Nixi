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
        public void TestInjecter_ShouldThrowExceptionWhen_Injecting_WrongNixInjectMonoBehaviourAttribute()
        {
            SorcererWithWrongNixInjectMonoBehaviour wrongSorcerer = new GameObject().AddComponent<SorcererWithWrongNixInjectMonoBehaviour>();

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
        public void NixInjecter_ShouldThrowExceptionWhen_Injecting_WrongNixInjectMonoBehaviourAttribute()
        {
            SorcererWithWrongNixInjectMonoBehaviour wrongSorcerer = new GameObject().AddComponent<SorcererWithWrongNixInjectMonoBehaviour>();

            NixInjecter injecter = new NixInjecter(wrongSorcerer);

            Assert.Throws<NixInjecterException>(() => wrongSorcerer.BuildInjections(injecter));
        }

        [Test]
        public void NixInjecter_ShouldThrowExceptionWhen_Injecting_WrongNixInjectAttributeOnMonoBehaviour()
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
