using System;
using System.Reflection;
using UnityEngine;

namespace Nixi.Injections.Attributes.Abstractions
{
    /// <summary>
    /// Base attribute to represent a dependency injection in a component (or interface) field of a class instance derived from MonoBehaviourInjectable with a Nixi approach
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public abstract class NixInjectComponentBaseAttribute : NixInjectAbstractBaseAttribute
    {
        /// <summary>
        /// Find component(s) which exactly matches criteria of a derived attribute from NixInjectComponentBaseAttribute using the Unity dependency injection method
        /// </summary>
        /// <param name="monoBehaviourInjectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <returns>Component(s) which exactly matches criteria of a NixInjectComponent injection using the Unity dependency injection method</returns>
        public abstract object GetComponentResult(MonoBehaviourInjectable monoBehaviourInjectable, FieldInfo componentField);

        /// <summary>
        /// Check if attribute decorate the right field.FieldType and setup data from component field into the nixi attribute component decorator
        /// </summary>
        /// <param name="field">Field info to check</param>
        public override void CheckIsValidAndBuildDataFromField(FieldInfo field)
        {
            if (field.FieldType.IsGenericType)
            {
                throw new NixiAttributeException($"Cannot inject component field with name {field.Name} and type {field.FieldType.Name}, because it is a generic type while using decorator {GetType().Name}, " +
                                                 $"it is only possible with NixInjectCompoListAttribute on an IEnumerable, List or array");
            }

            if (!IsComponent(field.FieldType) && !field.FieldType.IsInterface)
            {
                throw new NixiAttributeException($"Cannot inject component field with name {field.Name} and type {field.FieldType.Name}, because it not a component or an interface while using decorator {GetType().Name}");
            }
        }
    }
}