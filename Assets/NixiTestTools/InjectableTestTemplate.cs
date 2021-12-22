using Nixi.Injections;
using NixiTestTools.TestInjecterElements.Utils;
using NUnit.Framework;
using UnityEngine;

namespace NixiTestTools
{
    /// <summary>
    /// Test template for MonoBehaviourInjectable, it creates an instance of the MonoBehaviourInjectable and use TestInjecter to specially handle dependency injection for testing
    /// </summary>
    /// <seealso cref="TestInjecter"/>
    /// <typeparam name="T">MonoBehaviourInjectable</typeparam>
    public abstract class InjectableTestTemplate<T>
        where T : MonoBehaviourInjectable
    {
        /// <summary>
        /// Instance of the tested MonoBehaviourInjectable
        /// </summary>
        protected T MainTested { get; private set; }

        /// <summary>
        /// Injecter implemented for tests, it handle all injections of fields decorated with Nixi inject attributes of a class derived from MonoBehaviourInjectable during test execution
        /// <para/>His purpose is to simplify access to fields inexposed and make these fields testables/mockables
        /// </summary>
        protected TestInjecter MainInjecter { get; private set; }

        /// <summary>
        /// Name of the instance, mainly modified to consider MainTested as a root component
        /// <para/>See documentation part : "NixInjectRootComponent special case when you want your MainTested to be recognized as a root gameObject"
        /// </summary>
        protected virtual string InstanceName => "";

        /// <summary>
        /// Each mapping added into this container force a type to be used by his derived form during tests using TestInjecter, useful when working on abstract component injected with Nixi
        /// </summary>
        private AbstractComponentMappingContainer ComponentMappingContainer = null;

        /// <summary>
        /// Map a component type with a derived type into ComponentMappingContainer
        /// </summary>
        /// <typeparam name="TAbstract">Component key type</typeparam>
        /// <typeparam name="TImplementation">Implementation type</typeparam>
        protected void AddAbstractComponentMapping<TAbstract, TImplementation>()
            where TAbstract : Component
            where TImplementation : class, TAbstract
        {
            ComponentMappingContainer ??= new AbstractComponentMappingContainer();
            ComponentMappingContainer.Map<TAbstract, TImplementation>();
        }

        /// <summary>
        /// Create an instance of the test MonoBehaviourInjectable that we want to test as well as its TestInjecter which allows to expose the fields to test / mock with the Nixi approach
        /// </summary>
        [SetUp]
        public virtual void InitTests()
        {
            MainTested = new GameObject().AddComponent<T>();

            MainInjecter = new TestInjecter(MainTested, InstanceName, ComponentMappingContainer);

            MainInjecter.CheckAndInjectAll();
        }
    }
}