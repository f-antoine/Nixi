using Nixi.Injections;

namespace ScriptExample.ComponentsWithEnumerable.BadBasket
{
    public sealed class BadBasketNotEnumerable : MonoBehaviourInjectable
    {
        [Components]
        private Fruit fruit;
    }
}