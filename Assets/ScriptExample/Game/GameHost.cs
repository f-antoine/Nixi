using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Containers.Broken;
using ScriptExample.Containers.GameHost;
using ScriptExample.Game;
using ScriptExample.Players;
using UnityEngine;

namespace ScriptExample.GameHost
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