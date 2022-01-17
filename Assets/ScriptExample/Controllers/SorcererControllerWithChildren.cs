using Nixi.Injections;

namespace ScriptExample.Controllers
{
    public sealed class SorcererControllerWithChildren : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("MainRoot", "ChildMonsterController")]
        public MonsterControllerWithChildren ChildMonsterController;
    }
}