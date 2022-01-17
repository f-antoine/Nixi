using Nixi.Injections.Injectors;
using NixiTestTools;
using ScriptExample.Characters;
using ScriptExample.Players;
using UnityEngine;

namespace Tests.Builders
{
    internal sealed class PlayerBuilder
    {
        private Player player;

        private PlayerBuilder()
        {
            player = new GameObject("PlayerGameObjectName").AddComponent<Player>();
        }

        internal static PlayerBuilder Create()
        {
            return new PlayerBuilder();
        }

        internal Player Build()
        {
            return player;
        }

        internal NixInjector BuildDefaultInjector()
        {
            return new NixInjector(player);
        }

        internal TestInjector BuildTestInjector()
        {
            return new TestInjector(player);
        }

        internal PlayerBuilder WithSorcerer(Sorcerer sorcerer)
        {
            sorcerer.transform.SetParent(player.transform);
            return this;
        }
    }
}
