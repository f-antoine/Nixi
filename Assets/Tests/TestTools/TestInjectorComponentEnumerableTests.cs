using Moq;
using Nixi.Injections;
using NixiTestTools;
using NUnit.Framework;
using ScriptExample.AllParentsCases;
using ScriptExample.Cargos;
using ScriptExample.Characters;
using ScriptExample.Characters.ScriptableObjects;
using ScriptExample.ComponentsWithEnumerable;
using ScriptExample.Fallen.AllComponentAttributes;
using ScriptExample.Fallen.Enumerables;
using ScriptExample.Farms;
using ScriptExample.PlayerGroups;
using ScriptExample.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using Tests.Builders;
using UnityEngine;

namespace Tests.TestTools
{
    internal sealed class TestInjectorComponentEnumerableTests
    {
        #region General
        [Test]
        public void BasketInterfaceList_ShouldBeFilled_FromListInterfaceInjection_WithoutFieldName()
        {
            // Arrange
            SimpleBasket basket = InjectableBuilder<SimpleBasket>.Create().Build();

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
            TestInjector Injector = new TestInjector(basket);
            Injector.CheckAndInjectAll();
            Injector.InjectField(fruitsList);

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
            SimpleBasket basket = InjectableBuilder<SimpleBasket>.Create().Build();

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
            TestInjector Injector = new TestInjector(basket);
            Injector.CheckAndInjectAll();
            Injector.InjectField(fruitsEnumerable);

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
            BasketDualList basketDual = InjectableBuilder<BasketDualList>.Create().Build();

            List<IFruit> fruitsList = new List<IFruit>();
            Mock<IFruit> firstFruitMock = new Mock<IFruit>(MockBehavior.Strict);
            Mock<IFruit> secondFruitMock = new Mock<IFruit>(MockBehavior.Strict);

            firstFruitMock.Setup(x => x.Name).Returns("apple");
            secondFruitMock.Setup(x => x.Name).Returns("lemon");

            fruitsList.Add(firstFruitMock.Object);
            fruitsList.Add(secondFruitMock.Object);

            // Act
            TestInjector Injector = new TestInjector(basketDual);
            Injector.CheckAndInjectAll();
            Assert.Throws<TestInjectorException>(() => Injector.InjectField(fruitsList));
        }

        [Test]
        public void BasketWithTwoInterfaceList_ShouldBeFilled_FromListInterfaceInjection_WithName()
        {
            // Arrange
            BasketDualList basketDual = InjectableBuilder<BasketDualList>.Create().Build();

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
            TestInjector Injector = new TestInjector(basketDual);
            Injector.CheckAndInjectAll();
            Injector.InjectField(fruitsList, "firstFruitsList");

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
            TonsOfBasket tonsOfBasket = InjectableBuilder<TonsOfBasket>.Create().Build();

            // Prepare
            TestInjector Injector = new TestInjector(tonsOfBasket);
            Injector.CheckAndInjectAll();

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
            BasketDualList secondDualList = Injector.GetComponent<BasketDualList>("secondDualList");
            Injector.InjectField(fruitsList, "secondFruitsList", secondDualList);

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
            SimpleBasketComponent basketCompo = InjectableBuilder<SimpleBasketComponent>.Create().Build();

            Assert.Null(basketCompo.FruitsList);

            // Arrange + Act
            TestInjector Injector = new TestInjector(basketCompo);
            Injector.CheckAndInjectAll();
            IEnumerable<Fruit> fruits = Injector.GetEnumerableComponents<Fruit>();

            // Asserts
            Assert.NotNull(basketCompo.FruitsList);
            Assert.IsEmpty(basketCompo.FruitsList);
            Assert.IsEmpty(fruits);
        }

        [Test]
        public void BasketComponent_ShouldReturnEmptyList_ThenAddCorrectlyFromAccessor()
        {
            // Arrange
            SimpleBasketComponent basketCompo = InjectableBuilder<SimpleBasketComponent>.Create().Build();
            TestInjector Injector = new TestInjector(basketCompo);
            Injector.CheckAndInjectAll();
            IEnumerable<Fruit> fruits = Injector.GetEnumerableComponents<Fruit>();

            // Check
            Assert.IsEmpty(fruits);
            Assert.IsEmpty(basketCompo.FruitsList);

            // Act
            basketCompo.FruitsList.Add(new GameObject().AddComponent<Fruit>());
            basketCompo.FruitsList.Add(new GameObject().AddComponent<Fruit>());
            basketCompo.FruitsList.Add(new GameObject().AddComponent<Fruit>());

            // Checks
            Assert.Throws<TestInjectorException>(() => Injector.InitEnumerableComponents<Fruit>(2));

            IEnumerable<Fruit> secondFruits = Injector.GetEnumerableComponents<Fruit>();
            Assert.IsNotEmpty(secondFruits);
            Assert.AreEqual(3, secondFruits.Count());
        }

