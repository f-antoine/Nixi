using Assets.ScriptExample.Characters;
using Assets.ScriptExample.Characters.SameNamings;
using Assets.ScriptExample.Controllers;
using NixiTestTools;
using NUnit.Framework;
using ScriptExample.Characters;
using ScriptExample.Characters.Broken;
using Tests.Builders;
using UnityEngine;

namespace Tests.TestTools
{
    internal sealed class TestInjecterComponentInjectionTests
    {
        #region ComponentInjection without field name
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

        [Test]
        public void InjectTransform_ShouldInjectCorrectly()
        {
            Weapon weapon = WeaponBuilder.Create().Build();

            TestInjecter testInjecter = new TestInjecter(weapon);
            testInjecter.CheckAndInjectAll();

            Transform mainTransform = testInjecter.GetComponent<Transform>("MainTransform");
            Transform childTransform = testInjecter.GetComponent<Transform>("ChildTransform");

            Assert.That(weapon.MainTransform.GetInstanceID(), Is.EqualTo(mainTransform.GetInstanceID()));
            Assert.That(weapon.ChildTransform.GetInstanceID(), Is.EqualTo(childTransform.GetInstanceID()));
        }
        #endregion ComponentInjection without field name

        #region ComponentInjection with field name
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
        #endregion ComponentInjection with field name

        #region ComponentInjection from root
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

            MonsterController sameMonsterController = testInjecter.GetComponent<MonsterController>(sorcererController);
            Assert.That(sameMonsterController, Is.Not.Null);
            Assert.That(sameMonsterController.GetInstanceID(), Is.EqualTo(monsterController.GetInstanceID()));

            SorcererController sameSorcererController = testInjecter.GetComponent<SorcererController>(sameMonsterController);
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

            Assert.That(mightyChilds.FirstSorcerer.MonsterController.GetInstanceID(), Is.EqualTo(mightyChilds.SecondSorcerer.MonsterController.GetInstanceID()));

            Assert.That(mightyChilds.FirstSorcerer.MonsterController.SorcererController.GetInstanceID(), Is.Not.EqualTo(mightyChilds.FirstSorcerer.GetInstanceID()));
            Assert.That(mightyChilds.FirstSorcerer.MonsterController.SorcererController.GetInstanceID(), Is.Not.EqualTo(mightyChilds.SecondSorcerer.GetInstanceID()));

            Assert.That(mightyChilds.FirstSorcerer.GetInstanceID(), Is.Not.EqualTo(mightyChilds.SecondSorcerer.GetInstanceID()));
            Assert.That(mightyChilds.FirstSorcerer.transform.parent.GetInstanceID(), Is.EqualTo(mightyChilds.SecondSorcerer.transform.parent.GetInstanceID()));
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
        #endregion ComponentInjection from root

        #region ComponentInjection From Method
        [Test]
        public void InjectComponentFromChildMethod_ShouldNotReturnCurrentLevelComponent()
        {
            // CurrentLevel = MyName, si on le trouve, on le renvoi pas, on en crée un, on doit vérifier que le local est différent
        }

        [Test]
        public void InjectComponentFromParentMethod_ShouldNotReturnCurrentLevelComponent()
        {
            // CurrentLevel = MyName, si on le trouve, on le renvoi pas, on en crée un, on doit vérifier que le local est différent
        }

        [Test]
        public void InjectComponentFromChildMethod_WithSameNameAndSameType_AtSameLevelShouldReturn_SameInstances()
        {
            // Title say it
            // + Check if subInstance are same
        }

        [Test]
        public void InjectComponentFromChildMethod_WithSameNameAndDifferentType_AtSameLevelShouldReturn_DifferentInstancesWithSameName()
        {
            // Title say it
            // + Check if subInstance are same
        }

        [Test]
        public void InjectComponentFromParentMethod_WithSameNameAndSameType_AtSameLevelShouldReturn_SameInstances()
        {
            // Title say it
            // + Check if subInstance are same
        }

        [Test]
        public void InjectComponentFromParentMethod_WithSameNameAndDifferentType_AtSameLevelShouldReturn_DifferentInstancesWithSameName()
        {
            // Title say it
            // + Check if subInstance are same
        }

        // Si même GameObjectMethod avec même nom, on a la même instance, sinon on la crée et enregistre
        [Test]
        public void InjectComponentFrom_DifferentMethods_OnSameNameAndTypeAtSameLevelShouldReturn_DifferentInstances()
        {
            // Check instances are not same, even if this is same type and same name
            // -> Case same name / same type / different methods
            // -> Case same name / different type 
        }
        #endregion ComponentInjection From Method
    }
}
