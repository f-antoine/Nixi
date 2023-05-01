using Nixi.Injections;

namespace ScriptExample.Characters
{
    public sealed class Parasite : MonoBehaviourInjectable
    {
        [ComponentFromParents("ParentSorcererGameObjectName")]
        private Sorcerer parentSorcerer;
        public Sorcerer ParentSorcerer => parentSorcerer;
    }
}