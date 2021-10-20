using Assets.ScriptExample.Characters;
using UnityEngine;

namespace Tests.Builders
{
    internal sealed class WarriorBuilder
    {
        private Warrior warrior;

        private WarriorBuilder()
        {
            warrior = new GameObject("WarriorName").AddComponent<Warrior>();
        }

        internal static WarriorBuilder Create()
        {
            return new WarriorBuilder();
        }

        internal Warrior Build()
        {
            return warrior;
        }
    }
}