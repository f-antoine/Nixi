﻿using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.Fallen.Enumerables
{
    public sealed class FallenEnumerablesNonComponentList : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        public List<int> Fallen;
    }
}