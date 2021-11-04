using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;

namespace ScriptExample.Fallen.List
{
    public sealed class FallenCompoListInjectable : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        public Skill FallenSkill;
    }
}