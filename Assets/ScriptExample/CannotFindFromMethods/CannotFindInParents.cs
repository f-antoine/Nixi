using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;

namespace Assets.ScriptExample.CannotFindFromMethods
{
    public sealed class CannotFindInParents : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("CurrentName", GameObjectMethod.GetComponentsInParent)]
        public Skill SkillInChildren;
    }
}