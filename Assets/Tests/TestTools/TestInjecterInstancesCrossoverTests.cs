using Assets.ScriptExample.AllParentsCases;
using Assets.ScriptExample.Farms;
using Assets.ScriptExample.Flowers;
using NixiTestTools;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Tests.Builders;

namespace Tests.TestTools
{
    internal sealed class TestInjecterInstancesCrossoverTests
    {
        //OK 4) NixInjectComponent au même niveau
        //OK 1) NixInjectComponentList sur le même type doit renvoyer les mêmes instances d'en dessous
        //OK 2) AddComponentInList doit tout update s'il y a lieux d'être
        //OK 3) NixInjectRootComponent sur le même nom doit renvoyer la même instance
        //OK 4) NixInjectRootComponent sur le même nom et le même GameObjectName doit renvoyer la même instance en ayant le même parent et la même chose que pour 2)
        //5) NixInjectComponentFromMethod SameChildren, tout ceux en-dessous
        //6) NixInjectComponentFromMethod SameParents, tout ceux au-dessus
        //RAF car tu mock 7) Qu'en est-il des interface component ?
        //À dégager 8) Qu'en est-il des inactives ?

        #region GetComponent
        [Test]
        public void GetComponentOnSameType_AtSameLevel_ShouldReturnSameInstance()
        {
            // Arrange
            Parent parent = AllParentsBuilder.Create().BuildParent();
            TestInjecter injecter = new TestInjecter(parent);
            injecter.CheckAndInjectAll();

            // Check
            Assert.NotNull(parent.FirstChild);
            Assert.NotNull(parent.SecondChild);

            Assert.That(parent.FirstChild.GetInstanceID(), Is.EqualTo(parent.SecondChild.GetInstanceID()));
        }
        #endregion GetComponent

        #region GetComponentList
        [Test]
        public void AddAndGetComponentListOnTwoIdenticalList_ShouldReturnSameInstance()
        {
            // Arrange
            ParentWithSameChildLists parent = AllParentsBuilder.Create().BuildParentWithSameChildLists();
            TestInjecter injecter = new TestInjecter(parent);
            injecter.CheckAndInjectAll();

            // Check init
            Assert.NotNull(parent.FirstChildList);
            Assert.IsEmpty(parent.FirstChildList);
            Assert.NotNull(parent.SecondChildList);
            Assert.IsEmpty(parent.SecondChildList);

            // Add child in list (should impact both)
            Child newChild = injecter.AddInComponentList<Child>();

            // Check same list
            Assert.That(parent.FirstChildList.Count, Is.EqualTo(1));
            Assert.That(parent.FirstChildList[0].GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));

            Assert.That(parent.SecondChildList.Count, Is.EqualTo(1));
            Assert.That(parent.SecondChildList[0].GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));

