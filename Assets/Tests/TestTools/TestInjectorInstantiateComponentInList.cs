using Nixi.Injections;
using NixiTestTools;
using NUnit.Framework;
using ScriptExample.Characters;
using ScriptExample.Spawners;
using System.Collections.Generic;
using System.Linq;
using Tests.Builders;

namespace Tests.TestTools
{
    internal sealed class TestInjectorInstantiateComponentInList
    {
        [TestCase(null, GameObjectLevel.Children, GameObjectLevel.Parent)]
        [TestCase(GameObjectLevel.Children, null, GameObjectLevel.Parent)]
        [TestCase(GameObjectLevel.Parent, null, GameObjectLevel.Children)]
        public void InitEnumerableComponent_ShouldFillAllFields(GameObjectLevel? filledMethod, GameObjectLevel? emptyMethod,
                                                                GameObjectLevel? secondEmptyMethod)
        {
            // Init
            Spawner spawner = InjectableBuilder<Spawner>.Create().Build();
            TestInjector Injector = new TestInjector(spawner);
            Injector.CheckAndInjectAll();

            // Check empty
            Assert.IsEmpty(spawner.GetList(filledMethod));
            Assert.IsEmpty(spawner.GetList(emptyMethod));
            Assert.IsEmpty(spawner.GetList(secondEmptyMethod));

            Assert.IsEmpty(spawner.GetEnumerable(filledMethod));
            Assert.IsEmpty(spawner.GetEnumerable(emptyMethod));
            Assert.IsEmpty(spawner.GetEnumerable(secondEmptyMethod));

            Assert.IsEmpty(spawner.GetArray(filledMethod));
            Assert.IsEmpty(spawner.GetArray(emptyMethod));
            Assert.IsEmpty(spawner.GetArray(secondEmptyMethod));

            // Inject children
            Mob newMob;
            if (filledMethod.HasValue)
            {
                newMob = Injector.InitSingleEnumerableComponent<Mob>(filledMethod.Value);
            }
            else
            {
                newMob = Injector.InitSingleEnumerableComponent<Mob>();
            }

            // Check other not touched
            Assert.IsEmpty(spawner.GetList(emptyMethod));
            Assert.IsEmpty(spawner.GetEnumerable(emptyMethod));
            Assert.IsEmpty(spawner.GetArray(emptyMethod));

            Assert.IsEmpty(spawner.GetList(secondEmptyMethod));
            Assert.IsEmpty(spawner.GetEnumerable(secondEmptyMethod));
            Assert.IsEmpty(spawner.GetArray(secondEmptyMethod));

            Assert.That(spawner.GetList(filledMethod).Single().GetInstanceID(), Is.EqualTo(newMob.GetInstanceID()));
            Assert.That(spawner.GetEnumerable(filledMethod).Single().GetInstanceID(), Is.EqualTo(newMob.GetInstanceID()));
            Assert.That(spawner.GetArray(filledMethod).Single().GetInstanceID(), Is.EqualTo(newMob.GetInstanceID()));
        }

