using Nixi.Injections;
using Nixi.Injections.Injecters;
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

        internal NixInjecter BuildNixInjecter()
        {
            return new NixInjecter(new GameObject($"{typeof(T).Name}Name").AddComponent<T>());
        }

        internal TestInjecter BuildTestInjecter()
        {
            return new TestInjecter(new GameObject($"{typeof(T).Name}Name").AddComponent<T>());
        }
    }
}