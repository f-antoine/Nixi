using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
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
