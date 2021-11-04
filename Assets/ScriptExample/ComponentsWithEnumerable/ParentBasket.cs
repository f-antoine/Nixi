using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class ParentBasket : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public Basket Basket;
    }
}