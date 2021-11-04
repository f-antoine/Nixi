using Nixi.Injections;
using Nixi.Injections.Attributes;
using UnityEngine;

namespace ScriptExample.Geometrics
{
    public sealed class Square : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public Transform Transform;

        [NixInjectComponent]
        public Transform TransformSecond;

        [NixInjectComponent]
        public RectTransform RectTransform;
    }
}