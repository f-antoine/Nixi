using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.ScriptableObjectToInject
{
    public sealed class ParentInjectableWithSOInjectables : MonoBehaviourInjectable
    {
        [Component]
        private InjectableWithSOInjectables injectableWithSOInjectables;
        public InjectableWithSOInjectables InjectableWithSOInjectables => injectableWithSOInjectables;
    }
}