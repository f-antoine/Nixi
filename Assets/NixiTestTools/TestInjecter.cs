using Nixi.Injections;
using Nixi.Injections.Attributes.Fields;
using Nixi.Injections.Attributes.ComponentFields.Abstractions;
using Nixi.Injections.Injecters;
using NixiTestTools.TestInjecterElements;
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
        /// Container to handle TestInjecter instances, it is used to injectMock into a field or GetComponent from fields instantiated during the Test injections
        /// </summary>
        private InjectablesContainer injectablesContainer = new InjectablesContainer();

        /// <summary>
        /// ObjectToLink instance name to used in GetRootInjectable and avoid duplicate if there is mutual injection
        /// </summary>
        private string InstanceName = null;

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
            InstanceName = instanceName;
        }

        /// <summary>
        /// Inject all the fields decorated by NixInjectAttribute or NixInjectComponentAttribute in objectToLink
        /// </summary>
        /// <exception cref="TestInjecterException">Thrown if this method has already been called</exception>
        protected override void InjectAll()
        {
            InjectMonoBehaviourInjectable(objectToLink, InstanceName);
        }

        /// <summary>
        /// Inject all the nixi fields of a MonoBehaviourInjectable for testing
        /// </summary>
        /// <param name="monoBehaviourInjectable">MonoBehaviourInjectable to inject</param>
        /// <param name="instanceName">Name of the instance</param>
        private void InjectMonoBehaviourInjectable(MonoBehaviourInjectable monoBehaviourInjectable, string instanceName = "")
        {
            MonoBehaviourInjectableData newInjectableData = AddMonoBehaviourInjectable(monoBehaviourInjectable, instanceName);

            List<FieldInfo> fields = GetAllFields(monoBehaviourInjectable.GetType());
            InjectFields(fields.Where(NixiFieldPredicate), newInjectableData);
            InjectComponentFields(fields.Where(NixiComponentFieldPredicate), newInjectableData);
        }

        /// <summary>
        /// If instance with instanceName has already been registered, return MonoBehaviourInjectableData associated to these parameters
        /// <para/> If not, store a MonoBehaviourInjectable on which operations like InjectMock or GetComponent can be executed
        /// </summary>
        /// <param name="monoBehaviourInjectable">MonoBehaviourInjectable to inject</param>
        /// <param name="instanceName">Name of the instance</param>
        /// <returns>MonoBehaviourInjectableData if it was added successfully</returns>
        private MonoBehaviourInjectableData AddMonoBehaviourInjectable(MonoBehaviourInjectable monoBehaviourInjectable, string instanceName)
        {
            return injectablesContainer.Add(monoBehaviourInjectable, instanceName);
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
                NixInjectAttribute injectAttribute = nonComponentField.GetCustomAttribute<NixInjectAttribute>();

                if (injectAttribute.NixInjectType != NixInjectType.DoesNotFillButExposeForTesting)
                {
                    CheckIsNotComponent(nonComponentField);
                }
                
                newInjectableData.nonComponentFields.Add(nonComponentField);
            }
        }

        #region Component fields
        /// <summary>
        /// Create, populate and register all Component fields (decorated with class dervied from NixInjectComponentBaseAttribute) into testInjecter.componentFieldInjectionHandler
        /// <para/>If a field is of type MonoBehaviourInjectable, this will fill it in the same way as for this TestInjecter and register in tiContainer
        /// <para/>This is done recursively but all are stored in tiContainer
        /// </summary>
        /// <param name="componentFields">Component fields</param>
        /// <param name="newInjectableData">New MonoBehaviourInjectableData on which we are loading Component fields (newInjectableData.componentFields)</param>
        private void InjectComponentFields(IEnumerable<FieldInfo> componentFields, MonoBehaviourInjectableData newInjectableData)
        {
            foreach (FieldInfo componentField in componentFields)
            {
                MonoBehaviourInjectable injectableFromName = GetInjectableFromNameToFind(componentField);

                if (injectableFromName != null)
                {
                    FillFieldWithAlreadRegisteredGameObjectAndStoreInNewInjectableData(newInjectableData, componentField, injectableFromName);
                }
                else
                {
                    PopulateAndRegistercomponentField(componentField, newInjectableData);
                    InjectAndStoreIfIsMonoBehaviourInjectable(componentField, newInjectableData);
                }
            }
        }

        /// <summary>
        /// Fill the Component field with root MonoBehaviourInjectable found and store is in new InjectableData
        /// </summary>
        /// <param name="newInjectableData">New MonoBehaviourInjectableData on which we are loading Component fields</param>
        /// <param name="componentField">Component field</param>
        /// <param name="registeredInjectable">Root GameObject used to fill componentField</param>
        private static void FillFieldWithAlreadRegisteredGameObjectAndStoreInNewInjectableData(MonoBehaviourInjectableData newInjectableData, FieldInfo componentField, MonoBehaviourInjectable registeredInjectable)
        {
            componentField.SetValue(newInjectableData.Instance, registeredInjectable);
            newInjectableData.componentWithFieldInstantiated.Add(new GameObjectWithFieldInfo
            {
                GameObject = registeredInjectable.gameObject,
                FieldInfo = componentField
            });
        }

        /// <summary>
        /// Return the Root GameObject with instanceName = NixInjectComponentFromMethodRootAttribute.RootGameObjectName
        /// </summary>
        /// <param name="componentField">Component field to analyze</param>
        /// <returns>Null if not found or if componentField is not NixInjectComponentFromMethodRootAttribute</returns>
        private MonoBehaviourInjectable GetInjectableFromNameToFind(FieldInfo componentField)
        {
            string gameObjectNameToFind = GetGameObjectNameIfExists(componentField);

            if (string.IsNullOrEmpty(gameObjectNameToFind))
                return null;
            
            MonoBehaviourInjectableData injectableData = injectablesContainer.GetInjectable(gameObjectNameToFind, componentField.FieldType);
            return injectableData?.Instance;
        }

        /// <summary>
        /// Create, populate and register a Component field (decorated with a class derived from NixInjectComponentBaseAttribute) into componentFieldInjectionHandler
        /// </summary>
        /// <param name="componentField">Component field</param>
        private void PopulateAndRegistercomponentField(FieldInfo componentField, MonoBehaviourInjectableData injectableData)
        {
            CheckIsComponent(componentField);

            GameObject gameObjectToAdd = BuildAndInjectGameObject(componentField, injectableData);
            injectableData.componentWithFieldInstantiated.Add(new GameObjectWithFieldInfo
            {
                GameObject = gameObjectToAdd,
                FieldInfo = componentField
            });
        }

        /// <summary>
        /// Build a GameObject of type contained in componentField with the data contained in monoBehaviourInjectAttribute, then fill componentField with this instance
        /// </summary>
        /// <param name="componentField">Component field</param>
        /// <returns>GameObject instantiated</returns>
        private GameObject BuildAndInjectGameObject(FieldInfo componentField, MonoBehaviourInjectableData injectableData)
        {
            string gameObjectName = GetGameObjectNameIfExists(componentField);

            GameObject gameObjectToAdd = new GameObject(gameObjectName, componentField.FieldType);
            Component componentToRetrieve = gameObjectToAdd.GetComponent(componentField.FieldType);
            componentField.SetValue(injectableData.Instance, componentToRetrieve);
            
            return gameObjectToAdd;
        }

        /// <summary>
        /// If monoBehaviourInjectAttribute implements IHaveGameObjectNameToFind, it returns the GameObjectNameToFind value, if not it return string.Empty
        /// <para/>This will be treated in TestInjecter which link GameObjectName with TestInjecter.GetComponent to simplify access with GameObject name in tests
        /// </summary>
        /// <param name="componentField">Component field</param>
        /// <returns>GameObjectNameToFind if available, string.Empty if not</returns>
        private static string GetGameObjectNameIfExists(FieldInfo componentField)
        {
            NixInjectComponentBaseAttribute injectAttribute = componentField.GetCustomAttribute<NixInjectComponentBaseAttribute>();
            if (injectAttribute is IHaveGameObjectNameToFind gameObjectNameToFindInstance)
            {
                return gameObjectNameToFindInstance.GameObjectNameToFind;
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
        /// <param name="injectableData">MonoBehaviourInjectableData to check if instance is MonoBehaviourInjectable</param>
        private void InjectAndStoreIfIsMonoBehaviourInjectable(FieldInfo componentField, MonoBehaviourInjectableData injectableData)
        {
            if (componentField.GetValue(injectableData.Instance) is MonoBehaviourInjectable monoBehaviourInjectable)
            {
                CheckInfiniteRecursion(componentField, injectableData);

                string gameObjectName = GetGameObjectNameIfExists(componentField);

                InjectMonoBehaviourInjectable(monoBehaviourInjectable, gameObjectName);
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
        /// <para/>If multiple type T fields are found, you must use InjectMock(Mock mockToInject, string fieldName)
        /// </summary>
        /// <typeparam name="T">Targeted field type</typeparam>
        /// <param name="mockToInject">Mock to inject into field</param>
        public void InjectMock<T>(T mockToInject)
            where T : class
        {
            try
            {
                injectablesContainer.InjectMockIntoInstance(mockToInject, objectToLink);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot InjectMock because {e.Message}", objectToLink);
            }
        }

        /// <summary>
        /// Inject manually a mock into the Non-Component type T field and with the fieldName passed as a parameter
        /// </summary>
        /// <typeparam name="T">Targeted field type</typeparam>
        /// <param name="fieldName">Name of the field to mock</param>
        /// <param name="mockToInject">Mock to inject into field</param>
        public void InjectMock<T>(T mockToInject, string fieldName)
            where T : class
        {
            try
            {
                injectablesContainer.InjectMockIntoInstance(mockToInject, objectToLink, fieldName);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot InjectMock because {e.Message}", objectToLink);
            }
        }

        /// <summary>
        /// Return the only component with Component type T field
        /// <para/>If multiple type T fields are found, you must use GetComponent<T>(string fieldName) 
        /// </summary>
        /// <typeparam name="T">Type of Component searched</typeparam>
        /// <returns>Single component which corresponds to the type T field</returns>
        public T GetComponent<T>()
            where T : Component
        {
            try
            {
                return injectablesContainer.GetComponentFromInstance<T>(objectToLink);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot GetComponent because {e.Message}", objectToLink);
            }
        }

        /// <summary>
        /// Return the only component with Component type T field and with the fieldName passed as a parameter
        /// </summary>
        /// <typeparam name="T">Type of Component searched</typeparam>
        /// <param name="fieldName">Name of the field searched</param>
        /// <returns>Single component which corresponds to the type T field and fieldName passed as a parameter</returns>
        public T GetComponent<T>(string fieldName)
            where T : Component
        {
            try
            {
                return injectablesContainer.GetComponentFromInstance<T>(objectToLink, fieldName);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot GetComponent because {e.Message}", objectToLink);
            }
        }
        #endregion Mocks and GetComponents

        #region Mocks and GetComponents ChildInjected
        /// <summary>
        /// From the MonoBehaviourInjectable recursively injected
        /// <para/>Inject manually a mock into the Non-Component type T field
        /// <para/>If multiple type T fields are found, you must use InjectMockIntoChildInjected(Mock mockToInject, string fieldName)
        /// </summary>
        /// <typeparam name="T">Targeted field type</typeparam>
        /// <param name="mockToInject">Mock to inject into field</param>
        /// <param name="monoBehaviourInjectableRecursivelyInjected">MonoBehaviourInjectable recursively injected (contained in childInjecter)</param>
        /// <exception cref="TestInjecterException">Thrown if instance not found which means monoBehaviourInjectableRecursivelyInjected does not contain a field</exception>
        public void InjectMockIntoChildInjected<T>(T mockToInject, MonoBehaviourInjectable monoBehaviourInjectableRecursivelyInjected)
            where T : class
        {
            try
            {
                injectablesContainer.InjectMockIntoInstance(mockToInject, monoBehaviourInjectableRecursivelyInjected);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot InjectMockIntoChildInjected because {e.Message}", objectToLink);
            }
        }

        /// <summary>
        /// From the MonoBehaviourInjectable recursively injected
        /// <para/>Inject manually a mock into the Non-Component type T field and with the fieldName passed as a parameter
        /// </summary>
        /// <typeparam name="T">Targeted field type</typeparam>
        /// <param name="mockToInject">Mock to inject into field</param>
        /// <param name="fieldName">Name of the field to mock</param>
        /// <param name="monoBehaviourInjectableRecursivelyInjected">MonoBehaviourInjectable recursively injected (contained in childInjecter)</param>
        /// <exception cref="TestInjecterException">Thrown if instance not found which means monoBehaviourInjectableRecursivelyInjected does not contain a field</exception>
        public void InjectMockIntoChildInjected<T>(T mockToInject, MonoBehaviourInjectable monoBehaviourInjectableRecursivelyInjected, string fieldName)
            where T : class
        {
            try
            {
                injectablesContainer.InjectMockIntoInstance(mockToInject, monoBehaviourInjectableRecursivelyInjected, fieldName);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot InjectMockIntoChildInjected because {e.Message}", objectToLink);
            }
        }

        /// <summary>
        /// From the MonoBehaviourInjectable recursively injected
        /// <para/>Return the only component with Component type T field
        /// <para/>If multiple type T fields are found, you must use GetComponent(string fieldName) 
        /// </summary>
        /// <typeparam name="T">Type of Component searched</typeparam>
        /// <param name="monoBehaviourInjectableRecursivelyInjected">MonoBehaviourInjectable recursively injected (contained in childInjecter)</param>
        /// <returns>Single component which corresponds to the type T field</returns>
        /// <exception cref="TestInjecterException">Thrown if instance not found which means monoBehaviourInjectableRecursivelyInjected does not contain a field</exception>
        public T GetComponentFromChildInjected<T>(MonoBehaviourInjectable monoBehaviourInjectableRecursivelyInjected)
            where T : Component
        {
            try
            {
                return injectablesContainer.GetComponentFromInstance<T>(monoBehaviourInjectableRecursivelyInjected);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot GetComponentFromChildInjected because {e.Message}", objectToLink);
            }

        }

        /// <summary>
        /// From the MonoBehaviourInjectable recursively injected
        /// <para/>Return the only component with Component type T field and with the fieldName passed as a parameter
        /// </summary>
        /// <typeparam name="T">Type of Component searched</typeparam>
        /// <param name="monoBehaviourInjectableRecursivelyInjected">MonoBehaviourInjectable recursively injected (contained in childInjecter)</param>
        /// <param name="fieldName">Name of the field searched</param>
        /// <returns>Single component which corresponds to the type T field and fieldName passed as a parameter</returns>
        /// <exception cref="TestInjecterException">Thrown if instance not found which means monoBehaviourInjectableRecursivelyInjected does not contain a field</exception>
        public T GetComponentFromChildInjected<T>(MonoBehaviourInjectable monoBehaviourInjectableRecursivelyInjected, string fieldName)
            where T : Component
        {
            try
            {
                return injectablesContainer.GetComponentFromInstance<T>(monoBehaviourInjectableRecursivelyInjected, fieldName);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot GetComponentFromChildInjected because {e.Message}", objectToLink);
            }
        }
        #endregion Mocks and GetComponents ChildInjected
    }
}
