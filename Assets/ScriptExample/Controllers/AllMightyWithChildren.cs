using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Controllers
{
    // SorcererController
    //      -> FirstSorcerer
    //            -> FindMonsterController
    //      -> SecondSorcerer
    //            -> FindMonsterController
    // MonsterController
    //      -> SorcererController

    public sealed class AllMightyWithChildren : MonoBehaviourInjectable
    {
        [RootComponent("SorcererController", "FirstSorcerer")]
        public SorcererController FirstSorcerer;

        [RootComponent("SorcererController", "SecondSorcerer")]
        public SorcererController SecondSorcerer;
    }
}