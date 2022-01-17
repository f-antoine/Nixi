using System.Collections.Generic;

namespace NixiTestTools.TestInjectorElements.Relations
{
    /// <summary>
    /// Each relation is a link between a parent element of type T and its list of child relation (recursively)
    /// </summary>
    internal class Relation<T>
    {
        /// <summary>
        /// Current element
        /// </summary>
        internal T Parent;

        /// <summary>
        /// All child elements created at parent level
        /// </summary>
        internal List<Relation<T>> Children = new List<Relation<T>>();
    }
}