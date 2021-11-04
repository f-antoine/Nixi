using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Nixi.Injections.Attributes.ComponentFields.MultiComponents.Abstractions
{
    /// <summary>
    /// Attribute to represent a dependency injection on an enumerable of component (or interface) field of an instance of a class derived from MonoBehaviourInjectable
    /// <para/>This one has logic using GetComponentsInChildren and GetComponentsInParent on current gameObject, excluding the current gameObject in the search
    /// <para/>It handles IEnumerable, arrays and Lists
    /// <para/><see cref="NixInjectComponentsAttribute">Use NixInjectComponentsAttribute to handle current level</see>
    /// <para/><see cref="NixInjectComponentsFromChildrenAttribute">Use NixInjectComponentsFromChildrenAttribute to have more information about children level</see>
    /// <para/><see cref="NixInjectComponentsFromParentAttribute">Use NixInjectComponentsFromParentAttribute to have more information about parent level</see>
    /// </summary>
    public abstract class NixInjectComponentsExcludingItselfBaseAttribute : NixInjectMultiComponentsBaseAttribute
    {
        /// <summary>
        /// Define which Unity dependency injection method has to be called in order to get the components at the targeted level (current, child, parent)
        /// <para/><see cref="GameObjectLevel">Look at GameObjectLevel for more information about levels</see>
        /// </summary>
        protected abstract Func<MonoBehaviourInjectable, Component[]> MethodToGetComponents { get; }

        // TODO : Implement and test at the moment where regex will be implemented
        /// <summary>
        /// Define if method calls with Unity dependency injection way include inactive GameObject in the search or not
        /// </summary>
        protected bool IncludeInactive = true;

        /// <summary>
        /// Find all the components which exactly matches criteria of a derived attribute from NixInjectComponentBaseAttribute using the Unity dependency injection method
        /// </summary>
        /// <param name="injectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <returns>Component(s) which exactly matches criteria of a NixInjectComponent injection using the Unity dependency injection method</returns>
        public override object GetComponentResult(MonoBehaviourInjectable injectable, FieldInfo componentField)
        {
            Component[] components = MethodToGetComponents(injectable);

            // Ignore itself, ToList cover all cases (thanks to ComponentBinder)
            return components.Where(x => x.gameObject.GetInstanceID() != injectable.gameObject.GetInstanceID()).ToList();
        }
    }
}