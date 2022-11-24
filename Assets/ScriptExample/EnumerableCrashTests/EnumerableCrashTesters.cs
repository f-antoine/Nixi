using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;
using ScriptExample.ComponentsWithEnumerable;
using System.Collections.Generic;
using System.Linq;

namespace ScriptExample.EnumerableCrashTests
{
    public sealed class EnumerableCrashTesters : MonoBehaviourInjectable
    {
        [NixInjectComponentsFromChildren]
        private List<Fruit> fruitsChildren;
        public List<Fruit> FruitsChildren => fruitsChildren.ToList();
        public Fruit[] ArrayFruitsChildren => fruitsChildren.ToArray();
        public IEnumerable<Fruit> EnumerableFruitsChildren => fruitsChildren.AsEnumerable();
        public IReadOnlyList<Fruit> ReadOnlyFruitsChildren => fruitsChildren.AsReadOnly();

        [NixInjectComponents]
        private IEnumerable<Fruit> fruits;
        public List<Fruit> Fruits => fruits.ToList();
        public Fruit[] ArrayFruits => fruits.ToArray();
        public IEnumerable<Fruit> EnumerableFruits => fruits.AsEnumerable();
        public IReadOnlyList<Fruit> ReadOnlyFruits => fruits.ToList().AsReadOnly();

        [NixInjectComponentsFromParent]
        private Fruit[] fruitsParent;
        public List<Fruit> FruitsParent => fruitsParent.ToList();
        public Fruit[] ArrayFruitsParent => fruitsParent.ToArray();
        public IEnumerable<Fruit> EnumerableFruitsParent => fruitsParent.AsEnumerable();
        public IReadOnlyList<Fruit> ReadOnlyFruitsParent => fruitsParent.ToList().AsReadOnly();
    }   
}