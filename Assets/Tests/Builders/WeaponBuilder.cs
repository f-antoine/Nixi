using Assets.ScriptExample.Characters;
using UnityEngine;

namespace Tests.Builders
{
    internal sealed class WeaponBuilder
    {
        private Weapon weapon;

        private WeaponBuilder()
        {
            weapon = new GameObject("WeaponName").AddComponent<Weapon>();
        }

        internal static WeaponBuilder Create()
        {
            return new WeaponBuilder();
        }

        internal Weapon Build()
        {
            return weapon;
        }
    }
}
