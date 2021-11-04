using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class BasketDualList : MonoBehaviourInjectable
    {
        [SerializeField]
        [NixInjectComponents]
        private List<IFruit> firstFruitsList;
        public List<IFruit> FirstFruitsList => firstFruitsList;

        [SerializeField]
        [NixInjectComponents]
        private List<IFruit> secondFruitsList;
        public List<IFruit> SecondFruitsList => secondFruitsList;
    }
}