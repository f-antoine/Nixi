using Assets.ScriptExample.Characters;
using Assets.ScriptExample.Characters.SameNamings;
using Assets.ScriptExample.Controllers;
using Assets.Tests.Builders;
using Moq;
using Nixi.Containers;
using NixiTestTools;
using NUnit.Framework;
using ScriptExample.Characters;
using ScriptExample.Characters.Broken;
using ScriptExample.Characters.ScriptableObjects;
using ScriptExample.Containers;
using ScriptExample.Containers.Broken;
using ScriptExample.Players;
using System;
using System.Collections;
using Tests.Builders;
using UnityEngine;

namespace Tests.TestTools
{
    internal sealed class TestInjecterTests
    {
        [SetUp]
        public void InitTests()
        {
            NixiContainer.Remove<ITestInterface>();
            NixiContainer.MapSingle<ITestInterface, TestImplementation>();
        }

        [Test]
        public void InjectOnRecursiveInjection_ShouldThrowException()
        {
            InfiniteRecursionSorcerer infiniteRecursionSorcerer = new GameObject().AddComponent<InfiniteRecursionSorcerer>();

            TestInjecter testInjecter = new TestInjecter(infiniteRecursionSorcerer);

            Assert.Throws<StackOverflowException>(() => infiniteRecursionSorcerer.BuildInjections(testInjecter));
        }

        [Test]
        public void InjectOnRecursiveInjectionWithInheritance_ShouldThrowException()
        {
            InfiniteRecursionSorcererWithInheritance infiniteRecursionSorcererWithInheritance = new GameObject().AddComponent<InfiniteRecursionSorcererWithInheritance>();

            TestInjecter testInjecter = new TestInjecter(infiniteRecursionSorcererWithInheritance);

            Assert.Throws<StackOverflowException>(() => infiniteRecursionSorcererWithInheritance.BuildInjections(testInjecter));
        }

        [Test]
        public void SameInstanceNameAndType_ShouldReturnSameInstance()
        {
            RussianDoll russianDoll = RussianDollBuilder.Create().Build();

            TestInjecter testInjecter = new TestInjecter(russianDoll);
            testInjecter.CheckAndInjectAll();

            Assert.That(russianDoll.ChildDoll.GetInstanceID(), Is.EqualTo(russianDoll.ChildDoll2.GetInstanceID()));
        }

        [Test]
        public void InjectField_WithDoesNotFillButExposeForTesting_ShouldNotFillField_ButExposeIt()
        {
            // Init
            Warrior warrior = WarriorBuilder.Create().Build();
            Assert.That(warrior.Parasite, Is.Null);

            // Inject
            TestInjecter testInjecter = new TestInjecter(warrior);
            testInjecter.CheckAndInjectAll();

            // Verify not injected because marked with DoesNotFillButExposeForTesting
            Assert.That(warrior.Parasite, Is.Null);

            // Check not a component registered but a field
            Assert.Throws<TestInjecterException>(() => testInjecter.GetComponent<Warrior>());

            Parasite parasite = ParasiteBuilder.Create().Build();
            testInjecter.InjectMock(parasite);

            Assert.That(warrior.Parasite, Is.Not.Null);
            Assert.That(warrior.Parasite.GetInstanceID(), Is.EqualTo(parasite.GetInstanceID()));
        }

        #region FieldInjection : InjectMock without field name
        [Test]
        public void InjectMockWithoutName_ShouldThrowException_WhenIsNotInterface()
        {
            TestInjecter testInjecter = SorcererBuilder.Create().BuildTestInjecter();

            testInjecter.CheckAndInjectAll();

            Assert.Throws<TestInjecterException>(() => testInjecter.InjectMock(new Mock<TestImplementation>().Object));
        }

        [Test]
        public void InjectMockWithoutName_ShouldThrowException_WhenInterfaceNotReferencedInClass()
        {
            TestInjecter testInjecter = SorcererBuilder.Create().BuildTestInjecter();

            testInjecter.CheckAndInjectAll();

            Assert.Throws<TestInjecterException>(() => testInjecter.InjectMock(new Mock<IList>().Object));
        }

