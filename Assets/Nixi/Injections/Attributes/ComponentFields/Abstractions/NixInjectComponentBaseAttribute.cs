using System;
using System.Reflection;

namespace Nixi.Injections.Abstractions
{
    /// <summary>
    /// Base attribute thats represent a Unity dependency injection to retrieve a UnityEngine.Component
    /// (or a component that implements an interface type) present in the scene
    /// <para/>All attributes derived from this base attribute must be used on fields with type derived
    /// from UnityEngine.Component (or to target a component that implements an interface type)
    /// in a class derived from MonoBehaviourInjectable
    /// <para/>This base attribute can be used on IEnumerable of components
    /// (or to target multiple components that implement an interface type) fields too
    /// <para/>In play mode scene, it uses Unity dependency injection method to get the Component
    /// <para/>In tests, a component is created and you can get it with GetComponent
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public abstract class NixInjectComponentBaseAttribute : NixInjectAbstractBaseAttribute
    {
        /// <summary>
        /// Finds the component(s) that exactly matches criteria of a derived attribute from NixInjectComponentBaseAttribute using the corresponding Unity dependency injection method
        /// </summary>
        /// <param name="injectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <returns>Component(s) that exactly matches criteria of a derived attribute from NixInjectComponentBaseAttribute using the corresponding Unity dependency injection method</returns>
        public abstract object GetComponentResult(MonoBehaviourInjectable injectable, FieldInfo componentField);
    }
}