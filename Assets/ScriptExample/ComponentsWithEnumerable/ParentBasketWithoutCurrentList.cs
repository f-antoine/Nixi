using Nixi.Injections;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class ParentBasketWithoutCurrentList : MonoBehaviourInjectable
    {
        [Component]
        public BasketWithoutCurrentList BasketWithoutCurrentList;
    }
}