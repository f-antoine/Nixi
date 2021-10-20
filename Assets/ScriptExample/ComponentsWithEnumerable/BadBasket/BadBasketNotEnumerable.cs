using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.ComponentsWithEnumerable.BadBasket
{
    public sealed class BadBasketNotEnumerable : MonoBehaviourInjectable
    {
        [NixInjectComponentList]
        private Fruit fruit;
    }
}