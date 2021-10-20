using Nixi.Injections;
using Nixi.Injections.Attributes;
using UnityEngine.UI;

namespace Assets.ScriptExample.Audio
{
    public sealed class UnderGroundAudioController : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("SliderMusic", GameObjectMethod.GetComponentsInParent)]
        public Slider musicSlider;

        [NixInjectComponentFromMethod("SliderSpatialisation", GameObjectMethod.GetComponentsInParent)]
        public Slider spatialisationSlider;
    }
}