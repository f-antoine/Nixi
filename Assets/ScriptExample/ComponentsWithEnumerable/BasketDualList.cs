using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;
using System.Collections.Generic;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class BasketDualList : MonoBehaviourInjectable
    {
        [Components]
        private List<IFruit> firstFruitsList;
        public List<IFruit> FirstFruitsList => firstFruitsList;

        [Components]
        private List<IFruit> secondFruitsList;
        public List<IFruit> SecondFruitsList => secondFruitsList;
    }
}