        [Test]
        public void InjectMockWithoutName_ShouldThrowException_WhenTwoSameInterfaceAndNoNameToDefine()
        {
            TestInjecter testInjecter = BrokenSorcererBuilder.Create().BuildTestInjecter();

            testInjecter.CheckAndInjectAll();

            Assert.Throws<TestInjecterException>(() => testInjecter.InjectMock(new Mock<IBrokenTestInterface>().Object));
        }

        [Test]
        public void InjectMockWithoutName_ShouldFill()
        {
            // Init
            Sorcerer sorcerer = SorcererBuilder.Create().Build();
            Assert.That(sorcerer.TestInterface, Is.Null);

            // Inject
            TestInjecter testInjecter = new TestInjecter(sorcerer);
            testInjecter.CheckAndInjectAll();

            // Mock
            Mock<ITestInterface> testMock = new Mock<ITestInterface>(MockBehavior.Strict);
            testMock.SetupGet(x => x.ValueToRetrieve).Returns(4).Verifiable();
            testInjecter.InjectMock(testMock.Object);

            // Asserts
            Assert.That(sorcerer.TestInterface, Is.Not.Null);
            Assert.That(sorcerer.TestInterface.ValueToRetrieve, Is.EqualTo(4));
            testMock.VerifyGet(x => x.ValueToRetrieve, Times.Once);
        }

        [Test]
        public void InjectScriptableObjectMockWithoutName_ShouldFill()
        {
            // Init
            Sorcerer sorcerer = SorcererBuilder.Create().Build();
            Assert.That(sorcerer.SOInfos, Is.Null);

            // Inject
            TestInjecter testInjecter = new TestInjecter(sorcerer);
            testInjecter.CheckAndInjectAll();

            // Mock
            SO_Sorcerer soInfos = ScriptableObject.CreateInstance<SO_Sorcerer>();
            soInfos.CharaName = "SorcererCharaName";
            soInfos.ManaMax = 2000;
            testInjecter.InjectMock(soInfos);

            // Asserts
            Assert.That(sorcerer.SOInfos, Is.Not.Null);
            Assert.That(sorcerer.SOInfos.CharaName, Is.EqualTo(soInfos.CharaName));
            Assert.That(sorcerer.SOInfos.ManaMax, Is.EqualTo(soInfos.ManaMax));
        }
        #endregion FieldInjection : InjectMock without field name

        #region FieldInjection : InjectMock with field name
        [Test]
        public void InjectMockWithName_ShouldThrowException_WhenIsNotInterface()
        {
            TestInjecter testInjecter = SorcererBuilder.Create().BuildTestInjecter();

            testInjecter.CheckAndInjectAll();

            Assert.Throws<TestInjecterException>(() => testInjecter.InjectMock(new Mock<TestImplementation>().Object, "anyName"));
        }

        [Test]
        public void InjectMockWithName_ShouldThrowException_WhenInterfaceNotReferencedInClass()
        {
            TestInjecter testInjecter = SorcererBuilder.Create().BuildTestInjecter();

            testInjecter.CheckAndInjectAll();

            Assert.Throws<TestInjecterException>(() => testInjecter.InjectMock(new Mock<IList>().Object, "anyName"));
        }

        [Test]
        public void InjectMockWithName_ShouldThrowException_WhenInterfaceReferencedWithBadNameInClass()
        {
            TestInjecter testInjecter = SorcererBuilder.Create().BuildTestInjecter();

            testInjecter.CheckAndInjectAll();

            Assert.Throws<TestInjecterException>(() => testInjecter.InjectMock(new Mock<ITestInterface>().Object, "anyName"));
        }

        [Test]
        public void InjectMockWithName_ShouldFill()
        {
            // Init
            Sorcerer sorcerer = SorcererBuilder.Create().Build();
            Assert.That(sorcerer.TestInterface, Is.Null);

            // Inject
            TestInjecter testInjecter = new TestInjecter(sorcerer);
            testInjecter.CheckAndInjectAll();

            // Mock
            Mock<ITestInterface> testMock = new Mock<ITestInterface>(MockBehavior.Strict);
            testMock.SetupGet(x => x.ValueToRetrieve).Returns(4).Verifiable();
            testInjecter.InjectMock(testMock.Object, "testInterface");

            // Asserts
            Assert.That(sorcerer.TestInterface, Is.Not.Null);
            Assert.That(sorcerer.TestInterface.ValueToRetrieve, Is.EqualTo(4));
            testMock.VerifyGet(x => x.ValueToRetrieve, Times.Once);
        }

