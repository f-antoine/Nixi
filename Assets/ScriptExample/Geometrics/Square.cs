using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using UnityEngine;

namespace ScriptExample.Geometrics
{
    public sealed class Square : MonoBehaviourInjectable
    {
        [Component]
        public Transform Transform;

        [Component]
        public Transform TransformSecond;

        [Component]
        public RectTransform RectTransform;
    }
}