using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;

namespace ScriptExample.ComponentsWithEnumerable.BadBasket
{
    public sealed class BadBasketEnumerableNotInterfaceNorComponent : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        private IEnumerable<int> notFruits;
    }
}