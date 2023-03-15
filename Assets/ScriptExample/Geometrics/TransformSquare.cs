using Nixi.Injections;
using UnityEngine;

namespace ScriptExample.Geometrics
{
    public sealed class TransformSquare : MonoBehaviourInjectable
    {
        [Component]
        public Transform Transform;
    }
}