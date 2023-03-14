using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;
using System.Collections.ObjectModel;
using UnityEngine.UI;

namespace ScriptExample.Fallen.Enumerables
{
    public sealed class FallenEnumerablesReadOnlyCollection : MonoBehaviourInjectable
    {
        [Components]
        public ReadOnlyCollection<Slider> Fallen;
    }
}