using Nixi.Injections;

namespace ScriptExample.Controllers
{
    public sealed class SorcererControllerWithChilds : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("MainRoot", "ChildMonsterController")]
        public MonsterControllerWithChilds ChildMonsterController;
    }
}