using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections;

namespace Assets.ScriptExample.ComponentsWithInterface.BadDucks
{
    public sealed class BadDuckCompoInParents : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("any", GameObjectMethod.GetComponentsInParent)]
        private IList impossibleToInjectFieldFromGetComponent;
        public IList ImpossibleToInjectFieldFromGetComponent => impossibleToInjectFieldFromGetComponent;
    }
}
