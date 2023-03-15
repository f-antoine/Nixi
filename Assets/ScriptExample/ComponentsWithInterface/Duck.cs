using Nixi.Injections;

namespace ScriptExample.ComponentsWithInterface
{
    public sealed class Duck : MonoBehaviourInjectable
    {
        [Component]
        private IFlyBehavior wings;
        public IFlyBehavior Wings => wings;

        [ComponentFromChildren("Pocket")]
        private IDuckObjectContainer pocket;
        public IDuckObjectContainer Pocket => pocket;

        [ComponentFromParents("DuckCompany")]
        private IDuckObjectContainer duckCompanyBackPack;
        public IDuckObjectContainer DuckCompanyBackPack => duckCompanyBackPack;

        [RootComponent("FirstLake")]
        private ILakeProperty firstLake;
        public ILakeProperty FirstLake => firstLake;

        [RootComponent("Sky", "SecondLake")]
        private ILakeProperty secondLake;
        public ILakeProperty SecondLake => secondLake;
    }
}
