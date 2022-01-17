using System.Reflection;

namespace NixiTestTools.TestInjectorElements.Relations.Fields
{
    /// <summary>
    /// Wrap and handle a FieldInfo where a value injection is performed
    /// </summary>
    // This is easier than creating a class that inherits from FieldInfo which requires implementing many abstract methods/properties
    internal class SimpleFieldInfo
    {
        /// <summary>
        /// Field where a value injection is performed
        /// </summary>
        public FieldInfo FieldInfo { get; set; }
    }
}