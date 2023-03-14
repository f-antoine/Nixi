using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
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
