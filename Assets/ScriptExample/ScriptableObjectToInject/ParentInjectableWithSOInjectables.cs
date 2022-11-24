using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.ScriptableObjectToInject
{
    public sealed class ParentInjectableWithSOInjectables : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        private InjectableWithSOInjectables injectableWithSOInjectables;
        public InjectableWithSOInjectables InjectableWithSOInjectables => injectableWithSOInjectables;
    }
}