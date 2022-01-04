using Nixi.Injections.ComponentFields.SingleComponent.Abstractions;
using System;
using System.Reflection;
using UnityEngine;

namespace Nixi.Injections
{
    /// <summary>
    /// Attribute to represent a dependency injection on a component (or interface) field of an instance of a class derived from MonoBehaviourInjectable
    /// <para/>This one has logic using GetComponentsInParent from current gameObject (excluding itself)
    /// <para/>It handles single component/interface field
    /// <para/><see cref="NixInjectComponentAttribute">Use NixInjectComponentAttribute to use GetComponents from current gameObject</see>
    /// <para/><see cref="NixInjectComponentFromChildrenAttribute">Use NixInjectComponentFromChildrenAttribute to handle parent only (excluding current gameObject)</see>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NixInjectComponentFromParentAttribute : NixInjectComponentExcludingItselfBaseAttribute
    {
        /// <summary>
        /// Used to identify at which level the fields are injected : current, parent (excluding current) or child (excluding current)
        /// </summary>
        protected override Func<MonoBehaviourInjectable, FieldInfo, Component[]> MethodToGetComponents
            => (injectable, componentFieldInfo) => injectable.GetComponentsInParent(componentFieldInfo.FieldType, IncludeInactive);

        /// <summary>
        /// Target single component that match gameObjectNameToFind and type returned from Unity method : GetComponentsInParent, it excludes itself
        /// </summary>
        /// <param name="gameObjectName">Name of the GameObject to find</param>
        /// <param name="includeInactive">Define if method calls with Unity dependency injection way include inactive GameObject in the search or not</param>
        public NixInjectComponentFromParentAttribute(string gameObjectName = null, bool includeInactive = true)
            : base(gameObjectName, includeInactive)
        {
        }
    }
}