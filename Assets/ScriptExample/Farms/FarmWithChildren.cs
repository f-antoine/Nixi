using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.Farms
{
    public sealed class FarmWithChildren : MonoBehaviourInjectable
    {
        [ComponentsFromChildren]
        public List<Animal> Animals;

        [ComponentsFromChildren]
        public List<Cat> Cats;

        [ComponentsFromChildren]
        public IEnumerable<Animal> AnimalsEnumerable;

        [ComponentsFromChildren]
        public IEnumerable<Cat> CatsEnumerable;

        [ComponentsFromChildren]
        public Animal[] AnimalsArray;

        [ComponentsFromChildren]
        public Cat[] CatsArray;
    }
}