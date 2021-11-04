using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Controllers
{
    // SorcererController
    //      -> FirstSorcerer
    //            -> FindMonsterController
    //      -> SecondSorcerer
    //            -> FindMonsterController
    // MonsterController
    //      -> SorcererController

    public sealed class AllMightyWithChilds : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("SorcererController", "FirstSorcerer")]
        public SorcererController FirstSorcerer;

        [NixInjectRootComponent("SorcererController", "SecondSorcerer")]
        public SorcererController SecondSorcerer;
    }
}