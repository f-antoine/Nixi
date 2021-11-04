using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Players;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptExample.PlayerGroups
{
    public sealed class PlayerGroupWithName : MonoBehaviourInjectable
    {
        [SerializeField]
        [NixInjectComponents]
        private IEnumerable<Player> players;
        public IEnumerable<Player> Players => players;

        [SerializeField]
        [NixInjectComponents]
        private IEnumerable<Player> secondPlayers;
        public IEnumerable<Player> SecondPlayers => secondPlayers;
    }
}
