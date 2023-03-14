using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using Nixi.Injections.Attributes.Fields;
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
