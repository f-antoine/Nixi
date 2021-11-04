using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;

namespace ScriptExample.Farms
{
    public sealed class Farm : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        public List<Animal> Animals;

        [NixInjectComponents]
        public List<Cat> Cats;
        
        [NixInjectComponents]
        public IEnumerable<Animal> AnimalsEnumerable;

        [NixInjectComponents]
        public IEnumerable<Cat> CatsEnumerable;

        [NixInjectComponents]
        public Animal[] AnimalsArray;

        [NixInjectComponents]
        public Cat[] CatsArray;
    }
}