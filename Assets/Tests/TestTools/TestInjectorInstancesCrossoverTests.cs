using Nixi.Injections;
using NixiTestTools;
using NUnit.Framework;
using ScriptExample.AllParentsCases;
using ScriptExample.ComponentsWithEnumerable;
using ScriptExample.Farms;
using ScriptExample.Flowers;
using ScriptExample.OrphanRootComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using Tests.Builders;
using UnityEngine;
using UnityEngine.UI;

namespace Tests.TestTools
{
    internal sealed class TestInjectorInstancesCrossoverTests
    {
        #region GetComponent
        [Test]
        public void GetComponentOnSameType_AtSameLevel_ShouldReturnSameInstance()
        {
            // Arrange
            Parent parent = InjectableBuilder<Parent>.Create().Build();
            TestInjector Injector = new TestInjector(parent);
            Injector.CheckAndInjectAll();

            // Check
            Assert.NotNull(parent.FirstChild);
            Assert.NotNull(parent.SecondChild);

            // Parent gameobject = gameobject of first field (second field is tested below)
            Assert.That(parent.FirstChild.gameObject.GetInstanceID(), Is.EqualTo(parent.gameObject.GetInstanceID()));

            Assert.That(parent.FirstChild.GetInstanceID(), Is.EqualTo(parent.SecondChild.GetInstanceID()));
            Assert.That(parent.FirstChild.gameObject.GetInstanceID(), Is.EqualTo(parent.SecondChild.gameObject.GetInstanceID()));
            Assert.That(parent.FirstChild.transform.GetInstanceID(), Is.EqualTo(parent.SecondChild.transform.GetInstanceID()));
        }

        [Test]
        public void GetComponentOnSameTransform_AtSameLevel_ShouldReturnSameInstance()
        {
            // Arrange
            ParentWithTransformChild parent = InjectableBuilder<ParentWithTransformChild>.Create().Build();
            TestInjector Injector = new TestInjector(parent);
            Injector.CheckAndInjectAll();

            // Check
            Assert.NotNull(parent.FirstTransform);
            Assert.NotNull(parent.SecondTransform);

            // Parent gameobject = gameobject of first field (second field is tested below)
            Assert.That(parent.FirstTransform.gameObject.GetInstanceID(), Is.EqualTo(parent.gameObject.GetInstanceID()));

            Assert.That(parent.FirstTransform.GetInstanceID(), Is.EqualTo(parent.SecondTransform.GetInstanceID()));
            Assert.That(parent.FirstTransform.gameObject.GetInstanceID(), Is.EqualTo(parent.SecondTransform.gameObject.GetInstanceID()));
            Assert.That(parent.FirstTransform.transform.GetInstanceID(), Is.EqualTo(parent.SecondTransform.transform.GetInstanceID()));
        }

        [Test]
        public void GetComponentOnSameParentTransformFirstThenSkill_AtSameLevel_ShouldReturnSameInstance()
        {
            // Arrange
            ParentTransformFirstThenSkill parent = InjectableBuilder<ParentTransformFirstThenSkill>.Create().Build();
            TestInjector Injector = new TestInjector(parent);
            Injector.CheckAndInjectAll();

            // Check
            Assert.NotNull(parent.ATransform);
            Assert.NotNull(parent.ZSkill);

            // Parent gameobject = gameobject of first field (second field is tested below)
            Assert.That(parent.ATransform.gameObject.GetInstanceID(), Is.EqualTo(parent.gameObject.GetInstanceID()));

            Assert.That(parent.ATransform.GetInstanceID(), Is.Not.EqualTo(parent.ZSkill.GetInstanceID()));
            Assert.That(parent.ATransform.gameObject.GetInstanceID(), Is.EqualTo(parent.ZSkill.gameObject.GetInstanceID()));
            Assert.That(parent.ATransform.transform.GetInstanceID(), Is.EqualTo(parent.ZSkill.transform.GetInstanceID()));
        }

        [Test]
        public void GetComponentOnSameParentSkillFirstThenTransform_AtSameLevel_ShouldReturnSameInstance()
        {
            // Arrange
            ParentSkillFirstThenTransform parent = InjectableBuilder<ParentSkillFirstThenTransform>.Create().Build();
            TestInjector Injector = new TestInjector(parent);
            Injector.CheckAndInjectAll();

            // Check
            Assert.NotNull(parent.ASkill);
            Assert.NotNull(parent.ZTransform);

            // Parent gameobject = gameobject of first field (second field is tested below)
            Assert.That(parent.ASkill.gameObject.GetInstanceID(), Is.EqualTo(parent.gameObject.GetInstanceID()));

            Assert.That(parent.ASkill.GetInstanceID(), Is.Not.EqualTo(parent.ZTransform.GetInstanceID()));
            Assert.That(parent.ASkill.gameObject.GetInstanceID(), Is.EqualTo(parent.ZTransform.gameObject.GetInstanceID()));
            Assert.That(parent.ASkill.transform.GetInstanceID(), Is.EqualTo(parent.ZTransform.transform.GetInstanceID()));
        }

        [Test]
        public void GetComponentShouldHaveSameGameObject_ButDifferentComponentId_WhenDifferent()
        {
            // Arrange
            ParentSameLevelComponentButDifferent parent = InjectableBuilder<ParentSameLevelComponentButDifferent>.Create().Build();
            TestInjector Injector = new TestInjector(parent);
            Injector.CheckAndInjectAll();

            // Check
            Assert.NotNull(parent.Child);
            Assert.NotNull(parent.Skill);

            // Parent gameobject = gameobject of first field (second field is tested below)
            Assert.That(parent.Child.gameObject.GetInstanceID(), Is.EqualTo(parent.gameObject.GetInstanceID()));

            Assert.That(parent.Child.GetInstanceID(), Is.Not.EqualTo(parent.Skill.GetInstanceID()));
            Assert.That(parent.Child.gameObject.GetInstanceID(), Is.EqualTo(parent.Skill.gameObject.GetInstanceID()));
            Assert.That(parent.Child.transform.GetInstanceID(), Is.EqualTo(parent.Skill.transform.GetInstanceID()));
        }
        #endregion GetComponent

        #region GetComponentList
        [Test]
        public void AddAndGetComponentListOnTwoIdenticalList_ShouldReturnSameInstance()
        {
            // Arrange
            ParentWithSameChildLists parent = InjectableBuilder<ParentWithSameChildLists>.Create().Build();
            TestInjector Injector = new TestInjector(parent);
            Injector.CheckAndInjectAll();

            // Check init
            Assert.NotNull(parent.FirstChildList);
            Assert.IsEmpty(parent.FirstChildList);
            Assert.NotNull(parent.SecondChildList);
            Assert.IsEmpty(parent.SecondChildList);

            // Add child in list (should impact both)
            Child newChild = Injector.InitSingleEnumerableComponent<Child>();

            // Check same list
            Assert.That(parent.FirstChildList.Count, Is.EqualTo(1));
            Assert.That(parent.FirstChildList[0].GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));

            Assert.That(parent.SecondChildList.Count, Is.EqualTo(1));
            Assert.That(parent.SecondChildList[0].GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));

