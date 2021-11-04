using Nixi.Injections.Attributes.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Nixi.Injections.Attributes
{
    /// <summary>
    /// Attribute used to define a dependency injection in a component (or interface) field of a class instance derived from MonoBehaviourInjectable with a Nixi approach
    /// with Unity dependency injection approach
    /// <para/>This one get the method associated to GameObjectMethod and call it from the instance of the class derived from MonoBehaviourInjectable and fill the Component field
    /// <para/>This is filtered to match GameObjectType and GameObjectNameToFind
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NixInjectComponentFromMethodAttribute : NixInjectComponentBaseAttribute, IHaveComponentNameToFind
    {
        /// <summary>
        /// Method to use to find the GameObject with the name that match GameObjectName
        /// </summary>
        public GameObjectMethod GameObjectMethod { get; private set; }

        /// <summary>
        /// Name of the GameObject to find
        /// </summary>
        public string ComponentNameToFind { get; private set; }

        /// <summary>
        /// Define if method calls with Unity dependency injection way include inactive GameObject in the search or not
        /// </summary>
        public bool IncludeInactive { get; private set; }

        /// <summary>
        /// Attribute used to define a dependency injection in a Component field of a class instance derived from MonoBehaviourInjectable with a Nixi approach
        /// with Unity dependency injection approach
        /// <para/>This one get the method associated to GameObjectMethod and call it from the instance of the class derived from MonoBehaviourInjectable and fill the Component field
        /// <para/>This is filtered to match GameObjectType and GameObjectNameToFind
        /// </summary>
        /// <param name="gameObjectNameToFind">Name of the GameObject to find</param>
        /// <param name="gameObjectMethod">Method to use to find the GameObject with the name that match GameObjectName</param>
        /// <param name="includeInactive">Define if method calls with Unity dependency injection way include inactive GameObject in the search or not</param>
        public NixInjectComponentFromMethodAttribute(string gameObjectNameToFind, GameObjectMethod gameObjectMethod, bool includeInactive = true)
        {
            ComponentNameToFind = gameObjectNameToFind;
            GameObjectMethod = gameObjectMethod;
            IncludeInactive = includeInactive;
        }

        /// <summary>
        /// Find the unique component which exactly matches criteria of a derived attribute from NixInjectComponentFromMethodAttribute using the Unity dependency injection method
        /// </summary>
        /// <param name="monoBehaviourInjectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <returns>Unique component which exactly matches criteria of a NixInjectComponent injection using the Unity dependency injection method</returns>
        public override object GetComponentResult(MonoBehaviourInjectable monoBehaviourInjectable, FieldInfo componentField)
        {
            IEnumerable<Component> componentsFound = GetComponentsFromGameObjectMethod(monoBehaviourInjectable, componentField.FieldType);
            return CheckAndGetSingleComponent(componentField, componentsFound);
        }

        /// <summary>
        /// Get the method associated to the GameObjectMethod and call it to get Components from the instance of the class derived from MonoBehaviourInjectable
        /// </summary>
        /// <param name="monoBehaviourInjectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="gameObjectTypeToFind">GameObject type to find</param>
        /// <returns>All components returned from the method call</returns>
        private IEnumerable<Component> GetComponentsFromGameObjectMethod(MonoBehaviourInjectable monoBehaviourInjectable, Type gameObjectTypeToFind)
        {
            Component[] components;

            if (GameObjectMethod == GameObjectMethod.GetComponentsInChildren)
            {
                components = monoBehaviourInjectable.GetComponentsInChildren(gameObjectTypeToFind, IncludeInactive);
            }
            else
            {
                components = monoBehaviourInjectable.GetComponentsInParent(gameObjectTypeToFind, IncludeInactive);
            }

            // Ignore itself
            return components.Where(x => x.gameObject.GetInstanceID() != monoBehaviourInjectable.gameObject.GetInstanceID());
        }

        /// <summary>
        /// Check if there is only one component that match criteria from the result of the Unity dependency injection method call
        /// </summary>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <param name="componentsFound">All the components returned by Unity dependency injection method</param>
        /// <returns>Unique component which exactly matches criteria</returns>
        private Component CheckAndGetSingleComponent(FieldInfo componentField, IEnumerable<Component> componentsFound)
        {
            if (!componentsFound.Any())
                throw new NixiAttributeException($"No component with type {componentField.FieldType.Name} was found to fill field with name {componentField.Name}");

            IEnumerable<Component> componentsWithName = componentsFound.Where(x => x.name == ComponentNameToFind);

            if (!componentsWithName.Any())
                throw new NixiAttributeException($"No component with type {componentField.FieldType.Name} and gameObject name {ComponentNameToFind} was found to fill field with name {componentField.Name}");

            int nbFound = componentsWithName.Count();
            if (nbFound > 1)
                throw new NixiAttributeException($"Multiple components were found with type {componentField.FieldType.Name} and gameObject name {ComponentNameToFind} to fill field with name {componentField.Name}, could not define which one should be used ({nbFound} found instead of just one)");

            return componentsWithName.Single();
        }
    }
}