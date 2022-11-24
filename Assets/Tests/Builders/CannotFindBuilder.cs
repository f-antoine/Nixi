using ScriptExample.CannotFindFromMethods;
using ScriptExample.Characters;
using UnityEngine;

namespace Tests.Builders
{
    internal sealed class CannotFindBuilder
    {
        internal static CannotFindBuilder Create()
        {
            return new CannotFindBuilder();
        }

        internal CannotFindInChildren BuildForChildWithCurrentName()
        {
            GameObject gameObject = new GameObject("CurrentName");
            gameObject.AddComponent<Skill>();
            return gameObject.AddComponent<CannotFindInChildren>();
        }

        internal CannotFindInParents BuildForParentWithCurrentName()
        {
            GameObject gameObject = new GameObject("CurrentName");
            gameObject.AddComponent<Skill>();
            return gameObject.AddComponent<CannotFindInParents>();
        }
    }
}