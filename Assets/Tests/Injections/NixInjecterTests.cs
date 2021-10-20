using Assets.ScriptExample.Audio;
using Assets.ScriptExample.Characters;
using Assets.ScriptExample.Menu;
using Assets.Tests.Builders;
using Nixi.Containers;
using Nixi.Injections.Injecters;
using NUnit.Framework;
using ScriptExample.Characters;
using ScriptExample.Containers;
using Tests.Builders;
using UnityEngine;
using UnityEngine.UI;

namespace Tests.Injections
{
    internal sealed class NixInjecterTests
    {
        [SetUp]
        public void InitTests()
        {
            NixiContainer.Remove<ITestInterface>();
            NixiContainer.MapSingle<ITestInterface, TestImplementation>();
        }

        #region InjectFields
        [Test]
        public void InjectField_ShouldThrowException_WhenInterfaceNotMapped()
        {
            NixiContainer.Remove<ITestInterface>();

            NixInjecter nixInjecter = SorcererBuilder.Create().WithSkill().WithChildSkill().BuildNixInjecter();

            Assert.Throws<NixiContainerException>(() => nixInjecter.CheckAndInjectAll());
        }

        [Test]
        public void InjectField_ShouldFillField_WithCorrectInstance()
        {
            // Init
            Sorcerer sorcerer = SorcererBuilder.Create().WithSkill().WithChildSkill().Build();
            Assert.That(sorcerer.TestInterface, Is.Null);

            // Inject
            NixInjecter nixInjecter = new NixInjecter(sorcerer);
            nixInjecter.CheckAndInjectAll();

            // Retrieve interface
            ITestInterface element = NixiContainer.Resolve<ITestInterface>();
            element.ValueToRetrieve = 4;

            // Verify implementation setted and updated
            Assert.That(sorcerer.TestInterface, Is.Not.Null);
            Assert.That(sorcerer.TestInterface.ValueToRetrieve, Is.EqualTo(4));
        }

        [Test]
        public void InjectField_WithNoIsInjectedFromContainer_ShouldNotFillField_WithCorrectInstance()
        {
            // Init
            Sorcerer sorcerer = SorcererBuilder.Create().WithSkill().WithChildSkill().Build();
            Assert.That(sorcerer.SOInfos, Is.Null);

            // Inject
            NixInjecter nixInjecter = new NixInjecter(sorcerer);
            nixInjecter.CheckAndInjectAll();

            // Verify not injected because marked with IsInjectedFromContainer
            Assert.That(sorcerer.SOInfos, Is.Null);
        }
        #endregion InjectFields

