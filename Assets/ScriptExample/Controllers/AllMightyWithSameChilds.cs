using Nixi.Injections;
using Nixi.Injections.Attributes.MonoBehaviours;

namespace Assets.ScriptExample.Controllers
{
    public sealed class AllMightyWithSameChilds : MonoBehaviourInjectable
    {
        [NixInjectMonoBehaviourFromMethodRoot("SorcererController", "FirstSorcerer")]
        public SorcererController FirstSorcerer;

        [NixInjectMonoBehaviourFromMethodRoot("SorcererController", "FirstSorcerer")]
        public SorcererController FirstSorcererDuplicate;
    }
}
