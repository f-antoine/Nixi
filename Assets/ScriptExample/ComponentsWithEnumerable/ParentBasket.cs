using Nixi.Injections;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class ParentBasket : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public Basket Basket;
    }
}