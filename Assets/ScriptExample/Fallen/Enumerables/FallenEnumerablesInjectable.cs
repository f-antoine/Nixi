using Nixi.Injections;
using ScriptExample.Characters;

namespace ScriptExample.Fallen.Enumerables
{
    public sealed class FallenEnumerablesInjectable : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        public Skill Fallen;
    }
}