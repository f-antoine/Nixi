using System.Collections.Generic;
using UnityEngine;

namespace ScriptExample.ScriptableObjectToInject
{
    public sealed class SO_Container : ScriptableObject
    {
        [SerializeField]
        private SO_ContainerElement uniqueContainer;
        public SO_ContainerElement UniqueContainer => uniqueContainer;

        [SerializeField]
        private List<SO_ContainerElement> multiContainers;
        public List<SO_ContainerElement> MultiContainers => multiContainers;
    }
}