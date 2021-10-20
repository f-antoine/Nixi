using System;
using UnityEngine;

namespace Assets.Tests.Builders
{
    internal sealed class CompoBuilderWithExpliciteType
    {
        internal static CompoBuilderWithExpliciteType Create()
        {
            return new CompoBuilderWithExpliciteType();
        }

        internal Component Build(Type typeToBuild)
        {
            return new GameObject($"{typeToBuild.Name}Name").AddComponent(typeToBuild);
        }
    }
}
