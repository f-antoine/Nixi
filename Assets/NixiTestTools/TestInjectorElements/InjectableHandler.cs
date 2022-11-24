using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using Nixi.Injections.Injectors;
using NixiTestTools.TestInjectorElements.Relations.Components;
using NixiTestTools.TestInjectorElements.Relations.EnumerableComponents;
using NixiTestTools.TestInjectorElements.Relations.Fields;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;

namespace NixiTestTools.TestInjectorElements
{
    /// <summary>
    /// Contain all fields of an instance of a MonoBehaviourInjectable fully instantiated for TestInjector and simulate Unity
    /// </summary>
    internal sealed class InjectableHandler
    {
        /// <summary>
        /// Instance on which all the fields are injected and usable from all RelationHandlers
        /// </summary>
        internal MonoBehaviourInjectable Instance { get; private set; }

        /// <summary>
        /// Name of the instance, can help for specials operation like root GameObjects (NixInjectRootComponentAttribute)
        /// </summary>
        internal string InstanceName { get; private set; }

        /// <summary>
        /// Contains/Handles all non-component fields
        /// </summary>
        internal FieldRelationHandler FieldHandler { get; private set; } = new FieldRelationHandler();

        /// <summary>
        /// Contains/Handles all relations between components (parent/children links) and keep an access to all their FieldInfo (wrapped into ComponentWithFieldInfo)
        /// to handle all injections of theses relations
        /// </summary>
        internal ComponentRelationHandler ComponentRelationHandler { get; private set; } = new ComponentRelationHandler();

        /// <summary>
        /// Contains all relations between List/IEnumerable/array of components (parent/children links) and keep an access to all their FieldInfo (wrapped into ComponentListWithFieldInfo)
        /// to handle all injections of theses relations
        /// </summary>
        internal EnumerableComponentRelationHandler EnumerableComponentRelationHandler { get; private set; } = new EnumerableComponentRelationHandler();

        /// <summary>
        /// Contain all fields of an instance of a MonoBehaviourInjectable fully instantiated for TestInjector and simulate Unity
        /// </summary>
        public InjectableHandler(MonoBehaviourInjectable instance, string instanceName)
        {
            InstanceName = instanceName;
            Instance = instance;
        }

        /// <summary>
        /// Add nonComponentField into FieldHandler
        /// </summary>
        /// <param name="nonComponentField">Non component field</param>
        internal void AddField(FieldInfo nonComponentField)
        {
            FieldHandler.AddField(new SimpleFieldInfo { FieldInfo = nonComponentField });
        }

        /// <summary>
        /// Add and link a ComponentWithFieldInfo into the ComponentRelationHandler
        /// </summary>
        /// <param name="componentWithFieldInfo">Component with its fieldInfo</param>
        internal void AddComponentField(ComponentWithFieldInfo componentWithFieldInfo)
        {
            if (componentWithFieldInfo.FieldInfo.GetCustomAttribute<NixInjectComponentAttribute>() != null)
            {
                ComponentRelationHandler.AddRelation(componentWithFieldInfo.Component);
            }   

            ComponentRelationHandler.AddField(componentWithFieldInfo);
        }

        /// <summary>
        /// Add and link a ComponentListWithFieldInfo into the EnumerableComponentRelationHandler (it initialize the content of the list fieldInfo as empty)
        /// </summary>
        /// <param name="componentListWithFieldInfo">Component list with its fieldInfo</param>
        public void AddEnumerableComponentField(ComponentListWithFieldInfo componentListWithFieldInfo)
        {
            InitEnumerableComponentField(componentListWithFieldInfo.FieldInfo, componentListWithFieldInfo.EnumerableType);
            EnumerableComponentRelationHandler.AddRelation(componentListWithFieldInfo.Components);
            EnumerableComponentRelationHandler.AddField(componentListWithFieldInfo);
        }

        /// <summary>
        /// Initialize the content of enumerable component fieldInfo as empty
        /// </summary>
        /// <param name="enumerableFieldInfo">FieldInfo of the enumerable</param>
        /// <param name="enumerableType">Type of enumerable associated to the FieldInfo of the enumerable</param>
        private void InitEnumerableComponentField(FieldInfo enumerableFieldInfo, Type enumerableType)
        {
            object value = null;

            if (enumerableFieldInfo.FieldType.IsArray)
            {
                value = Array.CreateInstance(enumerableType, 0);
            }
            else if (enumerableFieldInfo.FieldType.IsGenericType)
            {
                Type genericTypeDefinition = enumerableFieldInfo.FieldType.GetGenericTypeDefinition();

                // Build List for Enumerable or Enumerable
                if (genericTypeDefinition == typeof(List<>)
                    || genericTypeDefinition == typeof(IEnumerable<>))
                {
                    Type genericListType = typeof(List<>).MakeGenericType(enumerableType);
                    value = Activator.CreateInstance(genericListType);
                }
            }

            if (value == null)
            {
                throw new NotImplementedException($"Cannot inject field with name {enumerableFieldInfo.Name}, type " +
                                                  $"{enumerableFieldInfo.FieldType.Name} on an enumerable field with enumerable type " +
                                                  $"{enumerableType.Name} in InjectableHandler");
            }

            enumerableFieldInfo.SetValue(Instance, value, BindingFlags.InvokeMethod, new EnumerableComponentBinder(), CultureInfo.InvariantCulture);
        }

