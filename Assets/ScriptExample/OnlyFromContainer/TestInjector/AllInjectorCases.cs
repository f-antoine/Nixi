using Nixi.Injections;
using ScriptExample.ComponentsWithEnumerable;
using ScriptExample.Containers;

namespace ScriptExample.OnlyFromContainer
{
    public sealed class AllInjectorCases : OnlyFromContainerInjectable
    {
        [NixInjectFromContainer]
        public ITestInterface TestInterface;

        [NixInjectFromContainer]
        public IFruit FirstFruit;

        [NixInjectFromContainer]
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