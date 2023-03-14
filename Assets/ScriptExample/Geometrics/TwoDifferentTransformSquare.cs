using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using UnityEngine;

namespace ScriptExample.Geometrics
{
    public sealed class TwoDifferentTransformSquare : MonoBehaviourInjectable
    {
        [Component]
        public Transform Transform;

        [Component]
        public RectTransform RectTransform;
    }
}