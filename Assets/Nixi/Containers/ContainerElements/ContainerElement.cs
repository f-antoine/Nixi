using System;

namespace Nixi.Containers.ContainerElements
{
    /// <summary>
    /// Represents a container element, this is intended to associate in a one way direction a KeyType with a ValueType
    /// </summary>
    internal abstract class ContainerElement
    {
        /// <summary>
        /// Key type of the mapping
        /// </summary>
        public Type KeyType { get; set; }

        /// <summary>
        /// Value type of the mapping
        /// </summary>
        public Type ValueType { get; set; }

        /// <summary>
        /// Constructor parameters
        /// </summary>
        public object[] ConstructorParameters { get; set; }
    }
}