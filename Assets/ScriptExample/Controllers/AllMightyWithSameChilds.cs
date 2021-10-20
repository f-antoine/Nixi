﻿using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields;

namespace Assets.ScriptExample.Controllers
{
    public sealed class AllMightyWithSameChilds : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("SorcererController", "FirstSorcerer")]
        public SorcererController FirstSorcerer;

        [NixInjectRootComponent("SorcererController", "FirstSorcerer")]
        public SorcererController FirstSorcererDuplicate;
    }
}
