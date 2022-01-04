using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.AllParentsCases
{
    public sealed class GrandParentWithChildListAndParent : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public ParentWithSameChildLists ParentWithSameChildLists;

        [NixInjectComponents]
        public List<Child> FirstChildList;
    }
}
