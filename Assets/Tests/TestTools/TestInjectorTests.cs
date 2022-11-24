using NixiTestTools;
using NUnit.Framework;
using ScriptExample.Characters;
using ScriptExample.Characters.Broken;
using ScriptExample.Characters.SameNamings;
using System;
using Tests.Builders;
using UnityEngine;

namespace Tests.TestTools
{
    internal sealed class TestInjectorTests
    {
        [Test]
        public void InjectOnRecursiveInjection_ShouldThrowException()
        {
            InfiniteRecursionSorcerer infiniteRecursionSorcerer = new GameObject().AddComponent<InfiniteRecursionSorcerer>();

            TestInjector Injector = new TestInjector(infiniteRecursionSorcerer);

            Assert.Throws<StackOverflowException>(() => Injector.CheckAndInjectAll());
        }

        [Test]
        public void InjectOnRecursiveInjectionWithInheritance_ShouldThrowException()
        {
            InfiniteRecursionSorcererWithInheritance infiniteRecursionSorcererWithInheritance = new GameObject().AddComponent<InfiniteRecursionSorcererWithInheritance>();

            TestInjector Injector = new TestInjector(infiniteRecursionSorcererWithInheritance);

            Assert.Throws<StackOverflowException>(() => Injector.CheckAndInjectAll());
        }

        [Test]
        public void SameInstanceNameAndType_ShouldReturnSameInstance()
        {
            RussianDoll russianDoll = InjectableBuilder<RussianDoll>.Create().Build();

            TestInjector testInjector = new TestInjector(russianDoll);
            testInjector.CheckAndInjectAll();

            Assert.That(russianDoll.ChildDoll.GetInstanceID(), Is.EqualTo(russianDoll.ChildDoll2.GetInstanceID()));
        }

        [Test]
        public void InjectField_WithNixInjectTestMockAttribute_ShouldNotFillField_ButExposeIt()
        {
            // Init
            Warrior warrior = InjectableBuilder<Warrior>.Create().Build();
            Assert.That(warrior.Parasite, Is.Null);

            // Inject
            TestInjector testInjector = new TestInjector(warrior);
            testInjector.CheckAndInjectAll();

            // Verify not injected because marked with NixInjectTestMockAttribute
            Assert.That(warrior.Parasite, Is.Null);

            // Check not a component registered but a field
            Assert.Throws<TestInjectorException>(() => testInjector.GetComponent<Parasite>());

            Parasite parasite = ParasiteBuilder.Create().Build();
            testInjector.InjectField(parasite);

            Assert.That(warrior.Parasite, Is.Not.Null);
            Assert.That(warrior.Parasite.GetInstanceID(), Is.EqualTo(parasite.GetInstanceID()));
        }
    }
}