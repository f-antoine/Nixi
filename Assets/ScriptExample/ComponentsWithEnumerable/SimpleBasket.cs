using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.ScriptExample.ComponentsWithEnumerable
{
    public sealed class SimpleBasket : MonoBehaviourInjectable
    {
        [SerializeField]
        [NixInjectComponentList]
        private List<IFruit> iFruitsList;
        public List<IFruit> IFruitsList => iFruitsList;

        [SerializeField]
        [NixInjectComponentList]
        private IEnumerable<IFruit> iFruitsEnumerable;
        public IEnumerable<IFruit> IFruitsEnumerable => iFruitsEnumerable;
    }
}