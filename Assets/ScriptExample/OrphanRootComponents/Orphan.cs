using Nixi.Injections;
using Nixi.Injections.Attributes;
using UnityEngine.UI;

namespace ScriptExample.OrphanRootComponents
{
    public sealed class Orphan : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("emptyParent", "subGameObject")]
        public Image Image;

        [NixInjectRootComponent("emptyParent", "subGameObject")]
        public Slider Slider;
    }
}
