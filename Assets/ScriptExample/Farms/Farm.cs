using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.Farms
{
    public sealed class Farm : MonoBehaviourInjectable
    {
        [Components]
        public List<Animal> Animals;

        [Components]
        public List<Cat> Cats;
        
        [Components]
        public IEnumerable<Animal> AnimalsEnumerable;

        [Components]
        public IEnumerable<Cat> CatsEnumerable;

        [Components]
        public Animal[] AnimalsArray;

        [Components]
        public Cat[] CatsArray;
    }
}