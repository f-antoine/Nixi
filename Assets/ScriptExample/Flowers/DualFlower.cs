using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Flowers
{
    public sealed class DualFlower : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("FlowerField")]
        public Flower FlowerField;

        [NixInjectRootComponent("FlowerField", "PerfectFlower")]
        public Flower PerfectFlower;
    }
}