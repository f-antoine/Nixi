using Nixi.Injections;

namespace ScriptExample.Farms
{
    public sealed class ParentFarm : MonoBehaviourInjectable
    {
        [ComponentFromChildren("ChildrenFarm")]
        public Farm Farm;
    }
}