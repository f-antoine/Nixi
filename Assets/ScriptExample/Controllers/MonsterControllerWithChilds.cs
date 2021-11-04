using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Controllers
{
    public sealed class MonsterControllerWithChilds : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("MainRoot", "ChildSorcererController")]
        public SorcererControllerWithChilds ChildSorcererController;
    }
}