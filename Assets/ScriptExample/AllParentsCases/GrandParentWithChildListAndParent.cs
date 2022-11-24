using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
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
