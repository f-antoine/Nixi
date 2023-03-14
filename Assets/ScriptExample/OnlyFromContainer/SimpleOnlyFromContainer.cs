using Nixi.Injections;
using Nixi.Injections.Attributes.Fields;
using ScriptExample.Containers;

namespace ScriptExample.OnlyFromContainer
{
    public sealed class SimpleOnlyFromContainer : OnlyFromContainerInjectable
    {
        [FromContainer]
        public ITestInterface TestInterface;

        [FromContainer]
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