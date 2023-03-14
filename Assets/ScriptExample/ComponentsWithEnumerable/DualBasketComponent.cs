using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;
using System.Collections.Generic;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class DualBasketComponent : MonoBehaviourInjectable
    {
        [Components]
        private List<Fruit> fruitsList;
        public List<Fruit> FruitsList => fruitsList;

        [Components]
        private IEnumerable<Fruit> fruitsEnumerable;
        public IEnumerable<Fruit> FruitsEnumerable => fruitsEnumerable;
    }
}