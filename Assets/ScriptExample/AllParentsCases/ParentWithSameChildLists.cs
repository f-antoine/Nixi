using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.AllParentsCases
{
    public sealed class ParentWithSameChildLists : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        public List<Child> FirstChildList;

        [NixInjectComponents]
        public List<Child> SecondChildList;
    }
}
