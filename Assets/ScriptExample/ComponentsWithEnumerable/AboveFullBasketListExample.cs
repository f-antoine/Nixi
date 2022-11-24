using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class AboveFullBasketListExample : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("BelowComponent")]
        public FullBasketListExample ParentBasket;
    }
}