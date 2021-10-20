using Nixi.Injections;
using Nixi.Injections.Attributes;
using UnityEngine;

namespace Assets.ScriptExample.Characters
{
    public sealed class Weapon : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public Transform MainTransform;

        [NixInjectComponentFromMethod("ChildTransform", GameObjectMethod.GetComponentsInChildren)]
        public Transform ChildTransform;
    }
}