using NixiTestTools;
using ScriptExample.Characters.Broken;
using UnityEngine;

namespace Tests.Builders
{
    internal sealed class BrokenSorcererBuilder
    {
        private BrokenSorcerer brokenSorcerer;

        private BrokenSorcererBuilder()
        {
            brokenSorcerer = new GameObject("BrokenSorcererGameObjectName").AddComponent<BrokenSorcerer>();
        }

        internal static BrokenSorcererBuilder Create()
        {
            return new BrokenSorcererBuilder();
        }

        internal BrokenSorcerer Build()
        {
            return brokenSorcerer;
        }

        internal TestInjecter BuildTestInjecter()
        {
            return new TestInjecter(brokenSorcerer);
        }
    }
}