using Assets.ScriptExample.Cargos;
using Assets.ScriptExample.ComponentsWithEnumerable;
using Assets.ScriptExample.PlayerGroups;
using Moq;
using NixiTestTools;
using NUnit.Framework;
using ScriptExample.Characters;
using ScriptExample.Characters.ScriptableObjects;
using ScriptExample.Players;
using System.Collections.Generic;
using System.Linq;
using Tests.Builders;
using UnityEngine;

namespace Tests.TestTools
{
    internal sealed class TestInjecterComponentEnumerableTests
    {
        #region Enumerable Injections
        [Test]
        public void BasketInterfaceList_ShouldBeFilled_FromListInterfaceInjection_WithoutFieldName()
        {
            // Arrange
            SimpleBasket basket = BasketBuilder.Create().BuildSimple();

            List<IFruit> fruitsList = new List<IFruit>();
            Mock<IFruit> firstFruitMock = new Mock<IFruit>(MockBehavior.Strict);
            Mock<IFruit> secondFruitMock = new Mock<IFruit>(MockBehavior.Strict);

            firstFruitMock.Setup(x => x.Name).Returns("apple");
            secondFruitMock.Setup(x => x.Name).Returns("lemon");

            fruitsList.Add(firstFruitMock.Object);
            fruitsList.Add(secondFruitMock.Object);

            Assert.That(basket.IFruitsList, Is.Null);
            Assert.That(basket.IFruitsEnumerable, Is.Null);

            // Act
            TestInjecter injecter = new TestInjecter(basket);
            injecter.CheckAndInjectAll();
            injecter.InjectMock(fruitsList);

            Assert.That(basket.IFruitsList, Is.Not.Null);
            Assert.That(basket.IFruitsEnumerable, Is.Null);
            Assert.That(basket.IFruitsList.Any(x => x.Name == "apple"));
            Assert.That(basket.IFruitsList.Any(x => x.Name == "lemon"));

            firstFruitMock.Verify(x => x.Name, Times.AtLeastOnce);
            secondFruitMock.Verify(x => x.Name, Times.AtLeastOnce);
        }

        [Test]
        public void BasketInterfaceEnumerable_ShouldBeFilled_FromListInterfaceInjection_WithoutFieldName()
        {
            // Arrange
            SimpleBasket basket = BasketBuilder.Create().BuildSimple();

            List<IFruit> fruitsList = new List<IFruit>();
            Mock<IFruit> firstFruitMock = new Mock<IFruit>(MockBehavior.Strict);
            Mock<IFruit> secondFruitMock = new Mock<IFruit>(MockBehavior.Strict);

            firstFruitMock.Setup(x => x.Name).Returns("apple");
            secondFruitMock.Setup(x => x.Name).Returns("lemon");

            fruitsList.Add(firstFruitMock.Object);
            fruitsList.Add(secondFruitMock.Object);

            IEnumerable<IFruit> fruitsEnumerable = fruitsList;

            Assert.That(basket.IFruitsList, Is.Null);
            Assert.That(basket.IFruitsEnumerable, Is.Null);

            // Act
            TestInjecter injecter = new TestInjecter(basket);
            injecter.CheckAndInjectAll();
            injecter.InjectMock(fruitsEnumerable);

            Assert.That(basket.IFruitsEnumerable, Is.Not.Null);
            Assert.That(basket.IFruitsList, Is.Null);
            Assert.That(basket.IFruitsEnumerable.Any(x => x.Name == "apple"));
            Assert.That(basket.IFruitsEnumerable.Any(x => x.Name == "lemon"));

            firstFruitMock.Verify(x => x.Name, Times.AtLeastOnce);
            secondFruitMock.Verify(x => x.Name, Times.AtLeastOnce);
        }

