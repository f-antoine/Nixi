using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;

namespace ScriptExample.Spawners
{
    public sealed class Mob : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public Skill Skill;
    }
}