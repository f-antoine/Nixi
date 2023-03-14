using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class ParentBasketWithoutCurrentList : MonoBehaviourInjectable
    {
        [Component]
        public BasketWithoutCurrentList BasketWithoutCurrentList;
    }
}