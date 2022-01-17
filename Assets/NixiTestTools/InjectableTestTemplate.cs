using Nixi.Injections;
using NixiTestTools.TestInjectorElements.Utils;
using UnityEngine;

namespace NixiTestTools
{
    /// <summary>
    /// Test template for MonoBehaviourInjectable, it creates an instance of the MonoBehaviourInjectable and use TestInjector 
    /// to specially handle dependency injection for testing
    /// </summary>
    /// <seealso cref="TestInjector"/>
    /// <typeparam name="T">MonoBehaviourInjectable</typeparam>
    public abstract class InjectableTestTemplate<T>
        where T : MonoBehaviourInjectable
    {
        /// <summary>
        /// Instance of the tested MonoBehaviourInjectable
        /// </summary>
        protected T MainTested { get; private set; }

        /// <summary>
        /// Injector implemented for tests, it handle all injections of fields decorated with Nixi inject attributes 
        /// of a class derived from MonoBehaviourInjectable during test execution
        /// <para/>His purpose is to simplify access to fields inexposed and make these fields testables/mockables
        /// </summary>
        protected TestInjector MainInjector { get; private set; }

        /// <summary>
        /// Name of the instance, mainly modified to consider MainTested as a root component
        /// <para/>See documentation part : "NixInjectRootComponent special case when you want your MainTested to be 
        /// recognized as a root gameObject"
        /// </summary>
        protected virtual string InstanceName => "";

        /// <summary>
        /// If true (default value), constructor call will set once MainTested and MainInjector (with method ResetTemplate())
        /// <para/>It was made to avoid NUnit dependency with SetUp decorator
        /// <para/>If you want to ResetTemplate() at each test with NUnit by example,
        /// you have to create a method decorated with [SetUp] and call ResetTemplate() in it
        /// </summary>
        protected virtual bool SetTemplateWithConstructor => true;

        /// <summary>
        /// Each mapping added into this container force a type to be used by its derived form during tests using TestInjector, 
        /// useful when working on abstract component injected with Nixi
        /// </summary>
        protected AbstractComponentMappingContainer ComponentMappingContainer = null;

        /// <summary>
        /// Map a component type with a derived type into ComponentMappingContainer
        /// <seealso cref="AbstractComponentMappingContainer"/>
        /// </summary>
        /// <typeparam name="TAbstract">Component key type</typeparam>
        /// <typeparam name="TImplementation">Implementation type</typeparam>
        protected void AddAbstractComponentMapping<TAbstract, TImplementation>()
            where TAbstract : Component
            where TImplementation : class, TAbstract
        {
            ComponentMappingContainer = ComponentMappingContainer ?? new AbstractComponentMappingContainer();
            ComponentMappingContainer.Map<TAbstract, TImplementation>();
        }

        /// <summary>
        /// Create an instance of the MonoBehaviourInjectable to test as well as its TestInjector which allows to expose the fields to 
        /// test / mock with the Nixi approach
        /// <para/>If you want to ResetTemplate() at each test with NUnit by example,
        /// you have to create a method decorated with [SetUp] and call ResetTemplate() in it
        /// </summary>
        protected virtual void ResetTemplate()
        {
            MainTested = new GameObject().AddComponent<T>();

            MainInjector = new TestInjector(MainTested, InstanceName, ComponentMappingContainer);

            MainInjector.CheckAndInjectAll();
        }

        /// <summary>
        /// This constructor was made to avoid NUnit dependency, it will set MainTested instance and its TestInjector
        /// named MainInjector using ResetTemplate() method only once.
        /// <para/>If you want to ResetTemplate() at each test with NUnit by example,
        /// you have to create a method decorated with [SetUp] and call ResetTemplate() in it
        /// You can suppress the first call on ResetTemplate (in this constructor) by setting value on SetTemplateWithConstructor to false
        /// </summary>
        public InjectableTestTemplate()
        {
            if (SetTemplateWithConstructor)
                ResetTemplate();
        }
    }
}