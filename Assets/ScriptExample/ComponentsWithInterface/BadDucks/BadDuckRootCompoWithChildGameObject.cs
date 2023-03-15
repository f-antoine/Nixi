using Nixi.Injections;
using System.Collections;

namespace ScriptExample.ComponentsWithInterface.BadDucks
{
    public sealed class BadDuckRootCompoWithChildGameObject : MonoBehaviourInjectable
    {
        [RootComponent("anyRootName", "anyChildGameObjectName")]
        private IList impossibleToInjectFieldFromGetComponent;
        public IList ImpossibleToInjectFieldFromGetComponent => impossibleToInjectFieldFromGetComponent;
    }
}
