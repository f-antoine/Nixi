using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;
using System.Collections.Generic;

namespace ScriptExample.Farms
{
    public sealed class FarmWithAllLevels : MonoBehaviourInjectable
    {
        // Children
        [NixInjectComponentsFromChildren]
        public List<Animal> AnimalsChildren;

        [NixInjectComponentsFromChildren]
        public List<Cat> CatsChildren;

        [NixInjectComponentsFromChildren]
        public IEnumerable<Animal> AnimalsEnumerableChildren;

        [NixInjectComponentsFromChildren]
        public IEnumerable<Cat> CatsEnumerableChildren;

        [NixInjectComponentsFromChildren]
        public Animal[] AnimalsArrayChildren;

        [NixInjectComponentsFromChildren]
        public Cat[] CatsArrayChildren;
        
        // Current
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

        // Parent
        [NixInjectComponentsFromParent]
        public List<Animal> AnimalsParent;

        [NixInjectComponentsFromParent]
        public List<Cat> CatsParent;

        [NixInjectComponentsFromParent]
        public IEnumerable<Animal> AnimalsEnumerableParent;

        [NixInjectComponentsFromParent]
        public IEnumerable<Cat> CatsEnumerableParent;

        [NixInjectComponentsFromParent]
        public Animal[] AnimalsArrayParent;

        [NixInjectComponentsFromParent]
        public Cat[] CatsArrayParent;
    }
}