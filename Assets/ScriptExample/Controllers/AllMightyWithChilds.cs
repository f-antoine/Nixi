using Nixi.Injections;
using Nixi.Injections.Attributes.MonoBehaviours;

namespace Assets.ScriptExample.Controllers
{
    public sealed class AllMightyWithChilds : MonoBehaviourInjectable
    {
        [NixInjectMonoBehaviourFromMethodRoot("SorcererController", "FirstSorcerer")]
        public SorcererController FirstSorcerer;

        [NixInjectMonoBehaviourFromMethodRoot("SorcererController", "SecondSorcerer")]
        public SorcererController SecondSorcerer;
    }
}