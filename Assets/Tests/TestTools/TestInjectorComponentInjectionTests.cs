using NixiTestTools;
using NUnit.Framework;
using ScriptExample.Characters;
using ScriptExample.Characters.Broken;
using ScriptExample.Characters.SameNamings;
using ScriptExample.Controllers;
using Tests.Builders;
using UnityEngine;

namespace Tests.TestTools
{
    internal sealed class TestInjectorComponentInjectionTests
    {
        #region ComponentInjection without field name
        [Test]
        public void RetrieveGameObject_ShouldThrowException_WhenTypeIsNotRepresented()
        {
            TestInjector testInjector = SorcererBuilder.Create().BuildTestInjector();

            testInjector.CheckAndInjectAll();

            // BrokenSorcer n'a rien à voir avec Sorcerer
            Assert.Throws<TestInjectorException>(() => testInjector.GetComponent<BrokenSorcerer>());
        }

        [Test]
        public void RetrieveGameObject_ShouldThrowException_WhenTypeHasManyRepresentation()
        {
            TestInjector testInjector = SorcererBuilder.Create().BuildTestInjector();

            testInjector.CheckAndInjectAll();

            // Deux skills trouvés, impossible de différencier
            Assert.Throws<TestInjectorException>(() => testInjector.GetComponent<Skill>());
        }

        [Test]
        public void RetrieveGameObject_ShouldReturnGameObjectWithUniqueSkillOnCharacter()
        {
            Character character = InjectableBuilder<Character>.Create().Build();
            TestInjector testInjector = new TestInjector(character);

            testInjector.CheckAndInjectAll();
            Skill attackSkill = testInjector.GetComponent<Skill>();

            // Attack skill checks
            Assert.That(attackSkill, Is.Not.Null);
            Assert.That(character.AttackSkill, Is.Not.Null);
            Assert.That(character.AttackSkill, Is.EqualTo(attackSkill));
        }

        [Test]
        public void InjectTransform_ShouldInjectCorrectly()
        {
            Weapon weapon = InjectableBuilder<Weapon>.Create().Build();

            TestInjector testInjector = new TestInjector(weapon);
            testInjector.CheckAndInjectAll();

            Transform mainTransform = testInjector.GetComponent<Transform>("MainTransform");
            Transform childTransform = testInjector.GetComponent<Transform>("ChildTransform");

            Assert.That(weapon.MainTransform.GetInstanceID(), Is.EqualTo(mainTransform.GetInstanceID()));
            Assert.That(weapon.ChildTransform.GetInstanceID(), Is.EqualTo(childTransform.GetInstanceID()));
        }
        #endregion ComponentInjection without field name

        #region ComponentInjection with field name
        [Test]
        public void RetrieveGameObjectWithFieldName_ShouldThrowException_WhenTypeIsNotRepresented()
        {
            TestInjector testInjector = SorcererBuilder.Create().BuildTestInjector();

            testInjector.CheckAndInjectAll();

            // BrokenSorcer n'a rien à voir avec Sorcerer
            Assert.Throws<TestInjectorException>(() => testInjector.GetComponent<BrokenSorcerer>("anyName"));
        }

        [Test]
        public void RetrieveGameObjectWithFieldName_ShouldThrowException_WhenTypeIsRepresentedButNotFieldName()
        {
            TestInjector testInjector = SorcererBuilder.Create().BuildTestInjector();

            testInjector.CheckAndInjectAll();

            Assert.Throws<TestInjectorException>(() => testInjector.GetComponent<Skill>("anyName"));
        }

        [Test]
        public void RetrieveGameObjectWithFieldName_ShouldReturnGameObjectWithFieldName()
        {
            Sorcerer sorcerer = SorcererBuilder.Create().Build();
            TestInjector testInjector = new TestInjector(sorcerer);

            testInjector.CheckAndInjectAll();
            Skill attackSkill = testInjector.GetComponent<Skill>("attackSkill");
            Skill magicSkill = testInjector.GetComponent<Skill>("magicSkill");

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
            MonsterController monsterController = InjectableBuilder<MonsterController>.Create().Build();

            TestInjector testInjector = new TestInjector(monsterController, "MonsterController");
            testInjector.CheckAndInjectAll();

            SorcererController sorcererController = testInjector.GetComponent<SorcererController>();

            Assert.AreEqual("MonsterController", monsterController.name);
            Assert.That(monsterController.SorcererController, Is.Not.Null);
            Assert.That(monsterController.SorcererController.GetInstanceID(), Is.EqualTo(sorcererController.GetInstanceID()));

            Assert.AreEqual("SorcererController", sorcererController.name);
            Assert.That(sorcererController.MonsterController, Is.Not.Null);
            Assert.That(sorcererController.MonsterController.GetInstanceID(), Is.EqualTo(monsterController.GetInstanceID()));
        }

