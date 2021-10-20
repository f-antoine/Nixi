using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;

namespace Assets.ScriptExample.ComponentsWithEnumerable.BadBasket
{
    public sealed class BadBasketListNotInterfaceNorComponent : MonoBehaviourInjectable
    {
        [NixInjectComponentList]
        private List<int> notFruits;
    }
}