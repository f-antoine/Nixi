using Nixi.Injections;
using UnityEngine.UI;

namespace ScriptExample.Audio
{
    public sealed class UnderGroundAudioControllerWithInactive : MonoBehaviourInjectable
    {
        [ComponentFromParents("SliderMusic", false)]
        public Slider musicSlider;

        [ComponentFromParents("SliderSpatialisation", false)]
        public Slider spatialisationSlider;
    }
}