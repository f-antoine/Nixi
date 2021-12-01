using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;
using UnityEngine;

namespace ScriptExample.ErrorMultiAttributes
{
    public sealed class FieldContainerWithSerializeField : MonoBehaviourInjectable
    {
        [NixInjectFromContainer]
        public Sorcerer Sorcerer;
    }
}
