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
        public static void MapTransient<TInterface, TImplementation>()
            where TImplementation : class, TInterface, new()
        {
            Map<TInterface, TImplementation, TransientContainerElement>();
        }

        /// <summary>
        /// Map an interface type with an implementation type, with a singleton approach (return the same instance at each resolve call or create it if never called)
        /// </summary>
        /// <typeparam name="TInterface">Interface key type</typeparam>
        /// <typeparam name="TImplementation">Implementation value type</typeparam>
        public static void MapSingle<TInterface, TImplementation>()
            where TImplementation : class, TInterface, new()
        {
            Map<TInterface, TImplementation, SingleContainerElement>();
        }

        /// <summary>
        /// Generic way to map an interface type with an implementation type
        /// </summary>
        /// <typeparam name="TInterface">Interface key type</typeparam>
        /// <typeparam name="TImplementation">Implementation value type</typeparam>
        private static void Map<TInterface, TImplementation, TypeOfMapping>()
            where TImplementation : class, TInterface, new()
            where TypeOfMapping : ContainerElement, new()
        {
            Type interfaceType = typeof(TInterface);
            Type implementationType = typeof(TImplementation);
            CheckInterfaceWithImplementation(interfaceType, implementationType);

            AddMapping(new TypeOfMapping
            {
                KeyType = interfaceType,
                ValueType = implementationType
            });
        }

        /// <summary>
        /// In NixiContainer, the left member of a mapping must be an interface, this method check this
        /// </summary>
        /// <param name="interfaceType">Interface key type</param>
        /// <param name="implementationType">Implementation value type</param>
        private static void CheckInterfaceWithImplementation(Type interfaceType, Type implementationType)
        {
            if (!interfaceType.IsInterface)
                throw new NixiContainerException($"Cannot map left member {interfaceType.Name} with right member {implementationType.Name} because the left member is not an interface");
        }

        /// <summary>
        /// Add a mapping in the container if not exist, if you try to add two same KeyType it will throw an exception
        /// </summary>
        /// <param name="elementToAdd">Container element to add</param>
        private static void AddMapping(ContainerElement elementToAdd)
        {
            if (registrations.Any(x => x.KeyType == elementToAdd.KeyType))
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
                    singleElementToResolve.Instance = Activator.CreateInstance(singleElementToResolve.ValueType);
                }
                return singleElementToResolve.Instance;
            }
            else
            {
                // Transient
                return Activator.CreateInstance(elementResolved.ValueType);
            }
        }

        /// <summary>
        /// Delete a mapping with a key type T if it is registered
        /// </summary>
        /// <typeparam name="TInterface">Interface key type to find in the registrations</typeparam>
        public static void Remove<TInterface>()
        {
            ContainerElement elementToResolve = registrations.SingleOrDefault(x => x.KeyType == typeof(TInterface));
            registrations.Remove(elementToResolve);
        }        
    }
}