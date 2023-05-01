using Nixi.Injections;
using ScriptExample.Containers;

namespace ScriptExample.OnlyFromContainer
{
    public sealed class ManyDecoratorErrorOnlyFromContainer : OnlyFromContainerInjectable
    {
        [FromContainer]
        [ComponentFromChildren]
        public ITestInterface TestInterface;

        public ManyDecoratorErrorOnlyFromContainer(bool autoInject = true)
            : base(autoInject)
        {
        }
    }
}
