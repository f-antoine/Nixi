using Nixi.Injections;
using ScriptExample.Characters;

namespace ScriptExample.CannotFindFromMethods
{
    public sealed class CannotFindInParents : MonoBehaviourInjectable
    {
        [ComponentFromParents("CurrentName")]
        public Skill SkillInChildren;
    }
}