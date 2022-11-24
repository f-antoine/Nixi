using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Controllers
{
    public sealed class SorcererControllerWithChildren : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("MainRoot", "ChildMonsterController")]
        public MonsterControllerWithChildren ChildMonsterController;
    }
}