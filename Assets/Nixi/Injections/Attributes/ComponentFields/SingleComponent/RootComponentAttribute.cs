using Nixi.Injections.Attributes;
using Nixi.Injections.Attributes.ComponentFields.Abstractions;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nixi.Injections
{
    /// <summary>
    /// Attribute used to represent an Unity dependency injection to get a single UnityEngine.Component
    /// (or to target a component that implement an interface type)
    /// <para/>This one retrieves all the root GameObjects of the current scene, then tries to find the one whose name matches
    /// the values contained in RootGameObjectName
    /// <para/>If SubGameObjectName is filled, then it executes GetComponentsInChildren method on the RootGameObject found (excluding itself).
    /// The result is filtered to find the GameObject that has the correct GameObjectType and SubGameObjectName and is used to fill the field
    /// <para/>If not, the RootGameObject found must match GameObjectType and this is the one used to fill the field
    /// <para/>This attribute must be used on a field in a class derived from MonoBehaviourInjectable
    /// <para/>In play mode scene, it uses Unity dependency injection method to get the Component
    /// <para/>In tests, a component is created and you can get it with GetComponent
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class RootComponentAttribute : NixInjectSingleComponentBaseAttribute, IHaveGameObjectNameToFind
    {
        /// <summary>
        /// True if constructor with subGameObjectName is used, it means we are targeting a child gameObject from RootGameObject
        /// </summary>
        private bool IsTargetingChildFromRoot => !string.IsNullOrEmpty(SubGameObjectName);

        /// <summary>
        /// Name of the root GameObject to find
        /// </summary>
        public string RootGameObjectName { get; private set; }

        /// <summary>
        /// Name of the GameObject to find in children from root GameObject found with name ComponentRootName
        /// </summary>
        public string SubGameObjectName { get; private set; }

        /// <summary>
        /// Name of the GameObject (SubGameObjectName if filled, ComponentRootName if not)
        /// </summary>
        public string GameObjectNameToFind => SubGameObjectName ?? RootGameObjectName;

        /// <summary>
        /// Define if GetComponentsInChildren method calls with Unity dependency injection way include inactive GameObject in the search or not
        /// </summary>
        public bool IncludeInactive { get; private set; }

        /// <summary>
        /// TargetName composed with RootGameObjectName and SubGameObjectName if this parameters was passed from constructor
        /// </summary>
        private string FullTargetName
        {
            get
            {
                string targetName = $"Root = {RootGameObjectName}";

                if (!string.IsNullOrEmpty(SubGameObjectName))
                {
                    targetName += $", Child = {SubGameObjectName}";
                }

                return targetName;
            }
        }

        /// <summary>
        /// Attribute used to represent an Unity dependency injection on a single UnityEngine.Component
        /// (or to target a component that implement an interface type)
        /// <para/>This one retrieves all the root GameObjects of the current scene, then tries to find the one whose name matches
        /// the values contained in RootGameObjectName.
        /// <para/>The RootGameObject found must match GameObjectType and this is the one used to fill the field
        /// <para/>This attribute must be used on a field in a class derived from MonoBehaviourInjectable
        /// </summary>
        /// <param name="rootGameObjectName">Method to use on root GameObjects from the current scene to find a GameObject that match GameObjectName</param>
        public RootComponentAttribute(string rootGameObjectName)
        {
            RootGameObjectName = rootGameObjectName;
        }

        /// <summary>
        /// Attribute used to represent an Unity dependency injection on a single UnityEngine.Component
        /// (or to target a component that implement an interface type)
        /// <para/>This one retrieves all the root GameObjects of the current scene, then tries to find the one whose name matches
        /// the values contained in RootGameObjectName
        /// <para/>If SubGameObjectName is filled, then it executes GetComponentsInChildren method on the RootGameObject found (excluding itself).
        /// The result is filtered to find the GameObject that has the correct GameObjectType and SubGameObjectName and is used to fill the field
        /// <para/>If not, the RootGameObject found must match GameObjectType and this is the one used to fill the field
        /// <para/>This attribute must be used on a field in a class derived from MonoBehaviourInjectable
        /// </summary>
        /// <param name="rootGameObjectName">Method to use on root GameObjects from the current scene to find a GameObject that match GameObjectName</param>
        /// <param name="subGameObjectName">Name of the GameObject to find in children of root game object with name componentRootName</param>
        /// <param name="includeInactive">Define if method calls with Unity dependency injection way include inactive GameObject in the search or not</param>
        public RootComponentAttribute(string rootGameObjectName, string subGameObjectName, bool includeInactive = true)
        {
            RootGameObjectName = rootGameObjectName;
            SubGameObjectName = subGameObjectName;
            IncludeInactive = includeInactive;
        }

        /// <summary>
        /// Finds the component that exactly matches criteria of a derived attribute from NixInjectComponentBaseAttribute using the corresponding Unity dependency injection method and parameters previously registered
        /// <para/>This one retrieves all the root GameObjects of the current scene, then tries to find the one whose name matches
        /// the values contained in RootGameObjectName
        /// <para/>If SubGameObjectName is filled, then it executes GetComponentsInChildren method on the RootGameObject found (excluding itself).
        /// The result is filtered to find the GameObject that has the correct GameObjectType and SubGameObjectName and is used to fill the field
        /// <para/>If not, the RootGameObject found must match GameObjectType and this is the one used to fill the field
        /// </summary>
        /// <returns>Unique component that exactly matches criteria of a derived attribute from NixInjectComponentBaseAttribute using the corresponding Unity dependency injection method</returns>
        protected override object GetComponentResultFromParameters()
        {
            GameObject targetedRootGameObject = GetTargetedRootGameObject();

            IEnumerable<Component> componentsFound = GetComponentsFromRootParameters(targetedRootGameObject);

            return CheckAndGetSingleComponent(targetedRootGameObject, componentsFound);
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
            {
                throw new NixiAttributeException($"No root GameObject with name {RootGameObjectName} was found in the root GameObjects", typeof(GameObject), FullTargetName);
            }

            if (nbRootGameObjectWithNameFound > 1)
            {
                throw new NixiAttributeException($"Multiple GameObjects with name {RootGameObjectName} were found in the root GameObjects, " +
                                                 $"cannot define which one to use", typeof(GameObject), FullTargetName);
            }

            return rootGameObjectsWithName.Single();
        }

        /// <summary>
        /// Get the method associated to the RootParameters (SubGameObjectName filled or not) and call it to get Components from the root GameObject targeted
        /// <para/>If SubGameObjectName is filled, it executes GetComponentsInChildren method on the RootGameObject found (excluding itself).
        /// The result is filtered to find the GameObject that has the correct GameObjectType and SubGameObjectName and is used to fill the field
        /// <para/>If not, the RootGameObject found must match GameObjectType and this is the one used to fill the field
        /// </summary>
        /// <param name="targetedRootGameObject">Unique root GameObject named with the value from RootGameObjectName</param>
        /// <returns>All components returned from the method associated</returns>
        private IEnumerable<Component> GetComponentsFromRootParameters(GameObject targetedRootGameObject)
        {
            // TODO : Separation in two methods for here and CheckAndGetSingleComponent at line if (isTargetingChildFromRoot)
            if (IsTargetingChildFromRoot)
            {
                return targetedRootGameObject.GetComponentsInChildren(FieldType, IncludeInactive)
                                             .Where(x => x.gameObject.GetInstanceID() != targetedRootGameObject.GetInstanceID());
            }
            return targetedRootGameObject.GetComponents(FieldType);
        }

        /// <summary>
        /// Check if there is only one component that match criteria from the result of the Unity dependency injection method call
        /// </summary>
        /// <param name="targetedRootGameObject">Unique root GameObject named with the value from RootGameObjectName</param>
        /// <param name="componentsFound">All the components returned by Unity dependency injection method</param>
        /// <returns>Unique component which exactly matches criteria</returns>
        private Component CheckAndGetSingleComponent(GameObject targetedRootGameObject, IEnumerable<Component> componentsFound)
        {
            

            // TODO : Separation in two methods for here and CheckAndGetSingleComponent at line if (isTargetingChildFromRoot)
            if (IsTargetingChildFromRoot)
            {
                if (!componentsFound.Any())
                {
                    throw new NixiAttributeException($"No component with type {FieldType.Name} was found on root GameObject with " +
                                                     $"name {RootGameObjectName} and child name {SubGameObjectName} to fill field with name {FieldName}",
                                                     FieldType, FullTargetName);
                }

                return CheckAndGetUniqueResultFromChildGameObjectNameFromRoot(targetedRootGameObject, componentsFound);
            }

            if (!componentsFound.Any())
            {
                throw new NixiAttributeException($"No component with type {FieldType.Name} was found on root GameObject with " +
                                                 $"name {RootGameObjectName} to fill field with name {FieldName}", FieldType, FullTargetName);
            }

            return CheckAndGetUniqueResultTypeFromRoot(targetedRootGameObject, componentsFound);
        }

        /// <summary>
        /// Check if there is only one component that match criteria from the result of the Unity dependency injection GetComponentsInChildren call
        /// </summary>
        /// <param name="targetedRootGameObject">Unique root GameObject named with the value from RootGameObjectName</param>
        /// <param name="componentsFound">All the components returned by Unity dependency injection method</param>
        /// <returns>Unique component which exactly matches criteria</returns>
        private Component CheckAndGetUniqueResultFromChildGameObjectNameFromRoot(GameObject targetedRootGameObject, IEnumerable<Component> componentsFound)
        {
            IEnumerable<Component> componentsWithName = componentsFound.Where(x => x.name == SubGameObjectName);

            if (!componentsWithName.Any())
            {
                throw new NixiAttributeException($"Components were found with type {FieldType.Name} but not with gameObject " +
                                                 $"name {SubGameObjectName} on root GameObject with name {targetedRootGameObject.name} to fill field with name {FieldName}", FieldType, FullTargetName); ;
            }

            int nbFound = componentsWithName.Count();
            if (nbFound > 1)
            {
                throw new NixiAttributeException($"Multiple components were found with type {FieldType.Name} and with " +
                                                 $"gameObject name {SubGameObjectName} on root GameObject with name {targetedRootGameObject.name} to fill field with " +
                                                 $"name {FieldName}, could not define which one should be used ({nbFound} found instead of just one)", FieldType, FullTargetName);
            }

            return componentsWithName.Single();
        }

        /// <summary>
        /// Check if there is only one component that match criteria from the result of the Unity dependency injection GetComponent call
        /// </summary>
        /// <param name="targetedRootGameObject">Unique root GameObject named with the value from RootGameObjectName</param>
        /// <param name="componentsFound">All the components returned by Unity dependency injection method</param>
        /// <returns>Unique component which exactly matches criteria</returns>
        private Component CheckAndGetUniqueResultTypeFromRoot(GameObject targetedRootGameObject, IEnumerable<Component> componentsFound)
        {
            int nbFound = componentsFound.Count();
            if (nbFound > 1)
            {
                throw new NixiAttributeException($"Multiple components were found with type {FieldType.Name} on root " +
                                                 $"GameObject with name {targetedRootGameObject.name} to fill field with name {FieldName}, " +
                                                 $"could not define which one should be used ({nbFound} found instead of just one)", FieldType, FullTargetName);
            }

            return componentsFound.Single();
        }
    }

    #region Obsolete version
    /// <summary>
    /// Attribute used to represent an Unity dependency injection to get a single UnityEngine.Component
    /// (or to target a component that implement an interface type)
    /// <para/>This one retrieves all the root GameObjects of the current scene, then tries to find the one whose name matches
    /// the values contained in RootGameObjectName
    /// <para/>If SubGameObjectName is filled, then it executes GetComponentsInChildren method on the RootGameObject found (excluding itself).
    /// The result is filtered to find the GameObject that has the correct GameObjectType and SubGameObjectName and is used to fill the field
    /// <para/>If not, the RootGameObject found must match GameObjectType and this is the one used to fill the field
    /// <para/>This attribute must be used on a field in a class derived from MonoBehaviourInjectable
    /// <para/>In play mode scene, it uses Unity dependency injection method to get the Component
    /// <para/>In tests, a component is created and you can get it with GetComponent
    /// </summary>
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    [Obsolete("Will be replaced with a shorter version : RootComponent")]
    public sealed class NixInjectRootComponentAttribute : RootComponentAttribute
    {
        /// <summary>
        /// Attribute used to represent an Unity dependency injection on a single UnityEngine.Component
        /// (or to target a component that implement an interface type)
        /// <para/>This one retrieves all the root GameObjects of the current scene, then tries to find the one whose name matches
        /// the values contained in RootGameObjectName.
        /// <para/>The RootGameObject found must match GameObjectType and this is the one used to fill the field
        /// <para/>This attribute must be used on a field in a class derived from MonoBehaviourInjectable
        /// </summary>
        /// <param name="rootGameObjectName">Method to use on root GameObjects from the current scene to find a GameObject that match GameObjectName</param>
        public NixInjectRootComponentAttribute(string rootGameObjectName)
            : base(rootGameObjectName) { }

        /// <summary>
        /// Attribute used to represent an Unity dependency injection on a single UnityEngine.Component
        /// (or to target a component that implement an interface type)
        /// <para/>This one retrieves all the root GameObjects of the current scene, then tries to find the one whose name matches
        /// the values contained in RootGameObjectName
        /// <para/>If SubGameObjectName is filled, then it executes GetComponentsInChildren method on the RootGameObject found (excluding itself).
        /// The result is filtered to find the GameObject that has the correct GameObjectType and SubGameObjectName and is used to fill the field
        /// <para/>If not, the RootGameObject found must match GameObjectType and this is the one used to fill the field
        /// <para/>This attribute must be used on a field in a class derived from MonoBehaviourInjectable
        /// </summary>
        /// <param name="rootGameObjectName">Method to use on root GameObjects from the current scene to find a GameObject that match GameObjectName</param>
        /// <param name="subGameObjectName">Name of the GameObject to find in children of root game object with name componentRootName</param>
        /// <param name="includeInactive">Define if method calls with Unity dependency injection way include inactive GameObject in the search or not</param>
        public NixInjectRootComponentAttribute(string rootGameObjectName, string subGameObjectName, bool includeInactive = true)
            : base(rootGameObjectName, subGameObjectName, includeInactive) { }
    }
    #endregion Obsolete version
}