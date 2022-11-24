using System.Collections.Generic;
using UnityEngine;

namespace NixiTestTools.TestInjectorElements.Relations.EnumerableComponents.Tools
{
    internal static class EnumerableTools
    {
        /// <summary>
        /// Transpose an object into a IEnumerable of generic type T derived from Component
        /// </summary>
        /// <typeparam name="T">Enumerable generic type</typeparam>
        /// <param name="value">Object to convert</param>
        /// <returns>IEnumerable with generic type T</returns>
        internal static IEnumerable<T> GetEnumerableFromObject<T>(object value)
            where T : Component
        {
            List<T> objectsToConvert = new List<T>();
            foreach (object element in (System.Collections.IEnumerable)value)
            {
                objectsToConvert.Add((T)element);
            }

            if (value.GetType().IsArray)
            {
                return objectsToConvert.ToArray();
            }
            return objectsToConvert;
        }
    }
}