        [Test]
        public void BasketWithTwoInterfaceList_ShouldThrowException_FromListInterfaceInjection_WithoutName()
        {
            // Arrange
            BasketDualList basketDual = BasketBuilder.Create().BuildDualList();

            List<IFruit> fruitsList = new List<IFruit>();
            Mock<IFruit> firstFruitMock = new Mock<IFruit>(MockBehavior.Strict);
            Mock<IFruit> secondFruitMock = new Mock<IFruit>(MockBehavior.Strict);

            firstFruitMock.Setup(x => x.Name).Returns("apple");
            secondFruitMock.Setup(x => x.Name).Returns("lemon");

            fruitsList.Add(firstFruitMock.Object);
            fruitsList.Add(secondFruitMock.Object);

            // Act
            TestInjecter injecter = new TestInjecter(basketDual);
            injecter.CheckAndInjectAll();
            Assert.Throws<TestInjecterException>(() => injecter.InjectMock(fruitsList));
        }

        [Test]
        public void BasketWithTwoInterfaceList_ShouldBeFilled_FromListInterfaceInjection_WithName()
        {
            // Arrange
            BasketDualList basketDual = BasketBuilder.Create().BuildDualList();

            List<IFruit> fruitsList = new List<IFruit>();
            Mock<IFruit> firstFruitMock = new Mock<IFruit>(MockBehavior.Strict);
            Mock<IFruit> secondFruitMock = new Mock<IFruit>(MockBehavior.Strict);

            firstFruitMock.Setup(x => x.Name).Returns("apple");
            secondFruitMock.Setup(x => x.Name).Returns("lemon");

            fruitsList.Add(firstFruitMock.Object);
            fruitsList.Add(secondFruitMock.Object);

            Assert.That(basketDual.FirstFruitsList, Is.Null);
            Assert.That(basketDual.SecondFruitsList, Is.Null);

            // Act
            TestInjecter injecter = new TestInjecter(basketDual);
            injecter.CheckAndInjectAll();
            injecter.InjectMock(fruitsList, "firstFruitsList");

            // Assert
            Assert.That(basketDual.FirstFruitsList, Is.Not.Null);
            Assert.That(basketDual.SecondFruitsList, Is.Null);
            Assert.That(basketDual.FirstFruitsList.Any(x => x.Name == "apple"));
            Assert.That(basketDual.FirstFruitsList.Any(x => x.Name == "lemon"));

            firstFruitMock.Verify(x => x.Name, Times.AtLeastOnce);
            secondFruitMock.Verify(x => x.Name, Times.AtLeastOnce);
        }

        [Test]
        public void TonsOfBasketWithNameAtBothLevels_ShouldBeFilled_FromListInterfaceInjection()
        {
            TonsOfBasket tonsOfBasket = BasketBuilder.Create().BuildTonsOfBasket();

            // Prepare
            TestInjecter injecter = new TestInjecter(tonsOfBasket);
            injecter.CheckAndInjectAll();

            // Asserts fields filled
            Assert.That(tonsOfBasket.FirstDualList, Is.Not.Null);
            Assert.That(tonsOfBasket.FirstDualList.FirstFruitsList, Is.Null);
            Assert.That(tonsOfBasket.FirstDualList.SecondFruitsList, Is.Null);

            Assert.That(tonsOfBasket.SecondDualList, Is.Not.Null);
            Assert.That(tonsOfBasket.SecondDualList.FirstFruitsList, Is.Null);
            Assert.That(tonsOfBasket.SecondDualList.SecondFruitsList, Is.Null);

            // Arrange
            List<IFruit> fruitsList = new List<IFruit>();
            Mock<IFruit> firstFruitMock = new Mock<IFruit>(MockBehavior.Strict);
            Mock<IFruit> secondFruitMock = new Mock<IFruit>(MockBehavior.Strict);

            firstFruitMock.Setup(x => x.Name).Returns("apple");
            secondFruitMock.Setup(x => x.Name).Returns("lemon");

            fruitsList.Add(firstFruitMock.Object);
            fruitsList.Add(secondFruitMock.Object);

            // Act
            BasketDualList secondDualList = injecter.GetComponent<BasketDualList>("secondDualList");
            injecter.InjectMock(fruitsList, "secondFruitsList", secondDualList);

            // Asserts
            Assert.That(tonsOfBasket.FirstDualList, Is.Not.Null);
            Assert.That(tonsOfBasket.FirstDualList.FirstFruitsList, Is.Null);
            Assert.That(tonsOfBasket.FirstDualList.SecondFruitsList, Is.Null);

            Assert.That(tonsOfBasket.SecondDualList, Is.Not.Null);
            Assert.That(tonsOfBasket.SecondDualList.FirstFruitsList, Is.Null);
            Assert.That(tonsOfBasket.SecondDualList.SecondFruitsList, Is.Not.Null);

            Assert.That(tonsOfBasket.SecondDualList.SecondFruitsList.Any(x => x.Name == "apple"));
            Assert.That(tonsOfBasket.SecondDualList.SecondFruitsList.Any(x => x.Name == "lemon"));

            firstFruitMock.Verify(x => x.Name, Times.AtLeastOnce);
            secondFruitMock.Verify(x => x.Name, Times.AtLeastOnce);
        }

