﻿using Moq;
using NixiTestTools;
using NUnit.Framework;
using ScriptExample.Characters;
using ScriptExample.Characters.Broken;
using ScriptExample.Characters.ScriptableObjects;
using ScriptExample.Containers;
using ScriptExample.Containers.Broken;
using ScriptExample.Containers.GameHost;
using ScriptExample.Containers.Player;
using ScriptExample.Game;
using ScriptExample.GameHost;
using ScriptExample.Players;
using UnityEngine;

namespace Tests.Injections
{
    internal sealed class RecursiveInjectionTests : InjectableTestTemplate<GameHost>
    {
        [SetUp]
        public void InitTests()
        {
            ResetTemplate();
        }

        [Test]
        public void RecursiveInjectionTests_ShouldGetGameObjectAtTwoLevels()
        {
            Player player = MainInjector.GetComponent<Player>();
            Assert.That(player, Is.Not.Null);

            Sorcerer sorcerer = MainInjector.GetComponent<Sorcerer>(player);
            Assert.That(sorcerer, Is.Not.Null);
        }

        [Test]
        public void RecursiveInjectionTests_ShouldGetGameObjectWithNameAtThirdLevel()
        {
            Player player = MainInjector.GetComponent<Player>();
            Sorcerer sorcerer = MainInjector.GetComponent<Sorcerer>(player);

            Skill magicSkill = MainInjector.GetComponent<Skill>("magicSkill", sorcerer);
            Skill attackSkill = MainInjector.GetComponent<Skill>("attackSkill", sorcerer);

            Assert.That(magicSkill, Is.Not.Null);
            Assert.That(magicSkill, Is.EqualTo(sorcerer.MagicSkill));

            Assert.That(attackSkill, Is.Not.Null);
            Assert.That(attackSkill, Is.EqualTo(sorcerer.AttackSkill));
        }

        [Test]
        public void RecursiveInjectionTests_ShouldThrowExceptionWhenGameObjectNotFound_AtSameLevel()
        {
            Assert.Throws<TestInjectorException>(() => MainInjector.GetComponent<Skill>());
        }

        [Test]
        public void RecursiveInjectionTests_ShouldThrowExceptionWhenGameObjectNotFound_AtSecondLevel()
        {
            Player player = MainInjector.GetComponent<Player>();
            Assert.Throws<TestInjectorException>(() => MainInjector.GetComponent<Skill>(player));
        }

        [Test]
        public void RecursiveInjectionTests_ShouldThrowExceptionWhenGameObjectWithNameNotFound_AtSecondLevel()
        {
            Player player = MainInjector.GetComponent<Player>();
            Assert.Throws<TestInjectorException>(() => MainInjector.GetComponent<Sorcerer>("anyName", player));
        }

        [Test]
        public void RecursiveInjectionTests_ShouldThrowExceptionFromGameObjectNeverInjected()
        {
            Player fakePlayer = new GameObject().AddComponent<Player>();
            Assert.Throws<TestInjectorException>(() => MainInjector.GetComponent<Sorcerer>(fakePlayer));
        }

        [Test]
        public void RecursiveInjectionTests_ShouldThrowException_WhenInjectScriptableObjectMockWithoutName_OnSameScriptableObjectType()
        {
            Player player = MainInjector.GetComponent<Player>();
            Sorcerer sorcerer = MainInjector.GetComponent<Sorcerer>(player);
            Assert.That(sorcerer.FirstInventoryBagInfos, Is.Null);
            Assert.That(sorcerer.SecondInventoryBagInfos, Is.Null);

            SO_InventoryBag firstInventoryBagInfos = ScriptableObject.CreateInstance<SO_InventoryBag>();

            Assert.Throws<TestInjectorException>(() => MainInjector.InjectField(firstInventoryBagInfos, sorcerer));
        }

        #region InjectFields without name
        [Test]
        public void RecursiveInjectionTests_ShouldInjectField_CorrectlyAtSameLevel()
        {
            Mock<ISettings> settingsMock = new Mock<ISettings>(MockBehavior.Strict);
            settingsMock.SetupGet(x => x.GameName).Returns("Nixi").Verifiable();
            
            MainInjector.InjectField(settingsMock.Object);

            Assert.That(MainTested.Settings.GameName, Is.EqualTo("Nixi"));
            settingsMock.VerifyGet(x => x.GameName, Times.Once);
        }

        [Test]
        public void RecursiveInjectionTests_ShouldInjectField_CorrectlyAtSecondLevel()
        {
            // Check player find
            Player player = MainInjector.GetComponent<Player>();
            Assert.That(player, Is.Not.Null);
            Assert.That(player.LifeBar, Is.Null);

            // Inject LifeBar at Player level
            Mock<ILifeBar> playerLifeBarMock = new Mock<ILifeBar>(MockBehavior.Strict);
            playerLifeBarMock.SetupGet(x => x.CurrentHealth).Returns(123).Verifiable();
            
            MainInjector.InjectField(playerLifeBarMock.Object, player);

            Assert.That(player.LifeBar, Is.Not.Null);
            Assert.That(player.LifeBar.CurrentHealth, Is.EqualTo(123));
            playerLifeBarMock.VerifyGet(x => x.CurrentHealth, Times.Once);
        }

