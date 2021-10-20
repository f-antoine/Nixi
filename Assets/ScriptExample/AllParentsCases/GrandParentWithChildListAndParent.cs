using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;

namespace Assets.ScriptExample.AllParentsCases
{
    public sealed class GrandParentWithChildListAndParent : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public ParentWithSameChildLists ParentWithSameChildLists;

        [NixInjectComponentList]
        public List<Child> FirstChildList;
    }
}
