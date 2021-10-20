using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Nixi.Injections.Injecters
{
    public sealed class EnumerableComponentBinder : Binder
    {
        public override FieldInfo BindToField(BindingFlags bindingAttr, FieldInfo[] match, object value, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

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
                    var valueEnumerable = value as System.Collections.IEnumerable;

                    // TODO : Voir si optimiser ?
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

        public override void ReorderArgumentArray(ref object[] args, object state)
        {
            throw new NotImplementedException();
        }

        public override MethodBase SelectMethod(BindingFlags bindingAttr, MethodBase[] match, Type[] types, ParameterModifier[] modifiers)
        {
            throw new NotImplementedException();
        }

        public override PropertyInfo SelectProperty(BindingFlags bindingAttr, PropertyInfo[] match, Type returnType, Type[] indexes, ParameterModifier[] modifiers)
        {
            throw new NotImplementedException();
        }
    }
}