        [Test]
        public void BasketComponent_ShouldReturnEmptyEnumerable_ThenAddCorrectlyFromAccessor()
        {
            // Arrange
            ParentWithSameChildListsDifferentsEnumerables mainTested = InjectableBuilder<ParentWithSameChildListsDifferentsEnumerables>.Create().Build();
            TestInjector Injector = new TestInjector(mainTested);
            Injector.CheckAndInjectAll();
            IEnumerable<Child> children = Injector.GetEnumerableComponents<Child>("SecondChildList");

            // Check
            Assert.IsEmpty(children);
            Assert.IsEmpty(mainTested.SecondChildList);

            // Act
            List<Child> childEnumerableToForce = new List<Child>
            {
                new GameObject().AddComponent<Child>(),
                new GameObject().AddComponent<Child>()
            };

            mainTested.SecondChildList = childEnumerableToForce;

            // Checks
            Assert.Throws<TestInjectorException>(() => Injector.InitEnumerableComponents<Child>(2));

            IEnumerable<Child> secondChildren = Injector.GetEnumerableComponents<Child>("SecondChildList");
            Assert.IsNotEmpty(secondChildren);
            Assert.AreEqual(2, secondChildren.Count());
        }

        [Test]
        public void BasketComponent_ShouldReturnEmptyArray_ThenAddCorrectlyFromAccessor()
        {
            // Arrange
            ParentWithSameChildListsDifferentsEnumerables mainTested = InjectableBuilder<ParentWithSameChildListsDifferentsEnumerables>.Create().Build();
            TestInjector Injector = new TestInjector(mainTested);
            Injector.CheckAndInjectAll();
            IEnumerable<Child> children = Injector.GetEnumerableComponents<Child>("ThirdChildArray");

            // Check
            Assert.IsEmpty(children);
            Assert.IsEmpty(mainTested.ThirdChildArray);

            // Act
            Child[] childArrayToForce = new Child[]
            {
                new GameObject().AddComponent<Child>(),
                new GameObject().AddComponent<Child>()
            };

            mainTested.ThirdChildArray = childArrayToForce;

            // Checks
            Assert.Throws<TestInjectorException>(() => Injector.InitEnumerableComponents<Child>(2));

            IEnumerable<Child> secondChildren = Injector.GetEnumerableComponents<Child>("ThirdChildArray");
            Assert.IsNotEmpty(secondChildren);
            Assert.AreEqual(2, secondChildren.Count());
        }

        [Test]
        public void BasketComponent_ShouldReturnFillList()
        {
            SimpleBasketComponent basketCompo = InjectableBuilder<SimpleBasketComponent>.Create().Build();

            Assert.Null(basketCompo.FruitsList);

            // Arrange
            TestInjector Injector = new TestInjector(basketCompo);
            Injector.CheckAndInjectAll();

            Assert.NotNull(basketCompo.FruitsList);

            // Act
            IEnumerable<Fruit> fruitsInjected = Injector.InitEnumerableComponents<Fruit>(2);
            Fruit newFruit = fruitsInjected.First();
            Fruit secondNewFruit = fruitsInjected.Skip(1).First();

            // Asserts on basketCompo
            Assert.That(basketCompo.FruitsList.Count, Is.EqualTo(2));
            Assert.That(basketCompo.FruitsList.Any(x => x.gameObject.GetInstanceID() == newFruit.gameObject.GetInstanceID()));
            Assert.That(basketCompo.FruitsList.Any(x => x.gameObject.GetInstanceID() == secondNewFruit.gameObject.GetInstanceID()));
            Assert.That(basketCompo.FruitsList.Any(x => x.GetInstanceID() == newFruit.GetInstanceID()));
            Assert.That(basketCompo.FruitsList.Any(x => x.GetInstanceID() == secondNewFruit.GetInstanceID()));

            // Asserts on GetComponentList
            IEnumerable<Fruit> fruits = Injector.GetEnumerableComponents<Fruit>();
            Assert.That(fruits.Count(), Is.EqualTo(2));
            Assert.That(fruits.Any(x => x.gameObject.GetInstanceID() == newFruit.gameObject.GetInstanceID()));
            Assert.That(fruits.Any(x => x.gameObject.GetInstanceID() == secondNewFruit.gameObject.GetInstanceID()));
            Assert.That(fruits.Any(x => x.GetInstanceID() == newFruit.GetInstanceID()));
            Assert.That(fruits.Any(x => x.GetInstanceID() == secondNewFruit.GetInstanceID()));
        }

