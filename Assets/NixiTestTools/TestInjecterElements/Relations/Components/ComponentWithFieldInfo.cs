using NixiTestTools.TestInjecterElements.Relations.Abstractions;
using UnityEngine;

namespace NixiTestTools.TestInjecterElements.Relations.Components
{
    /// <summary>
    /// Encapsulate and handle a component FieldInfo and his associated component to reduce the number of type resolution operations
    /// </summary>
    internal sealed class ComponentWithFieldInfo : SimpleFieldInfo
    {
        /// <summary>
        /// Component passed in the field
        /// </summary>
        internal Component Component { get; set; }
    }
}