        [Test]
        public void BasketComponent_ShouldReturnEmptyList()
        {
            SimpleBasketComponent basketCompo = BasketBuilder.Create().BuildSimpleBasketComponent();

            Assert.Null(basketCompo.FruitsList);

            // Arrange + Act
            TestInjecter injecter = new TestInjecter(basketCompo);
            injecter.CheckAndInjectAll();
            IEnumerable<Fruit> fruits = injecter.GetComponentList<Fruit>();

            // Asserts
            Assert.NotNull(basketCompo.FruitsList);
            Assert.IsEmpty(basketCompo.FruitsList);
            Assert.IsEmpty(fruits);
        }

        [Test]
        public void BasketComponent_ShouldReturnFillList()
        {
            SimpleBasketComponent basketCompo = BasketBuilder.Create().BuildSimpleBasketComponent();

            Assert.Null(basketCompo.FruitsList);

            // Arrange
            TestInjecter injecter = new TestInjecter(basketCompo);
            injecter.CheckAndInjectAll();

            Assert.NotNull(basketCompo.FruitsList);

            // Act
            Fruit newFruit = injecter.AddInComponentList<Fruit>();
            Fruit secondNewFruit = injecter.AddInComponentList<Fruit>();

            // Asserts on basketCompo
            Assert.That(basketCompo.FruitsList.Count, Is.EqualTo(2));
            Assert.That(basketCompo.FruitsList.Any(x => x.gameObject.GetInstanceID() == newFruit.gameObject.GetInstanceID()));
            Assert.That(basketCompo.FruitsList.Any(x => x.gameObject.GetInstanceID() == secondNewFruit.gameObject.GetInstanceID()));
            Assert.That(basketCompo.FruitsList.Any(x => x.GetInstanceID() == newFruit.GetInstanceID()));
            Assert.That(basketCompo.FruitsList.Any(x => x.GetInstanceID() == secondNewFruit.GetInstanceID()));

            // Asserts on GetComponentList
            IEnumerable<Fruit> fruits = injecter.GetComponentList<Fruit>();
            Assert.That(fruits.Count(), Is.EqualTo(2));
            Assert.That(fruits.Any(x => x.gameObject.GetInstanceID() == newFruit.gameObject.GetInstanceID()));
            Assert.That(fruits.Any(x => x.gameObject.GetInstanceID() == secondNewFruit.gameObject.GetInstanceID()));
            Assert.That(fruits.Any(x => x.GetInstanceID() == newFruit.GetInstanceID()));
            Assert.That(fruits.Any(x => x.GetInstanceID() == secondNewFruit.GetInstanceID()));
        }