        [Test]
        public void DualBasketComponent_ShouldReturnComponentsWithName_WhenGetComponentListOnTwoIdenticalTypeWithName()
        {
            DualBasketComponent dualBasketCompo = InjectableBuilder<DualBasketComponent>.Create().Build();

            Assert.Null(dualBasketCompo.FruitsList);

            // Arrange + Act
            TestInjector Injector = new TestInjector(dualBasketCompo);
            Injector.CheckAndInjectAll();

            // Checks
            Assert.NotNull(dualBasketCompo.FruitsList);
            Assert.NotNull(dualBasketCompo.FruitsEnumerable);
            Assert.IsEmpty(dualBasketCompo.FruitsList);
            Assert.IsEmpty(dualBasketCompo.FruitsEnumerable);

            // Get fruitsList
            IEnumerable<Fruit> fruitsList = Injector.GetEnumerableComponents<Fruit>("fruitsList");
            Assert.IsEmpty(fruitsList);

            // Get fruitsEnumerable
            IEnumerable<Fruit> fruitsEnumerable = Injector.GetEnumerableComponents<Fruit>("fruitsEnumerable");
            Assert.IsEmpty(fruitsEnumerable);
        }

        [Test]
        public void DualBasketComponent_ShouldFillListForBothWithName()
        {
            DualBasketComponent dualBasketCompo = InjectableBuilder<DualBasketComponent>.Create().Build();

            Assert.Null(dualBasketCompo.FruitsList);
            Assert.Null(dualBasketCompo.FruitsEnumerable);

            // Arrange
            TestInjector Injector = new TestInjector(dualBasketCompo);
            Injector.CheckAndInjectAll();

            Assert.NotNull(dualBasketCompo.FruitsList);
            Assert.NotNull(dualBasketCompo.FruitsEnumerable);

            // Act
            IEnumerable<Fruit> fruits = Injector.InitEnumerableComponents<Fruit>(2);
            Fruit newFruit = fruits.First();
            Fruit secondNewFruit = fruits.Skip(1).First();

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

            // Asserts on get list (same for both, because same enumerable type on children)
            IEnumerable<Fruit> fruitsList = Injector.GetEnumerableComponents<Fruit>("fruitsList");
            Assert.That(fruitsList.Count(), Is.EqualTo(2));
            Assert.That(fruitsList.Any(x => x.gameObject.GetInstanceID() == newFruit.gameObject.GetInstanceID()));
            Assert.That(fruitsList.Any(x => x.gameObject.GetInstanceID() == secondNewFruit.gameObject.GetInstanceID()));
            Assert.That(fruitsList.Any(x => x.GetInstanceID() == newFruit.GetInstanceID()));
            Assert.That(fruitsList.Any(x => x.GetInstanceID() == secondNewFruit.GetInstanceID()));

            // Asserts on get enumerable (same for both, because same enumerable type on children)
            IEnumerable<Fruit> fruitsEnumerable = Injector.GetEnumerableComponents<Fruit>("fruitsEnumerable");
            Assert.That(fruitsEnumerable.Count(), Is.EqualTo(2));
            Assert.That(fruitsEnumerable.Any(x => x.gameObject.GetInstanceID() == newFruit.gameObject.GetInstanceID()));
            Assert.That(fruitsEnumerable.Any(x => x.gameObject.GetInstanceID() == secondNewFruit.gameObject.GetInstanceID()));
            Assert.That(fruitsEnumerable.Any(x => x.GetInstanceID() == newFruit.GetInstanceID()));
            Assert.That(fruitsEnumerable.Any(x => x.GetInstanceID() == secondNewFruit.GetInstanceID()));
        }

