using Nixi.Injections.Attributes.ComponentFields.MultiComponents.Abstractions;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Nixi.Injections.Attributes
{
    /// <summary>
    /// Attribute to represent a dependency injection on an enumerable of component (or interface) field of an instance of a class derived from MonoBehaviourInjectable
    /// <para/>This one has logic using GetComponents from current gameObject
    /// <para/>It handles IEnumerable, arrays and Lists
    /// <para/><see cref="NixInjectComponentsFromParentAttribute">Use NixInjectComponentsFromParentAttribute to handle parent only (excluding current gameObject)</see>
    /// <para/><see cref="NixInjectComponentsFromChildrenAttribute">Use NixInjectComponentsFromChildrenAttribute to handle children only (excluding current gameObject)</see>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NixInjectComponentsAttribute : NixInjectMultiComponentsBaseAttribute
    {
        /// <summary>
        /// Used to identify at which level the fields are injected : current, parent (excluding current) or child (excluding current)
        /// </summary>
        public override GameObjectLevel GameObjectLevel => GameObjectLevel.Current;

        /// <summary>
        /// Find all the components which exactly matches criteria of a derived attribute from NixInjectComponentBaseAttribute using the Unity dependency injection method
        /// </summary>
        /// <param name="injectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <returns>Component(s) which exactly matches criteria of a NixInjectComponent injection using the Unity dependency injection method</returns>
        public override object GetComponentResult(MonoBehaviourInjectable injectable, FieldInfo componentField)
        {
            Component[] components = injectable.GetComponents(EnumerableType);

            // ToList cover all cases (thanks to ComponentBinder)
            return components.ToList();
        }
    }
}