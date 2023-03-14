using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Characters.SameNamings
{
    public sealed class FirstDoll : MonoBehaviourInjectable
    {
        [ComponentFromChildren("ChildDoll")]
        public SecondDoll ChildDoll;
    }
}