        [Test]
        public void PlayerGroupWithSubsequentLevel_ShouldFillRecursively()
        {
            PlayerGroup playerGroup = InjectableBuilder<PlayerGroup>.Create().Build();
            Assert.Null(playerGroup.Players);

            // Prepare
            TestInjector Injector = new TestInjector(playerGroup);
            Injector.CheckAndInjectAll();

            // Asserts fields filled
            Assert.IsNotNull(playerGroup.Players);
            Assert.IsEmpty(playerGroup.Players);

            IEnumerable<Player> players = Injector.InitEnumerableComponents<Player>(2);
            Player firstPlayer = players.First();
            Player secondPlayer = players.Skip(1).First();

            // check player in
            Assert.That(playerGroup.Players.Count, Is.EqualTo(2));
            Assert.That(playerGroup.Players.Any(x => x.GetInstanceID() == firstPlayer.GetInstanceID()));
            Assert.That(playerGroup.Players.Any(x => x.GetInstanceID() == secondPlayer.GetInstanceID()));

            // get last component level and check this is the same from the top
            Player firstPlayerRetrieved = playerGroup.Players.Single(x => x.GetInstanceID() == firstPlayer.GetInstanceID());
            Skill attackSkillRetrieved = firstPlayerRetrieved.Sorcerer.AttackSkill;

            Sorcerer firstSorcerer = Injector.GetComponent<Sorcerer>(firstPlayer);
            Assert.That(firstPlayerRetrieved.Sorcerer.GetInstanceID(), Is.EqualTo(firstSorcerer.GetInstanceID()));

            Skill attackSkill = Injector.GetComponent<Skill>("attackSkill", firstSorcerer);
            Assert.That(attackSkillRetrieved.GetInstanceID(), Is.EqualTo(attackSkill.GetInstanceID()));

            // injectField to sorcerer level
            SO_InventoryBag inventoryBag = ScriptableObject.CreateInstance<SO_InventoryBag>();
            inventoryBag.BagName = "Baggy";
            Injector.InjectField(inventoryBag, "secondInventoryBagInfos", firstSorcerer);

            Assert.That(firstSorcerer.SecondInventoryBagInfos.GetInstanceID(), Is.EqualTo(inventoryBag.GetInstanceID()));
            Assert.That(firstSorcerer.SecondInventoryBagInfos.BagName, Is.EqualTo(inventoryBag.BagName));

            // check from the top, values are correctly filled
            Assert.That(firstPlayerRetrieved.Sorcerer.SecondInventoryBagInfos.GetInstanceID(), Is.EqualTo(inventoryBag.GetInstanceID()));
            Assert.That(firstPlayerRetrieved.Sorcerer.SecondInventoryBagInfos.BagName, Is.EqualTo(inventoryBag.BagName));
        }

        [Test]
        public void PlayerGroupWithNameOnSameTypeAndSubsequentLevel_ShouldFillRecursively_WithName()
        {
            PlayerGroupWithName playerGroupWithName = InjectableBuilder<PlayerGroupWithName>.Create().Build();
            Assert.Null(playerGroupWithName.Players);
            Assert.Null(playerGroupWithName.SecondPlayers);

            // Prepare
            TestInjector Injector = new TestInjector(playerGroupWithName);
            Injector.CheckAndInjectAll();

            // Asserts fields filled
            Assert.IsNotNull(playerGroupWithName.Players);
            Assert.IsEmpty(playerGroupWithName.Players);
            Assert.IsNotNull(playerGroupWithName.SecondPlayers);
            Assert.IsEmpty(playerGroupWithName.SecondPlayers);

            IEnumerable<Player> players = Injector.InitEnumerableComponents<Player>(2);
            Player firstPlayer = players.First();
            Player secondPlayer = players.Skip(1).First();

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

            Sorcerer firstSorcerer = Injector.GetComponent<Sorcerer>(firstPlayer);
            Assert.That(firstPlayerRetrieved.Sorcerer.GetInstanceID(), Is.EqualTo(firstSorcerer.GetInstanceID()));

            Skill attackSkill = Injector.GetComponent<Skill>("attackSkill", firstSorcerer);
            Assert.That(attackSkillRetrieved.GetInstanceID(), Is.EqualTo(attackSkill.GetInstanceID()));

            // injectField to sorcerer level
            SO_InventoryBag inventoryBag = ScriptableObject.CreateInstance<SO_InventoryBag>();
            inventoryBag.BagName = "Baggy";
            Injector.InjectField(inventoryBag, "secondInventoryBagInfos", firstSorcerer);

            Assert.That(firstSorcerer.SecondInventoryBagInfos.GetInstanceID(), Is.EqualTo(inventoryBag.GetInstanceID()));
            Assert.That(firstSorcerer.SecondInventoryBagInfos.BagName, Is.EqualTo(inventoryBag.BagName));

            // check from the top, values are correctly filled
            Assert.That(firstPlayerRetrieved.Sorcerer.SecondInventoryBagInfos.GetInstanceID(), Is.EqualTo(inventoryBag.GetInstanceID()));
            Assert.That(firstPlayerRetrieved.Sorcerer.SecondInventoryBagInfos.BagName, Is.EqualTo(inventoryBag.BagName));
        }

