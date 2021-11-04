using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class ParentBasketWithoutCurrentList : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public BasketWithoutCurrentList BasketWithoutCurrentList;
    }
}