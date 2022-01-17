using Nixi.Injections.Injectors;
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

        internal NixInjector BuildNixInjector()
        {
            return new NixInjector(sorcerer);
        }

        internal TestInjector BuildTestInjector()
        {
            return new TestInjector(sorcerer);
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