﻿using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class DualBasketComponent : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        private List<Fruit> fruitsList;
        public List<Fruit> FruitsList => fruitsList;

        [NixInjectComponents]
        private IEnumerable<Fruit> fruitsEnumerable;
        public IEnumerable<Fruit> FruitsEnumerable => fruitsEnumerable;
    }
}