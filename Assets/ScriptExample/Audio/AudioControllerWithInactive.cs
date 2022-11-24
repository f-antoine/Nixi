using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using UnityEngine.UI;

namespace ScriptExample.Audio
{
    public sealed class AudioControllerWithInactive : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("SliderMusic", false)]
        public Slider musicSlider;

        [NixInjectComponentFromChildren("SliderSpatialisation", false)]
        public Slider spatialisationSlider;
    }
}