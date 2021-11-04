using Nixi.Injections;
using Nixi.Injections.Attributes;
using UnityEngine;

namespace ScriptExample.Geometrics
{
    public sealed class TransformSquare : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public Transform Transform;
    }
}