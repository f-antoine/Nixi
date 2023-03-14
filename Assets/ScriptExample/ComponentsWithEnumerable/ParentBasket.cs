using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class ParentBasket : MonoBehaviourInjectable
    {
        [Component]
        public Basket Basket;
    }
}