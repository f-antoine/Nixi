using Nixi.Injections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScriptExample.ComponentFromMethodWithoutName
{
    public sealed class ComponentChildWithoutName : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren]
        public Slider Slider;

        [NixInjectComponentFromChildren("")]
        public Slider SliderWithEmptyString;

        [NixInjectComponentFromChildren]
        public IDragHandler SliderInterface;

        [NixInjectComponentFromChildren("")]
        public IDragHandler SliderInterfaceWithEmptyString;
    }
}
