using Nixi.Injections;
using UnityEngine;

namespace ScriptExample.AllParentsCases
{
    public sealed class ParentWithTransformChild : MonoBehaviourInjectable
    {
        [Component]
        public Transform FirstTransform;

        [Component]
        public Transform SecondTransform;
    }
}
