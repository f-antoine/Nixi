using System.Reflection;

namespace NixiTestTools.TestInjecterElements.Relations.Abstractions
{
    /// <summary>
    /// Encapsulate and handle a FieldInfo (created to not have to implement every FieldInfo abstract methods/properties)
    /// </summary>
    internal class SimpleFieldInfo
    {
        /// <summary>
        /// Field which contains the value injected
        /// </summary>
        public FieldInfo FieldInfo { get; set; }
    }
}