        [Test]
        public void GetComponentInList_ShouldReturnEmptyWhenNeverAdded()
        {
            // Prepare
            FullBasketListExample basket = InjectableBuilder<FullBasketListExample>.Create().Build();
            TestInjector Injector = new TestInjector(basket);
            Injector.CheckAndInjectAll();

            // Parent
            IEnumerable<Fruit> parentFruitsList = Injector.GetEnumerableComponents<Fruit>("FruitsListParent");
            Assert.IsEmpty(parentFruitsList);
            IEnumerable<Fruit> parentFruitsEnumerable = Injector.GetEnumerableComponents<Fruit>("FruitsEnumerableParent");
            Assert.IsEmpty(parentFruitsEnumerable);
            Assert.IsEmpty(basket.FruitsEnumerableParent);
            Assert.IsEmpty(basket.FruitsListParent);

            // Current
            IEnumerable<Fruit> fruitsList = Injector.GetEnumerableComponents<Fruit>("FruitsList");
            Assert.IsEmpty(fruitsList);
            IEnumerable<Fruit> fruitsEnumerable = Injector.GetEnumerableComponents<Fruit>("FruitsEnumerable");
            Assert.IsEmpty(fruitsEnumerable);
            Assert.IsEmpty(basket.FruitsEnumerable);
            Assert.IsEmpty(basket.FruitsList);

            // Child
            IEnumerable<Fruit> childFruitsList = Injector.GetEnumerableComponents<Fruit>("FruitsListChildren");
            Assert.IsEmpty(childFruitsList);
            IEnumerable<Fruit> childFruitsEnumerable = Injector.GetEnumerableComponents<Fruit>("FruitsEnumerableChildren");
            Assert.IsEmpty(childFruitsEnumerable);
            Assert.IsEmpty(basket.FruitsEnumerableChildren);
            Assert.IsEmpty(basket.FruitsListChildren);
        }

        [Test]
        public void GetComponentInChildList_ShouldReturnEmptyWhenNeverAdded()
        {
            // Prepare
            AboveFullBasketListExample aboveBasket = InjectableBuilder<AboveFullBasketListExample>.Create().Build();
            TestInjector Injector = new TestInjector(aboveBasket);
            Injector.CheckAndInjectAll();

            // ChildComponent
            FullBasketListExample basket = Injector.GetComponent<FullBasketListExample>();

            // Parent
            IEnumerable<Fruit> parentFruitsList = Injector.GetEnumerableComponents<Fruit>("FruitsListParent", basket);
            Assert.IsEmpty(parentFruitsList);
            IEnumerable<Fruit> parentFruitsEnumerable = Injector.GetEnumerableComponents<Fruit>("FruitsEnumerableParent", basket);
            Assert.IsEmpty(parentFruitsEnumerable);
            Assert.IsEmpty(aboveBasket.ParentBasket.FruitsEnumerableParent);
            Assert.IsEmpty(aboveBasket.ParentBasket.FruitsListParent);

            // Current
            IEnumerable<Fruit> fruitsList = Injector.GetEnumerableComponents<Fruit>("FruitsList", basket);
            Assert.IsEmpty(fruitsList);
            IEnumerable<Fruit> fruitsEnumerable = Injector.GetEnumerableComponents<Fruit>("FruitsEnumerable", basket);
            Assert.IsEmpty(fruitsEnumerable);
            Assert.IsEmpty(aboveBasket.ParentBasket.FruitsEnumerable);
            Assert.IsEmpty(aboveBasket.ParentBasket.FruitsList);

            // Child
            IEnumerable<Fruit> childFruitsList = Injector.GetEnumerableComponents<Fruit>("FruitsListChildren", basket);
            Assert.IsEmpty(childFruitsList);
            IEnumerable<Fruit> childFruitsEnumerable = Injector.GetEnumerableComponents<Fruit>("FruitsEnumerableChildren", basket);
            Assert.IsEmpty(childFruitsEnumerable);
            Assert.IsEmpty(aboveBasket.ParentBasket.FruitsEnumerableChildren);
            Assert.IsEmpty(aboveBasket.ParentBasket.FruitsListChildren);
        }

