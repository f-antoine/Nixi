using Nixi.Injections;
using UnityEngine.UI;

namespace ScriptExample.Fallen.Enumerables
{
    public sealed class FallenEnumerablesComponent : MonoBehaviourInjectable
    {
        [Components]
        public Slider Fallen;
    }
}