using Nixi.Injections;
using Nixi.Injections.Injecters;
using NixiTestTools.TestInjecterElements.Relations.Abstractions;
using NixiTestTools.TestInjecterElements.Relations.Components;
using NixiTestTools.TestInjecterElements.Relations.RootRelations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace NixiTestTools.TestInjecterElements
{
    /// <summary>
    /// Container to handle TestInjecter instances, it is used to injectMock into a field or GetComponent from fields/root instantiated during the Test injections
    /// </summary>
    internal sealed class InjectablesContainer
    {
        /// <summary>
        /// All MonoBehaviourInjectable instances
        /// </summary>
        private List<MonoBehaviourInjectableData> injectables = new List<MonoBehaviourInjectableData>();

        /// <summary>
        /// Handle all root components with their children relation at one level below
        /// </summary>
        private RootRelationHandler rootRelationHandler = new RootRelationHandler();

        /// <summary>
        /// Find data of a MonoBehaviourInjectable from UnityEngine.Object.GetInstanceID()
        /// </summary>
        /// <param name="injectable">Injectable from which we look for data</param>
        /// <returns>Data of the injectable</returns>
        private MonoBehaviourInjectableData GetInjectableData(MonoBehaviourInjectable injectable)
        {
            MonoBehaviourInjectableData injectableData = GetInjectable(injectable);

            if (injectableData == null)
                throw new InjectablesContainerException($"no instance of MonoBehaviourInjectable with name {injectable.name} was found, {injectable.GetType().Name} class does not contain the field");

            return injectableData;
        }

        /// <summary>
        /// Return MonoBehaviourInjectableData which has Instance.GetInstanceID() equals to injectable.GetInstanceID()
        /// </summary>
        /// <param name="injectable">MonoBehaviourInjectable searched</param>
        /// <returns>MonoBehaviourInjectableData searched</returns>
        internal MonoBehaviourInjectableData GetInjectable(MonoBehaviourInjectable injectable)
        {
            return injectables.SingleOrDefault(x => x.Instance.GetInstanceID() == injectable.GetInstanceID());
        }

        /// <summary>
        /// Return MonoBehaviourInjectableData which has InstanceName equals to instanceName and Instance type equals typeOfInstanceToFind
        /// </summary>
        /// <param name="instanceName">Name searched</param>
        /// <param name="typeOfInstanceToFind">Type of instance searched</param>
        /// <returns>MonoBehaviourInjectableData searched</returns>
        internal MonoBehaviourInjectableData GetInjectable(string instanceName, Type typeOfInstanceToFind)
        {
            return injectables.SingleOrDefault(x => x.InstanceName == instanceName && x.Instance.GetType() == typeOfInstanceToFind);
        }

        /// <summary>
        /// Inject manually a mock into the Non-Component field with T Type in a MonoBehaviourInjectable
        /// </summary>
        /// <typeparam name="T">Targeted field type</typeparam>
        /// <param name="mockToInject">Mock to inject into field</param>
        /// <param name="monoBehaviourInjectableToFind">Targeted injectable</param>
        internal void InjectMockIntoInstance<T>(T mockToInject, MonoBehaviourInjectable monoBehaviourInjectableToFind)
        {
            MonoBehaviourInjectableData injectableData = GetInjectableData(monoBehaviourInjectableToFind);

            Type interfaceType = typeof(T);

            IEnumerable<SimpleFieldInfo> fieldSelecteds = injectableData.FieldHandler.Fields.Where(x => x.FieldInfo.FieldType == interfaceType);

            if (!fieldSelecteds.Any())
                throw new InjectablesContainerException($"no field with type {interfaceType.Name} was found");

            if (fieldSelecteds.Count() > 1)
                throw new InjectablesContainerException($"multiple fields with type {interfaceType.Name} were found, cannot define which one use, please use InjectMock<T>(T mockToInject, string fieldName) instead");

            fieldSelecteds.Single().FieldInfo.SetValue(injectableData.Instance, mockToInject);
        }

        /// <summary>
        /// Inject manually a mock into the Non-Component field with T Type and with the fieldName passed as a parameter in a MonoBehaviourInjectable
        /// </summary>
        /// <typeparam name="T">Targeted field type</typeparam>
        /// <param name="fieldName">Name of the field to mock</param>
        /// <param name="mockToInject">Mock to inject into field</param>
        /// <param name="monoBehaviourInjectableToFind">Targeted injectable</param>
        internal void InjectMockIntoInstance<T>(T mockToInject, MonoBehaviourInjectable monoBehaviourInjectableToFind, string fieldName)
        {
            MonoBehaviourInjectableData injectableData = GetInjectableData(monoBehaviourInjectableToFind);

            Type interfaceType = typeof(T);

            IEnumerable<SimpleFieldInfo> fieldsWithType = injectableData.FieldHandler.Fields.Where(x => x.FieldInfo.FieldType == interfaceType);
            if (!fieldsWithType.Any())
                throw new InjectablesContainerException($"no field with type {interfaceType.Name}");

            IEnumerable<SimpleFieldInfo> fieldsWithTypeAndName = fieldsWithType.Where(x => x.FieldInfo.Name == fieldName);
            if (!fieldsWithTypeAndName.Any())
                throw new InjectablesContainerException($"field with type {interfaceType.Name} was/were found, but none with fieldName {fieldName}");

            fieldsWithTypeAndName.Single().FieldInfo.SetValue(injectableData.Instance, mockToInject);
        }

        /// <summary>
        /// Add a MonoBehaviourInjectableData to the list of injectables
        /// </summary>
        /// <param name="monoBehaviourInjectableData">MonoBehaviourInjectableData to add</param>
        internal void Add(MonoBehaviourInjectableData monoBehaviourInjectableData)
        {
            injectables.Add(monoBehaviourInjectableData);
        }

        /// <summary>
        /// Return the only component with Component type T field from a MonoBehaviourInjectable
        /// <para/>If multiple type T fields are found, you must use GetComponent<T>(string fieldName) 
        /// </summary>
        /// <typeparam name="T">Type of Component searched</typeparam>
        /// <param name="monoBehaviourInjectableToFind">Targeted injectable</param>
        /// <returns>Single component which corresponds to the type T field</returns>
        internal T GetComponentFromInstance<T>(MonoBehaviourInjectable monoBehaviourInjectableToFind)
            where T : Component
        {
            MonoBehaviourInjectableData injectableData = GetInjectableData(monoBehaviourInjectableToFind);

            Type typeToFind = typeof(T);

            IEnumerable<ComponentWithFieldInfo> componentsWithType = injectableData.ComponentRelationHandler.Fields.Where(x => x.FieldInfo.FieldType == typeToFind);

            if (!componentsWithType.Any())
                throw new InjectablesContainerException($"no component with type {typeToFind.Name} was found");

            if (componentsWithType.Count() > 1)
                throw new InjectablesContainerException($"multiple components with type {typeToFind.Name} were found, cannot define which one use, please use GetComponent(fieldName)");

            return componentsWithType.Single().Component.GetComponent<T>();
        }

        /// <summary>
        /// Return the only component with Component type T field and with fieldName passed as parameter from a MonoBehaviourInjectable
        /// <para/>If multiple type T fields are found, you must use GetComponent<T>(string fieldName) 
        /// </summary>
        /// <typeparam name="T">Type of Component searched</typeparam>
        /// <param name="monoBehaviourInjectableToFind">Targeted injectable</param>
        /// <param name="fieldName">Name of the component field</param>
        /// <returns>Single component which corresponds to the type T field</returns>
        internal T GetComponentFromInstance<T>(MonoBehaviourInjectable monoBehaviourInjectableToFind, string fieldName)
            where T : Component
        {
            MonoBehaviourInjectableData injectableData = GetInjectableData(monoBehaviourInjectableToFind);

            Type typeToFind = typeof(T);

            IEnumerable<ComponentWithFieldInfo> componentsWithFieldType = injectableData.ComponentRelationHandler.Fields.Where(x => x.FieldInfo.FieldType == typeToFind);
            if (!componentsWithFieldType.Any())
                throw new InjectablesContainerException($"no component with type {typeToFind.Name} was found");

            IEnumerable <ComponentWithFieldInfo> componentsWithFieldTypeAndFieldName = componentsWithFieldType.Where(x => x.FieldInfo.Name == fieldName);
            if (!componentsWithFieldTypeAndFieldName.Any())
                throw new InjectablesContainerException($"component with type {typeToFind.Name} was/were found, but none with field name {fieldName}");

            return componentsWithFieldTypeAndFieldName.Single().Component.GetComponent<T>();
        }

        #region ComponentList
        /// <summary>
        /// Return list of components instantiated in a MonoBehaviourInjectable field which match an enumerable of type T (inherited from Component)
        /// </summary>
        /// <typeparam name="T">Enumerable type (inherited from Component)</typeparam>
        /// <param name="monoBehaviourInjectableToFind">Targeted injectable</param>
        /// <returns>List of component instantiated which corresponds to the type T field</returns>
        internal IEnumerable<T> GetComponentListFromInstance<T>(MonoBehaviourInjectable targetedInjectable)
            where T : Component
        {
            // We get all fieldInfos with enumerable of type T, it can hold many because IEnumerable != Array != List, etc.
            IEnumerable<ComponentListWithFieldInfo> componentsListWithType = FindComponentList<T>(targetedInjectable);
            
            // Take first because if many, they all have same components
            return componentsListWithType.First().Components.Select(x => x.GetComponent<T>());
        }

        /// <summary>
        /// Find componentsList instantiated in a MonoBehaviourInjectable field which match an enumerable of type T (inherited from Component)
        /// </summary>
        /// <typeparam name="T">Enumerable type searched inherited from Component)</typeparam>
        /// <param name="targetedInjectable">Targeted injectable</param>
        /// <returns>List of component instantiated which corresponds to type T field</returns>
        private IEnumerable<ComponentListWithFieldInfo> FindComponentList<T>(MonoBehaviourInjectable targetedInjectable)
            where T : Component
        {
            MonoBehaviourInjectableData injectableData = GetInjectableData(targetedInjectable);

            Type typeToFind = typeof(T);

            IEnumerable<ComponentListWithFieldInfo> componentsListWithType = injectableData.EnumerableComponentRelationHandler.Fields.Where(x => x.EnumerableType.IsAssignableFrom(typeToFind));
            if (!componentsListWithType.Any())
                throw new InjectablesContainerException($"no component list with type {typeToFind.Name} was found");

            return componentsListWithType;
        }

        /// <summary>
        /// Add an element in the list of components instantiated with Enumerable Component type T field (apply on all enumerable of same EnumerableType
        /// </summary>
        /// <typeparam name="T">Enumerable type searched inherited from Component)</typeparam>
        /// <returns>New component instantiated which corresponds to type T field</returns>
        internal T AddInComponentList<T>(MonoBehaviourInjectable monoBehaviourInjectableToFind)
            where T : Component
        {
            T newComponent = new GameObject().AddComponent<T>();

            foreach (ComponentListWithFieldInfo componentsListWithType in FindComponentList<T>(monoBehaviourInjectableToFind))
            {
                if (componentsListWithType.EnumerableType.IsAssignableFrom(typeof(T)))
                {
                    componentsListWithType.Components.Add(newComponent);
                    componentsListWithType.FieldInfo.SetValue(monoBehaviourInjectableToFind, componentsListWithType.Components, BindingFlags.InvokeMethod, new EnumerableComponentBinder(), CultureInfo.InvariantCulture);
                }
            }

            return newComponent;
        }
        #endregion ComponentList

        #region RootComponent
        /// <summary>
        /// Add parent root relation if not already exists, if it exists it adds component on this parent if not already added on it
        /// and refresh link between childs and parent gameObjects
        /// </summary>
        /// <param name="componentToAdd">Component to link to new or updated parent</param>
        /// <param name="parentName">Name of the parent relation root</param>
        public void AddOrUpdateRootRelation(Component componentToAdd, string parentName)
        {
            RootRelation parentRelation = rootRelationHandler.GetParentRelation(parentName);

            if (parentRelation == null)
                rootRelationHandler.AddParentWithoutChilds(parentName, componentToAdd);
            else
                rootRelationHandler.AddComponentIntoParentRelation(parentRelation, componentToAdd);
        }

        /// <summary>
        /// Add child to a parent root relation if not already exists, if it exists it adds component on this child if not already added on it
        /// and refresh link between childs and parent gameObjects
        /// </summary>
        /// <param name="componentToAdd">Component to link to new or updated parent</param>
        /// <param name="parentName">Name of the parent relation root</param>
        public void AddOrUpdateRootRelation(Component componentToAdd, string parentName, string childName)
        {
            if (string.IsNullOrEmpty(childName))
            {
                AddOrUpdateRootRelation(componentToAdd, parentName);
                return;
            }

            RootRelation parentRelation = rootRelationHandler.GetParentRelation(parentName);

            if (parentRelation == null)
            {
                rootRelationHandler.AddParentWithChilds(parentName, childName, componentToAdd);
            }
            else
            {
                ComponentsWithName child = rootRelationHandler.GetChildComponentsWithName(parentRelation, childName);

                if (child == null)
                    rootRelationHandler.AddAndLinkChildToParent(parentRelation, childName, componentToAdd);
                else
                    rootRelationHandler.UpdateAndLinkChildToParent(parentRelation, child, componentToAdd);
            }
        }

        /// <summary>
        /// Get all the components of a parent root relation (at parent level) with parentName
        /// </summary>
        /// <param name="parentName">Parent root relation name</param>
        /// <returns>All the components of a parent root relation (at parent level) with parentName</returns>
        public IEnumerable<Component> GetComponentsFromRelation(string parentName)
        {
            return rootRelationHandler.GetComponentsFromRelation(parentName);
        }

        /// <summary>
        /// Get all the components of a child in a parent root relation with name of parent = parentName and with name of child = childName (at child level from parent rootRelation with parentName)
        /// <para/> If childName is empty or null, return all the parent components
        /// </summary>
        /// <param name="parentName">Parent root relation name</param>
        /// <param name="childName">Name of the child in parent root relation name</param>
        /// <returns>All the components of a child in a parent root relation with parentName and with childName (at child level from parent rootRelation with parentName)</returns>
        public IEnumerable<Component> GetComponentsFromRelation(string parentName, string childName)
        {
            return rootRelationHandler.GetComponentsFromRelation(parentName, childName);
        }

        /// <summary>
        /// Get unique component of a parent root relation (at parent level) with parentName, which has type componentType
        /// </summary>
        /// <param name="componentType">Targeted componentType</param>
        /// <param name="parentName">Parent root relation name</param>
        /// <returns>Unique component of a parent root relation (at parent level) with parentName, which has type componentType</returns>
        public Component GetComponentFromRelation(Type componentType, string parentName)
        {
            return rootRelationHandler.GetComponentFromRelation(componentType, parentName);
        }

        /// <summary>
        /// Get unique component of a child in a parent root relation with parentName and with childName (at child level from parent rootRelation with parentName), which has type componentType
        /// </summary>
        /// <param name="componentType">Targeted componentType</param>
        /// <param name="parentName">Parent root relation name</param>
        /// <param name="childName">Name of the child in parent root relation name</param>
        /// <returns>Unique component of a child in a parent root relation with parentName and with childName (at child level from parent rootRelation with parentName), which has type componentType</returns>
        public Component GetComponentFromRelation(Type componentType, string parentName, string childName)
        {
            return rootRelationHandler.GetComponentFromRelation(componentType, parentName, childName);
        }
        #endregion RootComponent
    }
}