        [Test]
        public void RecursiveInjectionTests_ShouldInjectField_CorrectlyAtThirdLevel()
        {
            // Check sorcerer find in player find
            Player player = MainInjector.GetComponent<Player>();
            Sorcerer sorcerer = MainInjector.GetComponent<Sorcerer>(player);

            Assert.That(sorcerer, Is.Not.Null);
            Assert.That(sorcerer.TestInterface, Is.Null);

            // Inject TestInterface at Sorcerer level
            Mock<ITestInterface> sorcererTestInterfaceMock = new Mock<ITestInterface>(MockBehavior.Strict);
            sorcererTestInterfaceMock.SetupGet(x => x.ValueToRetrieve).Returns(222).Verifiable();

            MainInjector.InjectField(sorcererTestInterfaceMock.Object, sorcerer);

            Assert.That(sorcerer.TestInterface, Is.Not.Null);
            Assert.That(sorcerer.TestInterface.ValueToRetrieve, Is.EqualTo(222));
            sorcererTestInterfaceMock.VerifyGet(x => x.ValueToRetrieve, Times.Once);
        }

        [Test]
        public void RecursiveInjectionTests_ShouldInjectScriptableObjectMockWithoutName_CorrectlyAtFirstLevel()
        {
            Assert.That(MainTested.SOGameHostInfos, Is.Null);

            SO_GameHost gameHostInfos = ScriptableObject.CreateInstance<SO_GameHost>();
            gameHostInfos.NbSlot = 3;

            MainInjector.InjectField(gameHostInfos);

            Assert.That(MainTested.SOGameHostInfos, Is.Not.Null);
            Assert.That(MainTested.SOGameHostInfos.NbSlot, Is.EqualTo(gameHostInfos.NbSlot));
        }

        [Test]
        public void RecursiveInjectionTests_ShouldInjectScriptableObjectMockWithoutName_CorrectlyAtSecondLevel()
        {
            Player player = MainInjector.GetComponent<Player>();
            Assert.That(player.SOPlayerInfos, Is.Null);

            SO_Player playerInfos = ScriptableObject.CreateInstance<SO_Player>();
            playerInfos.Pseudo = "Kainy";

            MainInjector.InjectField(playerInfos, player);

            Assert.That(player.SOPlayerInfos, Is.Not.Null);
            Assert.That(player.SOPlayerInfos.Pseudo, Is.EqualTo(playerInfos.Pseudo));

            Assert.That(MainTested.Player.SOPlayerInfos.Pseudo, Is.EqualTo(playerInfos.Pseudo));
        }

        [Test]
        public void RecursiveInjectionTests_ShouldInjectScriptableObjectMockWithoutName_CorrectlyAtThirdLevel()
        {
            Player player = MainInjector.GetComponent<Player>();
            Sorcerer sorcerer = MainInjector.GetComponent<Sorcerer>(player);
            Assert.That(sorcerer.SOInfos, Is.Null);

            SO_Sorcerer sorcererInfos = ScriptableObject.CreateInstance<SO_Sorcerer>();
            sorcererInfos.CharaName = "Magda";
            sorcererInfos.ManaMax = 1234;

            MainInjector.InjectField(sorcererInfos, sorcerer);

            Assert.That(sorcerer.SOInfos, Is.Not.Null);
            Assert.That(sorcerer.SOInfos.CharaName, Is.EqualTo(sorcererInfos.CharaName));
            Assert.That(sorcerer.SOInfos.ManaMax, Is.EqualTo(sorcererInfos.ManaMax));

            Assert.That(MainTested.Player.Sorcerer.SOInfos.CharaName, Is.EqualTo(sorcererInfos.CharaName));
            Assert.That(MainTested.Player.Sorcerer.SOInfos.ManaMax, Is.EqualTo(sorcererInfos.ManaMax));
        }
        #endregion InjectFields without name

        #region InjectFields with name
        [Test]
        public void RecursiveInjectionTests_ShouldInjectFieldWithName_CorrectlyAtSameLevel()
        {
            // Check
            Assert.That(MainTested.FirstBrokenInterfaceGameHost, Is.Null);
            Assert.That(MainTested.SecondBrokenInterfaceGameHost, Is.Null);

            // Mock
            Mock<IBrokenTestInterface> brokenTestInterfaceMock = new Mock<IBrokenTestInterface>(MockBehavior.Strict);
            brokenTestInterfaceMock.SetupGet(x => x.ValueToRetrieve).Returns(1234).Verifiable();

            // Act
            MainInjector.InjectField(brokenTestInterfaceMock.Object, "FirstBrokenInterfaceGameHost");

            Assert.That(MainTested.FirstBrokenInterfaceGameHost.ValueToRetrieve, Is.EqualTo(1234));
            Assert.That(MainTested.SecondBrokenInterfaceGameHost, Is.Null);
            brokenTestInterfaceMock.VerifyGet(x => x.ValueToRetrieve, Times.Once);
        }

