using ScriptExample.Characters;
using ScriptExample.Characters.SameNamings;
using NixiTestTools;
using NUnit.Framework;
using ScriptExample.Characters.Broken;
using System;
using Tests.Builders;
using UnityEngine;

namespace Tests.TestTools
{
    internal sealed class TestInjecterTests
    {
        [Test]
        public void InjectOnRecursiveInjection_ShouldThrowException()
        {
            InfiniteRecursionSorcerer infiniteRecursionSorcerer = new GameObject().AddComponent<InfiniteRecursionSorcerer>();

            TestInjecter injecter = new TestInjecter(infiniteRecursionSorcerer);

            Assert.Throws<StackOverflowException>(() => injecter.CheckAndInjectAll());
        }

        [Test]
        public void InjectOnRecursiveInjectionWithInheritance_ShouldThrowException()
        {
            InfiniteRecursionSorcererWithInheritance infiniteRecursionSorcererWithInheritance = new GameObject().AddComponent<InfiniteRecursionSorcererWithInheritance>();

            TestInjecter injecter = new TestInjecter(infiniteRecursionSorcererWithInheritance);

            Assert.Throws<StackOverflowException>(() => injecter.CheckAndInjectAll());
        }

        [Test]
        public void SameInstanceNameAndType_ShouldReturnSameInstance()
        {
            RussianDoll russianDoll = InjectableBuilder<RussianDoll>.Create().Build();

            TestInjecter testInjecter = new TestInjecter(russianDoll);
            testInjecter.CheckAndInjectAll();

            Assert.That(russianDoll.ChildDoll.GetInstanceID(), Is.EqualTo(russianDoll.ChildDoll2.GetInstanceID()));
        }

        [Test]
        public void InjectField_WithNixInjectTestMockAttribute_ShouldNotFillField_ButExposeIt()
        {
            // Init
            Warrior warrior = InjectableBuilder<Warrior>.Create().Build();
            Assert.That(warrior.Parasite, Is.Null);

            // Inject
            TestInjecter testInjecter = new TestInjecter(warrior);
            testInjecter.CheckAndInjectAll();

            // Verify not injected because marked with NixInjectTestMockAttribute
            Assert.That(warrior.Parasite, Is.Null);

            // Check not a component registered but a field
            Assert.Throws<TestInjecterException>(() => testInjecter.GetComponent<Warrior>());

            Parasite parasite = ParasiteBuilder.Create().Build();
            testInjecter.InjectMock(parasite);

            Assert.That(warrior.Parasite, Is.Not.Null);
            Assert.That(warrior.Parasite.GetInstanceID(), Is.EqualTo(parasite.GetInstanceID()));
        }
    }
}