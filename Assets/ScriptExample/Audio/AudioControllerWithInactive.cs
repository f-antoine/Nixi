using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields;
using UnityEngine.UI;

namespace Assets.ScriptExample.Audio
{
    public sealed class AudioControllerWithInactive : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("SliderMusic", GameObjectMethod.GetComponentsInChildren, false)]
        public Slider musicSlider;

        [NixInjectComponentFromMethod("SliderSpatialisation", GameObjectMethod.GetComponentsInChildren, false)]
        public Slider spatialisationSlider;
    }
}