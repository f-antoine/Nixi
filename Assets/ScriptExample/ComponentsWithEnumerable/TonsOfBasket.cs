using Nixi.Injections;
using Nixi.Injections.Attributes;
using UnityEngine;

namespace Assets.ScriptExample.ComponentsWithEnumerable
{
    public sealed class TonsOfBasket : MonoBehaviourInjectable
    {
        [SerializeField]
        [NixInjectComponent]
        private BasketDualList firstDualList;
        public BasketDualList FirstDualList => firstDualList;

        // TODO : Décorer avec [NixInjectComponentList], car ça marchait alors que c'est interdit
        [SerializeField]
        [NixInjectComponentFromMethod("anotherBasketDualList", GameObjectMethod.GetComponentsInChildren)]
        private BasketDualList secondDualList;
        public BasketDualList SecondDualList => secondDualList;
    }
}