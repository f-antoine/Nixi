using Nixi.Injections.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Nixi.Injections.ComponentFields.SingleComponent.Abstractions
{
    /// <summary>
    /// Attribute to represent a dependency injection on a component (or interface) field of an instance of a class derived from MonoBehaviourInjectable
    /// <para/>This one has logic using GetComponentsInChildren and GetComponentsInParent on current gameObject, excluding the current gameObject in the search
    /// <para/>It handles single component/interface field
    /// <para/><see cref="NixInjectComponentAttribute">Use NixInjectComponentAttribute to handle current level</see>
    /// <para/><see cref="NixInjectComponentFromChildrenAttribute">Use NixInjectComponentFromChildrenAttribute to have more information about children level</see>
    /// <para/><see cref="NixInjectComponentFromParentAttribute">Use NixInjectComponentFromParentAttribute to have more information about parent level</see>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public abstract class NixInjectComponentExcludingItselfBaseAttribute : NixInjectSingleComponentBaseAttribute, IHaveComponentNameToFind
    {
        /// <summary>
        /// Name of the GameObject to find
        /// </summary>
        public string ComponentNameToFind { get; private set; }

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
        /// Target single component that match gameObjectNameToFind and type returned from Unity method : GetComponentsInChildren/GetComponentsInParent, it excludes itself
        /// </summary>
        /// <param name="gameObjectNameToFind">Name of the GameObject to find</param>
        /// <param name="includeInactive">Define if method calls with Unity dependency injection way include inactive GameObject in the search or not</param>
        internal NixInjectComponentExcludingItselfBaseAttribute(string gameObjectNameToFind, bool includeInactive = true)
        {
            ComponentNameToFind = gameObjectNameToFind;
            IncludeInactive = includeInactive;
        }

        /// <summary>
        /// Find single component which exactly matches criteria using the Unity dependency injection method and based on constructor use to decorate componentField
        /// </summary>
        /// <param name="monoBehaviourInjectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <returns>Unique component which exactly matches criteria of a NixInjectComponent injection using the Unity dependency injection method</returns>
        public override object GetComponentResult(MonoBehaviourInjectable monoBehaviourInjectable, FieldInfo componentField)
        {
            Component[] components = MethodToGetComponents(monoBehaviourInjectable, componentField);

            // Ignore itself
            IEnumerable<Component> componentsFound = components.Where(x => x.gameObject.GetInstanceID() != monoBehaviourInjectable.gameObject.GetInstanceID());

            return CheckAndGetSingleComponentFromChildrenOrParentExcludingItself(componentField, componentsFound);
        }

        /// <summary>
        /// Check if there is only one component that match criteria from the result of the Unity dependency injection method call (based on result from GetComponentsInChildren or GetComponentsInParent : excluding same level of the MonoBehaviour injectable instance)
        /// <para/> Criteria are : componentField.FieldType must match, component name must be gameObjectNameToFind and only one result is allowed
        /// </summary>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <param name="componentsFoundExcludingItself">All the components returned by Unity dependency injection method excluding gameObject of the MonoBehaviour injectable instance</param>
        /// <returns>Unique component which exactly matches criteria</returns>
        private Component CheckAndGetSingleComponentFromChildrenOrParentExcludingItself(FieldInfo componentField, IEnumerable<Component> componentsFoundExcludingItself)
        {
            if (!componentsFoundExcludingItself.Any())
                throw new NixiAttributeException($"No component with type {componentField.FieldType.Name} was found to fill field with name {componentField.Name}");

            if (string.IsNullOrEmpty(ComponentNameToFind))
            {
                return GetWithoutName(componentField, componentsFoundExcludingItself);
            }
            return GetWithName(componentField, componentsFoundExcludingItself);
        }

        /// <summary>
        /// Returns single component whose type that matches componentField.FieldType and whose name equals to ComponentNameToFind
        /// </summary>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <param name="componentsFoundExcludingItself">All the components returned by Unity dependency injection method excluding gameObject of the MonoBehaviour injectable instance</param>
        /// <returns>Unique component which exactly matches criteria</returns>
        private Component GetWithName(FieldInfo componentField, IEnumerable<Component> componentsFoundExcludingItself)
        {
            IEnumerable<Component> componentsWithName = componentsFoundExcludingItself.Where(x => x.name == ComponentNameToFind);

            if (!componentsWithName.Any())
                throw new NixiAttributeException($"No component with type {componentField.FieldType.Name} and gameObject name {ComponentNameToFind} was found to fill field with name {componentField.Name}");

            int nbFound = componentsWithName.Count();
            if (nbFound > 1)
                throw new NixiAttributeException($"Multiple components were found with type {componentField.FieldType.Name} and gameObject name {ComponentNameToFind} to fill field with name {componentField.Name}, could not define which one should be used ({nbFound} found instead of just one, please use NixInjectComponentsAttribute)");

            return componentsWithName.Single();
        }

        /// <summary>
        /// Returns single component whose type that matches componentField.FieldType
        /// </summary>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <param name="componentsFoundExcludingItself">All the components returned by Unity dependency injection method excluding gameObject of the MonoBehaviour injectable instance</param>
        /// <returns>Unique component which exactly matches criteria</returns>
        private Component GetWithoutName(FieldInfo componentField, IEnumerable<Component> componentsFoundExcludingItself)
        {
            int nbFound = componentsFoundExcludingItself.Count();
            
            if (nbFound > 1)
                throw new NixiAttributeException($"Multiple components were found with type {componentField.FieldType.Name} to fill field with name {componentField.Name}, could not define which one should be used ({nbFound} found instead of just one, please use gameObjectNameToFind parameter with {GetType().Name} to refine your search)");

            return componentsFoundExcludingItself.Single();
        }
    }
}