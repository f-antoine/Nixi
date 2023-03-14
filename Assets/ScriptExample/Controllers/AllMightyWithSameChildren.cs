using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Controllers
{
    public sealed class AllMightyWithSameChildren : MonoBehaviourInjectable
    {
        [RootComponent("SorcererController", "FirstSorcerer")]
        public SorcererController FirstSorcerer;

        [RootComponent("SorcererController", "FirstSorcerer")]
        public SorcererController FirstSorcererDuplicate;
    }
}
