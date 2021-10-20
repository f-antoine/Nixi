using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;

namespace Assets.ScriptExample.AllParentsCases
{
    public sealed class ParentWithSameChildLists : MonoBehaviourInjectable
    {
        [NixInjectComponentList]
        public List<Child> FirstChildList;

        [NixInjectComponentList]
        public List<Child> SecondChildList;
    }
}
