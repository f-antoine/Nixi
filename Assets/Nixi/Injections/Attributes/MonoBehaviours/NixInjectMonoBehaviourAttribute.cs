using Nixi.Injections.Attributes.MonoBehaviours.Abstractions;
using Nixi.Injections.Injecters;
using System;
using System.Linq;
using UnityEngine;

namespace Nixi.Injections.Attributes.MonoBehaviours
{
    /// <summary>
    /// Attribute used to define a dependency injection in a MonoBehaviour field of a class instance derived from MonoBehaviourInjectable with a Nixi approach
    /// with Unity dependency injection approach
    /// <para/>This one use GetComponent(gameObjectTypeToFind) from the instance of the class derived from MonoBehaviourInjectable and fill the MonoBehaviour field
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NixInjectMonoBehaviourAttribute : NixInjectMonoBehaviourBaseAttribute
    {
        /// <summary>
        /// Find the unique component which exactly matches criteria of a derived attribute from NixInjectMonoBehaviourAttribute using the Unity dependency injection method
        /// </summary>
        /// <param name="monoBehaviourInjectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="gameObjectTypeToFind">GameObject type to find</param>
        /// <returns>Unique component which exactly matches criteria of a NixInjectMonoBehaviour injection using the Unity dependency injection method</returns>
        public override Component GetSingleComponent(MonoBehaviourInjectable monoBehaviourInjectable, Type gameObjectTypeToFind)
        {
            Component[] componentsFound = monoBehaviourInjectable.GetComponents(gameObjectTypeToFind);
            return CheckAndGetSingleComponent(monoBehaviourInjectable, gameObjectTypeToFind, componentsFound);
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

            if (componentsFound.Length > 1)
                throw new NixInjecterException($"Multiple components were found with type {gameObjectTypeToFind.Name}, could not define which one should be used ({componentsFound.Length} found instead of just one)", monoBehaviourInjectable);

            return componentsFound.Single();
        }
    }
}