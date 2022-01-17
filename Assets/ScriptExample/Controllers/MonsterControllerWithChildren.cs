using Nixi.Injections;

namespace ScriptExample.Controllers
{
    public sealed class MonsterControllerWithChildren : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("MainRoot", "ChildSorcererController")]
        public SorcererControllerWithChildren ChildSorcererController;
    }
}