using System.Reflection;

namespace NixiTestTools.TestInjecterElements.Relations.Abstractions
{
    /// <summary>
    /// Encapsulate and handle a FieldInfo
    /// </summary>
    internal class SimpleFieldInfo
    {
        /// <summary>
        /// Field which contains the value injected
        /// </summary>
        public FieldInfo FieldInfo { get; set; }
    }
}