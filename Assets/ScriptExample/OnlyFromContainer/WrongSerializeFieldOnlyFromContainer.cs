using Nixi.Injections;
using ScriptExample.Containers;
using UnityEngine;

namespace ScriptExample.OnlyFromContainer
{
    public sealed class WrongSerializeFieldOnlyFromContainer : OnlyFromContainerInjectable
    {
        [SerializeField]
        public ITestInterface TestInterface;

        public WrongSerializeFieldOnlyFromContainer(bool autoInject = true)
            : base(autoInject)
        {
        }
    }
}