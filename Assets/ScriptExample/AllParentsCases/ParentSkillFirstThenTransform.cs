using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using ScriptExample.Characters;
using UnityEngine;

namespace ScriptExample.AllParentsCases
{
    public sealed class ParentSkillFirstThenTransform : MonoBehaviourInjectable
    {
        [Component]
        public Skill ASkill;

        [Component]
        public Transform ZTransform;
    }
}