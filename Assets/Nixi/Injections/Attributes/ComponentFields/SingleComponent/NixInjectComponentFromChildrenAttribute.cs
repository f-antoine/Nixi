using Nixi.Injections.Attributes.ComponentFields.SingleComponent.Abstractions;
using System;
using UnityEngine;

namespace Nixi.Injections.Attributes.ComponentFields.SingleComponent
{
    /// <summary>
    /// Attribute used to represent an Unity dependency injection to get a single UnityEngine.Component
    /// (or to target a component that implement an interface type)
    /// <para/>This one is used to retrieve all the UnityEngine.Component from GetComponentsInChildren call on current GameObject (excluding itself)
    /// and get the one that matches the type and gameObjectName
    /// <para/>This attribute must be used on a field in a class derived from MonoBehaviourInjectable
    /// <para/>In play mode scene, it uses Unity dependency injection method to get the Component
    /// <para/>In tests, a component is created and you can get it with GetComponent
    /// <para/><see cref="NixInjectComponentAttribute">Use NixInjectComponentAttribute to use GetComponents from current gameObject</see>
    /// <para/><see cref="NixInjectComponentFromParentAttribute">Use NixInjectComponentFromParentAttribute to handle parent only (excluding current gameObject)</see>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NixInjectComponentFromChildrenAttribute : NixInjectComponentExcludingItselfBaseAttribute
    {
        /// <summary>
        /// Define which Unity dependency injection method has to be called in order to get the components at the targeted level (current, child, parent)
        /// <para/>This one is at child level (executing GetComponentsInChildren from current GameObject excluding itself)
        /// <para/><see cref="GameObjectLevel">Look at GameObjectLevel for more information about levels</see>
        /// </summary>
        protected override Func<Component, Component[]> MethodToGetComponents
            => (injectable) => injectable.GetComponentsInChildren(FieldType, IncludeInactive);

        /// <summary>
        /// Attribute used to represent an Unity dependency injection to get a single UnityEngine.Component
        /// (or to target a component that implement an interface type)
        /// <para/>This one is used to retrieve all the UnityEngine.Component from GetComponentsInChildren call on current GameObject (excluding itself)
        /// and get the one that matches the type and gameObjectName
        /// <para/>This attribute must be used on a field in a class derived from MonoBehaviourInjectable
        /// <para/><see cref="NixInjectComponentAttribute">Use NixInjectComponentAttribute to use GetComponents from current gameObject</see>
        /// <para/><see cref="NixInjectComponentFromParentAttribute">Use NixInjectComponentFromParentAttribute to handle parent only (excluding current gameObject)</see>
        /// </summary>
        /// <param name="gameObjectName">Name of the GameObject to find</param>
        /// <param name="includeInactive">Define if method calls with Unity dependency injection way include inactive GameObject in the search or not</param>
        public NixInjectComponentFromChildrenAttribute(string gameObjectName = null, bool includeInactive = true)
            : base(gameObjectName, includeInactive)
        {
        }
    }
}