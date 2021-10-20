using Nixi.Injections;
using Nixi.Injections.Attributes;
using UnityEngine.UI;

namespace Assets.ScriptExample.Audio
{
    public sealed class AudioController : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("SliderMusic", GameObjectMethod.GetComponentsInChildren)]
        public Slider musicSlider;

        [NixInjectComponentFromMethod("SliderSpatialisation", GameObjectMethod.GetComponentsInChildren)]
        public Slider spatialisationSlider;
    }
}