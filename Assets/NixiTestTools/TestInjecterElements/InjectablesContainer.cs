using Nixi.Injections;
using Nixi.Injections.Attributes;
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
        private List<InjectableHandler> injectables = new List<InjectableHandler>();

        /// <summary>
        /// Handle all root components with their children relation at one level below (this is in InjectablesContainer because it concerns all field of all injectables)
        /// </summary>
        private RootRelationHandler rootRelationHandler = new RootRelationHandler();

        #region Managing MonoBehaviourInjectableData
        /// <summary>
        /// Create a MonoBehaviourInjectableData and add it to the list of injectables
        /// </summary>
        /// <param name="instanceName">Name of the instance</param>
        /// <param name="monoBehaviourInjectable">MonoBehaviourInjectable to load into new MonoBehaviourInjectableData</param>
        /// <returns>New MonoBehaviourInjectableData</returns>
        internal InjectableHandler CreateAndAdd(MonoBehaviourInjectable monoBehaviourInjectable, string instanceName)
        {
            InjectableHandler newInjectableHandler = new InjectableHandler(monoBehaviourInjectable, instanceName);
            injectables.Add(newInjectableHandler);
            return newInjectableHandler;
        }

        /// <summary>
        /// Find data of a MonoBehaviourInjectable from UnityEngine.Object.GetInstanceID()
        /// </summary>
        /// <param name="injectable">Injectable from which we look for data</param>
        /// <returns>Data of the injectable</returns>
        private InjectableHandler GetInjectableHandler(MonoBehaviourInjectable injectable)
        {
            InjectableHandler injectableHandler = GetInjectable(injectable);

            if (injectableHandler == null)
                throw new InjectablesContainerException($"no instance of MonoBehaviourInjectable with name {injectable.name} was found, {injectable.GetType().Name} class does not contain the field");

            return injectableHandler;
        }

        /// <summary>
        /// Return MonoBehaviourInjectableData which has Instance.GetInstanceID() equals to injectable.GetInstanceID()
        /// </summary>
        /// <param name="injectable">MonoBehaviourInjectable searched</param>
        /// <returns>MonoBehaviourInjectableData searched</returns>
        internal InjectableHandler GetInjectable(MonoBehaviourInjectable injectable)
        {
            return injectables.SingleOrDefault(x => x.Instance.GetInstanceID() == injectable.GetInstanceID());
        }

        /// <summary>
        /// Return MonoBehaviourInjectableData which has InstanceName equals to instanceName and Instance type equals typeOfInstanceToFind
        /// </summary>
        /// <param name="instanceName">Name searched</param>
        /// <param name="typeOfInstanceToFind">Type of instance searched</param>
        /// <returns>MonoBehaviourInjectableData searched</returns>
        internal InjectableHandler GetInjectable(string instanceName, Type typeOfInstanceToFind)
        {
            return injectables.SingleOrDefault(x => x.InstanceName == instanceName && x.Instance.GetType() == typeOfInstanceToFind);
        }
        #endregion Managing MonoBehaviourInjectableData 

        #region Mock injection and Get Component
        /// <summary>
        /// Inject manually a mock into the Non-Component field with T Type in a MonoBehaviourInjectable
        /// </summary>
        /// <typeparam name="T">Targeted field type</typeparam>
        /// <param name="mockToInject">Mock to inject into field</param>
        /// <param name="monoBehaviourInjectableToFind">Targeted injectable</param>
        internal void InjectMockIntoInstance<T>(T mockToInject, MonoBehaviourInjectable monoBehaviourInjectableToFind)
        {
            InjectableHandler injectableHandler = GetInjectableHandler(monoBehaviourInjectableToFind);

            Type interfaceType = typeof(T);

            IEnumerable<SimpleFieldInfo> fieldSelecteds = injectableHandler.FieldHandler.Fields.Where(x => x.FieldInfo.FieldType == interfaceType);

            if (!fieldSelecteds.Any())
                throw new InjectablesContainerException($"no field with type {interfaceType.Name} was found");

            if (fieldSelecteds.Count() > 1)
                throw new InjectablesContainerException($"multiple fields with type {interfaceType.Name} were found, cannot define which one use, please use InjectMock<T>(T mockToInject, string fieldName) instead");

            fieldSelecteds.Single().FieldInfo.SetValue(injectableHandler.Instance, mockToInject);
        }

        /// <summary>
        /// Inject manually a mock into the Non-Component field with T Type and with the fieldName passed as a parameter in a MonoBehaviourInjectable
        /// </summary>
        /// <typeparam name="T">Targeted field type</typeparam>
        /// <param name="fieldName">Name of the field to mock</param>
        /// <param name="mockToInject">Mock to inject into field</param>
        /// <param name="monoBehaviourInjectableToFind">Targeted injectable</param>
        internal void InjectMockIntoInstance<T>(string fieldName, T mockToInject, MonoBehaviourInjectable monoBehaviourInjectableToFind)
        {
            InjectableHandler injectableHandler = GetInjectableHandler(monoBehaviourInjectableToFind);

            Type interfaceType = typeof(T);

            IEnumerable<SimpleFieldInfo> fieldsWithType = injectableHandler.FieldHandler.Fields.Where(x => x.FieldInfo.FieldType == interfaceType);
            if (!fieldsWithType.Any())
                throw new InjectablesContainerException($"no field with type {interfaceType.Name}");

            IEnumerable<SimpleFieldInfo> fieldsWithTypeAndName = fieldsWithType.Where(x => x.FieldInfo.Name == fieldName);
            if (!fieldsWithTypeAndName.Any())
                throw new InjectablesContainerException($"field with type {interfaceType.Name} was/were found, but none with fieldName {fieldName}");

            fieldsWithTypeAndName.Single().FieldInfo.SetValue(injectableHandler.Instance, mockToInject);
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
            InjectableHandler injectableHandler = GetInjectableHandler(monoBehaviourInjectableToFind);

            Type typeToFind = typeof(T);

            IEnumerable<ComponentWithFieldInfo> componentsWithType = injectableHandler.ComponentRelationHandler.Fields.Where(x => x.FieldInfo.FieldType == typeToFind);

            if (!componentsWithType.Any())
                throw new InjectablesContainerException($"no component with type {typeToFind.Name} was found");

            if (componentsWithType.Count() > 1)
                throw new InjectablesContainerException($"multiple components with type {typeToFind.Name} were found, cannot define which one use, please use GetComponent(fieldName)");

            return componentsWithType.Single().Component as T;
        }

        /// <summary>
        /// Return the only component with Component type T field and with fieldName passed as parameter from a MonoBehaviourInjectable
        /// <para/>If multiple type T fields are found, you must use GetComponent<T>(string fieldName) 
        /// </summary>
        /// <typeparam name="T">Type of Component searched</typeparam>
        /// <param name="monoBehaviourInjectableToFind">Targeted injectable</param>
        /// <param name="fieldName">Name of the component field</param>
        /// <returns>Single component which corresponds to the type T field</returns>
        internal T GetComponentFromInstance<T>(string fieldName, MonoBehaviourInjectable monoBehaviourInjectableToFind)
            where T : Component
        {
            InjectableHandler injectableHandler = GetInjectableHandler(monoBehaviourInjectableToFind);

            Type typeToFind = typeof(T);

            IEnumerable<ComponentWithFieldInfo> componentsWithFieldType = injectableHandler.ComponentRelationHandler.Fields.Where(x => x.FieldInfo.FieldType == typeToFind);
            if (!componentsWithFieldType.Any())
                throw new InjectablesContainerException($"no component with type {typeToFind.Name} was found");

            IEnumerable <ComponentWithFieldInfo> componentsWithFieldTypeAndFieldName = componentsWithFieldType.Where(x => x.FieldInfo.Name == fieldName);
            if (!componentsWithFieldTypeAndFieldName.Any())
                throw new InjectablesContainerException($"component with type {typeToFind.Name} was/were found, but none with field name {fieldName}");

            return componentsWithFieldTypeAndFieldName.Single().Component as T;
        }
        #endregion Mock injection and Get Component

        #region EnumerableComponent
        /// <summary>
        /// Init an enumerable component field and fill it with "nbAdded" components instantiated that match type of T (apply on all enumerable of same EnumerableType and same GameObjectLevel)
        /// <para/>Throw exception if already initiliazed once
        /// </summary>
        /// <typeparam name="T">Enumerable type searched inherited from Component)</typeparam>
        /// <param name="gameObjectLevel">Precise method to use to target list (children, parents or current component level list)</param>
        /// <param name="nbAdded">Number of element of type T to add into the enumerable component fields</param>
        /// <param name="injectable">MonoBehaviourInjectable to find in the container</param>
        /// <returns>New component instantiated which corresponds to type T field</returns>
        internal IEnumerable<T> InitEnumerableComponents<T>(GameObjectLevel gameObjectLevel, int nbAdded, MonoBehaviourInjectable injectable)
            where T : Component
        {
            IEnumerable<ComponentListWithFieldInfo> enumerableFieldsData = GetComponentListsWithCriteria<T>(injectable, gameObjectLevel);

            CheckEnumerableComponentNotAlreadyInitiated<T>(enumerableFieldsData, injectable);

            List<T> components = new List<T>();
            for (int i = 0; i < nbAdded; i++)
            {
                components.Add(BuildComponentForEnumerableComponentFields<T>(gameObjectLevel, injectable));
            }

            foreach (ComponentListWithFieldInfo enumerableFieldData in enumerableFieldsData)
            {
                enumerableFieldData.FieldInfo.SetValue(injectable, components, BindingFlags.InvokeMethod, new EnumerableComponentBinder(), CultureInfo.InvariantCulture);
            }

            return components;
        }

        /// <summary>
        /// Init an enumerable component field and fill it with as many instances of component (instantiated in this method) as typeDeriveds (apply on all enumerable of same EnumerableType and same GameObjectLevel)
        /// <para/>Throw exception if already initiliazed once
        /// </summary>
        /// <typeparam name="T">Enumerable type searched inherited from Component)</typeparam>
        /// <param name="gameObjectLevel">Precise method to use to target list (children, parents or current component level list)</param>
        /// <param name="injectable">MonoBehaviourInjectable to find in the container</param>
        /// <param name="typeDeriveds">All typeDeriveds that must be equals or inherited from T</param>
        /// <returns>New component instantiated which corresponds to type T field</returns>
        internal IEnumerable<T> InitEnumerableComponentsWithType<T>(GameObjectLevel gameObjectLevel, MonoBehaviourInjectable injectable, params Type[] typeDeriveds)
            where T : Component
        {
            if (typeDeriveds.Length == 0)
                throw new InjectablesContainerException("no typeDerived was passed as parameter");

            IEnumerable<ComponentListWithFieldInfo> enumerableFieldsData = GetComponentListsWithCriteria<T>(injectable, gameObjectLevel);

            CheckEnumerableComponentNotAlreadyInitiated<T>(enumerableFieldsData, injectable);

            List<T> components = new List<T>();

            Type baseType = typeof(T);
            foreach (Type typeDerived in typeDeriveds)
            {
                if (!baseType.IsAssignableFrom(typeDerived))
                    throw new InjectablesContainerException($"{baseType.Name} is not assignable from {typeDerived.Name}");

                Component componentBuilded = BuildComponentForEnumerableComponentFields(gameObjectLevel, injectable, typeDerived);
                components.Add(componentBuilded as T);
            }

            foreach (ComponentListWithFieldInfo enumerableFieldData in enumerableFieldsData)
            {
                enumerableFieldData.FieldInfo.SetValue(injectable, components, BindingFlags.InvokeMethod, new EnumerableComponentBinder(), CultureInfo.InvariantCulture);
            }

            return components;
        }

        /// <summary>
        /// Get all values contained in an enumerable component field which match enumerable generic type, if only one is found, it returned it, otherwise throw an exception
        /// </summary>
        /// <typeparam name="T">Generic type of enumerable</typeparam>
        /// <param name="fieldName">Name of the fields targeted</param>
        /// <param name="injectable">Targeted injectable</param>
        /// <returns>Enumerable values of the enumerable component field</returns>
        internal IEnumerable<T> GetEnumerableComponents<T>(string fieldName, MonoBehaviourInjectable injectable)
            where T : Component
        {
            InjectableHandler injectableHandler = GetInjectableHandler(injectable);

            Type typeToFind = typeof(T);

            IEnumerable<ComponentListWithFieldInfo> componentsListWithType = injectableHandler.EnumerableComponentRelationHandler.Fields.Where(x => x.EnumerableType.IsAssignableFrom(typeToFind));

            if (!componentsListWithType.Any())
                throw new InjectablesContainerException($"no EnumerableComponent with type {typeToFind.Name} was found");

            IEnumerable<ComponentListWithFieldInfo> componentsListWithTypeMethodAndName = componentsListWithType.Where(x => x.FieldInfo.Name == fieldName);

            if (!componentsListWithTypeMethodAndName.Any())
                throw new InjectablesContainerException($"EnumerableComponent(s) with type {typeToFind.Name} was/were found, but none with fieldName {fieldName}");

            // Convert to IEnumerable
            object result = componentsListWithTypeMethodAndName.Single().FieldInfo.GetValue(injectable);
            return GetEnumerableFromObject<T>(result);
        }

        /// <summary>
        /// Get all values contained in an enumerable component field which match enumerable generic type, if only one is found, it returned it, if many found, use GetEnumerableComponents(fieldName)
        /// </summary>
        /// <typeparam name="T">Generic type of enumerable</typeparam>
        /// <param name="targetedInjectable">Targeted injectable</param>
        /// <returns>Enumerable values of the enumerable component field</returns>
        internal IEnumerable<T> GetEnumerableComponents<T>(MonoBehaviourInjectable targetedInjectable)
            where T : Component
        {
            InjectableHandler injectableHandler = GetInjectableHandler(targetedInjectable);

            Type typeToFind = typeof(T);

            IEnumerable<ComponentListWithFieldInfo> componentsListWithType = injectableHandler.EnumerableComponentRelationHandler.Fields.Where(x => x.EnumerableType.IsAssignableFrom(typeToFind));
                
            if (!componentsListWithType.Any())
                throw new InjectablesContainerException($"no EnumerableComponent with type {typeToFind.Name} was found");

            if (componentsListWithType.Count() > 1)
                throw new InjectablesContainerException($"multiple EnumerableComponents with type {typeToFind.Name} were found, cannot define which one use, please use GetEnumerableComponent(fieldName)");

            object result = componentsListWithType.Single().FieldInfo.GetValue(targetedInjectable);
            return GetEnumerableFromObject<T>(result);
        }

        /// <summary>
        /// Get all ComponentListWithFieldInfo instantiated in a MonoBehaviourInjectable field which match an enumerable of type T (inherited from Component) and a GameObjectLevel
        /// </summary>
        /// <typeparam name="T">Enumerable type searched inherited from Component)</typeparam>
        /// <param name="targetedInjectable">Targeted injectable</param>
        /// <param name="gameObjectLevel">Precise method to use to target list (children, parents or current component level list)</param>
        /// <returns>List of component instantiated which corresponds to type T field</returns>
        private IEnumerable<ComponentListWithFieldInfo> GetComponentListsWithCriteria<T>(MonoBehaviourInjectable targetedInjectable, GameObjectLevel gameObjectLevel)
            where T : Component
        {
            InjectableHandler injectableHandler = GetInjectableHandler(targetedInjectable);

            Type typeToFind = typeof(T);

            IEnumerable<ComponentListWithFieldInfo> componentsListWithType = injectableHandler.EnumerableComponentRelationHandler.Fields.Where(x => x.EnumerableType.IsAssignableFrom(typeToFind));

            if (!componentsListWithType.Any())
                throw new InjectablesContainerException($"no EnumerableComponent with type {typeToFind.Name} was found");

            IEnumerable<ComponentListWithFieldInfo> componentWithCriteria = componentsListWithType.Where(x => x.EnumerableType == typeToFind && x.GameObjectLevel == gameObjectLevel);
            if (!componentWithCriteria.Any())
                throw new InjectablesContainerException($"no EnumerableComponent with type {typeToFind.Name} and GameObjectLevel {gameObjectLevel} was found");

            return componentWithCriteria;
        }

        /// <summary>
        ///  Check if a component field enumerable (or a set of component field enumerable) has already been initiliazed (FieldInfo.GetValue is not empty it throws an exception)
        /// </summary>
        /// <typeparam name="T">IEnumerable type to find for the cast into IEnumerable</typeparam>
        /// <param name="componentsWithCriteria">All ComponentListWithFieldInfo tested</param>
        /// <param name="injectable">Injectable on which we are looking for the fieldInfo value contained</param>
        private void CheckEnumerableComponentNotAlreadyInitiated<T>(IEnumerable<ComponentListWithFieldInfo> componentsWithCriteria, MonoBehaviourInjectable injectable)
            where T : Component
        {
            foreach (var element in componentsWithCriteria)
            {
                if (element.EnumerableType == typeof(T))
                {
                    object value = element.FieldInfo.GetValue(injectable);

                    IEnumerable<T> enumerableValue = GetEnumerableFromObject<T>(value);

                    if (enumerableValue.Any())
                        throw new InjectablesContainerException($"Cannot init list of type {typeof(T).Name} twice");
                }   
            }
        }

        /// <summary>
        /// Create a component of type targetType, if this is at same level of the injectable instance, this is directly added to his gameObject, if not, a new gameObject is built
        /// </summary>
        /// <typeparam name="T">Type of component wanted (inherited from Component)</typeparam>
        /// <param name="gameObjectLevel">Precise method to use to target list (children, parents or current component level list)</param>
        /// <param name="injectable">Injectable for which we are adding a component</param>
        /// <returns>New component</returns>
        private static T BuildComponentForEnumerableComponentFields<T>(GameObjectLevel gameObjectLevel, MonoBehaviourInjectable injectable)
            where T : Component
        {
            T newComponent;

            if (gameObjectLevel == GameObjectLevel.Current)
            {
                // This is the same gameObject of monoBehaviourInjectableToFind and we only AddComponent of type wanted
                newComponent = injectable.gameObject.AddComponent<T>();
            }
            else
            {
                // Other cases, we create a new GameObject and AddComponent of type wanted
                newComponent = new GameObject().AddComponent<T>();
            }

            return newComponent;
        }

        /// <summary>
        /// Create a component of type targetType, if this is at same level of the injectable instance, this is directly added to his gameObject, if not, a new gameObject is built
        /// </summary>
        /// <param name="gameObjectLevel">Precise method to use to target list (children, parents or current component level list)</param>
        /// <param name="injectable">Injectable for which we are adding a component</param>
        /// <param name="targetedType">Type of component wanted (inherited from Component)</param>
        /// <returns>New component</returns>
        private static Component BuildComponentForEnumerableComponentFields(GameObjectLevel gameObjectLevel, MonoBehaviourInjectable injectable, Type targetedType)
        {
            Component newComponent;

            if (gameObjectLevel == GameObjectLevel.Current)
            {
                // This is the same gameObject of monoBehaviourInjectableToFind and we only AddComponent of type wanted
                newComponent = injectable.gameObject.AddComponent(targetedType);
            }
            else
            {
                // Other cases, we create a new GameObject and AddComponent of type wanted
                newComponent = new GameObject().AddComponent(targetedType);
            }

            return newComponent;
        }

        /// <summary>
        /// Transpose an object into a IEnumerable with generic type T derived from Component
        /// </summary>
        /// <typeparam name="T">Enumerable generic type</typeparam>
        /// <param name="value">Object to convert</param>
        /// <returns>IEnumerable with generic type T</returns>
        private static IEnumerable<T> GetEnumerableFromObject<T>(object value)
            where T : Component
        {
            if (value.GetType().IsArray)
            {
                var valueEnumerable = value as System.Collections.IEnumerable;

                List<T> objectsToConvert = new List<T>();
                foreach (var element in valueEnumerable)
                {
                    objectsToConvert.Add(element as T);
                }

                return objectsToConvert.ToArray();
            }
            return value as IEnumerable<T>;
        }
        #endregion EnumerableComponent

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