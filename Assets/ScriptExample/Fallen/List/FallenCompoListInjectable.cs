using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;

namespace Assets.ScriptExample.Fallen.List
{
    public sealed class FallenCompoListInjectable : MonoBehaviourInjectable
    {
        [NixInjectComponentList]
        public Skill FallenSkill;
    }
}