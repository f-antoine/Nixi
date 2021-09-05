namespace Nixi.Injections.Attributes.MonoBehaviours
{
    /// <summary>
    /// Method to use on root GameObjects from the current scene to find a GameObject that match GameObjectName
    /// <para/><see cref="UnityEngine.SceneManagement.SceneManager.GetActiveScene()"/>
    /// <para/>Then from the scene result :
    /// <para/><see cref="UnityEngine.SceneManagement.Scene.GetRootGameObjects()"/>
    /// </summary>
    public enum RootGameObjectMethod
    {
        /// <summary>
        /// Will call SceneManager.GetActiveScene().GetRootGameObjects(), then find the single GameObject which has for name RootName
        /// and will call GetComponents on it (Unity dependency injection way) to fill the decorated field in the instance of the class derived from MonoBehaviourInjectable
        /// </summary>
        GetComponentsInRoot = 0,

        /// <summary>
        /// Will call SceneManager.GetActiveScene().GetRootGameObjects(), then find the single GameObject which has for name RootName
        /// and will call GetComponentsInChildren on it (Unity dependency injection way) to fill the decorated field in the instance of the class derived from MonoBehaviourInjectable
        /// </summary>
        GetComponentsInChildrenFromRoot = 1
    }
}