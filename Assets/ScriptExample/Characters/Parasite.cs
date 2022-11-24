using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Characters
{
    public sealed class Parasite : MonoBehaviourInjectable
    {
        [NixInjectComponentFromParent("ParentSorcererGameObjectName")]
        private Sorcerer parentSorcerer;
        public Sorcerer ParentSorcerer => parentSorcerer;
    }
}