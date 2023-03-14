using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class TonsOfBasket : MonoBehaviourInjectable
    {
        [Component]
        private BasketDualList firstDualList;
        public BasketDualList FirstDualList => firstDualList;

        [ComponentFromChildren("anotherBasketDualList")]
        private BasketDualList secondDualList;
        public BasketDualList SecondDualList => secondDualList;
    }
}