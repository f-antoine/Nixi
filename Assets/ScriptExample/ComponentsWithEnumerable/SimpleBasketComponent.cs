using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class SimpleBasketComponent : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        private List<Fruit> fruitsList;
        public List<Fruit> FruitsList => fruitsList;
    }
}