        [Test]
        public void AddComponentInList_ShouldInjectRecurvisely_OnCurrentLists()
        {
            // Prepare
            Cargo cargo = InjectableBuilder<Cargo>.Create().Build();
            TestInjector Injector = new TestInjector(cargo);
            Injector.CheckAndInjectAll();

            // Check init
            Assert.IsEmpty(cargo.FirstContainers);
            Assert.IsEmpty(cargo.SecondContainers);
            Assert.NotNull(cargo.WaterTransform);

            // Adding new container in lists
            Container container = Injector.InitSingleEnumerableComponent<Container>();
            Assert.That(cargo.FirstContainers.Count(), Is.EqualTo(1));
            Assert.That(cargo.SecondContainers.Count(), Is.EqualTo(1));
            Assert.That(cargo.FirstContainers.First().GetInstanceID(), Is.EqualTo(cargo.SecondContainers.First().GetInstanceID()));
            Assert.That(cargo.FirstContainers.First().GetInstanceID(), Is.EqualTo(container.GetInstanceID()));

            // Check child init
            Assert.IsEmpty(container.FirstBananaPacks);
            Assert.IsEmpty(container.SecondBananaPacks);
            Assert.NotNull(container.OpenCloseButton);
            Assert.NotNull(container.LogoImg);

            // Adding new bananaPack in child lists (container)
            BananaPack bananaPack = Injector.InitSingleEnumerableComponent<BananaPack>(container);
            Assert.That(container.FirstBananaPacks.Count(), Is.EqualTo(1));
            Assert.That(container.SecondBananaPacks.Count(), Is.EqualTo(1));
            Assert.That(container.FirstBananaPacks.First().GetInstanceID(), Is.EqualTo(container.SecondBananaPacks.First().GetInstanceID()));
            Assert.That(container.FirstBananaPacks.First().GetInstanceID(), Is.EqualTo(bananaPack.GetInstanceID()));

            // Check nothing else was filled
            Assert.IsEmpty(cargo.ChildContainers);
            Assert.IsEmpty(cargo.ParentContainers);
            Assert.IsEmpty(container.ParentBananaPacks);
            Assert.IsEmpty(container.ChildBananaPacks);
        }

        [Test]
        public void AddComponentInList_ShouldInjectRecurvisely_OnParentLists()
        {
            // Prepare
            Cargo cargo = InjectableBuilder<Cargo>.Create().Build();
            TestInjector Injector = new TestInjector(cargo);
            Injector.CheckAndInjectAll();

            // Check init
            Assert.IsEmpty(cargo.ParentContainers);

            // Adding new container in lists
            Container container = Injector.InitSingleEnumerableComponent<Container>(GameObjectLevel.Parent);
            Assert.That(cargo.ParentContainers.Count(), Is.EqualTo(1));
            Assert.That(cargo.ParentContainers.First().GetInstanceID(), Is.EqualTo(container.GetInstanceID()));

            // Check child init
            Assert.IsEmpty(container.ParentBananaPacks);

            // Adding new bananaPack in child lists (container)
            BananaPack bananaPack = Injector.InitSingleEnumerableComponent<BananaPack>(GameObjectLevel.Parent, container);
            Assert.That(container.ParentBananaPacks.Count(), Is.EqualTo(1));
            Assert.That(container.ParentBananaPacks.First().GetInstanceID(), Is.EqualTo(bananaPack.GetInstanceID()));

            // Check nothing else was filled
            Assert.IsEmpty(cargo.FirstContainers);
            Assert.IsEmpty(cargo.SecondContainers);
            Assert.IsEmpty(cargo.ChildContainers);
            Assert.IsEmpty(container.FirstBananaPacks);
            Assert.IsEmpty(container.SecondBananaPacks);
            Assert.IsEmpty(container.ChildBananaPacks);
        }

        [Test]
        public void AddComponentInList_ShouldInjectRecurvisely_OnChildLists()
        {
            // Prepare
            Cargo cargo = InjectableBuilder<Cargo>.Create().Build();
            TestInjector Injector = new TestInjector(cargo);
            Injector.CheckAndInjectAll();

            // Check init
            Assert.IsEmpty(cargo.ChildContainers);

            // Adding new container in lists
            Container container = Injector.InitSingleEnumerableComponent<Container>(GameObjectLevel.Children);
            Assert.That(cargo.ChildContainers.Count(), Is.EqualTo(1));
            Assert.That(cargo.ChildContainers.First().GetInstanceID(), Is.EqualTo(container.GetInstanceID()));

            // Check child init
            Assert.IsEmpty(container.ChildBananaPacks);

            // Adding new bananaPack in child lists (container)
            BananaPack bananaPack = Injector.InitSingleEnumerableComponent<BananaPack>(GameObjectLevel.Children, container);
            Assert.That(container.ChildBananaPacks.Count(), Is.EqualTo(1));
            Assert.That(container.ChildBananaPacks.First().GetInstanceID(), Is.EqualTo(bananaPack.GetInstanceID()));

            // Check nothing else was filled
            Assert.IsEmpty(cargo.FirstContainers);
            Assert.IsEmpty(cargo.SecondContainers);
            Assert.IsEmpty(cargo.ParentContainers);
            Assert.IsEmpty(container.FirstBananaPacks);
            Assert.IsEmpty(container.SecondBananaPacks);
            Assert.IsEmpty(container.ParentBananaPacks);
        }

