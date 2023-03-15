using Nixi.Injections;
using UnityEngine;

namespace ScriptExample.Geometrics
{
    public sealed class RectTransformSquare : MonoBehaviourInjectable
    {
        [Component]
        public RectTransform RectTransform;
    }
}