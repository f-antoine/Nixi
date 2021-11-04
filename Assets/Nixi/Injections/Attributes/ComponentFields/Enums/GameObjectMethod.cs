namespace Nixi.Injections.Attributes
{
    /// <summary>
    /// Method to use from a Component instance to get a GameObject
    /// </summary>
    public enum GameObjectMethod
    {
        /// <summary>
        /// Will call GetComponentInChildren to get GameObject which has for name GameObjectName (Unity dependency injection way) 
        /// from the instance of the class derived from MonoBehaviourInjectable which contains the decorated field to fill the field
        /// <para/> Contrary to Unity, this not include itself
        /// </summary>
        GetComponentsInChildren = 0,

        /// <summary>
        /// Will call GetComponentInParents to get GameObject which has for name GameObjectName (Unity dependency injection way) 
        /// from the instance of the class derived from MonoBehaviourInjectable which contains the decorated field to fill the field
        /// <para/> Contrary to Unity, this not include itself
        /// </summary>
        GetComponentsInParent = 1
    }
}