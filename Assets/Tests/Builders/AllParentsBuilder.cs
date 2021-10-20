using Assets.ScriptExample.AllParentsCases;
using UnityEngine;

namespace Tests.Builders
{
    internal sealed class AllParentsBuilder
    {
        internal static AllParentsBuilder Create()
        {
            return new AllParentsBuilder();
        }

        internal Parent BuildParent()
        {
            return new GameObject("ParentName").AddComponent<Parent>();
        }

        internal ParentWithSameChildLists BuildParentWithSameChildLists()
        {
            return new GameObject("ParentWithSameChildListsName").AddComponent<ParentWithSameChildLists>();
        }

        internal ParentWithSameChildListsDifferentsEnumerables BuildParentWithSameChildListsDifferentsEnumerables()
        {
            return new GameObject("ParentWithSameChildListsDifferentsEnumerablesName").AddComponent<ParentWithSameChildListsDifferentsEnumerables>();
        }

        internal GrandParentWithChildListAndParent BuildGrandParentWithChildListAndParent()
        {
            return new GameObject("GrandParentWithChildListAndParentName").AddComponent<GrandParentWithChildListAndParent>();
        }
    }
}
