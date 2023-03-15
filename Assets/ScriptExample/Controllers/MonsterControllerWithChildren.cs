using Nixi.Injections;

namespace ScriptExample.Controllers
{
    public sealed class MonsterControllerWithChildren : MonoBehaviourInjectable
    {
        [RootComponent("MainRoot", "ChildSorcererController")]
        public SorcererControllerWithChildren ChildSorcererController;
    }
}