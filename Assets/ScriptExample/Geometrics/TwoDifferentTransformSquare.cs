using Nixi.Injections;
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