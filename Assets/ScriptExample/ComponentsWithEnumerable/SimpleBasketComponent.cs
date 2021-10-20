using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.ScriptExample.ComponentsWithEnumerable
{
    public sealed class SimpleBasketComponent : MonoBehaviourInjectable
    {
        [SerializeField]
        [NixInjectComponentList]
        private List<Fruit> fruitsList;
        public List<Fruit> FruitsList => fruitsList;
    }
}