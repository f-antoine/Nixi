using Nixi.Injections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScriptExample.ComponentFromMethodWithoutName
{
    public sealed class ComponentChildWithoutName : MonoBehaviourInjectable
    {
        [ComponentFromChildren]
        public Slider Slider;

        [ComponentFromChildren("")]
        public Slider SliderWithEmptyString;

        [ComponentFromChildren]
        public IDragHandler SliderInterface;

        [ComponentFromChildren("")]
        public IDragHandler SliderInterfaceWithEmptyString;
    }
}
