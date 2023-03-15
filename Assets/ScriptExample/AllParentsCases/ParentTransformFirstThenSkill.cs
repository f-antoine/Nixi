using Nixi.Injections;
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