        [TestCase(null, "")]
        [TestCase(GameObjectLevel.Children, "Children")]
        [TestCase(GameObjectLevel.Parent, "Parent")]
        public void InitEnumerableComponent_ShouldInitAllFields_InjectInjectableAndUseSameInstances(GameObjectLevel? filledMethod, string suffix)
        {
            // Init
            Spawner spawner = InjectableBuilder<Spawner>.Create().Build();
            TestInjector Injector = new TestInjector(spawner);
            Injector.CheckAndInjectAll();

            // Inject single enumerable
            Mob newMob = filledMethod.HasValue ? Injector.InitSingleEnumerableComponent<Mob>(filledMethod.Value) : Injector.InitSingleEnumerableComponent<Mob>();

            // Check same mob
            Assert.That(spawner.GetList(filledMethod).Count, Is.EqualTo(1));
            Assert.That(spawner.GetList(filledMethod).Single().GetInstanceID(), Is.EqualTo(newMob.GetInstanceID()));

            Assert.That(spawner.GetEnumerable(filledMethod).Count(), Is.EqualTo(1));
            Assert.That(spawner.GetEnumerable(filledMethod).Single().GetInstanceID(), Is.EqualTo(newMob.GetInstanceID()));

            Assert.That(spawner.GetArray(filledMethod).Length, Is.EqualTo(1));
            Assert.That(spawner.GetArray(filledMethod).Single().GetInstanceID(), Is.EqualTo(newMob.GetInstanceID()));

            // Check mob is same from Injector
            Mob mobFromInjector = Injector.GetEnumerableComponents<Mob>($"mobList{suffix}").Single();
            Assert.NotNull(mobFromInjector);

            Assert.That(mobFromInjector.GetInstanceID(), Is.EqualTo(newMob.GetInstanceID()));
            Assert.That(spawner.GetList(filledMethod).Single().GetInstanceID(), Is.EqualTo(mobFromInjector.GetInstanceID()));
            Assert.That(spawner.GetEnumerable(filledMethod).Single().GetInstanceID(), Is.EqualTo(mobFromInjector.GetInstanceID()));
            Assert.That(spawner.GetArray(filledMethod).Single().GetInstanceID(), Is.EqualTo(mobFromInjector.GetInstanceID()));

            // Check contains skill
            Skill skillFromSpawnerList = spawner.GetList(filledMethod).Single().Skill;
            Skill skillFromSpawnerEnumerable = spawner.GetEnumerable(filledMethod).Single().Skill;
            Skill skillFromSpawnerArray = spawner.GetArray(filledMethod).Single().Skill;

            Assert.NotNull(skillFromSpawnerList);
            Assert.NotNull(skillFromSpawnerEnumerable);
            Assert.NotNull(skillFromSpawnerArray);

            // Check skill is same from Injector
            Skill skillFromInjector = Injector.GetComponent<Skill>(newMob);
            Assert.NotNull(skillFromInjector);

            Assert.That(skillFromInjector.GetInstanceID(), Is.EqualTo(mobFromInjector.Skill.GetInstanceID()));

            Assert.That(skillFromSpawnerList.GetInstanceID(), Is.EqualTo(skillFromInjector.GetInstanceID()));
            Assert.That(skillFromSpawnerEnumerable.GetInstanceID(), Is.EqualTo(skillFromInjector.GetInstanceID()));
            Assert.That(skillFromSpawnerArray.GetInstanceID(), Is.EqualTo(skillFromInjector.GetInstanceID()));
        }

        [Test]
        public void InitEnumerableComponentTwice_ShouldThrowException()
        {
            // Init
            Spawner spawner = InjectableBuilder<Spawner>.Create().Build();
            TestInjector Injector = new TestInjector(spawner);
            Injector.CheckAndInjectAll();

            // Checks
            Injector.InitEnumerableComponents<Mob>(GameObjectLevel.Children);
            Assert.Throws<TestInjectorException>(() => Injector.InitEnumerableComponents<Mob>(GameObjectLevel.Children));

            Injector.InitEnumerableComponents<Mob>();
            Assert.Throws<TestInjectorException>(() => Injector.InitEnumerableComponents<Mob>());

            Injector.InitEnumerableComponents<Mob>(GameObjectLevel.Parent);
            Assert.Throws<TestInjectorException>(() => Injector.InitEnumerableComponents<Mob>(GameObjectLevel.Parent));
        }

        [Test]
        public void InitSingleEnumerableComponentTwice_ShouldThrowException()
        {
            // Init
            Spawner spawner = InjectableBuilder<Spawner>.Create().Build();
            TestInjector Injector = new TestInjector(spawner);
            Injector.CheckAndInjectAll();

            // Checks
            Injector.InitSingleEnumerableComponent<Mob>(GameObjectLevel.Children);
            Assert.Throws<TestInjectorException>(() => Injector.InitSingleEnumerableComponent<Mob>(GameObjectLevel.Children));

            Injector.InitSingleEnumerableComponent<Mob>();
            Assert.Throws<TestInjectorException>(() => Injector.InitSingleEnumerableComponent<Mob>());

            Injector.InitSingleEnumerableComponent<Mob>(GameObjectLevel.Parent);
            Assert.Throws<TestInjectorException>(() => Injector.InitSingleEnumerableComponent<Mob>(GameObjectLevel.Parent));
        }

        [Test]
        public void InitEnumerableComponentWithTypesTwice_ShouldThrowException()
        {
            // Init
            Spawner spawner = InjectableBuilder<Spawner>.Create().Build();
            TestInjector Injector = new TestInjector(spawner);
            Injector.CheckAndInjectAll();

            // Checks
            Injector.InitEnumerableComponentsWithTypes<Mob>(GameObjectLevel.Children, typeof(Mob));
            Assert.Throws<TestInjectorException>(() => Injector.InitEnumerableComponentsWithTypes<Mob>(GameObjectLevel.Children, typeof(Mob)));

            Injector.InitEnumerableComponentsWithTypes<Mob>(typeof(Mob));
            Assert.Throws<TestInjectorException>(() => Injector.InitEnumerableComponentsWithTypes<Mob>(typeof(Mob)));

            Injector.InitEnumerableComponentsWithTypes<Mob>(GameObjectLevel.Parent, typeof(Mob));
            Assert.Throws<TestInjectorException>(() => Injector.InitEnumerableComponentsWithTypes<Mob>(GameObjectLevel.Parent, typeof(Mob)));
        }

