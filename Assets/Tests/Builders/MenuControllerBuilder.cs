using ScriptExample.Menu;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Tests.Builders
{
    internal sealed class MenuControllerBuilder
    {
        private GameObject menuController;

        private GameObject rootObject;
        private GameObject lastChild;

        private MenuControllerBuilder(string rootName)
        {
            // New Scene for each test iteration, because if build many rootObject with same name
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            menuController = new GameObject();

            rootObject = new GameObject(rootName);
            lastChild = rootObject.gameObject;
        }

        internal static MenuControllerBuilder Create(string rootName)
        {
            return new MenuControllerBuilder(rootName);
        }

        internal MenuController Build()
        {
            menuController.name = "MenuController";
            return menuController.AddComponent<MenuController>();
        }

        internal MenuControllerWithInactive BuildWithInactive()
        {
            menuController.name = "MenuControllerWithInactive";
            return menuController.AddComponent<MenuControllerWithInactive>();
        }

        internal MenuControllerBuilder AddGameObjectLevelOnRoot<T>(string name, bool isActive = true)
            where T : Component
        {
            T component = new GameObject(name).AddComponent<T>();
            component.transform.SetParent(lastChild.transform);
            component.gameObject.SetActive(isActive);
            lastChild = component.gameObject;
            return this;
        }
    }
}