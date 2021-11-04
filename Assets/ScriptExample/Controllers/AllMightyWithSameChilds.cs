using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Controllers
{
    public sealed class AllMightyWithSameChilds : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("SorcererController", "FirstSorcerer")]
        public SorcererController FirstSorcerer;

        [NixInjectRootComponent("SorcererController", "FirstSorcerer")]
        public SorcererController FirstSorcererDuplicate;
    }
}
