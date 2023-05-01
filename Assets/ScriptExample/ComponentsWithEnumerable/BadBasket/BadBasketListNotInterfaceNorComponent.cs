using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.ComponentsWithEnumerable.BadBasket
{
    public sealed class BadBasketListNotInterfaceNorComponent : MonoBehaviourInjectable
    {
        [Components]
        private List<int> notFruits;
    }
}