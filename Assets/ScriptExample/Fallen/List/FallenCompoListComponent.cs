using Nixi.Injections;
using Nixi.Injections.Attributes;
using UnityEngine.UI;

namespace ScriptExample.Fallen.List
{
    public sealed class FallenCompoListComponent : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        public Slider FallenSlider;
    }
}