using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;

namespace Assets.ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenListComponentParent : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("any", GameObjectMethod.GetComponentsInParent)]
        public List<EmptyClass> FallenElement;
    }
}