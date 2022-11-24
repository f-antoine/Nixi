using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.Enums;
using NixiTestTools.TestInjectorElements.Relations.Components;
using NixiTestTools.TestInjectorElements.Relations.Fields;
using NixiTestTools.TestInjectorElements.Relations.RootRelations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NixiTestTools.TestInjectorElements
{
    /// <summary>
    /// Container to handle TestInjector instances, it is used to inject fields or GetComponent from fields/root instantiated during the tests injections
    /// </summary>
    internal sealed class InjectablesContainer
    {
        /// <summary>
        /// All InjectableHandler instances
        /// </summary>
        private List<InjectableHandler> injectables = new List<InjectableHandler>();

        /// <summary>
        /// Handle all root components with their children relation at one level below
        /// <para/>This handler is here because it concerns the simulated scene directly at the root level and is not directly linked
        /// to any of all MonoBehaviourInjectables injected
        /// </summary>
        private RootRelationHandler rootRelationHandler = new RootRelationHandler();

        #region Managing InjectableHandler
        /// <summary>
        /// Creates an InjectableHandler with a MonoBehaviourInjectable and add it to the list of injectables
        /// </summary>
        /// <param name="instanceName">Name of the instance</param>
        /// <param name="injectable">MonoBehaviourInjectable to load into new InjectableHandler</param>
        /// <returns>New InjectableHandler</returns>
        internal InjectableHandler CreateAndAdd(MonoBehaviourInjectable injectable, string instanceName)
        {
            InjectableHandler newInjectableHandler = new InjectableHandler(injectable, instanceName);
            injectables.Add(newInjectableHandler);
            return newInjectableHandler;
        }

        /// <summary>
        /// Retrieves an InjectableHandler from a MonoBehaviourInjectable
        /// </summary>
        /// <param name="injectable">Injectable from which we look for InjectableHandler</param>
        /// <returns>InjectableHandler found</returns>
        /// <exception cref="InjectablesContainerException">Thrown if not found</exception>
        private InjectableHandler GetInjectableHandler(MonoBehaviourInjectable injectable)
        {
            InjectableHandler injectableHandler = injectables.SingleOrDefault(x => x.Instance.GetInstanceID() == injectable.GetInstanceID());

            if (injectableHandler == null)
            {
                throw new InjectablesContainerException($"no instance of MonoBehaviourInjectable with name {injectable.name} and type " +
                                                        $"{injectable.GetType().Name} was found");
            }

            return injectableHandler;
        }

        /// <summary>
        /// Return InjectableHandler which has InstanceName equals to instanceName and Instance type equals typeOfInstanceToFind
        /// </summary>
        /// <param name="instanceName">Name searched</param>
        /// <param name="typeOfInstanceToFind">Type of instance searched</param>
        /// <returns>InjectableHandler searched</returns>
        internal InjectableHandler GetInjectable(string instanceName, Type typeOfInstanceToFind)
        {
            return injectables.SingleOrDefault(x => x.InstanceName == instanceName && x.Instance.GetType() == typeOfInstanceToFind);
        }
        #endregion Managing InjectableHandler 

        #region Field injection/reading and Get Component
        /// <summary>
        /// Returns field content into a non component field with T Type in a MonoBehaviourInjectable
        /// <para/>It can only be used on field decorated with NixInjectFromContainerAttribute or UnityEngine.SerializeField
        /// </summary>
        /// <typeparam name="T">Targeted field type</typeparam>
        /// <param name="injectable">Targeted injectable</param>
        internal T ReadField<T>(MonoBehaviourInjectable injectable)
        {
            InjectableHandler injectableHandler = GetInjectableHandler(injectable);

            SimpleFieldInfo fieldHandler = injectableHandler.FieldHandler.GetFieldInfoHandler(typeof(T));

            return (T)fieldHandler.FieldInfo.GetValue(injectableHandler.Instance);
        }

        /// <summary>
        /// Returns field content into a non component field with T Type and with the fieldName passed as a parameter in a MonoBehaviourInjectable
        /// <para/>It can only be used on field decorated with NixInjectFromContainerAttribute or UnityEngine.SerializeField
        /// </summary>
        /// <typeparam name="T">Targeted field type</typeparam>
        /// <param name="fieldName">Name of the field</param>
        /// <param name="injectable">Targeted injectable</param>
        internal T ReadField<T>(string fieldName, MonoBehaviourInjectable injectable)
        {
            InjectableHandler injectableHandler = GetInjectableHandler(injectable);

            SimpleFieldInfo fieldHandler = injectableHandler.FieldHandler.GetFieldInfoHandler(typeof(T), fieldName);

            return (T)fieldHandler.FieldInfo.GetValue(injectableHandler.Instance);
        }

        /// <summary>
        /// Inject value into field with T Type in a MonoBehaviourInjectable
        /// <para/>It can only be used on field decorated with NixInjectFromContainerAttribute or UnityEngine.SerializeField
        /// </summary>
        /// <typeparam name="T">Targeted field type</typeparam>
        /// <param name="valueToInject">Value to inject into field</param>
        /// <param name="injectable">Targeted injectable</param>
        internal void InjectField<T>(T valueToInject, MonoBehaviourInjectable injectable)
        {
            InjectableHandler injectableHandler = GetInjectableHandler(injectable);

            SimpleFieldInfo fieldHandler = injectableHandler.FieldHandler.GetFieldInfoHandler(typeof(T));

            fieldHandler.FieldInfo.SetValue(injectableHandler.Instance, valueToInject);
        }

        /// <summary>
        /// Inject value into field with T Type and with the fieldName passed as a parameter in a MonoBehaviourInjectable
        /// <para/>It can only be used on field decorated with NixInjectFromContainerAttribute or UnityEngine.SerializeField
        /// </summary>
        /// <typeparam name="T">Targeted field type</typeparam>
        /// <param name="fieldName">Name of the field</param>
        /// <param name="valueToInject">Value to inject into field</param>
        /// <param name="injectable">Targeted injectable</param>
        internal void InjectField<T>(string fieldName, T valueToInject, MonoBehaviourInjectable injectable)
        {
            InjectableHandler injectableHandler = GetInjectableHandler(injectable);

            SimpleFieldInfo fieldHandler = injectableHandler.FieldHandler.GetFieldInfoHandler(typeof(T), fieldName);

            fieldHandler.FieldInfo.SetValue(injectableHandler.Instance, valueToInject);
        }

        /// <summary>
        /// Return the only component with Component type T field from a MonoBehaviourInjectable
        /// <para/>If multiple type T fields are found, you can use GetComponent&lt;T&gt;(string fieldName) 
        /// <para/>It can only be used on field decorated with attribute derived from NixInjectSingleComponentBaseAttribute
        /// </summary>
        /// <typeparam name="T">Type of Component searched</typeparam>
        /// <param name="injectable">Targeted injectable</param>
        /// <returns>Single component which corresponds to the type T field</returns>
        internal T GetComponent<T>(MonoBehaviourInjectable injectable)
            where T : Component
        {
            InjectableHandler injectableHandler = GetInjectableHandler(injectable);

            ComponentWithFieldInfo fieldHandler = injectableHandler.ComponentRelationHandler.GetFieldInfoHandler(typeof(T));

            return (T)fieldHandler.Component;
        }

        /// <summary>
        /// Return the only component with Component type T field and with fieldName passed as parameter from a MonoBehaviourInjectable
        /// <para/>It can only be used on field decorated with attribute derived from NixInjectSingleComponentBaseAttribute
        /// </summary>
        /// <typeparam name="T">Type of Component searched</typeparam>
        /// <param name="injectable">Targeted injectable</param>
        /// <param name="fieldName">Name of the component field</param>
        /// <returns>Single component which corresponds to the type T field</returns>
        internal T GetComponent<T>(string fieldName, MonoBehaviourInjectable injectable)
            where T : Component
        {
            InjectableHandler injectableHandler = GetInjectableHandler(injectable);

            ComponentWithFieldInfo fieldHandler = injectableHandler.ComponentRelationHandler.GetFieldInfoHandler(typeof(T), fieldName);

            return (T)fieldHandler.Component;
        }
        #endregion Field injection/reading and Get Component

        #region EnumerableComponent
        /// <summary>
        /// Init an enumerable component field and fill it with "nbAdded" components instantiated that match type of T
        /// (apply on all enumerables of same EnumerableType and same GameObjectLevel)
        /// <para/>Throw exception if already initiliazed once
        /// <para/>It can only be used on field decorated with attribute derived from NixInjectMultiComponentsBaseAttribute
        /// </summary>
        /// <typeparam name="T">Enumerable type searched inherited from Component</typeparam>
        /// <param name="gameObjectLevel">Precise method to use to target list (children, parents or current component level list)</param>
        /// <param name="nbAdded">Number of element of type T to add into the enumerable component fields</param>
        /// <param name="injectable">MonoBehaviourInjectable to find in the container</param>
        /// <returns>New component instantiated which corresponds to type T field</returns>
        internal IEnumerable<T> InitEnumerableComponents<T>(GameObjectLevel gameObjectLevel, int nbAdded, MonoBehaviourInjectable injectable)
            where T : Component
        {
            InjectableHandler injectableHandler = GetInjectableHandler(injectable);
            return injectableHandler.EnumerableComponentRelationHandler.BuildManyEnumerableComponents<T>(gameObjectLevel, nbAdded, injectable);
        }

        /// <summary>
        /// Init an enumerable component field and fill it with as many instances of component (instantiated in this method) as typesDerived (apply on all enumerable of same EnumerableType and same GameObjectLevel)
        /// <para/>Throw exception if already initiliazed once
        /// <para/>It can only be used on field decorated with attribute derived from NixInjectMultiComponentsBaseAttribute
        /// </summary>
        /// <typeparam name="T">Enumerable type searched inherited from Component</typeparam>
        /// <param name="gameObjectLevel">Precise method to use to target list (children, parents or current component level list)</param>
        /// <param name="injectable">MonoBehaviourInjectable to find in the container</param>
        /// <param name="typesDerived">All typesDerived that must be equals or inherited from T</param>
        /// <returns>New component instantiated which corresponds to type T field</returns>
        internal IEnumerable<T> InitEnumerableComponentsWithType<T>(GameObjectLevel gameObjectLevel, MonoBehaviourInjectable injectable, params Type[] typesDerived)
            where T : Component
        {
            if (typesDerived.Length == 0)
                throw new InjectablesContainerException("no typeDerived was passed as parameter");

            InjectableHandler injectableHandler = GetInjectableHandler(injectable);
            
            return injectableHandler.EnumerableComponentRelationHandler.BuildEnumerableComponentsWithTypes<T>(gameObjectLevel, injectable, typesDerived);
        }

        /// <summary>
        /// Get all values contained in an enumerable component field which match enumerable generic type, if only one is found, it returned it, if many found, use GetEnumerableComponents(fieldName)
        /// <para/> It can only be used on field decorated with attribute derived from NixInjectMultiComponentsBaseAttribute
        /// </summary>
        /// <typeparam name="T">Generic type of enumerable</typeparam>
        /// <param name="targetedInjectable">Targeted injectable</param>
        /// <returns>Enumerable values of the enumerable component field</returns>
        internal IEnumerable<T> GetEnumerableComponents<T>(MonoBehaviourInjectable targetedInjectable)
            where T : Component
        {
            InjectableHandler injectableHandler = GetInjectableHandler(targetedInjectable);
            return injectableHandler.EnumerableComponentRelationHandler.GetEnumerableComponents<T>(targetedInjectable);
        }

        /// <summary>
        /// Get all values contained in an enumerable component field which match enumerable generic type, if only one is found, it returned it, otherwise throw an exception
        /// <para/> It can only be used on field decorated with attribute derived from NixInjectMultiComponentsBaseAttribute
        /// </summary>
        /// <typeparam name="T">Generic type of enumerable</typeparam>
        /// <param name="fieldName">Name of the fields targeted</param>
        /// <param name="injectable">Targeted injectable</param>
        /// <returns>Enumerable values of the enumerable component field</returns>
        internal IEnumerable<T> GetEnumerableComponents<T>(string fieldName, MonoBehaviourInjectable injectable)
            where T : Component
        {
            InjectableHandler injectableHandler = GetInjectableHandler(injectable);
            return injectableHandler.EnumerableComponentRelationHandler.GetEnumerableComponents<T>(fieldName, injectable);
        }
        #endregion EnumerableComponent

        #region RootComponent
        /// <summary>
        /// Add parent root relation if not already exists, if it exists it adds component on this parent if not already added on it
        /// and refresh link between children and parent gameObjects
        /// </summary>
        /// <param name="componentToAdd">Component to link to new or updated parent</param>
        /// <param name="parentName">Name of the parent relation root</param>
        public void AddOrUpdateRootRelation(Component componentToAdd, string parentName)
        {
            RootRelation parentRelation = rootRelationHandler.GetParentRelation(parentName);

            if (parentRelation == null)
                rootRelationHandler.AddParentWithoutChildren(parentName, componentToAdd);
            else
                rootRelationHandler.AddComponentIntoParentRelation(parentRelation, componentToAdd);
        }

        /// <summary>
        /// Add child to a parent root relation if not already exists, if it exists it adds component on this child if not already added on it
        /// and refresh link between children and parent gameObjects
        /// </summary>
        /// <param name="componentToAdd">Component to link to new or updated parent</param>
        /// <param name="parentName">Name of the parent in relation root</param>
        /// <param name="childName">Name of the child in relation root</param>
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
                rootRelationHandler.AddParentWithChildren(parentName, childName, componentToAdd);
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