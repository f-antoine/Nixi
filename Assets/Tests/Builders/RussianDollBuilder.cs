using Assets.ScriptExample.Characters.SameNamings;
using UnityEngine;

namespace Tests.Builders
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
            return new GameObject("RussianDollName").AddComponent<RussianDoll>(); ;
        }

        internal RussianDollSameLevel BuildSameLevel()
        {
            return new GameObject("RussianDollSameLevelName").AddComponent<RussianDollSameLevel>(); ;
        }

        internal RussianDollSameLevelParent BuildSameLevelParent()
        {
            return new GameObject("RussianDollSameLevelParentName").AddComponent<RussianDollSameLevelParent>(); ;
        }
    }
}