        [Test]
        public void InjectMockWithName_ShouldFillMoreComplicated()
        {
            // Init
            BrokenSorcerer brokenSorcerer = BrokenSorcererBuilder.Create().Build();
            Assert.That(brokenSorcerer.BrokenTestInterface, Is.Null);
            Assert.That(brokenSorcerer.BrokenTestInterfaceSecond, Is.Null);

            // Inject
            TestInjecter testInjecter = new TestInjecter(brokenSorcerer);
            testInjecter.CheckAndInjectAll();

            // Mock First
            Mock<IBrokenTestInterface> testMock = new Mock<IBrokenTestInterface>(MockBehavior.Strict);
            testMock.SetupGet(x => x.ValueToRetrieve).Returns(1).Verifiable();
            testInjecter.InjectMock(testMock.Object, "brokenTestInterface");

            // Mock Second
            Mock<IBrokenTestInterface> testMockSecond = new Mock<IBrokenTestInterface>(MockBehavior.Strict);
            testMockSecond.SetupGet(x => x.ValueToRetrieve).Returns(2).Verifiable();
            testInjecter.InjectMock(testMockSecond.Object, "brokenTestInterfaceSecond");

            // Asserts First
            Assert.That(brokenSorcerer.BrokenTestInterface, Is.Not.Null);
            Assert.That(brokenSorcerer.BrokenTestInterface.ValueToRetrieve, Is.EqualTo(1));
            testMock.VerifyGet(x => x.ValueToRetrieve, Times.Once);

            // Asserts Second
            Assert.That(brokenSorcerer.BrokenTestInterfaceSecond, Is.Not.Null);
            Assert.That(brokenSorcerer.BrokenTestInterfaceSecond.ValueToRetrieve, Is.EqualTo(2));
            testMockSecond.VerifyGet(x => x.ValueToRetrieve, Times.Once);
        }

        [Test]
        public void InjectScriptableObjectMock_ShouldThrowWhenTwoSameScriptableObjectType()
        {
            // Init
            Sorcerer sorcerer = SorcererBuilder.Create().Build();

            // Inject
            TestInjecter testInjecter = new TestInjecter(sorcerer);
            testInjecter.CheckAndInjectAll();

            // Mock
            SO_InventoryBag soInfosBag = ScriptableObject.CreateInstance<SO_InventoryBag>();
            
            // Assert
            Assert.Throws<TestInjecterException>(() => testInjecter.InjectMock(soInfosBag));
        }

        [Test]
        public void InjectScriptableObjectMockName_ShouldFill()
        {
            // Init
            Sorcerer sorcerer = SorcererBuilder.Create().Build();
            Assert.That(sorcerer.FirstInventoryBagInfos, Is.Null);
            Assert.That(sorcerer.SecondInventoryBagInfos, Is.Null);

            // Inject
            TestInjecter testInjecter = new TestInjecter(sorcerer);
            testInjecter.CheckAndInjectAll();

            // Mock
            SO_InventoryBag soInfosBag = ScriptableObject.CreateInstance<SO_InventoryBag>();
            soInfosBag.BagName = "Pocket";
            soInfosBag.NbSlot = 14;
            testInjecter.InjectMock(soInfosBag, "firstInventoryBagInfos");

            // Asserts
            Assert.That(sorcerer.FirstInventoryBagInfos, Is.Not.Null);
            Assert.That(sorcerer.SecondInventoryBagInfos, Is.Null);
            Assert.That(sorcerer.FirstInventoryBagInfos.BagName, Is.EqualTo(soInfosBag.BagName));
            Assert.That(sorcerer.FirstInventoryBagInfos.NbSlot, Is.EqualTo(soInfosBag.NbSlot));
        }

