using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

// Ignore warnings on not implemented methods when generating xml documentation
#pragma warning disable 1591
namespace Nixi.Injections.Injectors
{
    /// <summary>
    /// Special Binder used to handle IEnumerable, List and array for NixiInjection on multiple components.
    /// <para/>The default way was throwing exception everytime because type conversion was not accepted
    /// </summary>
    public sealed class EnumerableComponentBinder : Binder
    {
        [ExcludeFromCodeCoverage] // No reason to override or test
        public override FieldInfo BindToField(BindingFlags bindingAttr, FieldInfo[] match, object value, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage] // No reason to override or test
        public override MethodBase BindToMethod(BindingFlags bindingAttr, MethodBase[] match, ref object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] names, out object state)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// If object is an array, it converts its data into an array
        /// <para/>Else if it is a list, the list is rebuilt from object
        /// <para/>Else, return the object directly because it is already an IEnumerable
        /// </summary>
        /// <param name="value">Object to convert into array or return if not an array</param>
        /// <param name="type">Type of object</param>
        /// <param name="culture">Culture info</param>
        /// <returns>Value converted</returns>
        public override object ChangeType(object value, Type type, CultureInfo culture)
        {
            if (type.IsArray)
                return ConvertObjectToObjectList(value).ToArray();

            if (type.IsGenericType)
            {
                // Had to made this because value is always an IEnumerable (stored like this in TestInjector to match all kinds of collection used in Nixi).
                // If it was not rebuilt, this won't fill field correctly with list type during enumerable component injection.
                // (If a list has an IEnumerable setted and we try to do AsReadOnly() call, it will crash Unity)
                if (typeof(List<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
                    return RebuildListOfType(value, type);

                if (value is IEnumerable)
                    return value;
            }

            throw new NotImplementedException($"Cannot change object {value} of type {value.GetType().Name} with type parameter {type.Name} in EnumerableComponentBinder");
        }

        /// <summary>
        /// Rebuild a list of type from an object (value must be a list that match type)
        /// </summary>
        /// <param name="value">Object which should be represented as a list</param>
        /// <param name="type">Type of list</param>
        /// <returns>Object which match list type</returns>
        private static object RebuildListOfType(object value, Type type)
        {
            MethodInfo addMethod = type.GetMethod("Add");

            object secondList = Activator.CreateInstance(type);

            foreach (var element in ConvertObjectToObjectList(value))
            {
                addMethod.Invoke(secondList, new object[] { element });
            }

            return secondList;
        }

        /// <summary>
        /// Convert an object to a list of object (value must be a list)
        /// </summary>
        /// <param name="value">Object which should be represented as a list</param>
        /// <returns>Object converted</returns>
        private static List<object> ConvertObjectToObjectList(object value)
        {
            List<object> objectsToConvert = new List<object>();
            foreach (object element in (IEnumerable)value)
            {
                objectsToConvert.Add(element);
            }
            return objectsToConvert;
        }

        [ExcludeFromCodeCoverage] // No reason to override or test
        public override void ReorderArgumentArray(ref object[] args, object state)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage] // No reason to override or test
        public override MethodBase SelectMethod(BindingFlags bindingAttr, MethodBase[] match, Type[] types, ParameterModifier[] modifiers)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage] // No reason to override or test
        public override PropertyInfo SelectProperty(BindingFlags bindingAttr, PropertyInfo[] match, Type returnType, Type[] indexes, ParameterModifier[] modifiers)
        {
            throw new NotImplementedException();
        }
    }
}
#pragma warning restore 1591