        #region GameObjectInjection From MonoBehaviourInjectable
        [Test]
        public void InjectAll_ShouldThrowException_WhenSkillIsNot_OnCurrentElement()
        {
            NixInjecter nixInjecter = SorcererBuilder.Create().WithChildSkill().BuildNixInjecter();
            Assert.Throws<NixInjecterException>(() => nixInjecter.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_ShouldThrowException_WhenSkillIsNot_OnChildElement()
        {
            NixInjecter nixInjecter = SorcererBuilder.Create().WithSkill().BuildNixInjecter();
            Assert.Throws<NixInjecterException>(() => nixInjecter.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_ShouldThrowException_WhenTwoSkill_OnCurrentElement()
        {
            // Two skill added at same level
            NixInjecter nixInjecter = SorcererBuilder.Create().WithSkill().WithSkill().WithChildSkill().BuildNixInjecter();
            Assert.Throws<NixInjecterException>(() => nixInjecter.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_ShouldThrowException_WhenTwoSkill_OnChildElement()
        {
            // Two skills added at same level
            NixInjecter nixInjecter = SorcererBuilder.Create().WithSkill().WithChildSkill().WithChildSkill().BuildNixInjecter();
            Assert.Throws<NixInjecterException>(() => nixInjecter.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_ShouldThrowException_WhenSkillWithNameDoesNotExist_OnChildElement()
        {
            NixInjecter nixInjecter = SorcererBuilder.Create().WithSkill().WithChildSkill("NotFoundable").BuildNixInjecter();
            Assert.Throws<NixInjecterException>(() => nixInjecter.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_ShouldFillGameObjectFields_WithCorrectInstances()
        {
            // Init
            Sorcerer sorcerer = SorcererBuilder.Create().WithSkill().WithChildSkill().Build();
            Assert.That(sorcerer.AttackSkill, Is.Null);
            Assert.That(sorcerer.MagicSkill, Is.Null);

            // Inject
            NixInjecter nixInjecter = new NixInjecter(sorcerer);
            nixInjecter.CheckAndInjectAll();

            // Verify GameObjects setted
            Assert.That(sorcerer.AttackSkill, Is.Not.Null);
            Assert.That(sorcerer.AttackSkill.name, Is.EqualTo("SorcererGameObjectName"));

            Assert.That(sorcerer.MagicSkill, Is.Not.Null);
            Assert.That(sorcerer.MagicSkill.name, Is.EqualTo("SorcererChildGameObjectName"));
        }

        [Test]
        public void InjectAll_ShouldThrowException_WhenSorcerer_IsNotParent()
        {
            NixInjecter nixInjecter = ParasiteBuilder.Create().BuildDefaultInjecter();
            Assert.Throws<NixInjecterException>(() => nixInjecter.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_ShouldFillParasiteGameObjectField_WhenSorcerer_IsParent()
        {
            // Init
            Parasite parasite = ParasiteBuilder.Create().WithParentSorcerer().Build();

            Assert.That(parasite.ParentSorcerer, Is.Null);

            // Inject
            NixInjecter nixInjecter = new NixInjecter(parasite);
            nixInjecter.CheckAndInjectAll();

            // Verify GameObjects setted
            Assert.That(parasite.ParentSorcerer, Is.Not.Null);
        }

        #region Inactive versus Active
        [Test]
        public void InjectAllFromChildren_ShouldFillFieldsWhenAllActive()
        {
            // Init
            AudioController audioController = AudioControllerBuilder.Create().AddEmptyGameObjectLevel()
                                                                             .AddSliderGameObjectLevel("SliderMusic")
                                                                             .AddEmptyGameObjectLevel()
                                                                             .AddSliderGameObjectLevel("SliderSpatialisation")
                                                                             .Build();

            Assert.That(audioController.musicSlider, Is.Null);
            Assert.That(audioController.spatialisationSlider, Is.Null);

            // Inject
            NixInjecter nixInjecter = new NixInjecter(audioController);
            nixInjecter.CheckAndInjectAll();

            // Verify GameObjects setted
            Assert.That(audioController.musicSlider, Is.Not.Null);
            Assert.That(audioController.musicSlider.IsActive(), Is.True);

            Assert.That(audioController.spatialisationSlider, Is.Not.Null);
            Assert.That(audioController.spatialisationSlider.IsActive(), Is.True);
        }

        [Test]
        public void InjectAllFromChildren_ShouldFillFieldsWhenBothInactive()
        {
            // Init
            AudioController audioController = AudioControllerBuilder.Create().AddEmptyGameObjectLevel()
                                                                             .AddSliderGameObjectLevel("SliderMusic", false)
                                                                             .AddEmptyGameObjectLevel()
                                                                             .AddSliderGameObjectLevel("SliderSpatialisation", false)
                                                                             .Build();

            Assert.That(audioController.musicSlider, Is.Null);
            Assert.That(audioController.spatialisationSlider, Is.Null);

            // Inject
            NixInjecter nixInjecter = new NixInjecter(audioController);
            nixInjecter.CheckAndInjectAll();

            // Verify GameObjects setted
            Assert.That(audioController.musicSlider, Is.Not.Null);
            Assert.That(audioController.musicSlider.IsActive(), Is.False);

            Assert.That(audioController.spatialisationSlider, Is.Not.Null);
            Assert.That(audioController.spatialisationSlider.IsActive(), Is.False);
        }

        [Test]
        public void InjectAllFromChildren_ShouldThrowException_WhenBothInactiveWithFalseParameterOnDecorator()
        {
            // Init
            AudioControllerWithInactive audioController = AudioControllerBuilder.Create().AddEmptyGameObjectLevel()
                                                                                         .AddSliderGameObjectLevel("SliderMusic", false)
                                                                                         .AddEmptyGameObjectLevel()
                                                                                         .AddSliderGameObjectLevel("SliderSpatialisation", false)
                                                                                         .BuildWithInactive();

            Assert.That(audioController.musicSlider, Is.Null);
            Assert.That(audioController.spatialisationSlider, Is.Null);

            // Inject
            NixInjecter nixInjecter = new NixInjecter(audioController);
            Assert.Throws<NixInjecterException>(() => nixInjecter.CheckAndInjectAll());
        }

        [Test]
        public void InjectAllFromParent_ShouldFillFieldsWhenAllActive()
        {
            // Init
            UnderGroundAudioController ugAudioController = AudioControllerBuilder.Create().AddEmptyGameObjectLevel()
                                                                                          .AddSliderGameObjectLevel("SliderMusic")
                                                                                          .AddEmptyGameObjectLevel()
                                                                                          .AddSliderGameObjectLevel("SliderSpatialisation")
                                                                                          .BuildUnderGround();

            Assert.That(ugAudioController.musicSlider, Is.Null);
            Assert.That(ugAudioController.spatialisationSlider, Is.Null);

            // Inject
            NixInjecter nixInjecter = new NixInjecter(ugAudioController);
            nixInjecter.CheckAndInjectAll();

            // Verify GameObjects setted
            Assert.That(ugAudioController.musicSlider, Is.Not.Null);
            Assert.That(ugAudioController.musicSlider.IsActive(), Is.True);

            Assert.That(ugAudioController.spatialisationSlider, Is.Not.Null);
            Assert.That(ugAudioController.spatialisationSlider.IsActive(), Is.True);
        }

        [Test]
        public void InjectAllFromParent_ShouldFillFieldsWhenBothInactive()
        {
            // Init
            UnderGroundAudioController ugAudioController = AudioControllerBuilder.Create().AddEmptyGameObjectLevel()
                                                                                          .AddSliderGameObjectLevel("SliderMusic", false)
                                                                                          .AddEmptyGameObjectLevel()
                                                                                          .AddSliderGameObjectLevel("SliderSpatialisation", false)
                                                                                          .BuildUnderGround();

            Assert.That(ugAudioController.musicSlider, Is.Null);
            Assert.That(ugAudioController.spatialisationSlider, Is.Null);

            // Inject
            NixInjecter nixInjecter = new NixInjecter(ugAudioController);
            nixInjecter.CheckAndInjectAll();

            // Verify GameObjects setted
            Assert.That(ugAudioController.musicSlider, Is.Not.Null);
            Assert.That(ugAudioController.musicSlider.IsActive(), Is.False);

            Assert.That(ugAudioController.spatialisationSlider, Is.Not.Null);
            Assert.That(ugAudioController.spatialisationSlider.IsActive(), Is.False);
        }

        [Test]
        public void InjectAllFromParent_ShouldThrowException_WhenBothInactiveWithFalseParameterOnDecorator()
        {
            // Init
            UnderGroundAudioControllerWithInactive ugAudioController = AudioControllerBuilder.Create().AddEmptyGameObjectLevel()
                                                                                                      .AddSliderGameObjectLevel("SliderMusic", false)
                                                                                                      .AddEmptyGameObjectLevel()
                                                                                                      .AddSliderGameObjectLevel("SliderSpatialisation", false)
                                                                                                      .BuildUnderGroundWithInactive();

            Assert.That(ugAudioController.musicSlider, Is.Null);
            Assert.That(ugAudioController.spatialisationSlider, Is.Null);

            // Inject
            NixInjecter nixInjecter = new NixInjecter(ugAudioController);
            Assert.Throws<NixInjecterException>(() => nixInjecter.CheckAndInjectAll());
        }
        #endregion Inactive versus Active
        #endregion GameObjectInjection From MonoBehaviourInjectable

        #region GameObjectInjection From GameObject root
        #region GameObjectRoot errors
        [Test]
        public void InjectAll_ShouldThrowException_WhenGameObjectRootNotFound()
        {
            SceneWithChildProperties currentSceneProperties = SceneWithChildsBuilder.Create().WithChildSorcererController().WithRootName("FoundableName").Build();

            NixInjecter nixInjecter = new NixInjecter(currentSceneProperties.ChildSorcererController);
            Assert.Throws<NixInjecterException>(() => nixInjecter.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_ShouldThrowException_WhenManyGameObjectRootFound()
        {
            SceneWithChildProperties currentSceneProperties = SceneWithChildsBuilder.Create().WithChildSorcererController().WithDuplicateRoot().Build();

            NixInjecter nixInjecter = new NixInjecter(currentSceneProperties.ChildSorcererController);
            Assert.Throws<NixInjecterException>(() => nixInjecter.CheckAndInjectAll());
        }
        #endregion GameObjectRoot errors

        #region GetComponents
        [Test]
        public void InjectAll_FromGameObjectRoot_ShouldThrowException_WhenNotFound()
        {
            SceneWithProperties currentSceneProperties = SceneBuilder.Create().WithMonsterController().Build();

            NixInjecter nixInjecter = new NixInjecter(currentSceneProperties.MonsterController);
            Assert.Throws<NixInjecterException>(() => nixInjecter.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_FromGameObjectRoot_ShouldThrowException_WhenNotFoundSecondExample()
        {
            SceneWithProperties currentSceneProperties = SceneBuilder.Create().WithSorcererController().Build();

            NixInjecter nixInjecter = new NixInjecter(currentSceneProperties.SorcererController);
            Assert.Throws<NixInjecterException>(() => nixInjecter.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_FromGameObjectRoot_ShouldThrowException_WhenManyGameObjectWithTypeFoundOnRoot()
        {
            SceneWithProperties currentSceneProperties = SceneBuilder.Create().WithSorcererController().WithMonsterController().WithDuplicateSorcererControllerComponentAttached().Build();

            NixInjecter nixInjecter = new NixInjecter(currentSceneProperties.MonsterController);
            Assert.Throws<NixInjecterException>(() => nixInjecter.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_FromGameObjectRoot_ShouldFillGameObjectFields_WhenFound()
        {
            SceneWithProperties currentSceneProperties = SceneBuilder.Create().WithSorcererController().WithMonsterController().Build();

            Assert.That(currentSceneProperties.MonsterController.SorcererController, Is.Null);
            Assert.That(currentSceneProperties.SorcererController.MonsterController, Is.Null);

            NixInjecter sorcererInjecter = new NixInjecter(currentSceneProperties.SorcererController);
            NixInjecter monsterInjecter = new NixInjecter(currentSceneProperties.MonsterController);

            sorcererInjecter.CheckAndInjectAll();
            monsterInjecter.CheckAndInjectAll();
            
            Assert.That(currentSceneProperties.MonsterController.SorcererController, Is.Not.Null);
            Assert.That(currentSceneProperties.SorcererController.MonsterController, Is.Not.Null);

            Assert.That(currentSceneProperties.MonsterController.SorcererController.GetInstanceID(), Is.EqualTo(currentSceneProperties.SorcererController.GetInstanceID()));
            Assert.That(currentSceneProperties.SorcererController.MonsterController.GetInstanceID(), Is.EqualTo(currentSceneProperties.MonsterController.GetInstanceID()));
        }
        #endregion GetComponents

        #region GetComponentsInChildren
        [Test]
        public void InjectAll_FromGameObjectChildRoot_ShouldThrowException_WhenNotFound()
        {
            SceneWithChildProperties currentSceneProperties = SceneWithChildsBuilder.Create().WithChildMonsterController().Build();

            NixInjecter nixInjecter = new NixInjecter(currentSceneProperties.ChildMonsterController);
            Assert.Throws<NixInjecterException>(() => nixInjecter.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_FromGameObjectChildRoot_ShouldThrowException_WhenNotFoundSecondExample()
        {
            SceneWithChildProperties currentSceneProperties = SceneWithChildsBuilder.Create().WithChildSorcererController().Build();

            NixInjecter nixInjecter = new NixInjecter(currentSceneProperties.ChildSorcererController);
            Assert.Throws<NixInjecterException>(() => nixInjecter.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_FromGameObjectChildRoot_ShouldThrowException_WhenTypeFoundButNotNameInChildren()
        {
            SceneWithChildProperties currentSceneProperties = SceneWithChildsBuilder.Create().WithChildMonsterController()
                                                                                             .WithChildSorcererController("NotFoundableName")
                                                                                             .Build();

            NixInjecter nixInjecter = new NixInjecter(currentSceneProperties.ChildMonsterController);
            Assert.Throws<NixInjecterException>(() => nixInjecter.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_FromGameObjectChildRoot_ShouldThrowException_WhenManyFoundWithTypeAndNameInChildrenOfGameObjectRoot()
        {
            SceneWithChildProperties currentSceneProperties = SceneWithChildsBuilder.Create().WithChildMonsterController()
                                                                                             .WithChildSorcererController()
                                                                                             .WithDuplicateChildSorcererController()
                                                                                             .Build();

            NixInjecter nixInjecter = new NixInjecter(currentSceneProperties.ChildMonsterController);
            Assert.Throws<NixInjecterException>(() => nixInjecter.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_FromGameObjectChildRoot_ShouldFillGameObjectFields_WhenFound()
        {
            SceneWithChildProperties currentSceneProperties = SceneWithChildsBuilder.Create().WithChildSorcererController().WithChildMonsterController().Build();

            Assert.That(currentSceneProperties.ChildMonsterController.ChildSorcererController, Is.Null);
            Assert.That(currentSceneProperties.ChildSorcererController.ChildMonsterController, Is.Null);

            NixInjecter childSorcererInjecter = new NixInjecter(currentSceneProperties.ChildSorcererController);
            NixInjecter childMonsterInjecter = new NixInjecter(currentSceneProperties.ChildMonsterController);

            childSorcererInjecter.CheckAndInjectAll();
            childMonsterInjecter.CheckAndInjectAll();

            Assert.That(currentSceneProperties.ChildMonsterController.ChildSorcererController, Is.Not.Null);
            Assert.That(currentSceneProperties.ChildSorcererController.ChildMonsterController, Is.Not.Null);

            Assert.That(currentSceneProperties.ChildMonsterController.ChildSorcererController.GetInstanceID(), Is.EqualTo(currentSceneProperties.ChildSorcererController.GetInstanceID()));
            Assert.That(currentSceneProperties.ChildSorcererController.ChildMonsterController.GetInstanceID(), Is.EqualTo(currentSceneProperties.ChildMonsterController.GetInstanceID()));
        }
        #endregion GetComponentsInChildren

        #region Inactive versus Active
        [Test]
        public void InjectAllFromRootChildren_ShouldFillFieldsWhenAllActive()
        {
            // Init
            MenuController menuController = MenuControllerBuilder.Create("OptionRoot")
                                                                 .AddGameObjectLevelOnRoot<OptionsController>("OptionsController", true)
                                                                 .AddGameObjectLevelOnRoot<ScreenOptions>("ScreenOptions", true)
                                                                 .Build();

            Assert.That(menuController.OptionsController, Is.Null);
            Assert.That(menuController.ScreenOptions, Is.Null);

            // Inject
            NixInjecter nixInjecter = new NixInjecter(menuController);
            nixInjecter.CheckAndInjectAll();

            // Verify GameObjects setted
            Assert.That(menuController.OptionsController, Is.Not.Null);
            Assert.That(menuController.OptionsController.gameObject.activeSelf, Is.True);

            Assert.That(menuController.ScreenOptions, Is.Not.Null);
            Assert.That(menuController.ScreenOptions.gameObject.activeSelf, Is.True);
        }
        
        [Test]
        public void InjectAllFromRootChildren_ShouldFillFieldsWhenBothInactive()
        {
            // Init
            MenuController menuController = MenuControllerBuilder.Create("OptionRoot")
                                                                 .AddGameObjectLevelOnRoot<OptionsController>("OptionsController", false)
                                                                 .AddGameObjectLevelOnRoot<ScreenOptions>("ScreenOptions", false)
                                                                 .Build();

            Assert.That(menuController.OptionsController, Is.Null);
            Assert.That(menuController.ScreenOptions, Is.Null);

            // Inject
            NixInjecter nixInjecter = new NixInjecter(menuController);
            nixInjecter.CheckAndInjectAll();

            // Verify GameObjects setted
            Assert.That(menuController.OptionsController, Is.Not.Null);
            Assert.That(menuController.OptionsController.gameObject.activeSelf, Is.False);

            Assert.That(menuController.ScreenOptions, Is.Not.Null);
            Assert.That(menuController.ScreenOptions.gameObject.activeSelf, Is.False);
        }

        [Test]
        public void InjectAllFromRootChildren_ShouldThrowException_WhenBothInactiveWithFalseParameterOnDecorator()
        {
            // Init
            MenuControllerWithInactive menuController = MenuControllerBuilder.Create("OptionRoot")
                                                                             .AddGameObjectLevelOnRoot<OptionsController>("OptionsController", false)
                                                                             .AddGameObjectLevelOnRoot<ScreenOptions>("ScreenOptions", false)
                                                                             .BuildWithInactive();

            Assert.That(menuController.OptionsController, Is.Null);
            Assert.That(menuController.ScreenOptions, Is.Null);

            // Inject
            NixInjecter nixInjecter = new NixInjecter(menuController);
            Assert.Throws<NixInjecterException>(() => nixInjecter.CheckAndInjectAll());
        }

        [Test]
        public void InjectAllFromRootChildren_ShouldFillFields_WhenBothActiveWithFalseParameterOnDecorator()
        {
            // Init
            MenuControllerWithInactive menuController = MenuControllerBuilder.Create("OptionRoot")
                                                                             .AddGameObjectLevelOnRoot<OptionsController>("OptionsController", true)
                                                                             .AddGameObjectLevelOnRoot<ScreenOptions>("ScreenOptions", true)
                                                                             .BuildWithInactive();

            Assert.That(menuController.OptionsController, Is.Null);
            Assert.That(menuController.ScreenOptions, Is.Null);

            // Inject
            NixInjecter nixInjecter = new NixInjecter(menuController);
            nixInjecter.CheckAndInjectAll();

            // Verify GameObjects setted
            Assert.That(menuController.OptionsController, Is.Not.Null);
            Assert.That(menuController.OptionsController.gameObject.activeSelf, Is.True);

            Assert.That(menuController.ScreenOptions, Is.Not.Null);
            Assert.That(menuController.ScreenOptions.gameObject.activeSelf, Is.True);
        }
        #endregion Inactive versus Active
        #endregion GameObjectInjection From GameObject root
    }
}