        [Test]
        public void DualBasketComponent_ShouldReturnComponentsWithName_WhenGetComponentListOnTwoIdenticalTypeWithName()
        {
            DualBasketComponent dualBasketCompo = BasketBuilder.Create().BuildDualBasketComponent();

            Assert.Null(dualBasketCompo.FruitsList);

            // Arrange + Act
            TestInjecter injecter = new TestInjecter(dualBasketCompo);
            injecter.CheckAndInjectAll();

            // Get both
            IEnumerable<Fruit> fruitsList = injecter.GetComponentList<Fruit>();

            // Asserts
            Assert.NotNull(dualBasketCompo.FruitsList);
            Assert.NotNull(dualBasketCompo.FruitsEnumerable);
            Assert.IsEmpty(dualBasketCompo.FruitsList);
            Assert.IsEmpty(dualBasketCompo.FruitsEnumerable);
            Assert.IsEmpty(fruitsList);
        }

        [Test]
        public void DualBasketComponent_ShouldFillListForBothWithName()
        {
            DualBasketComponent dualBasketCompo = BasketBuilder.Create().BuildDualBasketComponent();

            Assert.Null(dualBasketCompo.FruitsList);
            Assert.Null(dualBasketCompo.FruitsEnumerable);

            // Arrange
            TestInjecter injecter = new TestInjecter(dualBasketCompo);
            injecter.CheckAndInjectAll();

            Assert.NotNull(dualBasketCompo.FruitsList);
            Assert.NotNull(dualBasketCompo.FruitsEnumerable);

            // Act
            Fruit newFruit = injecter.AddInComponentList<Fruit>();
            Fruit secondNewFruit = injecter.AddInComponentList<Fruit>();

            // Asserts on dualBasketCompo.FruitsList directly
            Assert.That(dualBasketCompo.FruitsList.Count, Is.EqualTo(2));
            Assert.That(dualBasketCompo.FruitsList.Any(x => x.gameObject.GetInstanceID() == newFruit.gameObject.GetInstanceID()));
            Assert.That(dualBasketCompo.FruitsList.Any(x => x.gameObject.GetInstanceID() == secondNewFruit.gameObject.GetInstanceID()));
            Assert.That(dualBasketCompo.FruitsList.Any(x => x.GetInstanceID() == newFruit.GetInstanceID()));
            Assert.That(dualBasketCompo.FruitsList.Any(x => x.GetInstanceID() == secondNewFruit.GetInstanceID()));

            // Asserts on dualBasketCompo.FruitsEnumerable directly
            Assert.That(dualBasketCompo.FruitsEnumerable.Count, Is.EqualTo(2));
            Assert.That(dualBasketCompo.FruitsEnumerable.Any(x => x.gameObject.GetInstanceID() == newFruit.gameObject.GetInstanceID()));
            Assert.That(dualBasketCompo.FruitsEnumerable.Any(x => x.gameObject.GetInstanceID() == secondNewFruit.gameObject.GetInstanceID()));
            Assert.That(dualBasketCompo.FruitsEnumerable.Any(x => x.GetInstanceID() == newFruit.GetInstanceID()));
            Assert.That(dualBasketCompo.FruitsEnumerable.Any(x => x.GetInstanceID() == secondNewFruit.GetInstanceID()));

            // Asserts on get (same for both, because same enumerable type on children)
            IEnumerable<Fruit> fruitsList = injecter.GetComponentList<Fruit>();
            Assert.That(fruitsList.Count(), Is.EqualTo(2));
            Assert.That(fruitsList.Any(x => x.gameObject.GetInstanceID() == newFruit.gameObject.GetInstanceID()));
            Assert.That(fruitsList.Any(x => x.gameObject.GetInstanceID() == secondNewFruit.gameObject.GetInstanceID()));
            Assert.That(fruitsList.Any(x => x.GetInstanceID() == newFruit.GetInstanceID()));
            Assert.That(fruitsList.Any(x => x.GetInstanceID() == secondNewFruit.GetInstanceID()));
        }

