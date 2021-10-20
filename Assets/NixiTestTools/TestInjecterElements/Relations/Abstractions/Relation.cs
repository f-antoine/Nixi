using System.Collections.Generic;

namespace NixiTestTools.TestInjecterElements.Relations.Abstractions
{
    /// <summary>
    /// A relation is a link between a parent element of type T and his list of child relation (recursively)
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
        internal List<Relation<T>> Childs = new List<Relation<T>>();
    }
}