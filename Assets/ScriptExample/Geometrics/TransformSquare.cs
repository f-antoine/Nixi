using Nixi.Injections;
using UnityEngine;

namespace ScriptExample.Geometrics
{
    public sealed class TransformSquare : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public Transform Transform;
    }
}