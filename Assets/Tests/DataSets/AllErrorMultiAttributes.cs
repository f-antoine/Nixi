using Assets.ScriptExample.ErrorMultiAttributes;
using System.Collections;
using UnityEngine;

namespace Assets.Tests.DataSets
{
    public sealed class AllErrorMultiAttributes : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new object[] { new GameObject().AddComponent<FieldWithMono>() };
            yield return new object[] { new GameObject().AddComponent<FieldWithMonoFromMethod>() };
            yield return new object[] { new GameObject().AddComponent<FieldWithMonoFromMethodRoot>() };
            yield return new object[] { new GameObject().AddComponent<MonoFromMethodWithMonoFromMethodRoot>() };
            yield return new object[] { new GameObject().AddComponent<MonoWithMonoFromMethod>() };
            yield return new object[] { new GameObject().AddComponent<MonoWithMonoFromMethodRoot>() };
            yield return new object[] { new GameObject().AddComponent<AllMonoAttributes>() };
        }
    }
}