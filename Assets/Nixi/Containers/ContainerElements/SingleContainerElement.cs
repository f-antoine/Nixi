namespace Nixi.Containers.ContainerElements
{
    /// <summary>
    /// Represent a container element, this aims to associate in a one way direction a KeyType with a ValueType
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