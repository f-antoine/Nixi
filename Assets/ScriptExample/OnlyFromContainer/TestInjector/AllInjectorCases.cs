using Nixi.Injections;
using ScriptExample.ComponentsWithEnumerable;
using ScriptExample.Containers;

namespace ScriptExample.OnlyFromContainer.TestInjector
{
    public sealed class AllInjectorCases : OnlyFromContainerInjectable
    {
        [FromContainer]
        public ITestInterface TestInterface;

        [FromContainer]
        public IFruit FirstFruit;

        [FromContainer]
        public IFruit SecondFruit;

        public AllInjectorCases(bool autoInject = true)
            : base(autoInject)
        {
        }

        public AllInjectorCases(int valueToRetrieve, bool autoInject = true)
            : base(autoInject)
        {
            TestInterface = new TestImplementation { ValueToRetrieve = valueToRetrieve };
        }
    }
}