            // Check from getter
            IEnumerable<Child> childsFromInjecter = injecter.GetComponentList<Child>();
            Assert.That(childsFromInjecter.Count, Is.EqualTo(1));
            Assert.That(childsFromInjecter.Single().GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));
        }

        [Test]
        public void AddAndGetComponentListOnTwoDifferentsEnumerables_ShouldReturnSameInstance()
        {
            // Arrange
            ParentWithSameChildListsDifferentsEnumerables parent = AllParentsBuilder.Create().BuildParentWithSameChildListsDifferentsEnumerables();
            TestInjecter injecter = new TestInjecter(parent);
            injecter.CheckAndInjectAll();

            // Check init
            Assert.NotNull(parent.FirstChildList);
            Assert.IsEmpty(parent.FirstChildList);
            Assert.NotNull(parent.SecondChildList);
            Assert.IsEmpty(parent.SecondChildList);

            // Add child in list (should impact both)
            Child newChild = injecter.AddInComponentList<Child>();

            // Check same list
            Assert.That(parent.FirstChildList.Count, Is.EqualTo(1));
            Assert.That(parent.FirstChildList[0].GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));

            Assert.That(parent.SecondChildList.Count(), Is.EqualTo(1));
            Assert.That(parent.SecondChildList.Single().GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));

            // Check from getter
            IEnumerable<Child> childsFromInjecter = injecter.GetComponentList<Child>();
            Assert.That(childsFromInjecter.Count, Is.EqualTo(1));
            Assert.That(childsFromInjecter.Single().GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));
        }

        [Test]
        public void AddAndGetComponentGrandParentWithChildListAndParent_ShouldReturnDifferentInstanceFromParent()
        {
            // Arrange
            GrandParentWithChildListAndParent grandParent = AllParentsBuilder.Create().BuildGrandParentWithChildListAndParent();
            TestInjecter injecter = new TestInjecter(grandParent);
            injecter.CheckAndInjectAll();

            // Check init
            Assert.NotNull(grandParent.FirstChildList);
            Assert.IsEmpty(grandParent.FirstChildList);
            Assert.NotNull(grandParent.ParentWithSameChildLists);
            Assert.IsEmpty(grandParent.ParentWithSameChildLists.FirstChildList);
            Assert.IsEmpty(grandParent.ParentWithSameChildLists.SecondChildList);

            // Add child in list (should impact both)
            Child newChild = injecter.AddInComponentList<Child>();

            // Check same list
            Assert.That(grandParent.FirstChildList.Count, Is.EqualTo(1));
            Assert.That(grandParent.FirstChildList[0].GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));

            Assert.IsEmpty(grandParent.ParentWithSameChildLists.FirstChildList);
            Assert.IsEmpty(grandParent.ParentWithSameChildLists.SecondChildList);

            // Check from getter
            IEnumerable<Child> grandParentChildsFromInjecter = injecter.GetComponentList<Child>();
            Assert.That(grandParentChildsFromInjecter.Count, Is.EqualTo(1));
            Assert.That(grandParentChildsFromInjecter.Single().GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));

            // Check from getter subchild lists empties
            ParentWithSameChildLists parentWithSameChildLists = injecter.GetComponent<ParentWithSameChildLists>();
            Assert.That(parentWithSameChildLists, Is.Not.Null);
            Assert.That(parentWithSameChildLists.FirstChildList, Is.Empty);
            Assert.That(parentWithSameChildLists.SecondChildList, Is.Empty);

            // Add child into subchild lists, should impact both, but not parent
            Child subChild = injecter.AddInComponentList<Child>(parentWithSameChildLists);
            Assert.That(parentWithSameChildLists.FirstChildList.Count, Is.EqualTo(1));
            Assert.That(parentWithSameChildLists.FirstChildList.Single().GetInstanceID(), Is.EqualTo(subChild.GetInstanceID()));
            Assert.That(parentWithSameChildLists.SecondChildList.Count, Is.EqualTo(1));
            Assert.That(parentWithSameChildLists.SecondChildList.Single().GetInstanceID(), Is.EqualTo(subChild.GetInstanceID()));

            Assert.That(grandParentChildsFromInjecter.Count, Is.EqualTo(1));
            Assert.That(grandParentChildsFromInjecter.Single().GetInstanceID(), Is.EqualTo(newChild.GetInstanceID()));
            Assert.That(grandParentChildsFromInjecter.Single().GetInstanceID(), Is.Not.EqualTo(subChild.GetInstanceID()));
        }

        [Test]
        public void AddAndGetComponentFarmWithListInjected_ShouldReturnDifferentValuesFromInheritance()
        {
            // Arrange
            Farm farm = CompoBuilder<Farm>.Create().Build();
            TestInjecter injecter = new TestInjecter(farm);
            injecter.CheckAndInjectAll();

            // Check init
            Assert.NotNull(farm.Animals);
            Assert.IsEmpty(farm.Animals);
            Assert.NotNull(farm.Cats);
            Assert.IsEmpty(farm.Cats);

            // Add child in list (should impact both)
            Animal firstAnimal = injecter.AddInComponentList<Animal>();
            Animal secondAnimal = injecter.AddInComponentList<Animal>();

            Cat firstCat = injecter.AddInComponentList<Cat>();
            Cat secondCat = injecter.AddInComponentList<Cat>();

            // Check same list
            Assert.That(farm.Animals.Count, Is.EqualTo(4));
            Assert.That(farm.Animals[0].GetInstanceID(), Is.EqualTo(firstAnimal.GetInstanceID()));
            Assert.That(farm.Animals[1].GetInstanceID(), Is.EqualTo(secondAnimal.GetInstanceID()));
            Assert.That(farm.Animals[2].GetInstanceID(), Is.EqualTo(firstCat.GetInstanceID()));
            Assert.That(farm.Animals[3].GetInstanceID(), Is.EqualTo(secondCat.GetInstanceID()));

            Assert.That(farm.Cats.Count, Is.EqualTo(2));
            Assert.That(farm.Cats[0].GetInstanceID(), Is.EqualTo(firstCat.GetInstanceID()));
            Assert.That(farm.Cats[1].GetInstanceID(), Is.EqualTo(secondCat.GetInstanceID()));

            // Check same enumerable
            Assert.That(farm.AnimalsEnumerable.Count, Is.EqualTo(4));
            Assert.That(farm.AnimalsEnumerable.First().GetInstanceID(), Is.EqualTo(firstAnimal.GetInstanceID()));
            Assert.That(farm.AnimalsEnumerable.Skip(1).First().GetInstanceID(), Is.EqualTo(secondAnimal.GetInstanceID()));
            Assert.That(farm.AnimalsEnumerable.Skip(2).First().GetInstanceID(), Is.EqualTo(firstCat.GetInstanceID()));
            Assert.That(farm.AnimalsEnumerable.Skip(3).First().GetInstanceID(), Is.EqualTo(secondCat.GetInstanceID()));

            Assert.That(farm.CatsEnumerable.Count, Is.EqualTo(2));
            Assert.That(farm.CatsEnumerable.First().GetInstanceID(), Is.EqualTo(firstCat.GetInstanceID()));
            Assert.That(farm.CatsEnumerable.Skip(1).First().GetInstanceID(), Is.EqualTo(secondCat.GetInstanceID()));

            // Check same array
            Assert.That(farm.AnimalsArray.Length, Is.EqualTo(4));
            Assert.That(farm.AnimalsArray[0].GetInstanceID(), Is.EqualTo(firstAnimal.GetInstanceID()));
            Assert.That(farm.AnimalsArray[1].GetInstanceID(), Is.EqualTo(secondAnimal.GetInstanceID()));
            Assert.That(farm.AnimalsArray[2].GetInstanceID(), Is.EqualTo(firstCat.GetInstanceID()));
            Assert.That(farm.AnimalsArray[3].GetInstanceID(), Is.EqualTo(secondCat.GetInstanceID()));

            Assert.That(farm.CatsArray.Length, Is.EqualTo(2));
            Assert.That(farm.CatsArray[0].GetInstanceID(), Is.EqualTo(firstCat.GetInstanceID()));
            Assert.That(farm.CatsArray[1].GetInstanceID(), Is.EqualTo(secondCat.GetInstanceID()));
        }
        #endregion GetComponentList

        #region RootComponent
        [TestCase("ChildFlower")]
        [TestCase("ParentFlower")]
        [TestCase("RootIsolatedFlower")]
        [TestCase("SubRootIsolatedFlower")]
        public void AllRootComponent_ShouldBeSame_ForEveryComponentInjectionCases(string fieldNameToFind)
        {
            Bouquet bouquet = CompoBuilder<Bouquet>.Create().Build();
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
            Bouquet bouquet = CompoBuilder<Bouquet>.Create().Build();
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
            DualFlower firstFlower = injecter.AddInComponentList<DualFlower>();
            DualFlower secondFlower = injecter.AddInComponentList<DualFlower>();

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
            PlagueBouquet plagueBouquet = CompoBuilder<PlagueBouquet>.Create().Build();
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
