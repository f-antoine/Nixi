using Nixi.Injections;
using ScriptExample.Characters.Broken;
using ScriptExample.Containers.Broken;

namespace ScriptExample.Players
{
    public sealed class SecondPlayer : MonoBehaviourInjectable
    {
        [NixInjectFromContainer]
        public IBrokenTestInterface FirstBrokenInterfacePlayer;

        [NixInjectFromContainer]
        public IBrokenTestInterface SecondBrokenInterfacePlayer;

        [NixInjectComponent]
        public BrokenSorcerer BrokenSorcerer;
    }
}