using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;

namespace Assets.ScriptExample.Characters
{
    public sealed class Parasite : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("ParentSorcererGameObjectName", GameObjectMethod.GetComponentsInParent)]
        private Sorcerer parentSorcerer;
        public Sorcerer ParentSorcerer => parentSorcerer;
    }
}