        #region Component building
        /// <summary>
        /// Build a component of type componentField.FieldType and set the value of the componentField with this new instance
        /// </summary>
        /// <param name="componentField">Component field</param>
        /// <param name="gameObjectName">Name of the GameObject</param>
        /// <param name="forcedType">If filled it will use an inherited type forced by AbstractComponentMappingContainer</param>
        /// <returns>Component instantiated</returns>
        internal Component BuildComponent(FieldInfo componentField, string gameObjectName, Type forcedType = null)
        {
            Component componentAdded;

            if (componentField.FieldType.IsAssignableFrom(typeof(Transform)))
            {
                // Build simple game object and get its transform (cannot AddComponent<Transform> this is a non sense for a gameObject which automatically add it)
                componentAdded = new GameObject(gameObjectName).transform;
            }
            else
            {
                // Standard GameObject build
                componentAdded = new GameObject(gameObjectName).AddComponent(forcedType ?? componentField.FieldType);
            }

            FillFieldWithComponentAndStore(componentField, componentAdded);
            return componentAdded;
        }

        /// <summary>
        /// Populates componentField with a component
        /// </summary>
        /// <param name="componentField">Component field</param>
        /// <param name="component">Component instance used to populate componentField</param>
        internal void FillFieldWithComponentAndStore(FieldInfo componentField, Component component)
        {
            componentField.SetValue(Instance, component);
            AddComponentField(new ComponentWithFieldInfo
            {
                Component = component,
                FieldInfo = componentField
            });
        }

        /// <summary>
        /// Method used for NixInjectComponent decorator (it means at top level/injectable level), they are several cases :
        /// <para/>If ComponentField.FieldType already exists at the injectable level, the same component is used to set the value of componentField
        /// <para/>Else if this is transform type it sets the value of componentField with Instance.transform (injectable.transform)
        /// <para/>The last case means it is a new type to add on injectable.gameObject, so a new component is added onto injectable.gameObject 
        /// and componentField value is setted with this new component
        /// </summary>
        /// <param name="componentField">Component Field</param>
        /// <param name="forcedType">If filled it will use an inherited type from componentField.FieldType forced by AbstractComponentMappingContainer
        /// instead of componentField.FieldType</param>
        /// <returns>State returned by InjectableHandler when linking or creating a new Component</returns>
        public InjectableComponentState InjectOrBuildComponentAtTopComponentLevel(FieldInfo componentField, Type forcedType = null)
        {
            // Case where we have same level instance
            Component sameLevelComponent = ComponentRelationHandler.GetSameLevelComponent(componentField.FieldType);

            if (sameLevelComponent != null)
            {
                // Found, we just link
                FillFieldWithComponentAndStore(componentField, sameLevelComponent);
                return InjectableComponentState.NoNeedToInject;
            }
            else
            {
                if (componentField.FieldType.IsAssignableFrom(typeof(Transform)))
                {
                    // Get transform of gameObject that already exists (cannot add it)
                    FillFieldWithComponentAndStore(componentField, Instance.transform);
                    return InjectableComponentState.NoNeedToInject;
                }
                else
                {
                    // AddComponent, the new component need to be injected
                    Component newComponent = Instance.gameObject.AddComponent(forcedType ?? componentField.FieldType);
                    FillFieldWithComponentAndStore(componentField, newComponent);
                    return InjectableComponentState.NeedToBeInjectedIfInjectable;
                }
            }
        }
        #endregion Component building

        /// <summary>
        /// Checks if injecting a MonoBehaviourInjectable into current instance does not cause an infinite injection loop of itself
        /// </summary>
        /// <param name="componentField">Component field to check</param>
        public void CheckInfiniteRecursion(FieldInfo componentField)
        {
            Type componentType = componentField.FieldType.DeclaringType ?? componentField.FieldType;
            Type injectableInstanceType = Instance.GetType();

            if (componentType.IsAssignableFrom(injectableInstanceType))
            {
                throw new StackOverflowException($"Infinite recursion detected on the field with name {componentField.Name} " +
                                                 $"and with type {componentField.FieldType} which has a type identical or inherited from " +
                                                 $"{injectableInstanceType.Name} type which has instance name {InstanceName}");
            }
        }
    }
}