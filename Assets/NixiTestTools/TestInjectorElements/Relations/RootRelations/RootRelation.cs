using System.Collections.Generic;

namespace NixiTestTools.TestInjectorElements.Relations.RootRelations
{
    /// <summary>
    /// A RootRelation is a link between a parent element of type T and its list of child relation 
    /// (not recursively because children is not List of RootRelation but List of ComponentsWithName)
    /// <para/>Currently, only one level of relation is used (rootComponent name and one child of type, with name under the rootComponent with name)
    /// </summary>
    internal sealed class RootRelation
    {
        /// <summary>
        /// Root parent
        /// </summary>
        internal ComponentsWithName Parent { get; private set; }

        /// <summary>
        /// All children linked to Parent
        /// </summary>
        private List<ComponentsWithName> children = new List<ComponentsWithName>();

        /// <summary>
        /// All children linked to Parent
        /// </summary>
        public IReadOnlyList<ComponentsWithName> Children => children.AsReadOnly();

        /// <summary>
        /// Build a RootRelation without children linked
        /// </summary>
        /// <param name="parent">Parent ComponentsWithName</param>
        public RootRelation(ComponentsWithName parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Build a RootRelation with children linked to parent
        /// </summary>
        /// <param name="parent">Parent ComponentsWithName</param>
        /// <param name="children">Children linked</param>
        public RootRelation(ComponentsWithName parent, List<ComponentsWithName> children)
        {
            Parent = parent;
            this.children = children;
        }

        /// <summary>
        /// Add a child linked to Parent
        /// </summary>
        /// <param name="childComponent">Child added</param>
        public void AddChildComponent(ComponentsWithName childComponent)
        {
            children.Add(childComponent);
        }
    }
}