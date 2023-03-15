using Nixi.Injections;

namespace ScriptExample.Controllers
{
    public sealed class SorcererControllerWithChildren : MonoBehaviourInjectable
    {
        [RootComponent("MainRoot", "ChildMonsterController")]
        public MonsterControllerWithChildren ChildMonsterController;
    }
}