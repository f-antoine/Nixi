using ScriptExample.Controllers;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tests.Builders
{
    internal sealed class SceneWithChildrenBuilder
    {
        private SceneWithChildProperties sceneWithChildProperties;

        private SceneWithChildrenBuilder()
        {
            sceneWithChildProperties = new SceneWithChildProperties
            {
                Scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single),
                MainRoot = new GameObject("MainRoot")
            };
        }

        internal static SceneWithChildrenBuilder Create()
        {
            return new SceneWithChildrenBuilder();
        }

        internal SceneWithChildProperties Build()
        {
            return sceneWithChildProperties;
        }

        internal SceneWithChildrenBuilder WithDuplicateRoot()
        {
            new GameObject(sceneWithChildProperties.MainRoot.name);
            return this;
        }

        internal SceneWithChildrenBuilder WithRootName(string rootName)
        {
            sceneWithChildProperties.MainRoot.name = rootName;
            return this;
        }

        internal SceneWithChildrenBuilder WithChildMonsterController()
        {
            // Added automatically on the scene
            sceneWithChildProperties.ChildMonsterController = new GameObject("ChildMonsterController").AddComponent<MonsterControllerWithChildren>();
            sceneWithChildProperties.ChildMonsterController.transform.SetParent(sceneWithChildProperties.MainRoot.transform);
            return this;
        }

        internal SceneWithChildrenBuilder WithChildSorcererController(string forcedName = "ChildSorcererController")
        {
            // Added automatically on the scene
            sceneWithChildProperties.ChildSorcererController = new GameObject(forcedName).AddComponent<SorcererControllerWithChildren>();
            sceneWithChildProperties.ChildSorcererController.transform.SetParent(sceneWithChildProperties.MainRoot.transform);
            return this;
        }

        internal SceneWithChildrenBuilder WithDuplicateChildSorcererController()
        {
            // Added automatically on the scene
            SorcererControllerWithChildren duplicatedSorcererController = new GameObject(sceneWithChildProperties.ChildSorcererController.name).AddComponent<SorcererControllerWithChildren>();
            duplicatedSorcererController.transform.SetParent(sceneWithChildProperties.MainRoot.transform);
            return this;
        }
    }

    internal sealed class SceneWithChildProperties
    {
        public Scene Scene;
        public GameObject MainRoot;
        public MonsterControllerWithChildren ChildMonsterController;
        public SorcererControllerWithChildren ChildSorcererController;
    }
}