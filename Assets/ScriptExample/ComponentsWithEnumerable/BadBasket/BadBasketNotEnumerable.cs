using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.ComponentsWithEnumerable.BadBasket
{
    public sealed class BadBasketNotEnumerable : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        private Fruit fruit;
    }
}