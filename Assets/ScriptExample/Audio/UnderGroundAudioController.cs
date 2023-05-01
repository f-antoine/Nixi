using Nixi.Injections;
using UnityEngine.UI;

namespace ScriptExample.Audio
{
    public sealed class UnderGroundAudioController : MonoBehaviourInjectable
    {
        [ComponentFromParents("SliderMusic")]
        public Slider musicSlider;

        [ComponentFromParents("SliderSpatialisation")]
        public Slider spatialisationSlider;
    }
}