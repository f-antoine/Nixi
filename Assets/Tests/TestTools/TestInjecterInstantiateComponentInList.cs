using Nixi.Injections.Attributes;
using NixiTestTools;
using NUnit.Framework;
using ScriptExample.Characters;
using ScriptExample.Spawners;
using System.Collections.Generic;
using System.Linq;
using Tests.Builders;

namespace Tests.TestTools
{
    internal sealed class TestInjecterInstantiateComponentInList
    {
        [TestCase(null, GameObjectLevel.Children, GameObjectLevel.Parent, "")]
        [TestCase(GameObjectLevel.Children, null, GameObjectLevel.Parent, "Childs")]
        [TestCase(GameObjectLevel.Parent, null, GameObjectLevel.Children, "Parent")]
        public void InitEnumerableComponent_ShouldFillAllFields(GameObjectLevel? filledMethod, GameObjectLevel? emptyMethod,
                                                                GameObjectLevel? secondEmptyMethod, string suffix)
        {
            // Init
            Spawner spawner = InjectableBuilder<Spawner>.Create().Build();
            TestInjecter injecter = new TestInjecter(spawner);
            injecter.CheckAndInjectAll();

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

            // Inject childs
            Mob newMob;
            if (filledMethod.HasValue)
            {
                newMob = injecter.InitSingleEnumerableComponent<Mob>(filledMethod.Value);
            }
            else
            {
                newMob = injecter.InitSingleEnumerableComponent<Mob>();
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
        [TestCase(GameObjectLevel.Children, "Childs")]
        [TestCase(GameObjectLevel.Parent, "Parent")]
        public void InitEnumerableComponent_ShouldInitAllFields_InjectInjectableAndUseSameInstances(GameObjectLevel? filledMethod, string suffix)
        {
            // Init
            Spawner spawner = InjectableBuilder<Spawner>.Create().Build();
            TestInjecter injecter = new TestInjecter(spawner);
            injecter.CheckAndInjectAll();

            // Inject single enumerable
            Mob newMob = filledMethod.HasValue ? injecter.InitSingleEnumerableComponent<Mob>(filledMethod.Value) : injecter.InitSingleEnumerableComponent<Mob>();

            // Check same mob
            Assert.That(spawner.GetList(filledMethod).Count, Is.EqualTo(1));
            Assert.That(spawner.GetList(filledMethod).Single().GetInstanceID(), Is.EqualTo(newMob.GetInstanceID()));

            Assert.That(spawner.GetEnumerable(filledMethod).Count(), Is.EqualTo(1));
            Assert.That(spawner.GetEnumerable(filledMethod).Single().GetInstanceID(), Is.EqualTo(newMob.GetInstanceID()));

            Assert.That(spawner.GetArray(filledMethod).Length, Is.EqualTo(1));
            Assert.That(spawner.GetArray(filledMethod).Single().GetInstanceID(), Is.EqualTo(newMob.GetInstanceID()));

            // Check mob is same from injecter
            Mob mobFromInjecter = injecter.GetEnumerableComponents<Mob>($"mobList{suffix}").Single();
            Assert.NotNull(mobFromInjecter);

            Assert.That(mobFromInjecter.GetInstanceID(), Is.EqualTo(newMob.GetInstanceID()));
            Assert.That(spawner.GetList(filledMethod).Single().GetInstanceID(), Is.EqualTo(mobFromInjecter.GetInstanceID()));
            Assert.That(spawner.GetEnumerable(filledMethod).Single().GetInstanceID(), Is.EqualTo(mobFromInjecter.GetInstanceID()));
            Assert.That(spawner.GetArray(filledMethod).Single().GetInstanceID(), Is.EqualTo(mobFromInjecter.GetInstanceID()));

            // Check contains skill
            Skill skillFromSpawnerList = spawner.GetList(filledMethod).Single().Skill;
            Skill skillFromSpawnerEnumerable = spawner.GetEnumerable(filledMethod).Single().Skill;
            Skill skillFromSpawnerArray = spawner.GetArray(filledMethod).Single().Skill;

            Assert.NotNull(skillFromSpawnerList);
            Assert.NotNull(skillFromSpawnerEnumerable);
            Assert.NotNull(skillFromSpawnerArray);

            // Check skill is same from injecter
            Skill skillFromInjecter = injecter.GetComponent<Skill>(newMob);
            Assert.NotNull(skillFromInjecter);

            Assert.That(skillFromInjecter.GetInstanceID(), Is.EqualTo(mobFromInjecter.Skill.GetInstanceID()));

            Assert.That(skillFromSpawnerList.GetInstanceID(), Is.EqualTo(skillFromInjecter.GetInstanceID()));
            Assert.That(skillFromSpawnerEnumerable.GetInstanceID(), Is.EqualTo(skillFromInjecter.GetInstanceID()));
            Assert.That(skillFromSpawnerArray.GetInstanceID(), Is.EqualTo(skillFromInjecter.GetInstanceID()));
        }

        [Test]
        public void InitEnumerableComponentTwice_ShouldThrowException()
        {
            // Init
            Spawner spawner = InjectableBuilder<Spawner>.Create().Build();
            TestInjecter injecter = new TestInjecter(spawner);
            injecter.CheckAndInjectAll();

            // Checks
            injecter.InitEnumerableComponents<Mob>(GameObjectLevel.Children);
            Assert.Throws<TestInjecterException>(() => injecter.InitEnumerableComponents<Mob>(GameObjectLevel.Children));

            injecter.InitEnumerableComponents<Mob>();
            Assert.Throws<TestInjecterException>(() => injecter.InitEnumerableComponents<Mob>());

            injecter.InitEnumerableComponents<Mob>(GameObjectLevel.Parent);
            Assert.Throws<TestInjecterException>(() => injecter.InitEnumerableComponents<Mob>(GameObjectLevel.Parent));
        }

        [Test]
        public void InitSingleEnumerableComponentTwice_ShouldThrowException()
        {
            // Init
            Spawner spawner = InjectableBuilder<Spawner>.Create().Build();
            TestInjecter injecter = new TestInjecter(spawner);
            injecter.CheckAndInjectAll();

            // Checks
            injecter.InitSingleEnumerableComponent<Mob>(GameObjectLevel.Children);
            Assert.Throws<TestInjecterException>(() => injecter.InitSingleEnumerableComponent<Mob>(GameObjectLevel.Children));

            injecter.InitSingleEnumerableComponent<Mob>();
            Assert.Throws<TestInjecterException>(() => injecter.InitSingleEnumerableComponent<Mob>());

            injecter.InitSingleEnumerableComponent<Mob>(GameObjectLevel.Parent);
            Assert.Throws<TestInjecterException>(() => injecter.InitSingleEnumerableComponent<Mob>(GameObjectLevel.Parent));
        }

        [Test]
        public void InitEnumerableComponentWithTypesTwice_ShouldThrowException()
        {
            // Init
            Spawner spawner = InjectableBuilder<Spawner>.Create().Build();
            TestInjecter injecter = new TestInjecter(spawner);
            injecter.CheckAndInjectAll();

            // Checks
            injecter.InitEnumerableComponentsWithTypes<Mob>(GameObjectLevel.Children, typeof(Mob));
            Assert.Throws<TestInjecterException>(() => injecter.InitEnumerableComponentsWithTypes<Mob>(GameObjectLevel.Children, typeof(Mob)));

            injecter.InitEnumerableComponentsWithTypes<Mob>(typeof(Mob));
            Assert.Throws<TestInjecterException>(() => injecter.InitEnumerableComponentsWithTypes<Mob>(typeof(Mob)));

            injecter.InitEnumerableComponentsWithTypes<Mob>(GameObjectLevel.Parent, typeof(Mob));
            Assert.Throws<TestInjecterException>(() => injecter.InitEnumerableComponentsWithTypes<Mob>(GameObjectLevel.Parent, typeof(Mob)));
        }

        [TestCase(null, GameObjectLevel.Children, GameObjectLevel.Parent)]
        [TestCase(GameObjectLevel.Children, null, GameObjectLevel.Parent)]
        [TestCase(GameObjectLevel.Parent, null, GameObjectLevel.Children)]
        public void InitEnumerableComponentMany_ShouldInitAllFields_WithMany(GameObjectLevel? filledMethod, GameObjectLevel? emptyMethod, GameObjectLevel? secondEmptyMethod)
        {
            // Init
            Spawner spawner = InjectableBuilder<Spawner>.Create().Build();
            TestInjecter injecter = new TestInjecter(spawner);
            injecter.CheckAndInjectAll();

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

            // Inject childs
            IEnumerable<Mob> newMobs;
            if (filledMethod.HasValue)
            {
                newMobs = injecter.InitEnumerableComponents<Mob>(filledMethod.Value, 2);
            }
            else
            {
                newMobs = injecter.InitEnumerableComponents<Mob>(2);
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
