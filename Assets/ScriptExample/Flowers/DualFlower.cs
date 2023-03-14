using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Flowers
{
    public sealed class DualFlower : MonoBehaviourInjectable
    {
        [RootComponent("FlowerField")]
        public Flower FlowerField;

        [RootComponent("FlowerField", "PerfectFlower")]
        public Flower PerfectFlower;
    }
}