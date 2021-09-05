using Nixi.Injections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace NixiTestTools.TestInjecterElements
{
    /// <summary>
    /// Container to handle TestInjecter instances, it is used to injectMock into a field or GetComponent from fields instantiated during the Test injections
    /// </summary>
    internal sealed class InjectablesContainer
    {
        /// <summary>
        /// All MonoBehaviourInjectable instances
        /// </summary>
        private List<MonoBehaviourInjectableData> injectables = new List<MonoBehaviourInjectableData>();

        /// <summary>
        /// Find data of a MonoBehaviourInjectable from UnityEngine.Object.GetInstanceID()
        /// </summary>
        /// <param name="injectable">Injectable from which we look for data</param>
        /// <returns>Data of the injectable</returns>
        private MonoBehaviourInjectableData GetInjectableData(MonoBehaviourInjectable injectable)
        {
            MonoBehaviourInjectableData injectableData = GetInjectable(injectable);

            if (injectableData == null)
                throw new InjectablesContainerException($"no instance of MonoBehaviourInjectable with name {injectable.name} was found, {injectable.GetType().Name} class does not contain the field");

            return injectableData;
        }

        /// <summary>
        /// Return MonoBehaviourInjectableData which has Instance.GetInstanceID() equals to injectable.GetInstanceID()
        /// </summary>
        /// <param name="injectable">MonoBehaviourInjectable searched</param>
        /// <returns>MonoBehaviourInjectableData searched</returns>
        internal MonoBehaviourInjectableData GetInjectable(MonoBehaviourInjectable injectable)
        {
            return injectables.SingleOrDefault(x => x.Instance.GetInstanceID() == injectable.GetInstanceID());
        }

        /// <summary>
        /// Return MonoBehaviourInjectableData which has InstanceName equals to instanceName and Instance type equals typeOfInstanceToFind
        /// </summary>
        /// <param name="instanceName">Name searched</param>
        /// <param name="typeOfInstanceToFind">Type of instance searched</param>
        /// <returns>MonoBehaviourInjectableData searched</returns>
        internal MonoBehaviourInjectableData GetInjectable(string instanceName, Type typeOfInstanceToFind)
        {
            return injectables.SingleOrDefault(x => x.InstanceName == instanceName && x.Instance.GetType() == typeOfInstanceToFind);
        }

        /// <summary>
        /// If instance with instanceName has already been registered, return MonoBehaviourInjectableData associated to these parameters
        /// <para/> If not, store a MonoBehaviourInjectable on which operations like InjectMock or GetComponent can be executed
        /// </summary>
        /// <param name="instance">Instance on which all the fields are fields and usable from monoBehaviourFieldsTypeInstantiated and nonMonoBehaviourFields</param>
        /// <param name="instanceName">Name of the instance, can help for specials operation like root GameObjects (NixInjectMonoBehaviourFromMethodRootAttribute)</param>
        /// <returns>Instance of the MonoBehaviourInjectable added</returns>
        internal MonoBehaviourInjectableData Add(MonoBehaviourInjectable instance, string instanceName = "")
        {
            MonoBehaviourInjectableData addedInjectableData = new MonoBehaviourInjectableData
            {
                Instance = instance,
                InstanceName = instanceName
            };

            injectables.Add(addedInjectableData);
            return addedInjectableData;
        }

        /// <summary>
        /// Inject manually a mock into the non-MonoBehaviour field with T Type in a MonoBehaviourInjectable
        /// </summary>
        /// <typeparam name="T">Targeted field type</typeparam>
        /// <param name="mockToInject">Mock to inject into field</param>
        /// <param name="monoBehaviourInjectableToFind">Targeted injectable</param>
        internal void InjectMockIntoInstance<T>(T mockToInject, MonoBehaviourInjectable monoBehaviourInjectableToFind)
            where T : class
        {
            MonoBehaviourInjectableData injectableData = GetInjectableData(monoBehaviourInjectableToFind);

            Type interfaceType = typeof(T);

            IEnumerable<FieldInfo> fieldSelecteds = injectableData.nonMonoBehaviourFields.Where(x => x.FieldType == interfaceType);

            if (!fieldSelecteds.Any())
                throw new InjectablesContainerException($"no field with type {interfaceType.Name} was found");

            if (fieldSelecteds.Count() > 1)
                throw new InjectablesContainerException($"multiple fields with type {interfaceType.Name} were found, cannot define which one use, please use InjectMock<T>(T mockToInject, string fieldName) instead");

            fieldSelecteds.Single().SetValue(injectableData.Instance, mockToInject);
        }

        /// <summary>
        /// Inject manually a mock into the non-MonoBehaviour field with T Type and with the fieldName passed as a parameter in a MonoBehaviourInjectable
        /// </summary>
        /// <typeparam name="T">Targeted field type</typeparam>
        /// <param name="fieldName">Name of the field to mock</param>
        /// <param name="mockToInject">Mock to inject into field</param>
        /// <param name="monoBehaviourInjectableToFind">Targeted injectable</param>
        internal void InjectMockIntoInstance<T>(T mockToInject, MonoBehaviourInjectable monoBehaviourInjectableToFind, string fieldName)
            where T : class
        {
            MonoBehaviourInjectableData injectableData = GetInjectableData(monoBehaviourInjectableToFind);

            Type interfaceType = typeof(T);

            IEnumerable<FieldInfo> fieldWithType = injectableData.nonMonoBehaviourFields.Where(x => x.FieldType == interfaceType);
            if (!fieldWithType.Any())
                throw new InjectablesContainerException($"no field with type {interfaceType.Name}");

            IEnumerable<FieldInfo> fieldWithTypeAndName = fieldWithType.Where(x => x.Name == fieldName);
            if (!fieldWithTypeAndName.Any())
                throw new InjectablesContainerException($"field with type {interfaceType.Name} was/were found, but none with fieldName {fieldName}");

            fieldWithTypeAndName.Single().SetValue(injectableData.Instance, mockToInject);
        }

        /// <summary>
        /// Return the only component with MonoBehaviour type T field from a MonoBehaviourInjectable
        /// <para/>If multiple type T fields are found, you must use GetComponent<T>(string fieldName) 
        /// </summary>
        /// <typeparam name="T">Type of MonoBehaviour searched</typeparam>
        /// <returns>Single component which corresponds to the type T field</returns>
        internal T GetComponentFromInstance<T>(MonoBehaviourInjectable monoBehaviourInjectableToFind)
            where T : MonoBehaviour
        {
            MonoBehaviourInjectableData injectableData = GetInjectableData(monoBehaviourInjectableToFind);

            Type typeToFind = typeof(T);

            IEnumerable<GameObjectWithFieldInfo> gameObjectWithType = injectableData.monoBehaviourWithFieldInstantiated.Where(x => x.FieldInfo.FieldType == typeToFind);

            if (!gameObjectWithType.Any())
                throw new InjectablesContainerException($"no GameObject with type {typeToFind.Name} was found");

            if (gameObjectWithType.Count() > 1)
                throw new InjectablesContainerException($"multiple GameObject with type {typeToFind.Name} were found, cannot define which one use, please use GetComponent(fieldName)");

            return gameObjectWithType.Single().GameObject.GetComponent<T>();
        }

        /// <summary>
        /// Return the only component with MonoBehaviour type T field and with fieldName passed as parameter from a MonoBehaviourInjectable
        /// <para/>If multiple type T fields are found, you must use GetComponent<T>(string fieldName) 
        /// </summary>
        /// <typeparam name="T">Type of MonoBehaviour searched</typeparam>
        /// <returns>Single component which corresponds to the type T field</returns>
        internal T GetComponentFromInstance<T>(MonoBehaviourInjectable monoBehaviourInjectableToFind, string fieldName)
            where T : MonoBehaviour
        {
            MonoBehaviourInjectableData injectableData = GetInjectableData(monoBehaviourInjectableToFind);

            Type typeToFind = typeof(T);

            IEnumerable<GameObjectWithFieldInfo> gameObjectWithFieldType = injectableData.monoBehaviourWithFieldInstantiated.Where(x => x.FieldInfo.FieldType == typeToFind);
            if (!gameObjectWithFieldType.Any())
                throw new InjectablesContainerException($"no GameObject with type {typeToFind.Name} was found");

            IEnumerable <GameObjectWithFieldInfo> gameObjectWithFieldTypeAndFieldName = gameObjectWithFieldType.Where(x => x.FieldInfo.Name == fieldName);
            if (!gameObjectWithFieldTypeAndFieldName.Any())
                throw new InjectablesContainerException($"gameObject with type {typeToFind.Name} was/were found, but none with field name {fieldName}");

            return gameObjectWithFieldTypeAndFieldName.Single().GameObject.GetComponent<T>();
        }
    }
}