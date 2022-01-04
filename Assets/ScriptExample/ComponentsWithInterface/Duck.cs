using Nixi.Injections;

namespace ScriptExample.ComponentsWithInterface
{
    public sealed class Duck : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        private IFlyBehavior wings;
        public IFlyBehavior Wings => wings;

        [NixInjectComponentFromChildren("Pocket")]
        private IDuckObjectContainer pocket;
        public IDuckObjectContainer Pocket => pocket;

        [NixInjectComponentFromParent("DuckCompany")]
        private IDuckObjectContainer duckCompanyBackPack;
        public IDuckObjectContainer DuckCompanyBackPack => duckCompanyBackPack;

        [NixInjectRootComponent("FirstLake")]
        private ILakeProperty firstLake;
        public ILakeProperty FirstLake => firstLake;

        [NixInjectRootComponent("Sky", "SecondLake")]
        private ILakeProperty secondLake;
        public ILakeProperty SecondLake => secondLake;
    }
}
