using Nixi.Injections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScriptExample.ComponentFromMethodWithoutName
{
    public sealed class ComponentParentWithoutName : MonoBehaviourInjectable
    {
        [ComponentFromParents]
        public Slider Slider;

        [ComponentFromParents("")]
        public Slider SliderWithEmptyString;

        [ComponentFromParents]
        public IDragHandler SliderInterface;

        [ComponentFromParents("")]
        public IDragHandler SliderInterfaceWithEmptyString;
    }
}