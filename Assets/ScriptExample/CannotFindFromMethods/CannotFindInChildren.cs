using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using ScriptExample.Characters;

namespace ScriptExample.CannotFindFromMethods
{
    public sealed class CannotFindInChildren : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("CurrentName")]
        public Skill SkillInChildren;
    }
}
