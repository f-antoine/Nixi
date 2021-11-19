using Nixi.Injections.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptExample.ScriptableObjectToInject
{
    public sealed class SO_Container : ScriptableObject
    {
        [NixInjectTestMock]
        private SO_ContainerElement uniqueContainer;
        public SO_ContainerElement UniqueContainer => uniqueContainer;

        [NixInjectTestMock]
        private List<SO_ContainerElement> multiContainers;
        public List<SO_ContainerElement> MultiContainers => multiContainers;
    }
}