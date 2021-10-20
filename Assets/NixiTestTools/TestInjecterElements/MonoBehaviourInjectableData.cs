using Nixi.Injections;
using Nixi.Injections.Injecters;
using NixiTestTools.TestInjecterElements.Relations.Abstractions;
using NixiTestTools.TestInjecterElements.Relations.Components;
using NixiTestTools.TestInjecterElements.Relations.EnumerableComponents;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace NixiTestTools.TestInjecterElements
{
    /// <summary>
    /// Contain all fields of an instance of a MonoBehaviourInjectable fully instantiated for TestInjecter
    /// </summary>
    internal sealed class MonoBehaviourInjectableData
    {
        // TODO : Se demander si utile ? Si oui commenter
        internal MonoBehaviourInjectableData ParentInstanceData { get; private set; }

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
        public MonoBehaviourInjectableData(MonoBehaviourInjectable instance, string instanceName, MonoBehaviourInjectableData parentInstanceData)
        {
            InstanceName = instanceName;
            Instance = instance;
            ParentInstanceData = parentInstanceData;
        }

        /// <summary>
        /// Add a non component field info to the FieldHandler
        /// </summary>
        /// <param name="nonComponentField">Non component field</param>
        public void AddField(FieldInfo nonComponentField)
        {
            FieldHandler.AddField(new SimpleFieldInfo { FieldInfo = nonComponentField } );
        }

        /// <summary>
        /// Add and link a ComponentWithFieldInfo to the ComponentRelationHandler
        /// </summary>
        /// <param name="componentWithFieldInfo">Component with his fieldInfo</param>
        public void AddComponentField(ComponentWithFieldInfo componentWithFieldInfo)
        {
            ComponentRelationHandler.AddFieldAndLink(componentWithFieldInfo);
        }

        /// <summary>
        /// Add and link a ComponentListWithFieldInfo to the EnumerableComponentRelationHandler (it initialize the content of the list fieldInfo as empty)
        /// </summary>
        /// <param name="componentListWithFieldInfo">Component list with his fieldInfo</param>
        public void AddEnumerableComponentField(ComponentListWithFieldInfo componentListWithFieldInfo)
        {
            InitEnumerableComponentField(componentListWithFieldInfo.FieldInfo);
            EnumerableComponentRelationHandler.AddFieldAndLink(componentListWithFieldInfo);
        }

        /// <summary>
        /// Initialize the content of the list fieldInfo as empty
        /// </summary>
        /// <param name="enumerableFieldInfo">FieldInfo of the list</param>
        private void InitEnumerableComponentField(FieldInfo enumerableFieldInfo)
        {
            enumerableFieldInfo.SetValue(Instance, Enumerable.Empty<Component>(), BindingFlags.InvokeMethod, new EnumerableComponentBinder(), CultureInfo.InvariantCulture);
        }
    }
}