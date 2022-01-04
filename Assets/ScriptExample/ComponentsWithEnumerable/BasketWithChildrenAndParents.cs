using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class BasketWithChildrenAndParents : MonoBehaviourInjectable
    {
        [NixInjectComponentsFromParent]
        public List<Fruit> FruitsListParents;

        [NixInjectComponentsFromParent]
        public IEnumerable<Fruit> FruitsEnumerableParents;

        [NixInjectComponentsFromParent]
        public List<IFruit> IFruitsListParents;

        [NixInjectComponentsFromParent]
        public IEnumerable<IFruit> IFruitsEnumerableParents;

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