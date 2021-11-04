using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class SimpleBasket : MonoBehaviourInjectable
    {
        [SerializeField]
        [NixInjectComponents]
        private List<IFruit> iFruitsList;
        public List<IFruit> IFruitsList => iFruitsList;

        [SerializeField]
        [NixInjectComponents]
        private IEnumerable<IFruit> iFruitsEnumerable;
        public IEnumerable<IFruit> IFruitsEnumerable => iFruitsEnumerable;
    }
}