        [Test]
        public void InjectScriptableObjectMockName_ShouldThrowException_WhenTypeIsRepresentedButNotFieldName()
        {
            Player player = PlayerBuilder.Create().Build();

            TestInjecter testInjecter = new TestInjecter(player);
            testInjecter.CheckAndInjectAll();

            Sorcerer sorcerer = testInjecter.GetComponent<Sorcerer>();

            Assert.That(sorcerer, Is.Not.Null);

            Skill skillMock = new GameObject().AddComponent<Skill>();
            Assert.Throws<TestInjecterException>(() => testInjecter.InjectMockIntoChildInjected(skillMock, sorcerer, "anyName"));
        }
        #endregion FieldInjection : InjectMock with field name

        #region GameObjectInjection without field name
        [Test]
        public void RetrieveGameObject_ShouldThrowException_WhenTypeIsNotRepresented()
        {
            TestInjecter testInjecter = SorcererBuilder.Create().BuildTestInjecter();

            testInjecter.CheckAndInjectAll();

            // BrokenSorcer n'a rien à voir avec Sorcerer
            Assert.Throws<TestInjecterException>(() => testInjecter.GetComponent<BrokenSorcerer>());
        }

        [Test]
        public void RetrieveGameObject_ShouldThrowException_WhenTypeHasManyRepresentation()
        {
            TestInjecter testInjecter = SorcererBuilder.Create().BuildTestInjecter();

            testInjecter.CheckAndInjectAll();

            // Deux skills trouvés, impossible de différencier
            Assert.Throws<TestInjecterException>(() => testInjecter.GetComponent<Skill>());
        }

        [Test]
        public void RetrieveGameObject_ShouldReturnGameObjectWithUniqueSkillOnCharacter()
        {
            Character character = CharacterBuilder.Create().Build();
            TestInjecter testInjecter = new TestInjecter(character);

            testInjecter.CheckAndInjectAll();
            Skill attackSkill = testInjecter.GetComponent<Skill>();

            // Attack skill checks
            Assert.That(attackSkill, Is.Not.Null);
            Assert.That(character.AttackSkill, Is.Not.Null);
            Assert.That(character.AttackSkill, Is.EqualTo(attackSkill));
        }
        #endregion GameObjectInjection without field name

        #region GameObjectInjection with field name
        [Test]
        public void RetrieveGameObjectWithFieldName_ShouldThrowException_WhenTypeIsNotRepresented()
        {
            TestInjecter testInjecter = SorcererBuilder.Create().BuildTestInjecter();

            testInjecter.CheckAndInjectAll();

            // BrokenSorcer n'a rien à voir avec Sorcerer
            Assert.Throws<TestInjecterException>(() => testInjecter.GetComponent<BrokenSorcerer>("anyName"));
        }

        [Test]
        public void RetrieveGameObjectWithFieldName_ShouldThrowException_WhenTypeIsRepresentedButNotFieldName()
        {
            TestInjecter testInjecter = SorcererBuilder.Create().BuildTestInjecter();

            testInjecter.CheckAndInjectAll();

            Assert.Throws<TestInjecterException>(() => testInjecter.GetComponent<Skill>("anyName"));
        }

        [Test]
        public void RetrieveGameObjectWithFieldName_ShouldReturnGameObjectWithFieldName()
        {
            Sorcerer sorcerer = SorcererBuilder.Create().Build();
            TestInjecter testInjecter = new TestInjecter(sorcerer);

            testInjecter.CheckAndInjectAll();
            Skill attackSkill = testInjecter.GetComponent<Skill>("attackSkill");
            Skill magicSkill = testInjecter.GetComponent<Skill>("magicSkill");

            // Attack skill checks
            Assert.That(attackSkill, Is.Not.Null);
            Assert.That(sorcerer.AttackSkill, Is.Not.Null);
            Assert.That(sorcerer.AttackSkill.GetInstanceID(), Is.EqualTo(attackSkill.GetInstanceID()));

            // Magic skill checks
            Assert.That(magicSkill, Is.Not.Null);
            Assert.That(sorcerer.MagicSkill, Is.Not.Null);
            Assert.That(sorcerer.MagicSkill.GetInstanceID(), Is.EqualTo(magicSkill.GetInstanceID()));

            // Both return are differents
            Assert.That(sorcerer.MagicSkill.GetInstanceID(), Is.Not.EqualTo(attackSkill.GetInstanceID()));
            Assert.That(sorcerer.AttackSkill.GetInstanceID(), Is.Not.EqualTo(magicSkill.GetInstanceID()));
        }
        #endregion GameObjectInjection with field name

