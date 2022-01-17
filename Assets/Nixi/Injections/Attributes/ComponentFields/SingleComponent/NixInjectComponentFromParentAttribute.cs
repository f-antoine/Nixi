using Nixi.Injections.ComponentFields.SingleComponent.Abstractions;
using System;
using System.Reflection;
using UnityEngine;

namespace Nixi.Injections
{
    /// <summary>
    /// Attribute used to represent an Unity dependency injection to get a single UnityEngine.Component
    /// (or to target a component that implement an interface type)
    /// <para/>This one is used to retrieve all the UnityEngine.Component from GetComponentsInParent call on current GameObject (excluding itself)
    /// and get the one that matches the type and gameObjectName
    /// <para/>This attribute must be used on a field in a class derived from MonoBehaviourInjectable
    /// <para/>In play mode scene, it uses Unity dependency injection method to get the Component
    /// <para/>In tests, a component is created and you can get it with GetComponent
    /// <para/><see cref="NixInjectComponentAttribute">Use NixInjectComponentAttribute to use GetComponents from current gameObject</see>
    /// <para/><see cref="NixInjectComponentFromParentAttribute">Use NixInjectComponentFromParentAttribute to handle parent only (excluding current gameObject)</see>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NixInjectComponentFromParentAttribute : NixInjectComponentExcludingItselfBaseAttribute
    {
        /// <summary>
        /// Define which Unity dependency injection method has to be called in order to get the components at the targeted level (current, child, parent)
        /// <para/>This one is at parent level (executing GetComponentsInParent from current GameObject excluding itself)
        /// <para/><see cref="GameObjectLevel">Look at GameObjectLevel for more information about levels</see>
        /// </summary>
        protected override Func<MonoBehaviourInjectable, FieldInfo, Component[]> MethodToGetComponents
            => (injectable, componentFieldInfo) => injectable.GetComponentsInParent(componentFieldInfo.FieldType, IncludeInactive);

        /// <summary>
        /// Attribute used to represent an Unity dependency injection to get a single UnityEngine.Component
        /// (or to target a component that implement an interface type)
        /// <para/>This one is used to retrieve all the UnityEngine.Component from GetComponentsInParent call on current GameObject (excluding itself)
        /// and get the one that matches the type and gameObjectName
        /// <para/>This attribute must be used on a field in a class derived from MonoBehaviourInjectable
        /// <para/><see cref="NixInjectComponentAttribute">Use NixInjectComponentAttribute to use GetComponents from current gameObject</see>
        /// <para/><see cref="NixInjectComponentFromParentAttribute">Use NixInjectComponentFromParentAttribute to handle parent only (excluding current gameObject)</see>
        /// </summary>
        /// <param name="gameObjectName">Name of the GameObject to find</param>
        /// <param name="includeInactive">Define if method calls with Unity dependency injection way include inactive GameObject in the search or not</param>
        public NixInjectComponentFromParentAttribute(string gameObjectName = null, bool includeInactive = true)
            : base(gameObjectName, includeInactive)
        {
        }
    }
}