        [TestCase(GameObjectLevel.Children)]
        [TestCase(GameObjectLevel.Parent)]
        public void AddComponentInList_ShouldThrowException_WhenWithGameObjectLevelDoesNotExist(GameObjectLevel gameObjectLevelToCheck)
        {
            // Prepare
            Basket basket = InjectableBuilder<Basket>.Create().Build();
            TestInjector Injector = new TestInjector(basket);
            Injector.CheckAndInjectAll();

            // Adding new fruit in lists which not exist with GameObjectLevel flag
            Assert.Throws<TestInjectorException>(() => Injector.InitEnumerableComponents<Fruit>(gameObjectLevelToCheck));
        }

        [TestCase(GameObjectLevel.Children)]
        [TestCase(GameObjectLevel.Parent)]
        public void AddComponentInList_ShouldThrowException_WhenWithGameObjectLevelDoesNotExist_ForChildList(GameObjectLevel gameObjectLevelToCheck)
        {
            // Prepare
            ParentBasket parentBasket = InjectableBuilder<ParentBasket>.Create().Build();
            TestInjector Injector = new TestInjector(parentBasket);
            Injector.CheckAndInjectAll();

            Basket basket = Injector.GetComponent<Basket>();

            // Adding new fruit in lists which not exist with GameObjectLevel flag
            Assert.Throws<TestInjectorException>(() => Injector.InitEnumerableComponents<Fruit>(gameObjectLevelToCheck, 1, basket));
        }

        [Test]
        public void AddComponentInList_ShouldThrowException_WhenWithoutGameObjectLevelDoesNotExist_ForCurrent_ForChild()
        {
            // Prepare
            ParentBasketWithoutCurrentList parentBasket = InjectableBuilder<ParentBasketWithoutCurrentList>.Create().Build();
            TestInjector Injector = new TestInjector(parentBasket);
            Injector.CheckAndInjectAll();

            BasketWithoutCurrentList basket = Injector.GetComponent<BasketWithoutCurrentList>();

            // Adding new fruit in lists which not exist with GameObjectLevel flag
            Assert.Throws<TestInjectorException>(() => Injector.InitEnumerableComponents<Fruit>(1, basket));
        }
        #endregion General

        #region GetEnumerable Exceptions
        [Test]
        public void InitEnumerableComponentsWithType_WithEmptyType_ShouldThrowException()
        {
            Basket basket = InjectableBuilder<Basket>.Create().Build();
            TestInjector Injector = new TestInjector(basket);
            Injector.CheckAndInjectAll();

            Type[] emptyTypes = new Type[0];
            Assert.Throws<TestInjectorException>(() => Injector.InitEnumerableComponentsWithTypes<Fruit>(emptyTypes));
        }

        [Test]
        public void InitEnumerableComponentsWithType_WithWrongType_ShouldThrowException()
        {
            Basket basket = InjectableBuilder<Basket>.Create().Build();
            TestInjector Injector = new TestInjector(basket);
            Injector.CheckAndInjectAll();

            Type[] wrongTypes = new Type[] { typeof(Cat) };
            Assert.Throws<TestInjectorException>(() => Injector.InitEnumerableComponentsWithTypes<Fruit>(wrongTypes));
        }

        [Test]
        public void GetEnumerableComponentsWithoutFieldName_WithWrongType_ShouldThrowException()
        {
            Basket basket = InjectableBuilder<Basket>.Create().Build();
            TestInjector Injector = new TestInjector(basket);
            Injector.CheckAndInjectAll();

            Assert.Throws<TestInjectorException>(() => Injector.GetEnumerableComponents<Cat>());
        }

        [Test]
        public void GetEnumerableComponentsWithoutFieldName_WithTypeFoundManyTimes_ShouldThrowException()
        {
            Basket basket = InjectableBuilder<Basket>.Create().Build();
            TestInjector Injector = new TestInjector(basket);
            Injector.CheckAndInjectAll();

            Assert.Throws<TestInjectorException>(() => Injector.GetEnumerableComponents<Fruit>());
        }

        [Test]
        public void GetEnumerableComponentsWithFieldName_WithWrongType_ShouldThrowException()
        {
            Basket basket = InjectableBuilder<Basket>.Create().Build();
            TestInjector Injector = new TestInjector(basket);
            Injector.CheckAndInjectAll();

            Assert.Throws<TestInjectorException>(() => Injector.GetEnumerableComponents<Cat>("anyName"));
        }

        [Test]
        public void GetEnumerableComponentsWithFieldName_WithTypeFoundManyTimes_AndWrongName_ShouldThrowException()
        {
            Basket basket = InjectableBuilder<Basket>.Create().Build();
            TestInjector Injector = new TestInjector(basket);
            Injector.CheckAndInjectAll();

            Assert.Throws<TestInjectorException>(() => Injector.GetEnumerableComponents<Fruit>("anyName"));
        }

