using System.Reflection;
using UnityEngine;

namespace NixiTestTools.TestInjecterElements
{
    /// <summary>
    /// Class which associates a GameObject with his fieldInfo and reduce the number of type resolution operations
    /// </summary>
    internal class GameObjectWithFieldInfo
    {
        /// <summary>
        /// Field which contains the GameObject
        /// </summary>
        internal FieldInfo FieldInfo { get; set; }

        /// <summary>
        /// GameObject passed in the field
        /// </summary>
        internal GameObject GameObject { get; set; }
    }
}
