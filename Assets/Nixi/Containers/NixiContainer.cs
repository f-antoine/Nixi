using Nixi.Containers.ContainerElements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nixi.Containers
{
    /// <summary>
    /// Container used to fill fields decorated with a Non-Component NixInjection
    /// </summary>
    public static class NixiContainer
    {
        /// <summary>
        /// Element contained in the NixiContainer
        /// </summary>
        private static List<ContainerElement> registrations = new List<ContainerElement>();

        /// <summary>
        /// Map an interface type with an implementation type, with a transient approach (create a new instance at each resolve call)
        /// </summary>
        /// <typeparam name="TInterface">Interface key type</typeparam>
        /// <typeparam name="TImplementation">Implementation value type</typeparam>
        public static void MapTransient<TInterface, TImplementation>(params object[] constructorParameters)
            where TImplementation : class, TInterface
        {
            Map<TInterface, TImplementation, TransientContainerElement>(constructorParameters);
        }

        /// <summary>
        /// Map an interface type with an implementation type, with a singleton approach (return the same instance at each resolve call or create it if never called)
        /// </summary>
        /// <typeparam name="TInterface">Interface key type</typeparam>
        /// <typeparam name="TImplementation">Implementation value type</typeparam>
        public static void MapSingle<TInterface, TImplementation>(params object[] constructorParameters)
            where TImplementation : class, TInterface
        {
            Map<TInterface, TImplementation, SingleContainerElement>(constructorParameters);
        }

        /// <summary>
        /// Map an interface type with an implementation type and register his instance from implementationToRegister parameter,
        /// with a singleton approach (return the same instance at each resolve call or create it if never called)
        /// </summary>
        /// <typeparam name="TInterface">Interface key type</typeparam>
        /// <typeparam name="TImplementation">Implementation value type</typeparam>
        /// <param name="implementationToRegister">Pass an implementation to register instead of building manually</param>
        public static void MapSingle<TInterface, TImplementation>(TImplementation implementationToRegister)
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
        /// Add a mapping in the container if not exist, if you try to add two same KeyType it will throw an exception
        /// </summary>
        /// <param name="elementToAdd">Container element to add</param>
        private static void AddMapping(ContainerElement elementToAdd)
        {
            if (CheckIfRegistered(elementToAdd.KeyType))
                throw new NixiContainerException($"Cannot add twice the same left member which correspond to the key value : {elementToAdd.KeyType.Name}, use Remove<{elementToAdd.KeyType.Name}> if you want to change it");

            registrations.Add(elementToAdd);
        }

        /// <summary>
        /// Return an instance that match the key type T if the mapping is registered in the container
        /// </summary>
        /// <typeparam name="TInterface">Interface key type from where the implementation has to be derived</typeparam>
        /// <returns>Instance thas was derived from type T</returns>
        public static TInterface Resolve<TInterface>()
        {
            return (TInterface)ResolveObject(typeof(TInterface));
        }

        /// <summary>
        /// Return an instance that match the key type if the mapping is registered in the container
        /// </summary>
        /// <param name="keyType">Interface key type from where the implementation has to be derived</param>
        /// <returns>Instance thas was derived from type T</returns>
        public static object Resolve(Type keyType)
        {
            return ResolveObject(keyType);
        }

        /// <summary>
        /// Return an implementation dervied of key type if the mapping is registered in the container
        /// </summary>
        /// <param name="keyType">Interface key type from where the implementation has to be derived</param>
        /// <returns>Instance thas was derived from type T</returns>
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
        public static bool CheckIfRegistered<TInterface>()
        {
            return CheckIfRegistered(typeof(TInterface));
        }

        /// <summary>
        /// Check if an interface was already registered in the container
        /// </summary>
        /// <param name="keyType">Type of interface to check</param>
        /// <returns>True if already registered</returns>
        public static bool CheckIfRegistered(Type keyType)
        {
            if (!keyType.IsInterface)
                throw new NixiContainerException($"Cannot check if {keyType.Name} is registered, because it is not an interface and only interface can be mapped");

            return registrations.Any(x => x.KeyType == keyType);
        }

        /// <summary>
        /// Delete a mapping with a key type T if it is registered
        /// </summary>
        /// <typeparam name="TInterface">Interface key type to find in the registrations</typeparam>
        public static void RemoveMap<TInterface>()
        {
            RemoveMap(typeof(TInterface));
        }

        /// <summary>
        /// Delete a mapping with a key type T if it is registered
        /// </summary>
        /// <param name="keyType">Type of interface to check</param>
        public static void RemoveMap(Type keyType)
        {
            ContainerElement elementToResolve = registrations.SingleOrDefault(x => x.KeyType == keyType);
            registrations.Remove(elementToResolve);
        }
    }
}