        [Test]
        public void RetrieveGameObjectFromChild_ShouldReturnGameObjectFromRoot()
        {
            MonsterController monsterController = InjectableBuilder<MonsterController>.Create().Build();

            TestInjector testInjector = new TestInjector(monsterController, "MonsterController");
            testInjector.CheckAndInjectAll();

            SorcererController sorcererController = testInjector.GetComponent<SorcererController>();

            MonsterController sameMonsterController = testInjector.GetComponent<MonsterController>(sorcererController);
            Assert.That(sameMonsterController, Is.Not.Null);
            Assert.That(sameMonsterController.GetInstanceID(), Is.EqualTo(monsterController.GetInstanceID()));

            SorcererController sameSorcererController = testInjector.GetComponent<SorcererController>(sameMonsterController);
            Assert.That(sameSorcererController, Is.Not.Null);
            Assert.That(sameSorcererController.GetInstanceID(), Is.EqualTo(sorcererController.GetInstanceID()));
        }

        [Test]
        public void RetrieveGameObjectOnLevelAbove_ShouldReturnGameObjectsFromRoots()
        {
            AllMightyController allMighty = InjectableBuilder<AllMightyController>.Create().Build();

            TestInjector testInjector = new TestInjector(allMighty, "AllMightyController");
            testInjector.CheckAndInjectAll();

            SorcererController sorcerer = testInjector.GetComponent<SorcererController>();
            MonsterController monster = testInjector.GetComponent<MonsterController>();

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
            AllMightyControllerFull allMightyFull = InjectableBuilder<AllMightyControllerFull>.Create().Build();

            TestInjector testInjector = new TestInjector(allMightyFull, "AllMightyControllerWithChildren");
            testInjector.CheckAndInjectAll();

            MonsterController monster = testInjector.GetComponent<MonsterController>();
            SorcererController firstSorcerer = testInjector.GetComponent<SorcererController>("FirstChildSorcererController");
            SorcererController secondSorcerer = testInjector.GetComponent<SorcererController>("SecondChildSorcererController");

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
            AllMightyWithChildren mightyChildren = InjectableBuilder<AllMightyWithChildren>.Create().Build();
            TestInjector testInjector = new TestInjector(mightyChildren);
            testInjector.CheckAndInjectAll();

            Assert.That(mightyChildren.FirstSorcerer, Is.Not.Null);
            Assert.That(mightyChildren.SecondSorcerer, Is.Not.Null);

            Assert.That(mightyChildren.FirstSorcerer.MonsterController.GetInstanceID(), Is.EqualTo(mightyChildren.SecondSorcerer.MonsterController.GetInstanceID()));

            Assert.That(mightyChildren.FirstSorcerer.MonsterController.SorcererController.GetInstanceID(), Is.Not.EqualTo(mightyChildren.FirstSorcerer.GetInstanceID()));
            Assert.That(mightyChildren.FirstSorcerer.MonsterController.SorcererController.GetInstanceID(), Is.Not.EqualTo(mightyChildren.SecondSorcerer.GetInstanceID()));

            Assert.That(mightyChildren.FirstSorcerer.GetInstanceID(), Is.Not.EqualTo(mightyChildren.SecondSorcerer.GetInstanceID()));
            Assert.That(mightyChildren.FirstSorcerer.transform.parent.GetInstanceID(), Is.EqualTo(mightyChildren.SecondSorcerer.transform.parent.GetInstanceID()));
        }

        [Test]
        public void SameRootObjectTargetingSameGameObjectName_ShouldReturnSameInstance()
        {
            RussianDollSameLevel russianDollSameLevel = InjectableBuilder<RussianDollSameLevel>.Create().Build();

            TestInjector testInjector = new TestInjector(russianDollSameLevel);

            testInjector.CheckAndInjectAll();

            Assert.That(russianDollSameLevel.Doll.GetInstanceID(), Is.EqualTo(russianDollSameLevel.SecondDoll.GetInstanceID()));
        }

        [Test]
        public void SameRootObjectTargeting_ShouldReturnSameInstance()
        {
            RussianDollSameLevelParent russianDollSameLevelParent = InjectableBuilder<RussianDollSameLevelParent>.Create().Build();

            TestInjector testInjector = new TestInjector(russianDollSameLevelParent);
            testInjector.CheckAndInjectAll();

            FirstDoll firstDoll = testInjector.GetComponent<FirstDoll>("FirstDoll");
            FirstDoll firstDollDuplicate = testInjector.GetComponent<FirstDoll>("FirstDollDuplicate");

            Assert.That(firstDoll.GetInstanceID(), Is.EqualTo(firstDollDuplicate.GetInstanceID()));
        }

        [Test]
        public void SameRootObjectTargetingSameGameObjectName_ShouldLoadAndReturnSameGameObjects()
        {
            AllMightyWithSameChildren sameMightyChildren = InjectableBuilder<AllMightyWithSameChildren>.Create().Build();

            TestInjector testInjector = new TestInjector(sameMightyChildren);
            testInjector.CheckAndInjectAll();

            Assert.That(sameMightyChildren.FirstSorcerer, Is.Not.Null);
            Assert.That(sameMightyChildren.FirstSorcererDuplicate, Is.Not.Null);

            Assert.That(sameMightyChildren.FirstSorcerer.GetInstanceID(), Is.EqualTo(sameMightyChildren.FirstSorcererDuplicate.GetInstanceID()));
        }
        #endregion ComponentInjection from root
    }
}
