using UnityEngine;

namespace ScriptExample.ScriptableObjectToInject
{
    public sealed class SO_ContainerElement : ScriptableObject
    {
        [SerializeField]
        private SO_ContainerElementChild elementChild;
        public SO_ContainerElementChild ElementChild => elementChild;
    }
}