using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;

namespace Assets.ScriptExample.Farms
{
    public sealed class Farm : MonoBehaviourInjectable
    {
        [NixInjectComponentList]
        public List<Animal> Animals;

        [NixInjectComponentList]
        public List<Cat> Cats;
        
        [NixInjectComponentList]
        public IEnumerable<Animal> AnimalsEnumerable;

        [NixInjectComponentList]
        public IEnumerable<Cat> CatsEnumerable;

        [NixInjectComponentList]
        public Animal[] AnimalsArray;

        [NixInjectComponentList]
        public Cat[] CatsArray;
    }
}