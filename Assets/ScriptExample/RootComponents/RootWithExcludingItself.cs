using Nixi.Injections;
using UnityEngine.UI;

namespace ScriptExample.RootComponents
{
    public sealed class RootWithExcludingItself : MonoBehaviourInjectable
    {
        [RootComponent("CurrentLevel")]
        public Image CurrentImage;

        [RootComponent("CurrentLevel", "ChildLevel")]
        public Image ChildImage;
    }
}
