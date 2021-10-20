using Assets.ScriptExample.Controllers;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tests.Builders
{
    internal sealed class SceneWithChildsBuilder
    {
        private SceneWithChildProperties sceneWithChildProperties;

        private SceneWithChildsBuilder()
        {
            sceneWithChildProperties = new SceneWithChildProperties
            {
                Scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single),
                MainRoot = new GameObject("MainRoot")
            };
        }

        internal static SceneWithChildsBuilder Create()
        {
            return new SceneWithChildsBuilder();
        }

        internal SceneWithChildProperties Build()
        {
            return sceneWithChildProperties;
        }

        internal SceneWithChildsBuilder WithDuplicateRoot()
        {
            new GameObject(sceneWithChildProperties.MainRoot.name);
            return this;
        }

        internal SceneWithChildsBuilder WithRootName(string rootName)
        {
            sceneWithChildProperties.MainRoot.name = rootName;
            return this;
        }

        internal SceneWithChildsBuilder WithChildMonsterController()
        {
            // Added automatically on the scene
            sceneWithChildProperties.ChildMonsterController = new GameObject("ChildMonsterController").AddComponent<MonsterControllerWithChilds>();
            sceneWithChildProperties.ChildMonsterController.transform.SetParent(sceneWithChildProperties.MainRoot.transform);
            return this;
        }

        internal SceneWithChildsBuilder WithChildSorcererController(string forcedName = "ChildSorcererController")
        {
            // Added automatically on the scene
            sceneWithChildProperties.ChildSorcererController = new GameObject(forcedName).AddComponent<SorcererControllerWithChilds>();
            sceneWithChildProperties.ChildSorcererController.transform.SetParent(sceneWithChildProperties.MainRoot.transform);
            return this;
        }

        internal SceneWithChildsBuilder WithDuplicateChildSorcererController()
        {
            // Added automatically on the scene
            SorcererControllerWithChilds duplicatedSorcererController = new GameObject(sceneWithChildProperties.ChildSorcererController.name).AddComponent<SorcererControllerWithChilds>();
            duplicatedSorcererController.transform.SetParent(sceneWithChildProperties.MainRoot.transform);
            return this;
        }
    }

    internal sealed class SceneWithChildProperties
    {
        public Scene Scene;
        public GameObject MainRoot;
        public MonsterControllerWithChilds ChildMonsterController;
        public SorcererControllerWithChilds ChildSorcererController;
    }
}