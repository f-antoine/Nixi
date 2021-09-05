using Nixi.Injections.Injecters;
using NixiTestTools;
using ScriptExample.Characters;
using UnityEngine;

namespace Tests.Builders
{
    internal sealed class SorcererBuilder
    {
        private Sorcerer sorcerer;

        private SorcererBuilder()
        {
            sorcerer = new GameObject("SorcererGameObjectName").AddComponent<Sorcerer>();
        }

        internal static SorcererBuilder Create()
        {
            return new SorcererBuilder();
        }

        internal Sorcerer Build()
        {
            return sorcerer;
        }

        internal NixInjecter BuildNixInjecter()
        {
            return new NixInjecter(sorcerer);
        }

        internal TestInjecter BuildTestInjecter()
        {
            return new TestInjecter(sorcerer);
        }

        internal SorcererBuilder WithSkill()
        {
            sorcerer.gameObject.AddComponent<Skill>();
            return this;
        }

        internal SorcererBuilder WithChildSkill(string forcedName = "SorcererChildGameObjectName")
        {
            Skill skillChildComponent = new GameObject(forcedName).AddComponent<Skill>();
            skillChildComponent.transform.SetParent(sorcerer.transform);
            return this;
        }
    }
}