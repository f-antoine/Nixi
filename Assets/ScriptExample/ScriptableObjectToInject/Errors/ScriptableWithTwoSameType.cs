using Nixi.Injections.Attributes;
using UnityEngine;

namespace ScriptExample.ScriptableObjectToInject.Errors
{
    public sealed class ScriptableWithTwoSameType : ScriptableObject
    {
        [NixInjectTestMock]
        private SO_ContainerElement firstContainer;
        public SO_ContainerElement FirstContainer => firstContainer;

        [NixInjectTestMock]
        private SO_ContainerElement secondContainer;
        public SO_ContainerElement SecondContainer => secondContainer;
    }
}