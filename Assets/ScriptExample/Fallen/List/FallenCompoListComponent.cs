using Nixi.Injections;
using Nixi.Injections.Attributes;
using UnityEngine.UI;

namespace Assets.ScriptExample.Fallen.List
{
    public sealed class FallenCompoListComponent : MonoBehaviourInjectable
    {
        [NixInjectComponentList]
        public Slider FallenSlider;
    }
}