using Nixi.Injections;
using Nixi.Injections.Injectors;
using UnityEngine;
using UnityEngine.UI;

namespace ScriptExample.SpecialOptions
{
    public sealed class AuthorizeSerializeField : MonoBehaviourInjectable
    {
        protected override NixInjectOptions NixInjectOptions => new NixInjectOptions
        {
            AuthorizeSerializedFieldWithNixiAttributes = true
        };

        [SerializeField]
        [NixInjectComponent]
        public Slider Slider;
    }
}