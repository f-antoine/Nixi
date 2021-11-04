using ScriptExample.Characters;
using Nixi.Injections.Injecters;
using NixiTestTools;
using ScriptExample.Characters;
using UnityEngine;

namespace Tests.Builders
{
    internal sealed class ParasiteBuilder
    {
        private Sorcerer parentSorcerer;
        private Parasite parasite;

        private ParasiteBuilder()
        {
            parasite = new GameObject("ParasiteGameObjectName").AddComponent<Parasite>();
        }

        internal static ParasiteBuilder Create()
        {
            return new ParasiteBuilder();
        }

        internal Parasite Build()
        {
            return parasite;
        }

        internal NixInjecter BuildDefaultInjecter()
        {
            return new NixInjecter(parasite);
        }

        internal ParasiteBuilder WithParentSorcerer()
        {
            parentSorcerer = new GameObject("ParentSorcererGameObjectName").AddComponent<Sorcerer>();
            parasite.transform.SetParent(parentSorcerer.transform);
            return this;
        }
    }
}