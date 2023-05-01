namespace Nixi.Injections.Attributes.ComponentFields.Abstractions
{
    /// <summary>
    /// This interface means that the class derived from NixInjectComponentBaseAttribute has a gameObject name 
    /// that needs to be referenced when using TestInjector
    /// <para/>This help to simulate Unity context during tests
    /// </summary>
    public interface IHaveGameObjectNameToFind
    {
        /// <summary>
        /// Name of the component to find
        /// </summary>
        string GameObjectNameToFind { get; }
    }
}