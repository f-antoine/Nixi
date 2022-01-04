using Nixi.Injections;

namespace ScriptExample.ScriptableObjectToInject
{
    public sealed class ParentInjectableWithSOInjectables : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        private InjectableWithSOInjectables injectableWithSOInjectables;
        public InjectableWithSOInjectables InjectableWithSOInjectables => injectableWithSOInjectables;
    }
}