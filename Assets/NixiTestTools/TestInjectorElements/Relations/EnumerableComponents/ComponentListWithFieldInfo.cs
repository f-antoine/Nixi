using Nixi.Injections;
using NixiTestTools.TestInjectorElements.Relations.Fields;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NixiTestTools.TestInjectorElements.Relations.Components
{
    /// <summary>
    /// Wrap and handle an Enumerable, a List or an array of Component into a FieldInfo.
    /// All components are stored in this class to reduce the number of type resolution operations
    /// </summary>
    internal sealed class ComponentListWithFieldInfo : SimpleFieldInfo
    {
        /// <summary>
        /// Type of enumerable associated to EnumerableFieldInfo (genericType/elementType if array)
        /// </summary>
        internal Type EnumerableType { get; set; }

        /// <summary>
        /// List of components passed in the FieldInfo
        /// </summary>
        internal List<Component> Components { get; set; } = new List<Component>();

        /// <summary>
        /// Used to identify at which level the enumerable field of UnityEngine.Component (or interface attached  
        /// </summary>
        internal GameObjectLevel GameObjectLevel { get; set; }
    }
}