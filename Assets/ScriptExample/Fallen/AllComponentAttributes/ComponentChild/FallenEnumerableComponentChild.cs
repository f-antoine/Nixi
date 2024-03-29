﻿using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenEnumerableComponentChild : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("any")]
        public IEnumerable<EmptyClass> FallenElement;
    }
}