using Nixi.Injections;
using Nixi.Injections.Attributes;
using UnityEngine;

namespace ScriptExample.Characters
{
    public sealed class Weapon : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public Transform MainTransform;

        [NixInjectComponentFromChildren("ChildTransform")]
        public Transform ChildTransform;
    }
}