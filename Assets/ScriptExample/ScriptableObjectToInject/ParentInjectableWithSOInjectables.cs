using Nixi.Injections;

namespace ScriptExample.ScriptableObjectToInject
{
    public sealed class ParentInjectableWithSOInjectables : MonoBehaviourInjectable
    {
        [Component]
        private InjectableWithSOInjectables injectableWithSOInjectables;
        public InjectableWithSOInjectables InjectableWithSOInjectables => injectableWithSOInjectables;
    }
}