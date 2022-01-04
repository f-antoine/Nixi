using Nixi.Injections.ComponentFields.SingleComponent.Abstractions;
using System;
using System.Reflection;
using UnityEngine;

namespace Nixi.Injections
{
    /// <summary>
    /// Attribute to represent a dependency injection on a component (or interface) field of an instance of a class derived from MonoBehaviourInjectable
    /// <para/>This one has logic using GetComponentsInChildren from current gameObject (excluding itself)
    /// <para/>It handles single component/interface field
    /// <para/><see cref="NixInjectComponentAttribute">Use NixInjectComponentAttribute to use GetComponents from current gameObject</see>
    /// <para/><see cref="NixInjectComponentFromParentAttribute">Use NixInjectComponentFromParentAttribute to handle parent only (excluding current gameObject)</see>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NixInjectComponentFromChildrenAttribute : NixInjectComponentExcludingItselfBaseAttribute
    {
        /// <summary>
        /// Used to identify at which level the fields are injected : current, parent (excluding current) or child (excluding current)
        /// </summary>
        protected override Func<MonoBehaviourInjectable, FieldInfo, Component[]> MethodToGetComponents
            => (injectable, componentFieldInfo) => injectable.GetComponentsInChildren(componentFieldInfo.FieldType, IncludeInactive);

        /// <summary>
        /// Target single component that match gameObjectNameToFind and type returned from Unity method : GetComponentsInChildren, it excludes itself
        /// </summary>
        /// <param name="gameObjectName">Name of the GameObject to find</param>
        /// <param name="includeInactive">Define if method calls with Unity dependency injection way include inactive GameObject in the search or not</param>
        public NixInjectComponentFromChildrenAttribute(string gameObjectName = null, bool includeInactive = true)
            : base(gameObjectName, includeInactive)
        {
        }
    }
}