using UnityEngine;

namespace Tests.Builders
{
    internal sealed class CompoBuilder<T>
        where T : Component
    {
        internal static CompoBuilder<T> Create()
        {
            return new CompoBuilder<T>();
        }

        internal T Build()
        {
            return new GameObject($"{typeof(T).Name}Name").AddComponent<T>();
        }
    }
}