        [Test]
        public void RecursiveInjectionTests_ShouldInjectFieldWithName_CorrectlyAtSecondLevel()
        {
            // Check player find
            SecondPlayer secondPlayer = MainInjector.GetComponent<SecondPlayer>();
            Assert.That(secondPlayer, Is.Not.Null);
            Assert.That(secondPlayer.FirstBrokenInterfacePlayer, Is.Null);
            Assert.That(secondPlayer.SecondBrokenInterfacePlayer, Is.Null);

            // Inject IBrokenTestInterface at Player level
            Mock<IBrokenTestInterface> playerBrokenInterfaceMock = new Mock<IBrokenTestInterface>(MockBehavior.Strict);
            playerBrokenInterfaceMock.SetupGet(x => x.ValueToRetrieve).Returns(345).Verifiable();

            MainInjector.InjectField(playerBrokenInterfaceMock.Object, "FirstBrokenInterfacePlayer", secondPlayer);

            Assert.That(secondPlayer.FirstBrokenInterfacePlayer, Is.Not.Null);
            Assert.That(secondPlayer.FirstBrokenInterfacePlayer.ValueToRetrieve, Is.EqualTo(345));
            playerBrokenInterfaceMock.VerifyGet(x => x.ValueToRetrieve, Times.Once);
        }

        [Test]
        public void RecursiveInjectionTests_ShouldInjectFieldWithName_CorrectlyAtThirdLevel()
        {
            // Check brokenSorcerer find in secondPlayer find
            SecondPlayer secondPlayer = MainInjector.GetComponent<SecondPlayer>();
            BrokenSorcerer brokenSorcerer = MainInjector.GetComponent<BrokenSorcerer>(secondPlayer);

            Assert.That(brokenSorcerer, Is.Not.Null);
            Assert.That(brokenSorcerer.BrokenTestInterface, Is.Null);

            // Inject TestInterface at brokenSorcerer level
            Mock<IBrokenTestInterface> brokenSorcererTestInterfaceMock = new Mock<IBrokenTestInterface>(MockBehavior.Strict);
            brokenSorcererTestInterfaceMock.SetupGet(x => x.ValueToRetrieve).Returns(222).Verifiable();

            MainInjector.InjectField(brokenSorcererTestInterfaceMock.Object, "brokenTestInterface", brokenSorcerer);

            Assert.That(brokenSorcerer.BrokenTestInterface, Is.Not.Null);
            Assert.That(brokenSorcerer.BrokenTestInterface.ValueToRetrieve, Is.EqualTo(222));
            brokenSorcererTestInterfaceMock.VerifyGet(x => x.ValueToRetrieve, Times.Once);
        }

        [Test]
        public void RecursiveInjectionTests_ShouldInjectScriptableObjectMockWithName_CorrectlyAtThirdLevel()
        {
            // Arrange
            Player player = MainInjector.GetComponent<Player>();
            Sorcerer sorcerer = MainInjector.GetComponent<Sorcerer>(player);
            Assert.That(sorcerer.FirstInventoryBagInfos, Is.Null);
            Assert.That(sorcerer.SecondInventoryBagInfos, Is.Null);

            // Arrange SO
            SO_InventoryBag firstInventoryBagInfos = ScriptableObject.CreateInstance<SO_InventoryBag>();
            firstInventoryBagInfos.BagName = "BigPocket";
            firstInventoryBagInfos.NbSlot = 48;

            // Act
            MainInjector.InjectField(firstInventoryBagInfos, "firstInventoryBagInfos", sorcerer);

            // Assert
            Assert.That(sorcerer.FirstInventoryBagInfos, Is.Not.Null);
            Assert.That(sorcerer.SecondInventoryBagInfos, Is.Null);
            Assert.That(sorcerer.FirstInventoryBagInfos.BagName, Is.EqualTo(firstInventoryBagInfos.BagName));
            Assert.That(sorcerer.FirstInventoryBagInfos.NbSlot, Is.EqualTo(firstInventoryBagInfos.NbSlot));

            Assert.That(MainTested.Player.Sorcerer.FirstInventoryBagInfos.BagName, Is.EqualTo(firstInventoryBagInfos.BagName));
            Assert.That(MainTested.Player.Sorcerer.FirstInventoryBagInfos.NbSlot, Is.EqualTo(firstInventoryBagInfos.NbSlot));
        }
        #endregion InjectFields with name
    }
}