            // Check from getter
            IEnumerable<Child> childrenFromInjectorFirst = Injector.GetEnumerableComponents<Child>("FirstChildList");
            Assert.That(childrenFromInjectorFirst.Count, Is.EqualTo(1));
            Assert.That(childrenFromInjectorFirst.Single().GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));

            IEnumerable<Child> childrenFromInjectorSecond = Injector.GetEnumerableComponents<Child>("SecondChildList");
            Assert.That(childrenFromInjectorSecond.Count, Is.EqualTo(1));
            Assert.That(childrenFromInjectorSecond.Single().GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));
        }

        [Test]
        public void AddAndGetComponentListOnTwoDifferentsEnumerables_ShouldReturnSameInstance()
        {
            // Arrange
            var parent = InjectableBuilder<ParentWithSameChildListsDifferentsEnumerables>.Create().Build();
            TestInjector Injector = new TestInjector(parent);
            Injector.CheckAndInjectAll();

            // Check init
            Assert.NotNull(parent.FirstChildList);
            Assert.IsEmpty(parent.FirstChildList);
            Assert.NotNull(parent.SecondChildList);
            Assert.IsEmpty(parent.SecondChildList);
            Assert.NotNull(parent.ThirdChildArray);
            Assert.IsEmpty(parent.ThirdChildArray);

            // Add child in list (should impact both)
            IEnumerable<Child> childrenInjected = Injector.InitEnumerableComponents<Child>(2);
            Child newChild = childrenInjected.First();
            Child secondChild = childrenInjected.Skip(1).First();

            // Check same list
            Assert.That(parent.FirstChildList.Count, Is.EqualTo(2));
            Assert.That(parent.FirstChildList[0].GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));
            Assert.That(parent.FirstChildList[1].GetInstanceID(), Is.EqualTo(secondChild.GetInstanceID()));

            Assert.That(parent.SecondChildList.Count(), Is.EqualTo(2));
            Assert.That(parent.SecondChildList.First().GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));
            Assert.That(parent.SecondChildList.Skip(1).First().GetInstanceID(), Is.EqualTo(secondChild.GetInstanceID()));

            Assert.That(parent.ThirdChildArray.Length, Is.EqualTo(2));
            Assert.That(parent.ThirdChildArray[0].GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));
            Assert.That(parent.ThirdChildArray[1].GetInstanceID(), Is.EqualTo(secondChild.GetInstanceID()));

            // Check from getter
            IEnumerable<Child> childrenFromInjectorList = Injector.GetEnumerableComponents<Child>("FirstChildList");
            Assert.That(childrenFromInjectorList.Count, Is.EqualTo(2));
            Assert.That(childrenFromInjectorList.First().GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));
            Assert.That(childrenFromInjectorList.Skip(1).First().GetInstanceID(), Is.EqualTo(secondChild.GetInstanceID()));

            IEnumerable<Child> childrenFromInjectorEnumerable = Injector.GetEnumerableComponents<Child>("SecondChildList");
            Assert.That(childrenFromInjectorEnumerable.Count, Is.EqualTo(2));
            Assert.That(childrenFromInjectorEnumerable.First().GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));
            Assert.That(childrenFromInjectorEnumerable.Skip(1).First().GetInstanceID(), Is.EqualTo(secondChild.GetInstanceID()));

            IEnumerable<Child> childrenFromInjectorArray = Injector.GetEnumerableComponents<Child>("ThirdChildArray");
            Assert.That(childrenFromInjectorArray.Count, Is.EqualTo(2));
            Assert.That(childrenFromInjectorArray.First().GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));
            Assert.That(childrenFromInjectorArray.Skip(1).First().GetInstanceID(), Is.EqualTo(secondChild.GetInstanceID()));
        }

        [Test]
        public void AddAndGetComponentGrandParentWithChildListAndParent_ShouldReturnDifferentInstanceFromParent()
        {
            // Arrange
            GrandParentWithChildListAndParent grandParent = InjectableBuilder<GrandParentWithChildListAndParent>.Create().Build();
            TestInjector Injector = new TestInjector(grandParent);
            Injector.CheckAndInjectAll();

            // Check init
            Assert.NotNull(grandParent.FirstChildList);
            Assert.IsEmpty(grandParent.FirstChildList);
            Assert.NotNull(grandParent.ParentWithSameChildLists);
            Assert.IsEmpty(grandParent.ParentWithSameChildLists.FirstChildList);
            Assert.IsEmpty(grandParent.ParentWithSameChildLists.SecondChildList);

            // Add child in list (should impact both)
            Child newChild = Injector.InitSingleEnumerableComponent<Child>();

            // Check same list
            Assert.That(grandParent.FirstChildList.Count, Is.EqualTo(1));
            Assert.That(grandParent.FirstChildList[0].GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));

            Assert.IsEmpty(grandParent.ParentWithSameChildLists.FirstChildList);
            Assert.IsEmpty(grandParent.ParentWithSameChildLists.SecondChildList);

            // Check from getter
            IEnumerable<Child> grandParentChildrenFromInjector = Injector.GetEnumerableComponents<Child>();
            Assert.That(grandParentChildrenFromInjector.Count, Is.EqualTo(1));
            Assert.That(grandParentChildrenFromInjector.Single().GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));

            // Check from getter subchild lists empties
            ParentWithSameChildLists parentWithSameChildLists = Injector.GetComponent<ParentWithSameChildLists>();
            Assert.That(parentWithSameChildLists, Is.Not.Null);
            Assert.That(parentWithSameChildLists.FirstChildList, Is.Empty);
            Assert.That(parentWithSameChildLists.SecondChildList, Is.Empty);

            // Add child into subchild lists, should impact both, but not parent
            Child subChild = Injector.InitSingleEnumerableComponent<Child>(parentWithSameChildLists);
            Assert.That(parentWithSameChildLists.FirstChildList.Count, Is.EqualTo(1));
            Assert.That(parentWithSameChildLists.FirstChildList.Single().GetInstanceID(), Is.EqualTo(subChild.GetInstanceID()));
            Assert.That(parentWithSameChildLists.SecondChildList.Count, Is.EqualTo(1));
            Assert.That(parentWithSameChildLists.SecondChildList.Single().GetInstanceID(), Is.EqualTo(subChild.GetInstanceID()));

            Assert.That(grandParentChildrenFromInjector.Count, Is.EqualTo(1));
            Assert.That(grandParentChildrenFromInjector.Single().GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));
            Assert.That(grandParentChildrenFromInjector.Single().GetInstanceID(), Is.Not.EqualTo(subChild.GetInstanceID()));
        }

        [Test]
        public void InitEnumerableComponentFromDerivedType_AtCurrent_ShouldFillOnlyTargetedEnumerable_AndNotInheritance()
        {
            // Arrange
            Farm farm = InjectableBuilder<Farm>.Create().Build();
            TestInjector Injector = new TestInjector(farm);
            Injector.CheckAndInjectAll();

            Type[] animalTypes = new Type[] { typeof(Animal), typeof(Dog), typeof(Dog), typeof(Cat), typeof(Cat), typeof(Cat) };

            // Init and returned
            IEnumerable<Animal> animals = Injector.InitEnumerableComponentsWithTypes<Animal>(animalTypes);
            List<Animal> animalsList = animals.ToList();

            Assert.That(animalTypes.Length, Is.EqualTo(animalsList.Count));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], animalsList[i]);
            }

            // All cat enumerables are empty!!!
            Assert.IsEmpty(farm.Cats);
            Assert.IsEmpty(farm.CatsArray);
            Assert.IsEmpty(farm.CatsEnumerable);

            // List from class
            Assert.That(animalTypes.Length, Is.EqualTo(farm.Animals.Count));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], farm.Animals[i]);
            }

            // Enumerable from class
            Assert.That(animalTypes.Length, Is.EqualTo(farm.AnimalsEnumerable.Count()));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], farm.AnimalsEnumerable.ElementAt(i));
            }

            // Array from class
            Assert.That(animalTypes.Length, Is.EqualTo(farm.AnimalsArray.Length));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], farm.AnimalsArray[i]);
            }

            // List from Injector
            IEnumerable<Animal> listFromInjector = Injector.GetEnumerableComponents<Animal>("Animals");
            Assert.That(animalTypes.Length, Is.EqualTo(listFromInjector.Count()));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], listFromInjector.ElementAt(i));
            }

            // Enumerable from Injector
            IEnumerable<Animal> enumerableFromInjector = Injector.GetEnumerableComponents<Animal>("AnimalsEnumerable");
            Assert.That(animalTypes.Length, Is.EqualTo(enumerableFromInjector.Count()));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], enumerableFromInjector.ElementAt(i));
            }

            // Array from Injector
            IEnumerable<Animal> arrayFromInjector = Injector.GetEnumerableComponents<Animal>("AnimalsArray");
            Assert.That(animalTypes.Length, Is.EqualTo(arrayFromInjector.Count()));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], arrayFromInjector.ElementAt(i));
            }
        }

        [Test]
        public void InitEnumerableComponentFromDerivedType_AtParent_ShouldFillOnlyTargetedEnumerable_AndNotInheritance()
        {
            // Arrange
            FarmWithParent farm = InjectableBuilder<FarmWithParent>.Create().Build();
            TestInjector Injector = new TestInjector(farm);
            Injector.CheckAndInjectAll();

            Type[] animalTypes = new Type[] { typeof(Animal), typeof(Dog), typeof(Dog), typeof(Cat), typeof(Cat), typeof(Cat) };

            // Init and returned
            IEnumerable<Animal> animals = Injector.InitEnumerableComponentsWithTypes<Animal>(GameObjectLevel.Parent, animalTypes);
            List<Animal> animalsList = animals.ToList();

            Assert.That(animalTypes.Length, Is.EqualTo(animalsList.Count));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], animalsList[i]);
            }

            // All cat enumerables are empty!!!
            Assert.IsEmpty(farm.Cats);
            Assert.IsEmpty(farm.CatsArray);
            Assert.IsEmpty(farm.CatsEnumerable);

            // List from class
            Assert.That(animalTypes.Length, Is.EqualTo(farm.Animals.Count));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], farm.Animals[i]);
            }

            // Enumerable from class
            Assert.That(animalTypes.Length, Is.EqualTo(farm.AnimalsEnumerable.Count()));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], farm.AnimalsEnumerable.ElementAt(i));
            }

            // Array from class
            Assert.That(animalTypes.Length, Is.EqualTo(farm.AnimalsArray.Length));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], farm.AnimalsArray[i]);
            }

            // List from Injector
            IEnumerable<Animal> listFromInjector = Injector.GetEnumerableComponents<Animal>("Animals");
            Assert.That(animalTypes.Length, Is.EqualTo(listFromInjector.Count()));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], listFromInjector.ElementAt(i));
            }

            // Enumerable from Injector
            IEnumerable<Animal> enumerableFromInjector = Injector.GetEnumerableComponents<Animal>("AnimalsEnumerable");
            Assert.That(animalTypes.Length, Is.EqualTo(enumerableFromInjector.Count()));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], enumerableFromInjector.ElementAt(i));
            }

            // Array from Injector
            IEnumerable<Animal> arrayFromInjector = Injector.GetEnumerableComponents<Animal>("AnimalsArray");
            Assert.That(animalTypes.Length, Is.EqualTo(arrayFromInjector.Count()));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], arrayFromInjector.ElementAt(i));
            }
        }

        [Test]
        public void InitEnumerableComponentFromDerivedType_AtChildren_ShouldFillOnlyTargetedEnumerable_AndNotInheritance()
        {
            // Arrange
            FarmWithChildren farm = InjectableBuilder<FarmWithChildren>.Create().Build();
            TestInjector Injector = new TestInjector(farm);
            Injector.CheckAndInjectAll();

            Type[] animalTypes = new Type[] { typeof(Animal), typeof(Dog), typeof(Dog), typeof(Cat), typeof(Cat), typeof(Cat) };

            // Init and returned
            IEnumerable<Animal> animals = Injector.InitEnumerableComponentsWithTypes<Animal>(GameObjectLevel.Children, animalTypes);
            List<Animal> animalsList = animals.ToList();

            Assert.That(animalTypes.Length, Is.EqualTo(animalsList.Count));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], animalsList[i]);
            }

            // All cat enumerables are empty!!!
            Assert.IsEmpty(farm.Cats);
            Assert.IsEmpty(farm.CatsArray);
            Assert.IsEmpty(farm.CatsEnumerable);

            // List from class
            Assert.That(animalTypes.Length, Is.EqualTo(farm.Animals.Count));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], farm.Animals[i]);
            }

            // Enumerable from class
            Assert.That(animalTypes.Length, Is.EqualTo(farm.AnimalsEnumerable.Count()));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], farm.AnimalsEnumerable.ElementAt(i));
            }

            // Array from class
            Assert.That(animalTypes.Length, Is.EqualTo(farm.AnimalsArray.Length));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], farm.AnimalsArray[i]);
            }

            // List from Injector
            IEnumerable<Animal> listFromInjector = Injector.GetEnumerableComponents<Animal>("Animals");
            Assert.That(animalTypes.Length, Is.EqualTo(listFromInjector.Count()));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], listFromInjector.ElementAt(i));
            }

            // Enumerable from Injector
            IEnumerable<Animal> enumerableFromInjector = Injector.GetEnumerableComponents<Animal>("AnimalsEnumerable");
            Assert.That(animalTypes.Length, Is.EqualTo(enumerableFromInjector.Count()));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], enumerableFromInjector.ElementAt(i));
            }

            // Array from Injector
            IEnumerable<Animal> arrayFromInjector = Injector.GetEnumerableComponents<Animal>("AnimalsArray");
            Assert.That(animalTypes.Length, Is.EqualTo(arrayFromInjector.Count()));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], arrayFromInjector.ElementAt(i));
            }
        }

        [Test]
        public void InitEnumerableComponentFromDerivedType_AtCurrentLevel_ShouldFillOnlyTargetedEnumerable_AndInheritanceToo_Reverse()
        {
            // Arrange
            Farm farm = InjectableBuilder<Farm>.Create().Build();
            TestInjector Injector = new TestInjector(farm);
            Injector.CheckAndInjectAll();

            // Init and returned
            IEnumerable<Cat> cats = Injector.InitEnumerableComponentsWithTypes<Cat>(typeof(Cat), typeof(Cat));

            Assert.AreEqual(2, cats.Count());

            Assert.AreEqual(2, farm.Animals.Count);
            Assert.AreEqual(2, farm.Cats.Count);
            Assert.AreEqual(2, farm.AnimalsArray.Length);
            Assert.AreEqual(2, farm.CatsArray.Length);
            Assert.AreEqual(2, farm.AnimalsEnumerable.Count());
            Assert.AreEqual(2, farm.CatsEnumerable.Count());
        }

        [Test]
        public void InitEnumerableComponentFromDerivedType_AtChildrenLevel_ShouldFillOnlyTargetedEnumerable_AndInheritanceToo_Reverse()
        {
            // Arrange
            FarmWithChildren farm = InjectableBuilder<FarmWithChildren>.Create().Build();
            TestInjector Injector = new TestInjector(farm);
            Injector.CheckAndInjectAll();

            // Init and returned
            IEnumerable<Cat> cats = Injector.InitEnumerableComponentsWithTypes<Cat>(GameObjectLevel.Children, typeof(Cat), typeof(Cat));

            Assert.AreEqual(2, cats.Count());

            Assert.AreEqual(2, farm.Animals.Count);
            Assert.AreEqual(2, farm.Cats.Count);
            Assert.AreEqual(2, farm.AnimalsArray.Length);
            Assert.AreEqual(2, farm.CatsArray.Length);
            Assert.AreEqual(2, farm.AnimalsEnumerable.Count());
            Assert.AreEqual(2, farm.CatsEnumerable.Count());
        }

        [Test]
        public void InitEnumerableComponentFromDerivedType_AtParentLevel_ShouldFillOnlyTargetedEnumerable_AndInheritanceToo_Reverse()
        {
            // Arrange
            FarmWithParent farm = InjectableBuilder<FarmWithParent>.Create().Build();
            TestInjector Injector = new TestInjector(farm);
            Injector.CheckAndInjectAll();

            // Init and returned
            IEnumerable<Cat> cats = Injector.InitEnumerableComponentsWithTypes<Cat>(GameObjectLevel.Parent, typeof(Cat), typeof(Cat));

            Assert.AreEqual(2, cats.Count());

            Assert.AreEqual(2, farm.Animals.Count);
            Assert.AreEqual(2, farm.Cats.Count);
            Assert.AreEqual(2, farm.AnimalsArray.Length);
            Assert.AreEqual(2, farm.CatsArray.Length);
            Assert.AreEqual(2, farm.AnimalsEnumerable.Count());
            Assert.AreEqual(2, farm.CatsEnumerable.Count());
        }

        [Test]
        public void InitEnumerableComponentFromDerivedType_AtAllLevels_ShouldFillOnlyTargetedEnumerable_AndInheritanceToo_Reverse()
        {
            // Arrange
            FarmWithAllLevels farm = InjectableBuilder<FarmWithAllLevels>.Create().Build();
            TestInjector Injector = new TestInjector(farm);
            Injector.CheckAndInjectAll();

            // Init and returned
            IEnumerable<Cat> catsChildren = Injector.InitEnumerableComponentsWithTypes<Cat>(GameObjectLevel.Children, typeof(Cat), typeof(Cat));
            IEnumerable<Cat> cats = Injector.InitEnumerableComponentsWithTypes<Cat>(typeof(Cat), typeof(Cat), typeof(Cat));
            IEnumerable<Cat> catsParent = Injector.InitEnumerableComponentsWithTypes<Cat>(GameObjectLevel.Parent, typeof(Cat), typeof(Cat), typeof(Cat), typeof(Cat));

            // Check enumerables
            Assert.AreEqual(2, catsChildren.Count());
            Assert.AreEqual(3, cats.Count());
            Assert.AreEqual(4, catsParent.Count());

            // Check children
            Assert.AreEqual(2, farm.AnimalsChildren.Count);
            Assert.AreEqual(2, farm.CatsChildren.Count);
            Assert.AreEqual(2, farm.AnimalsArrayChildren.Length);
            Assert.AreEqual(2, farm.CatsArrayChildren.Length);
            Assert.AreEqual(2, farm.AnimalsEnumerableChildren.Count());
            Assert.AreEqual(2, farm.CatsEnumerableChildren.Count());

            // Check current
            Assert.AreEqual(3, farm.Animals.Count);
            Assert.AreEqual(3, farm.Cats.Count);
            Assert.AreEqual(3, farm.AnimalsArray.Length);
            Assert.AreEqual(3, farm.CatsArray.Length);
            Assert.AreEqual(3, farm.AnimalsEnumerable.Count());
            Assert.AreEqual(3, farm.CatsEnumerable.Count());

            // Check parent
            Assert.AreEqual(4, farm.AnimalsParent.Count);
            Assert.AreEqual(4, farm.CatsParent.Count);
            Assert.AreEqual(4, farm.AnimalsArrayParent.Length);
            Assert.AreEqual(4, farm.CatsArrayParent.Length);
            Assert.AreEqual(4, farm.AnimalsEnumerableParent.Count());
            Assert.AreEqual(4, farm.CatsEnumerableParent.Count());
        }

        [Test]
        public void InitEnumerableComponentsWithTypes_ShouldFillFieldInRecursiveComponentInjected()
        {
            // Arrange
            ParentFarm parentFarm = InjectableBuilder<ParentFarm>.Create().Build();
            TestInjector Injector = new TestInjector(parentFarm);
            Injector.CheckAndInjectAll();

            // Throws because nothing at this level
            Assert.Throws<TestInjectorException>(() => Injector.InitEnumerableComponentsWithTypes<Cat>(typeof(Cat), typeof(Cat)));

            // Get farm
            Farm farm = Injector.GetComponent<Farm>();

            // Init and returned
            IEnumerable<Cat> cats = Injector.InitEnumerableComponentsWithTypes<Cat>(farm, typeof(Cat), typeof(Cat));

            // Check all cats fields
            Assert.AreEqual(2, cats.Count());
            Assert.AreEqual(2, farm.Cats.Count);
            Assert.AreEqual(2, farm.CatsArray.Length);
            Assert.AreEqual(2, farm.CatsEnumerable.Count());

            Assert.AreEqual(2, farm.Animals.Count);
            Assert.AreEqual(2, farm.AnimalsArray.Length);
            Assert.AreEqual(2, farm.AnimalsEnumerable.Count());
        }

        [Test]
        public void InitEnumerableComponents_ShouldFillFieldInRecursiveComponentInjected()
        {
            // Arrange
            ParentFarm parentFarm = InjectableBuilder<ParentFarm>.Create().Build();
            TestInjector Injector = new TestInjector(parentFarm);
            Injector.CheckAndInjectAll();

            // Throws because nothing at this level
            Assert.Throws<TestInjectorException>(() => Injector.InitEnumerableComponents<Cat>(2));

            // Get farm
            Farm farm = Injector.GetComponent<Farm>();

            // Init and returned
            IEnumerable<Cat> cats = Injector.InitEnumerableComponents<Cat>(2, farm);

            // Check all cats fields
            Assert.AreEqual(2, cats.Count());
            Assert.AreEqual(2, farm.Cats.Count);
            Assert.AreEqual(2, farm.CatsArray.Length);
            Assert.AreEqual(2, farm.CatsEnumerable.Count());

            Assert.AreEqual(2, farm.Animals.Count);
            Assert.AreEqual(2, farm.AnimalsArray.Length);
            Assert.AreEqual(2, farm.AnimalsEnumerable.Count());
        }
        #endregion GetComponentList

        #region GetComponentList Children, current and parents
        [Test]
        public void FullBasketGetComponentExample_ShouldFillOnlyParents()
        {
            // Arrange
            FullBasketListExample fullBasket = InjectableBuilder<FullBasketListExample>.Create().Build();
            TestInjector Injector = new TestInjector(fullBasket);
            Injector.CheckAndInjectAll();

            // Add in list (build component)
            IEnumerable<Fruit> fruitsInjected = Injector.InitEnumerableComponents<Fruit>(GameObjectLevel.Parent, 2);
            Fruit first = fruitsInjected.First();
            Fruit second = fruitsInjected.Skip(1).First();

            // Link to interface
            List<IFruit> fruits = new List<IFruit> { first, second };
            Injector.InjectField(fruits, "IFruitsListParent");
            Injector.InjectField(fruits.AsEnumerable(), "IFruitsEnumerableParent");

            // Only parent should have
            Assert.That(fullBasket.FruitsListParent.Count, Is.EqualTo(2));
            Assert.That(fullBasket.FruitsEnumerableParent.Count, Is.EqualTo(2));
            Assert.That(fullBasket.IFruitsListParent.Count, Is.EqualTo(2));
            Assert.That(fullBasket.IFruitsEnumerableParent.Count, Is.EqualTo(2));

            // Current should not have
            Assert.IsEmpty(fullBasket.FruitsList);
            Assert.IsEmpty(fullBasket.FruitsEnumerable);
            Assert.Null(fullBasket.IFruitsList);
            Assert.Null(fullBasket.IFruitsEnumerable);

            // Child should not have
            Assert.IsEmpty(fullBasket.FruitsListChildren);
            Assert.IsEmpty(fullBasket.FruitsEnumerableChildren);
            Assert.Null(fullBasket.IFruitsListChildren);
            Assert.Null(fullBasket.IFruitsEnumerableChildren);

            // Checks with injectableInstance (parent test)
            foreach (Fruit fruit in new[] { first, second })
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(fullBasket.transform.GetInstanceID()));
            }

            // Compare first and second
            Assert.That(first.gameObject.GetInstanceID(), Is.Not.EqualTo(second.gameObject.GetInstanceID()));
            Assert.That(first.transform.GetInstanceID(), Is.Not.EqualTo(second.transform.GetInstanceID()));
            Assert.That(first.GetInstanceID(), Is.Not.EqualTo(second.GetInstanceID()));

            // Check get component Enumerable
            IEnumerable<Fruit> fruitsEnumerableGetted = Injector.GetEnumerableComponents<Fruit>("FruitsEnumerableParent");
            foreach (Fruit fruit in fruitsEnumerableGetted)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(fullBasket.transform.GetInstanceID()));
            }

            // Check get component List
            IEnumerable<Fruit> fruitsListGetted = Injector.GetEnumerableComponents<Fruit>("FruitsListParent");
            foreach (Fruit fruit in fruitsListGetted)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(fullBasket.transform.GetInstanceID()));
            }
        }

        [Test]
        public void FullBasketGetComponentExample_ShouldFillOnlyCurrent()
        {
            // Arrange
            FullBasketListExample fullBasket = InjectableBuilder<FullBasketListExample>.Create().Build();
            TestInjector Injector = new TestInjector(fullBasket);
            Injector.CheckAndInjectAll();

            // Add in list (build component)
            IEnumerable<Fruit> fruitsInjected = Injector.InitEnumerableComponents<Fruit>(2);
            Fruit first = fruitsInjected.First();
            Fruit second = fruitsInjected.Skip(1).First();

            // Link to interface
            List<IFruit> fruits = new List<IFruit> { first, second };
            Injector.InjectField(fruits, "IFruitsList");
            Injector.InjectField(fruits.AsEnumerable(), "IFruitsEnumerable");

            // Parent should not have
            Assert.IsEmpty(fullBasket.FruitsListParent);
            Assert.IsEmpty(fullBasket.FruitsEnumerableParent);
            Assert.Null(fullBasket.IFruitsListParent);
            Assert.Null(fullBasket.IFruitsEnumerableParent);

            // Only current should have
            Assert.That(fullBasket.FruitsList.Count, Is.EqualTo(2));
            Assert.That(fullBasket.FruitsEnumerable.Count, Is.EqualTo(2));
            Assert.That(fullBasket.IFruitsList.Count, Is.EqualTo(2));
            Assert.That(fullBasket.IFruitsEnumerable.Count, Is.EqualTo(2));

            // Child should not have
            Assert.IsEmpty(fullBasket.FruitsListChildren);
            Assert.IsEmpty(fullBasket.FruitsEnumerableChildren);
            Assert.Null(fullBasket.IFruitsListChildren);
            Assert.Null(fullBasket.IFruitsEnumerableChildren);

            // Checks with injectableInstance (current test)
            foreach (Fruit fruit in new[] { first, second })
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.EqualTo(fullBasket.transform.GetInstanceID()));
            }

            // Compare first and second
            Assert.That(first.gameObject.GetInstanceID(), Is.EqualTo(second.gameObject.GetInstanceID()));
            Assert.That(first.transform.GetInstanceID(), Is.EqualTo(second.transform.GetInstanceID()));
            Assert.That(first.GetInstanceID(), Is.Not.EqualTo(second.GetInstanceID()));

            // Check get component Enumerable
            IEnumerable<Fruit> fruitsEnumerableGetted = Injector.GetEnumerableComponents<Fruit>("FruitsEnumerable");
            foreach (Fruit fruit in fruitsEnumerableGetted)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.EqualTo(fullBasket.transform.GetInstanceID()));
            }

            // Check get component List
            IEnumerable<Fruit> fruitsListGetted = Injector.GetEnumerableComponents<Fruit>("FruitsList");
            foreach (Fruit fruit in fruitsListGetted)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.EqualTo(fullBasket.transform.GetInstanceID()));
            }
        }

        [Test]
        public void FullBasketGetComponentExample_ShouldFillOnlyChildren()
        {
            // Arrange
            FullBasketListExample fullBasket = InjectableBuilder<FullBasketListExample>.Create().Build();
            TestInjector Injector = new TestInjector(fullBasket);
            Injector.CheckAndInjectAll();

            // Add in list (build component)
            IEnumerable<Fruit> fruitsInjected = Injector.InitEnumerableComponents<Fruit>(GameObjectLevel.Children, 2, fullBasket);
            Fruit first = fruitsInjected.First();
            Fruit second = fruitsInjected.Skip(1).First();

            // Link to interface
            List<IFruit> fruits = new List<IFruit> { first, second };
            Injector.InjectField(fruits, "IFruitsListChildren");
            Injector.InjectField(fruits.AsEnumerable(), "IFruitsEnumerableChildren");

            // Parent should not have
            Assert.IsEmpty(fullBasket.FruitsListParent);
            Assert.IsEmpty(fullBasket.FruitsEnumerableParent);
            Assert.Null(fullBasket.IFruitsListParent);
            Assert.Null(fullBasket.IFruitsEnumerableParent);

            // Current not have
            Assert.IsEmpty(fullBasket.FruitsList);
            Assert.IsEmpty(fullBasket.FruitsEnumerable);
            Assert.Null(fullBasket.IFruitsList);
            Assert.Null(fullBasket.IFruitsEnumerable);

            // Only child should have
            Assert.That(fullBasket.FruitsListChildren.Count, Is.EqualTo(2));
            Assert.That(fullBasket.FruitsEnumerableChildren.Count, Is.EqualTo(2));
            Assert.That(fullBasket.IFruitsListChildren.Count, Is.EqualTo(2));
            Assert.That(fullBasket.IFruitsEnumerableChildren.Count, Is.EqualTo(2));

            // Checks with injectableInstance (child test)
            foreach (Fruit fruit in new[] { first, second })
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(fullBasket.transform.GetInstanceID()));
            }

            // Compare first and second
            Assert.That(first.gameObject.GetInstanceID(), Is.Not.EqualTo(second.gameObject.GetInstanceID()));
            Assert.That(first.transform.GetInstanceID(), Is.Not.EqualTo(second.transform.GetInstanceID()));
            Assert.That(first.GetInstanceID(), Is.Not.EqualTo(second.GetInstanceID()));

            // Check get component Enumerable
            IEnumerable<Fruit> fruitsEnumerableGetted = Injector.GetEnumerableComponents<Fruit>("FruitsEnumerableChildren");
            foreach (Fruit fruit in fruitsEnumerableGetted)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(fullBasket.transform.GetInstanceID()));
            }

            // Check get component List
            IEnumerable<Fruit> fruitsListGetted = Injector.GetEnumerableComponents<Fruit>("FruitsListChildren");
            foreach (Fruit fruit in fruitsListGetted)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(fullBasket.transform.GetInstanceID()));
            }
        }
        #endregion GetComponentList Children, current and parents

        #region GetComponentList recursively Children, current and parents
        [Test]
        public void AboveFullBasketGetComponentExample_ShouldFillOnlyParents_RecursivelyInSecondLevel()
        {
            // Arrange
            AboveFullBasketListExample aboveFullBasket = InjectableBuilder<AboveFullBasketListExample>.Create().Build();
            TestInjector Injector = new TestInjector(aboveFullBasket);
            Injector.CheckAndInjectAll();

            // Get child component
            FullBasketListExample fullBasket = Injector.GetComponent<FullBasketListExample>();

            // Add in list (build component)
            IEnumerable<Fruit> fruitsInjected = Injector.InitEnumerableComponents<Fruit>(GameObjectLevel.Parent, 2, fullBasket);
            Fruit first = fruitsInjected.First();
            Fruit second = fruitsInjected.Skip(1).First();

            // Link to interface
            List<IFruit> fruits = new List<IFruit> { first, second };
            Injector.InjectField(fruits, "IFruitsListParent", fullBasket);
            Injector.InjectField(fruits.AsEnumerable(), "IFruitsEnumerableParent", fullBasket);

            // Only parent should have
            Assert.That(aboveFullBasket.ParentBasket.FruitsListParent.Count, Is.EqualTo(2));
            Assert.That(aboveFullBasket.ParentBasket.FruitsEnumerableParent.Count, Is.EqualTo(2));
            Assert.That(aboveFullBasket.ParentBasket.IFruitsListParent.Count, Is.EqualTo(2));
            Assert.That(aboveFullBasket.ParentBasket.IFruitsEnumerableParent.Count, Is.EqualTo(2));

            // Current should not have
            Assert.IsEmpty(aboveFullBasket.ParentBasket.FruitsList);
            Assert.IsEmpty(aboveFullBasket.ParentBasket.FruitsEnumerable);
            Assert.Null(aboveFullBasket.ParentBasket.IFruitsList);
            Assert.Null(aboveFullBasket.ParentBasket.IFruitsEnumerable);

            // Child should not have
            Assert.IsEmpty(aboveFullBasket.ParentBasket.FruitsListChildren);
            Assert.IsEmpty(aboveFullBasket.ParentBasket.FruitsEnumerableChildren);
            Assert.Null(aboveFullBasket.ParentBasket.IFruitsListChildren);
            Assert.Null(aboveFullBasket.ParentBasket.IFruitsEnumerableChildren);

            // Checks with injectableInstance (parent test)
            foreach (Fruit fruit in new[] { first, second })
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.transform.GetInstanceID()));
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(fullBasket.transform.GetInstanceID()));
            }

            // Compare first and second
            Assert.That(first.gameObject.GetInstanceID(), Is.Not.EqualTo(second.gameObject.GetInstanceID()));
            Assert.That(first.transform.GetInstanceID(), Is.Not.EqualTo(second.transform.GetInstanceID()));
            Assert.That(first.GetInstanceID(), Is.Not.EqualTo(second.GetInstanceID()));

            // Check get component Enumerable
            IEnumerable<Fruit> fruitsEnumerableGetted = Injector.GetEnumerableComponents<Fruit>("FruitsEnumerableParent", fullBasket);
            foreach (Fruit fruit in fruitsEnumerableGetted)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.transform.GetInstanceID()));
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(fullBasket.transform.GetInstanceID()));
            }

            // Check get component List
            IEnumerable<Fruit> fruitsListGetted = Injector.GetEnumerableComponents<Fruit>("FruitsListParent", fullBasket);
            foreach (Fruit fruit in fruitsListGetted)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.transform.GetInstanceID()));
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(fullBasket.transform.GetInstanceID()));
            }
        }

        [Test]
        public void AboveFullBasketGetComponentExample_ShouldFillOnlyCurrent_RecursivelyInSecondLevel()
        {
            // Arrange
            AboveFullBasketListExample aboveFullBasket = InjectableBuilder<AboveFullBasketListExample>.Create().Build();
            TestInjector Injector = new TestInjector(aboveFullBasket);
            Injector.CheckAndInjectAll();

            // Get child component
            FullBasketListExample fullBasket = Injector.GetComponent<FullBasketListExample>();

            // Add in list (build component)
            IEnumerable<Fruit> fruitsInjected = Injector.InitEnumerableComponents<Fruit>(2, fullBasket);
            Fruit first = fruitsInjected.First();
            Fruit second = fruitsInjected.Skip(1).First();

            // Link to interface
            List<IFruit> fruits = new List<IFruit> { first, second };
            Injector.InjectField(fruits, "IFruitsList", fullBasket);
            Injector.InjectField(fruits.AsEnumerable(), "IFruitsEnumerable", fullBasket);

            // Parent should not have
            Assert.IsEmpty(aboveFullBasket.ParentBasket.FruitsListParent);
            Assert.IsEmpty(aboveFullBasket.ParentBasket.FruitsEnumerableParent);
            Assert.Null(aboveFullBasket.ParentBasket.IFruitsListParent);
            Assert.Null(aboveFullBasket.ParentBasket.IFruitsEnumerableParent);

            // Only current should have
            Assert.That(aboveFullBasket.ParentBasket.FruitsList.Count, Is.EqualTo(2));
            Assert.That(aboveFullBasket.ParentBasket.FruitsEnumerable.Count, Is.EqualTo(2));
            Assert.That(aboveFullBasket.ParentBasket.IFruitsList.Count, Is.EqualTo(2));
            Assert.That(aboveFullBasket.ParentBasket.IFruitsEnumerable.Count, Is.EqualTo(2));

            // Child should not have
            Assert.IsEmpty(aboveFullBasket.ParentBasket.FruitsListChildren);
            Assert.IsEmpty(aboveFullBasket.ParentBasket.FruitsEnumerableChildren);
            Assert.Null(aboveFullBasket.ParentBasket.IFruitsListChildren);
            Assert.Null(aboveFullBasket.ParentBasket.IFruitsEnumerableChildren);

            // Checks with injectableInstance (current test)
            foreach (Fruit fruit in new[] { first, second })
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.transform.GetInstanceID()));
                Assert.That(fruit.gameObject.GetInstanceID(), Is.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.EqualTo(fullBasket.transform.GetInstanceID()));
            }

            // Compare first and second
            Assert.That(first.gameObject.GetInstanceID(), Is.EqualTo(second.gameObject.GetInstanceID()));
            Assert.That(first.transform.GetInstanceID(), Is.EqualTo(second.transform.GetInstanceID()));
            Assert.That(first.GetInstanceID(), Is.Not.EqualTo(second.GetInstanceID()));

            // Check get component Enumerable
            IEnumerable<Fruit> fruitsEnumerableGetted = Injector.GetEnumerableComponents<Fruit>("FruitsEnumerable", fullBasket);
            foreach (Fruit fruit in fruitsEnumerableGetted)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.transform.GetInstanceID()));
                Assert.That(fruit.gameObject.GetInstanceID(), Is.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.EqualTo(fullBasket.transform.GetInstanceID()));
            }

            // Check get component List
            IEnumerable<Fruit> fruitsListGetted = Injector.GetEnumerableComponents<Fruit>("FruitsList", fullBasket);
            foreach (Fruit fruit in fruitsListGetted)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.transform.GetInstanceID()));
                Assert.That(fruit.gameObject.GetInstanceID(), Is.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.EqualTo(fullBasket.transform.GetInstanceID()));
            }
        }

        [Test]
        public void AboveFullBasketGetComponentExample_ShouldFillOnlyChildren_RecursivelyInSecondLevel()
        {
            // Arrange
            AboveFullBasketListExample aboveFullBasket = InjectableBuilder<AboveFullBasketListExample>.Create().Build();
            TestInjector Injector = new TestInjector(aboveFullBasket);
            Injector.CheckAndInjectAll();

            // Get child component
            FullBasketListExample fullBasket = Injector.GetComponent<FullBasketListExample>();

            // Add in list (build component)
            IEnumerable<Fruit> fruitsInjected = Injector.InitEnumerableComponents<Fruit>(GameObjectLevel.Children, 2, fullBasket);
            Fruit first = fruitsInjected.First();
            Fruit second = fruitsInjected.Skip(1).First();

            // Link to interface
            List<IFruit> fruits = new List<IFruit> { first, second };
            Injector.InjectField(fruits, "IFruitsListChildren", fullBasket);
            Injector.InjectField(fruits.AsEnumerable(), "IFruitsEnumerableChildren", fullBasket);

            // Parent should not have
            Assert.IsEmpty(aboveFullBasket.ParentBasket.FruitsListParent);
            Assert.IsEmpty(aboveFullBasket.ParentBasket.FruitsEnumerableParent);
            Assert.Null(aboveFullBasket.ParentBasket.IFruitsListParent);
            Assert.Null(aboveFullBasket.ParentBasket.IFruitsEnumerableParent);

            // Current not have
            Assert.IsEmpty(aboveFullBasket.ParentBasket.FruitsList);
            Assert.IsEmpty(aboveFullBasket.ParentBasket.FruitsEnumerable);
            Assert.Null(aboveFullBasket.ParentBasket.IFruitsList);
            Assert.Null(aboveFullBasket.ParentBasket.IFruitsEnumerable);

            // Only child should have
            Assert.That(aboveFullBasket.ParentBasket.FruitsListChildren.Count, Is.EqualTo(2));
            Assert.That(aboveFullBasket.ParentBasket.FruitsEnumerableChildren.Count, Is.EqualTo(2));
            Assert.That(aboveFullBasket.ParentBasket.IFruitsListChildren.Count, Is.EqualTo(2));
            Assert.That(aboveFullBasket.ParentBasket.IFruitsEnumerableChildren.Count, Is.EqualTo(2));

            // Checks with injectableInstance (child test)
            foreach (Fruit fruit in new[] { first, second })
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.transform.GetInstanceID()));
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(fullBasket.transform.GetInstanceID()));
            }

            // Compare first and second
            Assert.That(first.gameObject.GetInstanceID(), Is.Not.EqualTo(second.gameObject.GetInstanceID()));
            Assert.That(first.transform.GetInstanceID(), Is.Not.EqualTo(second.transform.GetInstanceID()));
            Assert.That(first.GetInstanceID(), Is.Not.EqualTo(second.GetInstanceID()));

            // Check get component Enumerable
            IEnumerable<Fruit> fruitsEnumerableGetted = Injector.GetEnumerableComponents<Fruit>("FruitsEnumerableChildren", fullBasket);
            foreach (Fruit fruit in fruitsEnumerableGetted)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.transform.GetInstanceID()));
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(fullBasket.transform.GetInstanceID()));
            }

            // Check get component List
            IEnumerable<Fruit> fruitsListGetted = Injector.GetEnumerableComponents<Fruit>("FruitsListChildren", fullBasket);
            foreach (Fruit fruit in fruitsListGetted)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.transform.GetInstanceID()));
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(fullBasket.transform.GetInstanceID()));
            }            
        }
        #endregion GetComponentList recursively Children, current and parents

        #region RootComponent
        [Test]
        public void RootComponentFromChild_ButNoParentFilled_ShouldFillAndHaveGoodEmptyGameObject()
        {
            // SubRootComponent filled but not rootComponent
            // We have to handle subRootComponent correctly
            // and its parent must be an empty gameObject

            Orphan orphan = InjectableBuilder<Orphan>.Create().Build();
            TestInjector Injector = new TestInjector(orphan);
            Injector.CheckAndInjectAll();

            // Check init
            Assert.That(orphan.Image, Is.Not.Null);
            Assert.That(orphan.Slider, Is.Not.Null);

            // Check same gameObject at child level
            Assert.That(orphan.Image.gameObject.GetInstanceID(), Is.EqualTo(orphan.Slider.gameObject.GetInstanceID()));

            // Check parent exists
            Assert.That(orphan.Image.transform.parent, Is.Not.Null);
            Assert.That(orphan.Slider.transform.parent, Is.Not.Null);

            // Check same parent
            Assert.That(orphan.Image.transform.parent.GetInstanceID(), Is.EqualTo(orphan.Slider.transform.parent.GetInstanceID()));

            // Check parent has no component except transform
            Component[] components = orphan.Image.transform.parent.GetComponents<Component>();
            Assert.That(components.Length, Is.EqualTo(1));
            Assert.IsAssignableFrom(typeof(Transform), components[0]);
        }

        [Test]
        public void TwoRootComponent_WithSameRootNameAndSameSubGameObjectName_ButDifferentType_ShouldHaveSameGameObject()
        {
            Bouquet bouquet = InjectableBuilder<Bouquet>.Create().Build();
            TestInjector Injector = new TestInjector(bouquet);
            Injector.CheckAndInjectAll();

            DualFlower subRootIsolatedFlower = Injector.GetComponent<DualFlower>("SubRootIsolatedFlower");
            Image image = Injector.GetComponent<Image>();

            Assert.That(image.GetInstanceID(), Is.Not.EqualTo(subRootIsolatedFlower.GetInstanceID()));
            Assert.That(image.gameObject.GetInstanceID(), Is.EqualTo(subRootIsolatedFlower.gameObject.GetInstanceID()));
            Assert.That(image.transform.GetInstanceID(), Is.EqualTo(subRootIsolatedFlower.transform.GetInstanceID()));
        }

        [TestCase("ChildFlower")]
        [TestCase("ParentFlower")]
        [TestCase("RootIsolatedFlower")]
        [TestCase("SubRootIsolatedFlower")]
        public void AllRootComponent_ShouldBeSame_ForEveryComponentInjectionCases(string fieldNameToFind)
        {
            Bouquet bouquet = InjectableBuilder<Bouquet>.Create().Build();
            TestInjector Injector = new TestInjector(bouquet);
            Injector.CheckAndInjectAll();

            DualFlower referentialFlower = Injector.GetComponent<DualFlower>("ReferentialFlower");

            // Simplify referential accesses
            Flower flowerFieldOrigin = referentialFlower.FlowerField;
            Flower perfectFlowerOrigin = referentialFlower.PerfectFlower;

            // Initial checks
            Assert.NotNull(flowerFieldOrigin);
            Assert.NotNull(perfectFlowerOrigin);
            Assert.That(referentialFlower.PerfectFlower.transform.parent.GetInstanceID(), Is.EqualTo(referentialFlower.FlowerField.transform.GetInstanceID()));

            // Check every cases
            DualFlower flowerToCheck = Injector.GetComponent<DualFlower>(fieldNameToFind);
            Assert.That(flowerToCheck.FlowerField.GetInstanceID(), Is.EqualTo(flowerFieldOrigin.GetInstanceID()));
            Assert.That(flowerToCheck.PerfectFlower.GetInstanceID(), Is.EqualTo(perfectFlowerOrigin.GetInstanceID()));
            Assert.That(flowerToCheck.PerfectFlower.gameObject.GetInstanceID(), Is.EqualTo(perfectFlowerOrigin.gameObject.GetInstanceID()));
            Assert.That(flowerToCheck.PerfectFlower.transform.parent.GetInstanceID(), Is.EqualTo(flowerToCheck.FlowerField.transform.GetInstanceID()));
        }

        [Test]
        public void AllRootComponent_ShouldBeSame_ForEveryComponentInjectionCases_WithAddingInList()
        {
            Bouquet bouquet = InjectableBuilder<Bouquet>.Create().Build();
            TestInjector Injector = new TestInjector(bouquet);
            Injector.CheckAndInjectAll();

            DualFlower referentialFlower = Injector.GetComponent<DualFlower>("ReferentialFlower");

            // Simplify referential accesses
            Flower flowerFieldOrigin = referentialFlower.FlowerField;
            Flower perfectFlowerOrigin = referentialFlower.PerfectFlower;

            // Initial checks
            Assert.NotNull(flowerFieldOrigin);
            Assert.NotNull(perfectFlowerOrigin);
            Assert.That(referentialFlower.PerfectFlower.transform.parent.GetInstanceID(), Is.EqualTo(referentialFlower.FlowerField.transform.GetInstanceID()));

            // Add 2 in list
            IEnumerable<DualFlower> flowersInjected = Injector.InitEnumerableComponents<DualFlower>(2);
            DualFlower firstFlower = flowersInjected.First();
            DualFlower secondFlower = flowersInjected.Skip(1).First();

            Assert.That(firstFlower.GetInstanceID(), Is.Not.EqualTo(secondFlower.GetInstanceID()));

            // Check first
            Assert.That(firstFlower.FlowerField.GetInstanceID(), Is.EqualTo(flowerFieldOrigin.GetInstanceID()));
            Assert.That(firstFlower.PerfectFlower.GetInstanceID(), Is.EqualTo(perfectFlowerOrigin.GetInstanceID()));
            Assert.That(firstFlower.PerfectFlower.gameObject.GetInstanceID(), Is.EqualTo(perfectFlowerOrigin.gameObject.GetInstanceID()));
            Assert.That(firstFlower.PerfectFlower.transform.parent.GetInstanceID(), Is.EqualTo(firstFlower.FlowerField.transform.GetInstanceID()));

            // Check second
            Assert.That(secondFlower.FlowerField.GetInstanceID(), Is.EqualTo(flowerFieldOrigin.GetInstanceID()));
            Assert.That(secondFlower.PerfectFlower.GetInstanceID(), Is.EqualTo(perfectFlowerOrigin.GetInstanceID()));
            Assert.That(secondFlower.PerfectFlower.gameObject.GetInstanceID(), Is.EqualTo(perfectFlowerOrigin.gameObject.GetInstanceID()));
            Assert.That(secondFlower.PerfectFlower.transform.parent.GetInstanceID(), Is.EqualTo(secondFlower.FlowerField.transform.GetInstanceID()));            
        }

        [Test]
        public void SameRootOrSubRoot_ShouldHaveSameGameObjectWhenTypeDifferents_ButNotSameComponent()
        {
            // Init
            PlagueBouquet plagueBouquet = InjectableBuilder<PlagueBouquet>.Create().Build();
            TestInjector Injector = new TestInjector(plagueBouquet);
            Injector.CheckAndInjectAll();

            // Prepare
            Flower healthyFlower = Injector.GetComponent<Flower>("HealthyFlower");
            Flower subHealthyFlower = Injector.GetComponent<Flower>("SubHealthyFlower");
            PlagueFlower plagueFlower = Injector.GetComponent<PlagueFlower>("PlagueFlower");
            PlagueFlower subPlagueFlower = Injector.GetComponent<PlagueFlower>("SubPlagueFlower");            

            // Check SubHealthyFlower.transform.parent = HealthyFlower.transform
            Assert.That(subHealthyFlower.transform.parent.gameObject.GetInstanceID(), Is.EqualTo(healthyFlower.gameObject.GetInstanceID()));
            Assert.That(subHealthyFlower.transform.parent.GetInstanceID(), Is.EqualTo(healthyFlower.transform.GetInstanceID()));

            // Check SubPlagueFlower.transform.parent = PlagueFlower.transform
            Assert.That(subPlagueFlower.transform.parent.gameObject.GetInstanceID(), Is.EqualTo(plagueFlower.gameObject.GetInstanceID()));
            Assert.That(subPlagueFlower.transform.parent.GetInstanceID(), Is.EqualTo(plagueFlower.transform.GetInstanceID()));

            // Check healthyFlower not same componentId as plagueFlower, but same transform and gameObject
            Assert.That(healthyFlower.GetInstanceID(), Is.Not.EqualTo(plagueFlower.GetInstanceID()));
            Assert.That(healthyFlower.transform.GetInstanceID(), Is.EqualTo(plagueFlower.transform.GetInstanceID()));
            Assert.That(healthyFlower.gameObject.GetInstanceID(), Is.EqualTo(plagueFlower.gameObject.GetInstanceID()));

            // Check subHealthyFlower not same componentId as subPlagueFlower, but same transform and gameObject
            Assert.That(subHealthyFlower.GetInstanceID(), Is.Not.EqualTo(subPlagueFlower.GetInstanceID()));
            Assert.That(subHealthyFlower.transform.GetInstanceID(), Is.EqualTo(subPlagueFlower.transform.GetInstanceID()));
            Assert.That(subHealthyFlower.gameObject.GetInstanceID(), Is.EqualTo(subPlagueFlower.gameObject.GetInstanceID()));
        }
        #endregion RootComponent
    }
}
