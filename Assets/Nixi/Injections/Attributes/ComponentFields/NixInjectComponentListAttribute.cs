using Nixi.Injections.Attributes.Abstractions;
using System;
using System.Collections.Generic;
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
        // TODO : Implement and test at the moment where regex will be implemented
        /// <summary>
        /// Define if method calls with Unity dependency injection way include inactive GameObject in the search or not
        /// </summary>
        private bool IncludeInactive = true;

        /// <summary>
        /// Lock the generation of the enumerableType to only one call
        /// </summary>
        private bool enumerableTypeGenerated = false;

        /// <summary>
        /// If the decorated field is an IEnumerable or a list, this represents his single generic type,
        /// if this is an array it returns element type
        /// </summary>
        public Type EnumerableType { get; private set; } = null;

        /// <summary>
        /// Attribute used to define a dependency injection in a field which is enumerable of component (or interface) in a class instance derived from MonoBehaviourInjectable
        /// with Unity dependency injection approach
        /// <para/>This one use GetComponentsInChildren(gameObjectTypeToFind) from the instance of the class derived from MonoBehaviourInjectable and fill the enumerable component (or interface )field
        /// </summary>
        /// <param name="includeInactive">Define if method calls with Unity dependency injection way include inactive GameObject in the search or not</param>
        public NixInjectComponentListAttribute()
        {
        }

        /// <summary>
        /// Find all the components that exactly match criteria of a derived attribute from NixInjectComponentAttribute using the Unity dependency injection method
        /// </summary>
        /// <param name="monoBehaviourInjectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <returns>All the components that exactly matches criteria of a NixInjectComponent injection using the Unity dependency injection method</returns>
        public override object GetComponentResult(MonoBehaviourInjectable monoBehaviourInjectable, FieldInfo componentField)
        {
            Component[] result = monoBehaviourInjectable.GetComponentsInChildren(EnumerableType, IncludeInactive);

            // Cover all cases (thanks to ComponentBinder)
            return result.ToList();
        }

        /// <summary>
        /// Check if attribute decorate the right componentField.FieldType and setup data from component field into the nixi attribute component decorator
        /// </summary>
        /// <param name="componentField">Component field</param>
        public override void CheckIsValidAndBuildDataFromField(FieldInfo componentField)
        {
            if (!componentField.FieldType.IsArray && !CheckIfGenericEnumerableWithOnlyOneGenericArgument(componentField))
            {
                throw new NixiAttributeException($"Cannot inject component field with name {componentField.Name} and type {componentField.FieldType.Name}, " +
                                                 $"because it is not an array or IEnumerable/List (with a single generic argument) type while using decorator {GetType().Name}");
            }

            // Generate and check EnumerableType
            if (!enumerableTypeGenerated)
            {
                EnumerableType = GetEnumerableUniqueGenericInterfaceOrComponentTypeIfExist(componentField);

                if (!IsComponent(EnumerableType) && !EnumerableType.IsInterface)
                {
                    throw new NixiAttributeException($"Cannot inject component field with name {componentField.Name} and type {componentField.FieldType.Name}, because enumerable type is not a component or an interface while using decorator {GetType().Name}");
                }

                enumerableTypeGenerated = true;
            }
        }

        /// <summary>
        /// Check if a FieldInfo.FieldType is an IEnumerable and has exactly one generic argument (genericField must be check about field.FieldType.IsGenericType = true before)
        /// </summary>
        /// <param name="genericField">Generic field to check</param>
        /// <returns>True if this a generic field is an IEnumerable with exactly one generic argument</exception>
        private static bool CheckIfGenericEnumerableWithOnlyOneGenericArgument(FieldInfo genericField)
        {
            if (!genericField.FieldType.IsGenericType)
                return false;

            if (genericField.FieldType.GetGenericArguments().Length != 1)
                return false;

            Type genericType = genericField.FieldType.GetGenericTypeDefinition();
            bool isEnumerable = genericType == typeof(IEnumerable<>) || genericType == typeof(List<>);

            return isEnumerable;
        }

        /// <summary>
        /// If field.FieldType is an array or an IEnumerable and his unique generic argument is an Interface or a Component, it returns this unique generic argument or array type, null if not
        /// </summary>
        /// <param name="field">Field to check</param>
        /// <returns>Generic type of the enumerable if exists</returns>
        private static Type GetEnumerableUniqueGenericInterfaceOrComponentTypeIfExist(FieldInfo field)
        {
            if (field.FieldType.IsArray)
                return field.FieldType.GetElementType();

            return field.FieldType.GetGenericArguments()[0];
        }
    }
}