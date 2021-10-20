using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Players;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.ScriptExample.PlayerGroups
{
    public sealed class PlayerGroup : MonoBehaviourInjectable
    {
        [SerializeField]
        [NixInjectComponentList]
        private IEnumerable<Player> players;
        public IEnumerable<Player> Players => players;
    }
}