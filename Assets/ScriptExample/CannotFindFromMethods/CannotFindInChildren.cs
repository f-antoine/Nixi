using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;

namespace Assets.ScriptExample.CannotFindFromMethods
{
    public sealed class CannotFindInChildren : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("CurrentName", GameObjectMethod.GetComponentsInChildren)]
        public Skill SkillInChildren;
    }
}
