using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class AboveFullBasketListExample : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("BelowComponent")]
        public FullBasketListExample ParentBasket;
    }
}