        #region GameObjectInjection from root
        [Test]
        public void RetrieveGameObject_ShouldReturnGameObjectFromRoot()
        {
            MonsterController monsterController = ControllersBuilder.Create().BuildMonsterController();

            TestInjecter testInjecter = new TestInjecter(monsterController, "MonsterController");
            testInjecter.CheckAndInjectAll();

            SorcererController sorcererController = testInjecter.GetComponent<SorcererController>();

            Assert.That(monsterController.SorcererController, Is.Not.Null);
            Assert.That(monsterController.SorcererController.GetInstanceID(), Is.EqualTo(sorcererController.GetInstanceID()));

            Assert.That(sorcererController.MonsterController, Is.Not.Null);
            Assert.That(sorcererController.MonsterController.GetInstanceID(), Is.EqualTo(monsterController.GetInstanceID()));
        }

        [Test]
        public void RetrieveGameObjectFromChild_ShouldReturnGameObjectFromRoot()
        {
            MonsterController monsterController = ControllersBuilder.Create().BuildMonsterController();

            TestInjecter testInjecter = new TestInjecter(monsterController, "MonsterController");
            testInjecter.CheckAndInjectAll();

            SorcererController sorcererController = testInjecter.GetComponent<SorcererController>();

            MonsterController sameMonsterController = testInjecter.GetComponentFromChildInjected<MonsterController>(sorcererController);
            Assert.That(sameMonsterController, Is.Not.Null);
            Assert.That(sameMonsterController.GetInstanceID(), Is.EqualTo(monsterController.GetInstanceID()));

            SorcererController sameSorcererController = testInjecter.GetComponentFromChildInjected<SorcererController>(sameMonsterController);
            Assert.That(sameSorcererController, Is.Not.Null);
            Assert.That(sameSorcererController.GetInstanceID(), Is.EqualTo(sorcererController.GetInstanceID()));
        }

        [Test]
        public void RetrieveGameObjectOnLevelAbove_ShouldReturnGameObjectsFromRoots()
        {
            AllMightyController allMighty = ControllersBuilder.Create().BuildAllMightyController();

            TestInjecter testInjecter = new TestInjecter(allMighty, "AllMightyController");
            testInjecter.CheckAndInjectAll();

            SorcererController sorcerer = testInjecter.GetComponent<SorcererController>();
            MonsterController monster = testInjecter.GetComponent<MonsterController>();

            Assert.That(allMighty.AllMightySorcererController, Is.Not.Null);
            Assert.That(allMighty.AllMightySorcererController.GetInstanceID(), Is.EqualTo(sorcerer.GetInstanceID()));

            Assert.That(allMighty.AllMightyMonsterController, Is.Not.Null);
            Assert.That(allMighty.AllMightyMonsterController.GetInstanceID(), Is.EqualTo(monster.GetInstanceID()));

            Assert.That(allMighty.AllMightyMonsterController.SorcererController.GetInstanceID(), Is.EqualTo(allMighty.AllMightySorcererController.GetInstanceID()));
            Assert.That(allMighty.AllMightySorcererController.MonsterController.GetInstanceID(), Is.EqualTo(allMighty.AllMightyMonsterController.GetInstanceID()));
        }

