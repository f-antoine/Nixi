using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
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