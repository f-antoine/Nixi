using Nixi.Injections;
using UnityEngine;

namespace ScriptExample.Geometrics
{
    public sealed class TwoSameRectTransformSquare : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public RectTransform RectTransform;

        [NixInjectComponent]
        public RectTransform SecondRectTransform;
    }
}