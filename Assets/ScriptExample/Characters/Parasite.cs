using Nixi.Injections;
using Nixi.Injections.Attributes.MonoBehaviours;
using ScriptExample.Characters;

namespace Assets.ScriptExample.Characters
{
    public sealed class Parasite : MonoBehaviourInjectable
    {
        [NixInjectMonoBehaviourFromMethod("ParentSorcererGameObjectName", GameObjectMethod.GetComponentsInParent)]
        private Sorcerer parentSorcerer;
        public Sorcerer ParentSorcerer => parentSorcerer;
    }
}