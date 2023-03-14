using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using ScriptExample.Characters;

namespace ScriptExample.AllParentsCases
{
    public sealed class ParentSameLevelComponentButDifferent : MonoBehaviourInjectable
    {
        [Component]
        public Child Child;

        [Component]
        public Skill Skill;
    }
}
