using Nixi.Injections;
using Nixi.Injections.Attributes.Fields;
using Nixi.Injections.Attributes.ComponentFields;
using ScriptExample.Characters.Broken;
using ScriptExample.Containers.Broken;

namespace ScriptExample.Players
{
    public sealed class SecondPlayer : MonoBehaviourInjectable
    {
        [NixInject]
        public IBrokenTestInterface FirstBrokenInterfacePlayer;

        [NixInject]
        public IBrokenTestInterface SecondBrokenInterfacePlayer;

        [NixInjectComponent]
        public BrokenSorcerer BrokenSorcerer;
    }
}