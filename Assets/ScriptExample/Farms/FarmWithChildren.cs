using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.Farms
{
    public sealed class FarmWithChildren : MonoBehaviourInjectable
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