        [Test]
        public void RetrieveGameObjectFromMethod_ShouldReturnGameObjectFromRoot()
        {
            AllMightyControllerFull allMightyFull = ControllersBuilder.Create().BuildAllMightyControllerFull();

            TestInjecter testInjecter = new TestInjecter(allMightyFull, "AllMightyControllerWithChilds");
            testInjecter.CheckAndInjectAll();

            MonsterController monster = testInjecter.GetComponent<MonsterController>();
            SorcererController firstSorcerer = testInjecter.GetComponent<SorcererController>("FirstChildSorcererController");
            SorcererController secondSorcerer = testInjecter.GetComponent<SorcererController>("SecondChildSorcererController");

            // Check case where fieldName not equal to GameObjectName
            Assert.That(monster.name, Is.EqualTo("MonsterController"));
            Assert.That(firstSorcerer.name, Is.EqualTo("TheFirstChildSorcererController"));
            Assert.That(secondSorcerer.name, Is.EqualTo("TheSecondChildSorcererController"));

            Assert.That(monster.SorcererController, Is.Not.Null);
            Assert.That(firstSorcerer.MonsterController, Is.Not.Null);
            Assert.That(secondSorcerer.MonsterController, Is.Not.Null);

            // Check differents SorcererConstroller between the from root and from method
            Assert.That(monster.SorcererController.GetInstanceID(), Is.Not.EqualTo(firstSorcerer.GetInstanceID()));
            Assert.That(monster.SorcererController.GetInstanceID(), Is.Not.EqualTo(secondSorcerer.GetInstanceID()));

            // Check from root is all same (current or child level)
            Assert.That(firstSorcerer.MonsterController.GetInstanceID(), Is.EqualTo(monster.GetInstanceID()));
            Assert.That(secondSorcerer.MonsterController.GetInstanceID(), Is.EqualTo(monster.GetInstanceID()));
        }

        [Test]
        public void SameRootObjectTargetingDifferentGameObjectName_ShouldLoadAndReturnDifferentGameObjects()
        {
            AllMightyWithChilds mightyChilds = ControllersBuilder.Create().BuildAllMightyWithChilds();

            TestInjecter testInjecter = new TestInjecter(mightyChilds);

            testInjecter.CheckAndInjectAll();

            Assert.That(mightyChilds.FirstSorcerer, Is.Not.Null);
            Assert.That(mightyChilds.SecondSorcerer, Is.Not.Null);

            Assert.That(mightyChilds.FirstSorcerer.GetInstanceID(), Is.Not.EqualTo(mightyChilds.SecondSorcerer.GetInstanceID()));
        }

        [Test]
        public void SameRootObjectTargetingSameGameObjectName_ShouldReturnSameInstance()
        {
            RussianDollSameLevel russianDollSameLevel = RussianDollBuilder.Create().BuildSameLevel();

            TestInjecter testInjecter = new TestInjecter(russianDollSameLevel);

            testInjecter.CheckAndInjectAll();

            Assert.That(russianDollSameLevel.Doll.GetInstanceID(), Is.EqualTo(russianDollSameLevel.SecondDoll.GetInstanceID()));
        }

        [Test]
        public void SameRootObjectTargeting_ShouldReturnSameInstance()
        {
            RussianDollSameLevelParent russianDollSameLevelParent = RussianDollBuilder.Create().BuildSameLevelParent();

            TestInjecter testInjecter = new TestInjecter(russianDollSameLevelParent);
            testInjecter.CheckAndInjectAll();

            FirstDoll firstDoll = testInjecter.GetComponent<FirstDoll>("FirstDoll");
            FirstDoll firstDollDuplicate = testInjecter.GetComponent<FirstDoll>("FirstDollDuplicate");

            Assert.That(firstDoll.GetInstanceID(), Is.EqualTo(firstDollDuplicate.GetInstanceID()));
        }

        [Test]
        public void SameRootObjectTargetingSameGameObjectName_ShouldLoadAndReturnSameGameObjects()
        {
            AllMightyWithSameChilds sameMightyChilds = ControllersBuilder.Create().BuildAllMightyWithSameChilds();

            TestInjecter testInjecter = new TestInjecter(sameMightyChilds);
            testInjecter.CheckAndInjectAll();

            Assert.That(sameMightyChilds.FirstSorcerer, Is.Not.Null);
            Assert.That(sameMightyChilds.FirstSorcererDuplicate, Is.Not.Null);

            Assert.That(sameMightyChilds.FirstSorcerer.GetInstanceID(), Is.EqualTo(sameMightyChilds.FirstSorcererDuplicate.GetInstanceID()));
        }
        #endregion GameObjectInjection from root
    }
}