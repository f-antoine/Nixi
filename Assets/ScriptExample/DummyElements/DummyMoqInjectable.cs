using Nixi.Injections;

namespace ScriptExample.DummyElements
{
    public sealed class DummyMoqInjectable : MonoBehaviourInjectable
    {
        [FromContainer]
        private readonly IDummyInterface dummyInterface;

        [FromContainer]
        private readonly IOtherDummyInterface otherInterface;

        [FromContainer]
        private readonly IOtherDummyInterface otherInterfaceSecond;
    }
}