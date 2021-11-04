using Nixi.Injections.Attributes.Abstractions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Nixi.Injections.Attributes.ComponentFields.MultiComponents.Abstractions
{
    /// <summary>
    /// Base attribute to represent a dependency injection on an enumerable of component (or interface) field of an instance of a class derived from MonoBehaviourInjectable
    /// <para/>It handles IEnumerable, arrays and Lists of components/interfaces
    /// </summary>
    public abstract class NixInjectMultiComponentsBaseAttribute : NixInjectComponentBaseAttribute
    {
        /// <summary>
        /// Used to identify at which level the fields are injected : current, parent (excluding current) or child (excluding current)
        /// </summary>
        public abstract GameObjectLevel GameObjectLevel { get; }

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
        /// Check if attribute decorate the right field.FieldType and setup data from component field into the nixi attribute component decorator
        /// <para/>This one prevents from decorating fields that aren't IEnumerable, List or array
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