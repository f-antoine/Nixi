using Nixi.Injections;
using UnityEngine;

namespace ScriptExample.ScriptableObjectToInject
{
    public sealed class InjectableWithSOInjectables : MonoBehaviourInjectable
    {
        [SerializeField]
        private SO_Container soContainer;
        public SO_Container SOContainer => soContainer;
    }
}