        [Test]
        public void PlayerGroupWithSubsequentLevel_ShouldFillRecursively()
        {
            PlayerGroup playerGroup = PlayerGroupBuilder.Create().Build();
            Assert.Null(playerGroup.Players);

            // Prepare
            TestInjecter injecter = new TestInjecter(playerGroup);
            injecter.CheckAndInjectAll();

            // Asserts fields filled
            Assert.IsNotNull(playerGroup.Players);
            Assert.IsEmpty(playerGroup.Players);

            Player firstPlayer = injecter.AddInComponentList<Player>();
            Player secondPlayer = injecter.AddInComponentList<Player>();

            // check player in
            Assert.That(playerGroup.Players.Count, Is.EqualTo(2));
            Assert.That(playerGroup.Players.Any(x => x.GetInstanceID() == firstPlayer.GetInstanceID()));
            Assert.That(playerGroup.Players.Any(x => x.GetInstanceID() == secondPlayer.GetInstanceID()));

            // get last component level and check this is the same from the top
            Player firstPlayerRetrieved = playerGroup.Players.Single(x => x.GetInstanceID() == firstPlayer.GetInstanceID());
            Skill attackSkillRetrieved = firstPlayerRetrieved.Sorcerer.AttackSkill;

            Sorcerer firstSorcerer = injecter.GetComponent<Sorcerer>(firstPlayer);
            Assert.That(firstPlayerRetrieved.Sorcerer.GetInstanceID(), Is.EqualTo(firstSorcerer.GetInstanceID()));

            Skill attackSkill = injecter.GetComponent<Skill>("attackSkill", firstSorcerer);
            Assert.That(attackSkillRetrieved.GetInstanceID(), Is.EqualTo(attackSkill.GetInstanceID()));

            // injectMock to sorcerer level
            SO_InventoryBag inventoryBag = ScriptableObject.CreateInstance<SO_InventoryBag>();
            inventoryBag.BagName = "Baggy";
            injecter.InjectMock(inventoryBag, "secondInventoryBagInfos", firstSorcerer);

            Assert.That(firstSorcerer.SecondInventoryBagInfos.GetInstanceID(), Is.EqualTo(inventoryBag.GetInstanceID()));
            Assert.That(firstSorcerer.SecondInventoryBagInfos.BagName, Is.EqualTo(inventoryBag.BagName));

            // check from the top, values are correctly filled
            Assert.That(firstPlayerRetrieved.Sorcerer.SecondInventoryBagInfos.GetInstanceID(), Is.EqualTo(inventoryBag.GetInstanceID()));
            Assert.That(firstPlayerRetrieved.Sorcerer.SecondInventoryBagInfos.BagName, Is.EqualTo(inventoryBag.BagName));
        }

