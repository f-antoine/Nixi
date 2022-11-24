using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;

namespace ScriptExample.ComponentsWithEnumerable.BadBasket
{
    public sealed class BadBasketNotEnumerable : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        private Fruit fruit;
    }
}