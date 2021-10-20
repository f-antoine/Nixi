using System.Collections.Generic;

namespace NixiTestTools.TestInjecterElements.Relations.RootRelations
{
    /// <summary>
    /// A RootRelation is a link between a parent element of type T and his list of child relation 
    /// (not recursively because childs is not List of RootRelation but List of ComponentsWithName)
    /// <para/>Currently, only one level of relation is used (rootComponent name and one child of type, with name under the rootComponent with name)
    /// </summary>
    internal sealed class RootRelation
    {
        /// <summary>
        /// Root parent
        /// </summary>
        internal ComponentsWithName Parent { get; private set; }

        /// <summary>
        /// All childs of RootParent
        /// </summary>
        private List<ComponentsWithName> childs = new List<ComponentsWithName>();
        public IReadOnlyList<ComponentsWithName> Childs => childs.AsReadOnly();

        public RootRelation(ComponentsWithName parent)
        {
            Parent = parent;
        }

        public RootRelation(ComponentsWithName parent, List<ComponentsWithName> childs)
        {
            Parent = parent;
            this.childs = childs;
        }

        public void AddChildComponent(ComponentsWithName childComponent)
        {
            childs.Add(childComponent);
        }
    }
}