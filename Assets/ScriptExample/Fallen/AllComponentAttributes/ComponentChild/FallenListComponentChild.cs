using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;

namespace Assets.ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenListComponentChild : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("any", GameObjectMethod.GetComponentsInChildren)]
        public List<EmptyClass> FallenElement;
    }
}