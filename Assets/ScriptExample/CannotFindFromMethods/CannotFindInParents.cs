using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using ScriptExample.Characters;

namespace ScriptExample.CannotFindFromMethods
{
    public sealed class CannotFindInParents : MonoBehaviourInjectable
    {
        [ComponentFromParents("CurrentName")]
        public Skill SkillInChildren;
    }
}