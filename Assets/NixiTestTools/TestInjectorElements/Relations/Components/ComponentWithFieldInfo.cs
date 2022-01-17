using NixiTestTools.TestInjectorElements.Relations.Fields;
using UnityEngine;

namespace NixiTestTools.TestInjectorElements.Relations.Components
{
    /// <summary>
    /// Wrap and handle a component FieldInfo and its associated component to reduce the number of type resolution operations
    /// </summary>
    internal sealed class ComponentWithFieldInfo : SimpleFieldInfo
    {
        /// <summary>
        /// Component passed in the FieldInfo
        /// </summary>
        internal Component Component { get; set; }
    }
}