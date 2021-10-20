using Nixi.Containers;
using Nixi.Injections.Attributes.Fields;
using Nixi.Injections.Attributes.ComponentFields.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Nixi.Injections.Injecters
{
    /// <summary>
    /// Default way to handle all injections of fields decorated with Nixi inject attributes of a class derived from MonoBehaviourInjectable during play mode
    /// <list type="bullet">
    ///     <item>
    ///         <term>Component fields</term>
    ///         <description>Marked with NixInjectComponentAttribute will be populated with Unity dependency injection</description>
    ///     </item>
    ///     <item>
    ///         <term>Non-Component fields</term>
    ///         <description>Marked with NixInjectAttribute will be populated with NixiContainer</description>
    ///     </item>
    /// </list>
    /// </summary>
    public sealed class NixInjecter : NixInjecterBase
    {
        /// <summary>
        /// Default way to handle all injections of fields decorated with Nixi attributes of a class derived from MonoBehaviourInjectable during play mode
        /// </summary>
        /// <param name="objectToLink">Instance of the class derived from MonoBehaviourInjectable on which all injections will be triggered</param>
        public NixInjecter(MonoBehaviourInjectable objectToLink)
            : base(objectToLink)
        {
        }

        /// <summary>
        /// Inject all the fields decorated by NixInjectAttribute or NixInjectComponentAttribute in objectToLink
        /// </summary>
        /// <exception cref="NixInjecterException">Thrown if this method has already been called</exception>
        protected override void InjectAll()
        {
            List<FieldInfo> fields = GetAllFields(objectToLink.GetType());

            InjectFields(fields.Where(NixiFieldPredicate));
            InjectcomponentFields(fields.Where(NixiComponentFieldPredicate));
        }

        #region Non-Component Injections
        /// <summary>
        /// Fill all fields decorated with NixInjectAttribute (Non-Component fields) and with NixInjectType set to FillWithContainer 
        /// </summary>
        /// <param name="nonComponentFields">Non-Component fields</param>
        private void InjectFields(IEnumerable<FieldInfo> nonComponentFields)
        {
            foreach (FieldInfo field in nonComponentFields)
            {
                NixInjectAttribute injectAttribute = field.GetCustomAttribute<NixInjectAttribute>();

                if (injectAttribute.NixInjectType == NixInjectType.FillWithContainer)
                    InjectField(field);
            }
        }

        /// <summary>
        /// Fill a Non-Component field in the objectToLink with the mapping resolve from the NixiContainer
        /// </summary>
        /// <param name="nonComponentField">Non-Component field</param>
        private void InjectField(FieldInfo nonComponentField)
        {
            CheckIsNotComponent(nonComponentField);

            if (!nonComponentField.FieldType.IsInterface)
                throw new NixInjecterException($"The field with the name {nonComponentField.Name} with a NixInjectAttribute must be an interface " +
                    $"because the container works with only interfaces as a key for injection, " +
                    $"if you don't want to use the container and only expose for the tests from template, " +
                    $"you can use DoesNotFillButExposeForTesting option on the attribute constructor", objectToLink);

            object resolved = NixiContainer.Resolve(nonComponentField.FieldType);
            nonComponentField.SetValue(objectToLink, resolved);
        }
        #endregion Non-Component Injections

        #region Component Injections
        /// <summary>
        /// Fill all fields decorated with NixInjectComponentAttribute (Component fields)
        /// </summary>
        /// <param name="componentFields">Component fields</param>
        private void InjectcomponentFields(IEnumerable<FieldInfo> componentFields)
        {
            foreach (FieldInfo componentField in componentFields)
            {
                InjectcomponentField(componentField);
            }
        }

        /// <summary>
        /// Fill a Component field in the objectToLink with the Unity dependency injection method which corresponds to the value of NixInjectComponentAttribute.GameObjectMethod in GameObjectMethodBindings
        /// </summary>
        /// <param name="componentField">Component field</param>
        private void InjectcomponentField(FieldInfo componentField)
        {
            CheckIsComponent(componentField);

            NixInjectComponentBaseAttribute injectAttribute = componentField.GetCustomAttribute<NixInjectComponentBaseAttribute>();
            Component componentToTranspose = injectAttribute.GetSingleComponent(objectToLink, componentField.FieldType);
            componentField.SetValue(objectToLink, componentToTranspose);
        }
        #endregion Component Injections
    }
}