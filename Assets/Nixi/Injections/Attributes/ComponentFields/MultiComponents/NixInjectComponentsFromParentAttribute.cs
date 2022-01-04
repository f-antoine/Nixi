using Nixi.Injections.ComponentFields.MultiComponents.Abstractions;
using System;
using UnityEngine;

namespace Nixi.Injections
{
    /// <summary>
    /// Attribute to represent a dependency injection on an enumerable of component (or interface) field of an instance of a class derived from MonoBehaviourInjectable
    /// <para/>This one has logic using GetComponentsInParent from current gameObject (excluding itself)
    /// <para/>It handles IEnumerable, arrays and Lists
    /// <para/><see cref="NixInjectComponentsAttribute">Use NixInjectComponentsAttribute to use GetComponents from current gameObject</see>
    /// <para/><see cref="NixInjectComponentsFromChildrenAttribute">Use NixInjectComponentsFromParentAttribute to handle children only (excluding current gameObject)</see>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NixInjectComponentsFromParentAttribute : NixInjectComponentsExcludingItselfBaseAttribute
    {
        /// <summary>
        /// Used to identify at which level the fields are injected : current, parent (excluding current) or child (excluding current)
        /// </summary>
        public override GameObjectLevel GameObjectLevel => GameObjectLevel.Parent;

        /// <summary>
        /// Define which Unity dependency injection method has to be called in order to get the components at the targeted level (current, child, parent)
        /// <para/><see cref="Injections.GameObjectLevel">Look at GameObjectLevel for more information about levels</see>
        /// </summary>
        protected override Func<MonoBehaviourInjectable, Component[]> MethodToGetComponents
            => (injectable) => injectable.GetComponentsInParent(EnumerableType, IncludeInactive);
    }
}