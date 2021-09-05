using Nixi.Injections.Attributes.MonoBehaviours.Abstractions;
using Nixi.Injections.Injecters;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nixi.Injections.Attributes.MonoBehaviours
{
    /// <summary>
    /// Attribute used to define a dependency injection in a MonoBehaviour field of a class instance derived from MonoBehaviourInjectable with a Nixi approach
    /// with Unity dependency injection approach
    /// <para/>This one get all the root GameObjects of the current scene then try to find the single one named RootGameObjectName
    /// <para/>Then it gets the method associated to RootGameObjectMethod and call it from the root GameObject,
    /// the result is filtered to match GameObjectType and GameObjectNameToFind and get the unique instance to fill the MonoBehaviour field in the instance of the class derived from MonoBehaviourInjectable
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class NixInjectMonoBehaviourFromMethodRootAttribute : NixInjectMonoBehaviourBaseAttribute, IHaveGameObjectNameToFind
    {
        /// <summary>
        /// Name of the root GameObject to find
        /// </summary>
        public string RootGameObjectName { get; private set; }

        /// <summary>
        /// Method to use to find the GameObject with the name that match GameObjectName starting from the root game Object
        /// </summary>
        public RootGameObjectMethod RootGameObjectMethod { get; private set; }

        /// <summary>
        /// Name of the GameObject to find
        /// </summary>
        public string GameObjectNameToFind { get; private set; }

        /// <summary>
        /// Attribute used to define a dependency injection in a MonoBehaviour field of a class instance derived from MonoBehaviourInjectable with a Nixi approach
        /// with Unity dependency injection approach
        /// <para/>This one get all the root GameObjects of the current scene then try to find the single one named RootGameObjectName
        /// <para/>Then it gets the method associated to RootGameObjectMethod and call it from the root GameObject,
        /// the result is filtered to match GameObjectType and GameObjectNameToFind and get the unique instance to fill the MonoBehaviour field in the instance of the class derived from MonoBehaviourInjectable
        /// </summary>
        /// <param name="rootGameObjectName">Method to use on root GameObjects from the current scene to find a GameObject that match GameObjectName</param>
        public NixInjectMonoBehaviourFromMethodRootAttribute(string rootGameObjectName)
        {
            RootGameObjectName = rootGameObjectName;
            GameObjectNameToFind = rootGameObjectName;
            RootGameObjectMethod = RootGameObjectMethod.GetComponentsInRoot;
        }

        /// <summary>
        /// Attribute used to define a dependency injection in a MonoBehaviour field of a class instance derived from MonoBehaviourInjectable with a Nixi approach
        /// with Unity dependency injection approach
        /// <para/>This one get all the root GameObjects of the current scene then try to find the single one named RootGameObjectName
        /// <para/>Then it gets the method associated to RootGameObjectMethod (GetComponentInChildren) and call it from the root GameObject,
        /// the result is filtered to match GameObjectType and GameObjectNameToFind and get the unique instance to fill the MonoBehaviour field in the instance of the class derived from MonoBehaviourInjectable
        /// </summary>
        /// <param name="gameObjectNameToFind">Name of the GameObject to find in childrens</param>
        /// <param name="rootGameObjectName">Method to use on root GameObjects from the current scene to find a GameObject that match GameObjectName</param>
        public NixInjectMonoBehaviourFromMethodRootAttribute(string rootGameObjectName, string gameObjectNameToFind)
        {
            RootGameObjectName = rootGameObjectName;
            GameObjectNameToFind = gameObjectNameToFind;
            RootGameObjectMethod = RootGameObjectMethod.GetComponentsInChildrenFromRoot;
        }

        /// <summary>
        /// Find the unique component which exactly matches criteria of a derived attribute from NixInjectMonoBehaviourFromRootAttribute using the Unity dependency injection method
        /// </summary>
        /// <param name="monoBehaviourInjectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="gameObjectTypeToFind">GameObject type to find</param>
        /// <returns>Unique component which exactly matches criteria of a NixInjectMonoBehaviour injection using the Unity dependency injection method</returns>
        public override Component GetSingleComponent(MonoBehaviourInjectable monoBehaviourInjectable, Type gameObjectTypeToFind)
        {
            GameObject targetedRootGameObject = GetTargetedRootGameObject(monoBehaviourInjectable);

            Component[] componentsFound = GetComponentsFromRootGameObjectMethod(targetedRootGameObject, gameObjectTypeToFind);

            return CheckAndGetSingleComponent(monoBehaviourInjectable, targetedRootGameObject, gameObjectTypeToFind, componentsFound);
        }

        /// <summary>
        /// Find the unique root GameObject named with the value from RootGameObjectName
        /// </summary>
        /// <param name="monoBehaviourInjectable">Instance of the MonoBehaviourInjectable</param>
        /// <returns>Unique root GameObject named with the value from RootGameObjectName</returns>
        private GameObject GetTargetedRootGameObject(MonoBehaviourInjectable monoBehaviourInjectable)
        {
            GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            IEnumerable<GameObject> rootGameObjectsWithName = rootGameObjects.Where(x => x.name == RootGameObjectName);
            int nbRootGameObjectWithNameFound = rootGameObjectsWithName.Count();

            if (nbRootGameObjectWithNameFound == 0)
                throw new NixInjecterException($"No root GameObject with name {RootGameObjectName} was found in the root GameObjects", monoBehaviourInjectable);

            if (nbRootGameObjectWithNameFound > 1)
                throw new NixInjecterException($"Multiple GameObjects with name {RootGameObjectName} were found in the root GameObjects, cannot define which one to use", monoBehaviourInjectable);

            return rootGameObjectsWithName.Single();
        }

        /// <summary>
        /// Get the method associated to the RootGameObjectMethod and call it to get Components from the root GameObject targeted
        /// </summary>
        /// <param name="targetedRootGameObject">Root GameObject targeted</param>
        /// <param name="gameObjectTypeToFind">GameObject type to find</param>
        /// <returns>All components returned from the method call</returns>
        private Component[] GetComponentsFromRootGameObjectMethod(GameObject targetedRootGameObject, Type gameObjectTypeToFind)
        {
            if (RootGameObjectMethod == RootGameObjectMethod.GetComponentsInChildrenFromRoot)
            {
                return targetedRootGameObject.GetComponentsInChildren(gameObjectTypeToFind);
            }
            return targetedRootGameObject.GetComponents(gameObjectTypeToFind);
        }

        /// <summary>
        /// Check if there is only one component that match criteria from the result of the Unity dependency injection method call
        /// </summary>
        /// <param name="monoBehaviourInjectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="gameObjectTypeToFind">GameObject type to find</param>
        /// <param name="componentsFound">All the components returned by Unity dependency injection method</param>
        /// <returns>Unique component which exactly matches criteria</returns>
        private Component CheckAndGetSingleComponent(MonoBehaviourInjectable monoBehaviourInjectable, GameObject targetedRootGameObject,
                                                     Type gameObjectTypeToFind, Component[] componentsFound)
        {
            if (!componentsFound.Any())
                throw new NixInjecterException($"No component with type {gameObjectTypeToFind.Name} was found on root GameObject with name {targetedRootGameObject.name}", monoBehaviourInjectable);

            if (RootGameObjectMethod == RootGameObjectMethod.GetComponentsInChildrenFromRoot)
            {
                return CheckAndGetUniqueResultFromChildGameObjectNameFromRoot(monoBehaviourInjectable, targetedRootGameObject, gameObjectTypeToFind, componentsFound);
            }
            return CheckAndGetUniqueResultTypeFromRoot(monoBehaviourInjectable, targetedRootGameObject, gameObjectTypeToFind, componentsFound);
        }

        /// <summary>
        /// Check if there is only one component that match criteria from the result of the Unity dependency injection GetComponentsInChildren call
        /// </summary>
        /// <param name="monoBehaviourInjectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="gameObjectTypeToFind">GameObject type to find</param>
        /// <param name="componentsFound">All the components returned by Unity dependency injection method</param>
        /// <returns>Unique component which exactly matches criteria</returns>
        private Component CheckAndGetUniqueResultFromChildGameObjectNameFromRoot(MonoBehaviourInjectable monoBehaviourInjectable, GameObject targetedRootGameObject, Type gameObjectTypeToFind, Component[] componentsFound)
        {
            IEnumerable<Component> componentsWithName = componentsFound.Where(x => x.name == GameObjectNameToFind);

            if (!componentsWithName.Any())
                throw new NixInjecterException($"Components were found with type {gameObjectTypeToFind.Name} but not with name {GameObjectNameToFind} was found on root GameObject with name {targetedRootGameObject.name}", monoBehaviourInjectable);

            int nbFound = componentsWithName.Count();
            if (nbFound > 1)
                throw new NixInjecterException($"Multiple components were found with type {gameObjectTypeToFind.Name} and name {GameObjectNameToFind} on root GameObject with name {targetedRootGameObject.name}, could not define which one should be used ({nbFound} found instead of just one)", monoBehaviourInjectable);

            return componentsWithName.Single();
        }

        /// <summary>
        /// Check if there is only one component that match criteria from the result of the Unity dependency injection GetComponent call
        /// </summary>
        /// <param name="monoBehaviourInjectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="gameObjectTypeToFind">GameObject type to find</param>
        /// <param name="componentsFound">All the components returned by Unity dependency injection method</param>
        /// <returns>Unique component which exactly matches criteria</returns>
        private Component CheckAndGetUniqueResultTypeFromRoot(MonoBehaviourInjectable monoBehaviourInjectable, GameObject targetedRootGameObject, Type gameObjectTypeToFind, Component[] componentsFound)
        {
            int nbFound = componentsFound.Length;
            if (nbFound > 1)
                throw new NixInjecterException($"Multiple components were found with type {gameObjectTypeToFind.Name} on root GameObject with name {targetedRootGameObject.name}, could not define which one should be used ({nbFound} found instead of just one)", monoBehaviourInjectable);

            return componentsFound.Single();
        }
    }
}