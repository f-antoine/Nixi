using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScriptExample.ComponentFromMethodWithoutName
{
    public sealed class ComponentParentWithoutName : MonoBehaviourInjectable
    {
        [NixInjectComponentFromParent]
        public Slider Slider;

        [NixInjectComponentFromParent("")]
        public Slider SliderWithEmptyString;

        [NixInjectComponentFromParent]
        public IDragHandler SliderInterface;

        [NixInjectComponentFromParent("")]
        public IDragHandler SliderInterfaceWithEmptyString;
    }
}