        [Test]
        public void PlayerGroupWithNameOnSameTypeAndSubsequentLevel_ShouldFillRecursively_WithName()
        {
            PlayerGroupWithName playerGroupWithName = PlayerGroupBuilder.Create().BuildWithName();
            Assert.Null(playerGroupWithName.Players);
            Assert.Null(playerGroupWithName.SecondPlayers);

            // Prepare
            TestInjecter injecter = new TestInjecter(playerGroupWithName);
            injecter.CheckAndInjectAll();

            // Asserts fields filled
            Assert.IsNotNull(playerGroupWithName.Players);
            Assert.IsEmpty(playerGroupWithName.Players);
            Assert.IsNotNull(playerGroupWithName.SecondPlayers);
            Assert.IsEmpty(playerGroupWithName.SecondPlayers);

            Player firstPlayer = injecter.AddInComponentList<Player>();
            Player secondPlayer = injecter.AddInComponentList<Player>();

            // check player in
            Assert.That(playerGroupWithName.Players.Count, Is.EqualTo(2));
            Assert.That(playerGroupWithName.Players.Any(x => x.GetInstanceID() == firstPlayer.GetInstanceID()));
            Assert.That(playerGroupWithName.Players.Any(x => x.GetInstanceID() == secondPlayer.GetInstanceID()));
            Assert.That(playerGroupWithName.SecondPlayers.Count, Is.EqualTo(2));
            Assert.That(playerGroupWithName.SecondPlayers.Any(x => x.GetInstanceID() == firstPlayer.GetInstanceID()));
            Assert.That(playerGroupWithName.SecondPlayers.Any(x => x.GetInstanceID() == secondPlayer.GetInstanceID()));

            // get last component level and check this is the same from the top
            Player firstPlayerRetrieved = playerGroupWithName.SecondPlayers.Single(x => x.GetInstanceID() == firstPlayer.GetInstanceID());
            Skill attackSkillRetrieved = firstPlayerRetrieved.Sorcerer.AttackSkill;

            Sorcerer firstSorcerer = injecter.GetComponent<Sorcerer>(firstPlayer);
            Assert.That(firstPlayerRetrieved.Sorcerer.GetInstanceID(), Is.EqualTo(firstSorcerer.GetInstanceID()));

            Skill attackSkill = injecter.GetComponent<Skill>("attackSkill", firstSorcerer);
            Assert.That(attackSkillRetrieved.GetInstanceID(), Is.EqualTo(attackSkill.GetInstanceID()));

            // injectMock to sorcerer level
            SO_InventoryBag inventoryBag = ScriptableObject.CreateInstance<SO_InventoryBag>();
            inventoryBag.BagName = "Baggy";
            injecter.InjectMock(inventoryBag, "secondInventoryBagInfos", firstSorcerer);

            Assert.That(firstSorcerer.SecondInventoryBagInfos.GetInstanceID(), Is.EqualTo(inventoryBag.GetInstanceID()));
            Assert.That(firstSorcerer.SecondInventoryBagInfos.BagName, Is.EqualTo(inventoryBag.BagName));

            // check from the top, values are correctly filled
            Assert.That(firstPlayerRetrieved.Sorcerer.SecondInventoryBagInfos.GetInstanceID(), Is.EqualTo(inventoryBag.GetInstanceID()));
            Assert.That(firstPlayerRetrieved.Sorcerer.SecondInventoryBagInfos.BagName, Is.EqualTo(inventoryBag.BagName));
        }

        [Test]
        public void Cargo_AddInComponentListFromChildInjected_And_GetComponentListFromChildInjected_Correctly()
        {
            Cargo cargo = CargoBuilder.Create().Build();

            Assert.That(cargo.FirstContainers, Is.Empty);
            Assert.That(cargo.SecondContainers, Is.Empty);

            TestInjecter injecter = new TestInjecter(cargo);
            injecter.CheckAndInjectAll();

            // add two in secondContainer
            Container container = injecter.AddInComponentList<Container>();
            Container secondContainer = injecter.AddInComponentList<Container>();

            BananaPack bananaPack = injecter.AddInComponentList<BananaPack>(secondContainer);
            BananaPack secondBananaPack = injecter.AddInComponentList<BananaPack>(secondContainer);

            Assert.That(bananaPack.RainbowSummonSkill.GetInstanceID(), Is.EqualTo(secondBananaPack.RainbowSummonSkill.GetInstanceID()));

            int a = 0;

        }

        // MaClass contient des MonoBehaviourInjectable qui contiennent chacun des MonoBehaviourInjectable, dernier niveau auquel nous modifions une donnée
        // Puis on GetComponentListFromChildInjected

        //AddInComponentListFromChildInjected+GetComponentListFromChildInjected
        //AddInComponentListFromChildInjected bug without fieldname
        //GetComponentListFromChildInjected bug without fieldname
        //AddInComponentListFromChildInjected(fieldName)+GetComponentListFromChildInjected(fieldName)
        #endregion Enumerable Injections
    }
}
