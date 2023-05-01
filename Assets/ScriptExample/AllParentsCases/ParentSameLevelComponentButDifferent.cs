using Nixi.Injections;
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
