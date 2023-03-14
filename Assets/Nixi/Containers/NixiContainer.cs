using Nixi.Containers.ContainerElements;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nixi.Containers
{
    /// <summary>
    /// Container used to fill fields decorated with a non-component Nixi decorator or SerializeField
    /// </summary>
    public static class NixiContainer
    {
        /// <summary>
        /// Elements registered in the NixiContainer
        /// </summary>
        private static List<ContainerElement> registrations = new List<ContainerElement>();

        /// <summary>
        /// Map an interface type with an implementation type, this is the transient approach (create a new instance at each resolve call)
        /// </summary>
        /// <typeparam name="TInterface">Interface key type</typeparam>
        /// <typeparam name="TImplementation">Implementation value type</typeparam>
        /// <param name="constructorParameters">Optional parameters passed to constructor when constructing implementation, call default constructor if not filled</param>
        public static void MapTransient<TInterface, TImplementation>(params object[] constructorParameters)
            where TImplementation : class, TInterface
        {
            CheckImplementationIsNotComponent<TImplementation>();
            Map<TInterface, TImplementation, TransientContainerElement>(constructorParameters);
        }

        /// <summary>
        /// Map an interface type with an implementation type, this is the singleton approach (return the same instance at each resolve call or create it if never called)
        /// </summary>
        /// <typeparam name="TInterface">Interface key type</typeparam>
        /// <typeparam name="TImplementation">Implementation value type</typeparam>
        /// <param name="constructorParameters">Optional parameters passed to constructor when constructing implementation, call default constructor if not filled</param>
        public static void MapSingleton<TInterface, TImplementation>(params object[] constructorParameters)
            where TImplementation : class, TInterface
        {
            CheckImplementationIsNotComponent<TImplementation>();
            Map<TInterface, TImplementation, SingleContainerElement>(constructorParameters);
        }

        /// <summary>
        /// Check if implementation type is not derived from Component/MonoBehaviour
        /// </summary>
        /// <typeparam name="TImplementation">Implementation value type</typeparam>
        /// <exception cref="NixiContainerException">Thrown if TImplementation is derived from UnityEngine.Component type</exception>
        private static void CheckImplementationIsNotComponent<TImplementation>()
            where TImplementation : class
        {
            if (typeof(Component).IsAssignableFrom(typeof(TImplementation)))
            {
                throw new NixiContainerException("Cannot construct a class derived from Component in NixiContainer directly, " +
                                                 "it is only allowed when passing an implementation directly from the scene with " +
                                                 "MapSingle<TInterface, TImplementation>(TImplementation implementationToRegister) method");
            }
        }

        /// <summary>
        /// Map an interface type with an implementation type and register its instance from implementationToRegister parameter,
        /// this is done with a singleton approach (return the same instance at each resolve call or create it if never called)
        /// </summary>
        /// <typeparam name="TInterface">Interface key type</typeparam>
        /// <typeparam name="TImplementation">Implementation value type</typeparam>
        /// <param name="implementationToRegister">Implementation to register</param>
        public static void MapSingletonWithImplementation<TInterface, TImplementation>(TImplementation implementationToRegister)
            where TImplementation : class, TInterface
        {
            SingleContainerElement newMapping = Map<TInterface, TImplementation, SingleContainerElement>();
            newMapping.Instance = implementationToRegister;
        }

        /// <summary>
        /// Generic way to map an interface type with an implementation type
        /// </summary>
        /// <typeparam name="TInterface">Interface key type</typeparam>
        /// <typeparam name="TImplementation">Implementation value type</typeparam>
        /// <typeparam name="TypeOfMapping">ContainerElement which correspond to the of mapping (transient or singleton)</typeparam>
        private static TypeOfMapping Map<TInterface, TImplementation, TypeOfMapping>(params object[] constructorParameters)
            where TImplementation : class, TInterface
            where TypeOfMapping : ContainerElement, new()
        {
            Type interfaceType = typeof(TInterface);
            Type implementationType = typeof(TImplementation);

            TypeOfMapping newMapping = new TypeOfMapping
            {
                KeyType = interfaceType,
                ValueType = implementationType,
                ConstructorParameters = constructorParameters
            };
            AddMapping(newMapping);
            return newMapping;
        }

        /// <summary>
        /// Check mapping is valid and add it in the container
        /// </summary>
        /// <param name="elementToAdd">Container element to add</param>
        private static void AddMapping(ContainerElement elementToAdd)
        {
            CheckIfNotInterfaceOrAlreadyRegistered(elementToAdd.KeyType);
            registrations.Add(elementToAdd);
        }

        /// <summary>
        /// Check if keyType is not an interface and if it is not already registered in the container
        /// </summary>
        /// <param name="keyType">Type to check</param>
        /// <exception cref="NixiContainerException">Thrown if keyType is not an interface or if it is already registered in the container</exception>
        private static void CheckIfNotInterfaceOrAlreadyRegistered(Type keyType)
        {
            if (!keyType.IsInterface)
            {
                throw new NixiContainerException($"Cannot register {keyType.Name}, because it is not an interface and only interface " +
                                                 $"can be mapped");
            }

            if (registrations.Any(x => x.KeyType == keyType))
            {
                throw new NixiContainerException($"Cannot add twice the same left member which correspond to the key value : " +
                                                 $"{keyType.Name}, use Remove<{keyType.Name}> if you want to change it");
            }
        }

        /// <summary>
        ///Return an instance that match TInterface if the mapping is registered in the container
        /// </summary>
        /// <typeparam name="TInterface">Interface key type from where the implementation has to be derived</typeparam>
        /// <returns>Instance that was derived from type T</returns>
        public static TInterface ResolveMap<TInterface>()
        {
            return (TInterface)ResolveObject(typeof(TInterface));
        }

        /// <summary>
        /// Return an instance that match the key type if the mapping is registered in the container
        /// </summary>
        /// <param name="keyType">Interface key type from where the implementation has to be derived</param>
        /// <returns>Instance thas was derived from type T</returns>
        public static object ResolveMap(Type keyType)
        {
            return ResolveObject(keyType);
        }

        /// <summary>
        /// Return an instance that match the key type if the mapping is registered in the container
        /// </summary>
        /// <param name="keyType">Interface key type from where the implementation has to be derived</param>
        /// <returns>Instance thas was derived from keyType</returns>
        /// <exception cref="NixiContainerException">Thrown if keyType is not found in registrations</exception>
        private static object ResolveObject(Type keyType)
        {
            IEnumerable<ContainerElement> elementToResolves = registrations.Where(x => x.KeyType == keyType);

            if (!elementToResolves.Any())
                throw new NixiContainerException($"Cannot find mapping with the key type {keyType.Name}");

            ContainerElement elementResolved = elementToResolves.Single();

            if (elementResolved is SingleContainerElement singleElementToResolve)
            {
                // Singleton
                if (singleElementToResolve.Instance == null)
                {
                    singleElementToResolve.Instance = Activator.CreateInstance(singleElementToResolve.ValueType, elementResolved.ConstructorParameters);
                }
                return singleElementToResolve.Instance;
            }
            else
            {
                // Transient
                return Activator.CreateInstance(elementResolved.ValueType, elementResolved.ConstructorParameters);
            }
        }

        /// <summary>
        /// Check if an interface was already registered in the container
        /// </summary>
        /// <typeparam name="TInterface">Type of interface to check</typeparam>
        /// <returns>True if already registered</returns>
        /// <exception cref="NixiContainerException">Thrown if keyType is not an interface</exception>
        public static bool CheckIfMappingRegistered<TInterface>()
        {
            return CheckIfMappingRegistered(typeof(TInterface));
        }

        /// <summary>
        /// Check if an interface was already registered in the container
        /// </summary>
        /// <param name="keyType">Type of interface to check</param>
        /// <returns>True if already registered</returns>
        /// <exception cref="NixiContainerException">Thrown if keyType is not an interface</exception>
        public static bool CheckIfMappingRegistered(Type keyType)
        {
            if (!keyType.IsInterface)
                throw new NixiContainerException($"Cannot check if {keyType.Name} is registered, because it is not an interface and only interface can be mapped");

            return registrations.Any(x => x.KeyType == keyType);
        }

        /// <summary>
        /// Delete mapping with key type T if it is registered
        /// </summary>
        /// <typeparam name="TInterface">Interface key type to find in the registrations</typeparam>
        public static void RemoveMap<TInterface>()
        {
            RemoveMap(typeof(TInterface));
        }

        /// <summary>
        /// Delete mapping with key type if it is registered
        /// </summary>
        /// <param name="keyType">Type of interface to remove</param>
        public static void RemoveMap(Type keyType)
        {
            ContainerElement elementToResolve = registrations.SingleOrDefault(x => x.KeyType == keyType);
            registrations.Remove(elementToResolve);
        }

        public static void RegisterIfNotAlreadyRegistered<TInterface, TImplementation>(TImplementation implementation)
           where TImplementation : class, TInterface, new()
        {
            if (!CheckIfMappingRegistered<TInterface>())
            {
                MapSingletonWithImplementation<TInterface, TImplementation>(implementation);
            }
        }
    }
}