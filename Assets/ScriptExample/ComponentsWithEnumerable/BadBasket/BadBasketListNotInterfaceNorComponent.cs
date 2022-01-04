using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.ComponentsWithEnumerable.BadBasket
{
    public sealed class BadBasketListNotInterfaceNorComponent : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        private List<int> notFruits;
    }
}