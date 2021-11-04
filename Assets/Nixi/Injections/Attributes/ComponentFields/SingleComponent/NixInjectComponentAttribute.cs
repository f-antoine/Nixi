using Nixi.Injections.Attributes.ComponentFields.SingleComponent.Abstractions;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Nixi.Injections.Attributes
{
    /// <summary>
    /// Attribute to represent a dependency injection on  a component (or interface) field of an instance of a class derived from MonoBehaviourInjectable
    /// <para/>It handles single component/interface field
    /// <para/><see cref="NixInjectComponentFromParentAttribute">Use NixInjectComponentFromParentAttribute to handle parent only (excluding current gameObject)</see>
    /// <para/><see cref="NixInjectComponentFromChildrenAttribute">Use NixInjectComponentFromChildrenAttribute to handle children only (excluding current gameObject)</see>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NixInjectComponentAttribute : NixInjectSingleComponentBaseAttribute
    {
        /// <summary>
        /// Find single component which exactly matches criteria using the Unity dependency injection method and based on constructor use to decorate componentField
        /// </summary>
        /// <param name="monoBehaviourInjectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <returns>Unique component which exactly matches criteria of a NixInjectComponent injection using the Unity dependency injection method</returns>
        public override object GetComponentResult(MonoBehaviourInjectable monoBehaviourInjectable, FieldInfo componentField)
        {
            Component[] componentsFound = monoBehaviourInjectable.GetComponents(componentField.FieldType);
            return CheckAndGetSingleComponentAtInjectableLevel(componentField, componentsFound);
        }

        /// <summary>
        /// Check if there is only one component that match criteria from the result of the Unity dependency injection method call (based on result from GetComponents : same level of the gameObject of the MonoBehaviourInjectable)
        /// <para/>Criteria are : componentField.FieldType must match and only one result is allowed
        /// </summary>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <param name="componentsFound">All the components returned by Unity dependency injection method</param>
        /// <returns>Unique component which exactly matches criteria</returns>
        private Component CheckAndGetSingleComponentAtInjectableLevel(FieldInfo componentField, Component[] componentsFound)
        {
            if (!componentsFound.Any())
                throw new NixiAttributeException($"No component with type {componentField.FieldType.Name} was found to fill field with name {componentField.Name}");

            if (componentsFound.Length > 1)
                throw new NixiAttributeException($"Multiple components were found with type {componentField.FieldType.Name} to fill field with name {componentField.Name}, could not define which one should be used ({componentsFound.Length} found instead of just one, please use NixInjectComponentsAttribute)");

            return componentsFound.Single();
        }
    }
}