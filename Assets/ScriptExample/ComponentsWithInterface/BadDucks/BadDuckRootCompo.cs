using Nixi.Injections;
using System.Collections;

namespace ScriptExample.ComponentsWithInterface.BadDucks
{
    public sealed class BadDuckRootCompo : MonoBehaviourInjectable
    {
        [RootComponent("anyRootName")]
        private IList impossibleToInjectFieldFromGetComponent;
        public IList ImpossibleToInjectFieldFromGetComponent => impossibleToInjectFieldFromGetComponent;
    }
}
