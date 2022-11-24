using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;
using ScriptExample.Players;
using System.Collections.Generic;

namespace ScriptExample.PlayerGroups
{
    public sealed class PlayerGroupWithName : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        private IEnumerable<Player> players;
        public IEnumerable<Player> Players => players;

        [NixInjectComponents]
        private IEnumerable<Player> secondPlayers;
        public IEnumerable<Player> SecondPlayers => secondPlayers;
    }
}
