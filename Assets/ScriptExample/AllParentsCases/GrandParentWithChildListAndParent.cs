using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.AllParentsCases
{
    public sealed class GrandParentWithChildListAndParent : MonoBehaviourInjectable
    {
        [Component]
        public ParentWithSameChildLists ParentWithSameChildLists;

        [Components]
        public List<Child> FirstChildList;
    }
}
