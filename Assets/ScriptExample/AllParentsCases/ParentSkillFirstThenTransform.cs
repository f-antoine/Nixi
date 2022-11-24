using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using ScriptExample.Characters;
using UnityEngine;

namespace ScriptExample.AllParentsCases
{
    public sealed class ParentSkillFirstThenTransform : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public Skill ASkill;

        [NixInjectComponent]
        public Transform ZTransform;
    }
}