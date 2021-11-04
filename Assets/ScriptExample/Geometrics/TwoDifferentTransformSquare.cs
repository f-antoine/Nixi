using Nixi.Injections;
using Nixi.Injections.Attributes;
using UnityEngine;

namespace ScriptExample.Geometrics
{
    public sealed class TwoDifferentTransformSquare : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public Transform Transform;

        [NixInjectComponent]
        public RectTransform RectTransform;
    }
}