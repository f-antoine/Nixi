﻿using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;

namespace Assets.ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenEnumerableComponent : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public IEnumerable<EmptyClass> FallenElement;
    }
}