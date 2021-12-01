using Nixi.Injections;
using Nixi.Injections.Attributes;
using Nixi.Injections.Injecters;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.ScriptExample.SpecialOptions
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