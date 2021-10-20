using Nixi.Injections.Attributes.Abstractions;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Nixi.Injections.Attributes
{
    /// <summary>
    /// Attribute used to define a dependency injection in a component (or interface) field of a class instance derived from MonoBehaviourInjectable with a Nixi approach
    /// with Unity dependency injection approach
    /// <para/>This one use GetComponent(gameObjectTypeToFind) from the instance of the class derived from MonoBehaviourInjectable and fill the Component field
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NixInjectComponentAttribute : NixInjectComponentBaseAttribute
    {
        /// <summary>
        /// Find the unique component which exactly matches criteria of a derived attribute from NixInjectComponentAttribute using the Unity dependency injection method
        /// </summary>
        /// <param name="monoBehaviourInjectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <returns>Unique component which exactly matches criteria of a NixInjectComponent injection using the Unity dependency injection method</returns>
        public override object GetComponentResult(MonoBehaviourInjectable monoBehaviourInjectable, FieldInfo componentField)
        {
            Component[] componentsFound = monoBehaviourInjectable.GetComponents(componentField.FieldType);
            return CheckAndGetSingleComponent(monoBehaviourInjectable, componentField, componentsFound);
        }

        /// <summary>
        /// Check if there is only one component that match criteria from the result of the Unity dependency injection method call
        /// </summary>
        /// <param name="monoBehaviourInjectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <param name="componentsFound">All the components returned by Unity dependency injection method</param>
        /// <returns>Unique component which exactly matches criteria</returns>
        private Component CheckAndGetSingleComponent(MonoBehaviourInjectable monoBehaviourInjectable, FieldInfo componentField, Component[] componentsFound)
        {
            if (!componentsFound.Any())
                throw new NixiAttributeException($"No component with type {componentField.FieldType.Name} was found to fill field with name {componentField.Name}");

            if (componentsFound.Length > 1)
                throw new NixiAttributeException($"Multiple components were found with type {componentField.FieldType.Name} to fill field with name {componentField.Name}, could not define which one should be used ({componentsFound.Length} found instead of just one)");

            return componentsFound.Single();
        }
    }
}