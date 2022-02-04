using Nixi.Injections;
using ScriptExample.Containers;

namespace ScriptExample.OnlyFromContainer
{
    public sealed class SimpleOnlyFromContainer : OnlyFromContainerInjectable
    {
        [NixInjectFromContainer]
        public ITestInterface TestInterface;

        [NixInjectFromContainer]
        private ITestInterface testInterfacePrivate;

        public int ValueToRetrieveFromPrivateInterface => testInterfacePrivate.ValueToRetrieve;

        public void SetValueToRetrieve(int valueToRetrieve)
        {
            testInterfacePrivate.ValueToRetrieve = valueToRetrieve;
        }

        public SimpleOnlyFromContainer(bool autoInject = true)
            : base(autoInject)
        {
        }
    }
}