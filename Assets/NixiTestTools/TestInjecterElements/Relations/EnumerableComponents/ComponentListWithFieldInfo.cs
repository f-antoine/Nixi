using Nixi.Injections.Attributes;
using NixiTestTools.TestInjecterElements.Relations.Abstractions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NixiTestTools.TestInjecterElements.Relations.Components
{
    /// <summary>
    /// Encapsulate and handle a component list FieldInfo and his associated list to reduce the number of type resolution operations
    /// </summary>
    internal sealed class ComponentListWithFieldInfo : SimpleFieldInfo
    {
        /// <summary>
        /// Type of enumerable associated to EnumerableFieldInfo
        /// </summary>
        internal Type EnumerableType { get; set; }

        /// <summary>
        /// List of components passed in the field
        /// </summary>
        internal List<Component> Components { get; set; } = new List<Component>();

        /// <summary>
        /// Method to use from a Component instance to get GameObjects
        /// </summary>
        internal GameObjectLevel GameObjectLevel { get; set; }
    }
}