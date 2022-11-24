using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using ScriptExample.Characters;
using UnityEngine;

namespace ScriptExample.AllParentsCases
{
    public sealed class ParentTransformFirstThenSkill : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public Transform ATransform;

        [NixInjectComponent]
        public Skill ZSkill;
    }
}