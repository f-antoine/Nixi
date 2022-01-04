using System;
using System.Collections.Generic;
using UnityEngine;

namespace NixiTestTools.TestInjecterElements.Utils
{
    /// <summary>
    /// Each mapping added into this container force a type to be used by his derived form during tests using TestInjecter, useful when working on abstract component injected with Nixi
    /// </summary>
    public sealed class AbstractComponentMappingContainer
    {
        /// <summary>
        /// All mappings
        /// </summary>
        private Dictionary<Type, Type> abstractMappings = new Dictionary<Type, Type>();

        /// <summary>
        /// Map a component type with a derived type
        /// </summary>
        /// <typeparam name="TAbstract">Component key type</typeparam>
        /// <typeparam name="TImplementation">Implementation type</typeparam>
        public void Map<TAbstract, TImplementation>()
            where TAbstract : Component
            where TImplementation : class, TAbstract
        {
            Type abstractType = typeof(TAbstract);
            Type implementationType = typeof(TImplementation);

            if (abstractType.IsAssignableFrom(typeof(Transform)))
                throw new AbstractComponentMappingException($"Cannot map {abstractType.Name} with {implementationType.Name}, because abstract type ({abstractType.Name}) must not be derived from transform");

            if (abstractMappings.ContainsKey(abstractType))
                throw new AbstractComponentMappingException($"Cannot map {abstractType.Name} with {implementationType.Name}, because abstract type ({abstractType.Name}) is already registered");

            abstractMappings[abstractType] = implementationType;
        }

        /// <summary>
        /// Return implementation type based on abstract type added with Map method
        /// </summary>
        /// <typeparam name="TAbstract">Component key type</typeparam>
        /// <returns>Type derived/mapped with TAbstract</returns>
        public Type TryResolve<TAbstract>()
        {
            return TryResolve(typeof(TAbstract));
        }

        /// <summary>
        /// Return implementation type based on abstract type added with Map method
        /// </summary>
        /// <param name="abstractType">Component key type</param>
        /// <returns>Type derived/mapped with TAbstract</returns>
        public Type TryResolve(Type abstractType)
        {
            if (!abstractMappings.ContainsKey(abstractType))
                return null;

            return abstractMappings[abstractType];
        }
    }
}