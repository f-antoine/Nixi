using Nixi.Injections;
using Nixi.Injections.Attributes;
using Nixi.Injections.Injecters;
using NixiTestTools.TestInjecterElements.Relations.Abstractions;
using NixiTestTools.TestInjecterElements.Relations.Components;
using NixiTestTools.TestInjecterElements.Relations.EnumerableComponents;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace NixiTestTools.TestInjecterElements
{
    /// <summary>
    /// Contain all fields of an instance of a MonoBehaviourInjectable fully instantiated for TestInjecter
    /// </summary>
    internal sealed class InjectableHandler
    {
        /// <summary>
        /// Instance on which all the fields are fields and usable from componentWithFieldInstantiated and nonComponentFields
        /// </summary>
        internal MonoBehaviourInjectable Instance { get; private set; }

        /// <summary>
        /// Name of the instance, can help for specials operation like root GameObjects (NixInjectComponentFromMethodRootAttribute)
        /// </summary>
        internal string InstanceName { get; private set; }

        /// <summary>
        /// Contains all non-component fields
        /// </summary>
        internal FieldRelationHandler FieldHandler { get; private set; } = new FieldRelationHandler();

        /// <summary>
        /// Contains all child links between current component (parentLevel) and the next level
        /// </summary>
        internal ComponentRelationHandler ComponentRelationHandler { get; private set; } = new ComponentRelationHandler();

        /// <summary>
        /// Contains all child links between current enumerable component (parentLevel) and the next level
        /// </summary>
        internal EnumerableComponentRelationHandler EnumerableComponentRelationHandler { get; private set; } = new EnumerableComponentRelationHandler();

        /// <summary>
        /// Contain all fields of an instance of a MonoBehaviourInjectable fully instantiated for TestInjecter
        /// </summary>
        public InjectableHandler(MonoBehaviourInjectable instance, string instanceName)
        {
            InstanceName = instanceName;
            Instance = instance;
        }

        /// <summary>
        /// Add a non component field info to the FieldHandler
        /// </summary>
        /// <param name="nonComponentField">Non component field</param>
        internal void AddField(FieldInfo nonComponentField)
        {
            FieldHandler.AddField(new SimpleFieldInfo { FieldInfo = nonComponentField });
        }

        /// <summary>
        /// Add and link a ComponentWithFieldInfo to the ComponentRelationHandler
        /// </summary>
        /// <param name="componentWithFieldInfo">Component with his fieldInfo</param>
        internal void AddComponentField(ComponentWithFieldInfo componentWithFieldInfo)
        {
            ComponentRelationHandler.AddFieldAndLink(componentWithFieldInfo);
        }

        /// <summary>
        /// Add and link a ComponentListWithFieldInfo to the EnumerableComponentRelationHandler (it initialize the content of the list fieldInfo as empty)
        /// </summary>
        /// <param name="componentListWithFieldInfo">Component list with his fieldInfo</param>
        public void AddEnumerableComponentField(ComponentListWithFieldInfo componentListWithFieldInfo)
        {
            InitEnumerableComponentField(componentListWithFieldInfo.FieldInfo, componentListWithFieldInfo.EnumerableType);
            EnumerableComponentRelationHandler.AddFieldAndLink(componentListWithFieldInfo);
        }

        /// <summary>
        /// Initialize the content of the list fieldInfo as empty
        /// </summary>
        /// <param name="enumerableFieldInfo">FieldInfo of the enumerable</param>
        private void InitEnumerableComponentField(FieldInfo enumerableFieldInfo, Type enumerableType)
        {
            object value;

            if (IsGenericList(enumerableFieldInfo.FieldType))
            {
                Type genericListType = typeof(List<>).MakeGenericType(enumerableType);
                value = Activator.CreateInstance(genericListType);
            }
            else if (enumerableFieldInfo.FieldType.IsArray)
            {
                value = Array.CreateInstance(enumerableType, 0);
            }
            else if (IsEnumerable(enumerableFieldInfo.FieldType))
            {
                value = Enumerable.Empty<Component>();
            }
            else
            {
                throw new NotImplementedException($"Cannot inject field with name {enumerableFieldInfo.Name}, type {enumerableFieldInfo.FieldType.Name} on an enumerable field with enumerable type {enumerableType.Name}");
            }

            enumerableFieldInfo.SetValue(Instance, value, BindingFlags.InvokeMethod, new EnumerableComponentBinder(), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Check if type is a list
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <returns>True if this is a list</returns>
        private static bool IsGenericList(Type type)
        {
            return (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>)));
        }

        /// <summary>
        /// Check if type is an enumerable
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <returns>True if this is an enumerable</returns>
        private static bool IsEnumerable(Type type)
        {
            return typeof(System.Collections.IEnumerable).IsAssignableFrom(type);
        }

        #region Component building
        /// <summary>
        /// Build a component of type contained in componentField and link it to the componentField with the new instance
        /// </summary>
        /// <param name="componentField">Component field</param>
        /// <returns>Component instantiated</returns>
        internal Component BuildAndInjectComponent(FieldInfo componentField, string componentName)
        {
            Component componentAdded;

            if (componentField.FieldType.IsAssignableFrom(typeof(Transform)))
            {
                // Build simple game object and get his transform (cannot AddComponent<Transform> this is a non sense for a gameObject which automatically add it)
                componentAdded = new GameObject(componentName).transform;
            }
            else
            {
                // Standard GameObject build
                componentAdded = new GameObject(componentName).AddComponent(componentField.FieldType);
            }

            FillFieldWithComponentAndStore(componentField, componentAdded);
            return componentAdded;
        }

        /// <summary>
        /// Fill the Component field with root MonoBehaviourInjectable found and store it in new InjectableHandler
        /// </summary>
        /// <param name="componentField">Component field</param>
        /// <param name="component">Component instance used to fill componentField</param>
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
        /// If a component with same type is at the top level (injectable level), just link the same component in the field,
        /// <para/>If this is a transform, it only link the transform directly from the injectable.gameObject, because it already has one
        /// <para/>Lastly, it means this is a new type to add on injectable.gameObject, it's linked to componentField and return this as NeededToBeInjected
        /// </summary>
        /// <param name="componentField">Component Field</param>
        /// <returns>State returned by InjectableHandler when linking or creating a new Component</returns>
        public InjectableComponentState InjectOrBuildComponentAtTopComponentLevel(FieldInfo componentField)
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
                    Component newComponent = Instance.gameObject.AddComponent(componentField.FieldType);
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
                throw new StackOverflowException($"Infinite recursion detected on the field with name {componentField.Name}" +
                    $" and with type {componentField.FieldType} which has a type identical or inherited from {injectableInstanceType.Name} type" +
                    $" which has instance name {InstanceName}");
            }
        }
    }
}