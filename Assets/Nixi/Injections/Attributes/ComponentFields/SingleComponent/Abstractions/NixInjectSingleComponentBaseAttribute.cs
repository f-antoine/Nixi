using Nixi.Injections.Abstractions;
using System.Reflection;
using UnityEngine;

namespace Nixi.Injections.ComponentFields.SingleComponent.Abstractions
{
    /// <summary>
    /// Base attribute used to represent an Unity dependency injection to get a single UnityEngine.Component
    /// (or to target a component that implement an interface type)
    /// <para/>This attribute must be used on a field in a class derived from MonoBehaviourInjectable
    /// <para/>In play mode scene, it uses Unity dependency injection method to get the Component
    /// <para/>In tests, a component is created and you can get it with GetComponent
    /// </summary>
    public abstract class NixInjectSingleComponentBaseAttribute : NixInjectComponentBaseAttribute
    {
        /// <summary>
        /// Check if the field decorated by this attribute (derived from NixInjectAbstractBaseAttribute) is valid and fill it
        /// <para/>This one prevents from decorating fields that aren't interfaces and non-components
        /// </summary>
        /// <param name="componentField">Component field</param>
        public override void CheckIsValidAndBuildDataFromField(FieldInfo componentField)
        {
            if (componentField.FieldType.IsGenericType)
            {
                throw new NixiAttributeException($"Cannot inject component field with name {componentField.Name} and type {componentField.FieldType.Name}, " +
                                                 $"because it is a generic type while using decorator {GetType().Name}, " +
                                                 $"it is only possible with NixInjectComponentsAttribute on an IEnumerable, List or array");
            }

            if (!typeof(Component).IsAssignableFrom(componentField.FieldType)
                && !componentField.FieldType.IsInterface)
            {
                throw new NixiAttributeException($"Cannot inject component field with name {componentField.Name} and type " +
                                                 $"{componentField.FieldType.Name}, because it not a component or an interface while using " +
                                                 $"decorator {GetType().Name}");
            }
        }
    }
}