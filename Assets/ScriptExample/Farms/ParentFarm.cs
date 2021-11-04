using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Farms
{
    public sealed class ParentFarm : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("ChildrenFarm")]
        public Farm Farm;
    }
}