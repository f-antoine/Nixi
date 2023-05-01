using Nixi.Injections;
using System.Collections;

namespace ScriptExample.ComponentsWithInterface.BadDucks
{
    public sealed class BadDuckCompo : MonoBehaviourInjectable
    {
        [Component]
        private IList impossibleToInjectFieldFromGetComponent;
        public IList ImpossibleToInjectFieldFromGetComponent => impossibleToInjectFieldFromGetComponent;
    }
}