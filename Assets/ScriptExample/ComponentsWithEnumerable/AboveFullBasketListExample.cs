using Nixi.Injections;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class AboveFullBasketListExample : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("BelowComponent")]
        public FullBasketListExample ParentBasket;
    }
}