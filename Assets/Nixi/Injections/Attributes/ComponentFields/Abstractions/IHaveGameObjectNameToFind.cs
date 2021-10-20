namespace Nixi.Injections.Attributes.ComponentFields.Abstractions
{
    /// <summary>
    /// Interface for handling property GameObjectNameToFind on attribute derived from NixInjectComponentBaseAttribute.
    /// <para/>This will be treated in TestInjecter which link GameObjectName with TestInjecter.GetComponent to simplify access with GameObject name in tests
    /// </summary>
    public interface IHaveGameObjectNameToFind
    {
        /// <summary>
        /// Name of the GameObject to find
        /// </summary>
        public string GameObjectNameToFind { get; }
    }
}