using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using UnityEngine;

namespace ScriptExample.Geometrics
{
    public sealed class RectTransformSquare : MonoBehaviourInjectable
    {
        [Component]
        public RectTransform RectTransform;
    }
}