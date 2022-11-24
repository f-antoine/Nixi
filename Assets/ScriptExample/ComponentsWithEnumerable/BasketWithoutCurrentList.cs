using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;
using System.Collections.Generic;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class BasketWithoutCurrentList : MonoBehaviourInjectable
    {
        [NixInjectComponentsFromParent]
        private List<Fruit> fruitsListParents;
        public List<Fruit> FruitsListParents => fruitsListParents;

        [NixInjectComponentsFromParent]
        private IEnumerable<Fruit> fruitsEnumerableParents;
        public IEnumerable<Fruit> FruitsEnumerableParents => fruitsEnumerableParents;

        [NixInjectComponentsFromChildren]
        private List<Fruit> fruitsListChildren;
        public List<Fruit> FruitsListChildren => fruitsListChildren;

        [NixInjectComponentsFromChildren]
        private IEnumerable<Fruit> fruitsEnumerableChildren;
        public IEnumerable<Fruit> FruitsEnumerableChildren => fruitsEnumerableChildren;
    }
}