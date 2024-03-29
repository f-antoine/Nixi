﻿using Nixi.Containers;
using Nixi.Injections.Abstractions;
using Nixi.Injections.ComponentFields.MultiComponents.Abstractions;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Nixi.Injections.Injectors
{
    /// <summary>
    /// Default way to handle all injections of fields decorated with Nixi attributes from a class derived from MonoBehaviourInjectable 
    /// during play mode scene
    /// <list type="bullet">
    ///     <item>
    ///         <term>Component fields</term>
    ///         <description>All fields decorated with attributes derived from NixInjectComponentBaseAttribute will be populated with Unity
    ///         dependency injection</description>
    ///     </item>
    ///     <item>
    ///         <term>Non-Component fields</term>
    ///         <description>All fields decorated with attributes derived from NixInjectBaseAttribute will be populated with NixiContainer</description>
    ///     </item>
    /// </list>
    /// </summary>
    public sealed class NixInjector : NixInjectorBase
    {
        /// <summary>
        /// Default way to handle all injections of fields decorated with Nixi attributes from a class derived from MonoBehaviourInjectable 
        /// during play mode scene
        /// <list type="bullet">
        ///     <item>
        ///         <term>Component fields</term>
        ///         <description>All fields decorated with attributes derived from NixInjectComponentBaseAttribute will be populated with Unity
        ///         dependency injection</description>
        ///     </item>
        ///     <item>
        ///         <term>Non-Component fields</term>
        ///         <description>All fields decorated with attributes derived from NixInjectBaseAttribute will be populated with NixiContainer</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="mainInjectable">Instance of the class derived from MonoBehaviourInjectable on which all injections will be triggered</param>
        /// <param name="nixInjectOptions">Options available to parameterized the injections, if null this is the default behavior</param>
        public NixInjector(MonoBehaviourInjectable mainInjectable, NixInjectOptions nixInjectOptions = null)
            : base(mainInjectable, nixInjectOptions)
        {
        }

        /// <summary>
        /// Inject all the fields decorated by attributes derived from NixInjectBaseAttribute or NixInjectComponentBaseAttribute in mainInjectable
        /// </summary>
        /// <exception cref="NixInjectorException">Thrown if this method has already been called</exception>
        protected override void InjectAll()
        {
            IEnumerable<FieldInfo> fields = GetAllFields(mainInjectable.GetType());

            try
            {
                InjectFields(fields.Where(NixiFieldPredicate));
                InjectComponentFields(fields.Where(NixiComponentFieldPredicate));
            }
            catch (NixiAttributeException exception)
            {
                throw new NixInjectorException(exception.Message, mainInjectable);
            }
        }

        #region Non-Component Injections
        /// <summary>
        /// Populate all fields decorated with attributes derived from NixInjectBaseAttribute (non-component fields)
        /// </summary>
        /// <param name="nonComponentFields">Non-Component fields</param>
        private void InjectFields(IEnumerable<FieldInfo> nonComponentFields)
        {
            foreach (FieldInfo nonComponentField in nonComponentFields)
            {
                NixInjectBaseAttribute injectAttribute = nonComponentField.GetCustomAttribute<NixInjectBaseAttribute>();

                if (injectAttribute is NixInjectFromContainerAttribute)
                {
                    InjectField(nonComponentField);
                }
            }
        }

        /// <summary>
        /// Populates a Non-Component field in the mainInjectable with the mapping resolved from the NixiContainer
        /// </summary>
        /// <param name="nonComponentField">Non-Component field</param>
        private void InjectField(FieldInfo nonComponentField)
        {
            NixInjectBaseAttribute injectAttribute = nonComponentField.GetCustomAttribute<NixInjectBaseAttribute>();
            injectAttribute.CheckIsValidAndBuildDataFromField(nonComponentField);

            object resolved = NixiContainer.ResolveMap(nonComponentField.FieldType);
            nonComponentField.SetValue(mainInjectable, resolved);
        }
        #endregion Non-Component Injections

        #region Component Injections
        /// <summary>
        /// Populate all fields decorated with attributes derived from NixInjectComponentBaseAttribute (Component fields)
        /// </summary>
        /// <param name="componentFields">Component fields</param>
        private void InjectComponentFields(IEnumerable<FieldInfo> componentFields)
        {
            foreach (FieldInfo componentField in componentFields)
            {
                NixInjectComponentBaseAttribute injectAttribute = componentField.GetCustomAttribute<NixInjectComponentBaseAttribute>();
                injectAttribute.CheckIsValidAndBuildDataFromField(componentField);

                if (injectAttribute is NixInjectMultiComponentsBaseAttribute)
                {
                    InjectEnumerableComponentField(componentField, injectAttribute);
                }
                else
                {
                    InjectComponentField(componentField, injectAttribute);
                }
            }
        }

        /// <summary>
        /// Populates a multiple (enumerable) components field in the mainInjectable using the Unity dependency injection 
        /// method defined in attribute derived from NixInjectComponentBaseAttribute (this can target multiple components that implement an interface type)
        /// </summary>
        /// <param name="componentField">Component or interface field</param>
        /// <param name="componentsAttribute">Nixi component (or interface) list attribute which decorate componentField</param>
        private void InjectEnumerableComponentField(FieldInfo componentField, NixInjectComponentBaseAttribute componentsAttribute)
        {
            object componentResult = componentsAttribute.GetComponentResult(mainInjectable, componentField);
            componentField.SetValue(mainInjectable, componentResult, BindingFlags.SetProperty, new EnumerableComponentBinder(), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Populates a component or interface field in the mainInjectable using the Unity dependency injection method defined in 
        /// attribute derived from NixInjectComponentBaseAttribute
        /// </summary>
        /// <param name="componentField">Component or interface field</param>
        /// <param name="injectAttribute">Nixi component (or interface) attribute which decorate componentField</param>
        private void InjectComponentField(FieldInfo componentField, NixInjectComponentBaseAttribute injectAttribute)
        {
            object componentResult = injectAttribute.GetComponentResult(mainInjectable, componentField);
            componentField.SetValue(mainInjectable, componentResult);
        }
        #endregion Component Injections
    }   
}