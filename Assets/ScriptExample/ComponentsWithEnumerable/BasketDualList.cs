using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;
using System.Collections.Generic;

namespace ScriptExample.ComponentsWithEnumerable
{
    public sealed class BasketDualList : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        private List<IFruit> firstFruitsList;
        public List<IFruit> FirstFruitsList => firstFruitsList;

        [NixInjectComponents]
        private List<IFruit> secondFruitsList;
        public List<IFruit> SecondFruitsList => secondFruitsList;
    }
}