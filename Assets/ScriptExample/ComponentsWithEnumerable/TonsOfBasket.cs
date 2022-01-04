using Nixi.Injections;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class TonsOfBasket : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        private BasketDualList firstDualList;
        public BasketDualList FirstDualList => firstDualList;

        [NixInjectComponentFromChildren("anotherBasketDualList")]
        private BasketDualList secondDualList;
        public BasketDualList SecondDualList => secondDualList;
    }
}