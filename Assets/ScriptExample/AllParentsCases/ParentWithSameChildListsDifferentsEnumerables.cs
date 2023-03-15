using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.AllParentsCases
{
    public sealed class ParentWithSameChildListsDifferentsEnumerables : MonoBehaviourInjectable
    {
        [Components]
        public List<Child> FirstChildList;

        [Components]
        public IEnumerable<Child> SecondChildList;

        [Components]
        public Child[] ThirdChildArray;
    }
}
