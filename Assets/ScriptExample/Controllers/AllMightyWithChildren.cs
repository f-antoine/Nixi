﻿using Nixi.Injections;

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
        [NixInjectRootComponent("SorcererController", "FirstSorcerer")]
        public SorcererController FirstSorcerer;

        [NixInjectRootComponent("SorcererController", "SecondSorcerer")]
        public SorcererController SecondSorcerer;
    }
}