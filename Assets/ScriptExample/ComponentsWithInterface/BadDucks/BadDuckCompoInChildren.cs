using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections;

namespace Assets.ScriptExample.ComponentsWithInterface.BadDucks
{
    public sealed class BadDuckCompoInChildren : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("any", GameObjectMethod.GetComponentsInChildren)]
        private IList impossibleToInjectFieldFromGetComponent;
        public IList ImpossibleToInjectFieldFromGetComponent => impossibleToInjectFieldFromGetComponent;
    }
}
