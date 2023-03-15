namespace Nixi.Injections
{
    /// <summary>
    /// Used to identify at which level the enumerable field of UnityEngine.Component (or interface attached 
    /// to these components) are injected : current (on gameObject), parent (excluding current) or child (excluding current)
    /// </summary>
    public enum GameObjectLevel
    {
        /// <summary>
        /// Target current level of the gameObject, executing GetComponents (Unity dependency injection)
        /// from the instance of the class derived from MonoBehaviourInjectable which contains the decorated field to fill the field
        /// </summary>
        Current = 0,

        /// <summary>
        /// Target child levels of the gameObject, executing GetComponentsInChildren (Unity dependency injection)
        /// from the instance of the class derived from MonoBehaviourInjectable which contains the decorated field to fill the field
        /// <para/> Contrary to Unity, this not include itself
        /// </summary>
        Children = 1,

        /// <summary>
        /// Target parent levels of the gameObject, executing GetComponentsInParent (Unity dependency injection)
        /// from the instance of the class derived from MonoBehaviourInjectable which contains the decorated field to fill the field
        /// <para/> Contrary to Unity, this not include itself
        /// </summary>
        Parent = 2
    }
}