using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.ScriptExample.ComponentsWithEnumerable
{
    public sealed class BasketDualList : MonoBehaviourInjectable
    {
        [SerializeField]
        [NixInjectComponentList]
        private List<IFruit> firstFruitsList;
        public List<IFruit> FirstFruitsList => firstFruitsList;

        [SerializeField]
        [NixInjectComponentList]
        private List<IFruit> secondFruitsList;
        public List<IFruit> SecondFruitsList => secondFruitsList;
    }
}