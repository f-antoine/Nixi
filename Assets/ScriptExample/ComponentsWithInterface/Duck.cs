using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.ComponentsWithInterface
{
    public sealed class Duck : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        private IFlyBehavior wings;
        public IFlyBehavior Wings => wings;

        [NixInjectComponentFromMethod("Pocket", GameObjectMethod.GetComponentsInChildren)]
        private IDuckObjectContainer pocket;
        public IDuckObjectContainer Pocket => pocket;

        [NixInjectComponentFromMethod("DuckCompany", GameObjectMethod.GetComponentsInParent)]
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
