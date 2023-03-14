using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;
using UnityEngine.UI;

namespace ScriptExample.Fallen.Enumerables
{
    public sealed class FallenEnumerablesComponent : MonoBehaviourInjectable
    {
        [Components]
        public Slider Fallen;
    }
}