        [TestCase(null, GameObjectLevel.Children, GameObjectLevel.Parent)]
        [TestCase(GameObjectLevel.Children, null, GameObjectLevel.Parent)]
        [TestCase(GameObjectLevel.Parent, null, GameObjectLevel.Children)]
        public void InitEnumerableComponentMany_ShouldInitAllFields_WithMany(GameObjectLevel? filledMethod, GameObjectLevel? emptyMethod, GameObjectLevel? secondEmptyMethod)
        {
            // Init
            Spawner spawner = InjectableBuilder<Spawner>.Create().Build();
            TestInjector Injector = new TestInjector(spawner);
            Injector.CheckAndInjectAll();

            // Check empty
            Assert.IsEmpty(spawner.GetList(filledMethod));
            Assert.IsEmpty(spawner.GetList(emptyMethod));
            Assert.IsEmpty(spawner.GetList(secondEmptyMethod));

            Assert.IsEmpty(spawner.GetEnumerable(filledMethod));
            Assert.IsEmpty(spawner.GetEnumerable(emptyMethod));
            Assert.IsEmpty(spawner.GetEnumerable(secondEmptyMethod));

            Assert.IsEmpty(spawner.GetArray(filledMethod));
            Assert.IsEmpty(spawner.GetArray(emptyMethod));
            Assert.IsEmpty(spawner.GetArray(secondEmptyMethod));

            // Inject children
            IEnumerable<Mob> newMobs;
            if (filledMethod.HasValue)
            {
                newMobs = Injector.InitEnumerableComponents<Mob>(filledMethod.Value, 2);
            }
            else
            {
                newMobs = Injector.InitEnumerableComponents<Mob>(2);
            }
            Mob firstMob = newMobs.First();
            Mob secondMob = newMobs.Skip(1).First();

            Assert.IsEmpty(spawner.GetList(emptyMethod));
            Assert.IsEmpty(spawner.GetEnumerable(emptyMethod));
            Assert.IsEmpty(spawner.GetArray(emptyMethod));

            Assert.IsEmpty(spawner.GetList(secondEmptyMethod));
            Assert.IsEmpty(spawner.GetEnumerable(secondEmptyMethod));
            Assert.IsEmpty(spawner.GetArray(secondEmptyMethod));

            Assert.That(spawner.GetList(filledMethod).Count, Is.EqualTo(2));
            Assert.That(spawner.GetList(filledMethod).First().GetInstanceID(), Is.EqualTo(firstMob.GetInstanceID()));
            Assert.That(spawner.GetList(filledMethod).Skip(1).First().GetInstanceID(), Is.EqualTo(secondMob.GetInstanceID()));
            Assert.NotNull(spawner.GetList(filledMethod).First().Skill);
            Assert.NotNull(spawner.GetList(filledMethod).Skip(1).First().Skill);

            Assert.That(spawner.GetEnumerable(filledMethod).Count(), Is.EqualTo(2));
            Assert.That(spawner.GetEnumerable(filledMethod).First().GetInstanceID(), Is.EqualTo(firstMob.GetInstanceID()));
            Assert.That(spawner.GetEnumerable(filledMethod).Skip(1).First().GetInstanceID(), Is.EqualTo(secondMob.GetInstanceID()));
            Assert.NotNull(spawner.GetEnumerable(filledMethod).First().Skill);
            Assert.NotNull(spawner.GetEnumerable(filledMethod).Skip(1).First().Skill);

            Assert.That(spawner.GetArray(filledMethod).Length, Is.EqualTo(2));
            Assert.That(spawner.GetArray(filledMethod).First().GetInstanceID(), Is.EqualTo(firstMob.GetInstanceID()));
            Assert.That(spawner.GetArray(filledMethod).Skip(1).First().GetInstanceID(), Is.EqualTo(secondMob.GetInstanceID()));
            Assert.NotNull(spawner.GetArray(filledMethod).First().Skill);
            Assert.NotNull(spawner.GetArray(filledMethod).Skip(1).First().Skill);
        }
    }
}
