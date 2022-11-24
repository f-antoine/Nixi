using Nixi.Containers;
using Nixi.Injections;
using Nixi.Injections.Injectors;
using NUnit.Framework;
using ScriptExample.Audio;
using ScriptExample.CannotFindFromMethods;
using ScriptExample.Characters;
using ScriptExample.ComponentFromMethodWithoutName;
using ScriptExample.ComponentsWithEnumerable;
using ScriptExample.ComponentsWithEnumerable.BadBasket;
using ScriptExample.ComponentsWithInterface;
using ScriptExample.ComponentsWithInterface.BadDucks;
using ScriptExample.Containers;
using ScriptExample.EnumerableCrashTests;
using ScriptExample.Fallen.AllComponentAttributes.Component;
using ScriptExample.Fallen.AllComponentAttributes.ComponentChild;
using ScriptExample.Fallen.AllComponentAttributes.ComponentParent;
using ScriptExample.Fallen.AllComponentAttributes.ComponentRoot;
using ScriptExample.Fallen.AllComponentAttributes.ComponentRootChild;
using ScriptExample.Fallen.Enumerables;
using ScriptExample.Menu;
using ScriptExample.SpecialOptions;
using System;
using System.Linq;
using Tests.Builders;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Tests.Injections
{
    internal sealed class NixInjectorTests
    {
        [SetUp]
        public void InitTests()
        {
            NixiContainer.RemoveMap<ITestInterface>();
            NixiContainer.MapSingleton<ITestInterface, TestImplementation>();
        }

        #region InjectFields
        [Test]
        public void InjectField_ShouldThrowException_WhenInterfaceNotMapped()
        {
            NixiContainer.RemoveMap<ITestInterface>();

            NixInjector nixInjector = SorcererBuilder.Create().WithSkill().WithChildSkill().BuildNixInjector();

            Assert.Throws<NixiContainerException>(() => nixInjector.CheckAndInjectAll());
        }

        [Test]
        public void InjectField_ShouldFillField_WithCorrectInstance()
        {
            // Init
            Sorcerer sorcerer = SorcererBuilder.Create().WithSkill().WithChildSkill().Build();
            Assert.That(sorcerer.TestInterface, Is.Null);

            // Inject
            NixInjector nixInjector = new NixInjector(sorcerer);
            nixInjector.CheckAndInjectAll();

            // Retrieve interface
            ITestInterface element = NixiContainer.ResolveMap<ITestInterface>();
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
            NixInjector nixInjector = new NixInjector(sorcerer);
            nixInjector.CheckAndInjectAll();

            // Verify not injected because marked with IsInjectedFromContainer
            Assert.That(sorcerer.SOInfos, Is.Null);
        }
        #endregion InjectFields

        #region GameObjectInjection From MonoBehaviourInjectable
        [Test]
        public void InjectAll_ShouldThrowException_WhenSkillIsNot_OnCurrentElement()
        {
            NixInjector nixInjector = SorcererBuilder.Create().WithChildSkill().BuildNixInjector();
            Assert.Throws<NixInjectorException>(() => nixInjector.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_ShouldThrowException_WhenSkillIsNot_OnChildElement()
        {
            NixInjector nixInjector = SorcererBuilder.Create().WithSkill().BuildNixInjector();
            Assert.Throws<NixInjectorException>(() => nixInjector.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_ShouldThrowException_WhenTwoSkill_OnCurrentElement()
        {
            // Two skill added at same level
            NixInjector nixInjector = SorcererBuilder.Create().WithSkill().WithSkill().WithChildSkill().BuildNixInjector();
            Assert.Throws<NixInjectorException>(() => nixInjector.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_ShouldThrowException_WhenTwoSkill_OnChildElement()
        {
            // Two skills added at same level
            NixInjector nixInjector = SorcererBuilder.Create().WithSkill().WithChildSkill().WithChildSkill().BuildNixInjector();
            Assert.Throws<NixInjectorException>(() => nixInjector.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_ShouldThrowException_WhenSkillWithNameDoesNotExist_OnChildElement()
        {
            NixInjector nixInjector = SorcererBuilder.Create().WithSkill().WithChildSkill("NotFoundable").BuildNixInjector();
            Assert.Throws<NixInjectorException>(() => nixInjector.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_ShouldFillGameObjectFields_WithCorrectInstances()
        {
            // Init
            Sorcerer sorcerer = SorcererBuilder.Create().WithSkill().WithChildSkill().Build();
            Assert.That(sorcerer.AttackSkill, Is.Null);
            Assert.That(sorcerer.MagicSkill, Is.Null);

            // Inject
            NixInjector nixInjector = new NixInjector(sorcerer);
            nixInjector.CheckAndInjectAll();

            // Verify GameObjects setted
            Assert.That(sorcerer.AttackSkill, Is.Not.Null);
            Assert.That(sorcerer.AttackSkill.name, Is.EqualTo("SorcererGameObjectName"));

            Assert.That(sorcerer.MagicSkill, Is.Not.Null);
            Assert.That(sorcerer.MagicSkill.name, Is.EqualTo("SorcererChildGameObjectName"));
        }

        [Test]
        public void InjectAll_ShouldThrowException_WhenSorcerer_IsNotParent()
        {
            NixInjector nixInjector = ParasiteBuilder.Create().BuildDefaultInjector();
            Assert.Throws<NixInjectorException>(() => nixInjector.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_ShouldFillParasiteGameObjectField_WhenSorcerer_IsParent()
        {
            // Init
            Parasite parasite = ParasiteBuilder.Create().WithParentSorcerer().Build();

            Assert.That(parasite.ParentSorcerer, Is.Null);

            // Inject
            NixInjector nixInjector = new NixInjector(parasite);
            nixInjector.CheckAndInjectAll();

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
            NixInjector nixInjector = new NixInjector(audioController);
            nixInjector.CheckAndInjectAll();

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
            NixInjector nixInjector = new NixInjector(audioController);
            nixInjector.CheckAndInjectAll();

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
            NixInjector nixInjector = new NixInjector(audioController);
            Assert.Throws<NixInjectorException>(() => nixInjector.CheckAndInjectAll());
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
            NixInjector nixInjector = new NixInjector(ugAudioController);
            nixInjector.CheckAndInjectAll();

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
            NixInjector nixInjector = new NixInjector(ugAudioController);
            nixInjector.CheckAndInjectAll();

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
            NixInjector nixInjector = new NixInjector(ugAudioController);
            Assert.Throws<NixInjectorException>(() => nixInjector.CheckAndInjectAll());
        }
        #endregion Inactive versus Active
        #endregion GameObjectInjection From MonoBehaviourInjectable

        #region GameObjectInjection From GameObject root
        #region GameObjectRoot errors
        [Test]
        public void InjectAll_ShouldThrowException_WhenGameObjectRootNotFound()
        {
            SceneWithChildProperties currentSceneProperties = SceneWithChildrenBuilder.Create().WithChildSorcererController().WithRootName("FoundableName").Build();

            NixInjector nixInjector = new NixInjector(currentSceneProperties.ChildSorcererController);
            Assert.Throws<NixInjectorException>(() => nixInjector.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_ShouldThrowException_WhenManyGameObjectRootFound()
        {
            SceneWithChildProperties currentSceneProperties = SceneWithChildrenBuilder.Create().WithChildSorcererController().WithDuplicateRoot().Build();

            NixInjector nixInjector = new NixInjector(currentSceneProperties.ChildSorcererController);
            Assert.Throws<NixInjectorException>(() => nixInjector.CheckAndInjectAll());
        }
        #endregion GameObjectRoot errors

        #region GetComponents
        [Test]
        public void InjectAll_FromGameObjectRoot_ShouldThrowException_WhenNotFound()
        {
            SceneWithProperties currentSceneProperties = SceneBuilder.Create().WithMonsterController().Build();

            NixInjector nixInjector = new NixInjector(currentSceneProperties.MonsterController);
            Assert.Throws<NixInjectorException>(() => nixInjector.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_FromGameObjectRoot_ShouldThrowException_WhenNotFoundSecondExample()
        {
            SceneWithProperties currentSceneProperties = SceneBuilder.Create().WithSorcererController().Build();

            NixInjector nixInjector = new NixInjector(currentSceneProperties.SorcererController);
            Assert.Throws<NixInjectorException>(() => nixInjector.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_FromGameObjectRoot_ShouldThrowException_WhenManyGameObjectWithTypeFoundOnRoot()
        {
            SceneWithProperties currentSceneProperties = SceneBuilder.Create().WithSorcererController().WithMonsterController().WithDuplicateSorcererControllerComponentAttached().Build();

            NixInjector nixInjector = new NixInjector(currentSceneProperties.MonsterController);
            Assert.Throws<NixInjectorException>(() => nixInjector.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_FromGameObjectRoot_ShouldFillGameObjectFields_WhenFound()
        {
            SceneWithProperties currentSceneProperties = SceneBuilder.Create().WithSorcererController().WithMonsterController().Build();

            Assert.That(currentSceneProperties.MonsterController.SorcererController, Is.Null);
            Assert.That(currentSceneProperties.SorcererController.MonsterController, Is.Null);

            NixInjector sorcererInjector = new NixInjector(currentSceneProperties.SorcererController);
            NixInjector monsterInjector = new NixInjector(currentSceneProperties.MonsterController);

            sorcererInjector.CheckAndInjectAll();
            monsterInjector.CheckAndInjectAll();
            
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
            SceneWithChildProperties currentSceneProperties = SceneWithChildrenBuilder.Create().WithChildMonsterController().Build();

            NixInjector nixInjector = new NixInjector(currentSceneProperties.ChildMonsterController);
            Assert.Throws<NixInjectorException>(() => nixInjector.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_FromGameObjectChildRoot_ShouldThrowException_WhenNotFoundSecondExample()
        {
            SceneWithChildProperties currentSceneProperties = SceneWithChildrenBuilder.Create().WithChildSorcererController().Build();

            NixInjector nixInjector = new NixInjector(currentSceneProperties.ChildSorcererController);
            Assert.Throws<NixInjectorException>(() => nixInjector.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_FromGameObjectChildRoot_ShouldThrowException_WhenTypeFoundButNotNameInChildren()
        {
            SceneWithChildProperties currentSceneProperties = SceneWithChildrenBuilder.Create().WithChildMonsterController()
                                                                                             .WithChildSorcererController("NotFoundableName")
                                                                                             .Build();

            NixInjector nixInjector = new NixInjector(currentSceneProperties.ChildMonsterController);
            Assert.Throws<NixInjectorException>(() => nixInjector.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_FromGameObjectChildRoot_ShouldThrowException_WhenManyFoundWithTypeAndNameInChildrenOfGameObjectRoot()
        {
            SceneWithChildProperties currentSceneProperties = SceneWithChildrenBuilder.Create().WithChildMonsterController()
                                                                                             .WithChildSorcererController()
                                                                                             .WithDuplicateChildSorcererController()
                                                                                             .Build();

            NixInjector nixInjector = new NixInjector(currentSceneProperties.ChildMonsterController);
            Assert.Throws<NixInjectorException>(() => nixInjector.CheckAndInjectAll());
        }

        [Test]
        public void InjectAll_FromGameObjectChildRoot_ShouldFillGameObjectFields_WhenFound()
        {
            SceneWithChildProperties currentSceneProperties = SceneWithChildrenBuilder.Create().WithChildSorcererController().WithChildMonsterController().Build();

            Assert.That(currentSceneProperties.ChildMonsterController.ChildSorcererController, Is.Null);
            Assert.That(currentSceneProperties.ChildSorcererController.ChildMonsterController, Is.Null);

            NixInjector childSorcererInjector = new NixInjector(currentSceneProperties.ChildSorcererController);
            NixInjector childMonsterInjector = new NixInjector(currentSceneProperties.ChildMonsterController);

            childSorcererInjector.CheckAndInjectAll();
            childMonsterInjector.CheckAndInjectAll();

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
            NixInjector nixInjector = new NixInjector(menuController);
            nixInjector.CheckAndInjectAll();

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
            NixInjector nixInjector = new NixInjector(menuController);
            nixInjector.CheckAndInjectAll();

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
            NixInjector nixInjector = new NixInjector(menuController);
            Assert.Throws<NixInjectorException>(() => nixInjector.CheckAndInjectAll());
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
            NixInjector nixInjector = new NixInjector(menuController);
            nixInjector.CheckAndInjectAll();

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

            NixInjector Injector = new NixInjector(duck);
            Injector.CheckAndInjectAll();

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
            MonoBehaviourInjectable injectable = gameObject.GetComponent(type) as MonoBehaviourInjectable;

            NixInjector Injector = new NixInjector(injectable);

            Exception exception = Assert.Throws<NixInjectorException>(() => Injector.CheckAndInjectAll());

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
            MonoBehaviourInjectable injectable = gameObject.GetComponent(type) as MonoBehaviourInjectable;

            NixInjector Injector = new NixInjector(injectable);

            Exception exception = Assert.Throws<NixInjectorException>(() => Injector.CheckAndInjectAll());

            StringAssert.Contains("No component with type IList was found", exception.Message);
        }
        #endregion Component Interface

        #region Enumerable Injections
        [Test]
        public void InjectComponentList_OnEnumerable_ShouldFillOnlyCurrentLevel()
        {
            // Arrange
            Basket basket = BasketBuilder.Create()
                                         .WithParentFruit("firstLevelParent", 3)
                                         .WithParentFruit("secondLevelParent", 3)
                                         .WithLocalFruit(17)
                                         .WithLocalFruit(14)
                                         .WithChildFruit("apple", 2)
                                         .WithChildFruit("melon", 2)
                                         .WithChildFruit("lemon", 2)
                                         .Build();

            Assert.IsNull(basket.FruitsList);
            Assert.IsNull(basket.FruitsEnumerable);
            Assert.IsNull(basket.IFruitsList);
            Assert.IsNull(basket.IFruitsEnumerable);
            
            // Act
            NixInjector Injector = new NixInjector(basket);
            Injector.CheckAndInjectAll();

            // Check implementation in Unity
            Assert.That(basket.GetComponentsInParent<Fruit>().Length, Is.EqualTo(4));
            Assert.That(basket.GetComponents<Fruit>().Length, Is.EqualTo(2));
            Assert.That(basket.GetComponentsInChildren<Fruit>().Length, Is.EqualTo(5));

            // FruitsList
            Assert.NotNull(basket.FruitsList);
            Assert.That(basket.FruitsList.Count, Is.EqualTo(2));
            Assert.That(basket.FruitsList.Any(x => x.name == basket.name && x.Weight == 17));
            Assert.That(basket.FruitsList.Any(x => x.name == basket.name && x.Weight == 14));
            foreach (Fruit fruit in basket.FruitsList)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.EqualTo(basket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.EqualTo(basket.transform.GetInstanceID()));
                Assert.That(fruit.GetInstanceID(), Is.Not.EqualTo(basket.GetInstanceID()));
            }

            // FruitsEnumerable
            Assert.NotNull(basket.FruitsEnumerable);
            Assert.That(basket.FruitsEnumerable.Count, Is.EqualTo(2));
            Assert.That(basket.FruitsEnumerable.Any(x => x.name == basket.name && x.Weight == 17));
            Assert.That(basket.FruitsEnumerable.Any(x => x.name == basket.name && x.Weight == 14));
            foreach (Fruit fruit in basket.FruitsEnumerable)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.EqualTo(basket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.EqualTo(basket.transform.GetInstanceID()));
                Assert.That(fruit.GetInstanceID(), Is.Not.EqualTo(basket.GetInstanceID()));
            }

            // IFruitsList
            Assert.NotNull(basket.IFruitsList);
            Assert.That(basket.IFruitsList.Count, Is.EqualTo(2));
            Assert.That(basket.IFruitsList.Any(x => x.Name == basket.name && x.Weight == 17));
            Assert.That(basket.IFruitsList.Any(x => x.Name == basket.name && x.Weight == 14));
            foreach (Fruit fruit in basket.IFruitsList)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.EqualTo(basket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.EqualTo(basket.transform.GetInstanceID()));
                Assert.That(fruit.GetInstanceID(), Is.Not.EqualTo(basket.GetInstanceID()));
            }

            // IFruitsEnumerable
            Assert.NotNull(basket.IFruitsEnumerable);
            Assert.That(basket.IFruitsEnumerable.Count, Is.EqualTo(2));
            Assert.That(basket.IFruitsEnumerable.Any(x => x.Name == basket.name && x.Weight == 17));
            Assert.That(basket.IFruitsEnumerable.Any(x => x.Name == basket.name && x.Weight == 14));
            foreach (Fruit fruit in basket.IFruitsEnumerable)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.EqualTo(basket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.EqualTo(basket.transform.GetInstanceID()));
                Assert.That(fruit.GetInstanceID(), Is.Not.EqualTo(basket.GetInstanceID()));
            }
        }

        [Test]
        public void InjectComponentList_OnNotEnumerable_ShouldThrowException()
        {
            BadBasketNotEnumerable wrongBasket = InjectableBuilder<BadBasketNotEnumerable>.Create().Build();

            NixInjector Injector = new NixInjector(wrongBasket);

            Assert.Throws<NixInjectorException>(() => Injector.CheckAndInjectAll());
        }

        [Test]
        public void InjectComponentList_OnEnumerableNotInterfaceNorComponent_ShouldThrowException()
        {
            var wrongBasket = InjectableBuilder<BadBasketEnumerableNotInterfaceNorComponent>.Create().Build();

            NixInjector Injector = new NixInjector(wrongBasket);

            Assert.Throws<NixInjectorException>(() => Injector.CheckAndInjectAll());
        }

        [Test]
        public void InjectComponentList_OnListNotInterfaceNorComponent_ShouldThrowException()
        {
            var wrongBasket = InjectableBuilder<BadBasketListNotInterfaceNorComponent>.Create().Build();

            NixInjector Injector = new NixInjector(wrongBasket);

            Assert.Throws<NixInjectorException>(() => Injector.CheckAndInjectAll());
        }
        #endregion Enumerable Injections

        #region EnumerableFromMethod Injections
        [Test]
        public void InjectComponentListFromMethod_OnEnumerable_ShouldFillOtherThanCurrentLevel()
        {
            // Arrange
            BasketWithChildrenAndParents basket = BasketBuilder.Create()
                                                               .WithParentFruit("firstLevelParent", 4)
                                                               .WithParentFruit("secondLevelParent", 5)
                                                               .WithLocalFruit(17)
                                                               .WithChildFruit("apple", 1)
                                                               .WithChildFruit("melon", 2)
                                                               .WithChildFruit("lemon", 3)
                                                               .BuildBasketWithChildrenAndParents();

            // Checks parent and children
            Assert.IsNull(basket.FruitsListParents);
            Assert.IsNull(basket.FruitsEnumerableParents);
            Assert.IsNull(basket.IFruitsListParents);
            Assert.IsNull(basket.IFruitsEnumerableParents);

            Assert.IsNull(basket.FruitsListChildren);
            Assert.IsNull(basket.FruitsEnumerableChildren);
            Assert.IsNull(basket.IFruitsListChildren);
            Assert.IsNull(basket.IFruitsEnumerableChildren);

            // Act
            NixInjector Injector = new NixInjector(basket);
            Injector.CheckAndInjectAll();

            // Check implementation in Unity
            Assert.That(basket.GetComponentsInParent<Fruit>().Length, Is.EqualTo(3));
            Assert.That(basket.GetComponents<Fruit>().Length, Is.EqualTo(1));
            Assert.That(basket.GetComponentsInChildren<Fruit>().Length, Is.EqualTo(4));

            #region Parents checks
            // FruitsList
            Assert.NotNull(basket.FruitsListParents);
            Assert.That(basket.FruitsListParents.Count, Is.EqualTo(2));
            Assert.That(basket.FruitsListParents.Any(x => x.name == "firstLevelParent" && x.Weight == 4));
            Assert.That(basket.FruitsListParents.Any(x => x.name == "secondLevelParent" && x.Weight == 5));
            foreach (Fruit fruit in basket.FruitsListParents)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(basket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(basket.transform.GetInstanceID()));
                Assert.That(fruit.GetInstanceID(), Is.Not.EqualTo(basket.GetInstanceID()));
            }

            // FruitsEnumerable
            Assert.NotNull(basket.FruitsEnumerableParents);
            Assert.That(basket.FruitsEnumerableParents.Count, Is.EqualTo(2));
            Assert.That(basket.FruitsEnumerableParents.Any(x => x.name == "firstLevelParent" && x.Weight == 4));
            Assert.That(basket.FruitsEnumerableParents.Any(x => x.name == "secondLevelParent" && x.Weight == 5));
            foreach (Fruit fruit in basket.FruitsEnumerableParents)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(basket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(basket.transform.GetInstanceID()));
                Assert.That(fruit.GetInstanceID(), Is.Not.EqualTo(basket.GetInstanceID()));
            }

            // IFruitsList
            Assert.NotNull(basket.IFruitsListParents);
            Assert.That(basket.IFruitsListParents.Count, Is.EqualTo(2));
            Assert.That(basket.IFruitsListParents.Any(x => x.Name == "firstLevelParent" && x.Weight == 4));
            Assert.That(basket.IFruitsListParents.Any(x => x.Name == "secondLevelParent" && x.Weight == 5));
            foreach (Fruit fruit in basket.IFruitsListParents)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(basket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(basket.transform.GetInstanceID()));
                Assert.That(fruit.GetInstanceID(), Is.Not.EqualTo(basket.GetInstanceID()));
            }
            
            // IFruitsEnumerable
            Assert.NotNull(basket.IFruitsEnumerableParents);
            Assert.That(basket.IFruitsEnumerableParents.Count, Is.EqualTo(2));
            Assert.That(basket.IFruitsEnumerableParents.Any(x => x.Name == "firstLevelParent" && x.Weight == 4));
            Assert.That(basket.IFruitsEnumerableParents.Any(x => x.Name == "secondLevelParent" && x.Weight == 5));
            foreach (Fruit fruit in basket.IFruitsEnumerableParents)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(basket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(basket.transform.GetInstanceID()));
                Assert.That(fruit.GetInstanceID(), Is.Not.EqualTo(basket.GetInstanceID()));
            }
            #endregion Parents checks

            #region Children checks
            // FruitsList
            Assert.NotNull(basket.FruitsListChildren);
            Assert.That(basket.FruitsListChildren.Count, Is.EqualTo(3));
            Assert.That(basket.FruitsListChildren.Any(x => x.name == "apple" && x.Weight == 1));
            Assert.That(basket.FruitsListChildren.Any(x => x.name == "melon" && x.Weight == 2));
            Assert.That(basket.FruitsListChildren.Any(x => x.name == "lemon" && x.Weight == 3));
            foreach (Fruit fruit in basket.FruitsListChildren)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(basket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(basket.transform.GetInstanceID()));
                Assert.That(fruit.GetInstanceID(), Is.Not.EqualTo(basket.GetInstanceID()));
            }

            // FruitsEnumerable
            Assert.NotNull(basket.FruitsEnumerableChildren);
            Assert.That(basket.FruitsEnumerableChildren.Count, Is.EqualTo(3));
            Assert.That(basket.FruitsEnumerableChildren.Any(x => x.name == "apple" && x.Weight == 1));
            Assert.That(basket.FruitsEnumerableChildren.Any(x => x.name == "melon" && x.Weight == 2));
            Assert.That(basket.FruitsEnumerableChildren.Any(x => x.name == "lemon" && x.Weight == 3));
            foreach (Fruit fruit in basket.FruitsEnumerableChildren)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(basket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(basket.transform.GetInstanceID()));
                Assert.That(fruit.GetInstanceID(), Is.Not.EqualTo(basket.GetInstanceID()));
            }

            // IFruitsList
            Assert.NotNull(basket.IFruitsListChildren);
            Assert.That(basket.IFruitsListChildren.Count, Is.EqualTo(3));
            Assert.That(basket.IFruitsListChildren.Any(x => x.Name == "apple" && x.Weight == 1));
            Assert.That(basket.IFruitsListChildren.Any(x => x.Name == "melon" && x.Weight == 2));
            Assert.That(basket.IFruitsListChildren.Any(x => x.Name == "lemon" && x.Weight == 3));
            foreach (Fruit fruit in basket.IFruitsListChildren)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(basket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(basket.transform.GetInstanceID()));
                Assert.That(fruit.GetInstanceID(), Is.Not.EqualTo(basket.GetInstanceID()));
            }


            // IFruitsEnumerable
            Assert.NotNull(basket.IFruitsEnumerableChildren);
            Assert.That(basket.IFruitsEnumerableChildren.Count, Is.EqualTo(3));
            Assert.That(basket.IFruitsEnumerableChildren.Any(x => x.Name == "apple" && x.Weight == 1));
            Assert.That(basket.IFruitsEnumerableChildren.Any(x => x.Name == "melon" && x.Weight == 2));
            Assert.That(basket.IFruitsEnumerableChildren.Any(x => x.Name == "lemon" && x.Weight == 3));
            foreach (Fruit fruit in basket.IFruitsEnumerableChildren)
            {
                Assert.That(fruit.gameObject.GetInstanceID(), Is.Not.EqualTo(basket.gameObject.GetInstanceID()));
                Assert.That(fruit.transform.GetInstanceID(), Is.Not.EqualTo(basket.transform.GetInstanceID()));
                Assert.That(fruit.GetInstanceID(), Is.Not.EqualTo(basket.GetInstanceID()));
            }
            #endregion Children checks
        }
        #endregion EnumerableFromMethod Injections

        #region Enumerable Crash Tests
        [Test]
        public void EnumerableCrashers_FromList_ShouldNotThrowException_WhenInjected()
        {
            // Prepare
            EnumerableCrashTesters crasher = InjectableBuilder<EnumerableCrashTesters>.Create().Build();
            Fruit fruit = new GameObject("FirstFruit").AddComponent<Fruit>();
            Fruit secondFruit = new GameObject("SecondFruit").AddComponent<Fruit>();
            fruit.transform.SetParent(crasher.transform);
            secondFruit.transform.SetParent(crasher.transform);

            // Act
            NixInjector Injector = new NixInjector(crasher);
            Injector.CheckAndInjectAll();

            // Checks
            Assert.AreEqual(crasher.ReadOnlyFruitsChildren[0].GetInstanceID(), fruit.GetInstanceID());
            Assert.AreEqual(crasher.ReadOnlyFruitsChildren[1].GetInstanceID(), secondFruit.GetInstanceID());

            Assert.AreEqual(crasher.FruitsChildren[0].GetInstanceID(), fruit.GetInstanceID());
            Assert.AreEqual(crasher.FruitsChildren[1].GetInstanceID(), secondFruit.GetInstanceID());

            Assert.AreEqual(crasher.ArrayFruitsChildren[0].GetInstanceID(), fruit.GetInstanceID());
            Assert.AreEqual(crasher.ArrayFruitsChildren[1].GetInstanceID(), secondFruit.GetInstanceID());

            Assert.AreEqual(crasher.EnumerableFruitsChildren.First().GetInstanceID(), fruit.GetInstanceID());
            Assert.AreEqual(crasher.EnumerableFruitsChildren.Skip(1).First().GetInstanceID(), secondFruit.GetInstanceID());
        }

        [Test]
        public void EnumerableCrashers_FromEnumerable_ShouldNotThrowException_WhenInjected()
        {
            // Prepare
            EnumerableCrashTesters crasher = InjectableBuilder<EnumerableCrashTesters>.Create().Build();
            Fruit fruit = crasher.gameObject.AddComponent<Fruit>();
            Fruit secondFruit = crasher.gameObject.AddComponent<Fruit>();

            // Act
            NixInjector Injector = new NixInjector(crasher);
            Injector.CheckAndInjectAll();

            // Checks
            Assert.AreEqual(crasher.ReadOnlyFruits[0].GetInstanceID(), fruit.GetInstanceID());
            Assert.AreEqual(crasher.ReadOnlyFruits[1].GetInstanceID(), secondFruit.GetInstanceID());

            Assert.AreEqual(crasher.Fruits[0].GetInstanceID(), fruit.GetInstanceID());
            Assert.AreEqual(crasher.Fruits[1].GetInstanceID(), secondFruit.GetInstanceID());

            Assert.AreEqual(crasher.ArrayFruits[0].GetInstanceID(), fruit.GetInstanceID());
            Assert.AreEqual(crasher.ArrayFruits[1].GetInstanceID(), secondFruit.GetInstanceID());

            Assert.AreEqual(crasher.EnumerableFruits.First().GetInstanceID(), fruit.GetInstanceID());
            Assert.AreEqual(crasher.EnumerableFruits.Skip(1).First().GetInstanceID(), secondFruit.GetInstanceID());
        }

        [Test]
        public void EnumerableCrashers_FromArray_ShouldNotThrowException_WhenInjected()
        {
            // Prepare
            EnumerableCrashTesters crasher = InjectableBuilder<EnumerableCrashTesters>.Create().Build();
            Fruit fruit = new GameObject("FirstFruit").AddComponent<Fruit>();
            Fruit secondFruit = new GameObject("SecondFruit").AddComponent<Fruit>();
            crasher.transform.SetParent(fruit.transform);
            fruit.transform.SetParent(secondFruit.transform);

            // Act
            NixInjector Injector = new NixInjector(crasher);
            Injector.CheckAndInjectAll();

            // Checks
            Assert.AreEqual(crasher.ReadOnlyFruitsParent[0].GetInstanceID(), fruit.GetInstanceID());
            Assert.AreEqual(crasher.ReadOnlyFruitsParent[1].GetInstanceID(), secondFruit.GetInstanceID());

            Assert.AreEqual(crasher.FruitsParent[0].GetInstanceID(), fruit.GetInstanceID());
            Assert.AreEqual(crasher.FruitsParent[1].GetInstanceID(), secondFruit.GetInstanceID());

            Assert.AreEqual(crasher.ArrayFruitsParent[0].GetInstanceID(), fruit.GetInstanceID());
            Assert.AreEqual(crasher.ArrayFruitsParent[1].GetInstanceID(), secondFruit.GetInstanceID());

            Assert.AreEqual(crasher.EnumerableFruitsParent.First().GetInstanceID(), fruit.GetInstanceID());
            Assert.AreEqual(crasher.EnumerableFruitsParent.Skip(1).First().GetInstanceID(), secondFruit.GetInstanceID());
        }
        #endregion Enumerable Crash Tests

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
        public void NixInjector_InjectComponentList_OnWrongFieldType_ShouldThrowException(Type injectableTypeToBuild)
        {
            Component component = InjectableBuilderWithExpliciteType.Create().Build(injectableTypeToBuild);
            MonoBehaviourInjectable injectable = component as MonoBehaviourInjectable;

            NixInjector Injector = new NixInjector(injectable);

            Exception exception = Assert.Throws<NixInjectorException>(() => Injector.CheckAndInjectAll());

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
        public void NixInjector_InjectAnyComponentWhichIsNotListDecorator_OnWrongEnumerableFieldType_ShouldThrowException(Type injectableTypeToBuild, string attributeName)
        {
            Component component = InjectableBuilderWithExpliciteType.Create().Build(injectableTypeToBuild);
            MonoBehaviourInjectable injectable = component as MonoBehaviourInjectable;

            NixInjector Injector = new NixInjector(injectable);

            Exception exception = Assert.Throws<NixInjectorException>(() => Injector.CheckAndInjectAll());

            StringAssert.Contains($"using decorator {attributeName}", exception.Message);
        }
        #endregion Error decorator

        #region NixInjectComponentFromMethod without name (should find unique)
        [Test]
        public void GetChildren_ShouldFindUniqueElement_WithoutName()
        {
            ComponentChildWithoutName injectable = InjectableBuilder<ComponentChildWithoutName>.Create().Build();
            Slider childSlider = new GameObject().AddComponent<Slider>();
            childSlider.transform.SetParent(injectable.transform);
            
            Assert.Null(injectable.Slider);
            Assert.Null(injectable.SliderWithEmptyString);
            Assert.Null(injectable.SliderInterface);
            Assert.Null(injectable.SliderInterfaceWithEmptyString);

            // Act
            NixInjector Injector = new NixInjector(injectable);
            Injector.CheckAndInjectAll();

            // Check
            Assert.AreEqual(childSlider.GetInstanceID(), injectable.Slider.GetInstanceID());
            Assert.AreEqual(childSlider.GetInstanceID(), injectable.SliderWithEmptyString.GetInstanceID());
            Assert.NotNull(injectable.SliderInterface);
            Assert.NotNull(injectable.SliderInterfaceWithEmptyString);
        }

        [Test]
        public void GetChildren_ShouldThrowException_WhenNotFound()
        {
            ComponentChildWithoutName injectable = InjectableBuilder<ComponentChildWithoutName>.Create().Build();

            NixInjector Injector = new NixInjector(injectable);

            Assert.Throws<NixInjectorException>(() => Injector.CheckAndInjectAll());
        }

        [Test]
        public void GetChildren_ShouldThrowException_WhenFoundMany_WithoutName()
        {
            ComponentChildWithoutName injectable = InjectableBuilder<ComponentChildWithoutName>.Create().Build();

            // Add two child with good type
            new GameObject().AddComponent<Slider>().transform.SetParent(injectable.transform);
            new GameObject().AddComponent<Slider>().transform.SetParent(injectable.transform);

            // Act
            NixInjector Injector = new NixInjector(injectable);

            Assert.Throws<NixInjectorException>(() => Injector.CheckAndInjectAll());
        }

        [Test]
        public void GetParent_ShouldFindUniqueElement_WithoutName()
        {
            ComponentParentWithoutName injectable = InjectableBuilder<ComponentParentWithoutName>.Create().Build();
            Slider parentSlider = new GameObject().AddComponent<Slider>();
            injectable.transform.SetParent(parentSlider.transform);

            Assert.Null(injectable.Slider);
            Assert.Null(injectable.SliderWithEmptyString);
            Assert.Null(injectable.SliderInterface);
            Assert.Null(injectable.SliderInterfaceWithEmptyString);

            // Act
            NixInjector Injector = new NixInjector(injectable);
            Injector.CheckAndInjectAll();

            // Check
            Assert.AreEqual(parentSlider.GetInstanceID(), injectable.Slider.GetInstanceID());
            Assert.AreEqual(parentSlider.GetInstanceID(), injectable.SliderWithEmptyString.GetInstanceID());
            Assert.NotNull(injectable.SliderInterface);
            Assert.NotNull(injectable.SliderInterfaceWithEmptyString);
        }

        [Test]
        public void GetParent_ShouldThrowException_WhenNotFound()
        {
            ComponentParentWithoutName injectable = InjectableBuilder<ComponentParentWithoutName>.Create().Build();

            NixInjector Injector = new NixInjector(injectable);

            Assert.Throws<NixInjectorException>(() => Injector.CheckAndInjectAll());
        }

        [Test]
        public void GetParent_ShouldThrowException_WhenFoundMany_WithoutName()
        {
            ComponentParentWithoutName injectable = InjectableBuilder<ComponentParentWithoutName>.Create().Build();

            // Add two parent with good type
            Slider parent = new GameObject().AddComponent<Slider>();
            Slider grandParent = new GameObject().AddComponent<Slider>();

            parent.transform.SetParent(grandParent.transform);
            injectable.transform.SetParent(parent.transform);

            // Act
            NixInjector Injector = new NixInjector(injectable);

            Assert.Throws<NixInjectorException>(() => Injector.CheckAndInjectAll());
        }
        #endregion NixInjectComponentFromMethod without name (should find unique)

        #region NixInjectComponentFromMethod should ignore itself
        [Test]
        public void GetChildren_ShouldNotFindElementWhenItsTargetingItself()
        {
            CannotFindInChildren injectable = CannotFindBuilder.Create().BuildForChildWithCurrentName();
            
            NixInjector Injector = new NixInjector(injectable);

            Assert.Throws<NixInjectorException>(() => Injector.CheckAndInjectAll());
        }

        [Test]
        public void GetParents_ShouldNotFindElementWhenItsTargetingItself()
        {
            CannotFindInParents injectable = CannotFindBuilder.Create().BuildForParentWithCurrentName();

            NixInjector Injector = new NixInjector(injectable);

            Assert.Throws<NixInjectorException>(() => Injector.CheckAndInjectAll());
        }
        #endregion NixInjectComponentFromMethod should ignore itself

        #region NixInjectOptions
        [Test]
        public void AuthorizeSerializedFieldWithNixiAttributesOption_ShouldAllowSerializeWithNixiAttribute_OnCheckAndInjectAll()
        {
            AuthorizeSerializeField injectable = InjectableBuilder<AuthorizeSerializeField>.Create().Build();
            injectable.gameObject.AddComponent<Slider>();
            Assert.Null(injectable.Slider);

            // Act
            NixInjector Injector = new NixInjector(injectable, new NixInjectOptions
            {
                AuthorizeSerializedFieldWithNixiAttributes = true
            });
            Injector.CheckAndInjectAll();

            // Check
            Assert.NotNull(injectable.Slider);
        }
        #endregion NixInjectOptions
    }
}
