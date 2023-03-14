using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;
using System.Collections.Generic;

namespace ScriptExample.Farms
{
    public sealed class FarmWithAllLevels : MonoBehaviourInjectable
    {
        // Children
        [ComponentsFromChildren]
        public List<Animal> AnimalsChildren;

        [ComponentsFromChildren]
        public List<Cat> CatsChildren;

        [ComponentsFromChildren]
        public IEnumerable<Animal> AnimalsEnumerableChildren;

        [ComponentsFromChildren]
        public IEnumerable<Cat> CatsEnumerableChildren;

        [ComponentsFromChildren]
        public Animal[] AnimalsArrayChildren;

        [ComponentsFromChildren]
        public Cat[] CatsArrayChildren;
        
        // Current
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

        // Parent
        [ComponentsFromParents]
        public List<Animal> AnimalsParent;

        [ComponentsFromParents]
        public List<Cat> CatsParent;

        [ComponentsFromParents]
        public IEnumerable<Animal> AnimalsEnumerableParent;

        [ComponentsFromParents]
        public IEnumerable<Cat> CatsEnumerableParent;

        [ComponentsFromParents]
        public Animal[] AnimalsArrayParent;

        [ComponentsFromParents]
        public Cat[] CatsArrayParent;
    }
}