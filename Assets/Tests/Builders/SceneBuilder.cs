using Assets.ScriptExample.Controllers;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Tests.Builders
{
    internal sealed class SceneBuilder
    {
        private SceneWithProperties sceneWithProperties;

        private SceneBuilder()
        {
            sceneWithProperties = new SceneWithProperties
            {
                Scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single)
            };
        }

        internal static SceneBuilder Create()
        {
            return new SceneBuilder();
        }

        internal SceneWithProperties Build()
        {
            return sceneWithProperties;
        }

        internal SceneBuilder WithMonsterController()
        {
            // Added automatically on the scene
            sceneWithProperties.MonsterController = new GameObject("MonsterController").AddComponent<MonsterController>();
            return this;
        }

        internal SceneBuilder WithSorcererController()
        {
            // Added automatically on the scene
            sceneWithProperties.SorcererController = new GameObject("SorcererController").AddComponent<SorcererController>();
            return this;
        }

        internal SceneBuilder WithDuplicateSorcererControllerComponentAttached()
        {
            sceneWithProperties.SorcererController.gameObject.AddComponent<SorcererController>();
            return this;
        }
    }

    internal sealed class SceneWithProperties
    {
        public Scene Scene;
        public MonsterController MonsterController;
        public SorcererController SorcererController;
    }
}