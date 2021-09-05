using Nixi.Injections.Attributes.MonoBehaviours.Abstractions;
using Nixi.Injections.Injecters;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nixi.Injections.Attributes.MonoBehaviours
{
    /// <summary>
    /// Attribute used to define a dependency injection in a MonoBehaviour field of a class instance derived from MonoBehaviourInjectable with a Nixi approach
    /// with Unity dependency injection approach
    /// <para/>This one get the method associated to GameObjectMethod and call it from the instance of the class derived from MonoBehaviourInjectable and fill the MonoBehaviour field
    /// <para/>This is filtered to match GameObjectType and GameObjectNameToFind
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NixInjectMonoBehaviourFromMethodAttribute : NixInjectMonoBehaviourBaseAttribute, IHaveGameObjectNameToFind
    {
        /// <summary>
        /// Method to use to find the GameObject with the name that match GameObjectName
        /// </summary>
        public GameObjectMethod GameObjectMethod { get; private set; }

        /// <summary>
        /// Name of the GameObject to find
        /// </summary>
        public string GameObjectNameToFind { get; private set; }

        /// <summary>
        /// Attribute used to define a dependency injection in a MonoBehaviour field of a class instance derived from MonoBehaviourInjectable with a Nixi approach
        /// with Unity dependency injection approach
        /// <para/>This one get the method associated to GameObjectMethod and call it from the instance of the class derived from MonoBehaviourInjectable and fill the MonoBehaviour field
        /// <para/>This is filtered to match GameObjectType and GameObjectNameToFind
        /// </summary>
        /// <param name="gameObjectNameToFind">Name of the GameObject to find</param>
        /// <param name="gameObjectMethod">Method to use to find the GameObject with the name that match GameObjectName</param>
        public NixInjectMonoBehaviourFromMethodAttribute(string gameObjectNameToFind, GameObjectMethod gameObjectMethod)
        {
            GameObjectNameToFind = gameObjectNameToFind;
            GameObjectMethod = gameObjectMethod;
        }

        /// <summary>
        /// Find the unique component which exactly matches criteria of a derived attribute from NixInjectMonoBehaviourFromMethodAttribute using the Unity dependency injection method
        /// </summary>
        /// <param name="monoBehaviourInjectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="gameObjectTypeToFind">GameObject type to find</param>
        /// <returns>Unique component which exactly matches criteria of a NixInjectMonoBehaviour injection using the Unity dependency injection method</returns>
        public override Component GetSingleComponent(MonoBehaviourInjectable monoBehaviourInjectable, Type gameObjectTypeToFind)
        {
            Component[] componentsFound = GetComponentsFromGameObjectMethod(monoBehaviourInjectable, gameObjectTypeToFind);
            return CheckAndGetSingleComponent(monoBehaviourInjectable, gameObjectTypeToFind, componentsFound);
        }

        /// <summary>
        /// Get the method associated to the GameObjectMethod and call it to get Components from the instance of the class derived from MonoBehaviourInjectable
        /// </summary>
        /// <param name="monoBehaviourInjectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="gameObjectTypeToFind">GameObject type to find</param>
        /// <returns>All components returned from the method call</returns>
        private Component[] GetComponentsFromGameObjectMethod(MonoBehaviourInjectable monoBehaviourInjectable, Type gameObjectTypeToFind)
        {
            if (GameObjectMethod == GameObjectMethod.GetComponentsInChildren)
            {
                return monoBehaviourInjectable.GetComponentsInChildren(gameObjectTypeToFind);
            }
            return monoBehaviourInjectable.GetComponentsInParent(gameObjectTypeToFind);
        }

        /// <summary>
        /// Check if there is only one component that match criteria from the result of the Unity dependency injection method call
        /// </summary>
        /// <param name="monoBehaviourInjectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="gameObjectTypeToFind">GameObject type to find</param>
        /// <param name="componentsFound">All the components returned by Unity dependency injection method</param>
        /// <returns>Unique component which exactly matches criteria</returns>
        private Component CheckAndGetSingleComponent(MonoBehaviourInjectable monoBehaviourInjectable, Type gameObjectTypeToFind, Component[] componentsFound)
        {
            if (!componentsFound.Any())
                throw new NixInjecterException($"No component with type {gameObjectTypeToFind.Name} was found", monoBehaviourInjectable);

            IEnumerable<Component> componentsWithName = componentsFound.Where(x => x.name == GameObjectNameToFind);

            if (!componentsWithName.Any())
                throw new NixInjecterException($"No component with type {gameObjectTypeToFind.Name} and name {GameObjectNameToFind} was found", monoBehaviourInjectable);

            int nbFound = componentsWithName.Count();
            if (nbFound > 1)
                throw new NixInjecterException($"Multiple components were found with type {gameObjectTypeToFind.Name} and name {GameObjectNameToFind}, could not define which one should be used ({nbFound} found instead of just one)", monoBehaviourInjectable);

            return componentsWithName.Single();
        }
    }
}