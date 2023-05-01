using Nixi.Injections;

namespace ScriptExample.Flowers
{
    public sealed class PlagueBouquet : MonoBehaviourInjectable
    {
        [RootComponent("FlowerField")]
        public Flower HealthyFlower;

        [RootComponent("FlowerField")]
        public PlagueFlower PlagueFlower;

        [RootComponent("FlowerField", "SubFlowerField")]
        public Flower SubHealthyFlower;

        [RootComponent("FlowerField", "SubFlowerField")]
        public PlagueFlower SubPlagueFlower;
    }
}