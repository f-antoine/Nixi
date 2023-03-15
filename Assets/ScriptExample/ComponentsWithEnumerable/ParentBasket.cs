using Nixi.Injections;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class ParentBasket : MonoBehaviourInjectable
    {
        [Component]
        public Basket Basket;
    }
}