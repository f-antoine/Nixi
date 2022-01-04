using Nixi.Injections;

namespace ScriptExample.Farms
{
    public sealed class ParentFarm : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("ChildrenFarm")]
        public Farm Farm;
    }
}