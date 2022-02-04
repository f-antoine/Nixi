using Nixi.Injections;
using ScriptExample.Containers;

namespace ScriptExample.OnlyFromContainer
{
    public sealed class NoFieldToInjectOnlyFromContainer : OnlyFromContainerInjectable
    {
        public ITestInterface TestInterface;

        public NoFieldToInjectOnlyFromContainer(bool autoInject = true)
            : base(autoInject)
        {
        }
    }
}