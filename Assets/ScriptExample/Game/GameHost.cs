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
        [NixInjectComponentFromChildren("PlayerGameObjectName")]
        public Player Player;

        [NixInjectComponentFromChildren("SecondPlayerGameObjectName")]
        public SecondPlayer SecondPlayer;

        [NixInjectFromContainer]
        public ISettings Settings;

        [NixInjectFromContainer]
        public IBrokenTestInterface FirstBrokenInterfaceGameHost;

        [NixInjectFromContainer]
        public IBrokenTestInterface SecondBrokenInterfaceGameHost;

        [SerializeField]
        private SO_GameHost soGameHostInfos;
        public SO_GameHost SOGameHostInfos => soGameHostInfos;
    }
}