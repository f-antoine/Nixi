using System.Collections.Generic;
using UnityEngine;

namespace NixiTestTools.TestInjecterElements.Relations.RootRelations
{
    /// <summary>
    /// Associate a list of components with name of gameObject which owns the components
    /// </summary>
    internal sealed class ComponentsWithName
    {
        /// <summary>
        /// All components instantiated
        /// </summary>
        private List<Component> components = new List<Component>();

        /// <summary>
        /// All components instantiated
        /// </summary>
        internal IReadOnlyList<Component> Components => components;

        /// <summary>
        /// Name of gameObject which owns the components
        /// </summary>
        internal string Name { get; private set; }

        /// <summary>
        /// Build a ComponentsWithName, name will be stored and component is optional, if null, nothing is register
        /// </summary>
        /// <param name="name">Name of the gameObject</param>
        /// <param name="component">Optional parameter, this is the first component associated to the gameObject</param>
        internal ComponentsWithName(string name, Component component = null)
        {
            Name = name;

            if (component != null)
                components.Add(component);
        }

        /// <summary>
        /// Add a component to the list of component
        /// </summary>
        /// <param name="componentToAdd">Component to add to the list</param>
        internal void AddComponent(Component componentToAdd)
        {
            components.Add(componentToAdd);
        }
    }
}
