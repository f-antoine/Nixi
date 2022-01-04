using Nixi.Injections;

namespace ScriptExample.Characters
{
    public sealed class Parasite : MonoBehaviourInjectable
    {
        [NixInjectComponentFromParent("ParentSorcererGameObjectName")]
        private Sorcerer parentSorcerer;
        public Sorcerer ParentSorcerer => parentSorcerer;
    }
}