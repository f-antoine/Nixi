using Nixi.Injections;
using ScriptExample.Players;
using System.Collections.Generic;

namespace ScriptExample.PlayerGroups
{
    public sealed class PlayerGroup : MonoBehaviourInjectable
    {
        [Components]
        private IEnumerable<Player> players;
        public IEnumerable<Player> Players => players;
    }
}