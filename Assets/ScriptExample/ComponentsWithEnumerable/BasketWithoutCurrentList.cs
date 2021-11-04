using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class BasketWithoutCurrentList : MonoBehaviourInjectable
    {
        [SerializeField]
        [NixInjectComponentsFromParent]
        private List<Fruit> fruitsListParents;
        public List<Fruit> FruitsListParents => fruitsListParents;

        [SerializeField]
        [NixInjectComponentsFromParent]
        private IEnumerable<Fruit> fruitsEnumerableParents;
        public IEnumerable<Fruit> FruitsEnumerableParents => fruitsEnumerableParents;

        [SerializeField]
        [NixInjectComponentsFromChildren]
        private List<Fruit> fruitsListChildren;
        public List<Fruit> FruitsListChildren => fruitsListChildren;

        [SerializeField]
        [NixInjectComponentsFromChildren]
        private IEnumerable<Fruit> fruitsEnumerableChildren;
        public IEnumerable<Fruit> FruitsEnumerableChildren => fruitsEnumerableChildren;
    }
}