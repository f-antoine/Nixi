using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using Nixi.Injections.Attributes.Fields;
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