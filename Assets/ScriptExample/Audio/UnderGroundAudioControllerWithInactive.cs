using Nixi.Injections;
using Nixi.Injections.Attributes;
using UnityEngine.UI;

namespace Assets.ScriptExample.Audio
{
    public sealed class UnderGroundAudioControllerWithInactive : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("SliderMusic", GameObjectMethod.GetComponentsInParent, false)]
        public Slider musicSlider;

        [NixInjectComponentFromMethod("SliderSpatialisation", GameObjectMethod.GetComponentsInParent, false)]
        public Slider spatialisationSlider;
    }
}