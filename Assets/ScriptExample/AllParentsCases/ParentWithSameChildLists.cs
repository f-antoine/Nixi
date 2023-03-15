using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.AllParentsCases
{
    public sealed class ParentWithSameChildLists : MonoBehaviourInjectable
    {
        [Components]
        public List<Child> FirstChildList;

        [Components]
        public List<Child> SecondChildList;
    }
}
