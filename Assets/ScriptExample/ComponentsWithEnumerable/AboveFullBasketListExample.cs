using Nixi.Injections;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class AboveFullBasketListExample : MonoBehaviourInjectable
    {
        [ComponentFromChildren("BelowComponent")]
        public FullBasketListExample ParentBasket;
    }
}