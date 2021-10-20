using Assets.ScriptExample.Cargos;
using UnityEngine;

namespace Tests.Builders
{
    internal sealed class CargoBuilder
    {
        private Cargo cargo;

        private CargoBuilder()
        {
            cargo = new GameObject("CharacterGameObjectName").AddComponent<Cargo>();
        }

        internal static CargoBuilder Create()
        {
            return new CargoBuilder();
        }

        internal Cargo Build()
        {
            return cargo;
        }
    }
}