using Assets.ScriptExample.PlayerGroups;
using UnityEngine;

namespace Tests.Builders
{
    internal sealed class PlayerGroupBuilder
    {
        private PlayerGroup playerGroup;

        private PlayerGroupBuilder()
        {
            playerGroup = new GameObject("PlayerGroupName").AddComponent<PlayerGroup>();
        }

        internal static PlayerGroupBuilder Create()
        {
            return new PlayerGroupBuilder();
        }

        internal PlayerGroup Build()
        {
            return playerGroup;
        }

        internal PlayerGroupWithName BuildWithName()
        {
            return new GameObject("PlayerGroupWithNameName").AddComponent<PlayerGroupWithName>();
        }
    }
}