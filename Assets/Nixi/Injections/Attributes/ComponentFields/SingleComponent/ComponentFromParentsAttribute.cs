using Nixi.Injections.Attributes.ComponentFields.SingleComponent.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Nixi.Injections.Attributes.ComponentFields.SingleComponent
{
    /// <summary>
    /// Attribute used to represent an Unity dependency injection to get a single UnityEngine.Component
    /// (or to target a component that implement an interface type)
    /// <para/>This one is used to retrieve all the UnityEngine.Component from GetComponentsInParent call on current GameObject (excluding itself)
    /// and get the one that matches the type and gameObjectName
    /// <para/>This attribute must be used on a field in a class derived from MonoBehaviourInjectable
    /// <para/>In play mode scene, it uses Unity dependency injection method to get the Component
    /// <para/>In tests, a component is created and you can get it with GetComponent
    /// <para/><see cref="ComponentAttribute">Use ComponentAttribute to use GetComponents from current gameObject</see>
    /// <para/><see cref="ComponentFromChildrenAttribute">Use ComponentFromChildrenAttribute to handle children only (excluding current gameObject)</see>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ComponentFromParentsAttribute : NixInjectComponentExcludingItselfBaseAttribute
    {
        /// <summary>
        /// Define which Unity dependency injection method has to be called in order to get the components at the targeted level (current, child, parent)
        /// <para/>This one is at parent level (executing GetComponentsInParent from current GameObject excluding itself)
        /// <para/><see cref="Enums.GameObjectLevel">Look at GameObjectLevel for more information about levels</see>
        /// </summary>
        protected override Func<Component, Component[]> MethodToGetComponents
            => (injectable) => injectable.GetComponentsInParent(FieldType, IncludeInactive);

        /// <summary>
        /// Attribute used to represent an Unity dependency injection to get a single UnityEngine.Component
        /// (or to target a component that implement an interface type)
        /// <para/>This one is used to retrieve all the UnityEngine.Component from GetComponentsInParent call on current GameObject (excluding itself)
        /// and get the one that matches the type and gameObjectName
        /// <para/>This attribute must be used on a field in a class derived from MonoBehaviourInjectable
        /// <para/><see cref="ComponentAttribute">Use ComponentAttribute to use GetComponents from current gameObject</see>
        /// <para/><see cref="ComponentFromChildrenAttribute">Use ComponentFromChildrenAttribute to handle children only (excluding current gameObject)</see>
        /// </summary>
        /// <param name="gameObjectName">Name of the GameObject to find</param>
        /// <param name="includeInactive">Define if method calls with Unity dependency injection way include inactive GameObject in the search or not</param>
        public ComponentFromParentsAttribute(string gameObjectName = null, bool includeInactive = true)
            : base(gameObjectName, includeInactive)
        {
        }
    }

    #region Obsolete version
    /// <summary>
    /// Attribute used to represent an Unity dependency injection to get a single UnityEngine.Component
    /// (or to target a component that implement an interface type)
    /// <para/>This one is used to retrieve all the UnityEngine.Component from GetComponentsInParent call on current GameObject (excluding itself)
    /// and get the one that matches the type and gameObjectName
    /// <para/>This attribute must be used on a field in a class derived from MonoBehaviourInjectable
    /// <para/>In play mode scene, it uses Unity dependency injection method to get the Component
    /// <para/>In tests, a component is created and you can get it with GetComponent
    /// <para/><see cref="NixInjectComponentAttribute">Use NixInjectComponentAttribute to use GetComponents from current gameObject</see>
    /// <para/><see cref="NixInjectComponentFromChildrenAttribute">Use NixInjectComponentFromChildrenAttribute to handle children only (excluding current gameObject)</see>
    /// </summary>
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    [Obsolete("Will be replaced with a shorter version : ComponentFromParents")]
    public sealed class NixInjectComponentFromParentAttribute : ComponentFromParentsAttribute
    {
        /// <summary>
        /// Attribute used to represent an Unity dependency injection to get a single UnityEngine.Component
        /// (or to target a component that implement an interface type)
        /// <para/>This one is used to retrieve all the UnityEngine.Component from GetComponentsInParent call on current GameObject (excluding itself)
        /// and get the one that matches the type and gameObjectName
        /// <para/>This attribute must be used on a field in a class derived from MonoBehaviourInjectable
        /// <para/><see cref="NixInjectComponentAttribute">Use NixInjectComponentAttribute to use GetComponents from current gameObject</see>
        /// <para/><see cref="NixInjectComponentFromChildrenAttribute">Use NixInjectComponentFromChildrenAttribute to handle children only (excluding current gameObject)</see>
        /// </summary>
        /// <param name="gameObjectName">Name of the GameObject to find</param>
        /// <param name="includeInactive">Define if method calls with Unity dependency injection way include inactive GameObject in the search or not</param>
        public NixInjectComponentFromParentAttribute(string gameObjectName = null, bool includeInactive = true)
            : base(gameObjectName, includeInactive) { }
    }
    #endregion Obsolete version
}