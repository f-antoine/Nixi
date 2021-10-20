using Nixi.Injections.Attributes.Abstractions;
using Nixi.Injections.Extensions;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Nixi.Injections.Attributes
{
    /// <summary>
    /// Attribute used to define a dependency injection in a field which is enumerable of component (or interface) in a class instance derived from MonoBehaviourInjectable
    /// with Unity dependency injection approach
    /// <para/>This one use GetComponentsInChildren(gameObjectTypeToFind) from the instance of the class derived from MonoBehaviourInjectable and fill the enumerable component (or interface )field
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NixInjectComponentListAttribute : NixInjectComponentBaseAttribute
    {
        /// <summary>
        /// Define if method calls with Unity dependency injection way include inactive GameObject in the search or not
        /// </summary>
        public bool IncludeInactive { get; private set; }

        /// <summary>
        /// Attribute used to define a dependency injection in a field which is enumerable of component (or interface) in a class instance derived from MonoBehaviourInjectable
        /// with Unity dependency injection approach
        /// <para/>This one use GetComponentsInChildren(gameObjectTypeToFind) from the instance of the class derived from MonoBehaviourInjectable and fill the enumerable component (or interface )field
        /// </summary>
        /// <param name="includeInactive">Define if method calls with Unity dependency injection way include inactive GameObject in the search or not</param>
        public NixInjectComponentListAttribute(bool includeInactive = true)
        {
            IncludeInactive = includeInactive;
        }

        /// <summary>
        /// Find all the components that exactly match criteria of a derived attribute from NixInjectComponentAttribute using the Unity dependency injection method
        /// </summary>
        /// <param name="monoBehaviourInjectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <returns>All the components that exactly matches criteria of a NixInjectComponent injection using the Unity dependency injection method</returns>
        public override object GetComponentResult(MonoBehaviourInjectable monoBehaviourInjectable, FieldInfo componentField)
        {
            Type genericType = CheckEnumerableAndGetGenericType(componentField);

            Component[] result = monoBehaviourInjectable.GetComponentsInChildren(genericType, IncludeInactive);

            // Cover all cases
            return result.ToList();
        }

        /// <summary>
        /// Check if componentField is an IEnumerable of components or an IEnumerable of interfaces
        /// </summary>
        /// <param name="componentField">Component field to check</param>
        /// <returns>Generic type of the enumerable</returns>
        private Type CheckEnumerableAndGetGenericType(FieldInfo componentField)
        {
            Type enumerableGenericType = componentField.GetEnumerableUniqueGenericInterfaceOrComponentTypeIfExist();

            if (enumerableGenericType == null)
                throw new NixiAttributeException($"Cannot inject field with name {componentField.Name} and type {componentField.FieldType} with a NixInjectComponentListAttribute because it is not an IEnumerable of component (or interface) field");

            return enumerableGenericType;
        }
    }
}
