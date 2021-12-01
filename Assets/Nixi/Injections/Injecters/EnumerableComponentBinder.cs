using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

namespace Nixi.Injections.Injecters
{
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

        public override object ChangeType(object value, Type type, CultureInfo culture)
        {
            // Enumerable into array, we have to force the cast
            if (type.IsArray)
            {
                if (typeof(System.Collections.IEnumerable).IsAssignableFrom(value.GetType()))
                {
                    var valueEnumerable = (System.Collections.IEnumerable)value;

                    List<object> objectsToConvert = new List<object>();
                    foreach (var element in valueEnumerable)
                    {
                        objectsToConvert.Add(element);
                    }
                    return objectsToConvert.ToArray();
                }
            }

            return value;
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
