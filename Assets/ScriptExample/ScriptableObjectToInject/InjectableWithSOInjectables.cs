using Nixi.Injections;
using Nixi.Injections.Attributes;
using UnityEngine;

namespace ScriptExample.ScriptableObjectToInject
{
    public sealed class InjectableWithSOInjectables : MonoBehaviourInjectable
    {
        [SerializeField]
        [NixInjectTestMock]
        private SO_Container soContainer;
        public SO_Container SOContainer => soContainer;
    }
}