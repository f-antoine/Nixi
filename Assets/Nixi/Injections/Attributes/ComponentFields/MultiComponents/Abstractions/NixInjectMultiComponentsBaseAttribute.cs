using Nixi.Injections.Attributes.ComponentFields.Abstractions;
using Nixi.Injections.Attributes.ComponentFields.Enums;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Nixi.Injections.Attributes.ComponentFields.MultiComponents.Abstractions
{
    /// <summary>
    /// Attribute used to represent an Unity dependency injection to get an enumerable of UnityEngine.Component
    /// (or to target multiple components that implement an interface type)
    /// <para/>It handles IEnumerable, array and List
    /// <para/>Attributes derived from this attribute must be used on a field in a class derived from MonoBehaviourInjectable
    /// <para/>In play mode scene, it uses Unity dependency injection method to get the Component
    /// <para/>In tests, a component is created and you can get it with GetComponent
    /// </summary>
    public abstract class NixInjectMultiComponentsBaseAttribute : NixInjectComponentBaseAttribute
    {
        /// <summary>
        /// Used to identify at which level the enumerable field of UnityEngine.Component (or interface attached 
        /// to these components) are injected : current (on gameObject), parent (excluding current) or child (excluding current)
        /// </summary>
        public abstract GameObjectLevel GameObjectLevel { get; }

        /// <summary>
        /// If the decorated field is an IEnumerable or a list, this represents its single generic type,
        /// if this is an array it returns element type
        /// </summary>
        public Type EnumerableType { get; private set; } = null;

        /// <summary>
        /// Define which Unity dependency injection method has to be called in order to get the components at the targeted level (current, child, parent)
        /// <para/><see cref="GameObjectLevel">Look at GameObjectLevel for more information about levels</see>
        /// </summary>
        protected abstract Func<Component, IEnumerable<Component>> MethodToGetComponents { get; }

        /// <summary>
        /// Find all the components which exactly matches criteria of a derived attribute from NixInjectComponentBaseAttribute 
        /// using the corresponding Unity dependency injection method and parameters previously registered
        /// </summary>
        /// <returns>Components which exactly matches criteria of a NixInjectComponent injection using the corresponding
        /// Unity dependency injection method</returns>
        protected override object GetComponentResultFromParameters()
        {
            return MethodToGetComponents(Target);
        }

        /// <summary>
        /// Check if the field decorated by this attribute (derived from NixInjectAbstractBaseAttribute) is valid and fill it
        /// <para/>This one prevents from decorating fields that aren't IEnumerable, List or array
        /// </summary>
        /// <param name="componentField">Component field</param>
        public override void CheckIsValidAndBuildDataFromField(FieldInfo componentField)
        {
            if (!componentField.FieldType.IsArray
                && !CheckIfGenericEnumerableOrList(componentField))
            {
                throw new NixiAttributeException($"Cannot inject component field with name {componentField.Name} and type {componentField.FieldType.Name}, " +
                                                 $"because it is not an array or IEnumerable/List type while using decorator {GetType().Name}", componentField.FieldType, componentField.Name);
            }

            SetEnumerableType(componentField.FieldType);

            if (!typeof(Component).IsAssignableFrom(EnumerableType)
                && !EnumerableType.IsInterface)
            {
                throw new NixiAttributeException($"Cannot inject component field with name {componentField.Name} and type " +
                                                 $"{componentField.FieldType.Name}, because enumerable type is not a component or an " +
                                                 $"interface while using decorator {GetType().Name}", componentField.FieldType, componentField.Name);
            }
        }

        /// <summary>
        /// Check if a FieldInfo.FieldType is an IEnumerable or a list
        /// </summary>
        /// <param name="genericField">Generic field to check</param>
        /// <returns>True if this a generic field is an IEnumerable or a List</returns>
        private static bool CheckIfGenericEnumerableOrList(FieldInfo genericField)
        {
            if (!genericField.FieldType.IsGenericType)
                return false;

            Type genericType = genericField.FieldType.GetGenericTypeDefinition();
            bool isEnumerable = genericType == typeof(IEnumerable<>) || genericType == typeof(List<>);

            return isEnumerable;
        }

        /// <summary>
        /// If field.FieldType is is an IEnumerable or a list, it returns its single generic type,
        /// if this is an array it returns element type
        /// </summary>
        /// <param name="fieldType">Field type to check</param>
        /// <returns>Generic type of the enumerable if exists</returns>
        public void SetEnumerableType(Type fieldType)
        {
            if (fieldType.IsArray)
            {
                EnumerableType = fieldType.GetElementType();
            }
            else
            {
                EnumerableType = fieldType.GetGenericArguments()[0];
            }
        }
    }
}