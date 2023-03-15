using Nixi.Injections;
using UnityEngine.UI;

namespace ScriptExample.OrphanRootComponents
{
    public sealed class Orphan : MonoBehaviourInjectable
    {
        [RootComponent("emptyParent", "subGameObject")]
        public Image Image;

        [RootComponent("emptyParent", "subGameObject")]
        public Slider Slider;
    }
}
