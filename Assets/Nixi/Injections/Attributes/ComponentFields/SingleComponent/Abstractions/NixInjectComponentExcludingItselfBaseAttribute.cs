using Nixi.Injections.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Nixi.Injections.ComponentFields.SingleComponent.Abstractions
{
    /// <summary>
    /// Attribute used to represent an Unity dependency injection to get a single UnityEngine.Component
    /// (or to target a component that implement an interface type)
    /// <para/>This one is used to retrieve all the UnityEngine.Component in children or parent from current GameObject (excluding itself)
    /// and get the one that matches the type and name equals to GameObjectNameToFind
    /// <para/>This attribute must be used on a field in a class derived from MonoBehaviourInjectable
    /// <para/>In play mode scene, it uses Unity dependency injection method to get the Component
    /// <para/>In tests, a component is created and you can get it with GetComponent
    /// <para/><see cref="NixInjectComponentAttribute">Use NixInjectComponentAttribute to handle current level</see>
    /// <para/><see cref="NixInjectComponentFromChildrenAttribute">Use NixInjectComponentFromChildrenAttribute to have more information about children level</see>
    /// <para/><see cref="NixInjectComponentFromParentAttribute">Use NixInjectComponentFromParentAttribute to have more information about parent level</see>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public abstract class NixInjectComponentExcludingItselfBaseAttribute : NixInjectSingleComponentBaseAttribute, IHaveGameObjectNameToFind
    {
        /// <summary>
        /// Name of the GameObject to find
        /// </summary>
        public string GameObjectNameToFind { get; private set; }

        /// <summary>
        /// Define if method calls with Unity dependency injection way include inactive GameObject in the search or not
        /// </summary>
        public bool IncludeInactive { get; private set; }

        /// <summary>
        /// Define which Unity dependency injection method has to be called in order to get the components at the targeted level (current, child, parent)
        /// <para/><see cref="GameObjectLevel">Look at GameObjectLevel for more information about levels</see>
        /// </summary>
        protected abstract Func<MonoBehaviourInjectable, FieldInfo, Component[]> MethodToGetComponents { get; }

        /// <summary>
        /// Attribute used to represent an Unity dependency injection to get a single UnityEngine.Component
        /// (or to target a component that implement an interface type)
        /// <para/>This one is used to retrieve all the UnityEngine.Component in children or parent from current GameObject (excluding itself)
        /// and get the one that matches the type and name equals to GameObjectNameToFind
        /// <para/>This attribute must be used on a field in a class derived from MonoBehaviourInjectable
        /// <para/><see cref="NixInjectComponentAttribute">Use NixInjectComponentAttribute to handle current level</see>
        /// <para/><see cref="NixInjectComponentFromChildrenAttribute">Use NixInjectComponentFromChildrenAttribute to have more information about children level</see>
        /// <para/><see cref="NixInjectComponentFromParentAttribute">Use NixInjectComponentFromParentAttribute to have more information about parent level</see>
        /// </summary>
        /// <param name="gameObjectNameToFind">Name of the GameObject to find</param>
        /// <param name="includeInactive">Define if method calls with Unity dependency injection way include inactive GameObject in the search or not</param>
        internal NixInjectComponentExcludingItselfBaseAttribute(string gameObjectNameToFind, bool includeInactive = true)
        {
            GameObjectNameToFind = gameObjectNameToFind;
            IncludeInactive = includeInactive;
        }

        /// <summary>
        /// Finds the component that exactly matches criteria of a derived attribute from NixInjectComponentBaseAttribute using the corresponding Unity dependency injection method
        /// </summary>
        /// <param name="injectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <returns>Unique component that exactly matches criteria of a derived attribute from NixInjectComponentBaseAttribute using the corresponding Unity dependency injection method</returns>
        public override object GetComponentResult(MonoBehaviourInjectable injectable, FieldInfo componentField)
        {
            Component[] components = MethodToGetComponents(injectable, componentField);

            // Ignore itself
            IEnumerable<Component> componentsFound = components.Where(x => x.gameObject.GetInstanceID() != injectable.gameObject.GetInstanceID());

            return CheckAndGetSingleComponentFromChildrenOrParentExcludingItself(componentField, componentsFound);
        }

        /// <summary>
        /// Check if there is only one component that match criteria from the result of the Unity dependency injection method call
        /// <para/> Criteria are : componentField.FieldType must match, component name must be gameObjectNameToFind and only one result is allowed
        /// </summary>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <param name="componentsFoundExcludingItself">All the components returned by Unity dependency injection method excluding gameObject of the MonoBehaviour injectable instance</param>
        /// <returns>Unique component which exactly matches criteria</returns>
        private Component CheckAndGetSingleComponentFromChildrenOrParentExcludingItself(FieldInfo componentField, IEnumerable<Component> componentsFoundExcludingItself)
        {
            if (!componentsFoundExcludingItself.Any())
            {
                throw new NixiAttributeException($"No component with type {componentField.FieldType.Name} was found to fill field with name " +
                                                 $"{componentField.Name}");
            }

            if (string.IsNullOrEmpty(GameObjectNameToFind))
            {
                return GetSingleWithoutName(componentField, componentsFoundExcludingItself);
            }
            return GetSingleWithName(componentField, componentsFoundExcludingItself);
        }

        /// <summary>
        /// Returns single component whose type that matches componentField.FieldType and whose name equals to GameObjectNameToFind
        /// </summary>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <param name="componentsFoundExcludingItself">All the components returned by Unity dependency injection method excluding gameObject of the MonoBehaviour injectable instance</param>
        /// <returns>Unique component which exactly matches criteria</returns>
        private Component GetSingleWithName(FieldInfo componentField, IEnumerable<Component> componentsFoundExcludingItself)
        {
            IEnumerable<Component> componentsWithName = componentsFoundExcludingItself.Where(x => x.name == GameObjectNameToFind);

            if (!componentsWithName.Any())
            {
                throw new NixiAttributeException($"No component with type {componentField.FieldType.Name} and gameObject name " +
                                                 $"{GameObjectNameToFind} was found to fill field with name {componentField.Name}");
            }

            int nbFound = componentsWithName.Count();
            if (nbFound > 1)
            {
                throw new NixiAttributeException($"Multiple components were found with type {componentField.FieldType.Name} and gameObject " +
                                                 $"name {GameObjectNameToFind} to fill field with name {componentField.Name}, could not " +
                                                 $"define which one should be used ({nbFound} found instead of just one, please use NixInjectComponentsAttribute)");
            }

            return componentsWithName.Single();
        }

        /// <summary>
        /// Returns single component whose type that matches componentField.FieldType
        /// </summary>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <param name="componentsFoundExcludingItself">All the components returned by Unity dependency injection method excluding gameObject of the MonoBehaviour injectable instance</param>
        /// <returns>Unique component which exactly matches criteria</returns>
        private Component GetSingleWithoutName(FieldInfo componentField, IEnumerable<Component> componentsFoundExcludingItself)
        {
            int nbFound = componentsFoundExcludingItself.Count();

            if (nbFound > 1)
            {
                throw new NixiAttributeException($"Multiple components were found with type {componentField.FieldType.Name} to fill field " +
                                                 $"with name {componentField.Name}, could not define which one should be used ({nbFound} " +
                                                 $"found instead of just one, please use gameObjectNameToFind parameter with {GetType().Name} " +
                                                 $"to refine your search)");
            }

            return componentsFoundExcludingItself.Single();
        }
    }
}