        [Test]
        public void All_InitEnumerableComponents_WithoutTypeFound_ShouldThrowException()
        {
            Basket basket = InjectableBuilder<Basket>.Create().Build();
            TestInjector Injector = new TestInjector(basket);
            Injector.CheckAndInjectAll();

            Assert.Throws<TestInjectorException>(() => Injector.InitEnumerableComponents<Cat>(2));
            Assert.Throws<TestInjectorException>(() => Injector.InitEnumerableComponentsWithTypes<Cat>(typeof(Fruit)));
            Assert.Throws<TestInjectorException>(() => Injector.InitEnumerableComponentsWithTypes<Cat>(typeof(Cat)));
            Assert.Throws<TestInjectorException>(() => Injector.InitSingleEnumerableComponent<Cat>());
            Assert.Throws<TestInjectorException>(() => Injector.InitSingleEnumerableComponent<Cat>(GameObjectLevel.Children));
            Assert.Throws<TestInjectorException>(() => Injector.InitSingleEnumerableComponent<Cat>(GameObjectLevel.Parent));
        }
        #endregion GetEnumerable Exceptions

        #region Error decorator
        [TestCase(typeof(FallenEnumerablesComponent))]
        [TestCase(typeof(FallenEnumerablesEmptyClass))]
        [TestCase(typeof(FallenEnumerablesInjectable))]
        [TestCase(typeof(FallenEnumerablesInterface))]
        [TestCase(typeof(FallenEnumerablesIReadOnlyList))]
        [TestCase(typeof(FallenEnumerablesNonComponentArray))]
        [TestCase(typeof(FallenEnumerablesNonComponentEnumerable))]
        [TestCase(typeof(FallenEnumerablesNonComponentList))]
        [TestCase(typeof(FallenEnumerablesReadOnlyCollection))]
        public void TestInjector_InjectComponentList_OnWrongFieldType_ShouldThrowException(Type injectableTypeToBuild)
        {
            Component component = InjectableBuilderWithExpliciteType.Create().Build(injectableTypeToBuild);
            MonoBehaviourInjectable injectable = component as MonoBehaviourInjectable;

            TestInjector Injector = new TestInjector(injectable);

            Exception exception = Assert.Throws<TestInjectorException>(() => Injector.CheckAndInjectAll());

            StringAssert.Contains("using decorator NixInjectComponentsAttribute", exception.Message);
        }

        [TestCase(typeof(FallenArrayComponent), "NixInjectComponentAttribute")]
        [TestCase(typeof(FallenEnumerableComponent), "NixInjectComponentAttribute")]
        [TestCase(typeof(FallenListComponent), "NixInjectComponentAttribute")]
        [TestCase(typeof(FallenArrayComponentChild), "NixInjectComponentFromChildrenAttribute")]
        [TestCase(typeof(FallenEnumerableComponentChild), "NixInjectComponentFromChildrenAttribute")]
        [TestCase(typeof(FallenListComponentChild), "NixInjectComponentFromChildrenAttribute")]
        [TestCase(typeof(FallenArrayComponentParent), "NixInjectComponentFromParentAttribute")]
        [TestCase(typeof(FallenEnumerableComponentParent), "NixInjectComponentFromParentAttribute")]
        [TestCase(typeof(FallenListComponentParent), "NixInjectComponentFromParentAttribute")]
        [TestCase(typeof(FallenArrayComponentRoot), "NixInjectRootComponentAttribute")]
        [TestCase(typeof(FallenEnumerableComponentRoot), "NixInjectRootComponentAttribute")]
        [TestCase(typeof(FallenListComponentRoot), "NixInjectRootComponentAttribute")]
        [TestCase(typeof(FallenArrayComponentRootChild), "NixInjectRootComponentAttribute")]
        [TestCase(typeof(FallenEnumerableComponentRootChild), "NixInjectRootComponentAttribute")]
        [TestCase(typeof(FallenListComponentRootChild), "NixInjectRootComponentAttribute")]
        public void TestInjector_InjectAnyComponentWhichIsNotListDecorator_OnWrongEnumerableFieldType_ShouldThrowException(Type injectableTypeToBuild, string attributeName)
        {
            Component component = InjectableBuilderWithExpliciteType.Create().Build(injectableTypeToBuild);
            MonoBehaviourInjectable injectable = component as MonoBehaviourInjectable;

            TestInjector Injector = new TestInjector(injectable);

            Exception exception = Assert.Throws<TestInjectorException>(() => Injector.CheckAndInjectAll());

            StringAssert.Contains($"using decorator {attributeName}", exception.Message);
        }
        #endregion Error decorator
    }
}
