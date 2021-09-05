using System;
using UnityEngine;

namespace Nixi.Injections.Attributes.MonoBehaviours.Abstractions
{
    /// <summary>
    /// Base attribute to represent a dependency injection in a MonoBehaviour field of a class instance derived from MonoBehaviourInjectable with a Nixi approach
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public abstract class NixInjectMonoBehaviourBaseAttribute : Attribute
    {
        /// <summary>
        /// Find the unique component which exactly matches criteria of a derived attribute from NixInjectMonoBehaviourBaseAttribute using the Unity dependency injection method
        /// </summary>
        /// <param name="monoBehaviourInjectable">Instance of the MonoBehaviourInjectable</param>
        /// <param name="gameObjectTypeToFind">GameObject type to find</param>
        /// <returns>Unique component which exactly matches criteria of a NixInjectMonoBehaviour injection using the Unity dependency injection method</returns>
        public abstract Component GetSingleComponent(MonoBehaviourInjectable monoBehaviourInjectable, Type gameObjectTypeToFind);
    }
}