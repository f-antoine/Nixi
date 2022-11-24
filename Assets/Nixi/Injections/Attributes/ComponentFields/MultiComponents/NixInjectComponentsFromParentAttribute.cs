using Nixi.Injections.Attributes.ComponentFields.Enums;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nixi.Injections.Attributes.ComponentFields.MultiComponents
{
    /// <summary>
    /// Attribute used to represent an Unity dependency injection to get an enumerable of UnityEngine.Component
    /// (or to target multiple components that implement an interface type)
    /// <para/>This one has logic using GetComponentsInParent from current gameObject (excluding itself)
    /// <para/>It handles IEnumerable, arrays and Lists
    /// <para/>This attribute must be used on a field in a class derived from MonoBehaviourInjectable
    /// <para/>In play mode scene, it uses Unity dependency injection method to get the Component
    /// <para/>In tests, a component is created and you can get it with GetComponent
    /// <para/><see cref="NixInjectComponentsAttribute">Use NixInjectComponentsAttribute to use GetComponents from current gameObject</see>
    /// <para/><see cref="NixInjectComponentsFromChildrenAttribute">Use NixInjectComponentsFromParentAttribute to handle children only (excluding current gameObject)</see>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NixInjectComponentsFromParentAttribute : NixInjectMultiComponentsBaseAttribute
    {
        /// <summary>
        /// Used to identify at which level the fields are injected, this is one is at parent level (excluding current)
        /// </summary>
        public override GameObjectLevel GameObjectLevel => GameObjectLevel.Parent;

        // TODO : Implement when regex will be implemented
        /// <summary>
        /// Define if method calls with Unity dependency injection way include inactive GameObject in the search or not
        /// </summary>
        private bool IncludeInactive = true;

        /// <summary>
        /// Unity dependency injection method called for this attribute is : GetComponentsInParent (excluding itself)
        /// <para/><see cref="Injections.GameObjectLevel">Look at GameObjectLevel for more information about levels</see>
        /// </summary>
        protected override Func<Component, IEnumerable<Component>> MethodToGetComponents
            => (injectable) => injectable.GetComponentsInParent(EnumerableType, IncludeInactive)
                                         .Where(x => x.gameObject.GetInstanceID() != injectable.gameObject.GetInstanceID());
    }
}