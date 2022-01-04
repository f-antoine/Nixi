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
    internal sealed class TestInjecterInstancesCrossoverTests
    {
        #region GetComponent
        [Test]
        public void GetComponentOnSameType_AtSameLevel_ShouldReturnSameInstance()
        {
            // Arrange
            Parent parent = InjectableBuilder<Parent>.Create().Build();
            TestInjecter injecter = new TestInjecter(parent);
            injecter.CheckAndInjectAll();

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
            TestInjecter injecter = new TestInjecter(parent);
            injecter.CheckAndInjectAll();

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
            TestInjecter injecter = new TestInjecter(parent);
            injecter.CheckAndInjectAll();

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
            TestInjecter injecter = new TestInjecter(parent);
            injecter.CheckAndInjectAll();

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
            TestInjecter injecter = new TestInjecter(parent);
            injecter.CheckAndInjectAll();

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
            TestInjecter injecter = new TestInjecter(parent);
            injecter.CheckAndInjectAll();

            // Check init
            Assert.NotNull(parent.FirstChildList);
            Assert.IsEmpty(parent.FirstChildList);
            Assert.NotNull(parent.SecondChildList);
            Assert.IsEmpty(parent.SecondChildList);

            // Add child in list (should impact both)
            Child newChild = injecter.InitSingleEnumerableComponent<Child>();

            // Check same list
            Assert.That(parent.FirstChildList.Count, Is.EqualTo(1));
            Assert.That(parent.FirstChildList[0].GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));

            Assert.That(parent.SecondChildList.Count, Is.EqualTo(1));
            Assert.That(parent.SecondChildList[0].GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));

            // Check from getter
            IEnumerable<Child> childsFromInjecterFirst = injecter.GetEnumerableComponents<Child>("FirstChildList");
            Assert.That(childsFromInjecterFirst.Count, Is.EqualTo(1));
            Assert.That(childsFromInjecterFirst.Single().GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));

            IEnumerable<Child> childsFromInjecterSecond = injecter.GetEnumerableComponents<Child>("SecondChildList");
            Assert.That(childsFromInjecterSecond.Count, Is.EqualTo(1));
            Assert.That(childsFromInjecterSecond.Single().GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));
        }

        [Test]
        public void AddAndGetComponentListOnTwoDifferentsEnumerables_ShouldReturnSameInstance()
        {
            // Arrange
            var parent = InjectableBuilder<ParentWithSameChildListsDifferentsEnumerables>.Create().Build();
            TestInjecter injecter = new TestInjecter(parent);
            injecter.CheckAndInjectAll();

            // Check init
            Assert.NotNull(parent.FirstChildList);
            Assert.IsEmpty(parent.FirstChildList);
            Assert.NotNull(parent.SecondChildList);
            Assert.IsEmpty(parent.SecondChildList);
            Assert.NotNull(parent.ThirdChildArray);
            Assert.IsEmpty(parent.ThirdChildArray);

            // Add child in list (should impact both)
            IEnumerable<Child> childsInjected = injecter.InitEnumerableComponents<Child>(2);
            Child newChild = childsInjected.First();
            Child secondChild = childsInjected.Skip(1).First();

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
            IEnumerable<Child> childsFromInjecterList = injecter.GetEnumerableComponents<Child>("FirstChildList");
            Assert.That(childsFromInjecterList.Count, Is.EqualTo(2));
            Assert.That(childsFromInjecterList.First().GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));
            Assert.That(childsFromInjecterList.Skip(1).First().GetInstanceID(), Is.EqualTo(secondChild.GetInstanceID()));

            IEnumerable<Child> childsFromInjecterEnumerable = injecter.GetEnumerableComponents<Child>("SecondChildList");
            Assert.That(childsFromInjecterEnumerable.Count, Is.EqualTo(2));
            Assert.That(childsFromInjecterEnumerable.First().GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));
            Assert.That(childsFromInjecterEnumerable.Skip(1).First().GetInstanceID(), Is.EqualTo(secondChild.GetInstanceID()));

            IEnumerable<Child> childsFromInjecterArray = injecter.GetEnumerableComponents<Child>("ThirdChildArray");
            Assert.That(childsFromInjecterArray.Count, Is.EqualTo(2));
            Assert.That(childsFromInjecterArray.First().GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));
            Assert.That(childsFromInjecterArray.Skip(1).First().GetInstanceID(), Is.EqualTo(secondChild.GetInstanceID()));
        }

        [Test]
        public void AddAndGetComponentGrandParentWithChildListAndParent_ShouldReturnDifferentInstanceFromParent()
        {
            // Arrange
            GrandParentWithChildListAndParent grandParent = InjectableBuilder<GrandParentWithChildListAndParent>.Create().Build();
            TestInjecter injecter = new TestInjecter(grandParent);
            injecter.CheckAndInjectAll();

            // Check init
            Assert.NotNull(grandParent.FirstChildList);
            Assert.IsEmpty(grandParent.FirstChildList);
            Assert.NotNull(grandParent.ParentWithSameChildLists);
            Assert.IsEmpty(grandParent.ParentWithSameChildLists.FirstChildList);
            Assert.IsEmpty(grandParent.ParentWithSameChildLists.SecondChildList);

            // Add child in list (should impact both)
            Child newChild = injecter.InitSingleEnumerableComponent<Child>();

            // Check same list
            Assert.That(grandParent.FirstChildList.Count, Is.EqualTo(1));
            Assert.That(grandParent.FirstChildList[0].GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));

            Assert.IsEmpty(grandParent.ParentWithSameChildLists.FirstChildList);
            Assert.IsEmpty(grandParent.ParentWithSameChildLists.SecondChildList);

            // Check from getter
            IEnumerable<Child> grandParentChildsFromInjecter = injecter.GetEnumerableComponents<Child>();
            Assert.That(grandParentChildsFromInjecter.Count, Is.EqualTo(1));
            Assert.That(grandParentChildsFromInjecter.Single().GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));

            // Check from getter subchild lists empties
            ParentWithSameChildLists parentWithSameChildLists = injecter.GetComponent<ParentWithSameChildLists>();
            Assert.That(parentWithSameChildLists, Is.Not.Null);
            Assert.That(parentWithSameChildLists.FirstChildList, Is.Empty);
            Assert.That(parentWithSameChildLists.SecondChildList, Is.Empty);

            // Add child into subchild lists, should impact both, but not parent
            Child subChild = injecter.InitSingleEnumerableComponent<Child>(parentWithSameChildLists);
            Assert.That(parentWithSameChildLists.FirstChildList.Count, Is.EqualTo(1));
            Assert.That(parentWithSameChildLists.FirstChildList.Single().GetInstanceID(), Is.EqualTo(subChild.GetInstanceID()));
            Assert.That(parentWithSameChildLists.SecondChildList.Count, Is.EqualTo(1));
            Assert.That(parentWithSameChildLists.SecondChildList.Single().GetInstanceID(), Is.EqualTo(subChild.GetInstanceID()));

            Assert.That(grandParentChildsFromInjecter.Count, Is.EqualTo(1));
            Assert.That(grandParentChildsFromInjecter.Single().GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));
            Assert.That(grandParentChildsFromInjecter.Single().GetInstanceID(), Is.Not.EqualTo(subChild.GetInstanceID()));
        }

        [Test]
        public void InitEnumerableComponentFromDerivedType_AtCurrent_ShouldFillOnlyTargetedEnumerable_AndNotInheritance()
        {
            // Arrange
            Farm farm = InjectableBuilder<Farm>.Create().Build();
            TestInjecter injecter = new TestInjecter(farm);
            injecter.CheckAndInjectAll();

            Type[] animalTypes = new Type[] { typeof(Animal), typeof(Dog), typeof(Dog), typeof(Cat), typeof(Cat), typeof(Cat) };

            // Init and returned
            IEnumerable<Animal> animals = injecter.InitEnumerableComponentsWithTypes<Animal>(animalTypes);
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

            // List from injecter
            IEnumerable<Animal> listFromInjecter = injecter.GetEnumerableComponents<Animal>("Animals");
            Assert.That(animalTypes.Length, Is.EqualTo(listFromInjecter.Count()));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], listFromInjecter.ElementAt(i));
            }

            // Enumerable from injecter
            IEnumerable<Animal> enumerableFromInjecter = injecter.GetEnumerableComponents<Animal>("AnimalsEnumerable");
            Assert.That(animalTypes.Length, Is.EqualTo(enumerableFromInjecter.Count()));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], enumerableFromInjecter.ElementAt(i));
            }

            // Array from injecter
            IEnumerable<Animal> arrayFromInjecter = injecter.GetEnumerableComponents<Animal>("AnimalsArray");
            Assert.That(animalTypes.Length, Is.EqualTo(arrayFromInjecter.Count()));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], arrayFromInjecter.ElementAt(i));
            }
        }

        [Test]
        public void InitEnumerableComponentFromDerivedType_AtParent_ShouldFillOnlyTargetedEnumerable_AndNotInheritance()
        {
            // Arrange
            FarmWithParent farm = InjectableBuilder<FarmWithParent>.Create().Build();
            TestInjecter injecter = new TestInjecter(farm);
            injecter.CheckAndInjectAll();

            Type[] animalTypes = new Type[] { typeof(Animal), typeof(Dog), typeof(Dog), typeof(Cat), typeof(Cat), typeof(Cat) };

            // Init and returned
            IEnumerable<Animal> animals = injecter.InitEnumerableComponentsWithTypes<Animal>(GameObjectLevel.Parent, animalTypes);
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

            // List from injecter
            IEnumerable<Animal> listFromInjecter = injecter.GetEnumerableComponents<Animal>("Animals");
            Assert.That(animalTypes.Length, Is.EqualTo(listFromInjecter.Count()));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], listFromInjecter.ElementAt(i));
            }

            // Enumerable from injecter
            IEnumerable<Animal> enumerableFromInjecter = injecter.GetEnumerableComponents<Animal>("AnimalsEnumerable");
            Assert.That(animalTypes.Length, Is.EqualTo(enumerableFromInjecter.Count()));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], enumerableFromInjecter.ElementAt(i));
            }

            // Array from injecter
            IEnumerable<Animal> arrayFromInjecter = injecter.GetEnumerableComponents<Animal>("AnimalsArray");
            Assert.That(animalTypes.Length, Is.EqualTo(arrayFromInjecter.Count()));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], arrayFromInjecter.ElementAt(i));
            }
        }

        [Test]
        public void InitEnumerableComponentFromDerivedType_AtChilds_ShouldFillOnlyTargetedEnumerable_AndNotInheritance()
        {
            // Arrange
            FarmWithChilds farm = InjectableBuilder<FarmWithChilds>.Create().Build();
            TestInjecter injecter = new TestInjecter(farm);
            injecter.CheckAndInjectAll();

            Type[] animalTypes = new Type[] { typeof(Animal), typeof(Dog), typeof(Dog), typeof(Cat), typeof(Cat), typeof(Cat) };

            // Init and returned
            IEnumerable<Animal> animals = injecter.InitEnumerableComponentsWithTypes<Animal>(GameObjectLevel.Children, animalTypes);
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

            // List from injecter
            IEnumerable<Animal> listFromInjecter = injecter.GetEnumerableComponents<Animal>("Animals");
            Assert.That(animalTypes.Length, Is.EqualTo(listFromInjecter.Count()));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], listFromInjecter.ElementAt(i));
            }

            // Enumerable from injecter
            IEnumerable<Animal> enumerableFromInjecter = injecter.GetEnumerableComponents<Animal>("AnimalsEnumerable");
            Assert.That(animalTypes.Length, Is.EqualTo(enumerableFromInjecter.Count()));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], enumerableFromInjecter.ElementAt(i));
            }

            // Array from injecter
            IEnumerable<Animal> arrayFromInjecter = injecter.GetEnumerableComponents<Animal>("AnimalsArray");
            Assert.That(animalTypes.Length, Is.EqualTo(arrayFromInjecter.Count()));
            for (int i = 0; i < animalTypes.Length; i++)
            {
                Assert.IsAssignableFrom(animalTypes[i], arrayFromInjecter.ElementAt(i));
            }
        }

        [Test]
        public void InitEnumerableComponentFromDerivedType_AtCurrent_ShouldFillOnlyTargetedEnumerable_AndNotInheritance_Reverse()
        {
            // Arrange
            Farm farm = InjectableBuilder<Farm>.Create().Build();
            TestInjecter injecter = new TestInjecter(farm);
            injecter.CheckAndInjectAll();

            // Init and returned
            IEnumerable<Cat> cats = injecter.InitEnumerableComponentsWithTypes<Cat>(typeof(Cat), typeof(Cat));

            Assert.That(cats.Count(), Is.EqualTo(2));

            // All animal enumerables are empty!!!
            Assert.IsEmpty(farm.Animals);
            Assert.IsEmpty(farm.AnimalsArray);
            Assert.IsEmpty(farm.AnimalsEnumerable);
        }

        [Test]
        public void InitEnumerableComponentsWithTypes_ShouldFillFieldInRecursiveComponentInjected()
        {
            // Arrange
            ParentFarm parentFarm = InjectableBuilder<ParentFarm>.Create().Build();
            TestInjecter injecter = new TestInjecter(parentFarm);
            injecter.CheckAndInjectAll();

            // Throws because nothing at this level
            Assert.Throws<TestInjecterException>(() => injecter.InitEnumerableComponentsWithTypes<Cat>(typeof(Cat), typeof(Cat)));

            // Get farm
            Farm farm = injecter.GetComponent<Farm>();

            // Init and returned
            IEnumerable<Cat> cats = injecter.InitEnumerableComponentsWithTypes<Cat>(farm, typeof(Cat), typeof(Cat));

            // Check all cats fields
            Assert.That(cats.Count(), Is.EqualTo(2));
            Assert.That(farm.Cats.Count, Is.EqualTo(2));
            Assert.That(farm.CatsArray.Length, Is.EqualTo(2));
            Assert.That(farm.CatsEnumerable.Count(), Is.EqualTo(2));

            // All animal enumerables are empty!!!
            Assert.IsEmpty(farm.Animals);
            Assert.IsEmpty(farm.AnimalsArray);
            Assert.IsEmpty(farm.AnimalsEnumerable);
        }

        [Test]
        public void InitEnumerableComponents_ShouldFillFieldInRecursiveComponentInjected()
        {
            // Arrange
            ParentFarm parentFarm = InjectableBuilder<ParentFarm>.Create().Build();
            TestInjecter injecter = new TestInjecter(parentFarm);
            injecter.CheckAndInjectAll();

            // Throws because nothing at this level
            Assert.Throws<TestInjecterException>(() => injecter.InitEnumerableComponents<Cat>(2));

            // Get farm
            Farm farm = injecter.GetComponent<Farm>();

            // Init and returned
            IEnumerable<Cat> cats = injecter.InitEnumerableComponents<Cat>(2, farm);

            // Check all cats fields
            Assert.That(cats.Count(), Is.EqualTo(2));
            Assert.That(farm.Cats.Count, Is.EqualTo(2));
            Assert.That(farm.CatsArray.Length, Is.EqualTo(2));
            Assert.That(farm.CatsEnumerable.Count(), Is.EqualTo(2));

            // All animal enumerables are empty!!!
            Assert.IsEmpty(farm.Animals);
            Assert.IsEmpty(farm.AnimalsArray);
            Assert.IsEmpty(farm.AnimalsEnumerable);
        }
        #endregion GetComponentList

        #region GetComponentList Children, current and parents
        [Test]
        public void FullBasketGetComponentExample_ShouldFillOnlyParents()
        {
            // Arrange
            FullBasketListExample fullBasket = InjectableBuilder<FullBasketListExample>.Create().Build();
            TestInjecter injecter = new TestInjecter(fullBasket);
            injecter.CheckAndInjectAll();

            // Add in list (build component)
            IEnumerable<Fruit> fruitsInjected = injecter.InitEnumerableComponents<Fruit>(GameObjectLevel.Parent, 2);
            Fruit first = fruitsInjected.First();
            Fruit second = fruitsInjected.Skip(1).First();

            // Link to interface
            List<IFruit> fruits = new List<IFruit> { first, second };
            injecter.InjectField(fruits, "IFruitsListParent");
            injecter.InjectField(fruits.AsEnumerable(), "IFruitsEnumerableParent");

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
            IEnumerable<Fruit> fruitsEnumerableGetted = injecter.GetEnumerableComponents<Fruit>("FruitsEnumerableParent");
            foreach (Fruit fruit in fruitsEnumerableGetted)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(fullBasket.transform.GetInstanceID()));
            }

            // Check get component List
            IEnumerable<Fruit> fruitsListGetted = injecter.GetEnumerableComponents<Fruit>("FruitsListParent");
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
            TestInjecter injecter = new TestInjecter(fullBasket);
            injecter.CheckAndInjectAll();

            // Add in list (build component)
            IEnumerable<Fruit> fruitsInjected = injecter.InitEnumerableComponents<Fruit>(2);
            Fruit first = fruitsInjected.First();
            Fruit second = fruitsInjected.Skip(1).First();

            // Link to interface
            List<IFruit> fruits = new List<IFruit> { first, second };
            injecter.InjectField(fruits, "IFruitsList");
            injecter.InjectField(fruits.AsEnumerable(), "IFruitsEnumerable");

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
            IEnumerable<Fruit> fruitsEnumerableGetted = injecter.GetEnumerableComponents<Fruit>("FruitsEnumerable");
            foreach (Fruit fruit in fruitsEnumerableGetted)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.EqualTo(fullBasket.transform.GetInstanceID()));
            }

            // Check get component List
            IEnumerable<Fruit> fruitsListGetted = injecter.GetEnumerableComponents<Fruit>("FruitsList");
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
            TestInjecter injecter = new TestInjecter(fullBasket);
            injecter.CheckAndInjectAll();

            // Add in list (build component)
            IEnumerable<Fruit> fruitsInjected = injecter.InitEnumerableComponents<Fruit>(GameObjectLevel.Children, 2, fullBasket);
            Fruit first = fruitsInjected.First();
            Fruit second = fruitsInjected.Skip(1).First();

            // Link to interface
            List<IFruit> fruits = new List<IFruit> { first, second };
            injecter.InjectField(fruits, "IFruitsListChildren");
            injecter.InjectField(fruits.AsEnumerable(), "IFruitsEnumerableChildren");

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
            IEnumerable<Fruit> fruitsEnumerableGetted = injecter.GetEnumerableComponents<Fruit>("FruitsEnumerableChildren");
            foreach (Fruit fruit in fruitsEnumerableGetted)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(fullBasket.transform.GetInstanceID()));
            }

            // Check get component List
            IEnumerable<Fruit> fruitsListGetted = injecter.GetEnumerableComponents<Fruit>("FruitsListChildren");
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
            TestInjecter injecter = new TestInjecter(aboveFullBasket);
            injecter.CheckAndInjectAll();

            // Get child component
            FullBasketListExample fullBasket = injecter.GetComponent<FullBasketListExample>();

            // Add in list (build component)
            IEnumerable<Fruit> fruitsInjected = injecter.InitEnumerableComponents<Fruit>(GameObjectLevel.Parent, 2, fullBasket);
            Fruit first = fruitsInjected.First();
            Fruit second = fruitsInjected.Skip(1).First();

            // Link to interface
            List<IFruit> fruits = new List<IFruit> { first, second };
            injecter.InjectField(fruits, "IFruitsListParent", fullBasket);
            injecter.InjectField(fruits.AsEnumerable(), "IFruitsEnumerableParent", fullBasket);

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
            IEnumerable<Fruit> fruitsEnumerableGetted = injecter.GetEnumerableComponents<Fruit>("FruitsEnumerableParent", fullBasket);
            foreach (Fruit fruit in fruitsEnumerableGetted)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.transform.GetInstanceID()));
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(fullBasket.transform.GetInstanceID()));
            }

            // Check get component List
            IEnumerable<Fruit> fruitsListGetted = injecter.GetEnumerableComponents<Fruit>("FruitsListParent", fullBasket);
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
            TestInjecter injecter = new TestInjecter(aboveFullBasket);
            injecter.CheckAndInjectAll();

            // Get child component
            FullBasketListExample fullBasket = injecter.GetComponent<FullBasketListExample>();

            // Add in list (build component)
            IEnumerable<Fruit> fruitsInjected = injecter.InitEnumerableComponents<Fruit>(2, fullBasket);
            Fruit first = fruitsInjected.First();
            Fruit second = fruitsInjected.Skip(1).First();

            // Link to interface
            List<IFruit> fruits = new List<IFruit> { first, second };
            injecter.InjectField(fruits, "IFruitsList", fullBasket);
            injecter.InjectField(fruits.AsEnumerable(), "IFruitsEnumerable", fullBasket);

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
            IEnumerable<Fruit> fruitsEnumerableGetted = injecter.GetEnumerableComponents<Fruit>("FruitsEnumerable", fullBasket);
            foreach (Fruit fruit in fruitsEnumerableGetted)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.transform.GetInstanceID()));
                Assert.That(fruit.gameObject.GetInstanceID(), Is.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.EqualTo(fullBasket.transform.GetInstanceID()));
            }

            // Check get component List
            IEnumerable<Fruit> fruitsListGetted = injecter.GetEnumerableComponents<Fruit>("FruitsList", fullBasket);
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
            TestInjecter injecter = new TestInjecter(aboveFullBasket);
            injecter.CheckAndInjectAll();

            // Get child component
            FullBasketListExample fullBasket = injecter.GetComponent<FullBasketListExample>();

            // Add in list (build component)
            IEnumerable<Fruit> fruitsInjected = injecter.InitEnumerableComponents<Fruit>(GameObjectLevel.Children, 2, fullBasket);
            Fruit first = fruitsInjected.First();
            Fruit second = fruitsInjected.Skip(1).First();

            // Link to interface
            List<IFruit> fruits = new List<IFruit> { first, second };
            injecter.InjectField(fruits, "IFruitsListChildren", fullBasket);
            injecter.InjectField(fruits.AsEnumerable(), "IFruitsEnumerableChildren", fullBasket);

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
            IEnumerable<Fruit> fruitsEnumerableGetted = injecter.GetEnumerableComponents<Fruit>("FruitsEnumerableChildren", fullBasket);
            foreach (Fruit fruit in fruitsEnumerableGetted)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(aboveFullBasket.transform.GetInstanceID()));
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(fullBasket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(fullBasket.transform.GetInstanceID()));
            }

            // Check get component List
            IEnumerable<Fruit> fruitsListGetted = injecter.GetEnumerableComponents<Fruit>("FruitsListChildren", fullBasket);
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
            // and his parent must be an empty gameObject

            Orphan orphan = InjectableBuilder<Orphan>.Create().Build();
            TestInjecter injecter = new TestInjecter(orphan);
            injecter.CheckAndInjectAll();

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
        public void TwoRootComponent_WithSameRootNameAndSameSubComponentName_ButDifferentType_ShouldHaveSameGameObject()
        {
            Bouquet bouquet = InjectableBuilder<Bouquet>.Create().Build();
            TestInjecter injecter = new TestInjecter(bouquet);
            injecter.CheckAndInjectAll();

            DualFlower subRootIsolatedFlower = injecter.GetComponent<DualFlower>("SubRootIsolatedFlower");
            Image image = injecter.GetComponent<Image>();

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
            TestInjecter injecter = new TestInjecter(bouquet);
            injecter.CheckAndInjectAll();

            DualFlower referentialFlower = injecter.GetComponent<DualFlower>("ReferentialFlower");

            // Simplify referential accesses
            Flower flowerFieldOrigin = referentialFlower.FlowerField;
            Flower perfectFlowerOrigin = referentialFlower.PerfectFlower;

            // Initial checks
            Assert.NotNull(flowerFieldOrigin);
            Assert.NotNull(perfectFlowerOrigin);
            Assert.That(referentialFlower.PerfectFlower.transform.parent.GetInstanceID(), Is.EqualTo(referentialFlower.FlowerField.transform.GetInstanceID()));

            // Check every cases
            DualFlower flowerToCheck = injecter.GetComponent<DualFlower>(fieldNameToFind);
            Assert.That(flowerToCheck.FlowerField.GetInstanceID(), Is.EqualTo(flowerFieldOrigin.GetInstanceID()));
            Assert.That(flowerToCheck.PerfectFlower.GetInstanceID(), Is.EqualTo(perfectFlowerOrigin.GetInstanceID()));
            Assert.That(flowerToCheck.PerfectFlower.gameObject.GetInstanceID(), Is.EqualTo(perfectFlowerOrigin.gameObject.GetInstanceID()));
            Assert.That(flowerToCheck.PerfectFlower.transform.parent.GetInstanceID(), Is.EqualTo(flowerToCheck.FlowerField.transform.GetInstanceID()));
        }

        [Test]
        public void AllRootComponent_ShouldBeSame_ForEveryComponentInjectionCases_WithAddingInList()
        {
            Bouquet bouquet = InjectableBuilder<Bouquet>.Create().Build();
            TestInjecter injecter = new TestInjecter(bouquet);
            injecter.CheckAndInjectAll();

            DualFlower referentialFlower = injecter.GetComponent<DualFlower>("ReferentialFlower");

            // Simplify referential accesses
            Flower flowerFieldOrigin = referentialFlower.FlowerField;
            Flower perfectFlowerOrigin = referentialFlower.PerfectFlower;

            // Initial checks
            Assert.NotNull(flowerFieldOrigin);
            Assert.NotNull(perfectFlowerOrigin);
            Assert.That(referentialFlower.PerfectFlower.transform.parent.GetInstanceID(), Is.EqualTo(referentialFlower.FlowerField.transform.GetInstanceID()));

            // Add 2 in list
            IEnumerable<DualFlower> flowersInjected = injecter.InitEnumerableComponents<DualFlower>(2);
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
            TestInjecter injecter = new TestInjecter(plagueBouquet);
            injecter.CheckAndInjectAll();

            // Prepare
            Flower healthyFlower = injecter.GetComponent<Flower>("HealthyFlower");
            Flower subHealthyFlower = injecter.GetComponent<Flower>("SubHealthyFlower");
            PlagueFlower plagueFlower = injecter.GetComponent<PlagueFlower>("PlagueFlower");
            PlagueFlower subPlagueFlower = injecter.GetComponent<PlagueFlower>("SubPlagueFlower");            

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
