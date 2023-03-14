using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using ScriptExample.Characters;

namespace ScriptExample.Spawners
{
    public sealed class Mob : MonoBehaviourInjectable
    {
        [Component]
        public Skill Skill;
    }
}