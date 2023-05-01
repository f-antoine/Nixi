using Nixi.Injections;
using ScriptExample.Characters.Broken;
using ScriptExample.Containers.Broken;

namespace ScriptExample.Players
{
    public sealed class SecondPlayer : MonoBehaviourInjectable
    {
        [FromContainer]
        public IBrokenTestInterface FirstBrokenInterfacePlayer;

        [FromContainer]
        public IBrokenTestInterface SecondBrokenInterfacePlayer;

        [Component]
        public BrokenSorcerer BrokenSorcerer;
    }
}