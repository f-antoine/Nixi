using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.Characters.SameNamings
{
    public sealed class FirstDoll : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("ChildDoll", GameObjectMethod.GetComponentsInChildren)]
        public SecondDoll ChildDoll;
    }
}