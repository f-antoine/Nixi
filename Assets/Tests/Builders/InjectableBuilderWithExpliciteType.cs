using System;
using UnityEngine;

namespace Tests.Builders
{
    internal sealed class InjectableBuilderWithExpliciteType
    {
        internal static InjectableBuilderWithExpliciteType Create()
        {
            return new InjectableBuilderWithExpliciteType();
        }

        internal Component Build(Type typeToBuild)
        {
            return new GameObject($"{typeToBuild.Name}Name").AddComponent(typeToBuild);
        }
    }
}
