using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using UnityEngine.UI;

namespace ScriptExample.Audio
{
    public sealed class AudioControllerWithInactive : MonoBehaviourInjectable
    {
        [ComponentFromChildren("SliderMusic", false)]
        public Slider musicSlider;

        [ComponentFromChildren("SliderSpatialisation", false)]
        public Slider spatialisationSlider;
    }
}