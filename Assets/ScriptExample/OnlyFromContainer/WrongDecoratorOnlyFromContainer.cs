using Nixi.Injections;
using ScriptExample.Containers;

namespace ScriptExample.OnlyFromContainer
{
    public sealed class WrongDecoratorOnlyFromContainer : OnlyFromContainerInjectable
    {
        [Component]
        public ITestInterface TestInterface;

        public WrongDecoratorOnlyFromContainer(bool autoInject = true)
            : base(autoInject)
        {
        }
    }
}