using Nixi.Injections;
using Nixi.Injections.Attributes.Fields;
using Nixi.Injections.Attributes.MonoBehaviours;
using ScriptExample.Containers.Broken;
using ScriptExample.Containers.GameHost;
using ScriptExample.Game;
using ScriptExample.Players;
using UnityEngine;

namespace ScriptExample.GameHost
{
    public class GameHost : MonoBehaviourInjectable
    {
        [NixInjectMonoBehaviourFromMethod("PlayerGameObjectName", GameObjectMethod.GetComponentsInChildren)]
        public Player Player;

        [NixInjectMonoBehaviourFromMethod("SecondPlayerGameObjectName", GameObjectMethod.GetComponentsInChildren)]
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