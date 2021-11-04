using Nixi.Injections.Attributes.ComponentFields.MultiComponents.Abstractions;
using System;
using UnityEngine;

namespace Nixi.Injections.Attributes
{
    /// <summary>
    /// Attribute to represent a dependency injection on an enumerable of component (or interface) field of an instance of a class derived from MonoBehaviourInjectable
    /// <para/>This one has logic using GetComponentsInChildren from current gameObject (excluding itself)
    /// <para/>It handles IEnumerable, arrays and Lists
    /// <para/><see cref="NixInjectComponentsAttribute">Use NixInjectComponentsAttribute to use GetComponents from current gameObject</see>
    /// <para/><see cref="NixInjectComponentsFromParentAttribute">Use NixInjectComponentsFromParentAttribute to handle parent only (excluding current gameObject)</see>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NixInjectComponentsFromChildrenAttribute : NixInjectComponentsExcludingItselfBaseAttribute
    {
        /// <summary>
        /// Used to identify at which level the fields are injected : current, parent (excluding current) or child (excluding current)
        /// </summary>
        public override GameObjectLevel GameObjectLevel => GameObjectLevel.Children;

        /// <summary>
        /// Define which Unity dependency injection method has to be called in order to get the components at the targeted level (current, child, parent)
        /// <para/><see cref="Attributes.GameObjectLevel">Look at GameObjectLevel for more information about levels</see>
        /// </summary>
        protected override Func<MonoBehaviourInjectable, Component[]> MethodToGetComponents
            => (injectable) => injectable.GetComponentsInChildren(EnumerableType, IncludeInactive);
    }
}