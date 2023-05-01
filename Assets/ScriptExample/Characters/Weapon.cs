using Nixi.Injections;
using UnityEngine;

namespace ScriptExample.Characters
{
    public sealed class Weapon : MonoBehaviourInjectable
    {
        [Component]
        public Transform MainTransform;

        [ComponentFromChildren("ChildTransform")]
        public Transform ChildTransform;
    }
}