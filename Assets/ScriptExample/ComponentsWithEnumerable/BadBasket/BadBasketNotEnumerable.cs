using Nixi.Injections;

namespace ScriptExample.ComponentsWithEnumerable.BadBasket
{
    public sealed class BadBasketNotEnumerable : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        private Fruit fruit;
    }
}