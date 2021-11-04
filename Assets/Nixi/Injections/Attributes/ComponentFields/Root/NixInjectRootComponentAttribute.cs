﻿using Nixi.Injections.Attributes.Abstractions;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nixi.Injections.Attributes
{
    /// <summary>
    /// Attribute used to define a dependency injection in a component (or interface) field of a class instance derived from MonoBehaviourInjectable
    /// with Unity dependency injection approach
    /// <para/>This one get all the root GameObjects of the current scene then try to find the single one named RootGameObjectName
    /// <para/>If subComponentRootName is filled, then it execute GetComponentsInChildren and call it from the root GameObject (excluding itself)
    /// the result is filtered to match GameObjectType and GameObjectNameToFind and get the unique instance to fill the Component field in the instance of the class derived from MonoBehaviourInjectable
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class NixInjectRootComponentAttribute : NixInjectSingleComponentBaseAttribute, IHaveComponentNameToFind
    {
        /// <summary>
        /// True if constructor with subComponentRootName is used, it means we are targeting a child gameObject from rootComponent
        /// </summary>
        private bool isTargetingChildFromRoot => !string.IsNullOrEmpty(SubGameObjectName);

        /// <summary>
        /// Name of the root GameObject to find
        /// </summary>
        public string RootGameObjectName { get; private set; }

        /// <summary>
        /// Name of the GameObject to find in children of root game object found with name ComponentRootName
        /// </summary>
        public string SubGameObjectName { get; private set; }

        /// <summary>
        /// Name of the GameObject (SubComponentRootName if filled, ComponentRootName if not)
        /// </summary>
        public string ComponentNameToFind => SubGameObjectName ?? RootGameObjectName;

        /// <summary>
        /// Define if GetComponentsInChildren method calls with Unity dependency injection way include inactive GameObject in the search or not
        /// </summary>
        public bool IncludeInactive { get; private set; }

        /// <summary>
        /// Attribute used to define a dependency injection in a component (or interface) field of a class instance derived from MonoBehaviourInjectable
        /// with Unity dependency injection approach
        /// <para/>This one get all the root GameObjects of the current scene then try to find the single one named RootGameObjectName
        /// <para/>If subComponentRootName is filled, then it execute GetComponentsInChildren and call it from the root GameObject (excluding itself)
        /// the result is filtered to match GameObjectType and GameObjectNameToFind and get the unique instance to fill the Component field in the instance of the class derived from MonoBehaviourInjectable
        /// </summary>
        /// <param name="rootGameObjectName">Method to use on root GameObjects from the current scene to find a GameObject that match GameObjectName</param>
        public NixInjectRootComponentAttribute(string rootGameObjectName)
        {
            RootGameObjectName = rootGameObjectName;
        }

        /// <summary>
        /// Attribute used to define a dependency injection in a component (or interface) field of a class instance derived from MonoBehaviourInjectable
        /// with Unity dependency injection approach
        /// <para/>This one get all the root GameObjects of the current scene then try to find the single one named RootGameObjectName
        /// <para/>If subComponentRootName is filled, then it execute GetComponentsInChildren and call it from the root GameObject (excluding itself)
        /// the result is filtered to match GameObjectType and GameObjectNameToFind and get the unique instance to fill the Component field in the instance of the class derived from MonoBehaviourInjectable
        /// </summary>
        /// <param name="rootGameObjectName">Method to use on root GameObjects from the current scene to find a GameObject that match GameObjectName</param>
        /// <param name="subGameObjectName">Name of the GameObject to find in children of root game object with name componentRootName</param>
        /// <param name="includeInactive">Define if method calls with Unity dependency injection way include inactive GameObject in the search or not</param>
        public NixInjectRootComponentAttribute(string rootGameObjectName, string subGameObjectName, bool includeInactive = true)
        {
            RootGameObjectName = rootGameObjectName;
            SubGameObjectName = subGameObjectName;
            IncludeInactive = includeInactive;
        }

        /// <summary>
        /// Find the unique component which exactly matches criteria of a derived attribute from NixInjectComponentFromRootAttribute using the Unity dependency injection method
        /// </summary>
        /// <param name="monoBehaviourInjectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <returns>Unique component which exactly matches criteria of a NixInjectComponent injection using the Unity dependency injection method</returns>
        public override object GetComponentResult(MonoBehaviourInjectable monoBehaviourInjectable, FieldInfo componentField)
        {
            GameObject targetedRootGameObject = GetTargetedRootGameObject();

            Component[] componentsFound = GetComponentsFromRootParameters(targetedRootGameObject, componentField.FieldType);

            return CheckAndGetSingleComponent(targetedRootGameObject, componentField, componentsFound);
        }

        /// <summary>
        /// Find the unique root GameObject named with the value from RootGameObjectName
        /// </summary>
        /// <returns>Unique root GameObject named with the value from RootGameObjectName</returns>
        private GameObject GetTargetedRootGameObject()
        {
            GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            IEnumerable<GameObject> rootGameObjectsWithName = rootGameObjects.Where(x => x.name == RootGameObjectName);
            int nbRootGameObjectWithNameFound = rootGameObjectsWithName.Count();

            if (nbRootGameObjectWithNameFound == 0)
                throw new NixiAttributeException($"No root GameObject with name {RootGameObjectName} was found in the root GameObjects");

            if (nbRootGameObjectWithNameFound > 1)
                throw new NixiAttributeException($"Multiple GameObjects with name {RootGameObjectName} were found in the root GameObjects, cannot define which one to use");

            return rootGameObjectsWithName.Single();
        }

        /// <summary>
        /// Get the method associated to the RootParameters (SubComponentRootName filled or not) and call it to get Components from the root GameObject targeted
        /// </summary>
        /// <param name="targetedRootGameObject">Root GameObject targeted</param>
        /// <param name="gameObjectTypeToFind">GameObject type to find</param>
        /// <returns>All components returned from the method call</returns>
        private Component[] GetComponentsFromRootParameters(GameObject targetedRootGameObject, Type gameObjectTypeToFind)
        {
            if (isTargetingChildFromRoot)
            {
                return targetedRootGameObject.GetComponentsInChildren(gameObjectTypeToFind, IncludeInactive);
            }
            return targetedRootGameObject.GetComponents(gameObjectTypeToFind);
        }

        /// <summary>
        /// Check if there is only one component that match criteria from the result of the Unity dependency injection method call
        /// </summary>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <param name="componentsFound">All the components returned by Unity dependency injection method</param>
        /// <returns>Unique component which exactly matches criteria</returns>
        private Component CheckAndGetSingleComponent(GameObject targetedRootGameObject, FieldInfo componentField, Component[] componentsFound)
        {
            if (!componentsFound.Any())
                throw new NixiAttributeException($"No component with type {componentField.FieldType.Name} was found on root GameObject with name {targetedRootGameObject.name} to fill field with name {componentField.Name}");

            if (isTargetingChildFromRoot)
            {
                return CheckAndGetUniqueResultFromChildGameObjectNameFromRoot(targetedRootGameObject, componentField, componentsFound);
            }
            return CheckAndGetUniqueResultTypeFromRoot(targetedRootGameObject, componentField, componentsFound);
        }

        /// <summary>
        /// Check if there is only one component that match criteria from the result of the Unity dependency injection GetComponentsInChildren call
        /// </summary>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <param name="componentsFound">All the components returned by Unity dependency injection method</param>
        /// <returns>Unique component which exactly matches criteria</returns>
        private Component CheckAndGetUniqueResultFromChildGameObjectNameFromRoot(GameObject targetedRootGameObject, FieldInfo componentField, Component[] componentsFound)
        {
            IEnumerable<Component> componentsWithName = componentsFound.Where(x => x.name == SubGameObjectName);

            if (!componentsWithName.Any())
                throw new NixiAttributeException($"Components were found with type {componentField.FieldType.Name} but not with gameObject name {SubGameObjectName} on root GameObject with name {targetedRootGameObject.name} to fill field with name {componentField.Name}");

            int nbFound = componentsWithName.Count();
            if (nbFound > 1)
                throw new NixiAttributeException($"Multiple components were found with type {componentField.FieldType.Name} and with gameObject name {SubGameObjectName} on root GameObject with name {targetedRootGameObject.name} to fill field with name {componentField.Name}, could not define which one should be used ({nbFound} found instead of just one)");

            return componentsWithName.Single();
        }

        /// <summary>
        /// Check if there is only one component that match criteria from the result of the Unity dependency injection GetComponent call
        /// </summary>
        /// <param name="componentField">Component field to fill based on componentField.FieldType to find</param>
        /// <param name="componentsFound">All the components returned by Unity dependency injection method</param>
        /// <returns>Unique component which exactly matches criteria</returns>
        private Component CheckAndGetUniqueResultTypeFromRoot(GameObject targetedRootGameObject, FieldInfo componentField, Component[] componentsFound)
        {
            int nbFound = componentsFound.Length;
            if (nbFound > 1)
                throw new NixiAttributeException($"Multiple components were found with type {componentField.FieldType.Name} on root GameObject with name {targetedRootGameObject.name} to fill field with name {componentField.Name}, could not define which one should be used ({nbFound} found instead of just one)");

            return componentsFound.Single();
        }
    }
}