using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class FullBasketListExample : MonoBehaviourInjectable
    {
        // Parents
        [NixInjectComponentsFromParent]
        public List<Fruit> FruitsListParent;

        [NixInjectComponentsFromParent]
        public IEnumerable<Fruit> FruitsEnumerableParent;

        [NixInjectComponentsFromParent]
        public List<IFruit> IFruitsListParent;

        [NixInjectComponentsFromParent]
        public IEnumerable<IFruit> IFruitsEnumerableParent;

        // Current
        [NixInjectComponents]
        public List<Fruit> FruitsList;

        [NixInjectComponents]
        public IEnumerable<Fruit> FruitsEnumerable;

        [NixInjectComponents]
        public List<IFruit> IFruitsList;

        [NixInjectComponents]
        public IEnumerable<IFruit> IFruitsEnumerable;

        // Children
        [NixInjectComponentsFromChildren]
        public List<Fruit> FruitsListChildren;

        [NixInjectComponentsFromChildren]
        public IEnumerable<Fruit> FruitsEnumerableChildren;

        [NixInjectComponentsFromChildren]
        public List<IFruit> IFruitsListChildren;

        [NixInjectComponentsFromChildren]
        public IEnumerable<IFruit> IFruitsEnumerableChildren;
    }
}