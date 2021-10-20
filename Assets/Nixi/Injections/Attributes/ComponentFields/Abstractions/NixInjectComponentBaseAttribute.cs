using System;
using System.Reflection;

namespace Nixi.Injections.Attributes.Abstractions
{
    /// <summary>
    /// Base attribute to represent a dependency injection in a component (or interface) field of a class instance derived from MonoBehaviourInjectable with a Nixi approach
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public abstract class NixInjectComponentBaseAttribute : Attribute
    {
        /// <summary>
        /// Find component(s) which exactly matches criteria of a derived attribute from NixInjectComponentBaseAttribute using the Unity dependency injection method
        /// </summary>
        /// <param name="monoBehaviourInjectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <returns>Component(s) which exactly matches criteria of a NixInjectComponent injection using the Unity dependency injection method</returns>
        public abstract object GetComponentResult(MonoBehaviourInjectable monoBehaviourInjectable, FieldInfo componentField);
    }
}