using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class DualBasketComponent : MonoBehaviourInjectable
    {
        [SerializeField]
        [NixInjectComponents]
        private List<Fruit> fruitsList;
        public List<Fruit> FruitsList => fruitsList;

        [SerializeField]
        [NixInjectComponents]
        private IEnumerable<Fruit> fruitsEnumerable;
        public IEnumerable<Fruit> FruitsEnumerable => fruitsEnumerable;
    }
}