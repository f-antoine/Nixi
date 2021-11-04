using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;

namespace ScriptExample.CannotFindFromMethods
{
    public sealed class CannotFindInChildren : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("CurrentName")]
        public Skill SkillInChildren;
    }
}
