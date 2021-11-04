using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;

namespace ScriptExample.Farms
{
    public sealed class FarmWithChilds : MonoBehaviourInjectable
    {
        [NixInjectComponentsFromChildren]
        public List<Animal> Animals;

        [NixInjectComponentsFromChildren]
        public List<Cat> Cats;

        [NixInjectComponentsFromChildren]
        public IEnumerable<Animal> AnimalsEnumerable;

        [NixInjectComponentsFromChildren]
        public IEnumerable<Cat> CatsEnumerable;

        [NixInjectComponentsFromChildren]
        public Animal[] AnimalsArray;

        [NixInjectComponentsFromChildren]
        public Cat[] CatsArray;
    }
}