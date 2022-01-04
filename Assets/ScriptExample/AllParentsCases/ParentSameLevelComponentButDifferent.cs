using Nixi.Injections;
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
