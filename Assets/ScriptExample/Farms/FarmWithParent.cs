using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;
using System.Collections.Generic;

namespace ScriptExample.Farms
{
    public sealed class FarmWithParent : MonoBehaviourInjectable
    {
        [ComponentsFromParents]
        public List<Animal> Animals;

        [ComponentsFromParents]
        public List<Cat> Cats;

        [ComponentsFromParents]
        public IEnumerable<Animal> AnimalsEnumerable;

        [ComponentsFromParents]
        public IEnumerable<Cat> CatsEnumerable;

        [ComponentsFromParents]
        public Animal[] AnimalsArray;

        [ComponentsFromParents]
        public Cat[] CatsArray;
    }
}