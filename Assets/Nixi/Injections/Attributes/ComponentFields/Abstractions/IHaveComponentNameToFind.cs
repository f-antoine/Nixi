namespace Nixi.Injections.Attributes.Abstractions
{
    /// <summary>
    /// Interface for handling property ComponentNameToFind on attribute derived from NixInjectComponentBaseAttribute.
    /// <para/>This will be treated in TestInjecter which link ComponentName with TestInjecter.GetComponent to simplify access with component name in tests
    /// </summary>
    public interface IHaveComponentNameToFind
    {
        /// <summary>
        /// Name of the component to find
        /// </summary>
        public string ComponentNameToFind { get; }
    }
}