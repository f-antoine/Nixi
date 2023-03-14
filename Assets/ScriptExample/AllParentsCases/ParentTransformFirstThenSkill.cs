using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using ScriptExample.Characters;
using UnityEngine;

namespace ScriptExample.AllParentsCases
{
    public sealed class ParentTransformFirstThenSkill : MonoBehaviourInjectable
    {
        [Component]
        public Transform ATransform;

        [Component]
        public Skill ZSkill;
    }
}