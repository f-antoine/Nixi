using Nixi.Injections;
using Nixi.Injections.Attributes;
using UnityEngine;

namespace ScriptExample.Geometrics
{
    public sealed class TwoSameTransformSquare : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public Transform Transform;

        [NixInjectComponent]
        public Transform SecondTransform;
    }
}