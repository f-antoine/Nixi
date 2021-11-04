using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections;

namespace ScriptExample.ComponentsWithInterface.BadDucks
{
    public sealed class BadDuckCompo : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        private IList impossibleToInjectFieldFromGetComponent;
        public IList ImpossibleToInjectFieldFromGetComponent => impossibleToInjectFieldFromGetComponent;
    }
}