using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;
using System.Collections.Generic;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class BasketWithChildrenAndParents : MonoBehaviourInjectable
    {
        [ComponentsFromParents]
        public List<Fruit> FruitsListParents;

        [ComponentsFromParents]
        public IEnumerable<Fruit> FruitsEnumerableParents;

        [ComponentsFromParents]
        public List<IFruit> IFruitsListParents;

        [ComponentsFromParents]
        public IEnumerable<IFruit> IFruitsEnumerableParents;

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