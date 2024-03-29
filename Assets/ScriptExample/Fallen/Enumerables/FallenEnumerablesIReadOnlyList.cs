﻿using Nixi.Injections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ScriptExample.Fallen.Enumerables
{
    public sealed class FallenEnumerablesIReadOnlyList : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        public IReadOnlyList<Slider> Fallen;
    }
}