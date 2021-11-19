using Nixi.Injections;
using Nixi.Injections.Attributes;
using UnityEngine.UI;

namespace ScriptExample.RootComponents
{
    public sealed class RootWithExcludingItself : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("CurrentLevel")]
        public Image CurrentImage;

        [NixInjectRootComponent("CurrentLevel", "ChildLevel")]
        public Image ChildImage;
    }
}
