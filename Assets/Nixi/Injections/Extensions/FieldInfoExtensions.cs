using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Nixi.Injections.Extensions
{
    public static class FieldInfoExtensions
    {
        /// <summary>
        /// Check if a FieldInfo type is a Component
        /// </summary>
        /// <param name="field">Field to check</param>
        /// <returns>True if this is a component</exception>
        public static bool IsComponent(this FieldInfo field)
        {
            return typeof(Component).IsAssignableFrom(field.FieldType);
        }

        /// <summary>
        /// If field.FieldType is an array or an IEnumerable and his unique generic argument is an Interface or a Component, it returns this unique generic argument, null if not
        /// </summary>
        /// <param name="field">Field to check</param>
        /// <returns>Generic type of the enumerable if exists</returns>
        public static Type GetEnumerableUniqueGenericInterfaceOrComponentTypeIfExist(this FieldInfo field)
        {
            if (field.FieldType.IsArray)
                return field.FieldType.GetElementType();

            Type[] allGenericTypes = field.FieldType.GetGenericArguments();
            
            if (allGenericTypes.Length != 1)
                return null;

            Type enumerableGenericType = allGenericTypes[0];
            if (!enumerableGenericType.IsInterface && !typeof(Component).IsAssignableFrom(enumerableGenericType))
                return null;

            return enumerableGenericType;
        }

        /// <summary>
        /// Check if a FieldInfo.FieldType is an IEnumerable and has exactly one generic argument
        /// </summary>
        /// <param name="genericField">Generic field to check</param>
        /// <returns>True if this a generic field is an IEnumerable with exactly one generic argument</exception>
        public static bool CheckIfGenericEnumerableWithOnlyOneGenericArgument(this FieldInfo genericField)
        {
            if (!genericField.FieldType.IsGenericType)
                throw new NotImplementedException("Cannot check generic arguments on a non generic field");

            if (genericField.FieldType.GetGenericArguments().Length != 1)
                return false;

            Type genericType = genericField.FieldType.GetGenericTypeDefinition();
            bool isEnumerable = genericType == typeof(IEnumerable<>) || genericType == typeof(List<>);

            return isEnumerable;
        }
    }
}