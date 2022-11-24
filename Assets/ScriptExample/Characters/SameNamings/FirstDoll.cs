using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Characters.SameNamings
{
    public sealed class FirstDoll : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("ChildDoll")]
        public SecondDoll ChildDoll;
    }
}