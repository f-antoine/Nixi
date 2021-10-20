using Nixi.Injections;
using Nixi.Injections.Attributes;
using Nixi.Injections.Attributes.Abstractions;
using Nixi.Injections.Extensions;
using Nixi.Injections.Injecters;
using NixiTestTools.TestInjecterElements;
using NixiTestTools.TestInjecterElements.Relations.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

// TODO : Reprendre ici
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
        /// Container to handle TestInjecter instances, it is used to injectMock into a field or GetComponent from fields instantiated during the Test injections
        /// </summary>
        private InjectablesContainer injectablesContainer = new InjectablesContainer();

        /// <summary>
        /// ObjectToLink instance name to used in GetRootInjectable and avoid duplicate if there is mutual injection
        /// </summary>
        private string InstanceName = "";

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
        /// <param name="objectToLink">Instance of the class derived from MonoBehaviourInjectable on which all injections will be triggered</param>
        /// <param name="instanceName">ObjectToLink instance name to used in GetRootInjectable and avoid duplicate if there is mutual injection</param>
        public TestInjecter(MonoBehaviourInjectable objectToLink, string instanceName = "")
            : base(objectToLink)
        {
            if (!string.IsNullOrEmpty(instanceName))
            {
                injectablesContainer.AddOrUpdateRootRelation(objectToLink, instanceName);
                InstanceName = instanceName;
            }
        }

        /// <summary>
        /// Inject all the fields decorated by NixInjectAttribute or NixInjectComponentAttribute in objectToLink
        /// </summary>
        /// <exception cref="TestInjecterException">Thrown if this method has already been called</exception>
        protected override void InjectAll()
        {
            InjectMonoBehaviourInjectable(objectToLink, null, InstanceName);
        }

        // TODO : Mieux ranger oldInjectableData et se demander si utile + commenter
        /// <summary>
        /// Inject all the nixi fields of a MonoBehaviourInjectable for testing
        /// </summary>
        /// <param name="monoBehaviourInjectable">MonoBehaviourInjectable to inject</param>
        /// <param name="instanceName">Name of the instance</param>
        private void InjectMonoBehaviourInjectable(MonoBehaviourInjectable monoBehaviourInjectable, MonoBehaviourInjectableData oldInjectableData = null, string instanceName = "")
        {
            MonoBehaviourInjectableData newInjectableData = new MonoBehaviourInjectableData(monoBehaviourInjectable, instanceName, null);
            injectablesContainer.Add(newInjectableData);

            List<FieldInfo> fields = GetAllFields(monoBehaviourInjectable.GetType());

            try
            {
                InjectFields(fields.Where(NixiFieldPredicate), newInjectableData);
                InjectComponentFields(fields.Where(NixiComponentFieldPredicate), newInjectableData);
            }
            catch (NixiAttributeException exception)
            {
                throw new TestInjecterException(exception.Message, objectToLink);
            }
        }

        /// <summary>
        /// Register all Non-Component fields (decorated with NixInjectAttribute) into fieldInjectionHandler to expose them as mockable
        /// <para/> These fields values are null if not populated by a manual Non-Component field injection
        /// </summary>
        /// <param name="nonComponentFields">Non-Component fields</param>
        /// <param name="newInjectableData">New MonoBehaviourInjectableData on which we are loading Non-Component fields (newInjectableData.nonComponentFields)</param>
        private void InjectFields(IEnumerable<FieldInfo> nonComponentFields, MonoBehaviourInjectableData newInjectableData)
        {
            foreach (FieldInfo nonComponentField in nonComponentFields)
            {
                NixInjectBaseAttribute injectAttribute = nonComponentField.GetCustomAttribute<NixInjectBaseAttribute>();

                if (injectAttribute is NixInjectFromContainerAttribute)
                {
                    if (nonComponentField.IsComponent())
                        throw new TestInjecterException($"Cannot register field with name {nonComponentField.Name} with a NixInjectAttribute because it is a component field, you must use NixInjectComponentAttribute instead", objectToLink);
                }

                newInjectableData.AddField(nonComponentField);
            }
        }

        #region Component fields
        /// <summary>
        /// Create, populate and register all Component fields (decorated with class derived from NixInjectComponentBaseAttribute)
        /// <para/>If a field is of type MonoBehaviourInjectable, this will fill it in the same way as for this TestInjecter and register in tiContainer
        /// <para/>This is done recursively but all are stored in tiContainer
        /// </summary>
        /// <param name="componentFields">Component fields</param>
        /// <param name="newInjectableData">New MonoBehaviourInjectableData on which we are loading Component fields (newInjectableData.componentFields)</param>
        private void InjectComponentFields(IEnumerable<FieldInfo> componentFields, MonoBehaviourInjectableData newInjectableData)
        {
            foreach (FieldInfo componentField in componentFields)
            {
                CheckIfTypeIsGenericAndValid(newInjectableData, componentField);
                Type enumerableType = componentField.GetEnumerableUniqueGenericInterfaceOrComponentTypeIfExist();

                if (enumerableType != null)
                {
                    if (enumerableType.IsInterface)
                    {
                        StoreInterfaceComponentField(newInjectableData, componentField);
                    }
                    else
                    {
                        StoreEnumerableComponentField(newInjectableData, componentField, enumerableType);
                    }
                }
                else if (componentField.FieldType.IsInterface)
                {
                    StoreInterfaceComponentField(newInjectableData, componentField);
                }
                else
                {
                    StoreComponentField(newInjectableData, componentField);
                }
            }
        }

        /// <summary>
        /// Check if a FieldInfo.FieldType is a generic type and if so, if it is an IEnumerable and has exactly one generic argument if 
        /// </summary>
        /// <param name="newInjectableData">New MonoBehaviourInjectableData on which we are loading Component fields (newInjectableData.componentFields)</param>
        /// <param name="componentField">Component field checked</param>
        private void CheckIfTypeIsGenericAndValid(MonoBehaviourInjectableData newInjectableData, FieldInfo componentField)
        {
            if (componentField.FieldType.IsGenericType && !componentField.CheckIfGenericEnumerableWithOnlyOneGenericArgument())
                throw new TestInjecterException($"Cannot inject component field with name {componentField.Name} and type {componentField.FieldType.Name} on injectable with type {newInjectableData.Instance.GetType().Name} and instanceName {newInjectableData.InstanceName}", objectToLink);
        }

        /// <summary>
        /// Store the component field like a non-component field to expose field as a mockable like a standard interface
        /// <para/>This is useful when we want to work on interface with GetComponent in play mode, but it has the same behaviour
        /// as a mockable in the tests mode
        /// </summary>
        /// /// <param name="newInjectableData">New MonoBehaviourInjectableData on which we can store interface component field onto newInjectableData.nonComponentFields </param>
        /// <param name="interfaceComponentField">Component field as interface</param>
        private void StoreInterfaceComponentField(MonoBehaviourInjectableData newInjectableData, FieldInfo interfaceComponentField)
        {
            newInjectableData.AddField(interfaceComponentField);
        }

        /// <summary>
        /// Store the enumerable component field and fill it with empty IEnumerable
        /// </summary>
        /// <param name="newInjectableData">New MonoBehaviourInjectableData on which we are loading Component fields (newInjectableData.enumerableComponentFields)</param>
        /// <param name="enumerableField">Field where the IEnumerable content is stored</param>
        /// <param name="enumerableType">Generic type of the IEnumerable</param>
        private void StoreEnumerableComponentField(MonoBehaviourInjectableData newInjectableData, FieldInfo enumerableField, Type enumerableType)
        {
            newInjectableData.AddEnumerableComponentField(new ComponentListWithFieldInfo
            {
                FieldInfo = enumerableField,
                EnumerableType = enumerableType
            });
        }

        /// <summary>
        /// Fill a componentField with a component already instantiated if it is a MonoBehaviourInjectable with name already stored in InjectablesContainer
        /// <para/> If not, a new instance of Component is stored with name if it is precised in the attribute
        /// </summary>
        /// <param name="newInjectableData">New MonoBehaviourInjectableData on which we are loading Component fields (newInjectableData.componentFields)</param>
        /// <param name="componentField">Component field</param>
        private void StoreComponentField(MonoBehaviourInjectableData newInjectableData, FieldInfo componentField)
        {
            MonoBehaviourInjectable injectableFromName = GetInjectableFromNameToFind(componentField);

            if (injectableFromName != null)
            {
                FillFieldWithAlreadRegisteredComponentAndStoreInNewInjectableData(newInjectableData, componentField, injectableFromName);
            }
            else
            {
                if (!CheckAndInjectIfSameInstance(newInjectableData, componentField))
                {
                    PopulateAndRegisterComponentField(componentField, newInjectableData);
                    InjectAndStoreIfIsMonoBehaviourInjectable(componentField, newInjectableData, newInjectableData);
                }
            }
        }

        /// <summary>
        /// Based on Nixi component attribute decorator type, check if a component has been loaded previously on newInjectableData and inject it if found
        /// </summary>
        /// <param name="newInjectableData">New MonoBehaviourInjectableData on which we are loading fields</param>
        /// <param name="componentField">Component field</param>
        /// <returns>True if injected from instance found</returns>
        private bool CheckAndInjectIfSameInstance(MonoBehaviourInjectableData newInjectableData, FieldInfo componentField)
        {
            NixInjectComponentBaseAttribute baseAttribute = componentField.GetCustomAttribute<NixInjectComponentBaseAttribute>();

            if (baseAttribute is NixInjectComponentAttribute)
            {
                return CheckAndInjectIfAlreadyInjectedAtTopComponentLevel(newInjectableData, componentField);
            }
            else if (baseAttribute is NixInjectRootComponentAttribute rootAttribute)
            {
                return CheckAndInjectIfAlreadyInjectedInRootsComponent(newInjectableData, componentField, rootAttribute);
            }
            return false;
        }

        /// <summary>
        /// Check if a same level component with same type has been loaded previously on newInjectableData and inject it if found
        /// </summary>
        /// <param name="newInjectableData">New MonoBehaviourInjectableData on which we are loading fields</param>
        /// <param name="componentField">Component field</param>
        /// <returns>True if injected from instance found</returns>
        private bool CheckAndInjectIfAlreadyInjectedAtTopComponentLevel(MonoBehaviourInjectableData newInjectableData, FieldInfo componentField)
        {
            // Case where we have same level instance
            Component sameLevelComponent = newInjectableData.ComponentRelationHandler.GetSameLevelComponent(componentField.FieldType);

            // Not found, not filled
            if (sameLevelComponent != null)
            {
                FillFieldWithAlreadRegisteredComponentAndStoreInNewInjectableData(newInjectableData, componentField, sameLevelComponent);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if a root component with same type has been loaded previously on newInjectableData and inject it if found
        /// </summary>
        /// <param name="newInjectableData">New MonoBehaviourInjectableData on which we are loading fields</param>
        /// <param name="componentField">Component field</param>
        /// <param name="rootAttribute">RootComponent Nixi attribute decorator</param>
        /// <returns>True if injected from instance found</returns>
        private bool CheckAndInjectIfAlreadyInjectedInRootsComponent(MonoBehaviourInjectableData newInjectableData, FieldInfo componentField, NixInjectRootComponentAttribute rootAttribute)
        {
            // Component list on parent if SubComponentRootName (child) is empty, component list on child if not
            IEnumerable<Component> componentsInRootRelation = injectablesContainer.GetComponentsFromRelation(rootAttribute.ComponentRootName, rootAttribute.SubComponentRootName);

            // Case components found in rootRelation
            if (componentsInRootRelation != null && componentsInRootRelation.Any())
            {
                Component componentWithType = componentsInRootRelation.SingleOrDefault(x => x.GetType() == componentField.FieldType);

                if (componentWithType == null)
                {
                    // GameObject is the same on every components
                    GameObject gameObject = componentsInRootRelation.First().gameObject;
                    Component newComponent = gameObject.AddComponent(componentField.FieldType);
                    injectablesContainer.AddOrUpdateRootRelation(newComponent, rootAttribute.ComponentRootName, rootAttribute.SubComponentRootName);

                    // Fill with new built
                    newInjectableData.AddComponentField(new ComponentWithFieldInfo
                    {
                        Component = newComponent,
                        FieldInfo = componentField
                    });

                    componentField.SetValue(newInjectableData.Instance, newComponent);
                }
                else
                {
                    // Fill if found
                    newInjectableData.AddComponentField(new ComponentWithFieldInfo
                    {
                        Component = componentWithType,
                        FieldInfo = componentField
                    });
                    componentField.SetValue(newInjectableData.Instance, componentWithType);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Fill the Component field with root MonoBehaviourInjectable found and store it in new InjectableData
        /// </summary>
        /// <param name="newInjectableData">New MonoBehaviourInjectableData on which we are loading fields</param>
        /// <param name="componentField">Component field</param>
        /// <param name="component">Component instance used to fill componentField</param>
        private static void FillFieldWithAlreadRegisteredComponentAndStoreInNewInjectableData(MonoBehaviourInjectableData newInjectableData, FieldInfo componentField, Component component)
        {
            componentField.SetValue(newInjectableData.Instance, component);
            newInjectableData.AddComponentField(new ComponentWithFieldInfo
            {
                Component = component,
                FieldInfo = componentField
            });
        }

        /// <summary>
        /// Return injectable if componentField is associated to a name, null if is not the case, it is helpful for handling rootComponent
        /// </summary>
        /// <param name="componentField">Component field to analyze</param>
        /// <returns>Null if not found or if componentField Nix attribute does not implements IHaveComponentNameToFind</returns>
        private MonoBehaviourInjectable GetInjectableFromNameToFind(FieldInfo componentField)
        {
            string componentNameToFind = GetComponentNameIfExists(componentField);

            if (string.IsNullOrEmpty(componentNameToFind))
                return null;
            
            MonoBehaviourInjectableData injectableData = injectablesContainer.GetInjectable(componentNameToFind, componentField.FieldType);
            return injectableData?.Instance;
        }

        /// <summary>
        /// Create, populate and register a Component field (decorated with a class derived from NixInjectComponentBaseAttribute) into componentFieldInjectionHandler
        /// </summary>
        /// <param name="componentField">Component field</param>
        /// <param name="injectableData">New MonoBehaviourInjectableData on which we are loading fields</param>
        private void PopulateAndRegisterComponentField(FieldInfo componentField, MonoBehaviourInjectableData injectableData)
        {
            if (!componentField.IsComponent())
                throw new TestInjecterException($"Cannot inject field with name {componentField.Name} with a NixInjectComponentAttribute because it is not a component field, you must use NixInjectAttribute instead", objectToLink);

            Component componentToAdd = BuildAndInjectComponent(componentField, injectableData);
            injectableData.AddComponentField(new ComponentWithFieldInfo
            {
                Component = componentToAdd,
                FieldInfo = componentField
            });

            StoreRootComponentIntoInjectableContainer(componentField, componentToAdd);
        }

        /// <summary>
        /// Store component into root relations from injectable container if not already filled
        /// </summary>
        /// <param name="componentField">Component field</param>
        /// <param name="componentToAdd">New component created</param>
        private void StoreRootComponentIntoInjectableContainer(FieldInfo componentField, Component componentToAdd)
        {
            NixInjectRootComponentAttribute rootAttribute = componentField.GetCustomAttribute<NixInjectRootComponentAttribute>();
            if (rootAttribute != null)
            {
                Component existingComponent = injectablesContainer.GetComponentFromRelation(componentField.FieldType, rootAttribute.ComponentRootName, rootAttribute.SubComponentRootName);
                if (existingComponent == null)
                {
                    injectablesContainer.AddOrUpdateRootRelation(componentToAdd, rootAttribute.ComponentRootName, rootAttribute.SubComponentRootName);
                }
            }
        }

        /// <summary>
        /// Build a component of type contained in componentField with the data contained in monoBehaviourInjectAttribute, then fill componentField with this instance
        /// </summary>
        /// <param name="componentField">Component field</param>
        /// <returns>Component instantiated</returns>
        private Component BuildAndInjectComponent(FieldInfo componentField, MonoBehaviourInjectableData injectableData)
        {
            string componentName = GetComponentNameIfExists(componentField);

            GameObject gameObjectToAdd;

            if (componentField.FieldType.IsAssignableFrom(typeof(Transform)))
            {
                // Build simple game object and get his transform (cannot addComponent<Transform> this is a non sense for a gameObject which automatically add it)
                gameObjectToAdd = new GameObject(componentName);
                componentField.SetValue(injectableData.Instance, gameObjectToAdd.transform);
            }
            else
            {
                // Standard GameObject build
                gameObjectToAdd = new GameObject(componentName, componentField.FieldType);
                Component componentToRetrieve = gameObjectToAdd.GetComponent(componentField.FieldType);
                componentField.SetValue(injectableData.Instance, componentToRetrieve);
            }
            
            return gameObjectToAdd.GetComponent(componentField.FieldType);
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
        /// <param name="newInjectableData">MonoBehaviourInjectableData to check if instance is MonoBehaviourInjectable</param>
        private void InjectAndStoreIfIsMonoBehaviourInjectable(FieldInfo componentField, MonoBehaviourInjectableData oldInjectableData, MonoBehaviourInjectableData newInjectableData)
        {
            if (componentField.GetValue(newInjectableData.Instance) is MonoBehaviourInjectable monoBehaviourInjectable)
            {
                CheckInfiniteRecursion(componentField, newInjectableData);

                string componentName = GetComponentNameIfExists(componentField);

                InjectMonoBehaviourInjectable(monoBehaviourInjectable, oldInjectableData, componentName);
            }
        }

        /// <summary>
        /// Checks if injecting a MonoBehaviourInjectable into objectToLink does not cause an infinite injection loop of itself
        /// </summary>
        /// <param name="componentField">Component field to check</param>
        private void CheckInfiniteRecursion(FieldInfo componentField, MonoBehaviourInjectableData testInjecter)
        {
            Type typeToCheck = componentField.FieldType.DeclaringType ?? componentField.FieldType;
            Type currentType = testInjecter.Instance.GetType();

            if (typeToCheck.IsAssignableFrom(currentType))
            {
                throw new StackOverflowException($"Infinite recursion detected on the field with name {componentField.Name}" +
                    $" and with type {componentField.FieldType} which has a type identical or inherited from objectToLink type" +
                    $" which has name {objectToLink.name} and type {currentType.Name}");
            }
        }
        #endregion Recursion

        #region Mocks and GetComponents
        /// <summary>
        /// Inject manually a mock into the Non-Component type T field
        /// <para/>If no targetInjectable precised, it targets objectToLink by default
        /// <para/>If multiple type T fields are found, you must use InjectMock(Mock mockToInject, string fieldName)
        /// </summary>
        /// <typeparam name="T">Targeted field type</typeparam>
        /// <param name="mockToInject">Mock to inject into field</param>
        /// <param name="targetInjectable">Optional parameter, if null it targets objectToLink by default, if filled it must be a MonoBehaviourInjectable recursively injected and obtained from GetComponent</param>
        public void InjectMock<T>(T mockToInject, MonoBehaviourInjectable targetInjectable = null)
        {
            try
            {
                injectablesContainer.InjectMockIntoInstance(mockToInject, targetInjectable ?? objectToLink);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot InjectMock because {e.Message}", objectToLink);
            }
        }

        /// <summary>
        /// Inject manually a mock into the Non-Component type T field and with the fieldName passed as a parameter
        /// <para/>If no targetInjectable precised, it targets objectToLink by default
        /// </summary>
        /// <typeparam name="T">Targeted field type</typeparam>
        /// <param name="fieldName">Name of the field to mock</param>
        /// <param name="mockToInject">Mock to inject into field</param>
        /// <param name="targetInjectable">Optional parameter, if null it targets objectToLink by default, if filled it must be a MonoBehaviourInjectable recursively injected and obtained from GetComponent</param>
        public void InjectMock<T>(T mockToInject, string fieldName, MonoBehaviourInjectable targetInjectable = null)
        {
            try
            {
                injectablesContainer.InjectMockIntoInstance(mockToInject, targetInjectable ?? objectToLink, fieldName);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot InjectMock because {e.Message}", objectToLink);
            }
        }

        /// <summary>
        /// Return the only component with Component type T field
        /// <para/>If no targetInjectable precised, it targets objectToLink by default
        /// <para/>If multiple type T fields are found, you must use GetComponent<T>(string fieldName) 
        /// </summary>
        /// <typeparam name="T">Type of Component searched</typeparam>
        /// <param name="targetInjectable">Optional parameter, if null it targets objectToLink by default, if filled it must be a MonoBehaviourInjectable recursively injected and obtained from GetComponent</param>
        /// <returns>Single component which corresponds to the type T field</returns>
        public T GetComponent<T>(MonoBehaviourInjectable targetInjectable = null)
            where T : Component
        {
            try
            {
                return injectablesContainer.GetComponentFromInstance<T>(targetInjectable ?? objectToLink);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot GetComponent because {e.Message}", objectToLink);
            }
        }

        /// <summary>
        /// Return the only component with Component type T field and with the fieldName passed as a parameter
        /// <para/>If no targetInjectable precised, it targets objectToLink by default
        /// </summary>
        /// <typeparam name="T">Type of Component searched</typeparam>
        /// <param name="fieldName">Name of the field searched</param>
        /// <param name="targetInjectable">Optional parameter, if null it targets objectToLink by default, if filled it must be a MonoBehaviourInjectable recursively injected and obtained from GetComponent</param>
        /// <returns>Single component which corresponds to the type T field and fieldName passed as a parameter</returns>
        public T GetComponent<T>(string fieldName, MonoBehaviourInjectable targetInjectable = null)
            where T : Component
        {
            try
            {
                return injectablesContainer.GetComponentFromInstance<T>(targetInjectable ?? objectToLink, fieldName);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot GetComponent because {e.Message}", objectToLink);
            }
        }

        #region ComponentList
        /// <summary>
        /// Add an element in the list of components instantiated with Enumerable Component type T field
        /// <para/>If no targetInjectable precised, it targets objectToLink by default
        /// <para/>If multiple type T fields are found, you must use AddComponent<T>(string fieldName) 
        /// </summary>
        /// <typeparam name="T">Type of Component searched</typeparam>
        /// <param name="targetInjectable">Optional parameter, if null it targets objectToLink by default, if filled it must be a MonoBehaviourInjectable recursively injected and obtained from GetComponent</param>
        /// <returns>New component instantiated which corresponds to the type T field</returns>
        public T AddInComponentList<T>(MonoBehaviourInjectable targetInjectable = null)
            where T : Component
        {
            try
            {
                targetInjectable ??= objectToLink;

                T component = injectablesContainer.AddInComponentList<T>(targetInjectable);

                var oldInjectable = injectablesContainer.GetInjectable(targetInjectable);

                if (component is MonoBehaviourInjectable monoBehaviourInjectable)
                {
                    InjectMonoBehaviourInjectable(monoBehaviourInjectable, oldInjectable);
                }

                return component;
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot GetComponentList because {e.Message}", objectToLink);
            }
        }

        /// <summary>
        /// Return list of components instantiated with Enumerable Component type field
        /// <para/>If no targetInjectable precised, it targets objectToLink by default
        /// <para/>If multiple type T fields are found, you must use GetComponent<T>(string fieldName) 
        /// </summary>
        /// <typeparam name="T">Type of Component searched</typeparam>
        /// <param name="targetInjectable">Optional parameter, if null it targets objectToLink by default, if filled it must be a MonoBehaviourInjectable recursively injected and obtained from GetComponent</param>
        /// <returns>List of component instantiated which corresponds to the type T field</returns>
        public IEnumerable<T> GetComponentList<T>(MonoBehaviourInjectable targetInjectable = null)
            where T : Component
        {
            try
            {
                return injectablesContainer.GetComponentListFromInstance<T>(targetInjectable ?? objectToLink);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot GetComponentList because {e.Message}", objectToLink);
            }
        }
        #endregion ComponentList
        #endregion Mocks and GetComponents
    }
}
