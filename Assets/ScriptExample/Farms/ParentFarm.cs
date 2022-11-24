using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Farms
{
    public sealed class ParentFarm : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("ChildrenFarm")]
        public Farm Farm;
    }
}