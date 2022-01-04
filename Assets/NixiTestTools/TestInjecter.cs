using Nixi.Injections;
using Nixi.Injections.Abstractions;
using Nixi.Injections.ComponentFields.MultiComponents.Abstractions;
using Nixi.Injections.Injecters;
using NixiTestTools.TestInjecterElements;
using NixiTestTools.TestInjecterElements.Relations.Components;
using NixiTestTools.TestInjecterElements.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace NixiTestTools
{
    /// <summary>
    /// Test way to handle all injections of fields decorated with Nixi inject attributes of a class derived from MonoBehaviourInjectable during test execution
    /// <list type="bullet">
    ///     <item>
    ///         <term>Component fields</term>
    ///         <description>Marked with NixInjectComponentAttribute will be created, used to populate the field and registered in TestInjecter property of InjectionTestTemplate</description>
    ///     </item>
    ///     <item>
    ///         <term>Non-Component fields</term>
    ///         <description>Marked with NixInjectAttribute will be mockable, you can manually inject mock in it (values are null if not populated by a manual Non-Component field injection)</description>
    ///     </item>
    /// </list>
    /// <para/>If a field is of type MonoBehaviourInjectable, this will fill it in the same way as for this TestInjecter but will be stored as the childInjecter of this main TestInjecter
    /// <para/>This is done recursively but all are stored in the same list of childInjecters of this main TestInjecter
    /// </summary>
    public sealed class TestInjecter : NixInjecterBase
    {
        /// <summary>
        /// Container to handle TestInjecter instances, it is used to inject into a field or GetComponent from fields instantiated during the Test injections
        /// </summary>
        private InjectablesContainer injectablesContainer = new InjectablesContainer();

        /// <summary>
        /// Top MonoBehaviourInjectable instance name to used in GetRootInjectable and avoid duplicate if there is mutual injection
        /// </summary>
        private string instanceName = "";

        /// <summary>
        /// Each mapping added into this container force a type to be used by his derived form during tests using TestInjecter, useful when working on abstract component injected with Nixi
        /// </summary>
        private AbstractComponentMappingContainer componentMappingContainer;

        /// <summary>
        /// Test way to handle all injections of fields decorated with Nixi inject attributes of a class derived from MonoBehaviourInjectable during test execution
        /// <list type="bullet">
        ///     <item>
        ///         <term>Component fields</term>
        ///         <description>Marked with NixInjectComponentAttribute will be created, used to populate the field and registered in TestInjecter property of InjectionTestTemplate</description>
        ///     </item>
        ///     <item>
        ///         <term>Non-Component fields</term>
        ///         <description>Marked with NixInjectAttribute will be mockable, you can manually inject mock in it (values are null if not populated by a manual Non-Component field injection)</description>
        ///     </item>
        /// </list>
        /// <para/>If a field is of type MonoBehaviourInjectable, this will fill it in the same way as for this TestInjecter but will be stored as the childInjecter of this main TestInjecter
        /// <para/>This is done recursively but all are stored in the same list of childInjecters of this main TestInjecter
        /// </summary>
        /// <param name="objectToLink">Instance of the class derived from MonoBehaviourInjectable on which all injections will be triggered (top MonoBehaviourInjectable)</param>
        /// <param name="instanceName">Top MonoBehaviourInjectable instance name to used in GetRootInjectable and avoid duplicate if there is mutual injection</param>
        /// <param name="componentMappingContainer">If filled, each mapping added into this container force a type to be used by his derived form during tests using TestInjecter, useful when working on abstract component injected with Nixi</param>
        public TestInjecter(MonoBehaviourInjectable objectToLink, string instanceName = "",
                            AbstractComponentMappingContainer componentMappingContainer = null)
            : base(objectToLink)
        {
            this.instanceName = instanceName;
            this.componentMappingContainer = componentMappingContainer;
        }

        /// <summary>
        /// Inject all the fields decorated by NixInjectAttribute or NixInjectComponentAttribute in top MonoBehaviourInjectable
        /// </summary>
        /// <exception cref="TestInjecterException">Thrown if this method has already been called</exception>
        protected override void InjectAll()
        {
            InjectMonoBehaviourInjectable(objectToLink, instanceName);
        }

        /// <summary>
        /// Inject all the nixi fields of a MonoBehaviourInjectable for testing
        /// </summary>
        /// <param name="monoBehaviourInjectable">MonoBehaviourInjectable to inject</param>
        /// <param name="instanceName">Name of the instance</param>
        private void InjectMonoBehaviourInjectable(MonoBehaviourInjectable monoBehaviourInjectable, string instanceName = "")
        {
            InjectableHandler newInjectableHandler = injectablesContainer.CreateAndAdd(monoBehaviourInjectable, instanceName);
            List<FieldInfo> fields = GetAllFields(monoBehaviourInjectable.GetType());
            
            CheckNotDifferentTransformOnInjectable(fields);
            PrioritizeOnRectTransformIfExists(fields);

            try
            {
                InjectFields(fields.Where(NixiFieldPredicate), newInjectableHandler);
                InjectComponentFields(fields.Where(NixiComponentFieldPredicate), newInjectableHandler);
            }
            catch (NixiAttributeException exception)
            {
                throw new TestInjecterException(exception.Message, objectToLink);
            }
        }

        /// <summary>
        /// Check if all field of a monoBehaviourInjectable does not have different kind of Transform implemented at the same level of his instance
        /// <para/> e.g : exception will be thrown if Transform and RectTransform are at top level (same level) of the monoBehaviourInjectable instance
        /// </summary>
        /// <param name="fields">Fields to check</param>
        private void CheckNotDifferentTransformOnInjectable(List<FieldInfo> fields)
        {
            List<FieldInfo> transformFields = fields.Where(x => typeof(Transform).IsAssignableFrom(x.FieldType)).ToList();

            if (transformFields.Count <= 1)
                return;

            Type firstFieldType = transformFields.First().FieldType;
            foreach (FieldInfo transformField in transformFields.Skip(1))
            {
                if (transformField.FieldType != firstFieldType)
                    throw new TestInjecterException($"Cannot inject a monoBehaviourInjectable with two differents type of Transform, both (or all) of them correspond to {firstFieldType.Name} and {transformField.FieldType.Name}", objectToLink);
            }
        }

        /// <summary>
        /// Detect if a rect transform is present and place it at the top of the list (this is because can't add two different kind of transform on one gameObject)
        /// </summary>
        /// <param name="fields">All fields to check</param>
        private void PrioritizeOnRectTransformIfExists(List<FieldInfo> fields)
        {
            for (int i = 0; i < fields.Count; i++)
            {
                if (fields[i].FieldType == typeof(RectTransform))
                {
                    // Found at first position, nothing to change
                    if (i == 0)
                        return;

                    // Moving RectTransform field detected at top position (swap with first element)
                    FieldInfo rectTransformField = fields[i];
                    FieldInfo firstField = fields[0];
                    fields[0] = rectTransformField;
                    fields[i] = firstField;
                    return;
                }
            }
        }

        /// <summary>
        /// Register all Non-Component fields (decorated with NixInjectAttribute) into fieldInjectionHandler to expose them as mockable
        /// <para/> These fields values are null if not populated by a manual Non-Component field injection
        /// </summary>
        /// <param name="nonComponentFields">Non-Component fields</param>
        /// <param name="newInjectableHandler">New InjectableHandler on which we are loading Non-Component fields (newInjectableHandler.nonComponentFields)</param>
        private void InjectFields(IEnumerable<FieldInfo> nonComponentFields, InjectableHandler newInjectableHandler)
        {
            foreach (FieldInfo nonComponentField in nonComponentFields)
            {
                CheckIfNixInjectBaseAttribute(nonComponentField);
                newInjectableHandler.AddField(nonComponentField);
            }
        }

        /// <summary>
        /// Check if a field is decorated with an attribute derived from NixInjectBaseAttribute, if yes it applies his validity checks
        /// </summary>
        /// <param name="nonComponentField">Non-Component field</param>
        private static void CheckIfNixInjectBaseAttribute(FieldInfo nonComponentField)
        {
            NixInjectBaseAttribute injectAttribute = nonComponentField.GetCustomAttribute<NixInjectBaseAttribute>();
            if (injectAttribute != null)
            {
                injectAttribute.CheckIsValidAndBuildDataFromField(nonComponentField);
            }
        }

        #region Component fields
        /// <summary>
        /// Create, populate and register all Component fields (decorated with class derived from NixInjectComponentBaseAttribute)
        /// <para/>If a field is of type MonoBehaviourInjectable, this will fill it in the same way as for this TestInjecter and register in tiContainer
        /// <para/>This is done recursively but all are stored in tiContainer
        /// </summary>
        /// <param name="componentFields">Component fields</param>
        /// <param name="newInjectableHandler">New InjectableHandler on which we are loading Component fields (newInjectableHandler.componentFields)</param>
        private void InjectComponentFields(IEnumerable<FieldInfo> componentFields, InjectableHandler newInjectableHandler)
        {
            foreach (FieldInfo componentField in componentFields)
            {
                NixInjectComponentBaseAttribute baseAttribute = componentField.GetCustomAttribute<NixInjectComponentBaseAttribute>();
                baseAttribute.CheckIsValidAndBuildDataFromField(componentField);

                if (baseAttribute is NixInjectMultiComponentsBaseAttribute componentsAttribute)
                {
                    if (componentsAttribute.EnumerableType.IsInterface)
                    {
                        StoreInterfaceComponentField(newInjectableHandler, componentField);
                    }
                    else
                    {
                        StoreEnumerableComponentField(newInjectableHandler, componentField, componentsAttribute);
                    }
                }
                else if (componentField.FieldType.IsInterface)
                {
                    StoreInterfaceComponentField(newInjectableHandler, componentField);
                }
                else
                {
                    StoreComponentField(newInjectableHandler, componentField);
                }
            }
        }

        /// <summary>
        /// Store the component field like a non-component field to expose field as a mockable like a standard interface
        /// <para/>This is useful when we want to work on interface with GetComponent in play mode, but it has the same behaviour
        /// as a mockable in the tests mode
        /// </summary>
        /// /// <param name="newInjectableHandler">New InjectableHandler on which we can store interface component field onto newInjectableHandler.nonComponentFields </param>
        /// <param name="interfaceComponentField">Component field as interface</param>
        private void StoreInterfaceComponentField(InjectableHandler newInjectableHandler, FieldInfo interfaceComponentField)
        {
            newInjectableHandler.AddField(interfaceComponentField);
        }

        /// <summary>
        /// Store the enumerable component field and fill it with empty IEnumerable
        /// </summary>
        /// <param name="newInjectableHandler">New InjectableHandler on which we are loading Component fields (newInjectableHandler.enumerableComponentFields)</param>
        /// <param name="enumerableField">Field where the IEnumerable content is stored</param>
        /// <param name="componentsAttribute">Nixi list attribute decorating the field to retrieve data about his injection</param>
        private void StoreEnumerableComponentField(InjectableHandler newInjectableHandler, FieldInfo enumerableField, NixInjectMultiComponentsBaseAttribute componentsAttribute)
        {
            newInjectableHandler.AddEnumerableComponentField(new ComponentListWithFieldInfo
            {
                FieldInfo = enumerableField,
                EnumerableType = componentsAttribute.EnumerableType,
                GameObjectLevel = componentsAttribute.GameObjectLevel
            });
        }

        /// <summary>
        /// Fill a componentField with a component already instantiated if it is a MonoBehaviourInjectable with name already stored in InjectablesContainer
        /// <para/> If not, a new instance of Component is stored with name if it is precised in the attribute
        /// </summary>
        /// <param name="newInjectableHandler">New InjectableHandler on which we are loading Component fields (newInjectableHandler.componentFields)</param>
        /// <param name="componentField">Component field</param>
        private void StoreComponentField(InjectableHandler newInjectableHandler, FieldInfo componentField)
        {
            InjectableHandler injectableHandlerFromName = GetInjectableFromNameToFind(componentField);

            if (injectableHandlerFromName != null)
            {
                newInjectableHandler.FillFieldWithComponentAndStore(componentField, injectableHandlerFromName.Instance);
            }
            else
            {
                if (!CheckAndInjectIfSameInstance(newInjectableHandler, componentField))
                {
                    PopulateAndRegisterComponentField(componentField, newInjectableHandler);
                    InjectAndStoreIfIsMonoBehaviourInjectable(componentField, newInjectableHandler);
                }
            }
        }

        /// <summary>
        /// Return injectable if componentField is associated to a name, null if is not the case, it is helpful for handling rootComponent
        /// </summary>
        /// <param name="componentField">Component field to analyze</param>
        /// <returns>Null if not found or if componentField Nix attribute does not implements IHaveComponentNameToFind</returns>
        private InjectableHandler GetInjectableFromNameToFind(FieldInfo componentField)
        {
            string componentNameToFind = GetComponentNameIfExists(componentField);

            if (string.IsNullOrEmpty(componentNameToFind))
                return null;

            return injectablesContainer.GetInjectable(componentNameToFind, componentField.FieldType);
        }

        /// <summary>
        /// Based on Nixi component attribute decorator type, check if a component has been loaded previously on newInjectableHandler and inject it if found
        /// </summary>
        /// <param name="newInjectableHandler">New InjectableHandler on which we are loading fields</param>
        /// <param name="componentField">Component field</param>
        /// <returns>True if injected from instance found</returns>
        private bool CheckAndInjectIfSameInstance(InjectableHandler newInjectableHandler, FieldInfo componentField)
        {
            NixInjectComponentBaseAttribute baseAttribute = componentField.GetCustomAttribute<NixInjectComponentBaseAttribute>();

            if (baseAttribute is NixInjectComponentAttribute)
            {
                Type typeResolved = componentMappingContainer?.TryResolve(componentField.FieldType);
                InjectableComponentState state = newInjectableHandler.InjectOrBuildComponentAtTopComponentLevel(componentField, typeResolved);
                if (state == InjectableComponentState.NeedToBeInjectedIfInjectable)
                {
                    InjectAndStoreIfIsMonoBehaviourInjectable(componentField, newInjectableHandler);
                }
                return true;
            }
            else if (baseAttribute is NixInjectRootComponentAttribute rootAttribute)
            {
                return CheckAndInjectIfAlreadyInjectedInRootsComponent(newInjectableHandler, componentField, rootAttribute);
            }
            return false;
        }

        /// <summary>
        /// Check if a root component with same type has been loaded previously on newInjectableHandler and inject it if found
        /// </summary>
        /// <param name="newInjectableHandler">New InjectableHandler on which we are loading fields</param>
        /// <param name="componentField">Component field</param>
        /// <param name="rootAttribute">RootComponent Nixi attribute decorator</param>
        /// <returns>True if injected from instance found</returns>
        private bool CheckAndInjectIfAlreadyInjectedInRootsComponent(InjectableHandler newInjectableHandler, FieldInfo componentField, NixInjectRootComponentAttribute rootAttribute)
        {
            // Component list on parent if SubComponentRootName (child) is empty, component list on child if not
            IEnumerable<Component> componentsInRootRelation = injectablesContainer.GetComponentsFromRelation(rootAttribute.RootGameObjectName, rootAttribute.SubGameObjectName);

            if (componentsInRootRelation == null || !componentsInRootRelation.Any())
                return false;

            // Case components found in rootRelation, we just need to link it
            Component componentFoundWithType = componentsInRootRelation.SingleOrDefault(x => x.GetType() == componentField.FieldType);
            if (componentFoundWithType != null)
            {
                newInjectableHandler.FillFieldWithComponentAndStore(componentField, componentFoundWithType);
            }
            else
            {
                Component componentToStore = AddComponentOnGameObjectAssociatedToRootAttribute(componentField.FieldType, componentsInRootRelation);
                newInjectableHandler.FillFieldWithComponentAndStore(componentField, componentToStore);
                injectablesContainer.AddOrUpdateRootRelation(componentToStore, rootAttribute.RootGameObjectName, rootAttribute.SubGameObjectName);
            }

            return true;
        }

        /// <summary>
        /// Add component on the first gameObject of componentsInRootRelation (match to rootAttribute data means this is the same gameObject), 
        /// because they all have the same gameObject, it must be AddComponent and not a new gameObject built
        /// </summary>
        /// <param name="componentFieldType">Type contains in the component field</param>
        /// <param name="componentsInRootRelation">All root relations that match rootAttribute data</param>
        /// <returns>New component built</returns>
        private Component AddComponentOnGameObjectAssociatedToRootAttribute(Type componentFieldType, IEnumerable<Component> componentsInRootRelation)
        {
            // GameObject is the same for every components of this rootRelation (match rootAttribute data)
            GameObject gameObject = componentsInRootRelation.First().gameObject;
            Type typeResolved = componentMappingContainer?.TryResolve(componentFieldType);
            return gameObject.AddComponent(typeResolved ?? componentFieldType);
        }

        /// <summary>
        /// Create, populate and register a Component field (decorated with a class derived from NixInjectComponentBaseAttribute) into componentFieldInjectionHandler
        /// </summary>
        /// <param name="componentField">Component field</param>
        /// <param name="injectableHandler">New InjectableHandler on which we are loading fields</param>
        private void PopulateAndRegisterComponentField(FieldInfo componentField, InjectableHandler injectableHandler)
        {
            string componentName = GetComponentNameIfExists(componentField);

            Type typeResolved = componentMappingContainer?.TryResolve(componentField.FieldType);

            Component componentAdded = injectableHandler.BuildComponent(componentField, componentName, typeResolved);
            UpdateRootRelationIfRootComponent(componentField, componentAdded);
        }

        /// <summary>
        /// If componentField if decorated with NixInjectRootComponentAttribute, it update the root relation concerned by his data
        /// </summary>
        /// <param name="componentField">Component field</param>
        /// <param name="componentAdded">Component recently created</param>
        private void UpdateRootRelationIfRootComponent(FieldInfo componentField, Component componentAdded)
        {
            NixInjectRootComponentAttribute rootAttribute = componentField.GetCustomAttribute<NixInjectRootComponentAttribute>();
            if (rootAttribute != null)
            {
                Component existingComponent = injectablesContainer.GetComponentFromRelation(componentField.FieldType, rootAttribute.RootGameObjectName, rootAttribute.SubGameObjectName);
                if (existingComponent == null)
                {
                    injectablesContainer.AddOrUpdateRootRelation(componentAdded, rootAttribute.RootGameObjectName, rootAttribute.SubGameObjectName);
                }
            }
        }

        /// <summary>
        /// If monoBehaviourInjectAttribute implements IHaveComponentNameToFind, it returns the ComponentNameToFind value, if not it return string.Empty
        /// <para/>This will be treated in TestInjecter which link ComponentName with TestInjecter.GetComponent to simplify access with component name in tests
        /// </summary>
        /// <param name="componentField">Component field</param>
        /// <returns>ComponentNameToFind if available, string.Empty if not</returns>
        private static string GetComponentNameIfExists(FieldInfo componentField)
        {
            NixInjectComponentBaseAttribute injectAttribute = componentField.GetCustomAttribute<NixInjectComponentBaseAttribute>();
            if (injectAttribute is IHaveComponentNameToFind componentNameToFindInstance)
            {
                return componentNameToFindInstance.ComponentNameToFind;
            }
            return string.Empty;
        }
        #endregion Component fields

        #region Recursion
        /// <summary>
        /// Checks if the componentField is MonoBehaviourInjectable and inject it the same way as for this TestInjecter but will be stored as the childInjecter of this main TestInjecter
        /// <para/>This is done recursively but all are stored in the same list of childInjecters of this main TestInjecter
        /// </summary>
        /// <param name="componentField">Component field</param>
        /// <param name="newInjectableHandler">InjectableHandler to check if instance is MonoBehaviourInjectable</param>
        private void InjectAndStoreIfIsMonoBehaviourInjectable(FieldInfo componentField, InjectableHandler newInjectableHandler)
        {
            if (componentField.GetValue(newInjectableHandler.Instance) is MonoBehaviourInjectable monoBehaviourInjectable)
            {
                newInjectableHandler.CheckInfiniteRecursion(componentField);

                string componentName = GetComponentNameIfExists(componentField);

                InjectMonoBehaviourInjectable(monoBehaviourInjectable, componentName);
            }
        }
        #endregion Recursion

        #region Field injection/reading and Get Component
        /// <summary>
        /// Read value from a non component field with type T
        /// <para/>If no targetInjectable precised, it targets top MonoBehaviourInjectable by default
        /// </summary>
        /// <typeparam name="T">Targeted field type</typeparam>
        /// <param name="targetInjectable">Optional parameter, if null it targets top MonoBehaviourInjectable by default, if filled it must be a MonoBehaviourInjectable recursively injected and obtained from GetComponent</param>
        /// <returns>Targeted field value</returns>
        public T ReadField<T>(MonoBehaviourInjectable targetInjectable = null)
        {
            try
            {
                return injectablesContainer.ReadField<T>(targetInjectable ?? objectToLink);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot ReadExposedField because {e.Message}", objectToLink);
            }
        }

        /// <summary>
        /// Read value from a non component field with type T and with the fieldName passed as a parameter
        /// <para/>If no targetInjectable precised, it targets top MonoBehaviourInjectable by default
        /// </summary>
        /// <typeparam name="T">Targeted field type</typeparam>
        /// <param name="fieldName">Name of the field</param>
        /// <param name="targetInjectable">Optional parameter, if null it targets top MonoBehaviourInjectable by default, if filled it must be a MonoBehaviourInjectable recursively injected and obtained from GetComponent</param>
        /// <returns>Targeted field value</returns>
        public T ReadField<T>(string fieldName, MonoBehaviourInjectable targetInjectable = null)
        {
            try
            {
                return injectablesContainer.ReadField<T>(fieldName, targetInjectable ?? objectToLink);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot ReadExposedField because {e.Message}", objectToLink);
            }
        }

        /// <summary>
        /// Inject value into non component field with type T
        /// <para/>If no targetInjectable precised, it targets top MonoBehaviourInjectable by default
        /// <para/>If multiple type T fields are found, you must use InjectField(T valueToInject, string fieldName)
        /// </summary>
        /// <typeparam name="T">Targeted field type</typeparam>
        /// <param name="valueToInject">Value to inject into field</param>
        /// <param name="targetInjectable">Optional parameter, if null it targets top MonoBehaviourInjectable by default, if filled it must be a MonoBehaviourInjectable recursively injected and obtained from GetComponent</param>
        /// <returns>Value injected, it can help simplify test readilibity</returns>
        public T InjectField<T>(T valueToInject, MonoBehaviourInjectable targetInjectable = null)
        {
            try
            {
                injectablesContainer.InjectField(valueToInject, targetInjectable ?? objectToLink);
                return valueToInject;
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot InjectField because {e.Message}", objectToLink);
            }
        }

        /// <summary>
        /// Inject value into non component field with type T and with the fieldName passed as a parameter
        /// <para/>If no targetInjectable precised, it targets top MonoBehaviourInjectable by default
        /// </summary>
        /// <typeparam name="T">Targeted field type</typeparam>
        /// <param name="fieldName">Name of the field</param>
        /// <param name="valueToInject">Value to inject into field</param>
        /// <param name="targetInjectable">Optional parameter, if null it targets top MonoBehaviourInjectable by default, if filled it must be a MonoBehaviourInjectable recursively injected and obtained from GetComponent</param>
        /// <returns>Value injected, it can help simplify test readilibity</returns>
        public T InjectField<T>(T valueToInject, string fieldName, MonoBehaviourInjectable targetInjectable = null)
        {
            try
            {
                injectablesContainer.InjectField(fieldName, valueToInject, targetInjectable ?? objectToLink);
                return valueToInject;
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot InjectField because {e.Message}", objectToLink);
            }
        }

        /// <summary>
        /// Return the only component with Component type T field
        /// <para/>If no targetInjectable precised, it targets top MonoBehaviourInjectable by default
        /// <para/>If multiple type T fields are found, you must use GetComponent&lt;T&gt;(string fieldName) 
        /// </summary>
        /// <typeparam name="T">Type of Component searched</typeparam>
        /// <param name="targetInjectable">Optional parameter, if null it targets top MonoBehaviourInjectable by default, if filled it must be a MonoBehaviourInjectable recursively injected and obtained from GetComponent</param>
        /// <returns>Single component which corresponds to the type T field</returns>
        public T GetComponent<T>(MonoBehaviourInjectable targetInjectable = null)
            where T : Component
        {
            try
            {
                return injectablesContainer.GetComponent<T>(targetInjectable ?? objectToLink);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot GetComponent because {e.Message}", objectToLink);
            }
        }

        /// <summary>
        /// Return the only component with Component type T field and with the fieldName passed as a parameter
        /// <para/>If no targetInjectable precised, it targets top MonoBehaviourInjectable by default
        /// </summary>
        /// <typeparam name="T">Type of Component searched</typeparam>
        /// <param name="fieldName">Name of the field searched</param>
        /// <param name="targetInjectable">Optional parameter, if null it targets top MonoBehaviourInjectable by default, if filled it must be a MonoBehaviourInjectable recursively injected and obtained from GetComponent</param>
        /// <returns>Single component which corresponds to the type T field and fieldName passed as a parameter</returns>
        public T GetComponent<T>(string fieldName, MonoBehaviourInjectable targetInjectable = null)
            where T : Component
        {
            try
            {
                return injectablesContainer.GetComponent<T>(fieldName, targetInjectable ?? objectToLink);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot GetComponent because {e.Message}", objectToLink);
            }
        }
        #endregion Field injection/reading and Get Component

        #region EnumerableComponent

        #region Generic
        /// <summary>
        /// Init an enumerable component field and fill it with "nbAdded" components instantiated that match type of T (apply on all enumerable of same EnumerableType and same GameObjectLevel)
        /// <para/>Throw exception if already initiliazed once
        /// </summary>
        /// <typeparam name="T">Enumerable type searched inherited from Component)</typeparam>
        /// <param name="gameObjectLevel">Precise method to use to target list (children, parents or current component level list)</param>
        /// <param name="nbAdded">Number of element of type T to add into the enumerable component fields</param>
        /// <param name="targetInjectable">Optional parameter, if null it targets top MonoBehaviourInjectable by default (current injectable), if filled it must be a MonoBehaviourInjectable recursively injected and obtained from GetComponent</param>
        /// <returns>New component instantiated which corresponds to type T field</returns>
        private IEnumerable<T> InitEnumerableComponentGeneric<T>(GameObjectLevel gameObjectLevel, int nbAdded, MonoBehaviourInjectable targetInjectable = null)
            where T : Component
        {
            try
            {
                targetInjectable = targetInjectable ?? objectToLink;

                IEnumerable<T> components = injectablesContainer.InitEnumerableComponents<T>(gameObjectLevel, nbAdded, targetInjectable);

                InjectComponentsIfInjectable(components);

                return components;
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot InitEnumerableComponent with nbAdded : {nbAdded} on TargetedInjectable with name {targetInjectable.name} and {targetInjectable.GetType().Name} " +
                    $"at {gameObjectLevel} gameObject level, because {e.Message}", objectToLink);
            }
        }

        /// <summary>
        /// Init an enumerable component field and fill it with as many instances of component (instantiated in this method) as typeDeriveds (apply on all enumerable of same EnumerableType and same GameObjectLevel)
        /// <para/>Throw exception if already initiliazed once
        /// </summary>
        /// <typeparam name="T">Enumerable type searched inherited from Component)</typeparam>
        /// <param name="gameObjectLevel">Precise method to use to target list (children, parents or current component level list)</param>
        /// <param name="targetInjectable">Optional parameter, if null it targets top MonoBehaviourInjectable by default (current injectable), if filled it must be a MonoBehaviourInjectable recursively injected and obtained from GetComponent</param>
        /// <param name="typeDeriveds">All typeDeriveds that must be equals or inherited from T</param>
        /// <returns>New component instantiated which corresponds to type T field</returns>
        private IEnumerable<T> InitEnumerableComponentsWithTypesGeneric<T>(GameObjectLevel gameObjectLevel, MonoBehaviourInjectable targetInjectable = null, params Type[] typeDeriveds)
            where T : Component
        {
            try
            {
                targetInjectable = targetInjectable ?? objectToLink;

                IEnumerable<T> components = injectablesContainer.InitEnumerableComponentsWithType<T>(gameObjectLevel, targetInjectable, typeDeriveds);

                InjectComponentsIfInjectable(components);

                return components;
            }
            catch (InjectablesContainerException e)
            {
                string typeJoined = string.Join(",", typeDeriveds.ToList());
                throw new TestInjecterException($"Cannot InitEnumerableComponent with typeDeriveds ({typeJoined}) on TargetedInjectable with name {targetInjectable.name} and {targetInjectable.GetType().Name} " +
                    $"at {gameObjectLevel} gameObject level, because {e.Message}", objectToLink);
            }
        }
        #endregion Generic

        #region Init
        /// <summary>
        /// Init an enumerable component field and fill it with only one component instantiated that match type of T (at currentLevel of the gameObject of the monoBehaviourInjectable instance)
        /// <para/>If no targetInjectable precised, it targets top MonoBehaviourInjectable by default
        /// </summary>
        /// <typeparam name="T">Type of Component searched</typeparam>
        /// <param name="targetInjectable">Optional parameter, if null it targets top MonoBehaviourInjectable by default (current injectable), if filled it must be a MonoBehaviourInjectable recursively injected and obtained from GetComponent</param>
        /// <returns>New component instantiated which corresponds to the type T field</returns>
        public T InitSingleEnumerableComponent<T>(MonoBehaviourInjectable targetInjectable = null)
            where T : Component
        {
            IEnumerable<T> components = InitEnumerableComponentGeneric<T>(GameObjectLevel.Current, 1, targetInjectable);
            return components.Single();
        }

        /// <summary>
        /// Init an enumerable component field and fill it with only one component instantiated that match type of T (apply on all enumerable of same EnumerableType and same GameObjectLevel)
        /// <para/>Throw exception if already initiliazed once
        /// </summary>
        /// <typeparam name="T">Enumerable type searched inherited from Component)</typeparam>
        /// <param name="gameObjectLevel">Precise method to use to target list (children, parents or if null it is for current component level list)</param>
        /// <param name="targetInjectable">Optional parameter, if null it targets top MonoBehaviourInjectable by default (current injectable), if filled it must be a MonoBehaviourInjectable recursively injected and obtained from GetComponent</param>
        /// <returns>New component instantiated which corresponds to type T field</returns>
        public T InitSingleEnumerableComponent<T>(GameObjectLevel gameObjectLevel, MonoBehaviourInjectable targetInjectable = null)
            where T : Component
        {
            IEnumerable<T> components = InitEnumerableComponentGeneric<T>(gameObjectLevel, 1, targetInjectable);
            return components.Single();
        }

        /// <summary>
        /// Init an enumerable component field and fill it with "nbAdded" components instantiated that match type of T (apply on all enumerable of same EnumerableType at same level of targetInjectable instance)
        /// <para/>Throw exception if already initiliazed once
        /// </summary>
        /// <typeparam name="T">Enumerable type searched inherited from Component)</typeparam>
        /// <param name="nbAdded">Number of element of type T to add into the enumerable component fields</param>
        /// <param name="targetInjectable">Optional parameter, if null it targets top MonoBehaviourInjectable by default (current injectable), if filled it must be a MonoBehaviourInjectable recursively injected and obtained from GetComponent</param>
        /// <returns>New component instantiated which corresponds to type T field</returns>
        public IEnumerable<T> InitEnumerableComponents<T>(int nbAdded = 1, MonoBehaviourInjectable targetInjectable = null)
            where T : Component
        {
            return InitEnumerableComponentGeneric<T>(GameObjectLevel.Current, nbAdded, targetInjectable);
        }

        /// <summary>
        /// Add "nbAdded" element(s) in the list of components instantiated with Enumerable Component type T field (based on GameObjectLevel, this is at parent or child level of the gameObject of the monoBehaviourInjectable instance)
        /// <para/>If no targetInjectable precised, it targets top MonoBehaviourInjectable by default
        /// </summary>
        /// <typeparam name="T">Type of Component searched</typeparam>
        /// <param name="gameObjectLevel">Precise method to use to target list (children, parents or if null it is for current component level list)</param>
        /// <param name="nbAdded">Number of element of type T to add into the enumerable component fields</param>
        /// <param name="targetInjectable">Optional parameter, if null it targets top MonoBehaviourInjectable by default (current injectable), if filled it must be a MonoBehaviourInjectable recursively injected and obtained from GetComponent</param>
        /// <returns>New component instantiated which corresponds to the type T field</returns>
        public IEnumerable<T> InitEnumerableComponents<T>(GameObjectLevel gameObjectLevel, int nbAdded = 1, MonoBehaviourInjectable targetInjectable = null)
            where T : Component
        {
            return InitEnumerableComponentGeneric<T>(gameObjectLevel, nbAdded, targetInjectable);
        }

        /// <summary>
        /// Init an enumerable component field and fill it with as many instances of component (instantiated in this method) as typeDeriveds (apply on all enumerable of same EnumerableType and same GameObjectLevel)
        /// <para/>Throw exception if already initiliazed once
        /// </summary>
        /// <typeparam name="T">Enumerable type searched inherited from Component)</typeparam>
        /// <param name="typeDeriveds">All typeDeriveds that must be equals or inherited from T</param>
        /// <returns>New component instantiated which corresponds to type T field</returns>
        public IEnumerable<T> InitEnumerableComponentsWithTypes<T>(params Type[] typeDeriveds)
            where T : Component
        {
            return InitEnumerableComponentsWithTypesGeneric<T>(GameObjectLevel.Current, null, typeDeriveds);
        }

        /// <summary>
        /// Init an enumerable component field and fill it with as many instances of component (instantiated in this method) as typeDeriveds (apply on all enumerable of same EnumerableType and same GameObjectLevel)
        /// <para/>Throw exception if already initiliazed once
        /// </summary>
        /// <typeparam name="T">Enumerable type searched inherited from Component)</typeparam>
        /// <param name="targetInjectable">Optional parameter, if null it targets top MonoBehaviourInjectable by default (current injectable), if filled it must be a MonoBehaviourInjectable recursively injected and obtained from GetComponent</param>
        /// <param name="typeDeriveds">All typeDeriveds that must be equals or inherited from T</param>
        /// <returns>New component instantiated which corresponds to type T field</returns>
        public IEnumerable<T> InitEnumerableComponentsWithTypes<T>(MonoBehaviourInjectable targetInjectable = null, params Type[] typeDeriveds)
            where T : Component
        {
            return InitEnumerableComponentsWithTypesGeneric<T>(GameObjectLevel.Current, targetInjectable, typeDeriveds);
        }

        /// <summary>
        /// Init an enumerable component field and fill it with as many instances of component (instantiated in this method) as typeDeriveds (apply on all enumerable of same EnumerableType and same GameObjectLevel)
        /// <para/>Throw exception if already initiliazed once
        /// </summary>
        /// <typeparam name="T">Enumerable type searched inherited from Component)</typeparam>
        /// <param name="gameObjectLevel">Precise method to use to target list (children, parents or if null it is for current component level list)</param>
        /// <param name="typeDeriveds">All typeDeriveds that must be equals or inherited from T</param>
        /// <returns>New component instantiated which corresponds to type T field</returns>
        public IEnumerable<T> InitEnumerableComponentsWithTypes<T>(GameObjectLevel gameObjectLevel, params Type[] typeDeriveds)
            where T : Component
        {
            return InitEnumerableComponentsWithTypesGeneric<T>(gameObjectLevel, null, typeDeriveds);
        }

        /// <summary>
        /// Init an enumerable component field and fill it with as many instances of component (instantiated in this method) as typeDeriveds (apply on all enumerable of same EnumerableType and same GameObjectLevel)
        /// <para/>Throw exception if already initiliazed once
        /// </summary>
        /// <typeparam name="T">Enumerable type searched inherited from Component)</typeparam>
        /// <param name="gameObjectLevel">Precise method to use to target list (children, parents or if null it is for current component level list)</param>
        /// <param name="targetInjectable">Optional parameter, if null it targets top MonoBehaviourInjectable by default (current injectable), if filled it must be a MonoBehaviourInjectable recursively injected and obtained from GetComponent</param>
        /// <param name="typeDeriveds">All typeDeriveds that must be equals or inherited from T</param>
        /// <returns>New component instantiated which corresponds to type T field</returns>
        public IEnumerable<T> InitEnumerableComponentsWithTypes<T>(GameObjectLevel gameObjectLevel, MonoBehaviourInjectable targetInjectable = null, params Type[] typeDeriveds)
            where T : Component
        {
            return InitEnumerableComponentsWithTypesGeneric<T>(gameObjectLevel, targetInjectable, typeDeriveds);
        }

        /// <summary>
        /// Browse components and inject each one that is injectable
        /// </summary>
        /// <typeparam name="T">Generic type of the enumerable</typeparam>
        /// <param name="components">All components to inject if injectable</param>
        private void InjectComponentsIfInjectable<T>(IEnumerable<T> components)
            where T : Component
        {
            foreach (T component in components)
            {
                if (component is MonoBehaviourInjectable monoBehaviourInjectable)
                {
                    InjectMonoBehaviourInjectable(monoBehaviourInjectable);
                }
            }
        }
        #endregion Init

        #region Get
        /// <summary>
        /// Get all values contained in an enumerable component field which match enumerable generic type, if only one is found, it returned it, if many found, use GetEnumerableComponents(fieldName)
        /// </summary>
        /// <typeparam name="T">Generic type of enumerable</typeparam>
        /// <param name="injectable">Targeted injectable</param>
        /// <returns>Enumerable values of the enumerable component field</returns>
        public IEnumerable<T> GetEnumerableComponents<T>(MonoBehaviourInjectable injectable = null)
            where T : Component
        {
            try
            {
                return injectablesContainer.GetEnumerableComponents<T>(injectable ?? objectToLink);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot GetEnumerableComponents because {e.Message}", objectToLink);
            }
        }

        /// <summary>
        /// Get all values contained in an enumerable component field which match enumerable generic type, if only one is found, it returned it, otherwise throw an exception
        /// </summary>
        /// <typeparam name="T">Generic type of enumerable</typeparam>
        /// <param name="fieldName">Name of the fields targeted</param>
        /// <param name="injectable">Targeted injectable</param>
        /// <returns>Enumerable values of the enumerable component field</returns>
        public IEnumerable<T> GetEnumerableComponents<T>(string fieldName, MonoBehaviourInjectable injectable = null)
            where T : Component
        {
            try
            {
                return injectablesContainer.GetEnumerableComponents<T>(fieldName, injectable ?? objectToLink);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot GetEnumerableComponents(fieldName) because {e.Message}", objectToLink);
            }
        }
        #endregion Get
        #endregion EnumerableComponent
    }
}
