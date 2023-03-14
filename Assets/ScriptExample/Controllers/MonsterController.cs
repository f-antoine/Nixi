using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Controllers
{
    public sealed class MonsterController : MonoBehaviourInjectable
    {
        [RootComponent("SorcererController")]
        public SorcererController SorcererController;
    }
}