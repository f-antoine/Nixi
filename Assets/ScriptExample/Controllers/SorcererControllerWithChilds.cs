using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Controllers
{
    public sealed class SorcererControllerWithChilds : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("MainRoot", "ChildMonsterController")]
        public MonsterControllerWithChilds ChildMonsterController;
    }
}