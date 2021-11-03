using Assets.ScriptExample.Audio;
using Assets.ScriptExample.Characters;
using Assets.ScriptExample.ComponentsWithEnumerable;
using Assets.ScriptExample.ComponentsWithInterface;
using Assets.ScriptExample.ComponentsWithInterface.BadDucks;
using Assets.ScriptExample.Fallen.AllComponentAttributes;
using Assets.ScriptExample.Fallen.List;
using Assets.ScriptExample.Menu;
using Assets.Tests.Builders;
using Nixi.Containers;
using Nixi.Injections;
using Nixi.Injections.Injecters;
using NUnit.Framework;
using ScriptExample.Characters;
using ScriptExample.Containers;
using System;
using System.Linq;
using Tests.Builders;
using UnityEditor.SceneManagement;
using UnityEngine;

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

        #region Component Interface
        [Test]
        public void Duck_ShouldBeFilledFromInterfaceInjection()
        {
            Duck duck = DuckBuilder.Create().BuildFullDuck();

            Assert.That(duck.Wings, Is.Null);
            Assert.That(duck.Pocket, Is.Null);
            Assert.That(duck.DuckCompanyBackPack, Is.Null);
            Assert.That(duck.FirstLake, Is.Null);
            Assert.That(duck.SecondLake, Is.Null);

            NixInjecter injecter = new NixInjecter(duck);
            injecter.CheckAndInjectAll();

            Assert.That(duck.Wings, Is.Not.Null);
            Assert.That(duck.Pocket, Is.Not.Null);
            Assert.That(duck.DuckCompanyBackPack, Is.Not.Null);
            Assert.That(duck.FirstLake, Is.Not.Null);
            Assert.That(duck.SecondLake, Is.Not.Null);
        }

        [TestCase(typeof(BadDuckCompo))]
        [TestCase(typeof(BadDuckCompoInChildren))]
        [TestCase(typeof(BadDuckCompoInParents))]
        public void Duck_ShouldThrowExceptionWhenNotGettableFromGetComponent(Type type)
        {
            GameObject gameObject = new GameObject("any", type);
            MonoBehaviourInjectable monoBehaviourInjectable = gameObject.GetComponent(type) as MonoBehaviourInjectable;

            NixInjecter injecter = new NixInjecter(monoBehaviourInjectable);

            Exception exception = Assert.Throws<NixInjecterException>(() => injecter.CheckAndInjectAll());

            StringAssert.Contains("No component with type IList was found", exception.Message);
        }

        [TestCase(typeof(BadDuckRootCompo))]
        [TestCase(typeof(BadDuckRootCompoWithChildGameObject))]
        public void Duck_ShouldThrowExceptionWhenNotGettableFromGetRootComponent(Type type)
        {
            // New Scene for each test iteration, because if build many rootObject with same name
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // Add root component for tests
            new GameObject("anyRootName", type);

            GameObject gameObject = new GameObject("any", type);
            MonoBehaviourInjectable monoBehaviourInjectable = gameObject.GetComponent(type) as MonoBehaviourInjectable;

            NixInjecter injecter = new NixInjecter(monoBehaviourInjectable);

            Exception exception = Assert.Throws<NixInjecterException>(() => injecter.CheckAndInjectAll());

            StringAssert.Contains("No component with type IList was found", exception.Message);
        }
        #endregion Component Interface

        #region Enumerable Injections
        [Test]
        public void InjectComponentList_OnEnumerable_ShouldFill()
        {
            // Arrange
            Basket basket = BasketBuilder.Create().WithChildFruit("apple", 3).WithChildFruit("lemon", 2).Build();

            Assert.IsNull(basket.FruitsList);
            Assert.IsNull(basket.FruitsEnumerable);
            Assert.IsNull(basket.IFruitsList);
            Assert.IsNull(basket.IFruitsEnumerable);
            
            // Act
            NixInjecter injecter = new NixInjecter(basket);
            injecter.CheckAndInjectAll();

            // FruitsList
            Assert.NotNull(basket.FruitsList);
            Assert.That(basket.FruitsList.Count, Is.EqualTo(2));
            Assert.That(basket.FruitsList.Any(x => x.name == "apple" && x.Weight == 3));
            Assert.That(basket.FruitsList.Any(x => x.name == "lemon" && x.Weight == 2));

            // FruitsEnumerable
            Assert.NotNull(basket.FruitsEnumerable);
            Assert.That(basket.FruitsEnumerable.Count, Is.EqualTo(2));
            Assert.That(basket.FruitsEnumerable.Any(x => x.name == "apple" && x.Weight == 3));
            Assert.That(basket.FruitsEnumerable.Any(x => x.name == "lemon" && x.Weight == 2));

            // IFruitsList
            Assert.NotNull(basket.IFruitsList);
            Assert.That(basket.IFruitsList.Count, Is.EqualTo(2));
            Assert.That(basket.IFruitsList.Any(x => x.Name == "apple" && x.Weight == 3));
            Assert.That(basket.IFruitsList.Any(x => x.Name == "lemon" && x.Weight == 2));

            // IFruitsEnumerable
            Assert.NotNull(basket.IFruitsEnumerable);
            Assert.That(basket.IFruitsEnumerable.Count, Is.EqualTo(2));
            Assert.That(basket.IFruitsEnumerable.Any(x => x.Name == "apple" && x.Weight == 3));
            Assert.That(basket.IFruitsEnumerable.Any(x => x.Name == "lemon" && x.Weight == 2));
        }

        [Test]
        public void InjectComponentList_OnNotEnumerable_ShouldThrowException()
        {
            var wrongBasket = BasketBuilder.Create().BuildNotEnumerable();

            NixInjecter injecter = new NixInjecter(wrongBasket);

            Assert.Throws<NixInjecterException>(() => injecter.CheckAndInjectAll());
        }

        [Test]
        public void InjectComponentList_OnEnumerableNotInterfaceNorComponent_ShouldThrowException()
        {
            var wrongBasket = BasketBuilder.Create().BuildEnumerableNotInterfaceNorComponent();

            NixInjecter injecter = new NixInjecter(wrongBasket);

            Assert.Throws<NixInjecterException>(() => injecter.CheckAndInjectAll());
        }

        [Test]
        public void InjectComponentList_OnListNotInterfaceNorComponent_ShouldThrowException()
        {
            var wrongBasket = BasketBuilder.Create().BuildListNotInterfaceNorComponent();

            NixInjecter injecter = new NixInjecter(wrongBasket);

            Assert.Throws<NixInjecterException>(() => injecter.CheckAndInjectAll());
        }
        #endregion Enumerable Injections

        #region Error decorator
        [TestCase(typeof(FallenCompoListClass))]
        [TestCase(typeof(FallenCompoListComponent))]
        [TestCase(typeof(FallenCompoListInjectable))]
        [TestCase(typeof(FallenCompoListInterface))]
        public void NixInjecter_InjectComponentList_OnWrongFieldType_ShouldThrowException(Type injectableTypeToBuild)
        {
            Component component = CompoBuilderWithExpliciteType.Create().Build(injectableTypeToBuild);
            MonoBehaviourInjectable injectable = component as MonoBehaviourInjectable;

            NixInjecter injecter = new NixInjecter(injectable);

            Exception exception = Assert.Throws<NixInjecterException>(() => injecter.CheckAndInjectAll());

            StringAssert.Contains("using decorator NixInjectComponentListAttribute", exception.Message);
        }

        [TestCase(typeof(FallenArrayComponent), "NixInjectComponentAttribute")]
        [TestCase(typeof(FallenEnumerableComponent), "NixInjectComponentAttribute")]
        [TestCase(typeof(FallenListComponent), "NixInjectComponentAttribute")]
        [TestCase(typeof(FallenArrayComponentChild), "NixInjectComponentFromMethodAttribute")]
        [TestCase(typeof(FallenEnumerableComponentChild), "NixInjectComponentFromMethodAttribute")]
        [TestCase(typeof(FallenListComponentChild), "NixInjectComponentFromMethodAttribute")]
        [TestCase(typeof(FallenArrayComponentParent), "NixInjectComponentFromMethodAttribute")]
        [TestCase(typeof(FallenEnumerableComponentParent), "NixInjectComponentFromMethodAttribute")]
        [TestCase(typeof(FallenListComponentParent), "NixInjectComponentFromMethodAttribute")]
        [TestCase(typeof(FallenArrayComponentRoot), "NixInjectRootComponentAttribute")]
        [TestCase(typeof(FallenEnumerableComponentRoot), "NixInjectRootComponentAttribute")]
        [TestCase(typeof(FallenListComponentRoot), "NixInjectRootComponentAttribute")]
        [TestCase(typeof(FallenArrayComponentRootChild), "NixInjectRootComponentAttribute")]
        [TestCase(typeof(FallenEnumerableComponentRootChild), "NixInjectRootComponentAttribute")]
        [TestCase(typeof(FallenListComponentRootChild), "NixInjectRootComponentAttribute")]
        public void NixInjecter_InjectAnyComponentWhichIsNotListDecorator_OnWrongEnumerableFieldType_ShouldThrowException(Type injectableTypeToBuild, string attributeName)
        {
            Component component = CompoBuilderWithExpliciteType.Create().Build(injectableTypeToBuild);
            MonoBehaviourInjectable injectable = component as MonoBehaviourInjectable;

            NixInjecter injecter = new NixInjecter(injectable);

            Exception exception = Assert.Throws<NixInjecterException>(() => injecter.CheckAndInjectAll());

            StringAssert.Contains($"using decorator {attributeName}", exception.Message);
        }
        #endregion Error decorator
    }
}
