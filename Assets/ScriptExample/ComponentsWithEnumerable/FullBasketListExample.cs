using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;
using System.Collections.Generic;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class FullBasketListExample : MonoBehaviourInjectable
    {
        // Parents
        [ComponentsFromParents]
        public List<Fruit> FruitsListParent;

        [ComponentsFromParents]
        public IEnumerable<Fruit> FruitsEnumerableParent;

        [ComponentsFromParents]
        public List<IFruit> IFruitsListParent;

        [ComponentsFromParents]
        public IEnumerable<IFruit> IFruitsEnumerableParent;

        // Current
        [Components]
        public List<Fruit> FruitsList;

        [Components]
        public IEnumerable<Fruit> FruitsEnumerable;

        [Components]
        public List<IFruit> IFruitsList;

        [Components]
        public IEnumerable<IFruit> IFruitsEnumerable;

        // Children
        [ComponentsFromChildren]
        public List<Fruit> FruitsListChildren;

        [ComponentsFromChildren]
        public IEnumerable<Fruit> FruitsEnumerableChildren;

        [ComponentsFromChildren]
        public List<IFruit> IFruitsListChildren;

        [ComponentsFromChildren]
        public IEnumerable<IFruit> IFruitsEnumerableChildren;
    }
}