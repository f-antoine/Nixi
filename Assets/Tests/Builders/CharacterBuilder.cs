using Nixi.Injections.Injecters;
using NixiTestTools;
using ScriptExample.Characters;
using UnityEngine;

namespace Tests.Builders
{
    internal sealed class CharacterBuilder
    {
        private Character character;

        private CharacterBuilder()
        {
            character = new GameObject("CharacterGameObjectName").AddComponent<Character>();
        }

        internal static CharacterBuilder Create()
        {
            return new CharacterBuilder();
        }

        internal Character Build()
        {
            return character;
        }

        internal NixInjecter BuildDefaultInjecter()
        {
            return new NixInjecter(character);
        }

        internal TestInjecter BuildTestInjecter()
        {
            return new TestInjecter(character);
        }

        internal CharacterBuilder WithSkill()
        {
            character.gameObject.AddComponent<Skill>();
            return this;
        }
    }
}