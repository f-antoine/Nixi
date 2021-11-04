using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;

namespace ScriptExample.Farms
{
    public sealed class FarmWithParent : MonoBehaviourInjectable
    {
        [NixInjectComponentsFromParent]
        public List<Animal> Animals;

        [NixInjectComponentsFromParent]
        public List<Cat> Cats;

        [NixInjectComponentsFromParent]
        public IEnumerable<Animal> AnimalsEnumerable;

        [NixInjectComponentsFromParent]
        public IEnumerable<Cat> CatsEnumerable;

        [NixInjectComponentsFromParent]
        public Animal[] AnimalsArray;

        [NixInjectComponentsFromParent]
        public Cat[] CatsArray;
    }
}