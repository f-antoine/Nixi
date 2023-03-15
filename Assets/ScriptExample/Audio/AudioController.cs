using Nixi.Injections;
using UnityEngine.UI;

namespace ScriptExample.Audio
{
    public sealed class AudioController : MonoBehaviourInjectable
    {
        [ComponentFromChildren("SliderMusic")]
        public Slider musicSlider;

        [ComponentFromChildren("SliderSpatialisation")]
        public Slider spatialisationSlider;
    }
}