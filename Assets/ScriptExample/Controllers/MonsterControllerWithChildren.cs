using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Controllers
{
    public sealed class MonsterControllerWithChildren : MonoBehaviourInjectable
    {
        [RootComponent("MainRoot", "ChildSorcererController")]
        public SorcererControllerWithChildren ChildSorcererController;
    }
}