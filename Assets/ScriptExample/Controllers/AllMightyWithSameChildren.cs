using Nixi.Injections;

namespace ScriptExample.Controllers
{
    public sealed class AllMightyWithSameChildren : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("SorcererController", "FirstSorcerer")]
        public SorcererController FirstSorcerer;

        [NixInjectRootComponent("SorcererController", "FirstSorcerer")]
        public SorcererController FirstSorcererDuplicate;
    }
}
