using Nixi.Injections;

namespace ScriptExample.Controllers
{
    public sealed class MonsterControllerWithChilds : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("MainRoot", "ChildSorcererController")]
        public SorcererControllerWithChilds ChildSorcererController;
    }
}