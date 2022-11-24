using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Flowers
{
    public sealed class PlagueBouquet : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("FlowerField")]
        public Flower HealthyFlower;

        [NixInjectRootComponent("FlowerField")]
        public PlagueFlower PlagueFlower;

        [NixInjectRootComponent("FlowerField", "SubFlowerField")]
        public Flower SubHealthyFlower;

        [NixInjectRootComponent("FlowerField", "SubFlowerField")]
        public PlagueFlower SubPlagueFlower;
    }
}