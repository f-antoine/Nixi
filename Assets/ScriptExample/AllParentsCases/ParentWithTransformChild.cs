using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using UnityEngine;

namespace ScriptExample.AllParentsCases
{
    public sealed class ParentWithTransformChild : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public Transform FirstTransform;

        [NixInjectComponent]
        public Transform SecondTransform;
    }
}
