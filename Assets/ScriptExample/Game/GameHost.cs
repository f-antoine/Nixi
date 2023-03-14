using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using Nixi.Injections.Attributes.Fields;
using ScriptExample.Containers.Broken;
using ScriptExample.Containers.GameHost;
using ScriptExample.Players;
using UnityEngine;

namespace ScriptExample.Game
{
    public class GameHost : MonoBehaviourInjectable
    {
        [ComponentFromChildren("PlayerGameObjectName")]
        public Player Player;

        [ComponentFromChildren("SecondPlayerGameObjectName")]
        public SecondPlayer SecondPlayer;

        [FromContainer]
        public ISettings Settings;

        [FromContainer]
        public IBrokenTestInterface FirstBrokenInterfaceGameHost;

        [FromContainer]
        public IBrokenTestInterface SecondBrokenInterfaceGameHost;

        [SerializeField]
        private SO_GameHost soGameHostInfos;
        public SO_GameHost SOGameHostInfos => soGameHostInfos;
    }
}