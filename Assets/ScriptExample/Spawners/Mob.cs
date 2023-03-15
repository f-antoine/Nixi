using Nixi.Injections;
using ScriptExample.Characters;

namespace ScriptExample.Spawners
{
    public sealed class Mob : MonoBehaviourInjectable
    {
        [Component]
        public Skill Skill;
    }
}