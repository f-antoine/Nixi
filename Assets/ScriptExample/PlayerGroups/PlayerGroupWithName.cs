using Nixi.Injections;
using ScriptExample.Players;
using System.Collections.Generic;

namespace ScriptExample.PlayerGroups
{
    public sealed class PlayerGroupWithName : MonoBehaviourInjectable
    {
        [Components]
        private IEnumerable<Player> players;
        public IEnumerable<Player> Players => players;

        [Components]
        private IEnumerable<Player> secondPlayers;
        public IEnumerable<Player> SecondPlayers => secondPlayers;
    }
}
