using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields;

namespace Assets.ScriptExample.Controllers
{
    public sealed class MonsterControllerWithChilds : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("MainRoot", "ChildSorcererController")]
        public SorcererControllerWithChilds ChildSorcererController;
    }
}