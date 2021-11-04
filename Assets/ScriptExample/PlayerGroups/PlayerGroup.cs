using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Players;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptExample.PlayerGroups
{
    public sealed class PlayerGroup : MonoBehaviourInjectable
    {
        [SerializeField]
        [NixInjectComponents]
        private IEnumerable<Player> players;
        public IEnumerable<Player> Players => players;
    }
}