using Assets.ScriptExample.Characters.SameNamings;
using UnityEngine;

namespace Assets.Tests.Builders
{
    public sealed class RussianDollBuilder
    {
        private RussianDollBuilder()
        {
        }

        internal static RussianDollBuilder Create()
        {
            return new RussianDollBuilder();
        }

        internal RussianDoll Build()
        {
            return new GameObject("RussianDoll").AddComponent<RussianDoll>(); ;
        }

        internal RussianDollSameLevel BuildSameLevel()
        {
            return new GameObject("RussianDollSameLevel").AddComponent<RussianDollSameLevel>(); ;
        }

        internal RussianDollSameLevelParent BuildSameLevelParent()
        {
            return new GameObject("RussianDollSameLevelParent").AddComponent<RussianDollSameLevelParent>(); ;
        }
    }
}