using Nixi.Injections.Attributes;
using UnityEngine;

namespace ScriptExample.ScriptableObjectToInject
{
    public sealed class SO_ContainerElement : ScriptableObject
    {
        [SerializeField]
        [NixInjectTestMock]
        private SO_ContainerElementChild elementChild;
        public SO_ContainerElementChild ElementChild => elementChild;
    }
}