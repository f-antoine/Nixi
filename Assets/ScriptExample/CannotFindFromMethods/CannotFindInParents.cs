using Nixi.Injections;
using ScriptExample.Characters;

namespace ScriptExample.CannotFindFromMethods
{
    public sealed class CannotFindInParents : MonoBehaviourInjectable
    {
        [NixInjectComponentFromParent("CurrentName")]
        public Skill SkillInChildren;
    }
}