using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using ScriptExample.Characters;

namespace ScriptExample.AllParentsCases
{
    public sealed class ParentSameLevelComponentButDifferent : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public Child Child;

        [NixInjectComponent]
        public Skill Skill;
    }
}
