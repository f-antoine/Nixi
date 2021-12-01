using UnityEngine;

namespace ScriptExample.ScriptableObjectToInject.Errors
{
    public sealed class ScriptableWithTwoSameType : ScriptableObject
    {
        [SerializeField]
        private SO_ContainerElement firstContainer;
        public SO_ContainerElement FirstContainer => firstContainer;

        [SerializeField]
        private SO_ContainerElement secondContainer;
        public SO_ContainerElement SecondContainer => secondContainer;
    }
}