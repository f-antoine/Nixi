using Nixi.Injections;
using UnityEngine;

namespace ScriptExample.Geometrics
{
    public sealed class TwoSameRectTransformSquare : MonoBehaviourInjectable
    {
        [Component]
        public RectTransform RectTransform;

        [Component]
        public RectTransform SecondRectTransform;
    }
}