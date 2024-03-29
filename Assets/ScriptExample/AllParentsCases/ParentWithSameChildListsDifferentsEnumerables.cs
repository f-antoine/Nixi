﻿using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.AllParentsCases
{
    public sealed class ParentWithSameChildListsDifferentsEnumerables : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        public List<Child> FirstChildList;

        [NixInjectComponents]
        public IEnumerable<Child> SecondChildList;

        [NixInjectComponents]
        public Child[] ThirdChildArray;
    }
}
