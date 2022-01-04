using Nixi.Injections;
using UnityEngine.UI;

namespace ScriptExample.Fallen.Enumerables
{
    public sealed class FallenEnumerablesComponent : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        public Slider Fallen;
    }
}