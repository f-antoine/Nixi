﻿using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.Characters.SameNamings
{
    public sealed class RussianDollSameLevelParent : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("Doll")]
        public FirstDoll FirstDoll;

        [NixInjectRootComponent("Doll")]
        public FirstDoll FirstDollDuplicate;
    }
}