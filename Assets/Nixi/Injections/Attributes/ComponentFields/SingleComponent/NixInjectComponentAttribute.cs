using Nixi.Injections.ComponentFields.SingleComponent.Abstractions;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Nixi.Injections
{
    /// <summary>
    /// Attribute used to represent an Unity dependency injection to get a single UnityEngine.Component
    /// (or to target a component that implement an interface type)
    /// <para/>This one is used to retrieve all the UnityEngine.Component attached on current GameObject with GetComponents
    /// and get the one that matches the type
    /// <para/>This attribute must be used on a field in a class derived from MonoBehaviourInjectable
    /// <para/>In play mode scene, it uses Unity dependency injection method to get the Component
    /// <para/>In tests, a component is created and you can get it with GetComponent
    /// <para/><see cref="NixInjectComponentFromParentAttribute">Use NixInjectComponentFromParentAttribute to handle parent only (excluding current gameObject)</see>
    /// <para/><see cref="NixInjectComponentFromChildrenAttribute">Use NixInjectComponentFromChildrenAttribute to handle children only (excluding current gameObject)</see>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NixInjectComponentAttribute : NixInjectSingleComponentBaseAttribute
    {
        /// <summary>
        /// Finds the component that exactly matches criteria of a derived attribute from NixInjectComponentBaseAttribute using the corresponding Unity dependency injection method
        /// <para/>This one is used to retrieve all the UnityEngine.Component attached on current GameObject with GetComponents
        /// and get the one that matches the type
        /// </summary>
        /// <param name="injectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <returns>Unique component that exactly matches criteria of a derived attribute from NixInjectComponentBaseAttribute using the corresponding Unity dependency injection method</returns>
        public override object GetComponentResult(MonoBehaviourInjectable injectable, FieldInfo componentField)
        {
            Component[] componentsFound = injectable.GetComponents(componentField.FieldType);
            return CheckAndGetSingleComponentAtInjectableLevel(componentField, componentsFound);
        }

        /// <summary>
        /// Check if there is only one component that match criteria from the result of the Unity dependency injection method call
        /// </summary>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <param name="componentsFound">All the components returned by Unity dependency injection method</param>
        /// <returns>Unique component which exactly matches criteria</returns>
        private Component CheckAndGetSingleComponentAtInjectableLevel(FieldInfo componentField, Component[] componentsFound)
        {
            if (!componentsFound.Any())
            {
                throw new NixiAttributeException($"No component with type {componentField.FieldType.Name} was found to fill field with name " +
                                                 $"{componentField.Name}");
            }

            if (componentsFound.Length > 1)
            {
                throw new NixiAttributeException($"Multiple components were found with type {componentField.FieldType.Name} to fill field " +
                                                 $"with name {componentField.Name}, could not define which one should be used " +
                                                 $"({componentsFound.Length} found instead of just one, please use NixInjectComponentsAttribute)");
            }

            return componentsFound.Single();
        }
    }
}