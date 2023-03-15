using Nixi.Injections.Attributes.ComponentFields.Enums;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Nixi.Injections
{
    // TODO : Check tests diff
    /// <summary>
    /// Attribute used to represent an Unity dependency injection to get an enumerable of UnityEngine.Component
    /// (or to target multiple components that implement an interface type)
    /// <para/>This one has logic using GetComponents attached to current gameObject
    /// <para/>It handles IEnumerable, arrays and Lists
    /// <para/>This attribute must be used on a field in a class derived from MonoBehaviourInjectable
    /// <para/>In play mode scene, it uses Unity dependency injection method to get the Component
    /// <para/>In tests, a component is created and you can get it with GetComponent
    /// <para/><see cref="ComponentsFromParentsAttribute">Use ComponentsFromParentAttribute to handle parents only (excluding current gameObject)</see>
    /// <para/><see cref="ComponentsFromChildrenAttribute">Use ComponentsFromChildrenAttribute to handle children only (excluding current gameObject)</see>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ComponentsAttribute : NixInjectMultiComponentsBaseAttribute
    {
        /// <summary>
        /// Used to identify at which level the fields are injected, this is one is at current level
        /// </summary>
        public override GameObjectLevel GameObjectLevel => GameObjectLevel.Current;

        /// <summary>
        /// Unity dependency injection method called for this attribute is : GetComponents
        /// <para/><see cref="Injections.GameObjectLevel">Look at GameObjectLevel for more information about levels</see>
        /// </summary>
        protected override Func<Component, IEnumerable<Component>> MethodToGetComponents
            => (injectable) => injectable.GetComponents(EnumerableType);
    }

    #region Obsolete version
    /// <summary>
    /// Attribute used to represent an Unity dependency injection to get an enumerable of UnityEngine.Component
    /// (or to target multiple components that implement an interface type)
    /// <para/>This one has logic using GetComponents attached to current gameObject
    /// <para/>It handles IEnumerable, arrays and Lists
    /// <para/>This attribute must be used on a field in a class derived from MonoBehaviourInjectable
    /// <para/>In play mode scene, it uses Unity dependency injection method to get the Component
    /// <para/>In tests, a component is created and you can get it with GetComponent
    /// <para/><see cref="NixInjectComponentsFromParentAttribute">Use NixInjectComponentsFromParentAttribute to handle parents only (excluding current gameObject)</see>
    /// <para/><see cref="NixInjectComponentsFromChildrenAttribute">Use NixInjectComponentsFromChildrenAttribute to handle children only (excluding current gameObject)</see>
    /// </summary>
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    [Obsolete("Will be replaced with a shorter version : Components")]
    public sealed class NixInjectComponentsAttribute : ComponentsAttribute { }
    #endregion Obsolete version
}