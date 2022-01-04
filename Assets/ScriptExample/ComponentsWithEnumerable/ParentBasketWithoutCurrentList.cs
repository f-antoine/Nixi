using Nixi.Injections;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class ParentBasketWithoutCurrentList : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public BasketWithoutCurrentList BasketWithoutCurrentList;
    }
}