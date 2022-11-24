namespace Nixi.Containers.ContainerElements
{
    /// <summary>
    /// Represents a container element, this is intended to associate in a one way direction a KeyType with a ValueType
    /// This suit a singleton approach, the instance is registered in this container element
    /// </summary>
    internal class SingleContainerElement : ContainerElement
    {
        /// <summary>
        /// Instance of the singleton
        /// </summary>
        public object Instance { get; set; }
    }
}