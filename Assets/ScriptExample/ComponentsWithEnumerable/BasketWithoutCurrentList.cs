using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class BasketWithoutCurrentList : MonoBehaviourInjectable
    {
        [ComponentsFromParents]
        private List<Fruit> fruitsListParents;
        public List<Fruit> FruitsListParents => fruitsListParents;

        [ComponentsFromParents]
        private IEnumerable<Fruit> fruitsEnumerableParents;
        public IEnumerable<Fruit> FruitsEnumerableParents => fruitsEnumerableParents;

        [ComponentsFromChildren]
        private List<Fruit> fruitsListChildren;
        public List<Fruit> FruitsListChildren => fruitsListChildren;

        [ComponentsFromChildren]
        private IEnumerable<Fruit> fruitsEnumerableChildren;
        public IEnumerable<Fruit> FruitsEnumerableChildren => fruitsEnumerableChildren;
    }
}