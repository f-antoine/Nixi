using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;
using System.Collections.Generic;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class SimpleBasket : MonoBehaviourInjectable
    {
        [Components]
        private List<IFruit> iFruitsList;
        public List<IFruit> IFruitsList => iFruitsList;

        [Components]
        private IEnumerable<IFruit> iFruitsEnumerable;
        public IEnumerable<IFruit> IFruitsEnumerable => iFruitsEnumerable;
    }
}