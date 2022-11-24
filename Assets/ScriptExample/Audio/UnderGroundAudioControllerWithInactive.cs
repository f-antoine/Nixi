using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using UnityEngine.UI;

namespace ScriptExample.Audio
{
    public sealed class UnderGroundAudioControllerWithInactive : MonoBehaviourInjectable
    {
        [NixInjectComponentFromParent("SliderMusic", false)]
        public Slider musicSlider;

        [NixInjectComponentFromParent("SliderSpatialisation", false)]
        public Slider spatialisationSlider;
    }
}