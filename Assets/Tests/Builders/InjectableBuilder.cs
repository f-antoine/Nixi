using Nixi.Injections;
using Nixi.Injections.Injectors;
using NixiTestTools;
using UnityEngine;

namespace Tests.Builders
{
    internal sealed class InjectableBuilder<T>
        where T : MonoBehaviourInjectable
    {
        internal static InjectableBuilder<T> Create()
        {
            return new InjectableBuilder<T>();
        }

        internal T Build()
        {
            return new GameObject($"{typeof(T).Name}Name").AddComponent<T>();
        }

        internal NixInjector BuildNixInjector()
        {
            return new NixInjector(new GameObject($"{typeof(T).Name}Name").AddComponent<T>());
        }

        internal TestInjector BuildTestInjector()
        {
            return new TestInjector(new GameObject($"{typeof(T).Name}Name").AddComponent<T>());
        }
    }
}