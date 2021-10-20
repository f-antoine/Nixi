using Nixi.Injections;
using Nixi.Injections.Attributes.Fields;
using Nixi.Injections.Attributes.ComponentFields;
using ScriptExample.Containers.Broken;
using ScriptExample.Containers.GameHost;
using ScriptExample.Game;
using ScriptExample.Players;
using UnityEngine;

namespace ScriptExample.GameHost
{
    public class GameHost : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("PlayerGameObjectName", GameObjectMethod.GetComponentsInChildren)]
        public Player Player;

        [NixInjectComponentFromMethod("SecondPlayerGameObjectName", GameObjectMethod.GetComponentsInChildren)]
        public SecondPlayer SecondPlayer;

        [NixInject]
        public ISettings Settings;

        [NixInject]
        public IBrokenTestInterface FirstBrokenInterfaceGameHost;

        [NixInject]
        public IBrokenTestInterface SecondBrokenInterfaceGameHost;

        [NixInject(NixInjectType.DoesNotFillButExposeForTesting)]
        [SerializeField]
        private SO_GameHost soGameHostInfos;
        public SO_GameHost SOGameHostInfos => soGameHostInfos;
    }
}