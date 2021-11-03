using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;

namespace Assets.ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenEnumerableComponentChild : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("any", GameObjectMethod.GetComponentsInChildren)]
        public IEnumerable<EmptyClass> FallenElement;
    }
}