using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using ScriptExample.Characters;

namespace ScriptExample.CannotFindFromMethods
{
    public sealed class CannotFindInParents : MonoBehaviourInjectable
    {
        [NixInjectComponentFromParent("CurrentName")]
        public Skill SkillInChildren;
    }
}