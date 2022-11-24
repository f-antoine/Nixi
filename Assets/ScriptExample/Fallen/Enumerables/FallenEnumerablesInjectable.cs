using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;
using ScriptExample.Characters;

namespace ScriptExample.Fallen.Enumerables
{
    public sealed class FallenEnumerablesInjectable : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        public Skill Fallen;
    }
}