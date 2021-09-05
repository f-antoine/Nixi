using Nixi.Injections;
using Nixi.Injections.Attributes.MonoBehaviours.Abstractions;
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
    ///         <term>MonoBehaviour fields</term>
    ///         <description>Marked with NixInjectMonoBehaviourAttribute will be created, used to populate the field and registered in TestInjecter property of InjectionTestTemplate</description>
    ///     </item>
    ///     <item>
    ///         <term>Non-MonoBehaviour fields</term>
    ///         <description>Marked with NixInjectAttribute will be mockable, you can manually inject mock in it (values are null if not populated by a manual non-MonoBehaviour field injection)</description>
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
        ///         <term>MonoBehaviour fields</term>
        ///         <description>Marked with NixInjectMonoBehaviourAttribute will be created, used to populate the field and registered in TestInjecter property of InjectionTestTemplate</description>
        ///     </item>
        ///     <item>
        ///         <term>Non-MonoBehaviour fields</term>
        ///         <description>Marked with NixInjectAttribute will be mockable, you can manually inject mock in it (values are null if not populated by a manual non-MonoBehaviour field injection)</description>
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
        /// Inject all the fields decorated by NixInjectAttribute or NixInjectMonoBehaviourAttribute in objectToLink
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
            InjectMonoBehaviourFields(fields.Where(NixiMonoBehaviourFieldPredicate), newInjectableData);
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
        /// Register all non-MonoBehaviour fields (decorated with NixInjectAttribute) into fieldInjectionHandler to expose them as mockable
        /// <para/> These fields values are null if not populated by a manual non-MonoBehaviour field injection
        /// </summary>
        /// <param name="nonMonoBehaviourFields">Non-MonoBehaviour fields</param>
        /// <param name="newInjectableData">New MonoBehaviourInjectableData on which we are loading non-MonoBehaviour fields (newInjectableData.nonMonoBehaviourFields)</param>
        private void InjectFields(IEnumerable<FieldInfo> nonMonoBehaviourFields, MonoBehaviourInjectableData newInjectableData)
        {
            foreach (FieldInfo nonMonoBehaviourField in nonMonoBehaviourFields)
            {
                CheckIsNotMonoBehaviour(nonMonoBehaviourField);
                newInjectableData.nonMonoBehaviourFields.Add(nonMonoBehaviourField);
            }
        }

        #region MonoBehaviour fields
        /// <summary>
        /// Create, populate and register all MonoBehaviour fields (decorated with class dervied from NixInjectMonoBehaviourBaseAttribute) into testInjecter.monoBehaviourFieldInjectionHandler
        /// <para/>If a field is of type MonoBehaviourInjectable, this will fill it in the same way as for this TestInjecter and register in tiContainer
        /// <para/>This is done recursively but all are stored in tiContainer
        /// </summary>
        /// <param name="monoBehaviourFields">MonoBehaviour fields</param>
        /// <param name="newInjectableData">New MonoBehaviourInjectableData on which we are loading MonoBehaviour fields (newInjectableData.MonoBehaviourFields)</param>
        private void InjectMonoBehaviourFields(IEnumerable<FieldInfo> monoBehaviourFields, MonoBehaviourInjectableData newInjectableData)
        {
            foreach (FieldInfo monoBehaviourField in monoBehaviourFields)
            {
                MonoBehaviourInjectable injectableFromName = GetInjectableFromNameToFind(monoBehaviourField);

                if (injectableFromName != null)
                {
                    FillFieldWithAlreadRegisteredGameObjectAndStoreInNewInjectableData(newInjectableData, monoBehaviourField, injectableFromName);
                }
                else
                {
                    PopulateAndRegisterMonoBehaviourField(monoBehaviourField, newInjectableData);
                    InjectAndStoreIfIsMonoBehaviourInjectable(monoBehaviourField, newInjectableData);
                }
            }
        }

        /// <summary>
        /// Fill the MonoBehaviour field with root MonoBehaviourInjectable found and store is in new InjectableData
        /// </summary>
        /// <param name="newInjectableData">New MonoBehaviourInjectableData on which we are loading MonoBehaviour fields</param>
        /// <param name="monoBehaviourField">MonoBehaviour field</param>
        /// <param name="registeredInjectable">Root GameObject used to fill monoBehaviourField</param>
        private static void FillFieldWithAlreadRegisteredGameObjectAndStoreInNewInjectableData(MonoBehaviourInjectableData newInjectableData, FieldInfo monoBehaviourField, MonoBehaviourInjectable registeredInjectable)
        {
            monoBehaviourField.SetValue(newInjectableData.Instance, registeredInjectable);
            newInjectableData.monoBehaviourWithFieldInstantiated.Add(new GameObjectWithFieldInfo
            {
                GameObject = registeredInjectable.gameObject,
                FieldInfo = monoBehaviourField
            });
        }

        /// <summary>
        /// Return the Root GameObject with instanceName = NixInjectMonoBehaviourFromMethodRootAttribute.RootGameObjectName
        /// </summary>
        /// <param name="monoBehaviourField">MonoBehaviour field to analyze</param>
        /// <returns>Null if not found or if monoBehaviourField is not NixInjectMonoBehaviourFromMethodRootAttribute</returns>
        private MonoBehaviourInjectable GetInjectableFromNameToFind(FieldInfo monoBehaviourField)
        {
            string gameObjectNameToFind = GetGameObjectNameIfExists(monoBehaviourField);

            if (string.IsNullOrEmpty(gameObjectNameToFind))
                return null;
            
            MonoBehaviourInjectableData injectableData = injectablesContainer.GetInjectable(gameObjectNameToFind, monoBehaviourField.FieldType);
            return injectableData?.Instance;
        }

        /// <summary>
        /// Create, populate and register a MonoBehaviour field (decorated with a class derived from NixInjectMonoBehaviourBaseAttribute) into monoBehaviourFieldInjectionHandler
        /// </summary>
        /// <param name="monoBehaviourField">MonoBehaviour field</param>
        private void PopulateAndRegisterMonoBehaviourField(FieldInfo monoBehaviourField, MonoBehaviourInjectableData injectableData)
        {
            CheckIsMonoBehaviour(monoBehaviourField);

            GameObject gameObjectToAdd = BuildAndInjectGameObject(monoBehaviourField, injectableData);
            injectableData.monoBehaviourWithFieldInstantiated.Add(new GameObjectWithFieldInfo
            {
                GameObject = gameObjectToAdd,
                FieldInfo = monoBehaviourField
            });
        }

        /// <summary>
        /// Build a GameObject of type contained in monoBehaviourField with the data contained in monoBehaviourInjectAttribute, then fill monoBehaviourField with this instance
        /// </summary>
        /// <param name="monoBehaviourField">MonoBehaviour field</param>
        /// <returns>GameObject instantiated</returns>
        private GameObject BuildAndInjectGameObject(FieldInfo monoBehaviourField, MonoBehaviourInjectableData injectableData)
        {
            string gameObjectName = GetGameObjectNameIfExists(monoBehaviourField);

            GameObject gameObjectToAdd = new GameObject(gameObjectName, monoBehaviourField.FieldType);
            Component componentToRetrieve = gameObjectToAdd.GetComponent(monoBehaviourField.FieldType);
            monoBehaviourField.SetValue(injectableData.Instance, componentToRetrieve);
            
            return gameObjectToAdd;
        }

        /// <summary>
        /// If monoBehaviourInjectAttribute implements IHaveGameObjectNameToFind, it returns the GameObjectNameToFind value, if not it return string.Empty
        /// <para/>This will be treated in TestInjecter which link GameObjectName with TestInjecter.GetComponent to simplify access with GameObject name in tests
        /// </summary>
        /// <param name="monoBehaviourField">MonoBehaviour field</param>
        /// <returns>GameObjectNameToFind if available, string.Empty if not</returns>
        private static string GetGameObjectNameIfExists(FieldInfo monoBehaviourField)
        {
            NixInjectMonoBehaviourBaseAttribute injectAttribute = monoBehaviourField.GetCustomAttribute<NixInjectMonoBehaviourBaseAttribute>();
            if (injectAttribute is IHaveGameObjectNameToFind gameObjectNameToFindInstance)
            {
                return gameObjectNameToFindInstance.GameObjectNameToFind;
            }
            return string.Empty;
        }
        #endregion MonoBehaviour fields

        #region Recursion
        /// <summary>
        /// Checks if the monoBehaviourField is MonoBehaviourInjectable and inject it the same way as for this TestInjecter but will be stored as the childInjecter of this main TestInjecter
        /// <para/>This is done recursively but all are stored in the same list of childInjecters of this main TestInjecter
        /// </summary>
        /// <param name="monoBehaviourField">MonoBehaviour field</param>
        /// <param name="injectableData">MonoBehaviourInjectableData to check if instance is MonoBehaviourInjectable</param>
        private void InjectAndStoreIfIsMonoBehaviourInjectable(FieldInfo monoBehaviourField, MonoBehaviourInjectableData injectableData)
        {
            if (monoBehaviourField.GetValue(injectableData.Instance) is MonoBehaviourInjectable monoBehaviourInjectable)
            {
                CheckInfiniteRecursion(monoBehaviourField, injectableData);

                string gameObjectName = GetGameObjectNameIfExists(monoBehaviourField);

                InjectMonoBehaviourInjectable(monoBehaviourInjectable, gameObjectName);
            }
        }

        /// <summary>
        /// Checks if injecting a MonoBehaviourInjectable into objectToLink does not cause an infinite injection loop of itself
        /// </summary>
        /// <param name="monoBehaviourField">MonoBehaviour field to check</param>
        private void CheckInfiniteRecursion(FieldInfo monoBehaviourField, MonoBehaviourInjectableData testInjecter)
        {
            Type typeToCheck = monoBehaviourField.FieldType.DeclaringType ?? monoBehaviourField.FieldType;
            Type currentType = testInjecter.Instance.GetType();

            if (typeToCheck.IsAssignableFrom(currentType))
            {
                throw new StackOverflowException($"Infinite recursion detected on the field with name {monoBehaviourField.Name}" +
                    $" and with type {monoBehaviourField.FieldType} which has a type identical or inherited from objectToLink type" +
                    $" which has name {objectToLink.name} and type {currentType.Name}");
            }
        }
        #endregion Recursion

        #region Mocks and GetComponents
        /// <summary>
        /// Inject manually a mock into the non-MonoBehaviour type T field
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
        /// Inject manually a mock into the non-MonoBehaviour type T field and with the fieldName passed as a parameter
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
        /// Return the only component with MonoBehaviour type T field
        /// <para/>If multiple type T fields are found, you must use GetComponent<T>(string fieldName) 
        /// </summary>
        /// <typeparam name="T">Type of MonoBehaviour searched</typeparam>
        /// <returns>Single component which corresponds to the type T field</returns>
        public T GetComponent<T>()
            where T : MonoBehaviour
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
        /// Return the only component with MonoBehaviour type T field and with the fieldName passed as a parameter
        /// </summary>
        /// <typeparam name="T">Type of MonoBehaviour searched</typeparam>
        /// <param name="fieldName">Name of the field searched</param>
        /// <returns>Single component which corresponds to the type T field and fieldName passed as a parameter</returns>
        public T GetComponent<T>(string fieldName)
            where T : MonoBehaviour
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
        /// <para/>Inject manually a mock into the non-MonoBehaviour type T field
        /// <para/>If multiple type T fields are found, you must use InjectMockIntoChildInjected(Mock mockToInject, string fieldName)
        /// </summary>
        /// <typeparam name="T">Targeted field type</typeparam>
        /// <param name="mockToInject">Mock to inject into field</param>
        /// <param name="monoBehaviourRecursivelyInjected">MonoBehaviourInjectable recursively injected (contained in childInjecter)</param>
        /// <exception cref="TestInjecterException">Thrown if instance not found which means monoBehaviourRecursivelyInjected does not contain a field</exception>
        public void InjectMockIntoChildInjected<T>(T mockToInject, MonoBehaviourInjectable monoBehaviourRecursivelyInjected)
            where T : class
        {
            try
            {
                injectablesContainer.InjectMockIntoInstance(mockToInject, monoBehaviourRecursivelyInjected);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot InjectMockIntoChildInjected because {e.Message}", objectToLink);
            }
        }

        /// <summary>
        /// From the MonoBehaviourInjectable recursively injected
        /// <para/>Inject manually a mock into the non-MonoBehaviour type T field and with the fieldName passed as a parameter
        /// </summary>
        /// <typeparam name="T">Targeted field type</typeparam>
        /// <param name="mockToInject">Mock to inject into field</param>
        /// <param name="fieldName">Name of the field to mock</param>
        /// <param name="monoBehaviourRecursivelyInjected">MonoBehaviourInjectable recursively injected (contained in childInjecter)</param>
        /// <exception cref="TestInjecterException">Thrown if instance not found which means monoBehaviourRecursivelyInjected does not contain a field</exception>
        public void InjectMockIntoChildInjected<T>(T mockToInject, MonoBehaviourInjectable monoBehaviourRecursivelyInjected, string fieldName)
            where T : class
        {
            try
            {
                injectablesContainer.InjectMockIntoInstance(mockToInject, monoBehaviourRecursivelyInjected, fieldName);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot InjectMockIntoChildInjected because {e.Message}", objectToLink);
            }
        }

        /// <summary>
        /// From the MonoBehaviourInjectable recursively injected
        /// <para/>Return the only component with MonoBehaviour type T field
        /// <para/>If multiple type T fields are found, you must use GetComponent(string fieldName) 
        /// </summary>
        /// <typeparam name="T">Type of MonoBehaviour searched</typeparam>
        /// <param name="monoBehaviourRecursivelyInjected">MonoBehaviourInjectable recursively injected (contained in childInjecter)</param>
        /// <returns>Single component which corresponds to the type T field</returns>
        /// <exception cref="TestInjecterException">Thrown if instance not found which means monoBehaviourRecursivelyInjected does not contain a field</exception>
        public T GetComponentFromChildInjected<T>(MonoBehaviourInjectable monoBehaviourRecursivelyInjected)
            where T : MonoBehaviour
        {
            try
            {
                return injectablesContainer.GetComponentFromInstance<T>(monoBehaviourRecursivelyInjected);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot GetComponentFromChildInjected because {e.Message}", objectToLink);
            }

        }

        /// <summary>
        /// From the MonoBehaviourInjectable recursively injected
        /// <para/>Return the only component with MonoBehaviour type T field and with the fieldName passed as a parameter
        /// </summary>
        /// <typeparam name="T">Type of MonoBehaviour searched</typeparam>
        /// <param name="monoBehaviourRecursivelyInjected">MonoBehaviourInjectable recursively injected (contained in childInjecter)</param>
        /// <param name="fieldName">Name of the field searched</param>
        /// <returns>Single component which corresponds to the type T field and fieldName passed as a parameter</returns>
        /// <exception cref="TestInjecterException">Thrown if instance not found which means monoBehaviourRecursivelyInjected does not contain a field</exception>
        public T GetComponentFromChildInjected<T>(MonoBehaviourInjectable monoBehaviourRecursivelyInjected, string fieldName)
            where T : MonoBehaviour
        {
            try
            {
                return injectablesContainer.GetComponentFromInstance<T>(monoBehaviourRecursivelyInjected, fieldName);
            }
            catch (InjectablesContainerException e)
            {
                throw new TestInjecterException($"Cannot GetComponentFromChildInjected because {e.Message}", objectToLink);
            }
        }
        #endregion Mocks and GetComponents ChildInjected
    }
}
