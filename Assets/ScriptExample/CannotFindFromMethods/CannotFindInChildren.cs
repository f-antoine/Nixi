using Nixi.Injections;
using ScriptExample.Characters;

namespace ScriptExample.CannotFindFromMethods
{
    public sealed class CannotFindInChildren : MonoBehaviourInjectable
    {
        [ComponentFromChildren("CurrentName")]
        public Skill SkillInChildren;
    }
}
