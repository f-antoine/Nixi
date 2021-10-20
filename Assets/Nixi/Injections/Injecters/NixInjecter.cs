using Nixi.Containers;
using Nixi.Injections.Attributes;
using Nixi.Injections.Attributes.Abstractions;
using Nixi.Injections.Extensions;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

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

            try
            {
                InjectFields(fields.Where(NixiFieldPredicate));
                InjectComponentFields(fields.Where(NixiComponentFieldPredicate));
            }
            catch (NixiAttributeException exception)
            {
                throw new NixInjecterException(exception.Message, objectToLink);
            }
        }

        #region Non-Component Injections
        /// <summary>
        /// Fill all fields decorated with NixInjectFromContainerAttribute (non-component fields) and does not fill if decorated with NixInjectTestMockAttribute
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
        /// Fill a Non-Component field in the objectToLink with the mapping resolve from the NixiContainer
        /// </summary>
        /// <param name="nonComponentField">Non-Component field</param>
        private void InjectField(FieldInfo nonComponentField)
        {
            if (nonComponentField.IsComponent())
                throw new NixInjecterException($"Cannot register field with name {nonComponentField.Name} with a NixInjectAttribute because it is a Component field, you must use NixInjectComponentAttribute instead", objectToLink);

            if (!nonComponentField.FieldType.IsInterface)
                throw new NixInjecterException($"The field with the name {nonComponentField.Name} with a NixInjectAttribute must be an interface " +
                    $"because the container works only with interfaces as a key for injection, " +
                    $"if you don't want to use the container and only expose for the tests from template, " +
                    $"you can use NixInjectTestMockAttribute", objectToLink);

            object resolved = NixiContainer.Resolve(nonComponentField.FieldType);
            nonComponentField.SetValue(objectToLink, resolved);
        }
        #endregion Non-Component Injections

        #region Component Injections
        /// <summary>
        /// Fill all fields decorated with NixInjectComponentAttribute (Component fields)
        /// </summary>
        /// <param name="componentFields">Component fields</param>
        private void InjectComponentFields(IEnumerable<FieldInfo> componentFields)
        {
            foreach (FieldInfo componentField in componentFields)
            {
                NixInjectComponentBaseAttribute injectAttribute = componentField.GetCustomAttribute<NixInjectComponentBaseAttribute>();

                if (injectAttribute is NixInjectComponentListAttribute)
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
        /// Fill a component or interface (retrievable from Unity dependency injection methods) field in the objectToLink using the Unity dependency injection method which corresponds to the value of NixInjectComponentAttribute.GameObjectMethod in GameObjectMethodBindings
        /// </summary>
        /// <param name="componentField">Component or interface field</param>
        /// <param name="injectListAttribute">Nixi component (or interface) list attribute which decorate componentField</param>
        private void InjectEnumerableComponentField(FieldInfo componentField, NixInjectComponentBaseAttribute injectListAttribute)
        {
            object componentResult = injectListAttribute.GetComponentResult(objectToLink, componentField);
            componentField.SetValue(objectToLink, componentResult, BindingFlags.SetProperty, new EnumerableComponentBinder(), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Fill a component or interface (retrievable from Unity dependency injection methods) field in the objectToLink using the Unity dependency injection method which corresponds to the value of NixInjectComponentAttribute.GameObjectMethod in GameObjectMethodBindings
        /// </summary>
        /// <param name="componentField">Component or interface field</param>
        /// <param name="injectAttribute">Nixi component (or interface) attribute which decorate componentField</param>
        private void InjectComponentField(FieldInfo componentField, NixInjectComponentBaseAttribute injectAttribute)
        {
            if (!componentField.IsComponent() && !componentField.FieldType.IsInterface)
                throw new NixInjecterException($"Cannot inject field with name {componentField.Name} with a NixInjectComponentAttribute because it is not a component or an interface field, you must use NixInjectAttribute instead", objectToLink);

            object componentResult = injectAttribute.GetComponentResult(objectToLink, componentField);
            componentField.SetValue(objectToLink, componentResult);
        }
        #endregion Component Injections
    }

    
}